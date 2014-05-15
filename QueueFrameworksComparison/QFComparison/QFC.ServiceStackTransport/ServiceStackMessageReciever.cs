using System;
using System.Collections.Concurrent;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;
using QFC.Utilities.Log.ConfigurationSettings;
using QFC.Utilities.Log.Contracts;
using QFC.Utilities.Log.Logers;
using ServiceStack.RabbitMq;

namespace QFC.ServiceStackTransport
{
    public class ServiceStackMessageReciever : IQueueReceiver<PocoClass>, IDisposable
    {
        private readonly RabbitMqServer _server;
        private readonly ILoger<PocoClass> _loger;
        private readonly ConcurrentQueue<PocoClass> _data;

        private static ServiceStackMessageReciever _instance;

        private ServiceStackMessageReciever(QueueConfig config)
        {
            var logConfig = new LogConfig
            {
                IsAppend = true,
                SourceFilePath = config.LogFilePath
            };

            _loger = new JsonLoger<PocoClass>(logConfig);
            _data = new ConcurrentQueue<PocoClass>();
            _server = new RabbitMqServer(config.HostUrl);
        }

        public static ServiceStackMessageReciever GetInstance(QueueConfig config)
        {
            return _instance ?? (_instance = new ServiceStackMessageReciever(config));
        }

        public void Subscribe()
        {
            _server.RegisterHandler<PocoClass>(m =>
            {
                _loger.LogData(m.GetBody());
                ReceivedData.Enqueue(m.GetBody());
                return null;
            });
			_server.Start();
		}

        public ConcurrentQueue<PocoClass> ReceivedData
        {
            get { return _data; }
        }

        public void Dispose()
        {
            _server.Stop();
            _server.Dispose();
            _instance = null;
        }
    }
}
