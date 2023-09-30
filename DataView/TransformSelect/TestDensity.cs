﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;


namespace DataView
{

    //TODO: This class is ineffective, optimizations needed

    public class TestDensity
    {
        //Original radius describing what values are summed together
        //The algorithm used bellow gradually divides transformations into 2 groups one of which is considered relevant. This division continues until there is either only one value or all transformations have 0 distance to the selected one
        //Radius at the end of the algorithm should be equal to the min distance between transformation
        private double radius;

        public Transform3D Find(Transform3D[] transforms)
        {
            Candidate[] candidates = new Candidate[transforms.Length];

            for (int i = 0; i < candidates.Length; i++)
                candidates[i] = new Candidate(transforms[i]);

            //radius by se měl rovnat 2 x počet dělení (log2(transforms.length) x původní hodnota (taková, že je minimální vzdáleností mezi jednotlivými transformacemi)
            radius = Math.Floor(Math.Log(transforms.Length, 2)) * 2 * findSmallestDeviation(candidates);

            Random rnd = new Random();
            return findClosest(candidates, radius, rnd.Next(candidates.Length));

        }

        private Transform3D findClosest(Candidate[] candidates, double radius, int selectedNode)
        {
            TransformationDistances transformationDistances = fillArrayWithDistances(selectedNode, candidates); //the selected node as the initial can be random

            /*
            Console.WriteLine("These are the distances");

            Console.Write("[");
            for (int i = 0; i < transformationDistances.getDistances().Length; i++)
            {
                if (i != transformationDistances.getDistances().Length - 1)
                    Console.Write(transformationDistances.getDistances()[i] + ", ");
                else
                    Console.Write(transformationDistances.getDistances()[i] + "]");
            }

            Console.WriteLine();
            */

            //printDistances(transformationDistances.getDistances());
            

            double[] distances = normalizeDistances(transformationDistances);


            /*
            Console.WriteLine("These are the normalized distances");
            printDistances(distances);
            */

            ScoreElement[] scores = fillArrayWithScores(distances, radius);

            Array.Sort(scores);

            /*
            Console.WriteLine("Scores sorted");
            for (int i = 0; i < scores.Length; i++)
                Console.Write(scores[i].getScore() + " ");

            Console.WriteLine();
            */

            //If the first score (when sorted should have the highest value) is equal to 0, then all must be 0 and all are approximately the same
            if (scores[0].getScore() == 0)
                return candidates[0].toTransform3D();

            Candidate[] tempCandidates = new Candidate[scores.Length/2];

            for(int i = 0; i<scores.Length/2; i++)
                tempCandidates[i] = candidates[scores[i].getCandidateIndex()];

            candidates = tempCandidates;


            if (candidates.Length == 1)
                return candidates[0].toTransform3D();

            return findClosest(candidates, radius / 2, 0);
        }

        private double findSmallestDeviation(Candidate[] candidates)
        {
            TransformationDistances transformationDistances = fillArrayWithDistances(0, candidates); //the selected node as the initial can be random

            double[] distances = normalizeDistances(transformationDistances);

            Array.Sort(distances);

            double minDeviation = double.MaxValue;
            for(int i = 0; i<distances.Length-1; i++)
            {
                double currentDeviation = distances[i + 1] - distances[i];
                minDeviation = Math.Min(currentDeviation, minDeviation);
            }

            return minDeviation + double.Epsilon;

        }

        /// <summary>
        /// This evaluates the score of transformations by counting how many transformations are closer than radius/2 after normalization
        /// </summary>
        /// <param name="distances">Array with distances of transformations</param>
        /// <param name="radius">Currently used radius</param>
        /// <returns>Returns scores of transformations</returns>
        private ScoreElement[] fillArrayWithScores(double[] distances, double radius)
        {
            ScoreElement[] scores = new ScoreElement[distances.Length];

            for(int i = 0; i<scores.Length; i++)
            {
                //Goes over the whole array (highly ineffective)
                for(int j = 0; j<scores.Length; j++)
                {
                    double upperBound = (distances[i] + radius / 2);
                    double lowerBound = (distances[i] - radius / 2);

                    if ((distances[j] <= upperBound) && (distances[j] >= lowerBound))
                    {

                        if (scores[i] == null)
                            scores[i] = new ScoreElement(1, i); //One is the current score

                        else
                            scores[i].incrementScore();
                    }
                }
            }

            return scores;
        }

        /// <summary>
        /// Prints distances in a line
        /// </summary>
        /// <param name="distances">Array of distances</param>
        private void printDistances(double[] distances)
        {
            for (int i = 0; i < distances.Length; i++)
                Console.Write(distances[i] + " ");

            Console.WriteLine();
        }

