using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPortal.Codebase;
using AdminPortal.Helpers;
using AdminPortal.Models;
using ClosedXML.Excel;
using ICSharpCode.SharpZipLib.Zip;

namespace AdminPortal.Controllers
{
    public class ReportsController : Controller
    {
        public ActionResult Outbox(int Id = 1,string sender = null, string receiver = null, string datetime = null)
        {
            ViewBag.datetime = datetime;
            datetime = string.IsNullOrEmpty(datetime) ? null: datetime.Replace(" ", "");
            sender = sender == "" ? null : sender;
            receiver = receiver == "" ? null : receiver;
            OutboxProcessing.COR_USP_Outbox(Convert.ToInt32(Session["UserId"]), Id, sender,receiver,datetime, out List<dynamic> outbox, out int totalPages);
            
            ViewBag.PageNumber = Id;
            ViewBag.totalPages = totalPages;
            ViewBag.action = "Outbox";
            ViewBag.sender = sender;
            ViewBag.receiver = receiver;

            return View("Outbox", outbox);
        }
        public ActionResult OutboxCamp(int Id = 1, string sender = null, string receiver = null, string datetime = null)
        {
            ViewBag.datetime = datetime;
            datetime = string.IsNullOrEmpty(datetime) ? null : datetime.Replace(" ", "");
            sender = sender == "" ? null : sender;
            receiver = receiver == "" ? null : receiver;
            OutboxProcessing.COR_USP_Outbox_Camp(Convert.ToInt32(Session["UserId"]), Id, sender, receiver, datetime, out List<dynamic> outbox, out int totalPages);

            ViewBag.PageNumber = Id;    
            ViewBag.totalPages = totalPages;
            ViewBag.action = "OutboxCamp";
            ViewBag.sender = sender;
            ViewBag.receiver = receiver;

            return View("Outbox", outbox);
        }
        public ActionResult Download(string sender = null, string receiver = null, string datetime = null)
        {
            datetime = string.IsNullOrEmpty(datetime) ? null : datetime.Replace(" ", "");
            sender = sender == "" ? null : sender;
            receiver = receiver == "" ? null : receiver;
            OutboxProcessing.COR_USP_OutboxDownload(Convert.ToInt32(Session["UserId"]), sender, receiver, datetime, out List<dynamic> outbox);

            DataTable dt = new DataTable();
            CSVExtension.getData(dt);
            string fileName = "outbox.zip";
            string path = Path.Combine(Server.MapPath("~/UploadFiles/"), fileName);
            var files = new List<string>();

            using (ZipOutputStream zip = new ZipOutputStream(System.IO.File.Create(path)))
            {
                int i = 1;
                string fname;
                zip.SetLevel(9);
                byte[] buffer = new byte[4096];
                foreach (var v in outbox)
                {
                    DataRow dr = dt.NewRow();

                    dr["username"] = v.username;
                    dr["smstype"] = Convert.ToInt32(v.smstype);
                    dr["masking"] = v.masking;
                    dr["receiver"] = v.receiver;
                    dr["msgdata"] = v.msgdata;
                    dr["senttime"] = Convert.ToDateTime(v.senttime).ToString("dd/MM/yyyy HH:mm:ss");
                    dr["status"] = v.status;
                    dr["cost"] = Convert.ToInt32(v.cost);
                    dr["route"] = Convert.ToInt32(v.route);
                    dt.Rows.Add(dr);

                    if (i%500000 == 0)
                    {
                        fname = "outbox" + i + ".xlsx";
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(dt);
                            using (MemoryStream stream = new MemoryStream())
                            {
                                wb.SaveAs(stream);
                                FileStream file = new FileStream(Server.MapPath("~/UploadFiles/"+fname), FileMode.Create, FileAccess.Write);
                                stream.WriteTo(file);
                                file.Close();
                                dt.Clear();
                            }
                        }
                        files.Add(Server.MapPath("~/UploadFiles/" + fname));
                    }
                    ++i;
                }
                fname = "outbox" + i + ".xlsx";
                files.Add(Server.MapPath("~/UploadFiles/" + fname));

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        FileStream file = new FileStream(Server.MapPath("~/UploadFiles/outbox" + i + ".xlsx"), FileMode.Create, FileAccess.Write);
                        stream.WriteTo(file);
                        file.Close();
                    }
                }

