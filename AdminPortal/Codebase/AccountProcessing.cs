using Microsoft.Ajax.Utilities;
using Simple.Data;
using Simple.Data.Ado;
using System;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Razor.Generator;

namespace AdminPortal.Codebase
{
    public class AccountProcessing
    {
        dynamic AppDB = Database.OpenNamedConnection("MainDB");
        public dynamic UserLogin(string username, string password)
        {
            var result = AppDB.COR_WEB_LoginUser(Username: username, Password: password);
            return result;
        }

        public string SendMail(string email,string username, string name, string host)
        {
            
            string sender = WebConfigurationManager.AppSettings["PFUserName"].ToString();
            string password = WebConfigurationManager.AppSettings["PFPassword"].ToString();
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            string msg = string.Empty;
            try
            {
                MailAddress fromAddress = new MailAddress(sender, "ITS");
                message.From = fromAddress;
                message.To.Add(email);
                message.Subject = "Reset Password";
                message.IsBodyHtml = true;
                message.Body = "<div style='width: 75%;margin-left: 5%; background: #ebeef1;padding: 20px; border-radius: 5px;border: 1px solid #34abb8;border-top: 2px solid #34abb8;'> Dear <b> " + name + ", </b><br/> <p>We received a request to change your password. Use the link below to set up a new password for your account.</p><br/><br/><div><a target='_blank'  href='" + host + "/Account/ResetPassword?username="+username+ "' ><form method='get' id='EmailForm' action='/Account/ResetPassword'><input type='submit' value='Reset Password' id='Reset' class='form-control' style='margin-left: 35%; height: 41px; width: 200px; font-size: 18px; background-color: #0B610B!important'/></form></a></div><br/><br/>If you did not make this request, you do not need to do anything.<br/>Thanks for your time,<br/>Team ITS.<br/><br/><img width='200' src='https://its.com.pk/wp-content/uploads/2020/04/black-logo.png'></div>";
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new System.Net.NetworkCredential(sender, password);
                smtpClient.Send(message);
                msg = "Successful";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
    }
}