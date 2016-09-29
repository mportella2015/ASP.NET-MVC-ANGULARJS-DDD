using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Agnus.SOAPAssyncronousCallAPI
{
    public class ApacheMQMessageConsumer : ApacheActiveMQMessage
    {
        public string ResponseQueueName { get; set; }

        public override ITextMessage ExecuteAction(IConnection connection, ISession session, IDestination destination)
        {
            ITextMessage message = null;
            using (IMessageConsumer consumer = session.CreateConsumer(destination))
            {
                connection.Start();

                message = consumer.Receive() as ITextMessage;
                if (message == null)
                    throw new Exception("No message received!");

                var url = message.Properties["URL"];
                if (url == null)
                    throw new Exception("Invalid URL");

                var soapEnvelope = message.Text;
                var soapEnvelopeXml = new XmlDocument();
                soapEnvelopeXml.LoadXml(soapEnvelope);

                var response = SOAPAssyncronousCall.CallWebService(url.ToString()  , soapEnvelopeXml);

                var apacheMQProducer = new ApacheMQMessageProducer { CorrelationMessageId = message.NMSMessageId, Content = response };
                apacheMQProducer.ExecuteTransaction(ResponseQueueName);
            }
            return message;
        }
    }
}
