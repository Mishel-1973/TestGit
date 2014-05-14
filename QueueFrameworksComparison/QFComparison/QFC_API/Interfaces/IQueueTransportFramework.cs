namespace QFC.Contracts.Interfaces
{
    public interface IQueueTransportFramework<TPoco> where TPoco : class
	{
        bool Publish(TPoco message);
	}
}
