using EasyNetQ;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;

namespace QFC.EasyNetQ
{
	public class EasyNetQPublisher : IQueuePublisher<PocoClass>
	{
		private static EasyNetQPublisher _instance;

		private readonly QueueConfig _config;

		private EasyNetQPublisher(QueueConfig cfg)
		{
			_config = cfg;
		}

		public static EasyNetQPublisher GetInstance(QueueConfig config)
		{
			return _instance ?? (_instance = new EasyNetQPublisher(config));
		}

		public void Publish(PocoClass message)
		{
			using (var bus = RabbitHutch.CreateBus(_config.HostUrl))
			{
				bus.Publish( message );
			}
		}

		public static void StaticPublish(PocoClass message)
		{
			GetInstance(null).Publish(message);
		}


	}
}
