using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using System.Text.RegularExpressions;
using DevExpress.Persistent.Base.General;
using DevExpress.ExpressApp.DC;
using Vinabits_OM_2017.Module.FunctionCriteriaOperator;

namespace Vinabits_OM_2017.Module.BusinessObjects
{
    [DefaultClassOptions]
    [Appearance("Expired", TargetItems = "*", Context = "ListView", Criteria = "[Status] <> 3 And [Status] <> 4 And Today() > [EndOn]", BackColor = "#fedede", FontColor = "Red")]
    [Appearance("ExpiredToday", TargetItems = "*", Context = "ListView", Criteria = "[Status] <> 3 And [Status] <> 4 And Today() = [EndOn]", BackColor = "#fdfbc2", FontColor = "#ff9900")]
    [Appearance("Completed", TargetItems = "*", Context = "ListView", Criteria = "[Status] = 3", FontColor = "Green")]
    [Appearance("WaitingForSomeoneElse", TargetItems = "*", Context = "ListView", Criteria = "[Status] = 2", FontColor = "Yellow", BackColor = "#7c7c7c", FontStyle = System.Drawing.FontStyle.Italic)]
    [Appearance("Deferred", TargetItems = "*", Context = "ListView", Criteria = "[Status] = 4", FontColor = "#666666")]
    [Appearance("NotStarted", TargetItems = "*", Context = "ListView", Criteria = "[Status] = 0", FontColor = "#000000")]
    //[Appearance("PermissionToEdit", TargetItems = "TaskDueDate,EmployeeCreated,TaskAssignedTo,StartDate,Subject,DateCreated,Status", Context = "DetailView", Enabled = false, Criteria = "[EmployeeCreated.Oid] <> CurrentEmployeeOid()")]
    //[Appearance("PermissionToView", TargetItems = "EmployeeCreated,DateCreated,Project", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "[EmployeeCreated.Oid] <> CurrentEmployeeOid()")]
    [Appearance("IsReadLastNote", TargetItems = "*", Context = "ListView", Criteria = "CheckIsRead([LastNote])", FontStyle = System.Drawing.FontStyle.Bold)]
    [Appearance("IsRead", TargetItems = "*", Context = "ListView", Criteria = "CheckIsRead(@This)", FontStyle = System.Drawing.FontStyle.Bold)]
    [XafDefaultProperty("Subject")]
    [XafDisplayName("Công việc")]
    [ImageName("BO_Task")]
    public class TaskExtra : BaseObject, IEvent, IMultiUpload
    {
        public TaskExtra(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
            if (SecuritySystem.CurrentUser != null && Session.IsNewObject(this))
            {
                employeecreated = Session.GetObjectByKey(SecuritySystem.CurrentUser.GetType(), Session.GetKeyValue(SecuritySystem.CurrentUser)) as Employee;
                TaskAssignedTo = employeecreated;
                datecreated = DateTime.Now;
                EndOn = DateTime.Now.AddDays(2);
                StartOn = DateTime.Now;
            }
        }

        /// Add for IEvent Interface
        [XafDisplayName("Cả ngày?")]
        public bool AllDay { get; set; }
        [XafDisplayName("Địa điểm")]
        public string Location { get; set; }
        [XafDisplayName("Nhãn")]
        public int Label { get; set; }
        [XafDisplayName("Tình trạng")]
        public int Status { get; set; }
        [XafDisplayName("Loại")]
        public int Type { get; set; }

        //private string resourceId;
        public string ResourceId
        {
            get; set;
        }
        [XafDisplayName("Id cuộc hẹn")]
        public object AppointmentId { get; }
        //////////////////////////////////////////////

        private Employee taskAssignedTo;
        [DataSourceCriteria("IsActive && Department.Oid = CurrentDepartmentOid()"), XafDisplayName("Chủ trì")]
        public Employee TaskAssignedTo 
        {
            get { return taskAssignedTo; }
            set { SetPropertyValue("TaskAssignedTo", ref taskAssignedTo, value); }
        }    
        private Employee employeecreated;
        private DateTime datecreated;

