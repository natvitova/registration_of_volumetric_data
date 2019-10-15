using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    class Point3D
    {
        public double x;
        public double y;
        public double z;

        public Point3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double GetCoordinate(int direction)
        {
            switch (direction)
            {
                case 0: return this.x;
                case 1: return this.y;
                default: return this.z;
            }
        }

        public override string ToString()
        {
            return "x:" + x + " y:" + y + " z:" + z;
        }
    }
}
