using CommonDomain;
using CommonDomain.Persistence;
using Shipping.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DomainModel
{
    public class AggregateFactory : IConstructAggregates
    {
        public IAggregate Build(Type type, Guid id, IMemento snapshot)
        {
            //if (type == typeof(IMyInterface))
            //    return new MyAggregate();
            //else
            //    return Activator.CreateInstance(type) as IAggregate;


            ConstructorInfo constructor = type.GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(Guid), typeof(WeightTaxProxy) }, null);

            return constructor.Invoke(new object[] { id, new WeightTaxProxy() }) as IAggregate;
        }
    }
}
