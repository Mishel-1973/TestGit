using System;
using System.Runtime.CompilerServices;
using MassTransit;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;

namespace QFC.MassTransitTransport
{
    public class MassTransitMessageTransmiter : IQueuePublisher<PocoClass>
    {
        private MassTransitMessageTransmiter _instance;

        private MassTransitMessageTransmiter(QueueConfig config)
        {
            Bus.Initialize(cfg  =>
            {
                cfg.UseRabbitMq();
                cfg.ReceiveFrom(config.HostUrl);
            });
        }

        public MassTransitMessageTransmiter GetInstance(QueueConfig config)
        {
            return _instance ?? (_instance = new MassTransitMessageTransmiter(config));
        }

        public void Publish(PocoClass message)
        {
                Bus.Instance.Publish(message);
        }
    }
}
