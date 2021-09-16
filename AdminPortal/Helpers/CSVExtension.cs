using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AdminPortal.Helpers
{
    public class CSVExtension
    {
        private static string prevStr;
        private static string column;
        private static string nextStr;
        private static string mobNumber;
        private static IDictionary<string, int> cols = new Dictionary<string, int>();
        
        public static List<string> getDataFromExcel(string path) 
        {
            List<string> l = new List<string>();
            using (XLWorkbook excel = new XLWorkbook(path)) 
            {
                IXLWorksheet xLWorksheet = excel.Worksheet(1);
                foreach (IXLRow row in xLWorksheet.RowsUsed()) 
                {
                    mobNumber = row.Cell(1).Value.ToString() != null ? row.Cell(1).Value.ToString() : "";
                    if (!l.Contains(row.Cell(1).Value.ToString())) 
                    {
                        l.Add(mobNumber);
                    }
                }
            }
            return l;
        }

        public static bool getDataFromExcel1(string path,string msgdata1,out IDictionary<string, string> list)
        {
            cols.Clear();
            list = new Dictionary<string, string>();
            list.Clear();
            
            cols.Add("B", 2);
            cols.Add("C", 3);
            cols.Add("D", 4);
            cols.Add("E", 5);
            cols.Add("F", 6);
            cols.Add("G", 7);
            cols.Add("H", 8);
            cols.Add("I", 9);
            cols.Add("J", 10);

            using (XLWorkbook excel = new XLWorkbook(path))
            {
                IXLWorksheet xLWorksheet = excel.Worksheet(1);

                foreach (IXLRow row in xLWorksheet.RowsUsed()) 
                {
                    string msgdata = msgdata1;
                    string mobNumber;
                    string cellValue;

                    while (msgdata.IndexOf('$') > 0)
                    {
                        prevStr = msgdata.Substring(0, msgdata.IndexOf('$'));

                        int _endIndex = msgdata.IndexOf('$', msgdata.IndexOf('$') + 1);

                        column = msgdata.Substring(msgdata.IndexOf('$') + 1, _endIndex - (msgdata.IndexOf('$') + 1)).Trim();

                        if (cols.ContainsKey(column))
                        {
                            column = cols[column].ToString();
                            mobNumber = row.Cell(1).Value.ToString() != null ? row.Cell(1).Value.ToString(): "";
                            cellValue = row.Cell(column).Value.ToString() != null ? row.Cell(column).Value.ToString() : "";
                            nextStr = (msgdata.IndexOf('$') + 3) >= msgdata.Length ? "" : msgdata.Substring(msgdata.IndexOf('$') + 3, msgdata.Length - (msgdata.IndexOf('$') + 3));

                            msgdata = prevStr + "" + cellValue + "" + nextStr;

                            if (!list.ContainsKey(mobNumber))
                            {
                                list.Add(mobNumber, msgdata);
                            }
                            else
                            {
                                list[mobNumber] = msgdata;
                            }
                        }
                    }
                }
            }
            
            return true;
        }

        public static bool getDataFromCSV(string path, string msgdata1, out IDictionary<string, string> list)
        {
            cols.Clear();
            list = new Dictionary<string, string>();
            list.Clear();
            cols.Add("B", 1);
            cols.Add("C", 2);
            cols.Add("D", 3);
            cols.Add("E", 4);
            cols.Add("F", 5);
            cols.Add("G", 6);
            cols.Add("H", 7);
            cols.Add("I", 8);
            cols.Add("J", 9);

            string mobNumber;
            string cellValue = "";
            int colNum;

            try
            {
                DataTable datatable = CSVLibraryAK.CSVLibraryAK.Import(path, false);

                foreach (DataRow data in datatable.Rows)
                {
                    string msgdata = msgdata1;
                    while (msgdata.IndexOf('$') > 0)
                    {
                        prevStr = msgdata.Substring(0, msgdata.IndexOf('$'));

                        int _endIndex = msgdata.IndexOf('$', msgdata.IndexOf('$') + 1);

                        column = msgdata.Substring(msgdata.IndexOf('$') + 1, _endIndex - (msgdata.IndexOf('$') + 1)).Trim();

                        if (cols.ContainsKey(column))
                        {
                            colNum = cols[column];
                            mobNumber = data.ItemArray[0].ToString() != null ? data.ItemArray[0].ToString() : "";
                            cellValue = data.ItemArray[colNum].ToString() != null ? data.ItemArray[colNum].ToString() : "";

                            nextStr = (msgdata.IndexOf('$') + 3) >= msgdata.Length ? "" : msgdata.Substring(msgdata.IndexOf('$') + 3, msgdata.Length - (msgdata.IndexOf('$') + 3));

                            if (!list.ContainsKey(mobNumber))
                            {
                                msgdata = prevStr + "" + cellValue + "" + nextStr;
                                list.Add(mobNumber, msgdata);
                            }
                            else
                            {
                                msgdata = prevStr + "" + cellValue + "" + nextStr;
                                list[mobNumber] = msgdata;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                return false;
            }
 
            return true;
        }
    }
}