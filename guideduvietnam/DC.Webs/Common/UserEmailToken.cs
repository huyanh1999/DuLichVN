using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Threading;
using DC.Common.Utility;

namespace DC.Webs.Common
{
    public class UserEmailToken
    {
        public static string FromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
        public static string SmtpHostValue = ConfigurationManager.AppSettings["SmtpHostValue"].ToString();
        public static string SmtpPortValue = ConfigurationManager.AppSettings["SmtpPortValue"].ToString();
        public static string EmailName = ConfigurationManager.AppSettings["EmailName"].ToString();
        public static bool SMTPAuthenticationEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPAuthenticationEnable"]);
        public static string SMTPAuthenticationPassword = ConfigurationManager.AppSettings["SMTPAuthenticationPassword"].ToString();

        
        /// <summary>
        /// This method use a Thread for sending Email(Gmail or extend gmail only) 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool SendMail(string to, string subject, string message)
        {
            try
            {
                NetworkCredential loginInfo = new NetworkCredential(FromEmailAddress, SMTPAuthenticationPassword);

                MailMessage mm = new MailMessage(FromEmailAddress, to);

                mm.Sender = new MailAddress(FromEmailAddress);
                mm.Subject = subject;
                mm.Body = message;
                mm.IsBodyHtml = true;
                SmtpClient smtp;
                if (SmtpPortValue != "-1")
                    smtp = new SmtpClient(SmtpHostValue, Int32.Parse(SmtpPortValue));
                else
                    smtp = new SmtpClient(SmtpHostValue);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = loginInfo;
                smtp.Send(mm);

                return true;
            }
            catch(Exception ex)
            {                
                return false;
            }

        }
    }
}