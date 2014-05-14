using MassTransit;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;

namespace QFC.MassTransitTransport
{
    public class MassTransitMessagePublisher : IQueuePublisher<PocoClass>
    {
        private MassTransitMessagePublisher _instance;

        private MassTransitMessagePublisher(QueueConfig config)
        {
            Bus.Initialize(cfg  =>
            {
                cfg.UseRabbitMq();
                cfg.ReceiveFrom(config.HostUrl);
            });
        }

        public MassTransitMessagePublisher GetInstance(QueueConfig config)
        {
            return _instance ?? (_instance = new MassTransitMessagePublisher(config));
        }

        public void Publish(PocoClass message)
        {
                Bus.Instance.Publish(message);
        }
    }
}
