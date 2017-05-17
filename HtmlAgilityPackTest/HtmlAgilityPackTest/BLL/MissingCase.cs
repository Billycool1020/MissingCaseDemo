using HtmlAgilityPackTest.DAL;
using HtmlAgilityPackTest.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlAgilityPackTest.BLL
{
    class MissingCase
    {
        MissingCaseDataContext db = new MissingCaseDataContext();
        public List<MissingCaseModel> getMissingCase()
        {
            List<MissingCaseModel> MissingList = new List<MissingCaseModel>();
            var now = DateTime.Now.ToUniversalTime().AddHours(8).ToString();
            DataTable dt = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["CA"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select t.QuestionId,t.Title,t.[Url],p.DisplayName,t.CreatedOn from [tbl_instances] as t join tbl_products as p on t.ProductId = p.Id where PlatformId = '3' and t.CreatedOn > GETDATE() - 1 order by t.CreatedOn desc"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                        cmd.CommandText = "select t.QuestionId,t.Title,t.[Url],p.DisplayName,t.CreatedOn from [tbl_instances] as t join tbl_products as p on t.ProductId = p.Id where PlatformId = '1' and t.CreatedOn > GETDATE() - 1 order by t.CreatedOn desc";
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                    }
                }
            }




            var cat = new List<string>();
            cat.Add("e8973131-e48e-4970-8d75-6e791bf5d6a1");
            cat.Add("2121577");
            
          
            foreach (DataRow t in dt.Rows)
            {
                var a = t[0];
                if (!cat.Contains(t[0]))
                {
                    if (t[2].ToString().Contains("social.technet.microsoft.com"))
                    {
                        continue;
                    }
                    MissingCaseModel MC = new MissingCaseModel();
                    MC.IfInsetintoCAT = false;
                    MC.ThreadId = t[0].ToString();
                    MC.Title = t[1].ToString();
                    MC.Link = t[2].ToString();
                    MC.Product = t[3].ToString();
                    MC.PostDate = (DateTime)t[4];
                    MissingList.Add(MC);
                }
            }
            return MissingList;
        }
    }
}
