using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;

namespace QFC.EasyNetQ
{
	public class EasyNetQReceiver : IQueueReceiver<PocoClass>
	{
		private readonly ConcurrentQueue<PocoClass> _data;
		private EasyNetQReceiver _instance;

		private readonly QueueConfig _config;

		private EasyNetQReceiver(QueueConfig cfg)
		{
			_config = cfg;
			_data = new ConcurrentQueue<PocoClass>();
		}

		public EasyNetQReceiver GetInstance(QueueConfig config)
		{
			return _instance ?? (_instance = new EasyNetQReceiver(config));
		}

		public void Subscribe()
		{
			using (var bus = RabbitHutch.CreateBus(_config.HostUrl))
			{
				bus.Subscribe<PocoClass>("asdfasf", HandleMessage);
			}
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
			_data.Enqueue(message);
		}
	}
}