        [XafDisplayName("Tiêu đề")]
        public string Subject { get; set; }

        private string description;
        [ModelDefault("PropertyEditorType", "Vinabits_OM_2017.Module.Web.PropertyEditors.ASPxHtmlEditors")]
        [XafDisplayName("Nội dung công việc")]
        public string Description 
        {
            get { return description; }
            set { SetPropertyValue("Description", ref description, value); }
        }

        private Int32 percentCompleted;
        [ModelDefault("PropertyEditorType", "Vinabits_OM_2017.Module.Web.PropertyEditors.ASPxTrackBarEditors")]
        [XafDisplayName("Tiến độ hoàn thành (%)")]
        public Int32 PercentCompleted
        {
            get { return percentCompleted; }
            set 
            {
                SetPropertyValue("PercentCompleted", ref percentCompleted, value);
            }
        }

        [XafDisplayName("Tóm tắt nội dung")]
        public string ShortDesc
        {
            get
            {
                string tmp = this.Description;
                if (!string.IsNullOrEmpty(tmp))
                {
                    tmp = Regex.Replace(tmp, "<.*?>", string.Empty);
                    tmp = tmp.SubStringExt(100) + "..";
                }

            	return tmp;
            }
        }

        [XafDisplayName("Thông tin mới nhất")]
        public DateTime LastDateUpdated
        {
            get
            {
                if (lastnote == null)
                    return datecreated;
                else if (lastnote.DateCreated != null && datecreated != null)
                {
                    if (datecreated.CompareTo(lastnote.DateCreated) > 0)
                        return datecreated;
                    else
                        return lastnote.DateCreated;
                }

                return new DateTime();
            }
        }

        [DataSourceCriteria("IsActive")]
        [XafDisplayName("Người tạo")]
        public Employee EmployeeCreated
        {
            get { return employeecreated; }
            set { SetPropertyValue("EmployeeCreated", ref employeecreated, value); }
        }

        [XafDisplayName("Ngày tạo việc")]
        public DateTime DateCreated
        {
            get { return datecreated; }
            set { SetPropertyValue("DateCreated", ref datecreated, value); }
        }

        [XafDisplayName("Ngày bắt đầu"), ImmediatePostData()]
        public DateTime StartOn { get; set; }
        [XafDisplayName("Ngày hết hạn"), ImmediatePostData()]
        public DateTime EndOn { get; set; }

        //Override để gán quyền edit
        [XafDisplayName("Thời gian thực hiện (ngày)")]
        public int TaskDueDate
        {
            get { return (EndOn - StartOn).Days + 1; }
        }

        private NoteExtra firstnote;
        [XafDisplayName("Trao đổi đầu")]
        public NoteExtra FirstNote
        {
            get { return firstnote; }
            set { SetPropertyValue("FirstNote", ref firstnote, value); }
        }

        private NoteExtra lastnote;
        [XafDisplayName("Trao đổi cuối")]
        public NoteExtra LastNote
        {
            get { return lastnote; }
            set { SetPropertyValue("LastNote", ref lastnote, value); }
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            //Send Mail
            if (Session.IsNewObject(this))
            {
                string mailTos = string.Empty;
                if (this.EmployeeCreated != null && this.TaskAssignedTo != null)
                {
                    mailTos = !this.EmployeeCreated.Equals(Global.CurrentEmployee) ? this.EmployeeCreated.Email : string.Empty;
                    mailTos = (!this.TaskAssignedTo.Equals(Global.CurrentEmployee) && mailTos.Contains(this.TaskAssignedTo.Email)) ? (!mailTos.Equals(string.Empty) ? string.Format("{0}, {1}", mailTos, this.TaskAssignedTo.Email) : this.TaskAssignedTo.Email) : mailTos;
                }
                foreach (Employee emp in this.EmployeeReceiveds)
                {
                    if (!emp.Equals(Global.CurrentEmployee))
                        if (!emp.Email.Equals(string.Empty) && !mailTos.Contains(emp.Email))
                            mailTos = mailTos.Equals(string.Empty) ? emp.Email : string.Format("{0}, {1}", mailTos, emp.Email);
                }
                //Global.sendMailBMS(mailTos, string.Format("[BMS NEW] Việc mới - {0}", this.ShortDesc), string.Format("{0}<br />Vinabits BMS system.", this.Description), true);
                Global.sendMailBMSDelegate(mailTos, string.Format("[OM NEW] Việc mới - {0}", this.ShortDesc), string.Format("{0}<br />Vinabits OM system.", this.Description), true);
            }
            //End send mail
        }

