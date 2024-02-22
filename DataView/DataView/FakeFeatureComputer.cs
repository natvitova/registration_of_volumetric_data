using System;

namespace DataView
{
    class FakeFeatureComputer : IFeatureComputer
    {
        public FeatureVector ComputeFeatureVector(IData d, Point3D p)
        {
            return new FeatureVector(p, p.X, p.Y, p.Z, 0, 0);
        }
    }
}

