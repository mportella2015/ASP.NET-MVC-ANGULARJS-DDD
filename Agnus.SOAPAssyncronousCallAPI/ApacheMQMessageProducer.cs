using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.SOAPAssyncronousCallAPI
{
    public class ApacheMQMessageProducer : ApacheActiveMQMessage
    {
        public string Url { get; set; }
        public string Content { get; set; }
        public string CorrelationMessageId { get; set; }
        public string MessageId { get; set; }

        public ApacheMQMessageProducer()
        {
        }

        public ApacheMQMessageProducer(string url, string content)
        {
            Url = url;
            Content = content;
        }

        public override ITextMessage ExecuteAction(IConnection connection, ISession session, IDestination destination)
        {
            ITextMessage message = null;
            using (IMessageProducer producer = session.CreateProducer(destination))
            {
                connection.Start();

                producer.DeliveryMode = MsgDeliveryMode.Persistent;

                message = session.CreateTextMessage(this.Content);
                
                if(!string.IsNullOrEmpty(CorrelationMessageId))
                    message.NMSCorrelationID = CorrelationMessageId;
                
                if(!string.IsNullOrEmpty(this.Url))
                    message.Properties["URL"] = this.Url;

                producer.Send(message);

                this.MessageId = message.NMSMessageId;
            }
            return message;
        }
    }
}
