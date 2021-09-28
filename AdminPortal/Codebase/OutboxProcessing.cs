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
        public static void  COR_USP_Outbox(int user_id, int pageNo,string sender,string receiver,string date, out List<dynamic> outbox, out int totalPages)
        {
            outbox = new List<dynamic>();

            dynamic Records = AppDB.COR_USP_Outbox(user_id: user_id, sender: sender,receiver: receiver, date: date, currentPage: pageNo);

            if (Records.FirstOrDefault() != null)
            {
                outbox = Records.ToList<dynamic>();
            }
            Records.NextResult();
            totalPages = Records.FirstOrDefault().totalPages;
        }

        public static void COR_USP_Outbox_Camp(int user_id, int pageNo, string sender, string receiver, string date, out List<dynamic> outbox, out int totalPages)
        {
            outbox = new List<dynamic>();

            dynamic Records = AppDB.COR_USP_Outbox_Camp(user_id: user_id, sender: sender, receiver: receiver, date: date, currentPage: pageNo);

            if (Records.FirstOrDefault() != null)
            {
                outbox = Records.ToList<dynamic>();
            }
            Records.NextResult();
            totalPages = Records.FirstOrDefault().totalPages;
        }
    }
}