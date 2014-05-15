using System;
using System.Collections.Concurrent;
using EasyNetQ;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;
using QFC.Utilities.Log.ConfigurationSettings;
using QFC.Utilities.Log.Contracts;
using QFC.Utilities.Log.Logers;

namespace QFC.EasyNetQ
{
	public class EasyNetQReceiver : IQueueReceiver<PocoClass>, IDisposable
	{
		private readonly IBus _bus;
		private readonly ConcurrentQueue<PocoClass> _data;
        private readonly ILoger<PocoClass> _loger;
        private static EasyNetQReceiver _instance;
		private const string EasyQSubscriptionID = "QFS_testing";

		private EasyNetQReceiver(QueueConfig cfg)
		{
            var logConfig = new LogConfig
            {
                IsAppend = true,
                SourceFilePath = cfg.LogFilePath
            };

            _loger = new JsonLoger<PocoClass>(logConfig);

			_data = new ConcurrentQueue<PocoClass>();
			_bus = RabbitHutch.CreateBus(cfg.HostUrl);
		}

		public static EasyNetQReceiver GetInstance(QueueConfig config)
		{
			return _instance ?? (_instance = new EasyNetQReceiver(config));
		}

		public void Subscribe()
		{
			_bus.Subscribe<PocoClass>(EasyQSubscriptionID, HandleMessage);
		}

		public ConcurrentQueue<PocoClass> ReceivedData
		{
			get
			{
				return _data;
			}
		}

		protected void HandleMessage(PocoClass message)
		{
            _loger.LogData(message);
			_data.Enqueue(message);
		}

		public void Dispose()
		{
			_bus.Dispose();
			_instance = null;
		}
	}
}
