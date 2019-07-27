using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataView
{
    class TestDataBuilder
    {
        

        public TestDataBuilder()
        {

        }

        public void BuildData()
        {

            double[] elementSpacing = new double[3];
            int[] dimSize = new int[3];
            string elementDataFile = null;
            string elementType = null;//Tyhle 4 parametry jeste musim doplnit

            elementSpacing[0] = 0.053059;//xspacing
            elementSpacing[1] = 0.053059;//yspacing
            elementSpacing[2] = 0.053058;//zspacing

            dimSize[0] = 1012;
            dimSize[1] = 1024;
            dimSize[2] = 1014;

            elementType = "MET_UCHAR";

            string fileName = "testData100";

            CreateDataFile(fileName, dimSize);
            CreateMetaDataFile(fileName, elementType);

        }

        public void CreateDataFile(string fileName, int[] dimSize)
        {
            

            BinaryWriter bw;
            try {
                 bw = new BinaryWriter(new FileStream(string.Format("{0}{1}{2}.raw", Environment.CurrentDirectory, System.IO.Path.DirectorySeparatorChar, fileName), FileMode.Create));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot create file.");
                return;
            }

            //WriteConstantToFile(bw, dimSize, 100);
            WriteConstantsToHalves(bw, dimSize, 0, 255);
            bw.Close();
           
        }

        public void WriteConstantsToHalves(BinaryWriter bw, int[] dimSize, byte c1, byte c2)
        {
            try
            {
                for (int i = 0; i < dimSize[0]; i++)
                {
                    for (int j = 0; j < dimSize[1]; j++)
                    {
                        for (int k = 0; k < dimSize[2]; k++)
                        {
                            if (i < dimSize[0] / 2)
                                bw.Write(c1);
                            else
                                bw.Write(c2);
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot write to file.");
                return;
            }
        }

        /// <summary>
        /// Sets the whole image to a constant color
        /// </summary>
        /// <param name="c"></param>
        public void WriteConstantToFile(BinaryWriter bw, int[] dimSize, byte c)
        {
            try
            {
                for (int i = 0; i < dimSize[0]; i++)
                {
                    for (int j = 0; j < dimSize[1]; j++)
                    {
                        for (int k = 0; k < dimSize[2]; k++)
                        {
                            bw.Write(c);
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot write to file.");
                return;
            }
        }

        private void CreateMetaDataFile(string fileName, string elementType)
        {
            string[] lines =
                {
                "ObjectType = Image", "NDims = 3",
                "BinaryData = True", "BinaryDataByteOrderMSB = False",
                "CompressedData = False", "TransformMatrix = 1 0 0 0 1 0 0 0 1",
                "Offset = 0 0 0", "CenterOfRotation = 0 0 0",
                "AnatomicalOrientation = RAI", "ElementSpacing = 0.053059 0.053059 0.053058",
                "DimSize = 1012 1024 1014", string.Format("ElementType = {0}", elementType),
                string.Format("ElementDataFile = {0}.raw", fileName) 
            };
            System.IO.File.WriteAllLines(string.Format("{0}{1}{2}.mhd", Environment.CurrentDirectory, System.IO.Path.DirectorySeparatorChar, fileName), lines);

        }

    }
}
