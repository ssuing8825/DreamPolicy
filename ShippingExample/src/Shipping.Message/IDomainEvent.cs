﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.Message
{
    public interface IDomainEvent
    {
        string Value { get;  }
    }
}
