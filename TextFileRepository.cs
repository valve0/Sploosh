using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeSploosh
{
    internal class TextFileRepository
    {

        public static string directory =
            Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,"Sploosh"); // + @"\LeSploosh\Text Files\";
          
        //public static string directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);


        public static string LoadStringFromFile(string fileName)
        {
            string path = @$"{directory}\{fileName}";

            StringBuilder stringBuilder = new StringBuilder();


            try
            {
                if(File.Exists(path))
                {

                    foreach (string line in File.ReadAllLines(path))
                    {
                        if (line != null)
                            stringBuilder.AppendLine(line);

                    }
                }
                
            }
            catch (FileNotFoundException fnfex)
            {
                //
                Debug.WriteLine(fnfex.ToString());
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }

            return stringBuilder.ToString().TrimEnd('\r', '\n');

        }

        public static void WriteStringToFile(string fileName, string stringToWrite) 
        {
            string path = $"{directory}{fileName}";

            //Overwrites all text in file
            File.WriteAllText(path, stringToWrite);
        }


        public static int GetNumberOfLinesFile(string fileName)
        {
            string stringToCount = LoadStringFromFile(fileName);

            return stringToCount.Split('\n').Count();
        }

    }
}
