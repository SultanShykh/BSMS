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
        public static void GetContacts(int currentPage, string fullname, string email, int user_id, out List<ContactsModel> contacts, out int totalPages) 
        {
            contacts = new List<ContactsModel>();
            var list = AppDB.COR_Contacts_GetContacts(currentPage: currentPage, user_id: user_id, fullname: fullname, email: email);

            if (list.FirstOrDefault() != null)
            {
                contacts = list.ToList<ContactsModel>();
            }
            list.NextResult();
            totalPages = list.FirstOrDefault().totalPages;
        }
        public static string COR_Contacts_UpdateContact(ContactsModel contacts) 
        {
            var result = AppDB.COR_Contacts_UpdateContact(contacts.id, contacts.user_id, contacts.fullname, contacts.groupname, contacts.numbers, contacts.emails, contacts.option1, contacts.option2, contacts.option3, contacts.ugc_id).FirstOrDefault();
            return result.message.ToString();
        }
        public static string COR_Contacts_AddContact(ContactsModel contacts)
        {
            var result = AppDB.COR_Contacts_AddContact(contacts.user_id, contacts.fullname, contacts.groupname, contacts.numbers, contacts.emails, contacts.option1, contacts.option2, contacts.option3).FirstOrDefault();
            return result.message.ToString();
        }
        public static List<ContactGroupModel> GetContactGroups() 
        {
            var list = AppDB.groups.FindAllBy(user_id: Convert.ToInt32(HttpContext.Current.Session["UserId"])).ToList<ContactGroupModel>();
            return list;
        }
        public static void GetGroups(int currentPage, int user_id, out List<dynamic> contacts, out int totalPages)
        {
            contacts = new List<dynamic>();
            var list = AppDB.COR_Contacts_GetGroups(currentPage: currentPage, user_id: user_id);

            if (list.FirstOrDefault() != null)
            {
                contacts = list.ToList<dynamic>();
            }
            list.NextResult();
            totalPages = list.FirstOrDefault().totalPages;
        }
        public static void COR_Contacts_AddGroup(ContactGroupModel group, out string result, out string status)
        {
            var record = AppDB.COR_Contacts_AddGroup(group.user_id, group.groupname, group.description);
            result = record.FirstOrDefault().message;
            record.NextResult();
            status = record.FirstOrDefault().status;
        }
        public static void COR_Contacts_UpdateGroup(ContactGroupModel group, out string result, out string status)
        {
            var record = AppDB.COR_Contacts_UpdateGroup(group.id, group.groupname, group.description);
            result = record.FirstOrDefault().message;
            record.NextResult();
            status = record.FirstOrDefault().status;
        }
        public static void DeleteGroup(int Id, int user_id) 
        {
            AppDB.groups.Delete(id:Id, user_id:user_id);
            AppDB.user_groups_contacts.Delete(group_id: Id, user_id: user_id);
        }
        public static void DeleteContact(int Id, int user_id, int groupId)
        {
            AppDB.user_groups_contacts.Delete(contact_id:Id, user_id: user_id, group_id: groupId);
        }
        public static void AssignMultipleGroup(ContactsModel model, out string result, out string status) 
        {
            var record = AppDB.COR_Contacts_AssignMultipleGroup(model.id, model.user_id, model.groupname);
            result = record.FirstOrDefault().message;
            record.NextResult();
            status = record.FirstOrDefault().status;
        }
        public static List<ContactsModel> ViewContacts(int userid, int? Id) 
        {
            List<ContactsModel> model = AppDB.COR_Contacts_ViewContacts(user_id: userid, groupid: Id) ?? new List<ContactsModel>();
            return model;
        }
        public static List<ContactsModel> ViewAssignContacts(int userid, int groupId, int contactId) 
        {
            List<ContactsModel> model = AppDB.COR_Contacts_ViewAssignContacts(user_id: userid, groupid: groupId, contactid: contactId) ?? new List<ContactsModel>();
            return model;
        }
        public static void _AssignContacts(int numbers, int groupId, int user_id) 
        {
            AppDB.user_groups_contacts.Insert(user_id: user_id, group_id: groupId, contact_id: numbers);
        }
        public static List<ContactsModel> DownloadContacts(int user_id, string fullname, string email) 
        {
            var list = AppDB.COR_Contacts_Download(user_id, fullname, email) ?? new List<ContactsModel>();
            return list;
        }
        public static List<ContactsModel> GetUserContacts(int user_id) 
        {
            return AppDB.contacts.FindAllBy(user_id: user_id) ?? new List<ContactsModel>();
        }
        public static List<ContactGroupModel> GetAssociatedGroups() 
        {
            List<ContactGroupModel> list = AppDB.groups.FindAll(AppDB.groups.user_id == Convert.ToInt32(HttpContext.Current.Session["UserId"])) ?? new List<ContactGroupModel>();
            return list;
        }
    }
}