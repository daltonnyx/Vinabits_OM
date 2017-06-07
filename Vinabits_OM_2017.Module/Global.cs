using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using Vinabits_OM_2017.Module.BusinessObjects;
using DevExpress.ExpressApp.MiddleTier;
using DevExpress.ExpressApp.Security.ClientServer;
using System.Net.Mail;
using System.Net;
using System.Threading;
using DevExpress.XtraReports.UI;
using System.Text;
using System.IO;
using DevExpress.ExpressApp.ReportsV2;
using System.Collections.Generic;
using DevExpress.Xpo.Metadata;

namespace Vinabits_OM_2017.Module
{
    public static class Global
    {
        private delegate void SentMailAsyncDelegate(string mailTos, string subject, string body, bool isHtml = false); //delegate for the action

        public static Configuration Config;
        public static Employee CurrentEmployee;
        public static int NewMessageCount = 0;

        public static string SubStringExt(this string value, int length)
        {
            if (value == null || value.Length < length || value.IndexOf(" ", length) == -1)
                return value;

            return value.Substring(0, value.IndexOf(" ", length));
        }

        public static string genPaymentsRequestCode(int num)
        {
            string stCode = Config.PreCodePaymentsRequest;

            //Tạm thời: lấy năm + tháng
            stCode += DateTime.Today.ToString("yyMM");

            stCode += string.Format(string.Format("{{0:d{0}}}", Config.NumCodePaymentsRequest), num);
            return stCode;
        }

        public static string genReceiptsCode(int num)
        {
            string stCode = Config.PreCodeReceipts;

            //Tạm thời: lấy năm + tháng
            stCode += DateTime.Today.ToString("yyMM");

            stCode += string.Format(string.Format("{{0:d{0}}}", Config.NumCodeReceipts), num);
            return stCode;
        }

        public static void sendMailBMSDelegate(string mailTos, string subject, string body, bool isHtml = false)
        {
            SentMailAsyncDelegate sent = new SentMailAsyncDelegate(sendMailBMS);
            sent.BeginInvoke(mailTos, subject, body, isHtml, null, null);
        }

        private static void sendMailBMS(string mailTos, string subject, string body, bool isHtml = false)
        {
            if (!mailTos.Equals(string.Empty) && !Config.SmtpFrom.Equals(string.Empty))
            {
                // Emulating some background thread operations.
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s1, e1) =>
                {
                    Thread.Sleep(2000);
                    string userToken = string.Format("BMS send to '{0}'", mailTos);
                    MailMessage mailMessage = new MailMessage();
                    SmtpClient mailSender = new SmtpClient(Config.SmtpHost, Config.SmtpPort);
                    mailSender.Credentials = new NetworkCredential(Config.SmtpUser, Config.SmtpPass);
                    //mailSender.SendCompleted += new SendCompletedEventHandler(mailSender_SendCompleted);
                    mailMessage.Subject = subject;
                    mailMessage.IsBodyHtml = isHtml;
                    mailMessage.Body = body;
                    mailMessage.To.Add(mailTos);
                    mailMessage.From = new MailAddress(Config.SmtpFrom);
                    mailSender.SendCompleted += (s, e) =>
                    {
                        mailSender.Dispose();
                        mailMessage.Dispose();
                        String token = (string)e.UserState;
                        if (e.Cancelled)
                        {
                            Console.WriteLine("[{0}] Send canceled.", token);
                        }
                        if (e.Error != null)
                        {
                            Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                        }
                        else
                        {
                            Console.WriteLine("Message sent.");
                        }
                    };
                    //mailSender.SendAsync(mailMessage, userToken);
                    mailSender.Send(mailMessage);
                    //mailMessage.Dispose();
                };
                //Updating the current View to take the latest database changes into account.
                worker.RunWorkerCompleted += (s2, e2) =>
                {
                    Console.WriteLine("Send Completed!!");
                };
                worker.RunWorkerAsync();
            }

            /*
            if (!mailTos.Equals(string.Empty) && !Config.SmtpFrom.Equals(string.Empty))
            {
                string userToken = string.Format("BMS send to '{0}'", mailTos);
                MailMessage mailMessage = new MailMessage();
                SmtpClient mailSender = new SmtpClient(Config.SmtpHost, Config.SmtpPort);
                mailSender.Credentials = new NetworkCredential(Config.SmtpUser, Config.SmtpPass);
                //mailSender.SendCompleted += new SendCompletedEventHandler(mailSender_SendCompleted);
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = isHtml;
                mailMessage.Body = body;
                mailMessage.To.Add(mailTos);
                mailMessage.From = new MailAddress(Config.SmtpFrom);
                mailSender.SendCompleted += (s, e) =>
                {
                    mailSender.Dispose();
                    mailMessage.Dispose();
                };
                mailSender.SendAsync(mailMessage, userToken);
                //mailMessage.Dispose();
            }
             * */
        }

