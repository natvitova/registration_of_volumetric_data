using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    interface ISampler
    {
       Point3D[] Sample(VolumetricData d, int count);
    }
}
