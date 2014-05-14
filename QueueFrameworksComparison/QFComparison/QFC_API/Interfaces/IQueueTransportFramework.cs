namespace QFC.Contracts.Interfaces
{
    interface IQueueTransportFramework<TPoco> where TPoco : class
	{
        bool Publish(TPoco message);
	}
}
