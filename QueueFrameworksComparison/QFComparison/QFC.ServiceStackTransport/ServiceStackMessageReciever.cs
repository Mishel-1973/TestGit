using System;
using System.Collections.Concurrent;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;
using ServiceStack.RabbitMq;

namespace QFC.ServiceStackTransport
{
    public class ServiceStackMessageReciever : IQueueReceiver<PocoClass>, IDisposable
    {
        private readonly RabbitMqServer _server;
        private static ServiceStackMessageReciever _instance;

        private ServiceStackMessageReciever(QueueConfig config)
        {
            _server = new RabbitMqServer(config.HostUrl);
            _server.Start();
        }

        public static ServiceStackMessageReciever GetInstance(QueueConfig config)
        {
            return _instance ?? (_instance = new ServiceStackMessageReciever(config));
        }

        public void Subscribe()
        {
            _server.RegisterHandler<PocoClass>(m =>
            {
                ReceivedData.Enqueue(m.GetBody());
                return null;
            });
        }

        public ConcurrentQueue<PocoClass> ReceivedData { get; private set; }

        public void Dispose()
        {
            _server.Stop();
            _server.Dispose();
            _instance = null;
        }
    }
}
