using System;
using System.Collections.Concurrent;
using QFC.Contracts.Data;
using QFC.Contracts.Interfaces;

namespace QFC.MassTransitTransport
{
    public class MassTransitMessageReciever : IQueueReceiver<PocoClass>
    {
        public void Subscribe()
        {
            throw new NotImplementedException();
        }

        public ConcurrentQueue<PocoClass> ReceivedData { get; private set; }
    }
}
