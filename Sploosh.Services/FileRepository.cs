﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Sploosh.Services
{
    public class FileRepository
    {

        /// <summary>
        ///  For a given txt file filename in the assembly directory, 
        ///  this will return the contents as a single string.
        /// </summary>
        public static string LoadStringFromFile(string fileName)
        {
            string path = @$"{EnvironmentVariables.AssemblyDirectory}\{fileName}";

            StringBuilder stringBuilder = new StringBuilder();


            try
            {
                if (File.Exists(path))
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

            return stringBuilder.ToString();//.TrimEnd('\r', '\n');

        }

        /// <summary>
        /// This writes a given string to a given filename in the assembly directory
        /// </summary>
        public static void WriteStringToFile(string fileName, string stringToWrite)
        {
            string path = @$"{EnvironmentVariables.AssemblyDirectory}\{fileName}";

            //Overwrites all text in file
            File.WriteAllText(path, stringToWrite);

        }


    }
}
