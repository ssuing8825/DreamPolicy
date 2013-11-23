using NEventStore;
using Shipping.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEventStore.Persistence.MongoDB;
using Shipping.DomainModel;
using NEventStore.Dispatcher;
using NEventStore.Serialization;
using CommonDomain.Persistence.EventStore;
using CommonDomain.Core;
using NEventStore.Logging;


namespace Shipping.ShipService
{
    public class ShipHandlerBase
    {
        protected EventStoreRepository repository;
        
        //This should not be hard coded like this.
        protected static readonly ILog Logger = new ConsoleWindowLogger(typeof(ShipHandlerBase));


        private static readonly byte[] EncryptionKey = new byte[]
            {
                0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf
            };

        public ShipHandlerBase()
        {
            repository = new EventStoreRepository(WireupEventStore(), new AggregateFactory(), new ConflictDetector());
        }

        private IStoreEvents WireupEventStore()
        {
            var t = new DocumentObjectSerializer();

            return Wireup.Init()
                   .LogToOutputWindow()
                   .UsingMongoPersistence(GetConnectionString(), t)
                   .EnlistInAmbientTransaction()
                   .UsingSynchronousDispatchScheduler()
                       .DispatchTo(new DelegateMessageDispatcher(DispatchCommit))
                   .Build();
        }

        private void DispatchCommit(ICommit commit)
        {
            // This is where we'd hook into our messaging infrastructure, such as NServiceBus,
            // MassTransit, WCF, or some other communications infrastructure.
            // This can be a class as well--just implement IDispatchCommits.
            try
            {
                foreach (var @event in commit.Events)
                    Logger.Debug("Message Dispatched " + ((IDomainEvent)@event.Body).Value);
            }
            catch (Exception)
            {
                Logger.Debug("Message Not Dispatched");
            }
        }

        public string GetConnectionString()
        {
            return "Mongo";
        }
    }
}
