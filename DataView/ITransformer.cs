using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    interface ITransformer
    {
        Transform3D GetTransformation(Match m, VolumetricData d1, VolumetricData d2);
    }
}
