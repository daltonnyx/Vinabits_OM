using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using System.Collections;

namespace Vinabits_OM_2017.Module.BusinessObjects
{
    public enum RadioButtonEnum {[XafDisplayName("Đi")] Value1 = 1, [XafDisplayName("Đến")] Value2 = 2 }; //1: Văn bản đi, 2: Văn bản đến

    [DefaultClassOptions, OptimisticLocking(false)]
    [ImageName("BO_FileAttachment")]
    [DefaultProperty("Title")]
    [FileAttachment("FileAttachment")]
    [XafDisplayName("Văn bản")]
    [Appearance("DocIsRead", TargetItems = "*", Context = "ListView", Criteria = "!DocumentCheckIsRead(@This)", FontStyle = System.Drawing.FontStyle.Bold)]
    [Appearance("DocIsFollow", TargetItems = "*", Context = "Any", Criteria = "DocumentCheckIsFollow(@This)", FontColor = "#FFBF00")]
    public class Document : BaseObject, IMultiUpload
    { 
        public Document(Session session)
            : base(session)
        {

        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUser != null && Session.IsNewObject(this))
            {
                employeeCreated = Session.GetObjectByKey(SecuritySystem.CurrentUser.GetType(), Session.GetKeyValue(SecuritySystem.CurrentUser)) as Employee;
                Doctype = Session.FindObject<DocType>(new BinaryOperator("Code","congvan"));
                dateCreated = DateTime.Now;
                DocDateReceived = DateTime.Now;
                DocDateSent = DateTime.Now;
                DocDate = DateTime.Now.AddDays(-1);
                InOutDocument = RadioButtonEnum.Value2;
                isApproved = false;

            }
        }

        private Employee employeeCreated;
        [XafDisplayName("Người nhập")] //, ImmediatePostData
        public Employee EmployeeCreated
        {
            get { return employeeCreated; }
            set { SetPropertyValue<Employee>("EmployeeCreated", ref employeeCreated, value); }
        }

