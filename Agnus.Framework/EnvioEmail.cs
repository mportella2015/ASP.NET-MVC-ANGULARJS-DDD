using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Agnus.Framework
{
    public class EnvioEmail
    {
        public void EnvioEmailService(string destinatario, string corpo, string assunto)
        {
            var msg = new MailMessage();
            msg.To.Add(new MailAddress(destinatario));
            msg.From = new MailAddress(ConfigurationManager.AppSettings["EmailRemetente"], string.Empty);
            msg.Subject = assunto;
            msg.Body = corpo;
            msg.IsBodyHtml = true;

            var client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailLogin"], ConfigurationManager.AppSettings["EmailPassword"], ConfigurationManager.AppSettings["EmailDomain"]);
            client.Host = ConfigurationManager.AppSettings["EmailHost"];
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    delegate(object s, X509Certificate certificate,
                             X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };

                client.Send(msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        } 
    }
}