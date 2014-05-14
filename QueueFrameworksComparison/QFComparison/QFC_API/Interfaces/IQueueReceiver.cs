using System.Collections.Concurrent;

namespace QFC.Contracts.Interfaces
{
	public interface IQueueReceiver<TPoco> where TPoco : class
	{
		void Subscribe();
		ConcurrentQueue<TPoco> ReceivedData { get; }
	}
}
