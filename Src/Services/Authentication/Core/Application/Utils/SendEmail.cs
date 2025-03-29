using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Appliction.Utils
{
    public class SendEmail
    {
        public static void Send(string to, string subject, string body)
        {

            MailMessage mail = new MailMessage();
            var SmtpServer = new SmtpClient("parsiantamir.ir");

            mail.From = new MailAddress("info@parsiantamir.ir", "پارسیان سرویس");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;


            SmtpServer.Port = 25;

            SmtpServer.Credentials = new System.Net.NetworkCredential("info@parsiantamir.ir", "B$w83e3j1");


            SmtpServer.Send(mail);



        }
    }
}
    