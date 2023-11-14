using System;

namespace DataView
{
	interface ITransformationDistance
	{
        double GetTransformationsSecond(Transform3D transformation1, Transform3D transformation2, IData micro);

    }
}

