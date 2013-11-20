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


namespace Shipping.ShipService
{
    public class ShipCreateHandler
    {
        private EventStoreRepository repository;

        private static readonly byte[] EncryptionKey = new byte[]
            {
                0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf
            };

        public ShipCreateHandler()
        {
            repository = new EventStoreRepository(WireupEventStore(), new AggregateFactory(), new ConflictDetector());
        }

        public void HandleCreateShipCommand(ShipCreateCommand message)
        {
            //Get the Aggregate
            //Call the method on it
            // Save the aggregate. THe save must save the uncommitted events.
            var agg = new Ship(message.ShipId,message.Name, message.Port);
            repository.Save(agg, message.MessageId, h => h["OriginalMessageHeader"] = message);

        }

        private  IStoreEvents WireupEventStore()
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
                    Console.WriteLine("Message Dispatched " + ((IDomainEvent)@event.Body).Value);
            }
            catch (Exception)
            {
                Console.WriteLine("Message Not Dispatched");
            }
        }

        public string GetConnectionString()
        {
            return "Mongo";
        }
    }
}
