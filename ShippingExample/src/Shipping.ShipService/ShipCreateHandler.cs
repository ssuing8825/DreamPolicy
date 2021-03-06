﻿using NEventStore;
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
using Shipping.Gateway;


namespace Shipping.ShipService
{
    public class ShipCreateHandler : ShipHandlerBase
    {
        private IWeightTaxProxy taxProxy;

        public ShipCreateHandler(IWeightTaxProxy taxProxy)
        {
            this.taxProxy = taxProxy;
        }
        public void HandleShipCommand(ShipCreateCommand message)
        {


            //Get the Aggregate
            //Call the method on it
            // Save the aggregate. THe save must save the uncommitted events.
            Logger.Debug("Creating  the aggregate");
            var agg = new Ship(message.ShipId, taxProxy, message.Name, message.Port);
            Logger.Debug("Calling Save on the aggregate");
            repository.Save(agg, message.MessageId, h => h["OriginalMessageHeader"] = message);
        }
    }
}
