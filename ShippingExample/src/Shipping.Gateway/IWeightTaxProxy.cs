using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.Gateway
{
    public interface IWeightTaxProxy
    {
        WeightTax GetTax();
    }
}
