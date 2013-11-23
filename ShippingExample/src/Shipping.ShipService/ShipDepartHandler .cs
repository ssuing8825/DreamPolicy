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
    public class ShipDepartHandler : ShipHandlerBase
    {
        public void HandleShipCommand(ShipDepartCommand message)
        {
            //Get the Aggregate
            //Call the method on it
            // Save the aggregate. THe save must save the uncommitted events.


            Logger.Debug("Retrieving Aggregate");
            var agg = repository.GetById<Ship>(message.ShipId);
            Logger.Debug("Calling Depart on the Aggregate");
            agg.Depart();
            Logger.Debug("Calling Save on the aggregate");
            repository.Save(agg, message.MessageId, h => h["OriginalMessageHeader"] = message);

        }
    }
}
