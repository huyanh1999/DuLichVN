using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DC.Common.Extensions
{
    public class EmailHelper
    {
        public static bool SendMail(string to,string cc,string bcc,string subject,string body,ref string response)
        {
            try
            {
                var message = new MailMessage();
                message.To.Add(new MailAddress(to));
                if (!string.IsNullOrEmpty(cc))
                {
                    List<string> emailCc = cc.Split(',').ToList();
                    foreach (var item in emailCc)
                    {
                        message.CC.Add(new MailAddress(item));
                    }                    
                }
                if (!string.IsNullOrEmpty(bcc))
                {
                    List<string> emailBcc = bcc.Split(',').ToList();
                    foreach (var item in emailBcc)
                    {
                        message.Bcc.Add(new MailAddress(item));
                    }
                }
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                }
                return true;
            }
            catch (Exception ex)
            {
                response = ex.ToString();
                return false;
            }
            
        }
    }
}
