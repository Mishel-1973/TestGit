using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;
using QFC.EasyNetQ;
using QFC.ServiceStackTransport;

namespace TestReceiverApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var receiver = ServiceStackMessageReciever.GetInstance(new QueueConfig()
					{
						HostUrl = "localhost",
						SubscriberId = "console_app",
						LogFilePath = "E:\\Logs"
					}))
			{
				receiver.Subscribe();
				int numReceivedMessages = 0;
				PocoClass obj;
				while (numReceivedMessages < 100)
				{
					if (receiver.ReceivedData.Count > 0)
					{
						if (receiver.ReceivedData.TryDequeue(out obj))
						{
							numReceivedMessages++;
							Console.WriteLine("received " + numReceivedMessages.ToString() + " " + obj.Description);
						}
						else
						{
							Console.WriteLine("trydequeue failed");
						}
					}
					else
					{
						//Thread.Sleep(100);
					}
				}
			}
		}
	}
}