                for (int j = 0; j < files.Count; j++)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(files[j]));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    zip.PutNextEntry(entry);
                    using (FileStream fileStream = System.IO.File.OpenRead(files[j])) 
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                            zip.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                zip.Finish();
                zip.Flush();
                zip.Close();
            }
            byte[] finalResult = System.IO.File.ReadAllBytes(path);
            foreach (var v in files) 
            {
                System.IO.File.Delete(v);
            }
            return File(finalResult, "application/zip", fileName);
        }
        public ActionResult DownloadCamp(string sender = null, string receiver = null, string datetime = null)
        {
            datetime = string.IsNullOrEmpty(datetime) ? null : datetime.Replace(" ", "");
            sender = sender == "" ? null : sender;
            receiver = receiver == "" ? null : receiver;
            OutboxProcessing.COR_USP_OutboxCampDownload(Convert.ToInt32(Session["UserId"]), sender, receiver, datetime, out List<dynamic> outbox);

            DataTable dt = new DataTable();
            CSVExtension.getData(dt);
            string fileName = "outbox.zip";
            string path = Path.Combine(Server.MapPath("~/UploadFiles/"), fileName);
            var files = new List<string>();

            try
            {
                using (ZipOutputStream zip = new ZipOutputStream(System.IO.File.Create(path)))
                {
                    int i = 1;
                    string fname;
                    zip.SetLevel(9);
                    byte[] buffer = new byte[4096];
                    foreach (var v in outbox)
                    {
                        DataRow dr = dt.NewRow();

                        dr["username"] = v.username;
                        dr["smstype"] = Convert.ToInt32(v.smstype);
                        dr["masking"] = v.masking;
                        dr["receiver"] = v.receiver;
                        dr["msgdata"] = v.msgdata;
                        dr["senttime"] = Convert.ToDateTime(v.senttime).ToString("dd/MM/yyyy HH:mm:ss");
                        dr["status"] = v.status;
                        dr["cost"] = Convert.ToInt32(v.cost);
                        dr["route"] = Convert.ToInt32(v.route);
                        dt.Rows.Add(dr);

                        if (i % 500000 == 0)
                        {
                            fname = "outbox" + i + ".xlsx";
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt);
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    wb.SaveAs(stream);
                                    FileStream file = new FileStream(Server.MapPath("~/UploadFiles/" + fname), FileMode.Create, FileAccess.Write);
                                    stream.WriteTo(file);
                                    file.Close();
                                    dt.Clear();
                                }
                            }
                            files.Add(Server.MapPath("~/UploadFiles/" + fname));
                        }
                        ++i;
                    }
                    fname = "outbox" + i + ".xlsx";
                    files.Add(Server.MapPath("~/UploadFiles/" + fname));

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            FileStream file = new FileStream(Server.MapPath("~/UploadFiles/outbox" + i + ".xlsx"), FileMode.Create, FileAccess.Write);
                            stream.WriteTo(file);
                            file.Close();
                        }
                    }

                    for (int j = 0; j < files.Count; j++)
                    {
                        ZipEntry entry = new ZipEntry(Path.GetFileName(files[j]));
                        entry.DateTime = DateTime.Now;
                        entry.IsUnicodeText = true;
                        zip.PutNextEntry(entry);
                        using (FileStream fileStream = System.IO.File.OpenRead(files[j]))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                                zip.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                    }
                    zip.Finish();
                    zip.Flush();
                    zip.Close();
                }
            } 
            catch (Exception ex) 
            {
                throw new Exception();
            }
            byte[] finalResult = System.IO.File.ReadAllBytes(path);
            if(System.IO.File.Exists(path)) System.IO.File.Delete(path);
            foreach (var v in files)
            {
                System.IO.File.Delete(v);
            }
            return File(finalResult, "application/zip", fileName);
        }
    }
}