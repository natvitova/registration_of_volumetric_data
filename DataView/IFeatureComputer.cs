using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    interface IFeatureComputer
    {
        double[] ComputeFeatureVector(VolumetricData d, Point3D p);
    }
}
