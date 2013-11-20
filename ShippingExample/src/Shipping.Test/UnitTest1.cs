using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shipping.DomainModel;
using Shipping.ShipService;
using Shipping.Message;
using CommonDomain.Persistence.EventStore;
using CommonDomain.Core;
using NEventStore;
using NEventStore.Serialization;
using NEventStore.Dispatcher;

namespace Shipping.Test
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()
        {
            var s = new Ship(Guid.NewGuid(), "Titantic", "Cleveland");
            Assert.AreEqual(s.Name, "Titantic");
        }

        [TestMethod]
        public void TestMethod2()
        {
            var i = Guid.NewGuid();

            var t = new ShipCreateHandler();
            t.HandleCreateShipCommand(new ShipCreateCommand() { MessageId = Guid.NewGuid(), ShipId = i, Name = "Titantic", Port = "Cleveland" });

            var d = new ShipDepartHandler();
            d.HandleShipDepartCommand(new ShipDepartCommand() { MessageId = Guid.NewGuid(), ShipId = i });

            //Get out the ship and see where it's been
            IStoreEvents store = WireupEventStore();
            IEventStream stream = store.OpenStream(i);

            Console.WriteLine("*****     Opening Stream and writing event values     *****");
            int eventNumber = 1;

            foreach (var e in stream.CommittedEvents)
            {
                Console.WriteLine("Event number: {0} , {1}", eventNumber, ((IDomainEvent)e.Body).Value);
                eventNumber++;
            }

            Console.WriteLine("*****     Getting out the aggregates    *****");
            //Load the first version
            var repository = new EventStoreRepository(WireupEventStore(), new AggregateFactory(), new ConflictDetector());
            var agg = repository.GetById<Ship>(i);
            Console.WriteLine("This is the current location of the ship '{0}'" , agg.Location);

            ////Load the second version. 
            repository = new EventStoreRepository(WireupEventStore(), new AggregateFactory(), new ConflictDetector());
            agg = repository.GetById<Ship>(i,1);
            Console.WriteLine("This is the first location of the ship '{0}'", agg.Location);


        }

        private IStoreEvents WireupEventStore()
        {
            var t = new DocumentObjectSerializer();

            return Wireup.Init()
                   .LogToOutputWindow()
                   .UsingMongoPersistence(GetConnectionString(), t)
                   .EnlistInAmbientTransaction()
                   .Build();
        }

        public string GetConnectionString()
        {
            return "Mongo";
        }
    }
}
