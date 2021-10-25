using AdminPortal.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPortal.Codebase
{
    public class ContactGroupProcessing
    {
        static dynamic AppDB = Database.OpenNamedConnection("MainDB");
        public static List<ContactGroupModel> GetContactGroups() 
        {
            return AppDB.groups.All().ToList<ContactGroupModel>();
        }
    }
}