        private DateTime dateCreated;
        [XafDisplayName("Ngày nhập")] //, ImmediatePostData
        //[Appearance("Document_Datecreated_Hide", Visibility = ViewItemVisibility.Hide, Criteria = "IsNewObject([@This])", Context = "Any")]
        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { SetPropertyValue<DateTime>("DateCreated", ref dateCreated, value); }
        }

        ///Không sử dụng: đã có Detail riêng cho New
        //[Appearance("Document_Datecreated_Hide", AppearanceItemType = "ViewItem", Context = "Any", Visibility = ViewItemVisibility.Hide, Enabled = false, TargetItems = "DateCreated,EmployeeCreated")]
        //protected bool IsNew()
        //{
        //    return Session.IsNewObject(this);
        //}

        //[DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never), NoForeignKey, ImmediatePostData]
        //[XafDisplayName("File đính kèm")]
        //public FileSystemStoreObject FileAttachment
        //{
        //    get { return GetPropertyValue<FileSystemStoreObject>("FileAttachment"); }
        //    set { SetPropertyValue<FileSystemStoreObject>("FileAttachment", value); }
        //}

        [ImmediatePostData]
        [Association("Document-DocumentFiles", typeof(DocumentFile)), DevExpress.Xpo.Aggregated]
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
            get;set;
        }

        private DateTime _docDate;
        [XafDisplayName("Ngày phát hành")]
        public DateTime DocDate
        {
            get { return _docDate; }
            set { SetPropertyValue<DateTime>("DocDate", ref _docDate, value); }
        }

        private DateTime _docDateReceived;
        [XafDisplayName("Ngày nhận văn bản")]
        [Appearance("Document_DateReceived_HideOnIn", AppearanceItemType = "ViewItem", Context = "Any", Visibility = ViewItemVisibility.Hide, 
            Criteria = "[InOutDocument] = ##Enum#Vinabits_OM_2017.Module.BusinessObjects.RadioButtonEnum,Value1#")] //Context = "Any" */
        public DateTime DocDateReceived
        {
            get { return _docDateReceived; }
            set { SetPropertyValue<DateTime>("DocDateReceived", ref _docDateReceived, value); }
        }


        private DateTime _docDateSent;
        [XafDisplayName("Ngày gởi văn bản")]
        [Appearance("Document_DateSent_HideOnOut", AppearanceItemType = "ViewItem", Context = "Any", Visibility = ViewItemVisibility.Hide,
            Criteria = "[InOutDocument] = ##Enum#Vinabits_OM_2017.Module.BusinessObjects.RadioButtonEnum,Value2#")] //*/
        public DateTime DocDateSent
        {
            get { return _docDateSent; }
            set { SetPropertyValue<DateTime>("DocDateSent", ref _docDateSent, value); }
        }

        private string _docSaveId;
        [XafDisplayName("Số VB đến")]
        [Appearance("Document_DocSaveId_HideOnIn", AppearanceItemType = "ViewItem", Context = "Any", Visibility = ViewItemVisibility.Hide,
            Criteria = "[InOutDocument] = ##Enum#Vinabits_OM_2017.Module.BusinessObjects.RadioButtonEnum,Value1#")] //Context = "Any" */
        public string DocSaveId
        {
            get { return _docSaveId; }
            set { SetPropertyValue("DocSaveId", ref _docSaveId, value); }
        }

        private string _docId;
        [XafDisplayName("Số hiệu"), Size(256)]
        public string DocId
        {
            get { return _docId; }
            set { SetPropertyValue("DocId", ref _docId, value); }
        }
        private DocType _docType;
        [XafDisplayName("Loại văn bản")]
        public DocType Doctype
        {
            get { return _docType; }
            set { SetPropertyValue<DocType>("Doctype", ref _docType, value); }
        }

        private RadioButtonEnum inoutDocument;
        [XafDisplayName("Chiều văn bản"), ImmediatePostData()] //, ImmediatePostData()
        public RadioButtonEnum InOutDocument
        {
            get { return inoutDocument; }
            set
            {
                SetPropertyValue("InOutDocument", ref inoutDocument, value);
                OnChanged("InOutDocument");
            }
        }

        private DocumentSignees docSignees;
        [XafDisplayName("Người ký"), Association("Document-DocumentSignees", typeof(DocumentSignees))]
        public DocumentSignees DocSignees
        {
            get { return docSignees; }
            set { SetPropertyValue<DocumentSignees>("DocSignees", ref docSignees, value); }
        }
        private DocumentSigneesOrganization docOrganization;
        [XafDisplayName("Nơi phát hành")]
        public DocumentSigneesOrganization DocOrganization
        {
            get
            {
                
                if(docOrganization == null)
                {
                    if (DocSignees?.Organization != null) { 
                        docOrganization = this.DocSignees.Organization;
                    }
                }
                return docOrganization;
            }
            set
            {
                SetPropertyValue("DocOrganization",ref docOrganization, value);
            }
        }
        

        private string _excerpt;
        [XafDisplayName("Trích yếu"),Size(SizeAttribute.Unlimited)]
        public string Excerpt
        {
            get { return _excerpt; }
            set { SetPropertyValue("Excerpt", ref _excerpt, value); }
        }

        [XafDisplayName("Tiêu đề")]
        public string Title
        {
            get { return string.Format("[{0}] {1}", this.DocId, this.Excerpt); }
        }

        private string _note;
        [XafDisplayName("Ghi chú"),Size(SizeAttribute.Unlimited)]
        public string Note
        {
            get { return _note; }
            set { SetPropertyValue("Note", ref _note, value); }
        }

        private TaskExtra taskextra;
        [VisibleInDetailView(false),VisibleInListView(false),VisibleInLookupListView(false)]
        [Association("TaskExtra-Document", typeof(TaskExtra))]
        [XafDisplayName("Công việc theo văn bản")]
        //[DataSourceCriteria("Oid = @This.TaskExtra.Oid")]
        public TaskExtra TaskExtra
        {
            get { return taskextra; }
            set
            {
                SetPropertyValue("TaskExtra", ref taskextra, value);
            }
        }

        [Association, Browsable(false)] //, ImmediatePostData()
        public IList<DocumentEmployees> DocumentEmployees
        {
            get
            {
                return GetList<DocumentEmployees>("DocumentEmployees");
            }
        }

        //[Association("Document-Employees", typeof(Employee), UseAssociationNameAsIntermediateTableName = true)]
        [ManyToManyAlias("DocumentEmployees", "LinkEmployee")]
        [XafDisplayName("Người nhận"), DataSourceCriteria("IsActive")] //, ImmediatePostData()
        public IList<Employee> EmployeeReceiveds
        {
            get
            {
                return GetList<Employee>("EmployeeReceiveds");
            }
        }

        public IList<DocumentEmployees> getListEmployeeReceiveds
        {
            get
            {
                return this.DocumentEmployees;
            }
        }

        private Department departmentMain;
        [XafDisplayName("Phòng/ban chủ trì"), Association("DepartmentMain-Document", typeof(Department))] //, ImmediatePostData()
        public Department DepartmentMain
        {
            get
            {
                return departmentMain;
            }
            set
            {
                SetPropertyValue("DepartmentMain", ref departmentMain, value);
            }
        }

        private IList<Department> departmentReceiveds;
        [XafDisplayName("Phòng/ban phối hợp"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)] //, ImmediatePostData() NonPersistent, 
        //[Association("DocumentDepartments", UseAssociationNameAsIntermediateTableName = true)]
        public IList<Department> DepartmentReceiveds
        {
            get
            {
                System.Collections.ICollection departmentReceiveds = this.Session.GetObjects(Session.GetClassInfo(typeof(Department)), null, null, 200, false, true);

                return departmentReceiveds.Cast<Department>().ToList();
            }
            set
            {
                SetPropertyValue("DepartmentReceiveds", ref departmentReceiveds, value);
                OnChanged("DepartmentReceiveds");
            }
        }

        public IList<Department> DepartmentReceivedsSelected
        {
            get
            {
                List<Department> lstDepSelected = new List<Department>();
                if(this.DocumentEmployees != null && this.DocumentEmployees.Count > 0)
                {
                    foreach(DocumentEmployees docEmp in this.DocumentEmployees)
                    {
                        if (docEmp.LinkEmployee != null && docEmp.LinkEmployee.Department != null && DepartmentReceiveds.Contains<Department>(docEmp.LinkEmployee.Department))
                        {
                            lstDepSelected.Add(docEmp.LinkEmployee.Department);
                        }
                    }
                }

                return lstDepSelected;
            }
        }

        private bool isApproved;
        [XafDisplayName("Đã duyệt/bút phê?"), ImmediatePostData()] //, ImmediatePostData()
        //[Appearance("DocIsApproved", TargetItems = "*", Context = "Any", Criteria = "'@This.EmployeeReceiveds.Count' < 1", Enabled = false)]
        public bool IsApproveed
        {
            get { return isApproved; }
            set { SetPropertyValue("IsApproveed", ref isApproved, value); }
        }

        private Employee empApproved;
        [XafDisplayName("Người duyệt VB:"), NonPersistent] //, ImmediatePostData()
        //[DataSourceProperty("EmployeeReceiveds")]  // ==> Cho chọn thoải mái hơn
        [DataSourceCriteria("[Position.PositionLevel] >= 50")]
        //[Appearance("DocSelectEmployeeApproved", TargetItems = "*", Context = "Any", Criteria = "!'@This.IsApproveed'", Visibility = ViewItemVisibility.Hide)]
        public Employee EmpApproved
        {
            get {
                empApproved = this.DocumentEmployees.Where(
                                                        de => de.LinkDocument.Oid == this.Oid 
                                                        && de.IsCurrentDirected && de.IsDirected
                                                    ).FirstOrDefault()?.LinkEmployee;
                return empApproved;
            }
            set
            {
                SetPropertyValue("EmpApproved", ref empApproved, value);
            }
        }

        private string contentApproved;
        [ModelDefault("PropertyEditorType", "Vinabits_OM_2017.Module.Web.PropertyEditors.ASPxHtmlEditors")]
        [XafDisplayName("Nội dung chỉ đạo:"), NonPersistent, Size(SizeAttribute.Unlimited)]
        [Appearance("DocContentEmployeeApproved", TargetItems = "*", Context = "Any", Criteria = "!'@This.IsApproveed'", Visibility = ViewItemVisibility.Hide)]
        public string ContentApproved
        {
            get { return contentApproved; }
            set { SetPropertyValue("ContentApproved", ref contentApproved, value); }
        }

        private Document parentDocument;
        [XafDisplayName("Văn bản gốc")]
        public Document ParentDocument
        {
            get { return parentDocument; }
            set { SetPropertyValue("ParentDocument", ref parentDocument, value); }
        }

        public Document CloneDocument(Session targetSession)
        {
            CloneHelper helper = new CloneHelper(targetSession);
            return (Document)helper.Clone(this);
        }

        private string actionInfo;
        public string ActionInfo
        {
            get
            {
                //1. Thông tin chỉ đạo
                actionInfo = "(Chưa có thông tin chỉ đạo)";
                this.DocumentEmployees.OrderBy(x => x.DateRead);
                if (!this.isApproved)
                {
                    int idx = this.DocumentEmployees.FindIndex(x => x.IsCurrentDirected && x.IsDirected);
                    if(idx > -1)
                    {
                        actionInfo = string.Format("<p style=\"color: darkgray;\">VB chưa được duyệt, người chỉ đạo hiện tại: <strong>{0}</strong> - {1}{2}</p>", 
                            (this.DocumentEmployees[idx].LinkEmployee != null ? this.DocumentEmployees[idx].LinkEmployee.Fullname : "N/A"),
                            (this.DocumentEmployees[idx].LinkEmployee != null && this.DocumentEmployees[idx].LinkEmployee.Position != null ? this.DocumentEmployees[idx].LinkEmployee.Position.Title : "N/A"),
                            (this.DocumentEmployees[idx].LinkEmployee != null && this.DocumentEmployees[idx].LinkEmployee.Department != null ? string.Format(" ({0})", this.DocumentEmployees[idx].LinkEmployee.Department.Title) : ""));
                    }
                }
                else {
                    foreach (DocumentEmployees docEmp in this.DocumentEmployees)
                    {
                        if (docEmp != null && docEmp.IsDirected && docEmp.DateDirected != null && !string.IsNullOrEmpty(docEmp.DirectedContent) && docEmp.LinkEmployee != null)
                        {
                            if (actionInfo == "(Chưa có thông tin chỉ đạo)")
                                actionInfo = "<b>Nội dung phê duyệt:</b>";

                            actionInfo += string.Format("<br /><u>{1}</u>:<br /><strong>{0}</strong>: <span style='color: #006DF'>{2}</span><br />", docEmp.LinkEmployee.Fullname, docEmp.DateDirected, docEmp.DirectedContent);
                        }
                    }
                }

                //2. Thông tin cập nhật
                if (EmployeeCreated != null && DateCreated != null)
                {
                    actionInfo = string.Format("<p style='background-color: #F5F5F5'><strong>Ngày nhập:</strong> {0}<br /><strong>Người nhập:</strong> {1}</p>", DateCreated.ToString("dd/MM/yyyy h:m t"), EmployeeCreated.Fullname) + actionInfo;
                    if(this.DepartmentMain != null)
                    {
                        actionInfo = string.Format("<p style='color:darkorange'>Chủ trì: <strong>{0}</strong></p>{1}", this.departmentMain.ManagerDepartment != null ? string.Format("{0} - {1}", this.DepartmentMain.Title, this.DepartmentMain.ManagerDepartment.Title): this.DepartmentMain.Title, actionInfo);
                    }
                    if (this.IsApproveed)
                    {
                        actionInfo = string.Format("<p style='color:green'>Văn bản đã được duyệt!</p>{0}", actionInfo);
                    }
                }

                return actionInfo;
            }
        }

        #region OnSaving - Kiểm tra cập nhật thông tin trước khi lưu
        protected override void OnSaving()
        {
            //1. Có phòng ban chủ trì? => kiểm tra xem có trong danh sách người nhận chưa
            if (this.DepartmentMain != null)
            {
                Employee leader = this.DepartmentMain.getLeader();
                if (leader != null && !this.EmployeeReceiveds.Contains(leader))
                {
                    this.EmployeeReceiveds.Add(leader);
                }
            }

            //2. Có chọn người duyệt? => Kiểm tra xem có trong danh sách người nhận chưa
            //if (this.EmpApproved != null)
            //{
            //    if (!this.EmployeeReceiveds.Contains(EmpApproved))
            //    {
            //        this.EmployeeReceiveds.Add(EmpApproved);
            //    }
            //}

            //3. Thêm người nhận mặc định nếu Đơn vị họ có trong danh sách nhận
            ICollection docDefaultReceiveds = Session.GetObjects(Session.GetClassInfo<DocumentDefaultReceived>(), null, null, 1000, false, false);
            foreach(DocumentDefaultReceived docDefaultReceived in docDefaultReceiveds)
            {
                bool allowAdd = false;
                if(docDefaultReceived != null && docDefaultReceived.EmpDefault != null && docDefaultReceived.EmpDefault.Department != null)
                {
                    if(docDefaultReceived.EmpDefault.Department == this.DepartmentMain ||  this.DepartmentReceivedsSelected.Contains(docDefaultReceived.EmpDefault.Department))
                    {
                        allowAdd = true;
                    }
                    else
                    {
                        foreach(Department dep in this.DepartmentReceivedsSelected)
                        {
                            if(docDefaultReceived.EmpDefault.Department.ListManagerDepartmentOid == null)
                            {
                                docDefaultReceived.EmpDefault.Department.updateListManagerDepartmentOid();
                            }

                            if(dep != null && !string.IsNullOrEmpty(docDefaultReceived.EmpDefault.Department.ListManagerDepartmentOid) && docDefaultReceived.EmpDefault.Department.ListManagerDepartmentOid.Contains(dep.Oid.ToString()))
                            {
                                allowAdd = true;
                            }
                        }
                    }

                    if (allowAdd)
                    {
                        this.EmployeeReceiveds.Add(docDefaultReceived.EmpDefault);
                    }
                }
            }

            //4. Nếu có chọn người chỉ đạo: enable chức năng "Đã phê duyệt"
            if(this.EmpApproved != null)
            {
                this.IsApproveed = true;
            }

            //4. Cập nhật người chỉ đạo và chỉ đạo hiện tại: là người có chức danh cao nhất trong danh sách người nhận
            Employee maxEmp = null;
            int idx = -1;
            int selectedIdx = -1;
            bool isDiretedAlready = false;
            foreach(DocumentEmployees docEmp in this.DocumentEmployees)
            {
                idx++;
                if((maxEmp == null) || 
                    (maxEmp.Position != null && docEmp.LinkEmployee != null && docEmp.LinkEmployee.Position.PositionLevel > maxEmp.Position.PositionLevel))
                {
                    if (docEmp.LinkEmployee != null && docEmp.LinkEmployee.Position != null)
                    {
                        maxEmp = docEmp.LinkEmployee;
                        selectedIdx = idx;
                        if (docEmp.IsDirected && docEmp.IsCurrentDirected)
                            isDiretedAlready = true;
                    }
                }
            }

            //Nếu chưa có ai chỉ đạo thì mới set
            if (selectedIdx > -1 && !isDiretedAlready)
            {
                this.DocumentEmployees[selectedIdx].IsCurrentDirected = true;
                this.DocumentEmployees[selectedIdx].IsDirected = true;
                this.DocumentEmployees[selectedIdx].DirectedOrder = 1;
                isDiretedAlready = true;
            }
            
            base.OnSaving();
            
        }
        #endregion

        protected override void OnSaved()
        {
            base.OnSaved();
            //5. Cập nhật ý kiến chỉ đạo, nếu có nhập đồng thời
            // Let Object saved first
            if (this.isApproved)
            {
                if (this.EmpApproved == null)
                    throw new UserFriendlyException("Chưa chọn người duyệt");
                DocumentEmployees docEmp = null;
                int idxEmp = this.DocumentEmployees.FindIndex(x => x.LinkEmployee == this.EmpApproved && x.LinkDocument == this);
                if (idxEmp > -1 && this.DocumentEmployees[idxEmp] != null)
                {
                    docEmp = Session.GetObjectByKey<DocumentEmployees>(this.DocumentEmployees[idxEmp].Oid);
                }

                if (docEmp == null)
                {
                    docEmp = new DocumentEmployees(this.Session);
                    docEmp.LinkDocument = this;
                    docEmp.LinkEmployee = this.EmpApproved;
                }
                docEmp.DirectedContent = contentApproved;
                docEmp.DateDirected = DateTime.Now;
                docEmp.IsCurrentDirected = false;
                docEmp.IsDirected = true;
            }
            this.Session.CommitTransaction();
        }

    }

    #region DocType - Loại văn bản
    [DefaultClassOptions]
    [ImageName("Action_Printing_PageSetup")]
    [XafDefaultProperty("Title")]
    [XafDisplayName("Loại văn bản")]
    public class DocType : BaseObject
    {
        public DocType(Session session) : base(session) { }
        private string title;
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Tên loại")]
        public string Title
        {
            get { return title; }
            set { SetPropertyValue("Title", ref title, value); }
        }

        private string code;
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Mã loại")]
        public string Code
        {
            get { return code; }
            set { SetPropertyValue("Code", ref code, value); }
        }
    }
    #endregion

    #region DocumentSignees - Người ký văn bản
    [DefaultClassOptions]
    [XafDefaultProperty("Title")]
    [XafDisplayName("Người ký")]
    public class DocumentSignees : BaseObject
    {
        public DocumentSignees(Session session) : base(session) { }

        public string Title
        {
            get
            {
                string title = fullname;
                if (!string.IsNullOrEmpty(this.Position))
                    title = string.Format("{0}, {1}", title, this.Position);
                if (Organization != null && !string.IsNullOrEmpty(this.Organization.Title))
                    title = string.Format("{0} - {1}", title, this.Organization.Title);

                return Global.SubStringExt(title,200);
            }
        }

        private string fullname;
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Họ tên")]
        public string Fullname
        {
            get { return fullname; }
            set { SetPropertyValue("Fullname", ref fullname, value); }
        }

        private string position;
        [XafDisplayName("Chức vụ")]
        public string Position
        {
            get { return position; }
            set { SetPropertyValue("Position", ref position, value); }
        }

        private DocumentSigneesOrganization organization;
        [XafDisplayName("Cơ quan/Tổ chức")]
        public DocumentSigneesOrganization Organization
        {
            get { return organization; }
            set { SetPropertyValue("Organization", ref organization, value); }
        }

        [Association("Document-DocumentSignees", typeof(Document))]
        public XPCollection<Document> Documents
        {
            get { return GetCollection<Document>("Documents"); }
        }
    }
    #endregion

    #region DocumentSigneesOrganization - Cơ quan/tổ chức của người ký văn bản / Nơi phát hành văn bản
    [DefaultClassOptions]
    [XafDefaultProperty("Title")]
    [XafDisplayName("Nơi phát hành")]
    public class DocumentSigneesOrganization : BaseObject
    {
        public DocumentSigneesOrganization(Session session) : base(session) { }
        private string title;
        [RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Tên cơ quan/tổ chức")]
        public string Title
        {
            get { return title; }
            set { SetPropertyValue("Title", ref title, value); }
        }

        private string note;
        [XafDisplayName("Ghi chú")]
        public string Note
        {
            get { return note; }
            set { SetPropertyValue("Note", ref note, value); }
        }
    }
    #endregion

    #region DocumentDirected - Chỉ đạo trên văn bản
    [DefaultClassOptions]
    [XafDefaultProperty("DirectedContent")]
    [XafDisplayName("Chỉ đạo")]
    public class DocumentDirected : BaseObject
    {
        public DocumentDirected(Session session) : base(session)
        {
            if (this.Session.IsNewObject(this))
            {
                int numOfRecords = 0;
                numOfRecords = (int)Session.Evaluate<DocumentDirected>(CriteriaOperator.Parse("Count()"), null);
                this.order = numOfRecords + 1;

                this.dateCreated = DateTime.Now;
            }
        }

        private string content;
        [ModelDefault("PropertyEditorType", "Vinabits_OM_2017.Module.Web.PropertyEditors.ASPxHtmlEditors")]
        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Nội dung chỉ đạo")]
        public string Content
        {
            get { return content; }
            set { SetPropertyValue("Content", ref content, value); }
        }

        private int order;
        [XafDisplayName("Thứ tự chỉ đạo")]
        public int Order
        {
            get { return order; }
            set { SetPropertyValue("Order", ref order, value); }
        }

        private DateTime dateCreated;
        [XafDisplayName("Ngày chỉ đạo")]
        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { SetPropertyValue<DateTime>("DateCreated", ref dateCreated, value); }
        }

    }
    #endregion

    #region DocumentDefaultReceived - Người nhận mặc định
    [DefaultClassOptions]
    [XafDefaultProperty("DirectedContent")]
    [XafDisplayName("DS người nhận VB mặc định")]
    public class DocumentDefaultReceived : BaseObject
    {
        public DocumentDefaultReceived(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUser != null && this.Session.IsNewObject(this))
            {
                this.dateCreated = DateTime.Now;
                this.EmpCreated = Session.GetObjectByKey(SecuritySystem.CurrentUser.GetType(), Session.GetKeyValue(SecuritySystem.CurrentUser)) as Employee;
            }
        }

        private Employee empDefault;
        [XafDisplayName("Nhân viên"), DataSourceCriteria("IsActive")]
        public Employee EmpDefault
        {
            get { return empDefault; }
            set { SetPropertyValue("EmpDefault", ref empDefault, value); }
        }

        private DateTime dateCreated;
        [XafDisplayName("Ngày thêm"), Browsable(false)]
        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { SetPropertyValue<DateTime>("DateCreated", ref dateCreated, value); }
        }

        private Employee empCreated;
        [XafDisplayName("Nhân viên thực hiện"), Browsable(false)]
        public Employee EmpCreated
        {
            get { return empCreated; }
            set { SetPropertyValue("EmpCreated", ref empCreated, value); }
        }
    }
    #endregion
}
