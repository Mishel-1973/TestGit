using System;
using System.Text;
using QFC.Contracts.Configuration;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;
using QFC.Utilities.Serialization.Serializators;
using RabbitMQ.Client;

namespace QFC.RabbitMqClient
{
	public class RabbitMqPublisher : IQueuePublisher<PocoClass>, IDisposable 
	{
        private static RabbitMqPublisher _instance;
		private readonly QueueConfig _config;
		private readonly IConnection _connection;
		private readonly IModel _channel;
		private readonly JSonSerializer<PocoClass> _serializer;

        private RabbitMqPublisher(QueueConfig config)
        {
			_serializer = new JSonSerializer<PocoClass>();
			_config = config;
			var factory = new ConnectionFactory { HostName = _config.HostUrl };
			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.QueueDeclare(_config.SubscriberId, false, false, false, null);
        }

        public static RabbitMqPublisher GetInstance(QueueConfig config)
        {
            return _instance ?? (_instance = new RabbitMqPublisher(config));
        }

		public void Publish(PocoClass message)
		{
			_channel.BasicPublish(string.Empty, _config.SubscriberId, null, Encoding.UTF8.GetBytes(_serializer.Serialize(message)));
		}

		public void Dispose()
		{
			_channel.Dispose();
			_connection.Dispose();
		}
	}
}
