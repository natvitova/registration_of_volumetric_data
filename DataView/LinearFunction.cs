using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    class LinearFunction : IFunction
    {
        private double x;
        private double y;
        private double z;

        public LinearFunction(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double GetValue(double x, double y, double z)
        {
            return (this.x * x + this.y * y + this.z * z);
        }
    }
}
