using CommonDomain.Core;
using Shipping.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Shipping.DomainModel
{
    /// <summary>
    /// Ship is the aggregate root
    /// </summary>
    public class Ship : AggregateBase
    {

        private Ship(Guid id)
        {
            this.Id = id;
        }
        public Ship(Guid id, string name, string location)
            : this(id)
        {
            this.RaiseEvent(new ShipCreatedEvent { Id = this.Id, Name = name, Location = location });
        }

        public string Name { get; set; }

        public string Location { get; set; }

        public void Arrive(string location)
        {
            this.RaiseEvent(new ShipArrivedEvent { Id = this.Id, Location = location });
        }
        public void Depart()
        {
            this.RaiseEvent(new ShipDepartedEvent { Id = this.Id });
        }
  
        private void Apply(ShipCreatedEvent @event)
        {
            this.Name = @event.Name;
            this.Location = @event.Location;
        }
        private void Apply(ShipDepartedEvent @event)
        {
            this.Location = "Out to Sea";
        }
        private void Apply(ShipArrivedEvent @event)
        {
            this.Location = @event.Location;
        }


    }
}
