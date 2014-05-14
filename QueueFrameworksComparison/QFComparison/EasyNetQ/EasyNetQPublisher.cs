using System;
using EasyNetQ;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;

namespace QFC.EasyNetQ
{
	public class EasyNetQPublisher : IQueuePublisher<PocoClass>, IDisposable
	{
		private readonly IBus _bus;
		private static EasyNetQPublisher _instance;

		private EasyNetQPublisher(QueueConfig cfg)
		{
			_bus = RabbitHutch.CreateBus(cfg.HostUrl);
		}

		public static EasyNetQPublisher GetInstance(QueueConfig config)
		{
			return _instance ?? (_instance = new EasyNetQPublisher(config));
		}

		public void Publish(PocoClass message)
		{
			this._bus.Publish( message );
		}

		public void Dispose()
		{
			_bus.Dispose();
			_instance = null;
		}
	}
}
