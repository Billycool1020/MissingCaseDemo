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
        public void Sendmail(List<Threads> List)
        {
            var threads = from l in List
                          group l by l.Product into g
                          select g;
            
            foreach(var thread in threads)
            {
                string content= "New threads:<br /> ";
              
                    content = content + thread.Key + "<br />";
              
                foreach(Threads t in thread)
                {
                    content = content + "<a href=" + t.Link + ">" + t.Title + "</a><br />";
                }
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

            mailMessage.Subject = "New Threads";
            mailMessage.Body = content;
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
