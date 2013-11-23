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
using NEventStore.Logging;


namespace Shipping.Test
{
    [TestClass]
    public class BDDTests
    {
        private static readonly ILog Logger = new ConsoleWindowLogger(typeof(BDDTests));


        [TestMethod]
        public void TestLog()
        {
          Logger.Debug("Test");

        }
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
            t.HandleShipCommand(new ShipCreateCommand() { MessageId = Guid.NewGuid(), ShipId = i, Name = "Titantic", Port = "Southampton" });

            var d = new ShipDepartHandler();
            d.HandleShipCommand(new ShipDepartCommand() { MessageId = Guid.NewGuid(), ShipId = i });

            var q = new ShipArriveHandler();
            q.HandleShipCommand(new ShipArriveCommand() { MessageId = Guid.NewGuid(), ShipId = i, Port = "New York" });


            //Get out the ship and see where it's been
            IStoreEvents store = WireupEventStore();
            IEventStream stream = store.OpenStream(i);

            Logger.Debug("*****     Opening Stream and writing event values     *****");
            int eventNumber = 1;

            foreach (var e in stream.CommittedEvents)
            {
                Logger.Debug("Event number: {0} , {1}", eventNumber, ((IDomainEvent)e.Body).Value);
                eventNumber++;
            }

            Logger.Debug("*****     Retrieving the aggregates at different points   *****");

            Logger.Debug("*****     Loading the current version   *****");
            //Load the first version
            var repository = new EventStoreRepository(WireupEventStore(), new AggregateFactory(), new ConflictDetector());
            var agg = repository.GetById<Ship>(i);
            Logger.Debug("This is the last location of the ship '{0}'", agg.Location);

            ////Load the second version. 
            Logger.Debug("*****     Loading version 1   *****");
            repository = new EventStoreRepository(WireupEventStore(), new AggregateFactory(), new ConflictDetector());
            agg = repository.GetById<Ship>(i, 1);
            Logger.Debug("This is the first location of the ship '{0}'", agg.Location);


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
