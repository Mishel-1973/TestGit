using System.Diagnostics;
using MassTransit;
using QFC.Contracts.Data;

namespace QFC.MassTransitTransport
{
    public class MessageConsumer : Consumes<PocoClass>.All
    {
        public void Consume(PocoClass message)
        {
            MassTransitMessageReciever.MessageRecieved.Add(message);
            Debug.WriteLine("Recieved message...");
        }
    }
}
