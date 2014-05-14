namespace QFC.Contracts.Interfaces
{
    public interface IQueuePublisher<TPoco> where TPoco : class
	{
        void Publish(TPoco message);
	}
}