        /// <summary>
        /// Fills array with distances of transformations with respect to the transformation at index selectedNode
        /// </summary>
        /// <param name="selectedNode">Index of selected transformation that is used as a reference</param>
        /// <param name="candidates">Array of candidate transformations</param>
        /// <returns>Returns instance of TransformationDistances that contains the array of distances with respect to the transformation at selectedNode index.</returns>
        private TransformationDistances fillArrayWithDistances(int selectedNode, Candidate[] candidates)
        {

            double[] distances = new double[candidates.Length];
            double maxDistance = double.MinValue;
            double minDistance = double.MaxValue;

            for (int i = 0; i < distances.Length; i++)
            {
                if (i == selectedNode)
                {
                    distances[i] = 0;
                    maxDistance = Math.Max(0, maxDistance);
                    minDistance = Math.Min(0, minDistance);
                    continue;
                }

                //distances[i] = candidates[selectedNode].SqrtDistanceTo(candidates[i]);
                try
                {
                    //distances[i] = candidates[selectedNode].RotationMatrixDistance(candidates[i]);
                    //distances[i] = candidates[selectedNode].RotationMatrixDistance(candidates[i]);
                    distances[i] = candidates[selectedNode].FrobeniusRotationMatrixDistance(candidates[i]);
                    maxDistance = Math.Max(distances[i], maxDistance);
                    minDistance = Math.Min(distances[i], minDistance);
                }
                catch (Exception e)
                {
                    //Transition matrix for compared matrices doesn't exist
                    Console.WriteLine(e.Message);
                    distances[i] = double.PositiveInfinity;
                }
            }

            return new TransformationDistances(distances, minDistance, maxDistance);
        }

        /// <summary>
        /// Normalizes the distances so that they can be interpreted as percentages from the range
        /// </summary>
        /// <param name="transformationDistances">Wrapper object with distances and minimum and maximum distances</param>
        /// <returns>Returns normalized array of distances</returns>
        private double[] normalizeDistances(TransformationDistances transformationDistances)
        {

            double minDistance = transformationDistances.getMinDistance();
            double maxDistance = transformationDistances.getMaxDistance();

            double[] distances = transformationDistances.getDistances();


            for(int i = 0; i<distances.Length; i++)
            {
                double denominator = maxDistance - minDistance;

                //maxDistance == minDistance
                if (Math.Abs(denominator) < double.Epsilon)
                    distances[i] = 0;
                else
                    distances[i] = (distances[i] - minDistance) / denominator;
            }

            return distances;
        }
    }

    /// <summary>
    /// Wrapper object for score and candidate index
    /// </summary>
    public class ScoreElement: IComparable<ScoreElement>
    {
        private int score;
        private int candidateIndex;

        /// <summary>
        /// Wrapper for score and candidate index
        /// </summary>
        /// <param name="score">Score of a candidate transformation at candidateIndex</param>
        /// <param name="candidateIndex">Index of the particular candidate in candidate array</param>
        public ScoreElement(int score, int candidateIndex)
        {
            this.score = score;
            this.candidateIndex = candidateIndex;
        }

        /// <summary>
        /// Getter for score
        /// </summary>
        /// <returns>Returns the score of a candidate at candidateIndex</returns>
        public int getScore()
        {
            return score;
        }

        /// <summary>
        /// Getter for candidate index
        /// </summary>
        /// <returns>Returns the index of a candidate.</returns>
        public int getCandidateIndex()
        {
            return candidateIndex;
        }

        /// <summary>
        /// Increments score
        /// </summary>
        public void incrementScore()
        {
            score++;
        }

        /// <summary>
        /// Descending order used
        /// </summary>
        /// <param name="other">Other instance of ScoreElement</param>
        /// <returns>
        /// Returns positive number if this instance has lower score than other.
        /// If this instance has higher score, the return value is negative.
        /// If equal, it returns 0
        /// </returns>
        public int CompareTo(ScoreElement other)
        {
            return other.getScore() - this.getScore();
        }
    }

    /// <summary>
    /// Class that represents the distances array
    /// </summary>
    public class TransformationDistances
    {
        private double[] distances;
        private double maxDistance;
        private double minDistance;

        public TransformationDistances(double[] distances, double minDistance, double maxDistance)
        {
            this.distances = distances;
            this.maxDistance = maxDistance;
            this.minDistance = minDistance;
        }

        /// <summary>
        /// Returns an array with distances between currently selected
        /// transformation and the transformation at a corresponding index
        /// </summary>
        /// <returns>Returns an array of distances</returns>
        public double[] getDistances()
        {
            return distances;
        }

        /// <summary>
        /// Returns maximum distance from the array
        /// </summary>
        /// <returns></returns>
        public double getMaxDistance()
        {
            return maxDistance;
        }

        /// <summary>
        /// Returns minimum distance from the array (should always be the case of distance between selected transformation and itself, thus it would be 0)
        /// </summary>
        /// <returns></returns>
        public double getMinDistance()
        {
            return minDistance;
        }
    }
}
