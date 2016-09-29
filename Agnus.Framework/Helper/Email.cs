using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.Framework.Helper
{
    public static class Email
    {
        public static void EnvioEmailService(string destinatario, string corpo, string assunto, IEnumerable<string> arquivos = null)
        {
            try
            {
                if (string.IsNullOrEmpty(destinatario))
                    return;

                var msg = new MailMessage();
                msg.To.Add(new MailAddress(destinatario));
                msg.From = new MailAddress(ConfigurationManager.AppSettings["EmailRemetente"], "Conspiração Web");
                msg.Subject = assunto;
                msg.Body = corpo;
                msg.IsBodyHtml = true;

                if (arquivos != null && arquivos.Count() > 0)
                    arquivos.ToList().ForEach(arq => msg.Attachments.Add(new Attachment(arq)));

                var client = new SmtpClient();                
                client.UseDefaultCredentials = true;
                var dominio = string.IsNullOrEmpty(ConfigurationManager.AppSettings["EmailDomain"]) ? null : ConfigurationManager.AppSettings["EmailDomain"]; 
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailLogin"], ConfigurationManager.AppSettings["EmailPassword"], dominio);
                client.Host = ConfigurationManager.AppSettings["EmailHost"];
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;

                ServicePointManager.ServerCertificateValidationCallback =
                    delegate(object s, X509Certificate certificate,
                             X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };

                client.Send(msg);
            }
            catch (Exception ex)
            {
                //TODO: Implementar log
                //throw new Exception("Não foi possível enviar email para o destinatário: " + destinatario);
            }
        }
    }
}
