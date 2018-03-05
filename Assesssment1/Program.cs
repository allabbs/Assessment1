using System;
using System.Collections.Generic;
using System.Linq;

namespace Assessment1
{

    class program
    {

        public static void Main(string[] args)
        {
            string inputFile = "test.csv";
            string outputFile = "output.csv";

            //
            // read-in file
            //

            CsvInterface csvIn = new CsvInterface();
            List<CsvRow> list = csvIn.CsvToList(inputFile);

            //
            // calculate and output results
            //

            // Output the total number of records in the file.
            int totalRecords = GetTotalRecordCount(list);
            Console.WriteLine("Total number of records:\t" + totalRecords);

            // Show the largest sum of Val1 and Val2 for any single row in the CSV, as well as the GUID for that row.
            int index = GetLargestVal1andVal2comboRow(list) - 1;
            Console.WriteLine("Row with largest val1+val2 value:\tGUID=" + list[index].GUID + "\tval1+val2=" + (Int32.Parse(list[index].val1) + Int32.Parse(list[index].val2)));

            // Show any Duplicate GUID values.
            List<CsvRow> dupGuidRows = GetDuplicateGuidRows(list);
            foreach (string s in GetDuplicateGuidList(dupGuidRows))
            {
                Console.WriteLine(s);
            }

            // Show the average length of Val3 across all input rows.
            float averageLength = GetAverageVal3length(list);
            Console.WriteLine("Average Val3 length:\t" + averageLength);



            // output file
            CsvInterface csvOut = new CsvInterface();
            csvOut.csvList = GenerateOutputFileData(list, dupGuidRows, averageLength);
            csvOut.OutputCsvFile(outputFile);

        } //


        /////////////////////////////////
        // Input CSV ////////////////////
        /////////////////////////////////

        //Output the total number of records in the file
        private static int GetTotalRecordCount(List<CsvRow> list)
        {
            return list.Count;
        }

        //Show the largest sum of Val1 and Val2 for any single row in the CSV, as well as the GUID for that row
        private static int GetLargestVal1andVal2comboRow(List<CsvRow> list)
        {
            CsvRow resultRow = new CsvRow();
            int largestNumber = 0;
            int largestComboRow = 0;

            foreach (CsvRow r in list)
            {
                int currentRowComboValue = Int32.Parse(r.val1) + Int32.Parse(r.val2);
                if (resultRow.GUID == null || largestNumber < currentRowComboValue)
                {
                    resultRow.GUID = r.GUID;
                    resultRow.val1 = currentRowComboValue.ToString();
                    largestComboRow = r.rowNumber;
                }
            }

            return largestComboRow;
        }

        //Find duplicate rows
        private static List<CsvRow> GetDuplicateGuidRows(List<CsvRow> list)
        {
            // find duplicate GUID rows
            List<CsvRow> dupList = new List<CsvRow>();
            var dupGUID =
                from l in list
                group l by l.GUID into c
                where c.Count() > 1
                select c.Key;

            var dup = list.FindAll(l => dupGUID.Contains(l.GUID));

            dup.ForEach(d => dupList.Add(d));

            return dupList;
        }

        // get duplicate GUID values
        private static List<string> GetDuplicateGuidList(List<CsvRow> dupRows)
        {
            List<string> dupGuids = dupRows.Select(x => x.GUID).Distinct().ToList();

            return dupGuids;
        }


        //Show the average length of Val3 across all input rows
        private static float GetAverageVal3length(List<CsvRow> list)
        {
            int counter = 0;
            int lengthTotals = 0;


            foreach (CsvRow r in list)
            {
                counter++;
                lengthTotals += r.val3.Length;
            }

            return lengthTotals / (float)counter;

        }

        // output file data
        public static List<CsvRow> GenerateOutputFileData(List<CsvRow> list, List<CsvRow> dupList, float averageLength)
        {
            List<CsvRow> outputData = new List<CsvRow>();
            //
            // generate output data
            //

            foreach (CsvRow r in list)
            {
                //duplicate GUID check
                char isGuidDup = 'N';
                foreach (CsvRow d in dupList)
                    if (d.rowNumber == r.rowNumber)
                    {
                        isGuidDup = 'Y';
                        break;
                    }

                //average check
                char isGreaterThanAverageLength;
                if (r.val3.Length > averageLength) isGreaterThanAverageLength = 'Y';
                else isGreaterThanAverageLength = 'N';


                outputData.Add(new CsvRow()
                {
                    GUID = r.GUID,
                    val1 = (Int32.Parse(r.val1) + Int32.Parse(r.val2)).ToString(),
                    val2 = isGuidDup.ToString(),
                    val3 = isGreaterThanAverageLength.ToString()
                });
            }

            return outputData;
        } //

    } // class
}
