using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Firlansa.WebUI.AppCode.Extensions
{
    public static partial class Extension
    {
        static public bool SendEmail(this IConfiguration configuration,
            string to,
            string subject,
            string body,
            bool appendCC = false)
        {
            try
            {
                var displayName = configuration["emailAccount:displayName"];
                var smtpServer = configuration["emailAccount:smtpServer"];
                var smtpPort = Convert.ToInt32(configuration["emailAccount:smtpPort"]);
                var fromMail = configuration["emailAccount:userName"];
                var password = configuration["emailAccount:password"];
                var cc = configuration["emailAccount:cc"];

                using (MailMessage message = new MailMessage(new MailAddress(fromMail, displayName), new MailAddress(to))
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    if (!string.IsNullOrWhiteSpace(cc) && appendCC)
                        message.CC.Add(cc);

                    SmtpClient client = new SmtpClient(smtpServer, smtpPort);
                    client.Credentials = new NetworkCredential(fromMail, password);
                    client.EnableSsl = true;
                    client.Send(message);
                    
                }
                    

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
