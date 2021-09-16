using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminPortal.Models;
using Simple.Data;
using Simple.Data.Ado;

namespace AdminPortal.AccountProcessing
{
    public class AccountProcessingController
    {
        private dynamic AppDB = Database.OpenNamedConnection("MainDB");

        public dynamic userlogin(string username,string pass) 
        {
            var db = AppDB.dbuserlogin(username: username,password: pass);
            return db;
        }

        public dynamic dbusers() 
        {
            var db = AppDB.dbusers();
            return db;
        }

        public dynamic dbuserinfo(int? id)
        {
            var db = AppDB.dbusersinfo(id);
            return db;
        }

        public dynamic dbresellers()
        {
            var db = AppDB.dbresellers();
            return db;
        }

        public dynamic dbresellerupdate(int id, string username, string email, string pass, string userrole, string ph_no) 
        {
            var db = AppDB.dbusersupdate(id,username,email,pass,userrole,ph_no);
            return db;
        }

        public dynamic dbresellercreate(string username, string email, string pass, string userrole, string ph_no,string path = null)
        {
            var db = AppDB.dbresellercreate(username, email, pass, userrole, ph_no,path);
            return db;
        }

        public dynamic dbresellerdelete(int id)
        {
            var db = AppDB.dbresellerdelete(id);
            return db;
        }

        public IEnumerable<User_roles> SP_user_roles()
        {
            var user_Roles = AppDB.SP_user_roles() ?? new List<User_roles>();
            return user_Roles;
        }
    }
}