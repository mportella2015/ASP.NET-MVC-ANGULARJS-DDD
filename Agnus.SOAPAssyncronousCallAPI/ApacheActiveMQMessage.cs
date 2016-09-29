using Apache.NMS;
using Apache.NMS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agnus.SOAPAssyncronousCallAPI
{
    public abstract class ApacheActiveMQMessage
    {
        public abstract ITextMessage ExecuteAction(IConnection connection, ISession session, IDestination destination);

        public ITextMessage ExecuteTransaction(string queueName)
        {
            var factory = CreateFactory();

            ITextMessage message = null;
            using (IConnection connection = factory.CreateConnection())
            using (ISession session = connection.CreateSession(AcknowledgementMode.Transactional))
            {
                IDestination destination = CreateDestination(session, queueName);

                message = ExecuteAction(connection, session, destination);

                session.Commit();
            }
            return message;
        }

        private IDestination CreateDestination(ISession session, string queueName)
        {
            return SessionUtil.GetDestination(session, "queue://" + queueName);                
        }

        private IConnectionFactory CreateFactory()
        {
            var apacheMQConnection = System.Configuration.ConfigurationManager.AppSettings["ApacheActiveMQConnection"];

            Uri connecturi = new Uri(apacheMQConnection);
            IConnectionFactory factory = new NMSConnectionFactory(connecturi);
            return factory;
        }
    }
}
