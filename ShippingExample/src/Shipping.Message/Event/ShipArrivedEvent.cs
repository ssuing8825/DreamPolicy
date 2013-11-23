using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.Message
{
    public class ShipArrivedEvent : IDomainEvent
    {
        public Guid Id { get; set; }

        public string Value
        {
            get
            {
                return "Ship arrived at " + Location;
            }
        }

        public string Location { get; set; }

    }

}
