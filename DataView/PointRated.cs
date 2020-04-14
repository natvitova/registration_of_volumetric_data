using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    class PointRated : PointWithFeatures
    {
        public double rating;
        public PointRated(PointWithFeatures point, double rating) : base(point.X, point.Y, point.Z, point.featureVector)
        {
            this.rating = rating;
        }
    }
}
