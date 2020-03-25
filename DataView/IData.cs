﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    interface IData
    {
        double XSpacing { get; set; }
        double YSpacing { get; set; }
        double ZSpacing { get; set; }
        int[] Measures { get; set; }
        int GetValue(double x, double y, double z);
        int GetValue(Point3D p);
    }
}
