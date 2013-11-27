using Shipping.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.Message
{

    [Serializable]
    public class ShipCreatedEvent : IDomainEvent
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public WeightTax TaxResponse { get; set; }

        public string Value
        {
            get
            {
                return "Ship Create at " + Location;
            }
        }

        public string Location { get; set; }

    }

}
