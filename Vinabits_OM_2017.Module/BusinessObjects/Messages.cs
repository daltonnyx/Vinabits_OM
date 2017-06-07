using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.Collections.Generic;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using System.Drawing;
using DevExpress.ExpressApp.DC;
using Vinabits_OM_2017.Module.FunctionCriteriaOperator;

namespace Vinabits_OM_2017.Module.BusinessObjects
{
    [DefaultClassOptions, XafDefaultProperty("Subject")]
    [Appearance("IsRead", TargetItems = "*", Context = "ListView", Criteria = "CheckIsRead(@This)", FontStyle = System.Drawing.FontStyle.Bold)]
    [XafDisplayName("Trao đổi nhanh")]
    [ImageName("OM_Sms")]
    public class Messages : BaseObject
    {
        private DateTime datecreated;
        private string subject;
        [Size(1000)]
        private string content;
        private Employee empsend;
        private bool isnew = true;

        public Messages(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
            datecreated = DateTime.Now;
            if (SecuritySystem.CurrentUser != null && Session.IsNewObject(this))
            {
                empsend = Session.GetObjectByKey(SecuritySystem.CurrentUser.GetType(), Session.GetKeyValue(SecuritySystem.CurrentUser)) as Employee;
                datecreated = DateTime.Now;
            }
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            //Send Mail
            string mailTos = string.Empty;
            foreach (Employee emp in this.EmployeeReceiveds)
            {
                if (!emp.Equals(Global.CurrentEmployee))
                    if (!emp.Email.Equals(string.Empty) && !mailTos.Contains(emp.Email))
                        mailTos = mailTos.Equals(string.Empty) ? emp.Email : string.Format("{0}, {1}", mailTos, emp.Email);
            }
            //Global.sendMailBMS(mailTos, string.Format("[BMS NEW] Bạn có tin nhắn từ {0}", this.EmployeeSend.FullName), string.Format("{0}<br />Vinabits BMS system.", this.Content), true);
            Global.sendMailBMSDelegate(mailTos, string.Format("[BMS NEW] Bạn có tin nhắn từ {0}", this.EmployeeSend.Fullname), string.Format("{0}<br />Vinabits BMS system.", this.Content), true);
            //End send mail
        }

        public string Folder
        {
            get 
            {
                string folder = "Inbox";
                if (empsend != null && SecuritySystem.CurrentUser != null && (empsend.Oid == ((Employee)SecuritySystem.CurrentUser).Oid))
                    folder = "Sent";

                return folder;
            }
        }

        [XafDisplayName("Ngày tạo")]
        public DateTime DateCreated
        {
            get { return datecreated; }
            set { SetPropertyValue("DateCreated", ref datecreated, value); }
        }

        [XafDisplayName("Tiêu đề")]
        public string Subject
        {
            get { return subject; }
            set { SetPropertyValue("Subject", ref subject, value); }
        }

        [ModelDefault("PropertyEditorType", "Vinabits_OM_2017.Module.Web.PropertyEditors.ASPxHtmlEditors")]
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Nội dung tin nhắn")]
        public string Content
        {
            get { return content; }
            set { SetPropertyValue("Content", ref content, value); }
        }

        [DataSourceCriteria("IsActive")]
        [XafDisplayName("Người gởi")]
        public Employee EmployeeSend
        {
            get { return empsend; }
            set { SetPropertyValue("EmployeeSend", ref empsend, value); }
        }

        [Association("Employees-Message"), DataSourceCriteria("IsActive")]
        [XafDisplayName("Người nhận")]
        public XPCollection<Employee> EmployeeReceiveds
        {
            get
            {
                return GetCollection<Employee>("EmployeeReceiveds");
            }
        }

        public bool IsNew
        {
            get 
            {
                bool returnisnew = isnew;
                if (Session.IsNewObject(this))
                    returnisnew = true;
                else
                {
                    returnisnew = false;
                    isnew = returnisnew;
                }
                return returnisnew; 
            }
            set { SetPropertyValue("IsNew", ref isnew, value); }
        }

        [DevExpress.ExpressApp.DC.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        [ModelDefault("PropertyEditorType", "Vinabits_OM_2017.Module.Web.PropertyEditors.ASPxUploadToFileUploadsEditors")]
        public FileUploads UploadFile
        {
            get { return GetPropertyValue<FileUploads>("UploadFile"); }
            set { SetPropertyValue<FileUploads>("UploadFile", value); }
        }

        private Image uploadicon;
        public Image UploadIcon
        {
            get
            {
                if (UploadFile != null)
                    if (System.IO.File.Exists("Images/Attach.png"))
                        uploadicon = Image.FromFile(string.Format("{0}/{1}",System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath,"Images/Attach.png"));

                return uploadicon;
            }
        }

    }

}
