namespace QFC.Contracts.Interfaces
{
<<<<<<< HEAD
    public interface IQueueTransportFramework<TPoco> where TPoco : class
=======
    public interface IQueuePublisher<TPoco> where TPoco : class
>>>>>>> 457981c4e0cbdd3e1c4099707673663ef16f8169
	{
        void Publish(TPoco message);
	}
}
