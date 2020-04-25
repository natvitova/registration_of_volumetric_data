using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataView
{
    /// <summary>
    /// 
    /// </summary>
    class Data
    {
        private double[] elementSpacing; // three double values indicating distance between two points in concrete direction
        private int[] dimSize; // three int values indicating the "length" of the side of the cuboid in concerete direction
        private string elementDataFile; // the name of the file .raw (with data)
        private string elementType; // USHORT(2 bytes)/UCHAR(1 byte)

        /// <summary>
        /// 
        /// </summary>
        public Data(string fileName)  
        {
            elementSpacing = null;
            dimSize = null;
            elementDataFile = null;
            elementType = null;

            this.SetFeatures(fileName);
        }

        public Data(Data copy)
        {
            this.elementSpacing = copy.ElementSpacing;
            this.dimSize = copy.DimSize;
            this.elementDataFile = copy.ElementDataFile;
            this.elementType = copy.ElementType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        private void SetFeatures(string fileName) // method setting instance atributes
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string newLine;
                string s; // pomocná proměnná
                while ((newLine = sr.ReadLine()) != null)
                {
                    if (newLine.Contains("ElementSpacing"))
                    {
                        s = newLine.Substring(newLine.IndexOf("=") + 2);
                        string[] a = s.Split(' ');
                        elementSpacing = new double[a.Length];
                        for (int i = 0; i < a.Length; i++)
                        {
                            ElementSpacing[i] = GetDouble(a[i], -1234);
                        }
                    }
                    if (newLine.Contains("DimSize"))
                    {
                        s = newLine.Substring(newLine.IndexOf("=") + 2);
                        string[] a = s.Split(' ');
                        dimSize = new int[a.Length];
                        for (int i = 0; i < a.Length; i++)
                        {
                            DimSize[i] = int.Parse(a[i]);
                        }
                    }
                    else if (newLine.Contains("ElementType"))
                    {
                        s = newLine.Substring(newLine.IndexOf("=") + 2);
                        elementType = s;
                    }
                    else if (newLine.Contains("ElementDataFile"))
                    {
                        s = newLine.Substring(newLine.IndexOf("=") + 2);
                        elementDataFile = s;
                    }
                    else
                    {

                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static double GetDouble(string value, double defaultValue)
        {
            // Try parsing in the current culture
            if (!double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out double result) &&
                // Then try in US english
                !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                // Then in neutral language
                !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        public double[] ElementSpacing { get => elementSpacing; }
        public int[] DimSize { get => dimSize; set => dimSize = value; }
        public string ElementDataFile { get => elementDataFile; }
        public string ElementType { get => elementType; }

    }
}
