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
        public static void  COR_USP_Outbox(int user_id, int pageNo, out List<dynamic> outbox, out int totalPages)
        {
            outbox = new List<dynamic>();
            totalPages = 0;

            dynamic Records = AppDB.COR_USP_Outbox(user_id: user_id, currentPage: pageNo);

            if (Records.FirstOrDefault() != null)
            {
                outbox = Records.ToList<dynamic>();
            }
            Records.NextResult();
            totalPages = Records.FirstOrDefault().totalPages;
        }
    }
}