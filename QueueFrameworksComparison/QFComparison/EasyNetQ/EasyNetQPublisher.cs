using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;

namespace QFC.EasyNetQ
{
	public class EasyNetQTransport : IQueueTransportFramework<PocoClass>
	{
		public const string HostPath = "host=localhost";
		public bool Publish(PocoClass message)
		{
			using (var bus = RabbitHutch.CreateBus(HostPath))
			{
				bus.Publish( message );
			}
			return true;
		}
	}
}
