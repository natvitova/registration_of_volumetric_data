using System;
namespace DataView
{
	/// <summary>
	/// This class is supposed to keep the information about number distribution
	/// This information is then used for computing feature vectors
	/// Min number is expected to be 0, since the input values are always considered positive including 0
	/// </summary>
	public class VolumetricDataDistribution
	{

		private int numberOfValues;
		private int maxValue;

		public int[] numberOfOccurences;
		public double[] distributions;

		public VolumetricDataDistribution(double step, int maxValue)
		{
			this.numberOfValues = 0;
			this.maxValue = maxValue;
			this.numberOfOccurences = new int[(int)(maxValue / step)];
		}

		public void AddValue(double value)
		{
			double percentage = value / maxValue;
			int index = (int)(percentage * (numberOfOccurences.Length - 1));


			//Increase the associated number of occurences by 1
			numberOfOccurences[index]++;

			numberOfValues++;
		}

		public void CreateDistributionArray()
		{
			distributions = new double[numberOfOccurences.Length];

			double previousDistribution = 0;
			for(int i = 0; i<numberOfOccurences.Length; i++)
			{
				double currentDistribution = (double)(numberOfOccurences[i]) / numberOfValues;

				distributions[i] = Math.Min(previousDistribution + currentDistribution, 1);

				previousDistribution = distributions[i];
			}
		}

		public double GetDistributionPercentage(double value)
		{
			if (distributions == null)
			{
				Console.WriteLine("You forgot to create a distribution array before calling this method, so it gets created automatically.");
				CreateDistributionArray();
			}
            double percentage = value / maxValue;
            int index = (int)(percentage * (numberOfOccurences.Length - 1));
			return distributions[index];
        }
	}
}

