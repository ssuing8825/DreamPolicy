using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.Message
{
    public class ShipArriveCommand
    {
        public Guid ShipId { get; set; }

        public Guid MessageId { get; set; }

        public string Port { get; set; }

    }
}
