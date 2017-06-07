using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.DC;

namespace Vinabits_OM_2017.Module
{
    [DefaultClassOptions]
    [XafDisplayName("Cấu hình")]
    public class Configuration : BaseObject
    {
        internal Configuration(Session session) : base(session) { }
        public static Configuration GetInstance(IObjectSpace objectSpace)
        {
            return GetInstance(((XPObjectSpace)objectSpace).Session);
        }

        public static Configuration GetInstance(Session sess)
        {
            Configuration result = sess.FindObject<Configuration>(null);
            if (result == null)
            {
                result = new Configuration(sess);
                result.CompanyName = "Vinabits Co.,LTD";
                result.Description = "Công ty thiết kế Web.";
                result.NumDayBeforeExpired = 30;
                result.USDRate = 21500;
                result.UploadPath = "uploads/";
                result.Save();
            }
            return result;
        }

        private bool checkDBVersion;
        public bool CheckDBVersion
        {
            get { return checkDBVersion; }
            set { SetPropertyValue("CheckDBVersion", ref checkDBVersion, value); }
        }

        private int numdaybeforeexpired;
        public int NumDayBeforeExpired
        {
            get { return numdaybeforeexpired; }
            set { SetPropertyValue("NumDayBeforeExpired", ref numdaybeforeexpired, value); }
        }

        [Size(128)]
        private string companyname;
        public string CompanyName {
            get { return companyname; }
            set {
                SetPropertyValue("CompanyName", ref companyname, value);
            }
        }

        [Size(256)]
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                SetPropertyValue("Description", ref description, value);
            }
        }

        [Size(256)]
        private string uploadpath;
        public string UploadPath
        {
            get { return uploadpath; }
            set
            {
                SetPropertyValue("UploadPath", ref uploadpath, value);
            }
        }

        private double usdrate;
        public double USDRate
        {
            get { return usdrate; }
            set { SetPropertyValue("USDRate", ref usdrate, value); }
        }

        private string _PreCodePaymentsRequest = "COD";
        public string PreCodePaymentsRequest
        {
            get { return _PreCodePaymentsRequest; }
            set { SetPropertyValue("PreCodePaymentsRequest", ref _PreCodePaymentsRequest, value); }
        }

        private int _NumCodePaymentsRequest = 3;
        public int NumCodePaymentsRequest
        {
            get { return _NumCodePaymentsRequest; }
            set { SetPropertyValue("NumCodePaymentsRequest", ref _NumCodePaymentsRequest, value); }
        }

        private string _PreCodeReceipts = "COD";
        public string PreCodeReceipts
        {
            get { return _PreCodeReceipts; }
            set { SetPropertyValue("PreCodeReceipts", ref _PreCodeReceipts, value); }
        }

        private int _NumCodeReceipts = 3;
        public int NumCodeReceipts
        {
            get { return _NumCodeReceipts; }
            set { SetPropertyValue("NumCodeReceipts", ref _NumCodeReceipts, value); }
        }

        private string _SmtpFrom;
        public string SmtpFrom
        {
            get { return _SmtpFrom; }
            set { SetPropertyValue("SmtpFrom", ref _SmtpFrom, value); }
        }

        private string _SmtpHost;
        [ToolTip("ex: mail.example.com")]
        public string SmtpHost
        {
            get { return _SmtpHost; }
            set { SetPropertyValue("SmtpHost", ref _SmtpHost, value); }
        }

        private int _SmtpPort;
        [ToolTip("ex: 25")]
        public int SmtpPort
        {
            get { return _SmtpPort; }
            set { SetPropertyValue("SmtpPort", ref _SmtpPort, value); }
        }

        private string _SmtpUser;
        [ToolTip("ex: abc@example.com")]
        public string SmtpUser
        {
            get { return _SmtpUser; }
            set { SetPropertyValue("SmtpUser", ref _SmtpUser, value); }
        }

        private string _SmtpPass;
        public string SmtpPass
        {
            get { return _SmtpPass; }
            set { SetPropertyValue("SmtpPass", ref _SmtpPass, value); }
        }

        protected override  void OnDeleting()
        {
            throw new UserFriendlyException("This object cannot be deleted.");
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
    }

}
