﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    interface IFunction
    {
        double GetValue(double x, double y, double z);
    }
}