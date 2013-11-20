using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.Message
{

    [Serializable]
    public class ShipDepartedEvent : IDomainEvent
    {
        public Guid Id { get; set; }

        public string Value
        {
            get
            {
                return "Ship departed";
            }
        }
    }
}
