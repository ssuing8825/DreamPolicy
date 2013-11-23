using Shipping.DomainModel;
using Shipping.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.ShipService
{
  public  class ShipArriveHandler : ShipHandlerBase
    {
        public void HandleShipCommand(ShipArriveCommand message)
        {
            //Get the Aggregate
            //Call the method on it
            // Save the aggregate. THe save must save the uncommitted events.

            Logger.Debug("Retrieving Aggregate");
            var agg = repository.GetById<Ship>(message.ShipId);
            Logger.Debug("Calling Arrive on the Aggregate");
            agg.Arrive(message.Port);
            Logger.Debug("Calling Save on the aggregate");
            repository.Save(agg, message.MessageId, h => h["OriginalMessageHeader"] = message);

        }
    }
}
