using AdminPortal.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPortal.Codebase
{
    public class OutboxProcessing
    {
        static dynamic AppDB = Database.OpenNamedConnection("MainDB");
        public static void  COR_USP_Outbox(int user_id, int pageNo,string sender,string receiver,string date, string smstype, out List<dynamic> outbox, out int totalPages)
        {
            outbox = new List<dynamic>();

            dynamic Records = AppDB.COR_USP_Outbox(user_id: user_id, sender: sender,receiver: receiver, date: date, currentPage: pageNo, smstype: smstype);

            if (Records.FirstOrDefault() != null)
            {
                outbox = Records.ToList<dynamic>();
            }
            Records.NextResult();
            totalPages = Records.FirstOrDefault().totalPages;
        }
        public static void COR_USP_OutboxDownload(int user_id, string sender, string receiver, string date, string smstype, out List<dynamic> outbox)
        {
            outbox = AppDB.COR_USP_OutboxDownload(user_id: user_id, sender: sender, receiver: receiver, date: date, smstype: smstype).ToList<dynamic>();           
        }
        public static void COR_USP_OutboxCampDownload(int user_id, string sender, string receiver, string date, string smstype, out List<dynamic> outbox)
        {
            outbox = AppDB.COR_USP_OutboxCampDownload(user_id: user_id, sender: sender, receiver: receiver, date: date, smstype: smstype).ToList<dynamic>();
        }
        public static void COR_USP_Outbox_Camp(int user_id, int pageNo, string sender, string receiver, string date, string smstype, out List<dynamic> outbox, out int totalPages)
        {
            outbox = new List<dynamic>();

            dynamic Records = AppDB.COR_USP_Outbox_Camp(user_id: user_id, sender: sender, receiver: receiver, date: date, currentPage: pageNo, smstype: smstype);

            if (Records.FirstOrDefault() != null)
            {
                outbox = Records.ToList<dynamic>();
            }
            Records.NextResult();
            totalPages = Records.FirstOrDefault().totalPages;
        }
        public static void COR_USP_Inbox(int pageNo, string sender, string receiver, out List<Inbox> inbox, out int totalPages)
        {
            inbox = new List<Inbox>();

            dynamic Records = AppDB.COR_USP_Inbox(sender: sender, receiver: receiver, currentPage: pageNo);

            if (Records.FirstOrDefault() != null)
            {
                inbox = Records.ToList<Inbox>();
            }
            Records.NextResult();
            totalPages = Records.FirstOrDefault().totalPages;
        }
        public static void COR_USP_SMSSummary(int UserId, int pageNo, string sender, out List<SMSSummaryModel> SMSSummaryModel, out int totalPages)
        {
            SMSSummaryModel = new List<SMSSummaryModel>();
            dynamic Records = AppDB.COR_USP_SMSSummary(sender: sender, currentPage: pageNo);

            if (Records.FirstOrDefault() != null)
            {
                SMSSummaryModel = Records.ToList<SMSSummaryModel>();
            }
            Records.NextResult();
            totalPages = Records.FirstOrDefault().totalPages;
        }
        public static void COR_USP_ActivityLogs(int pageNo, out List<ActivityLog> ActivityLog, out int totalPages)
        {
            ActivityLog = new List<ActivityLog>();

            dynamic Records = AppDB.COR_USP_ActivityLog(currentPage: pageNo);

            if (Records.FirstOrDefault() != null)
            {
                ActivityLog = Records.ToList<ActivityLog>();
            }
            Records.NextResult();
            totalPages = Records.FirstOrDefault().totalPages;
        }
    }
}