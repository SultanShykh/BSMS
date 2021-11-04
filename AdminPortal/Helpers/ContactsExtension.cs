using AdminPortal.Models;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace AdminPortal.Helpers
{
    public class ContactsExtension
    {
        public static bool getContactsFromExcel(string path, int user_id, out DataTable dt)
        {
            dt = new DataTable();
            List<string> list = new List<string>(); 
            list.Clear();
            dt.Clear();
            CSVExtension.makeData(out dt);
            try
            {
                using (XLWorkbook excel = new XLWorkbook(path))
                {
                    IXLWorksheet xLWorksheet = excel.Worksheet(1);
                    foreach (IXLRow row in xLWorksheet.RowsUsed())
                    {
                        if (Validation.ValidateRecipient(row.Cell(1).Value.ToString(), out string validRes))
                        {
                            DataRow dr = dt.NewRow();
                            
                            if (!list.Contains(row.Cell(1).Value.ToString())) 
                            {
                                dr["user_id"] = user_id;
                                dr["emails"] = row.Cell(2).Value.ToString();
                                dr["fullname"] = row.Cell(3).Value.ToString();
                                dr["numbers"] = validRes;
                                dr["option1"] = row.Cell(4).Value.ToString();
                                dr["option2"] = row.Cell(5).Value.ToString();
                                dr["option3"] = row.Cell(6).Value.ToString();
                                dr["crtime"] = DateTime.Now;
                                dt.Rows.Add(dr);
                                list.Add(validRes);
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
        public static bool getContactsFromCSV(string path, int user_id, out DataTable dt) 
        {
            dt = new DataTable();
            try
            {
                DataTable datatable = CSVLibraryAK.CSVLibraryAK.Import(path, false);

                foreach (DataRow data in datatable.Rows)
                {
                    Validation.ValidateRecipient(data.ItemArray[0].ToString(), out string mobile_number);

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