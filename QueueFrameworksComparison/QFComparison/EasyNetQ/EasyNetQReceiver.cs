using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;

namespace QFC.EasyNetQ
{
	public class EasyNetQReceiver : IQueueReceiver<PocoClass>
	{
		private readonly ConcurrentQueue<PocoClass> _data;

		public EasyNetQReceiver()
		{
			_data = new ConcurrentQueue<PocoClass>();
		}

		public void Subscribe(string connectionString)
		{
			using (var bus = RabbitHutch.CreateBus(connectionString))
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
