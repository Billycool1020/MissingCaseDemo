using HtmlAgilityPackTest.DAL;
using HtmlAgilityPackTest.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HtmlAgilityPackTest.BLL
{
    class MissingCase
    {
        MissingCaseDataContext db = new MissingCaseDataContext();
        public int CheckIsAnswer()
        {
            var threads = (from t in db.MissingCaseModels
                           where t.IsAnswered == false && t.PostDate.Month == DateTime.Now.Month
                           select t);

            DateTime firstdate = (from t in db.MissingCaseModels
                                  where t.IsAnswered == false && t.PostDate.Month == DateTime.Now.Month
                                  orderby t.PostDate
                                  select t.PostDate).FirstOrDefault().Date;
            DataTable CA = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["CA"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select t.QuestionId,q.Answered from [tbl_instances] as t join tbl_question_metrics_new as q on t.QuestionId=q.QuestionId  where t.PlatformId = '3' and t.ProductId !='1' and t.ProductId !='24' and t.CreatedOn >= @date order by t.CreatedOn desc"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.AddWithValue("@date", firstdate);
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(CA);
                    }
                }
            }
            int cnt = 0;
            foreach (var t in threads)
            {
                var row = CA.Select("QuestionId='" + t.ThreadId + "'");
                if (row.Count() != 0)
                {
                    t.IsAnswered = Convert.ToBoolean(CA.Select("QuestionId='" + t.ThreadId + "'")[0][1]);
                } 
                if(t.IsAnswered)
                {
                    cnt++;
                }           
            }
            db.SaveChanges();
            return cnt;
        }

        public void recheck()
        {
            //DateTime firstday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime firstday = (from t in db.MissingCaseModels
                                where t.IsInsetintoCAT == false && t.PostDate.Month == DateTime.Now.Month
                                orderby t.PostDate
                                select t.PostDate).FirstOrDefault().Date;

            var threads = (from t in db.MissingCaseModels
                           where t.IsInsetintoCAT == false && t.PostDate.Month == DateTime.Now.Month
                           select t);


            DataTable CAT = new DataTable();
            string constr2 = ConfigurationManager.ConnectionStrings["CAT"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr2))
            {
                using (SqlCommand cmd = new SqlCommand("select cat_externalid from cat_thread where cat_externalcreatedon >= @date order by cat_externalcreatedOn"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@date", firstday);
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 600;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(CAT);
                    }
                }
            }

            var cat = new List<string>();
            foreach (DataRow t in CAT.Rows)
            {
                cat.Add(t[0].ToString());
            }
         
            foreach (var t in threads)
            {
                if (cat.Contains(t.ThreadId))
                {
                    t.IsInsetintoCAT = true;
                }
            }
            db.SaveChanges();
        }



        public List<MissingCaseModel> getMissingCase()
        {
            List<MissingCaseModel> MissingList = new List<MissingCaseModel>();
            var now = DateTime.Now.AddDays(-4).ToString();
            DataTable CA = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["CA"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select t.QuestionId,t.Title,t.[Url],p.DisplayName,t.CreatedOn,q.Answered from [tbl_instances] as t join tbl_products as p on t.ProductId = p.Id join tbl_question_metrics_new as q on t.QuestionId=q.QuestionId  where t.PlatformId = '3' and t.ProductId !='1' and t.ProductId !='24' and t.CreatedOn > @date order by t.CreatedOn desc"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@date", now);
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(CA);
                    }
                }
            }


            //var date = DateTime.Now.ToUniversalTime().AddDays(-4).ToShortDateString();

            //DataTable CAT = new DataTable();
            //string constr2 = ConfigurationManager.ConnectionStrings["CAT"].ConnectionString;
            //using (SqlConnection con = new SqlConnection(constr2))
            //{
            //    using (SqlCommand cmd = new SqlCommand("select cat_externalid from cat_thread where cat_externalcreatedon > @date order by cat_externalcreatedOn"))
            //    //using (SqlCommand cmd = new SqlCommand("select cat_externalid from cat_thread where cat_externalcreatedon > '4/26/2017' and cat_externalcreatedon <'5/6/2017'  order by cat_externalcreatedOn"))
            //    {
            //        using (SqlDataAdapter sda = new SqlDataAdapter())
            //        {
            //            cmd.Parameters.AddWithValue("@date", date);
            //            cmd.CommandType = CommandType.Text;
            //            cmd.CommandTimeout = 600;
            //            cmd.Connection = con;
            //            sda.SelectCommand = cmd;
            //            sda.Fill(CAT);
            //        }
            //    }
            //}


            var cat = new List<string>();
            cat.Add("e8973131-e48e-4970-8d75-6e791bf5d6a1");
            cat.Add("2121577");


            HttpWebRequest myHttpWebRequest;
            foreach (DataRow t in CA.Rows)
            {
                if (!cat.Contains(t[0]))
                {
                    if (t[2].ToString().Contains("social.technet.microsoft.com"))
                    {
                        continue;
                    }
                    try
                    {
                        myHttpWebRequest = (HttpWebRequest)WebRequest.Create(t[2].ToString().Replace("&outputAs=xml", ""));
                        // Sends the HttpWebRequest and waits for a response.
                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        myHttpWebResponse.Close();
                        MissingCaseModel MC = new MissingCaseModel();
                        MC.IsInsetintoCAT = false;
                        MC.Link = t[2].ToString().Replace("&outputAs=xml", "");
                        MC.PostDate = (DateTime)t[4];
                        MC.Product = t[3].ToString();
                        MC.ThreadId = t[0].ToString();
                        MC.Title = t[1].ToString();
                        MC.IsAnswered = Convert.ToBoolean(t[5]);
                        MissingList.Add(MC);
                    }
                    catch
                    {

                    }

                }
            }
            return MissingList;
        }

        public List<MissingCaseModel> CheckMissingCase(List<MissingCaseModel> list)
        {
            List<MissingCaseModel> MissingList = new List<MissingCaseModel>();
            var date = DateTime.UtcNow.AddDays(-2);
            var dblist = db.MissingCaseModels.Where(m => m.PostDate > date).Select(x => x.ThreadId).ToList();
            foreach (MissingCaseModel mc in list)
            {
                if (!dblist.Contains(mc.ThreadId))
                {
                    MissingList.Add(mc);
                }
            }
            return MissingList;
        }
    }
}
