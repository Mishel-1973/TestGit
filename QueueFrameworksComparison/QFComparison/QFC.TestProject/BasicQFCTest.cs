using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.EasyNetQ;
using QFC.MassTransitTransport;
using QFC.ServiceStackTransport;

namespace QFC.TestProject
{
	[TestClass]
	public class QFCTest
	{
		private PocoClass _sentObject = new PocoClass
		{
			Adress = new ComplexType {Id = 1012, Names = new List<string> {"mama", "mila", "ramu"}, Street = "Bluhera"},
			CreateDate = DateTime.Now,
			BasicCost = 3.1415f,
			DeletedTime = DateTime.Now.AddDays(1),
			Creator = "Person",
			Description = "UnitTestObject",
			Id = 912345,
			Identifier = new Guid(),
			IsActive = false,
			IsDeleted = true,
			Latitude = 50.01,
			LongId = 12334,
			Longitude = 35.67,
			NumChild = 12,
			ParentId = Guid.NewGuid(),
			Responsibility = "testing data",
			UpdateBy = "Mishel",
			UpdateTime = DateTime.MinValue.AddYears(911),
			WrongFieldType = 123.12m,
			Houses = new Dictionary<int, DateTime>(new Dictionary<int, DateTime> {{1, DateTime.Now}, {3, DateTime.Now}})
		};

		private readonly QueueConfig _config = new QueueConfig
		{
			HostUrl = "Host=localhost",
			LogFilePath = "D:\\Logs\\EasyNetQ\\temp.json",
			SubscriberId = "QFC_perfomance_tests"
		};

        private QueueConfig _configMassTransit = new QueueConfig
        {
            HostUrl = "rabbitmq://localhost/mybus",
            LogFilePath = "D:\\Logs\\MassTranit\\temp.json"
        };

        private QueueConfig _configServiceStack = new QueueConfig
        {
            HostUrl = "localhost",
            LogFilePath = "D:\\Logs\\ServiceStackServiceStack\\temp.json"
        };

		[TestMethod]
		public void SendMessagesViaEasyNetQ()
		{
            const int messageCount = 10;
            var timer = new Stopwatch();

		    using (var publisher = EasyNetQPublisher.GetInstance(_config))
		    {
                timer.Start();

                for (int i = 0; i < messageCount; i++)
                {
                    publisher.Publish(_sentObject);
                }

                timer.Stop();
                Debug.Write(string.Format("Elapsed time {0} message sent: {1} \n", messageCount, timer.ElapsedMilliseconds));
                timer.Reset();
		    }

            using (var subscriber = EasyNetQReceiver.GetInstance(_config))
		    {
                timer.Start();
                subscriber.Subscribe();
                while (subscriber.ReceivedData.Count < messageCount)
                {
                }

                var recievedData = new List<PocoClass>();
                while (subscriber.ReceivedData.Count != 0)
                {
                    PocoClass tepmoraryObject;
                    subscriber.ReceivedData.TryDequeue(out tepmoraryObject);
                    recievedData.Add(tepmoraryObject);
                }

                timer.Stop();
                Debug.Write(string.Format("Elapsed time {0} messages recived: {1} ms", messageCount, timer.ElapsedMilliseconds));
                timer.Reset();   
		    }
	    }

	    [TestMethod]
	    public void SendMessagesViaMassTransit()
	    {
            const int messageCount = 100;
            var timer = new Stopwatch();

            using (var subscriber = MassTransitMessageReciever.GetInstance(_configMassTransit))
            {
                using (var publisher = MassTransitMessagePublisher.GetInstance(_configMassTransit))
                {
                    timer.Start();

                    for (int i = 0; i < messageCount; i++)
                    {
                        publisher.Publish(_sentObject);
                    }

                    timer.Stop();
                    Debug.Write(string.Format("Elapsed time {0} message sent: {1} \n", messageCount, timer.ElapsedMilliseconds));
                    timer.Reset();
                }

                timer.Start();
                subscriber.Subscribe();
                while (MassTransitMessageReciever.MessageRecieved.Count < messageCount)
                {
                }

                while (MassTransitMessageReciever.MessageRecieved.Count != 0)
                {
                    PocoClass tepmoraryObject = MassTransitMessageReciever.MessageRecieved.First();
                    MassTransitMessageReciever.MessageRecieved.Remove(tepmoraryObject);
                }

                timer.Stop();
                Debug.Write(string.Format("Elapsed time {0} messages recived: {1} ms", messageCount, timer.ElapsedMilliseconds));
                timer.Reset();
            }
	    }

        [TestMethod]
        public void SendMessagesViaServiceStack()
        {
            const int messageCount = 100;
            var timer = new Stopwatch();

            using (var publisher = ServiceStackMessagePublisher.GetInstance(_configServiceStack))
            {
                timer.Start();

                for (int i = 0; i < messageCount; i++)
                {
                    publisher.Publish(_sentObject);
                }

                timer.Stop();
                Debug.Write(string.Format("Elapsed time {0} message sent: {1} \n", messageCount, timer.ElapsedMilliseconds));
                timer.Reset();
            }

            using (var subscriber = ServiceStackMessageReciever.GetInstance(_configServiceStack))
            {
                timer.Start();
                subscriber.Subscribe();
                while (subscriber.ReceivedData.Count < messageCount)
                {
                }

                var recievedData = new List<PocoClass>();
                while (subscriber.ReceivedData.Count != 0)
                {
                    PocoClass tepmoraryObject;
                    subscriber.ReceivedData.TryDequeue(out tepmoraryObject);
                    recievedData.Add(tepmoraryObject);
                }

                timer.Stop();
                Debug.Write(string.Format("Elapsed time {0} messages recived: {1} ms", messageCount, timer.ElapsedMilliseconds));
                timer.Reset();
            }
        }
	}
}
