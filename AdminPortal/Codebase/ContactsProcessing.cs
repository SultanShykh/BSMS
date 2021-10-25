using AdminPortal.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPortal.Codebase
{
    public class ContactsProcessing
    {
        static dynamic AppDB = Database.OpenNamedConnection("MainDB");
        public static void GetContacts(int currentPage, out List<ContactsModel> contacts, out int totalPages) 
        {
            contacts = new List<ContactsModel>();
            var list = AppDB.COR_Contacts_GetContacts(currentPage: currentPage);

            if (list.FirstOrDefault() != null)
            {
                contacts = list.ToList<ContactsModel>();
            }
            list.NextResult();
            totalPages = list.FirstOrDefault().totalPages;
        }
    }
}