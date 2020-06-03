using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    class Test1
    {
        public Transform3D t;
        public double alpha;
        public Point3D point;

        public Test1(Transform3D t, double a)
        {
            this.t = t;
            this.alpha = a;
        }
        public Test1(Transform3D t, double a, Point3D p)
        {
            this.t = t;
            this.alpha = a;
            this.point = p;
        }
    }
}
