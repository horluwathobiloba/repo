using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Text.RegularExpressions; 
using ReventInject.Utilities;
using System.Threading;

namespace ReventInject.Services
{
    public class EmailService
    {

        const string defaultEmail = "support@reventtechnologies.com";

        public static bool isEmail(string inputEmail)
        {
            if (string.IsNullOrEmpty(inputEmail))
            {
                inputEmail = " ";
            }

            string strRegex = "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}" + "\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\" + ".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string getHtmlFilePath(string fileName)
        {
            string result = null;
            dynamic p = Directory.GetCurrentDirectory();
            if (!string.IsNullOrEmpty(p))
            {
                result = Path.Combine(p + "Mail\\", fileName);

            }
            return result;
        }

        public string ReadHtmlFile(string fileName)
        {
            string result = null;
            dynamic p = Directory.GetCurrentDirectory();

            if (!string.IsNullOrEmpty(p))
            {
                dynamic htmlPath = Path.Combine(p + "Mail\\", fileName);
                result = File.ReadAllText(htmlPath);
            }
            return result;
        }

        public string Append
        {

            get
            {
                string title = SysConfig.AppName;
                try
                {
                    if (SysConfig.TestMode)
                    {
                        title = title + " - Demo Mode";
                    }

                }
                catch (Exception ex)
                {
                }

                var b = new StringBuilder();

                b.AppendLine("<table style=\"font-family: Calibri; font-size: 12px; font-weight: bold; width: 100%;\"><tr><td colspan=\"2\">");
                b.AppendLine("<br /><span style=\"color: #0000FF; font-size: 18px;\">FBNQuest </span>");
                b.AppendLine(" <br />");
                b.AppendLine(" <span style=\"font-size: 12px;\">Visit our Website: <a href=\"" + MailConfig.WebsiteURL + "\">" + MailConfig.WebsiteURL + "</a> </span>");
                b.AppendLine(" <br /><br />");
                b.AppendLine(" <br /><br /></td></tr> </table>");

                var result = b.ToString();
                return result;
            }
        }

        public bool SendEmailAlert(string ToEmails, string CCEmails, string BCCEmails, string strMessage, string strTitle,
            string strSenderEmail = defaultEmail, bool deleteAttachments = false, params string[] attache_files)
        {
            bool result = false;
            result = false;

            try
            {
                if (!strMessage.ToLower().Contains("html"))
                {
                    strMessage = "<html><head><title></title></head><body style=\"font-family: Calibri; font-size: 14px;\"> <p>" + strMessage + "</p>" + Append + "</body></html>";
                }

                result = SendMailAlert(ToEmails, CCEmails, BCCEmails, strMessage, strTitle, strSenderEmail, deleteAttachments, attache_files);

            }
            catch (Exception ex)
            {
            }


            return result;

        }

        public bool SendMailAlert(string ToEmails, string CCEmails, string BCCEmails, string strMessage, string strTitle,
            string strSenderEmail = defaultEmail, bool deleteAttachments = false, params string[] attache_files)
        {
            bool result = false;

            if (SysConfig.TestMode)
            {
                strTitle = strTitle + " - Demo Mode";
            }

            string toEmail = ToEmails.ToLower();

            string port = MailConfig.SmtpPort;
            string mailfrom = string.IsNullOrEmpty(strSenderEmail) ? MailConfig.MailFrom : strSenderEmail;
            bool usesAuth = MailConfig.UsesAuthentication;
            string smtpUsername = MailConfig.SmtpUserName;
            string password = MailConfig.SmtpPassword;

            SmtpClient client = new SmtpClient();

            if (Convert.ToBoolean(usesAuth))
            {
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(smtpUsername, password);
            }

            try
            {
                string appPath = SysConfig.AppFolder;
                MailMessage mail = new MailMessage();

                string filepath = appPath + "coylogo.jpg";
                //string ContentID = filepath.Replace(".", "") + "@zofm";
                string HtmlMsg = "";
                AlternateView alternateView;
                //check if the message has fbn logo
                if (strMessage.Contains("#img#"))
                {
                    LinkedResource inline = new LinkedResource(filepath, MediaTypeNames.Image.Jpeg);
                    inline.ContentId = Guid.NewGuid().ToString();
                    string inline_img = @"<img src='cid:" + inline.ContentId + @"'/>";
                    alternateView = AlternateView.CreateAlternateViewFromString(inline_img, null, MediaTypeNames.Text.Html);
                    alternateView.LinkedResources.Add(inline);

                    HtmlMsg = strMessage.Replace("#img#", inline_img);
                    mail.AlternateViews.Add(alternateView);
                }
                else
                {
                    HtmlMsg = strMessage;
                }



                int t = 0;


                if (!string.IsNullOrEmpty(mailfrom) & isEmail(mailfrom))
                {

                    mail = new MailMessage(mailfrom, ToEmails, strTitle, HtmlMsg);

                    //Add the copy addresses
                    if (!string.IsNullOrEmpty(CCEmails))
                    {
                        if (CCEmails.Contains(",") | CCEmails.Contains(";"))
                        {
                            var spp = CCEmails.Split(new char[] { ',', ';' });
                            if (spp != null)
                            {
                                foreach (string x in spp)
                                {
                                    mail.CC.Add(x);
                                }
                            }
                        }
                        else
                        {
                            mail.CC.Add(CCEmails);
                        }
                    }

                    //Add the bcc addresses
                    if (!string.IsNullOrEmpty(BCCEmails))
                    {
                        if (BCCEmails.Contains(",") | BCCEmails.Contains(";"))
                        {
                            var spp = BCCEmails.Split(new char[] { ',', ';' });
                            if (spp != null)
                            {
                                foreach (string x in spp)
                                {
                                    mail.Bcc.Add(x);
                                }
                            }
                        }
                        else
                        {
                            mail.Bcc.Add(CCEmails);
                        }
                    }

                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.Priority = MailPriority.High;
                    mail.IsBodyHtml = true;

                    try
                    {
                        if (attache_files != null)
                        {
                            int i;
                            for (i = 0; i <= attache_files.Length - 1; i++)
                            {
                                try
                                {
                                    string path = attache_files[i];
                                    //string cid = path.Replace(".", "") + "@zofm";
                                    if (path != null)
                                    {
                                        Attachment attach = new Attachment(path);
                                        //attach.ContentDisposition.Inline = true;
                                        //attach.ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
                                        //attach.ContentId = cid;
                                        //attach.ContentType.MediaType = "image/png";
                                        attach.ContentType.Name = Path.GetFileName(path);
                                        mail.Attachments.Add(attach);
                                    }

                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //continue
                    }

                    //if (strMessage.Contains("#img#"))
                    //{
                    //    //Attachment inline = new Attachment(filepath);
                    //    //inline.ContentDisposition.Inline = true;
                    //    //inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                    //    //inline.ContentId = ContentID;
                    //    //inline.ContentType.MediaType = "image/png";
                    //    //inline.ContentType.Name = Path.GetFileName(filepath);
                    //    //mail.Attachments.Add(inline);                       
                    //}

                    client.Host = MailConfig.SmtpHost;

                    client.Send(mail);
                    Console.WriteLine("Mail has been sent to " + toEmail);
                    result = true;

                    mail.Dispose();
                    client = null;

                    if (deleteAttachments)
                    {
                        int i;
                        for (i = 0; i <= attache_files.Length - 1; i++)
                        {
                            try
                            {
                                string path = attache_files[i];
                                File.Delete(path);
                            }
                            catch (Exception ex)
                            {
                                //continue
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ox)
            {
                throw ox;
            }

            return result;
        }

        private AlternateView getEmbeddedImage(String filePath)
        {
            LinkedResource inline = new LinkedResource(filePath);
            inline.ContentId = Guid.NewGuid().ToString();
            string htmlBody = @"<img src='cid:" + inline.ContentId + @"'/>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(inline);
            return alternateView;
        }
    }


}
