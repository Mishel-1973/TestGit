using System;
using System.Collections.Concurrent;
using MassTransit;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;

namespace QFC.MassTransitTransport
{
    public class MassTransitMessageReciever : IQueueReceiver<PocoClass>, IDisposable
    {
        private readonly IServiceBus _bus;
        private static MassTransitMessageReciever _instance;
        
        private MassTransitMessageReciever(QueueConfig config)
        {
            this._bus = ServiceBusFactory.New(cfg =>
            {
                cfg.UseRabbitMq();
                cfg.ReceiveFrom(config.HostUrl);
            });
        }

        public static MassTransitMessageReciever GetInstance(QueueConfig config)
        {
            return _instance ?? (_instance = new MassTransitMessageReciever(config));
        }

        public ConcurrentQueue<PocoClass> ReceivedData { get; private set; }

        public void Subscribe()
        {
            this._bus.SubscribeHandler<PocoClass>(msg => ReceivedData.Enqueue(msg));
        }

        public void Dispose()
        {
            _bus.Dispose();
            _instance = null;
        }
    }
}
