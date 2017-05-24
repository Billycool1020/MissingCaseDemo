using HtmlAgilityPackTest.DAL;
using HtmlAgilityPackTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HtmlAgilityPackTest
{
    class Email
    {
        public void Sendmail()
        {
            MissingCaseDataContext db = new MissingCaseDataContext();
            DateTime firstday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var threads = from l in db.MissingCaseModels
                          where l.PostDate >= firstday
                          group l by l.Product into g
                          select g;
            //if (threads.Count > 0)
            {


                string content = "<br /> ";
                content = content + "Missing Case:<br /> <table  border='1'><tr> <td>Title</td> <td>ID</td> <td style='width:100px'>IsAnswered</td> </tr>";

                foreach (var thread in threads)
                {
                    content = content + "<tr><td colspan='3' >" + thread.Key + "</td></tr>";

                    foreach (MissingCaseModel t in thread)
                    {
                        content = content + "<tr><td><a href=" + t.Link + ">" + t.Title + "</a></td>";
                        content = content + "<td>" + t.ThreadId + "</td>";
                        content = content + "<td>" + t.IsAnswered + "</td></tr>";
                    }

                }
                content = content + "</table>";
                Send("", content);
            }
        }

        public bool Send(string target, string content)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage mailMessage = new MailMessage();
            smtpClient.EnableSsl = true;
            //mailMessage.To.Add(new MailAddress(target));
            //foreach (var t in teamleaderlist)
            //{
            //    mailMessage.CC.Add(new MailAddress(t));
            //}
            mailMessage.To.Add(new MailAddress("billyliu.cool@hotmail.com"));

            mailMessage.Subject = "Missing Case " + DateTime.Now.Month + "/1~" + DateTime.Now.Month + "/" + DateTime.Now.Day;
            mailMessage.Body = content;
            mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
            mailMessage.IsBodyHtml = true;
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
