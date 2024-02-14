using System;
using MathNet.Numerics.LinearAlgebra;

namespace DataView
{
	
	class PointApproximation
	{

		private IData data;

		private double targetPointValue;
		private double learningRate;

        /// <summary>
        /// Class that uses Gradient Descent to find point with closest value to target value
        /// This found point should be in the surrounding of given original point
        /// Doesn't significantly improve accuracy of points at the moment, tested with cost function implemented as difference of feature vectors and difference in points distribution values
        /// </summary>
        /// <param name="data">Interface for getting data for a given point to be moved towards optimum (to have as close value to target value as possible)</param>
        /// <param name="learningRate">Coeficient that influences programs sensitivity to calculate result. The lower it is, the more accurate the result is, but the more time it takes to be fully calculated.</param>
        public PointApproximation(IData data, IFeatureComputer featureComputer, double learningRate)
		{
			this.data = data;
			this.learningRate = learningRate;
		}

		public Point3D FindClosePoint(Point3D originalPoint, double targetPointValue, double epsilon)
		{
			Point3D currentPoint = originalPoint.Copy();

			this.targetPointValue = targetPointValue;

			//condition if diverges needs to be implemented
			while(true)
			{

				Point3D offsetPointX = new Point3D(currentPoint.X + epsilon, currentPoint.Y, currentPoint.Z);
				Point3D offsetPointY = new Point3D(currentPoint.X, currentPoint.Y + epsilon, currentPoint.Z);
                Point3D offsetPointZ = new Point3D(currentPoint.X, currentPoint.Y, currentPoint.Z + epsilon);


                if (OutOfBounds(offsetPointX.X, offsetPointY.Y, offsetPointZ.Z))
                    break;

                double dx = learningRate * DerivativeFunction(currentPoint, offsetPointX, epsilon);
				double dy = learningRate * DerivativeFunction(currentPoint, offsetPointY, epsilon);
				double dz = learningRate * DerivativeFunction(currentPoint, offsetPointZ, epsilon);
                double newX = currentPoint.X - dx;
                double newY = currentPoint.Y - dy;
                double newZ = currentPoint.Z - dz;

				if (FunctionConverges(newX, currentPoint.X, 1E-12))
					break;

                if (OutOfBounds(newX, newY, newZ))
                    break;

                currentPoint.X = newX;
                currentPoint.Y = newY;
                currentPoint.Z = newZ;
            }

			return currentPoint;
		}

		private double DerivativeFunction(Point3D currentPoint, Point3D offsetPoint, double epsilon)
		{
			return (CalculateFunction(offsetPoint) - CalculateFunction(currentPoint)) / epsilon;
		}

		private double CalculateFunction(Point3D point)
		{
			return Math.Abs(targetPointValue - data.GetValueDistribution(data.GetValue(point)));
		}

		private bool FunctionConverges(double newCoordinate, double oldCoordinate, double epsilon)
		{
			return Math.Abs(newCoordinate - oldCoordinate) < epsilon;
        }

		private bool OutOfBounds(double x, double y, double z)
		{
			if (data.Measures[0] <= x)
				return true;

            if (data.Measures[1] <= y)
                return true;

            if (data.Measures[2] <= z)
                return true;

			return (x < 0 || y < 0 || z < 0);
        }
	}
}

