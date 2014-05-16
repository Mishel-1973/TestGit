using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;
using QFC.Utilities.Log.ConfigurationSettings;
using QFC.Utilities.Log.Contracts;
using QFC.Utilities.Log.Logers;
using QFC.Utilities.Serialization.Serializators;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QFC.RabbitMqClient
{
	public class RabbitMqReceiver : IQueueReceiver<PocoClass>, IDisposable
	{
		private const int DataReadyTimeout = 10;
		private readonly ILoger<PocoClass> _loger;
		private readonly ConcurrentQueue<PocoClass> _data;
		private readonly IConnection _connection;
		private readonly IModel _channel;
		private QueueingBasicConsumer _consumer;
		private readonly Thread _listenerThread;
		private static RabbitMqReceiver _instance;
		private bool _disconnect;
		private readonly QueueConfig _config;

		private RabbitMqReceiver(QueueConfig config)
		{
			_config = config;
            var logConfig = new LogConfig
            {
                IsAppend = true,
                SourceFilePath = config.LogFilePath
            };

            _loger = new JsonLoger<PocoClass>(logConfig);
            _data = new ConcurrentQueue<PocoClass>();

			var factory = new ConnectionFactory{ HostName = config.HostUrl};
			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			this._listenerThread = new Thread(this.ListenerThreadFunc)
			{
				Priority = ThreadPriority.Normal,
				Name = "RabbitMQ listener thread"
			};
			_disconnect = false;
        }

		public static RabbitMqReceiver GetInstance(QueueConfig config)
        {
			return _instance ?? (_instance = new RabbitMqReceiver(config));
        }

		public void Subscribe()
		{
			_channel.ExchangeDeclare(_config.SubscriberId, "fanout");
			var queueName = _channel.QueueDeclare();

			_channel.QueueBind(queueName, _config.SubscriberId, string.Empty);
			_consumer = new QueueingBasicConsumer(_channel);
			_channel.BasicConsume(queueName, true, _consumer);

			this._listenerThread.Start();
		}

		public ConcurrentQueue<PocoClass> ReceivedData {
			get { return _data; }
		}

		public void Dispose()
		{
			_disconnect = true;
			this._listenerThread.Join(1000);
			_channel.Dispose();
			_connection.Dispose();
		}

		private void ListenerThreadFunc()
		{
			var  serializer = new JSonSerializer<PocoClass>();

			while (!this._disconnect)
			{
				BasicDeliverEventArgs ea;
				if (_consumer.Queue.Dequeue(DataReadyTimeout, out ea))
				{
					var body = ea.Body;
					var message = Encoding.UTF8.GetString(body);
					PocoClass temporaryObject = serializer.Deserialize(message);
					this._loger.LogData(temporaryObject);
					this.ReceivedData.Enqueue(temporaryObject);
				}
			}
		}

	}
}
