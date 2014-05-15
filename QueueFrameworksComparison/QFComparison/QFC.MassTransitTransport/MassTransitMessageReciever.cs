using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using MassTransit;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;
using QFC.Utilities.Log.ConfigurationSettings;
using QFC.Utilities.Log.Contracts;
using QFC.Utilities.Log.Logers;

namespace QFC.MassTransitTransport
{
    public class MassTransitMessageReciever : IQueueReceiver<PocoClass>, IDisposable
    {
        public static List<PocoClass> MessageRecieved = new List<PocoClass>();
 
        private readonly IServiceBus _bus;
        private readonly ConcurrentQueue<PocoClass> _data;
        private readonly ILoger<PocoClass> _loger;
        private static MassTransitMessageReciever _instance;
        
        private MassTransitMessageReciever(QueueConfig config)
        {
            var logConfig = new LogConfig
            {
                IsAppend = true,
                SourceFilePath = config.LogFilePath
            };

            _data = new ConcurrentQueue<PocoClass>();
            _loger = new JsonLoger<PocoClass>(logConfig);
            this._bus = ServiceBusFactory.New(cfg =>
            {
                cfg.UseRabbitMq();
                cfg.ReceiveFrom(config.HostUrl);
                cfg.UseRabbitMqRouting();
            });

            _bus.SubscribeConsumer<MessageConsumer>();
        }

        public static MassTransitMessageReciever GetInstance(QueueConfig config)
        {
            return _instance ?? (_instance = new MassTransitMessageReciever(config));
        }

        public ConcurrentQueue<PocoClass> ReceivedData
        {
            get { return _data; }
        }

        public void Subscribe()
        {
            this._bus.SubscribeHandler<PocoClass>(msg =>
            {
                /*Debug.WriteLine("Handling....");
                //_loger.LogData(msg);
                _data.Enqueue(msg);*/
            });
        }

        public void Dispose()
        {
            _bus.Dispose();
            _instance = null;
        }
    }
}
