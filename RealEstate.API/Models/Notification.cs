using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace RealEstate.API.Models
{
    public class Notification
    {
        public bool SendMail(string ToMailAddress,string mailSubject, string mailBody)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress(ToMailAddress));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
                mail.Sender = new MailAddress(ConfigurationManager.AppSettings["UserName"]);
                mail.Subject = mailSubject;
                mail.Body = mailBody;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;

                smtp.UseDefaultCredentials = false;
                NetworkCredential credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserName"], ConfigurationManager.AppSettings["Password"]);
                smtp.Credentials = credentials;
                smtp.EnableSsl = true;
                mail.Priority = MailPriority.High;
                mail.IsBodyHtml = true;
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}