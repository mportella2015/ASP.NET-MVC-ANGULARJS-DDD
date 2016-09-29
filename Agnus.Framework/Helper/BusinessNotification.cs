using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conspiracao.Framework.Helper
{
    public class BusinessNotification
    {
        public BusinessNotification() { }

        public BusinessNotification(BusinessNotificationMessages message)
        {
            Mensagens = new StringBuilder();
            switch (message)
            {
                case BusinessNotificationMessages.Incluido:
                    Mensagens.AppendLine("Dados incluídos com sucesso!");
                    break;
                case BusinessNotificationMessages.Alterado:
                    Mensagens.AppendLine("Dados alterados com sucesso!");
                    break;
                case BusinessNotificationMessages.Excluido:
                    Mensagens.AppendLine("Dados excluídos com sucesso!");
                    break;
                default:
                    break;
            }
            
            Tipo = BusinessNotificationTipo.Sucess;
        }

        public BusinessNotification(StringBuilder messages, BusinessNotificationTipo tipo)
        {
            Mensagens = messages;
            Tipo = tipo;
        }

        public BusinessNotification(string messages, BusinessNotificationTipo tipo)
        {
            Mensagens = new StringBuilder();
            Mensagens.AppendLine(messages);
            Tipo = tipo;
        }

        public BusinessNotificationTipo Tipo { get; set; }
        public StringBuilder Mensagens { get; set; }
    }
}
