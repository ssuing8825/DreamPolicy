using CommonDomain.Core;
using NEventStore.Logging;
using Shipping.Gateway;
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
        private IWeightTaxProxy taxProxy;
        private Ship(Guid id, IWeightTaxProxy taxProxy)
        {
            this.Id = id;
            this.taxProxy = taxProxy;
        }
        public Ship(Guid id, IWeightTaxProxy taxProxy, string name, string location)
            : this(id, taxProxy)
        {
            Logger.Debug("Raising Created event");

            //Here we are going to an outside service to determine what to set things
            var tax = taxProxy.GetTax();

            this.RaiseEvent(new ShipCreatedEvent { Id = this.Id, Name = name, Location = location, TaxResponse = tax });
        }

        public string Name { get; set; }

        public string Location { get; set; }

        public Decimal Cost { get; set; }

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

            Console.WriteLine("Applying the Weight Tax");
            this.Cost = 2m * Convert.ToDecimal(@event.TaxResponse.TaxRate);

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
