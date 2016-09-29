using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Agnus.SOAPAssyncronousCallAPI
{
    public class ApacheMQMessageSelector : ApacheActiveMQMessage
    {
        public string NMSMessageId { get; set; }

        public override ITextMessage ExecuteAction(IConnection connection, ISession session, IDestination destination)
        {
            ITextMessage message = null;
            var selector = "JMSMessageID = '" + NMSMessageId + "'";
            using (IMessageConsumer subscriber = session.CreateConsumer(destination, selector))
            {
                connection.Start();

                message = subscriber.ReceiveNoWait() as ITextMessage;
                if (message == null)
                    throw new Exception("No message received!");
            }
            return message;
        }
    }
}
