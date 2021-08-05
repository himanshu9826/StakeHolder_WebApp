using StakeHolder.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StakeHolder.BAL.Common
{
    public class MailManager
    {
        /// <summary>
        /// Send Email Locally
        /// </summary>
        /// <param name="to">To</param>
        /// <param name="body">Body</param>
        /// <param name="subject">Subject</param>
        public void SendEmailLocally(string to, string body, string subject)
        {

            try
            {
                int EnableMail = Convert.ToInt32(ConfigurationManager.AppSettings["EnableMail"]);
                if (EnableMail == 1)
                {
                    string bccEmailAddress = ConfigurationManager.AppSettings["BCCEmail"].ToString();
                    string SupportDisplayName = ConfigurationManager.AppSettings["SupportDisplayName"].ToString();
                    string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    string userName = ConfigurationManager.AppSettings["UserName"].ToString();
                    string password = ConfigurationManager.AppSettings["Password"].ToString();
                    string hostName = ConfigurationManager.AppSettings["HostName"].ToString();
                    int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                    bool isSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSL"]);
                    bool UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]);
                    MailMessage msg = new MailMessage();
                    msg.To.Add(new MailAddress(to));
                    msg.Bcc.Add(new MailAddress(bccEmailAddress));
                    msg.From = new MailAddress(fromEmail, SupportDisplayName);
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient();
                    client.Host = hostName;
                    client.UseDefaultCredentials = UseDefaultCredentials;
                    client.Credentials = new System.Net.NetworkCredential(userName, password);
                    client.Port = portNumber;
                    client.EnableSsl = isSSL;

                    client.Send(msg);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MailManager", "SendEmailLocally", ex);
            }

        }

        /// <summary>
        /// Send mail to multiple address
        /// </summary>
        /// <param name="to">To Email</param>
        /// <param name="body">Message Body</param>
        /// <param name="subject">Subject</param>
        public void SendEmailLocallyToMultipleAddress(string to, string body, string subject)
        {
            try
            {
                int EnableMail = Convert.ToInt32(ConfigurationManager.AppSettings["EnableMail"]);

                if (EnableMail == 1)
                {
                    string bccEmailAddress = ConfigurationManager.AppSettings["BCCEmail"].ToString();
                    string SupportDisplayName = ConfigurationManager.AppSettings["SupportDisplayName"].ToString();
                    string userName = ConfigurationManager.AppSettings["UserName"].ToString();
                    string password = ConfigurationManager.AppSettings["Password"].ToString();
                    string hostName = ConfigurationManager.AppSettings["HostName"].ToString();
                    int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                    bool isSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSL"]);
                    MailMessage msg = new MailMessage();

                    List<string> toList = to.Split(',').ToList();
                    if (toList != null && toList.Count > 0)
                    {
                        foreach (string item in toList)
                            msg.To.Add(new MailAddress(item));
                    }

                    msg.Bcc.Add(new MailAddress(bccEmailAddress));
                    msg.From = new MailAddress(userName, SupportDisplayName);
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient();
                    client.Host = hostName;
                    client.Credentials = new System.Net.NetworkCredential(userName, password);
                    client.Port = portNumber;
                    client.EnableSsl = isSSL;
                    client.Send(msg);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MailManager", "SendEmailLocallyToMultipleAddress", ex);
            }

        }

        /// <summary>
        /// Send Email Locally
        /// </summary>
        /// <param name="to">To</param>
        /// <param name="body">Body</param>
        /// <param name="subject">Subject</param>
        public bool SendEmailLocallyWithConfirmation(string to, string body, string subject)
        {
            bool flag = false;
            try
            {
                int EnableMail = Convert.ToInt32(ConfigurationManager.AppSettings["EnableMail"]);
                if (EnableMail == 1)
                {
                    string bccEmailAddress = ConfigurationManager.AppSettings["BCCEmail"].ToString();
                    string SupportDisplayName = ConfigurationManager.AppSettings["SupportDisplayName"].ToString();
                    string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    string userName = ConfigurationManager.AppSettings["UserName"].ToString();
                    string password = ConfigurationManager.AppSettings["Password"].ToString();
                    string hostName = ConfigurationManager.AppSettings["HostName"].ToString();
                    int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                    bool isSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSL"]);
                    bool UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]);

                    MailMessage msg = new MailMessage();
                    msg.To.Add(new MailAddress(to));
                    msg.Bcc.Add(new MailAddress(bccEmailAddress));
                    msg.From = new MailAddress(fromEmail, SupportDisplayName);
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient();
                    client.Host = hostName;
                    client.UseDefaultCredentials = UseDefaultCredentials;
                    client.Credentials = new System.Net.NetworkCredential(userName, password);
                    client.EnableSsl = isSSL;
                    client.Send(msg);
                    flag = true;

                }
            }
            catch (Exception ex)
            {
                flag = false;
                LibLogging.WriteErrorToDB("MailManager", "SendEmailLocally", ex);
            }
            return flag;
        }



        #region Create send mail async

        public void SendEmailLocallyAsync(string to, string body, string subject)
        {
            try
            {
                int EnableMail = Convert.ToInt32(ConfigurationManager.AppSettings["EnableMail"]);
                if (EnableMail == 1)
                {
                    string bccEmailAddress = ConfigurationManager.AppSettings["BCCEmail"].ToString();
                    string SupportDisplayName = ConfigurationManager.AppSettings["SupportDisplayName"].ToString();
                    string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    string userName = ConfigurationManager.AppSettings["UserName"].ToString();
                    string password = ConfigurationManager.AppSettings["Password"].ToString();
                    string hostName = ConfigurationManager.AppSettings["HostName"].ToString();
                    int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                    bool isSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSL"]);
                    bool UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]);
                    MailMessage msg = new MailMessage();
                    msg.To.Add(new MailAddress(to));
                    msg.Bcc.Add(new MailAddress(bccEmailAddress));
                    msg.From = new MailAddress(fromEmail, SupportDisplayName);
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient();
                    client.Host = hostName;
                    client.UseDefaultCredentials = UseDefaultCredentials;
                    client.Credentials = new System.Net.NetworkCredential(userName, password);
                    client.Port = portNumber;
                    client.EnableSsl = isSSL;

                    client.SendMailAsync(msg);

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MailManager", "SendEmailLocallyAsync", ex);
            }
        }



        #endregion
    }
}
