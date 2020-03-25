using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    interface IFeatureComputer
    {
        FeatureVector ComputeFeatureVector(IData d, Point3D p);
    }
}
