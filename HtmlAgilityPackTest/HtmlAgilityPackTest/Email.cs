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
        public bool Send()
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

            mailMessage.Subject = "Review Results";
            //mailMessage.Body = content;
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
