using System;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;
using ServiceStack;
using ServiceStack.RabbitMq;

namespace QFC.ServiceStackTransport
{
    public class ServiceStackMessagePublisher : IQueuePublisher<PocoClass>, IDisposable 
    {
        private readonly RabbitMqServer _server;
        private static ServiceStackMessagePublisher _instance;

        private ServiceStackMessagePublisher(QueueConfig config)
        {
            _server = new RabbitMqServer(config.HostUrl);
            _server.Start();
        }

        public static ServiceStackMessagePublisher GetInstance(QueueConfig config)
        {
            return _instance ?? (_instance = new ServiceStackMessagePublisher(config));
        }

        public void Publish(PocoClass message)
        {
            using (var mqClient = _server.CreateMessageQueueClient())
            {
                mqClient.Publish(message);
            }
        }

        public void Dispose()
        {
            _server.Stop();
            _server.Dispose();
            _instance = null;
        }
    }
}
