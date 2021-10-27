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
        public static string COR_Contacts_UpdateContact(ContactsModel contacts) 
        {
            var result = AppDB.COR_Contacts_UpdateContact(contacts.id, contacts.user_id, contacts.fullname, contacts.groupname, contacts.numbers, contacts.emails, contacts.option1, contacts.option2, contacts.option3).FirstOrDefault();
            return result.message.ToString();
        }
        public static string COR_Contacts_AddContact(ContactsModel contacts)
        {
            var result = AppDB.COR_Contacts_AddContact(contacts.user_id, contacts.fullname, contacts.groupname, contacts.numbers, contacts.emails, contacts.option1, contacts.option2, contacts.option3).FirstOrDefault();
            return result.message.ToString();
        }
        public static List<ContactGroupModel> GetContactGroups() 
        {
            var list = AppDB.groups.All().ToList<ContactGroupModel>();
            return list;
        }
        public static void GetGroups(int currentPage, out List<dynamic> contacts, out int totalPages)
        {
            contacts = new List<dynamic>();
            var list = AppDB.COR_Contacts_GetGroups(currentPage: currentPage);

            if (list.FirstOrDefault() != null)
            {
                contacts = list.ToList<dynamic>();
            }
            list.NextResult();
            totalPages = list.FirstOrDefault().totalPages;
        }
        public static void COR_Contacts_AddGroup(ContactGroupModel group, out string result, out string status)
        {
            result = "";
            var record = AppDB.COR_Contacts_AddGroup(group.user_id, group.groupname, group.description);
            result = record.FirstOrDefault().message;
            record.NextResult();
            status = record.FirstOrDefault().status;
        }
        public static void COR_Contacts_UpdateGroup(ContactGroupModel group, out string result, out string status)
        {
            result = "";
            var record = AppDB.COR_Contacts_UpdateGroup(group.id, group.groupname, group.description);
            result = record.FirstOrDefault().message;
            record.NextResult();
            status = record.FirstOrDefault().status;
        }
        public static void DeleteGroup(int Id) 
        {
            AppDB.groups.DeleteById(Id);
            AppDB.user_groups_contacts.Delete(group_id: Id);
        }
        public static void DeleteContact(int Id)
        {
            AppDB.contacts.DeleteById(Id);
            AppDB.user_groups_contacts.Delete(contact_id: Id);
        }
    }
}