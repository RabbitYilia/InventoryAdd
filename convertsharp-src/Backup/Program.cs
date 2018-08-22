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

        static void RestoreCurrentDirectory(string oldDir)
        {
            System.Environment.CurrentDirectory = oldDir;
        }

        static bool IsInputFilesReady()
        {
            string[] inputFiles = { "demo.cdx", "input.sdf", "m.cdx", "target.cdx", "reaction.cdx" };

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
            Console.WriteLine(CambridgeSoft.ChemScript14.Environment.Version());
            Console.WriteLine("This .NET session will demonstrate many of the features in ChemScript.\n\n");

            string oldDir = SetCurrentDirectory();

            if (!IsInputFilesReady())
            {
                Console.WriteLine("Press <Enter> to quit ...");
                Console.ReadLine();
                return;
            }

            try
            {
                Console.WriteLine(" 0. Loading the ChemScript module");
                Console.WriteLine(" 1. Creating a StructureData object");
                Console.WriteLine(" 2. Writing a StructureData object to a string or a file");
                Console.WriteLine(" 3. Working with Atom and Bond objects");
                Console.WriteLine(" 4. Converting names to structures and structures to names");
                Console.WriteLine(" 5. Converting 2D structures to 3D");
                Console.WriteLine(" 6. Minimizing MM2 and energy calculations for 3D structures");
                Console.WriteLine(" 7. Salt stripping");
                Console.WriteLine(" 8. Searching structures atom by atom");
                Console.WriteLine(" 9. Finding the largest common substructure");
                Console.WriteLine("10. Overlaying structures");
                Console.WriteLine("11. Computing molecular topological properties");
                Console.WriteLine("12. Working with ReactionData objects");
                Console.WriteLine("13. Reading and writing SD files");
                Console.WriteLine("\n");
                Console.WriteLine("Press <Enter> to continue ...");
                Console.ReadLine();


                Console.WriteLine("\n\n\n   0. Loading the ChemScript module\n--------------------------------------");
                Console.WriteLine("\nBefore starting, you must load the ChemScript module by typing the following:\n(Note: This is case-sensitive)");
                Console.WriteLine("\nusing CambridgeSoft.ChemScript14;");
                Console.WriteLine("\n\nIt will now load ChemScript14");

                Console.WriteLine("\n\n\n   1. Creating a StructureData object\n--------------------------------------");
                Console.ReadLine();

                Console.WriteLine("\nCreating a StructureData object from a SMILES string.");
                Console.ReadLine();
                Console.WriteLine("StructureData m = StructureData.LoadData(\"C=CCO\");");
                StructureData m = StructureData.LoadData("C=CCO");

                Console.WriteLine("\nLet's take a look at what's in this StructureData object:");
                Console.ReadLine();
                Console.WriteLine("m.List();");
                m.List();

                Console.WriteLine("\nCreating a StructureData object from a file.");
                Console.ReadLine();
                Console.WriteLine("m = StructureData.LoadFile(\"demo.cdx\");");
                m = StructureData.LoadFile("demo.cdx");

                Console.WriteLine("\nYou can create a StructureData object first, and then load data into it.");
                Console.ReadLine();
                Console.WriteLine("m = new StructureData();");
                m = new StructureData();
                Console.WriteLine("m.ReadFile(\"demo.cdx\");");
                m.ReadFile("demo.cdx");

                Console.WriteLine("\nWhen loading data or files, you can specify a mimetype.  This will speed the loading because ChemScript will not need to determine the data type from the file contents.");
                Console.ReadLine();
                Console.WriteLine("m = StructureData.LoadData(\"C=CCO\", \"smiles\");");
                m = StructureData.LoadData("C=CCO", "smiles");

                Console.WriteLine("\nIt is also possible to use the full mimetype name:");
                Console.ReadLine();
                Console.WriteLine("m = StructureData.LoadData(\"C=CCO\", \"chemical/smiles\");");
                m = StructureData.LoadData("C=CCO", "chemical/smiles");

                try
                {
                    Console.WriteLine("\nBut if the given mimetype does not match, the loading will fail.");
                    Console.ReadLine();
                    Console.WriteLine("m = StructureData.LoadData(\"C=CCO\", \"cdx\");");
                    m = StructureData.LoadData("C=CCO", "cdx");
                    Console.WriteLine("Console.WriteLine(m)");
                    Console.WriteLine(m);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine("\nYou have to use \"smiles\" instead of \"cdx\".");
                Console.ReadLine();
                Console.WriteLine("m = StructureData.LoadData(\"C=CCO\", \"smiles\");");
                m = StructureData.LoadData("C=CCO", "smiles");

                Console.WriteLine("\nHere are all of the mimetypes you can use.");
                Console.ReadLine();
                Console.WriteLine("foreach (String types in StructureData.MimeTypes())");
                Console.WriteLine("    Console.WriteLine(types);");
                foreach (String types in StructureData.MimeTypes())
                {
                    Console.WriteLine(types);
                }


                Console.WriteLine("\n\n\n   2. Writing a StructureData object to a string or a file\n--------------------------------------");
                Console.ReadLine();

                Console.WriteLine("\nWrite the StructureData object into a base64 encoded CDX string.");
                Console.ReadLine();
                Console.WriteLine("Console.WriteLine(m.WriteData(\"cdx\", true));");
                Console.WriteLine(m.WriteData("cdx", true));

                Console.WriteLine("\nGet a SMILES string representation of the StructureData object.");
                Console.ReadLine();
                Console.WriteLine("Console.WriteLine(m.WriteData(\"smiles\"));");
                Console.WriteLine(m.WriteData("smiles"));

                Console.WriteLine("\nGet an InChI string representation of the StructureData object.");
                Console.ReadLine();
                Console.WriteLine("Console.WriteLine(m.WriteData(\"inchi\"));");
                Console.WriteLine(m.WriteData("inchi"));

                Console.WriteLine("\nWrite a file.");
                Console.ReadLine();
                Console.WriteLine("m.WriteFile(\"output.cdx\", \"cdx\");");
                m.WriteFile("output.cdx", "cdx");

                Console.WriteLine("\nIf no mimetype is specified, the file extension is used to determine the file format.");
                Console.ReadLine();
                Console.WriteLine("m.WriteFile(\"output.cdx\");");
                m.WriteFile("output.cdx");


                Console.WriteLine("\n\n\n   3. Working with Atom and Bond objects\n--------------------------------------");
                Console.ReadLine();

                Console.WriteLine("\nIterate over all atoms");
                Console.ReadLine();
                Console.WriteLine("foreach (Atom a in m.Atoms);");
                Console.WriteLine("    Console.WriteLine(a.Name + \", \" + a.GetCartesian().ToString());");
                foreach (Atom a in m.Atoms)
                {
                    Console.WriteLine(a.Name + ", " + a.GetCartesian().ToString());
                }

                Console.WriteLine("\nIterate over all bonds");
                Console.ReadLine();
                Console.WriteLine("foreach (Bond b in m.Bonds);");
                Console.WriteLine("    Console.WriteLine(b.Atom1.Name + \", \" + b.Atom2.Name + \", \" + b.Order.Name)");
                foreach (Bond b in m.Bonds)
                {
                    Console.WriteLine(b.Atom1.Name + "," + b.Atom2.Name + "," + b.Order.Name);
                }

                Console.WriteLine("\nAdd an atom");
                Console.ReadLine();
                Console.WriteLine("Atom a1 = m.CreateAtom(\"O\");");
                Atom a1 = m.CreateAtom("O");

                Console.WriteLine("\nAdd a bond");
                Console.ReadLine();
                Console.WriteLine("Bond b1 = m.CreateBond(a1, m.Atoms[0], null);");
                Bond b1 = m.CreateBond(a1, m.Atoms[0], null);

                Console.WriteLine("\nRemove an atom");
                Console.ReadLine();
                Console.WriteLine("m.RemoveAtom(m.Atoms[0]);");
                m.RemoveAtom(m.Atoms[0]);

                Console.WriteLine("\nRemove a bond");
                Console.ReadLine();
                Console.WriteLine("m.RemoveBond(m.Bonds[0]);");
                m.RemoveBond(m.Bonds[0]);

                Console.WriteLine("\nRemove a bond and its bonded atoms");
                Console.ReadLine();
                Console.WriteLine("m.RemoveBond(m.bonds[0], true);");
                m.RemoveBond(m.Bonds[0], true);


                Console.WriteLine("\n\n\n   4. Converting names to structures and structures to names\n--------------------------------------");
                Console.ReadLine();

                Console.WriteLine("\nConvert a chemical name into a structure.");
                Console.ReadLine();
                Console.WriteLine("m = StructureData.LoadData(\"benzene\", \"name\");");
                m = StructureData.LoadData("benzene", "name");

                Console.WriteLine("\nGet the chemical name of a structure.");
                Console.ReadLine();
                Console.WriteLine("Console.WriteLine(m.WriteData(\"name\"));");
                Console.WriteLine(m.WriteData("name"));

                Console.WriteLine("\nAnother way to get the chemical name.");
                Console.ReadLine();
                Console.WriteLine("Console.WriteLine(m.ChemicalName());");
                Console.WriteLine(m.ChemicalName());


                Console.WriteLine("\n\n\n   5. Converting 2D structures to 3D\n--------------------------------------");
                Console.ReadLine();

                Console.WriteLine("\nFirst we will create a 2D structure.");
                Console.ReadLine();
                Console.WriteLine("m = StructureData.LoadData(\"C=CCO\");");
                m = StructureData.LoadData("C=CCO");
                Console.WriteLine("m.List(true, true, true)");
                m.List(true, true, true);

                Console.WriteLine("\nConvert 2D to 3D.");
                Console.ReadLine();
                Console.WriteLine("m.ConvertTo3DStructure();");
                m.ConvertTo3DStructure();
                Console.WriteLine("m.List(true, true, true);");
                m.List(true, true, true);

                Console.WriteLine("\nRemove Z coordinates.");
                Console.ReadLine();
                Console.WriteLine("m.ClearZCoordinates();");
                m.ClearZCoordinates();

                Console.WriteLine("\nConvert a structure to a 2D structure and clean it up");
                Console.ReadLine();
                Console.WriteLine("m.CleanupStructure();");
                m.CleanupStructure();


                Console.WriteLine("\n\n\n   6. Minimizing MM2 and energy calculations for 3D structures\n--------------------------------------");
                Console.ReadLine();

                Console.WriteLine("\nMM2 minimization");
                Console.ReadLine();
                Console.WriteLine("m.Mm2OptimizeGeometry();");
                m.Mm2OptimizeGeometry();

                Console.WriteLine("\nCompute MM2 energy");
                Console.ReadLine();
                Console.WriteLine("Console.WriteLine(m.Mm2Energy());");
                Console.WriteLine(m.Mm2Energy());


                Console.WriteLine("\n\n\n   7. Salt stripping\n--------------------------------------");
                Console.ReadLine();

                Console.WriteLine("\nLet's load a StructureData object with a salt");
                Console.ReadLine();
                Console.WriteLine("m = StructureData.LoadData(\"CCCCCN.[Na+].[Na+].c1ccccc1.[O]\");");
                m = StructureData.LoadData("CCCCCN.[Na+].[Na+].c1ccccc1.[O]");


                Console.WriteLine("\nSplit salt");
                Console.ReadLine();
                Console.WriteLine("SaltTable st = new SaltTable();");
                SaltTable st = new SaltTable();

                Console.ReadLine();
                Console.WriteLine("st.RegisterWithSmiles(\"[Na+]\", false);");
                st.RegisterWithSmiles("[Na+]", false);
                Console.ReadLine();
                Console.WriteLine("st.RegisterWithSmiles(\"c1ccccc1\", true);");
                st.RegisterWithSmiles("c1ccccc1", true);
                Console.ReadLine();
                Console.WriteLine("st.RegisterWithSmiles(\"[O]\", true);");
                st.RegisterWithSmiles("[O]", true);
                Console.ReadLine();
                Console.WriteLine("VectorOfStructVector list = st.SplitSaltsAndSolvents(m);");
                VectorOfStructVector list = st.SplitSaltsAndSolvents(m);
                Console.ReadLine();
                Console.WriteLine("StructureDataList mainStructureData = list[0];");
                StructureDataList mainStructureData = list[0];
                Console.ReadLine();
                Console.WriteLine("StructureDataList saltPart = list[1];");
                StructureDataList saltPart = list[1];
                Console.ReadLine();
                Console.WriteLine("Console.WriteLine(\"main StructureData:\" + mainStructureData[0].Smiles + \", salt:\" + saltPart[0].Smiles)");
                Console.WriteLine("main StructureData:" + mainStructureData[0].Smiles + ", salt:" + saltPart[0].Smiles);


                Console.WriteLine("\n\n\n   8. Searching structures atom by atom\n--------------------------------------");
                Console.ReadLine();

                Console.WriteLine("\nLet's load two molecules");
                Console.ReadLine();
                Console.WriteLine("StructureData target = StructureData.LoadData(\"C1CCCCC1C\");");
                StructureData target = StructureData.LoadData("C1CCCCC1C");
                Console.WriteLine("StructureData query = StructureData.LoadData(\"C1CCCCC1\");");
                StructureData query = StructureData.LoadData("C1CCCCC1");

                Console.WriteLine("\nNow start the atom-by-atom searching.  This results in a map from the target to query");
                Console.ReadLine();
                Console.WriteLine("AtomsMapVector maps = query.AtomByAtomSearch(target);");
                AtomsMapVector maps = query.AtomByAtomSearch(target);
                Console.WriteLine("foreach(AtomAtomMap dict in maps){");
                Console.WriteLine("    Console.WriteLine(\"one dict:\");");
                Console.WriteLine("    foreach(Atom aa in dict.Keys){");
                Console.WriteLine("        String r = aa.Name + \" -> \" + dict[aa].Name;");
                Console.WriteLine("        Console.WriteLine(r);");
                Console.WriteLine("    }");
                Console.WriteLine("}");
                foreach (AtomAtomMap dict in maps)
                {
                    Console.WriteLine("one dict:");
                    foreach (Atom aa in dict.Keys)
                    {
                        String r0 = aa.Name + " -> " + dict[aa].Name;
                        Console.WriteLine(r0);
                    }
                }

                Console.WriteLine("\nIf you don't care about these atom maps and just want the substructure search result, you can use \"ContainsSubstructure\".");
                Console.ReadLine();
                Console.WriteLine("Console.WriteLine(target.ContainsSubstructure(query));");
                Console.WriteLine(target.ContainsSubstructure(query));


                Console.WriteLine("\n\n\n   9. Finding the largest common substructure\n--------------------------------------");
                Console.ReadLine();

                Console.WriteLine("\nLet's load two molecules");
                Console.ReadLine();
                Console.WriteLine("StructureData structure1 = StructureData.LoadData(\"C1(C)CCCC1CCO\");");
                StructureData structure1 = StructureData.LoadData("C1(C)CCCC1CCO");
                Console.WriteLine("StructureData structure2 = StructureData.LoadData(\"C1CCCC1C\");");
                StructureData structure2 = StructureData.LoadData("C1CCCC1C");

                Console.WriteLine("Use class LargestCommonSubstructure to compute the most common structure\n");
                Console.ReadLine();
                Console.WriteLine("LargestCommonSubstructure common = LargestCommonSubstructure.Compute(structure1, structure2)");
                LargestCommonSubstructure common = LargestCommonSubstructure.Compute(structure1, structure2);
                Console.WriteLine("AtomAtomMap atommap1 = common.AtomMapM1()");
                Console.WriteLine("AtomAtomMap bondmap1 = common.BondMapM1()");
                Console.WriteLine("AtomAtomMap atommap2 = common.AtomMapM2()");
                Console.WriteLine("AtomAtomMap bondmap2 = common.BondMapM2()");
                AtomAtomMap atommap1 = common.AtomMapM1();
                BondBondMap bondmap1 = common.BondMapM1();
                AtomAtomMap atommap2 = common.AtomMapM2();
                BondBondMap bondmap2 = common.BondMapM2();

                Console.WriteLine("foreach(Atom a in atommap1.Keys){");
                Console.WriteLine("    r0 = a.Name + \"->\" + atommap1[a].Name + \"->\" + atommap2[a].Name;");
                Console.WriteLine("        Console.WriteLine(r0);");
                Console.WriteLine("}");
                foreach (Atom a in atommap1.Keys)
                {
                    String r0 = a.Name + "->" + atommap1[a].Name + "->" + atommap2[a].Name;
                    Console.WriteLine(r0);
                }


                Console.WriteLine("\n\n\n   10. Overlaying structures\n--------------------------------------");
                Console.ReadLine();

                Console.WriteLine("\n(1). 2D alignment");
                Console.ReadLine();

                Console.WriteLine("\nFirst let's load two cdx (2D)");
                Console.ReadLine();
                Console.WriteLine("m = StructureData.LoadFile(\"m.cdx\");");
                m = StructureData.LoadFile("m.cdx");
                Console.WriteLine("target = StructureData.LoadFile(\"target.cdx\");");
                target = StructureData.LoadFile("target.cdx");

                Console.WriteLine("\nMake 2D alignment");
                Console.ReadLine();
                Console.WriteLine("Console.WriteLine(m.Overlay(target));");
                Console.WriteLine(m.Overlay(target));

                Console.WriteLine("\nWrite the output into a file");
                Console.ReadLine();
                Console.WriteLine("m.WriteFile(\"m_output.cdx\");");
                m.WriteFile("m_output.cdx");

                Console.WriteLine("\n(2). 3D Overlay");
                Console.WriteLine("\nIf the input consists of 3D structures, the overlay will operate on the 3D coordinates");
                Console.ReadLine();


                Console.WriteLine("\n\n\n   11. Computing molecular topological properties\n--------------------------------------");
                Console.ReadLine();

                Console.WriteLine("\nFirst create a Topology object");
                Console.ReadLine();
                Console.WriteLine("Topology top = m.Topology();");
                Topology top = m.Topology();

                Console.WriteLine("\nThen you are able to get many topology properties");
                Console.ReadLine();
                Console.WriteLine("Console.WriteLine(top.WienerIndex);");
                Console.WriteLine(top.WienerIndex);
                Console.WriteLine("Console.WriteLine(top.BalabanIndex);");
                Console.WriteLine(top.BalabanIndex);
                Console.WriteLine("Console.WriteLine(top.ShapeCoefficient);");
                Console.WriteLine(top.ShapeCoefficient);
                Console.WriteLine("\n\n and more ...");


                Console.WriteLine("\n\n\n   12. Working with ReactionData objects\n--------------------------------------");
                Console.ReadLine();

                Console.WriteLine("\nCreate a reaction from a SMILES string");
                Console.ReadLine();
                Console.WriteLine("ReactionData r = ReactionData.LoadData(\"C1CCC=CC1>>CCCCCC\", \"smiles\");");
                ReactionData r = ReactionData.LoadData("C1CCC=CC1>>CCCCCC", "smiles");

                Console.WriteLine("\nLoad a reaction from a file");
                Console.ReadLine();
                Console.WriteLine("r = ReactionData.LoadFile(\"reaction.cdx\");");
                r = ReactionData.LoadFile("reaction.cdx");
                Console.WriteLine("Console.WriteLine(r.Formula());");
                Console.WriteLine(r.Formula());

                Console.WriteLine("\nGet reactants as a StructureData list");
                Console.ReadLine();
                Console.WriteLine("foreach (StructureData rtn in r.Reactants)");
                Console.WriteLine("     Console.WriteLine(rtn.ToString())");
                foreach (StructureData rtn in r.Reactants)
                    Console.WriteLine(rtn.ToString());

                Console.WriteLine("\nGet products as a StructureData list");
                Console.ReadLine();
                Console.WriteLine("foreach(StructureData prod in r.Products)");
                Console.WriteLine("       Console.WriteLine(prod.ToString())");
                foreach (StructureData prod in r.Products)
                    Console.WriteLine(prod.ToString());


                Console.WriteLine("\n\n\n   13. Reading and writing SD files\n--------------------------------------");
                Console.ReadLine();

                Console.WriteLine("\nRead StructureData from an SD file");
                Console.ReadLine();
                Console.WriteLine("SDFileReader sd = SDFileReader.OpenFile(\"input.sdf\");");
                SDFileReader sd = SDFileReader.OpenFile("input.sdf");
                Console.WriteLine("m = sd.ReadNext()");
                Console.WriteLine("while (m != null){");
                Console.WriteLine("    Console.WriteLine(m.Formula());");
                Console.WriteLine("    Dictionary_String_String items = m.GetDataItems();");
                Console.WriteLine("    foreach(String item in items.Keys){");
                Console.WriteLine("        r0 = item + \"->\" + items[item];");
                Console.WriteLine("        Console.WriteLine(r0)");
                Console.WriteLine("    }");
                Console.WriteLine("    m = sd.ReadNext();");
                Console.WriteLine("}");
                m = sd.ReadNext();
                while (m != null)
                {
                    Console.WriteLine(m.Formula());
                    Dictionary_String_String items = m.GetDataItems();
                    foreach (String item in items.Keys)
                    {
                        String r0 = item + "->" + items[item];
                        Console.WriteLine(r0);
                    }
                    m = sd.ReadNext();
                }

                Console.WriteLine("\nWrite StructureData into an SD file");
                Console.ReadLine();
                Console.WriteLine("SDFileWriter sw = SDFileWriter.OpenFile(\"out.sdf\", FileOpenMode.OverWrite);");
                Console.WriteLine("StructureData m = StructureData.LoadData(\"CCC\");");
                Console.WriteLine("m.SetDataItem(\"atomcount\", \"3\");");
                Console.WriteLine("sd.WriteStructure(m)");
                Console.WriteLine("m = StructureData.LoadData(\"C1CCCCC1\");");
                Console.WriteLine("m.SetDataItem(\"atomcount\", \"6\");");
                Console.WriteLine("sd.WriteStructure(m)");
                SDFileWriter sw = SDFileWriter.OpenFile("out.sdf", FileOpenMode.OverWrite);
                m = StructureData.LoadData("CCC");
                m.SetDataItem("atomcount", "3");
                sw.WriteStructure(m);
                m = StructureData.LoadData("C1CCCCC1");
                m.SetDataItem("atomcount", "6");
                sw.WriteStructure(m);
                Console.WriteLine("\nWelcome to ChemScript!");
               
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Press <Enter> to quit ...");
            Console.ReadLine();

            RestoreCurrentDirectory(oldDir);
        }
    }
}


