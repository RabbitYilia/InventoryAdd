using System;
using System.Collections.Generic;
using System.Text;
using CambridgeSoft.ChemScript14;
using System.IO;

namespace Demo
{
    class Program
    {
        /// <summary>
        /// Set current directory to the execute file folder
        /// </summary>
        /// <returns></returns>
        static string SetCurrentDirectory()
        {
            string curDir = System.Environment.CurrentDirectory;
            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            System.Environment.CurrentDirectory = baseDir;

            return curDir;
        }

        static bool IsInputFilesReady()
        {
            string[] inputFiles = { "smiles.input" };

            StringVector missingFiles = new StringVector();
            foreach (string fileName in inputFiles)
            {
                FileInfo file = new FileInfo(fileName);
                if (!file.Exists)
                    missingFiles.Add(fileName);
            }

            if (missingFiles.Count > 0)
            {
                Console.WriteLine("The following input files are missing, please make sure that they are in the same directory as Demo.exe!");
                foreach (string fileName in missingFiles)
                {
                    Console.WriteLine(fileName);
                }

                return false;
            }

            return true;
        }

        static void Main(string[] args)
        {
            CambridgeSoft.ChemScript14.Environment.SetVerbosity(true);
            CambridgeSoft.ChemScript14.Environment.SetThrowExceptions(true);

            string oldDir = SetCurrentDirectory();

            if (!IsInputFilesReady())
            {
                return;
            }

            try
            {
                StreamReader sr = new StreamReader("smiles.input", Encoding.Default);
                String line;
                line = sr.ReadToEnd();
                Console.WriteLine(line);
                StructureData m = StructureData.LoadData(line, "smiles");
                sr.Close();
                FileStream fs = new FileStream("cdx.output", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                Console.WriteLine(m.WriteData("cdx", true));
                sw.Write(m.WriteData("cdx", true));
                sw.Flush();
                sw.Close();
                fs.Close();

                 fs = new FileStream("name.output", FileMode.Create);
                 sw = new StreamWriter(fs);
                Console.WriteLine(m.WriteData("name"));
                sw.Write(m.WriteData("name"));
                sw.Flush();
                sw.Close();
                fs.Close();

                 fs = new FileStream("formula.output", FileMode.Create);
                 sw = new StreamWriter(fs);
                Console.WriteLine(m.Formula());
                sw.Write(m.Formula());
                sw.Flush();
                sw.Close();
                fs.Close();

                fs = new FileStream("weight.output", FileMode.Create);
                sw = new StreamWriter(fs);
                Console.WriteLine(m.Weight);
                sw.Write(m.Weight);
                sw.Flush();
                sw.Close();
                fs.Close();
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}


