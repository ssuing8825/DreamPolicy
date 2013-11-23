using CommonDomain.Core;
using NEventStore.Logging;
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
        //This should not be hard coded like this.
        private static readonly ILog Logger = new ConsoleWindowLogger(typeof(Ship));

        private Ship(Guid id)
        {
            this.Id = id;
        }
        public Ship(Guid id, string name, string location)
            : this(id)
        {
            Logger.Debug("Raising Created event");
            this.RaiseEvent(new ShipCreatedEvent { Id = this.Id, Name = name, Location = location });
        }

        public string Name { get; set; }

        public string Location { get; set; }

        public void Arrive(string location)
        {
            Logger.Debug("Raising Arrived event");
            this.RaiseEvent(new ShipArrivedEvent { Id = this.Id, Location = location });
        }
        public void Depart()
        {
            Logger.Debug("Raising Departed event");
            this.RaiseEvent(new ShipDepartedEvent { Id = this.Id });
        }
  
        private void Apply(ShipCreatedEvent @event)
        {
            Logger.Debug("Applying Created Event");
            this.Name = @event.Name;
            this.Location = @event.Location;
        }
        private void Apply(ShipDepartedEvent @event)
        {
            Logger.Debug("Applying Departed Event");
            this.Location = "Out to Sea";
        }
        private void Apply(ShipArrivedEvent @event)
        {
            Logger.Debug("Applying Arrived Event");
            this.Location = @event.Location;
        }


    }
}
