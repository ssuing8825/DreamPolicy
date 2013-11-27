using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.Gateway
{
    public class WeightTaxProxy : IWeightTaxProxy
    {
        private Random r = new Random();
        public WeightTax GetTax()
        {
            Console.WriteLine("Getting the Weight Tax");
            return new WeightTax { RequestDate = DateTime.Now, TaxRate = r.NextDouble() };
        }
    }
}
