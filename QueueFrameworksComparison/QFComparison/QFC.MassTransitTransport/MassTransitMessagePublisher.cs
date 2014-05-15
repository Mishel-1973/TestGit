using System;
using MassTransit;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;

namespace QFC.MassTransitTransport
{
    public class MassTransitMessagePublisher : IQueuePublisher<PocoClass>, IDisposable
    {
        private readonly IServiceBus _bus;
        private static MassTransitMessagePublisher _instance;

        private MassTransitMessagePublisher(QueueConfig config)
        {
            this._bus = ServiceBusFactory.New(cfg  =>
            {
                cfg.UseRabbitMq();
                // Publish and receive use the same setting method (ReceiveFrom)
                cfg.ReceiveFrom(config.HostUrl);
            });
        }

        public static MassTransitMessagePublisher GetInstance(QueueConfig config)
        {
            return _instance ?? (_instance = new MassTransitMessagePublisher(config));
        }

        public void Publish(PocoClass message)
        {
                this._bus.Publish(message);
        }

        public void Dispose()
        {
            _bus.Dispose();
            _instance = null;
        }
    }
}
