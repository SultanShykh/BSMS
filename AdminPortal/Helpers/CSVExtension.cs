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
        
        public static bool geContactsFromExcel(string path, out List<string> list) 
        {
            list = new List<string>();
            list.Clear();

            try
            {
                using (XLWorkbook excel = new XLWorkbook(path))
                {
                    IXLWorksheet xLWorksheet = excel.Worksheet(1);
                    foreach (IXLRow row in xLWorksheet.RowsUsed())
                    {
                        if (Validation.ValidateRecipient(row.Cell(1).Value.ToString(), out string validRes))
                            if (!list.Contains(validRes)) list.Add(validRes);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool getContactsFromCSV(string path, out List<string> list)
        {
            list = new List<string>();
            list.Clear();

            try
            {
                DataTable datatable = CSVLibraryAK.CSVLibraryAK.Import(path, false);

                foreach (DataRow data in datatable.Rows)
                {
                    if (Validation.ValidateRecipient(data.ItemArray[0].ToString(), out string validRes))
                        if (!list.Contains(validRes)) list.Add(validRes);
                }
            }
            catch (Exception ex) 
            {
                return false;
            }
            return true;
        }

        public static bool getDataFromExcel(string path,string msgdata1,out IDictionary<string, string> list)
        {
            list = new Dictionary<string, string>();
            int _endIndex;
            string msgdata = msgdata1;
            string mobNumber = "";
            string cellValue;

            list.Clear();
            cols.Clear();

            cols.Add("B", 2);
            cols.Add("C", 3);
            cols.Add("D", 4);
            cols.Add("E", 5);
            cols.Add("F", 6);
            cols.Add("G", 7);
            cols.Add("H", 8);
            cols.Add("I", 9);
            cols.Add("J", 10);

            try
            {
                using (XLWorkbook excel = new XLWorkbook(path))
                {
                    IXLWorksheet xLWorksheet = excel.Worksheet(1);

                    foreach (IXLRow row in xLWorksheet.RowsUsed())
                    {
                        while (msgdata.IndexOf('$') > 0)
                        {
                            prevStr = msgdata.Substring(0, msgdata.IndexOf('$'));

                            _endIndex = msgdata.IndexOf('$', msgdata.IndexOf('$') + 1);

                            column = msgdata.Substring(msgdata.IndexOf('$') + 1, _endIndex - (msgdata.IndexOf('$') + 1)).Trim();

                            if (cols.ContainsKey(column))
                            {
                                column = cols[column].ToString();

                                if (Validation.ValidateRecipient(row.Cell(1).Value.ToString(), out string validRes)) mobNumber = validRes;

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
            }
            catch (Exception ex) 
            {
                return false;
            }
            
            return true;
        }

        public static bool getDataFromCSV(string path, string msgdata1, out IDictionary<string, string> list)
        {
            list = new Dictionary<string, string>();
            string cellValue = "";
            int colNum;
            int _endIndex;
            string msgdata = msgdata1;

            list.Clear();
            cols.Clear();
            
            cols.Add("B", 1);
            cols.Add("C", 2);
            cols.Add("D", 3);
            cols.Add("E", 4);
            cols.Add("F", 5);
            cols.Add("G", 6);
            cols.Add("H", 7);
            cols.Add("I", 8);
            cols.Add("J", 9);

            try
            {
                DataTable datatable = CSVLibraryAK.CSVLibraryAK.Import(path, false);

                foreach (DataRow data in datatable.Rows)
                {
                    while (msgdata.IndexOf('$') > 0)
                    {
                        prevStr = msgdata.Substring(0, msgdata.IndexOf('$'));

                        _endIndex = msgdata.IndexOf('$', msgdata.IndexOf('$') + 1);

                        column = msgdata.Substring(msgdata.IndexOf('$') + 1, _endIndex - (msgdata.IndexOf('$') + 1)).Trim(' ');

                        if (cols.ContainsKey(column))
                        {
                            colNum = cols[column];

                            if (Validation.ValidateRecipient(data.ItemArray[0].ToString(), out string validRes)) mobNumber = validRes;

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