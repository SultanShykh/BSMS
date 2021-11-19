using AdminPortal.Models;
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
        private static IDictionary<string, int> cols = new Dictionary<string, int>();
        public static bool getContactsFromExcel(string path, out List<string> list, out int count, out DataTable dt, Campaign campaign,int camp_id) 
        {
            dt = new DataTable();
            count = 0;
            list = new List<string>();
            list.Clear();
            dt.Clear();
            makeColumns(out dt);

            try
            {
                using (XLWorkbook excel = new XLWorkbook(path))
                {
                    IXLWorksheet xLWorksheet = excel.Worksheet(1);
                    foreach (IXLRow row in xLWorksheet.RowsUsed())
                    {
                        if (Validation.ValidateRecipient(row.Cell(1).Value.ToString(), out string validRes))
                            list.Add(validRes);
                    }

                    list = list.Distinct().ToList();

                    if (list.Count <= 0) return false;

                    foreach (var v in list)
                    {
                        DataRow dr = dt.NewRow();

                        dr["camp_id"] = camp_id;
                        dr["user_id"] = campaign.user_id;
                        dr["sender"] = campaign.sender;
                        dr["receiver"] = v;
                        dr["status"] = 1;
                        dr["route"] = 4;
                        dr["cost"] = Math.Ceiling(Convert.ToDecimal(campaign.msgdata.Length) / Convert.ToDecimal(160));
                        dr["senttime"] = DateTime.Now;
                        dr["smstype"] = campaign.camp_smstype;
                        dr["operator"] = 4;
                        dr["isswallow"] = 1;
                        dr["isotpallow"] = 1;
                        dr["RemoteIP"] = "";
                        dr["CurrentDateTime"] = DateTime.Now;
                        dt.Rows.Add(dr);
                        ++count;
                    }
                    if (!string.IsNullOrEmpty(campaign.receiver)) 
                    {
                        foreach (var v in campaign.receiver.Trim(',').Split(','))
                        {
                            DataRow dr = dt.NewRow();

                            dr["camp_id"] = camp_id;
                            dr["user_id"] = campaign.user_id;
                            dr["sender"] = campaign.sender;
                            dr["receiver"] = v;
                            dr["status"] = 1;
                            dr["route"] = 4;
                            dr["cost"] = Math.Ceiling(Convert.ToDecimal(campaign.msgdata.Length) / Convert.ToDecimal(160));
                            dr["senttime"] = DateTime.Now;
                            dr["smstype"] = campaign.camp_smstype;
                            dr["operator"] = 4;
                            dr["isswallow"] = 1;
                            dr["isotpallow"] = 1;
                            dr["RemoteIP"] = "";
                            dr["CurrentDateTime"] = DateTime.Now;
                            dt.Rows.Add(dr);
                            list.Add(v);
                            ++count;
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
        public static bool getContactsFromCSV(string path, out List<string> list, out int count, out DataTable dt, Campaign campaign)
        {
            count = 0;
            dt = new DataTable();
            list = new List<string>();
            list.Clear();
            dt.Clear();
            makeColumns(out dt);
            try
            {
                DataTable datatable = CSVLibraryAK.CSVLibraryAK.Import(path, false);

                foreach (DataRow data in datatable.Rows)
                {
                    if (Validation.ValidateRecipient(data.ItemArray[0].ToString(), out string validRes)) 
                        list.Add(validRes);
                }

                list = list.Distinct().ToList();

                if (list.Count <= 0) return false;

                foreach (var v in list) 
                {
                    DataRow dr = dt.NewRow();
                    
                    dr["camp_id"] = campaign.id;
                    dr["user_id"] = campaign.user_id;
                    dr["sender"] = campaign.sender;
                    dr["receiver"] = v;
                    dr["status"] = 1;
                    dr["route"] = 4;
                    dr["cost"] = Math.Ceiling(Convert.ToDecimal(campaign.msgdata.Length) / Convert.ToDecimal(160));
                    dr["senttime"] = DateTime.Now;
                    dr["smstype"] = campaign.camp_smstype;
                    dr["operator"] = 4;
                    dr["isswallow"] = 1;
                    dr["isotpallow"] = 1;
                    dr["RemoteIP"] = "";
                    dr["CurrentDateTime"] = DateTime.Now;
                    dt.Rows.Add(dr);
                    ++count;
                }
                if (!string.IsNullOrEmpty(campaign.receiver))
                {
                    foreach (var v in campaign.receiver.Trim(',').Split(','))
                    {
                        DataRow dr = dt.NewRow();

                        dr["camp_id"] = campaign.id;
                        dr["user_id"] = campaign.user_id;
                        dr["sender"] = campaign.sender;
                        dr["receiver"] = v;
                        dr["status"] = 1;
                        dr["route"] = 4;
                        dr["cost"] = Math.Ceiling(Convert.ToDecimal(campaign.msgdata.Length) / Convert.ToDecimal(160));
                        dr["senttime"] = DateTime.Now;
                        dr["smstype"] = campaign.camp_smstype;
                        dr["operator"] = 4;
                        dr["isswallow"] = 1;
                        dr["isotpallow"] = 1;
                        dr["RemoteIP"] = "";
                        dr["CurrentDateTime"] = DateTime.Now;
                        dt.Rows.Add(dr);
                        list.Add(v);
                        ++count;
                    }
                }
            }
            catch (Exception ex) 
            {
                return false;
            }
            return true;
        }
        public static bool getDataFromExcel(string path, out IDictionary<string, string> list, out DataTable dt,Campaign campaign, out int count, out int flag)
        {
            list = new Dictionary<string, string>();
            dt = new DataTable();
            string cellValue;
            flag = 0;
            count = 0;

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

            makeColumns(out dt);

            try
            {
                using (XLWorkbook excel = new XLWorkbook(path))
                {
                    IXLWorksheet xLWorksheet = excel.Worksheet(1);

                    foreach (IXLRow row in xLWorksheet.RowsUsed())
                    {
                        string msgdata = campaign.msgdata;
                        Validation.ValidateRecipient(row.Cell(1).Value.ToString(), out string mobile_number);

                        while (msgdata.IndexOf('$') >= 0)
                        {
                            personalizedMessageString(msgdata);

                            if (cols.ContainsKey(column))
                            {
                                column = cols[column].ToString();
                                    
                                cellValue = row.Cell(column).Value.ToString() != null ? row.Cell(column).Value.ToString() : "";
                                nextStr = (msgdata.IndexOf('$') + 3) >= msgdata.Length ? "" : msgdata.Substring(msgdata.IndexOf('$') + 3, msgdata.Length - (msgdata.IndexOf('$') + 3));

                                msgdata = prevStr + "" + cellValue + "" + nextStr;

                                if (!list.ContainsKey(mobile_number))
                                {
                                    list.Add(mobile_number, msgdata);
                                }
                                else
                                {
                                    list[mobile_number] = msgdata;
                                }
                            }
                        }
                    }

                    if (list.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> keyVal in list)
                        {
                            DataRow dr = dt.NewRow();

                            dr["camp_id"] = campaign.id;
                            dr["user_id"] = campaign.user_id;
                            dr["sender"] = campaign.sender;
                            dr["receiver"] = keyVal.Key;
                            dr["msgdata"] = keyVal.Value;
                            dr["status"] = 1;
                            dr["route"] = 4;
                            dr["cost"] = Math.Ceiling(Convert.ToDecimal(keyVal.Value.Length) / Convert.ToDecimal(160));
                            dr["senttime"] = DateTime.Now;
                            dr["smstype"] = campaign.camp_smstype;
                            dr["operator"] = 4;
                            dr["isswallow"] = 1;
                            dr["isotpallow"] = 1;
                            dr["RemoteIP"] = "";
                            dr["CurrentDateTime"] = DateTime.Now;
                            dt.Rows.Add(dr);
                            ++count;
                        }
                    }
                    else
                    {
                        flag = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                flag = 1;
            }

            return true;
        }
        public static bool getDataFromCSV(string path, out IDictionary<string, string> list, out DataTable dt, Campaign campaign, out int count, out int flag)
        {
            list = new Dictionary<string, string>();
            string cellValue = "";
            int colNum;
            flag = 0;
            count = 0;

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

            dt = new DataTable(); 
            makeColumns(out dt);

            try
            {
                DataTable datatable = CSVLibraryAK.CSVLibraryAK.Import(path, false);
                
                foreach (DataRow data in datatable.Rows)
                {
                    string msgdata = campaign.msgdata;
                    Validation.ValidateRecipient(data.ItemArray[0].ToString(), out string mobile_number);

                    while (msgdata.IndexOf('$') >= 0)
                    {
                        personalizedMessageString(msgdata);

                        if (cols.ContainsKey(column))
                        {
                            colNum = cols[column];

                            cellValue = data.ItemArray[colNum].ToString() != null ? data.ItemArray[colNum].ToString() : "";
                               
                            nextStr = (msgdata.IndexOf('$') + 3) >= msgdata.Length ? "" : msgdata.Substring(msgdata.IndexOf('$') + 3, msgdata.Length - (msgdata.IndexOf('$') + 3));

                            msgdata = prevStr + "" + cellValue + "" + nextStr;

                            if (!list.ContainsKey(mobile_number))
                            {
                                list.Add(mobile_number, msgdata);
                            }
                            else
                            {
                                list[mobile_number] = msgdata;
                            }
                        }
                    }
                }

                if (list.Count > 0)
                {
                    foreach (KeyValuePair<string, string> keyVal in list)
                    {
                        DataRow dr = dt.NewRow();

                        dr["camp_id"] = campaign.id;
                        dr["user_id"] = campaign.user_id;
                        dr["sender"] = campaign.sender;
                        dr["receiver"] = keyVal.Key;
                        dr["msgdata"] = keyVal.Value;
                        dr["status"] = 1;
                        dr["route"] = 4;
                        dr["cost"] = Math.Ceiling(Convert.ToDecimal(keyVal.Value.Length) / Convert.ToDecimal(160)); ;
                        dr["senttime"] = DateTime.Now;
                        dr["smstype"] = campaign.camp_smstype;
                        dr["operator"] = 4;
                        dr["isswallow"] = 1;
                        dr["isotpallow"] = 1;
                        dr["RemoteIP"] = "";
                        dr["CurrentDateTime"] = DateTime.Now;
                        dt.Rows.Add(dr);
                        ++count;
                    }
                }
                else
                    flag = 2;
            }
            catch (Exception ex)
            {
                flag = 1;
            }

            return true;
        }
        public static void makeColumns(out DataTable dt) 
        {
            dt = new DataTable();

            dt.Columns.Add(new DataColumn("camp_id", typeof(Int32)));
            dt.Columns.Add(new DataColumn("user_id", typeof(Int32)));
            dt.Columns.Add(new DataColumn("sender", typeof(string)));
            dt.Columns.Add(new DataColumn("receiver", typeof(string)));
            dt.Columns.Add(new DataColumn("msgdata", typeof(string)));
            dt.Columns.Add(new DataColumn("status", typeof(bool)));
            dt.Columns.Add(new DataColumn("route", typeof(Int32)));
            dt.Columns.Add(new DataColumn("cost", typeof(Int32)));
            dt.Columns.Add(new DataColumn("senttime", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("smstype", typeof(string)));
            dt.Columns.Add(new DataColumn("operator", typeof(Int32)));
            dt.Columns.Add(new DataColumn("isswallow", typeof(bool)));
            dt.Columns.Add(new DataColumn("isotpallow", typeof(bool)));
            dt.Columns.Add(new DataColumn("RemoteIP", typeof(string)));
            dt.Columns.Add(new DataColumn("CurrentDateTime", typeof(DateTime)));
        }
        public static void personalizedMessageString(string msgdata)
        {
            prevStr = msgdata.Substring(0, msgdata.IndexOf('$'));

            int _endIndex = msgdata.IndexOf('$', msgdata.IndexOf('$') + 1);

            column = msgdata.Substring(msgdata.IndexOf('$') + 1, _endIndex - (msgdata.IndexOf('$') + 1)).Trim(' ');
        }
        public static DataTable getData(DataTable dt) 
        {
            dt.TableName = "Outbox";
            dt.Clear();
            dt.Columns.Add("username", typeof(string));
            dt.Columns.Add("smstype", typeof(int));
            dt.Columns.Add("masking", typeof(string));
            dt.Columns.Add("receiver", typeof(string));
            dt.Columns.Add("msgdata", typeof(string));
            dt.Columns.Add("senttime", typeof(DateTime));
            dt.Columns.Add("status", typeof(string));
            dt.Columns.Add("cost", typeof(int));
            dt.Columns.Add("route", typeof(int));
            return dt;
        }
    }
}