using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DataView
{
    interface ITransformer
    {

        Transform3D GetTransformation(Match m, IData d1, IData d2);

        Transform3D GetTransformation(Match m, IData d1, IData d2, IConfiguration configuration);
    }
}
