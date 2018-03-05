using System;
using System.IO;
using System.Collections.Generic;

namespace Assesssment1
{

        class CsvInterface
        {
            public List<CsvRow> csvList { get; set; }

            // Constructor
            public CsvInterface()
            { }


            // read-in CSV
            public List<CsvRow> CsvToList(string fullFilePath)
            {
                List<CsvRow> list = new List<CsvRow>();
                int counter = 0;
                string line = string.Empty;

                try
                {
                    using (var reader = new StreamReader(fullFilePath))
                    {
                        //read header
                        line = reader.ReadLine();


                        // read data rows
                        while (reader.EndOfStream == false)
                        {
                            counter++;
                            line = reader.ReadLine();

                            // remove text delimiters
                            line = line.Replace("\"", "").Replace(" ", "");

                            string[] elem = line.Split(",");

                            //Console.WriteLine(counter + " " + elem[0] + " " + elem[1] + " " + elem[2] + " " + elem[3]);

                            list.Add(new CsvRow()
                            {
                                rowNumber = counter,
                                GUID = elem[0],
                                val1 = elem[1],
                                val2 = elem[2],
                                val3 = elem[3]
                            });
                        }

                        //Console.WriteLine(counter);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error in file input...");
                    Console.WriteLine(e);

                }
                return list;
            } //


            public void OutputCsvFile(string fullFilePath)
            {

                // create file (overwrite)

                try
                {
                    if (File.Exists(fullFilePath))
                        File.Delete(fullFilePath);

                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(fullFilePath))
                    {
                        // header
                        sw.WriteLine("\"GUID\", \"Val1\", \"Val2\", \"Val3\"");

                        // data
                        foreach (CsvRow r in csvList)
                        {
                            sw.Write("\"");
                            sw.Write(r.GUID);
                            sw.Write("\", \"");
                            sw.Write(r.val1);
                            sw.Write("\", \"");
                            sw.Write(r.val2);
                            sw.Write("\", \"");
                            sw.Write(r.val3);
                            sw.WriteLine("\"");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error in file ouput...");
                    Console.WriteLine(e);
                }

            } // 






        } // class
    }
}