        //[ImmediatePostData]
        [Association("TaskExtra-Document", typeof(Document))]
        [XafDisplayName("Văn bản liên quan")]
        public XPCollection<Document> Documents
        {
            get { return GetCollection<Document>("Documents"); }
        }

        [ImmediatePostData]
        [Association("TaskExtra-TaskExtraFiles", typeof(TaskExtraFile))]
        [XafDisplayName("File đính kèm")]
        public XPCollection FileAttachments
        {
            get { return GetCollection("FileAttachments"); }
        }

        [NonPersistent]
        [XafDisplayName("Upload Files")]
        [VisibleInListView(false)]
        [ModelDefault("PropertyEditorType", "Vinabits_OM_2017.Module.Web.PropertyEditors.MultiFileUploadEditor")]
        public string MultiUpload
        {
            get; set;
        }

        [Association("TaskExtra-NoteExtra", typeof(NoteExtra))]
        [XafDisplayName("Trao đổi, thảo luận")]
        public XPCollection NoteExtras
        {
            get { return GetCollection("NoteExtras"); }
        }

        [Association("Employees-TaskExtra"), DataSourceCriteria("IsActive")]
        [XafDisplayName("Nhân sự phối hợp")]
        public XPCollection<Employee> EmployeeReceiveds
        {
            get
            {
                return GetCollection<Employee>("EmployeeReceiveds");
            }
        }
        [NonPersistent]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string ChatBox
        {
            get;set;
        }

        [XafDisplayName("Nhân sự phối hợp")]
        public string ListEmployeeReceived
        {
            get
            {
                string ret = string.Empty;
                foreach (Employee emp in EmployeeReceiveds)
                {
                    ret += string.Format("{0} {1}{2} ", emp.MiddleName, emp.LastName, ",");
                }
                if (ret.Length > 2)
                    ret = ret.Substring(0, ret.Length - 2);
                return ret;
            }
        }

        [Action(Caption = "Make In Progress", ImageName = "State_Task_Completed", TargetObjectsCriteria = "[Status] <> 1 And ([TaskAssignedTo.Oid] = CurrentEmployeeOid() Or [EmployeeCreated.Oid] = CurrentEmployeeOid() Or CurrentEmployeeIsRoot())")]
        public void MakeInProgress()
        {
            this.Status = (int)DevExpress.Persistent.Base.General.TaskStatus.InProgress;
        }

        [Action(Caption = "Wait to completed", ImageName = "BO_Validation", TargetObjectsCriteria = "[Status] <> 2 And ([TaskAssignTo.Oid] = CurrentEmployeeOid() Or CurrentEmployeeIsRoot())")]
        public void MakePreCompleted()
        {
            this.Status = (int)DevExpress.Persistent.Base.General.TaskStatus.WaitingForSomeoneElse;
        }

        [Action(Caption = "Make completed", ImageName = "State_Task_Completed", TargetObjectsCriteria = "[Status] <> 4 And ([EmployeeCreated.Oid] = CurrentEmployeeOid() Or CurrentEmployeeIsRoot())")]
        public void MakeCompleted()
        {
            this.Status = (int)DevExpress.Persistent.Base.General.TaskStatus.Completed;
        }
    }

}
