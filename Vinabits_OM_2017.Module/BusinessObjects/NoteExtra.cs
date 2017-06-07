using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using System.Text.RegularExpressions;
using System.Net.Mail;
using DevExpress.ExpressApp.DC;

namespace Vinabits_OM_2017.Module.BusinessObjects
{
    [DefaultClassOptions]
    [Appearance("PermissionToEdit", TargetItems = "*", Context = "DetailView", Enabled = false, Criteria = "[EmployeeCreated.Oid] <> CurrentEmployeeOid()")]
    [XafDefaultProperty("ShortText")]
    [XafDisplayName("Thảo luận công việc")]
    [ImageName("BO_Note")]
    public class NoteExtra : Note
    {
        private TaskExtra tasknote;
        private Employee empcreated;

        public NoteExtra(Session session)
            : base(session)
        {
        }

        //protected override void OnLoading()
        //{
        //    base.OnLoading();
        //}
        [ModelDefault("PropertyEditorType", "Vinabits_OM_2017.Module.Web.PropertyEditors.ASPxHtmlEditors")]
        [XafDisplayName("Nội dung")]
        public new string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        [XafDisplayName("Vắn tắt")]
        public string ShortText
        {
            get {
                string stext = string.Empty;
                if (string.IsNullOrEmpty(this.Text))
                    return stext;
                stext = Regex.Replace(this.Text, "<.*?>", string.Empty);
                stext = Global.SubStringExt(stext, 70);
                stext = stext.Length < this.Text.Length ? stext + ".." : stext;
                return stext; 
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            //Kiểm tra và lưu First+Last Note
            if (tasknote != null)
            {
                if (tasknote.FirstNote == null)
                    tasknote.FirstNote = this;
                tasknote.LastNote = this;
            }
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            if (tasknote != null && Session.IsNewObject(this))
            {
                //Send Mail
                string mailTos = string.Empty;
                if (tasknote.EmployeeCreated != null && tasknote.TaskAssignedTo != null)
                {
                    mailTos = !tasknote.EmployeeCreated.Equals(Global.CurrentEmployee) ? tasknote.EmployeeCreated.Email : string.Empty;
                    mailTos = (!tasknote.TaskAssignedTo.Equals(Global.CurrentEmployee) && mailTos.Contains(tasknote.TaskAssignedTo.Email)) ? (!mailTos.Equals(string.Empty) ? string.Format("{0}, {1}", mailTos, tasknote.TaskAssignedTo.Email) : tasknote.TaskAssignedTo.Email) : mailTos;
                }
                foreach (Employee emp in tasknote.EmployeeReceiveds)
                {
                    if (!emp.Equals(Global.CurrentEmployee))
                        if (!emp.Email.Equals(string.Empty) && !mailTos.Contains(emp.Email))
                            mailTos = mailTos.Equals(string.Empty) ? emp.Email : string.Format("{0}, {1}", mailTos, emp.Email);
                }
                //Global.sendMailBMS(mailTos, string.Format("[BMS NEW] Trao đổi việc - {0}", this.ShortText), string.Format("{0}<br />Vinabits BMS system.",this.Text), true);
                //Global.sendMailBMSDelegate(mailTos, string.Format("[BMS NEW] Trao đổi việc - {0}", this.ShortText), string.Format("{0}<br />Vinabits BMS system.", this.Text), true);
                //End send mail
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
            if (SecuritySystem.CurrentUser != null && Session.IsNewObject(this))
            {
                empcreated = Session.GetObjectByKey(SecuritySystem.CurrentUser.GetType(), Session.GetKeyValue(SecuritySystem.CurrentUser)) as Employee;
                datecreated = DateTime.Now;
            }
            else if (!Session.IsNewObject(this) && (datecreated == null || datecreated.Equals(new DateTime())))
                datecreated = DateTime.Now;
        }

        [DevExpress.ExpressApp.DC.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        [ModelDefault("PropertyEditorType", "Vinabits_OM_2017.Module.Web.PropertyEditors.ASPxUploadToFileUploadsEditors")]
        [XafDisplayName("File đính kèm")]
        public FileUploads AttachFile
        {
            get { return GetPropertyValue<FileUploads>("AttachFile"); }
            set { SetPropertyValue<FileUploads>("AttachFile", value); }
        }

        [Association("TaskExtra-NoteExtra", typeof(TaskExtra))]
        [XafDisplayName("Công việc đang thảo luận")]
        public TaskExtra TaskNote
        {
            get { return tasknote; }
            set
            {
                SetPropertyValue("TaskNote", ref tasknote, value);
            }
        }

        [DataSourceCriteria("IsActive")]
        [XafDisplayName("Người trao đổi")]
        public Employee EmployeeCreated
        {
            get { return empcreated; }
            set { SetPropertyValue("EmployeeCreated", ref empcreated, value); }
        }

        private DateTime datecreated;
        [XafDisplayName("Ngày tạo")]
        public DateTime DateCreated
        {
            get { return datecreated; }
            set { SetPropertyValue("DateCreated", ref datecreated, value); }
        }

        [NonPersistent, Size(SizeAttribute.Unlimited)]
        public string ChatBox
        {
            get
            {
                string box = "<div class=\"Box\">";
                box += "<h5>" + this.EmployeeCreated.Fullname + "</h5>";
                box += "<span class=\"date\">" + this.DateCreated.ToString("dd/MM/yyyy HH:mm") + "</span>";
                box += "<div>" + this.Text + "</div>";
                if (this.AttachFile != null) {
                    box += "<hr/>";
                    box += "<div>File đính kèm: <a target=\"_blank\" href=\""+this.AttachFile.FileUrl+"\">"+this.AttachFile.FileName+"</a></div>";
                }
                box += "</div>";
                return box;
            }
        }

    }

}