        private static void sendMailBMSNow(string mailTos, string subject, string body, bool isHtml = false)
        {
            if (!mailTos.Equals(string.Empty) && !Config.SmtpFrom.Equals(string.Empty))
            {
                string userToken = string.Format("BMS send to '{0}'", mailTos);
                MailMessage mailMessage = new MailMessage();
                SmtpClient mailSender = new SmtpClient(Config.SmtpHost, Config.SmtpPort);
                mailSender.Credentials = new NetworkCredential(Config.SmtpUser, Config.SmtpPass);
                //mailSender.SendCompleted += new SendCompletedEventHandler(mailSender_SendCompleted);
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = isHtml;
                mailMessage.Body = body;
                mailMessage.To.Add(mailTos);
                mailMessage.From = new MailAddress(Config.SmtpFrom);
                mailSender.SendCompleted += (s, e) =>
                {
                    mailSender.Dispose();
                    mailMessage.Dispose();
                    String token = (string)e.UserState;
                    if (e.Cancelled)
                    {
                        Console.WriteLine("[{0}] Send canceled.", token);
                    }
                    if (e.Error != null)
                    {
                        Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Message sent.");
                    }
                };
                mailSender.Send(mailMessage);
                //mailMessage.Dispose();
            }
        }

        static void mailSender_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            //Tracing.LogText(new Guid(), e.Error.Message, sender);
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
        }

        #region Hàm đọc tiền "bằng chữ"
        public static string DocsoBangchu(string number)
        {
            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;
            string firstNegative = string.Empty;

            if (!string.IsNullOrEmpty(number) && number.Substring(0, 1).Equals("-")) 
            {
                firstNegative = "[-]";
                number = number.Substring(1);
            }
            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "lẻ ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if (i + j == len - 1)
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += dv[n - j - 1] + " ";
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            //Fix: mươi năm => mươi lăm, mươi một => mươi mốt
            doc = doc.Replace("mươi năm", "mươi lăm");
            doc = doc.Replace("mươi một", "mươi mốt");

            return string.Format("{0}{1}",firstNegative,doc);
        }
        #endregion

    }

    public class CloneHelper : IDisposable
    {
        readonly Dictionary<object, object> clonedObjects;
        readonly Session targetSession;

        public CloneHelper(Session targetSession)
        {
            clonedObjects = new Dictionary<object, object>();
            this.targetSession = targetSession;
        }

        public T Clone<T>(T source)
        {
            return Clone<T>(source, false);
        }
        public T Clone<T>(T source, bool synchronize)
        {
            return (T)Clone((object)source, synchronize);
        }
        public object Clone(object source)
        {
            return Clone(source, false);
        }

        /// <param name="synchronize">If set to true, reference properties are only cloned in case
        /// the reference object does not exist in the targetsession. Otherwise the exising object will be
        /// reused and synchronized with the source. Set this property to false when knowing at forehand 
        /// that the targetSession will not contain any of the objects of the source.</param>
        /// <returns></returns>
        public object Clone(object source, bool synchronize)
        {
            if (source == null)
                return null;
            XPClassInfo targetClassInfo = targetSession.GetClassInfo(source.GetType());
            object target = targetClassInfo.CreateNewObject(targetSession);
            clonedObjects.Add(source, target);

            foreach (XPMemberInfo m in targetClassInfo.PersistentProperties)
            {
                CloneProperty(m, source, target, synchronize);
            }
            foreach (XPMemberInfo m in targetClassInfo.CollectionProperties)
            {
                CloneCollection(m, source, target, synchronize);
            }
            return target;
        }
        private void CloneProperty(XPMemberInfo memberInfo, object source, object target, bool synchronize)
        {
            if (memberInfo is DevExpress.Xpo.Metadata.Helpers.ServiceField || memberInfo.IsKey)
            {
                return;
            }
            object clonedValue = null;
            if (memberInfo.ReferenceType != null)
            {
                object value = memberInfo.GetValue(source);
                if (value != null)
                {
                    clonedValue = CloneValue(value, synchronize, false);
                }
            }
            else {
                clonedValue = memberInfo.GetValue(source);
            }
            memberInfo.SetValue(target, clonedValue);
        }
        private void CloneCollection(XPMemberInfo memberInfo, object source, object target, bool synchronize)
        {
            if (memberInfo.IsAssociation && (memberInfo.IsManyToMany || memberInfo.IsAggregated))
            {
                XPBaseCollection colTarget = (XPBaseCollection)memberInfo.GetValue(target);
                XPBaseCollection colSource = (XPBaseCollection)memberInfo.GetValue(source);
                foreach (IXPSimpleObject obj in colSource)
                {
                    colTarget.BaseAdd(CloneValue(obj, synchronize, !memberInfo.IsManyToMany));
                }
            }
        }
        private object CloneValue(object propertyValue, bool synchronize, bool cloneAlways)
        {
            if (clonedObjects.ContainsKey(propertyValue))
            {
                return clonedObjects[propertyValue];
            }
            object clonedValue = null;
            if (synchronize && !cloneAlways)
            {
                clonedValue = targetSession.GetObjectByKey(targetSession.GetClassInfo(propertyValue), targetSession.GetKeyValue(propertyValue));
            }
            if (clonedValue == null)
            {
                clonedValue = Clone(propertyValue, synchronize);
            }
            return clonedValue;
        }

        public void Dispose()
        {
            if (targetSession != null)
                targetSession.Dispose();
        }
    }
}
