using System.Collections.Concurrent;

namespace QFC.Contracts.Interfaces
{
	public interface IQueueReceiver<TPoco> where TPoco : class
	{
		void Subscribe(string connectionString);
		ConcurrentQueue<TPoco> ReceivedData { get; }
	}
}
