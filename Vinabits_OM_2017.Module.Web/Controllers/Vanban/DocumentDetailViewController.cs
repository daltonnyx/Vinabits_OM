using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Vinabits_OM_2017.Module.BusinessObjects;
using DevExpress.ExpressApp.TreeListEditors.Web;
using DevExpress.Xpo;
using DevExpress.Web.ASPxTreeList;

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DocumentDetailViewController : ViewController
    {
        private ChoiceActionItem setEmployeeDirected;
        private bool firstControlCreated = true;
        //private List<string> empOidStrFlag = new List<string>();
        //private List<string> leaderOidStrUnSelected = new List<string>();
        public DocumentDetailViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Document);
            TargetViewType = ViewType.DetailView;
       
            actChuyenXuly.Items.Clear();
            setEmployeeDirected = new ChoiceActionItem("Chuyển xử lý cho ..", null);
            actChuyenXuly.Items.Add(setEmployeeDirected);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            if(setEmployeeDirected != null && !ObjectSpace.IsModified) //Lọc chuyển XL văn bản khi ở chế độ xem
                FillItemWithLeaderEmployeeValues(setEmployeeDirected);

            Employee emp = (Employee)SecuritySystem.CurrentUser;
            Document curDoc = View.CurrentObject as Document;
            if (emp != null && curDoc != null)
            {
                //1. Cập nhật ngày đọc văn bản nếu ở chế độ View
                if (!ObjectSpace.IsModified)
                {
                    CriteriaOperator crit = CriteriaOperator.And(new BinaryOperator("LinkEmployee.Oid", emp.Oid), new BinaryOperator("LinkDocument", curDoc));
                    DocumentEmployees docEmp = curDoc.Session.FindObject<DocumentEmployees>(crit);
                    if (docEmp != null && (docEmp.DateRead == null || docEmp.DateRead == DateTime.MinValue))
                    {
                        docEmp.DateRead = DateTime.Now;
                        docEmp.Save();
                        View.ObjectSpace.CommitChanges();
                    }
                }
            }
        }

        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            //Frame.GetController<ModificationsController>().SaveAction.Executed += DocumentSaveAction_Executed;
        }

        private void DocumentSaveAction_Executed(object sender, ActionBaseEventArgs e)
        {
            #region SaveMessage - cảnh báo message sau khi lưu
            //Application.ShowViewStrategy.ShowViewInPopupWindow(Application.CreateDashboardView(Application.CreateObjectSpace(), "ActionSaveOkDashboard", true), null, null, "Đồng ý", null);
            #endregion
        }

        #region FillItemWithLeaderEmployeeValues - tạo danh sách người để chuyển Xử lý văn bản!
        private void FillItemWithLeaderEmployeeValues(ChoiceActionItem parentItem)
        {
            if (ObjectSpace != null && SecuritySystem.CurrentUser != null)
            {
                Employee curEmp = SecuritySystem.CurrentUser as Employee;
                IList<Employee> lstEmployee = ObjectSpace.GetObjects<Employee>(
                    CriteriaOperator.And(new NotOperator(new NullOperator("Position")), new BinaryOperator("Position.PositionLevel", 50, BinaryOperatorType.GreaterOrEqual),
                                         new BinaryOperator("Position.PositionLevel", curEmp.Position.PositionLevel, BinaryOperatorType.LessOrEqual)));
                //IList<Employee> lstEmployee = ObjectSpace.GetObjects<Employee>();
                if (lstEmployee != null)
                {
                    lstEmployee = lstEmployee.OrderBy(x => x.UserName).ToList(); //Nếu kỹ: phải hạn chế, chỉ cho phép chuyển cho cấp dưới PositionLevel <= current PositionLevel
                    
                    parentItem.Items.Clear();
                    foreach (Employee emp in lstEmployee)
                    {
                        if (emp != null && curEmp != null && emp.Oid != curEmp.Oid)
                        {
                            ChoiceActionItem item = new ChoiceActionItem(emp.Title, emp);
                            //item.ImageName = ImageLoader.Instance.GetEnumValueImageName(current);
                            parentItem.Items.Add(item);
                        }
                    }
                }
            }
        }
        #endregion
        

        protected override void OnViewControllersActivated()
        {
            base.OnViewControllersActivated();
        }
        
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.

            firstControlCreated = false;
        }
        protected override void OnDeactivated()
        {
            //Frame.GetController<ModificationsController>().SaveAction.Executed -= DocumentSaveAction_Executed;
            if(((DetailView)View).ViewEditMode == ViewEditMode.Edit)
            {
                Document vb = View.CurrentObject as Document;
                if(View.Id == "Document_DetailView_New")
                {
                    if(vb.Excerpt == null || vb.Excerpt.Trim() == string.Empty)
                    {
                        try {
                            View.ObjectSpace.Delete(vb);
                            View.ObjectSpace.CommitChanges();
                        }
                        catch
                        {

                        }
                    }
                }
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        IObjectSpace os;
        #region actChiDaoVanBan_Execute
        private void actChiDaoVanBan_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            os = Application.CreateObjectSpace();
            Document currentDoc = e.CurrentObject as Document; // View.CurrentObject as Document;
            Employee curUser = SecuritySystem.CurrentUser as Employee;
            //CriteriaOperator crit;
            DocumentEmployees objDEmp = null;
            foreach (DocumentEmployees docEmp in currentDoc.DocumentEmployees)
            {
                if(docEmp != null && docEmp.LinkEmployee != null && curUser != null && docEmp.LinkEmployee.Oid == curUser.Oid && docEmp.IsDirected && docEmp.IsCurrentDirected)
                {
                    objDEmp = os.FindObject<DocumentEmployees>(new BinaryOperator("Oid", docEmp.Oid));
                }
            }

            if (objDEmp != null)
            {
                //DocumentEmployees objDE = os.FindObject<DocumentEmployees>(new BinaryOperator()); //os.CreateObject<DocumentEmployees>();
                objDEmp.LinkDocument = os.GetObject<Document>(currentDoc);
                DetailView dv = Application.CreateDetailView(os, "DocumentEmployees_DetailView_Approved", true, objDEmp);
                dv.ViewEditMode = ViewEditMode.Edit;
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                e.ShowViewParameters.NewWindowTarget = NewWindowTarget.Default;
                e.ShowViewParameters.CreatedView = dv;
                e.ShowViewParameters.CreateAllControllers = true;
                DialogController defaultDialogController = Application.CreateController<DialogController>();
                defaultDialogController.AcceptAction.Execute += AcceptAction_Execute;
                defaultDialogController.AcceptAction.Executed += AcceptAction_Executed;
                e.ShowViewParameters.Controllers.Add(defaultDialogController); 
               // os.SetModified(objDEmp);
                
            }
            else
            {
                //Hiển thị lỗi quyền truy cập
                Application.ShowViewStrategy.ShowViewInPopupWindow(Application.CreateDashboardView(Application.CreateObjectSpace(), "AccessDeniedMessage", true), null, null,"OK", "");
            }
        }

        private void AcceptAction_Executed(object sender, ActionBaseEventArgs e)
        {
            View.ObjectSpace.Refresh();
        }

        //Bắt hàm này để tránh load event saving nhiều lần
        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //Cập nhật ngày chỉ đạo
            if (e.CurrentObject is DocumentEmployees)
            {
                DocumentEmployees docEmp = e.CurrentObject as DocumentEmployees;
                docEmp.DateDirected = DateTime.Now;
                docEmp.LinkDocument.IsApproveed = true;
                docEmp.LinkDocument.EmpApproved = docEmp.LinkEmployee;
                docEmp.LinkDocument.ContentApproved = docEmp.DirectedContent;
                //docEmp.Session.CommitTransaction();
                os.CommitChanges();
                os.Refresh();
            }
        }
        #endregion

        #region actForwardDocument_Execute
        private void actForwardDocument_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            os = Application.CreateObjectSpace();
            Document currentDoc = View.CurrentObject as Document;
            Document newDoc = os.CreateObject<Document>(); //.GetObject<Document>(currentDoc);
            //newDoc = currentDoc.CloneDocument(newDoc.Session);
            newDoc.DocId = currentDoc.DocId;
            newDoc.IsApproveed = false;
            newDoc.Note = @"<u>VB chuyển tiếp:</u><br />" + currentDoc.DocId;
            newDoc.Excerpt = string.Format("[CT] {0}", currentDoc.Excerpt);
            newDoc.DocDate = currentDoc.DocDate;
            //newDoc.DocSignees = os.GetObject<DocumentSignees>(currentDoc.DocSignees); 
            newDoc.DocOrganization = os.GetObject<DocumentSigneesOrganization>(currentDoc.DocOrganization);
            newDoc.EmployeeReceiveds.Clear();
            newDoc.DocumentEmployees.Clear();
            newDoc.ParentDocument = os.GetObject<Document>(currentDoc);
            //newDoc.Save();
            //Lấy file đính kèm
            foreach (DocumentFile curDocFile in currentDoc.FileAttachments)
            {
                if (curDocFile != null)
                {
                    DocumentFile newDocFile = os.CreateObject<DocumentFile>();
                    newDocFile.Document = os.GetObject(newDoc);
                    newDocFile.DocFile = os.GetObject(curDocFile.DocFile);
                    newDocFile.Save();
                }
            }
            ///////
            //os.CommitChanges();

            Employee curUser = SecuritySystem.CurrentUser as Employee;

            // Gán người duyệt nếu user chuyển tiếp có vị trí lớn hơn 50
            if(curUser.Position.PositionLevel >= 50)
            {
                newDoc.IsApproveed = true;
                newDoc.EmpApproved = os.GetObject<Employee>(curUser);
                DocumentEmployees approver = os.CreateObject<DocumentEmployees>();
                approver.LinkDocument = newDoc;
                approver.LinkEmployee = newDoc.EmpApproved;
                newDoc.DocumentEmployees.Add(approver);
            }
            //CriteriaOperator crit;
            DocumentEmployees objDEmp = os.CreateObject<DocumentEmployees>();
            //DocumentEmployees objDE = os.FindObject<DocumentEmployees>(new BinaryOperator()); //os.CreateObject<DocumentEmployees>();
            //DetailView dv = Application.CreateDetailView(os, "DocumentEmployees_DetailView_Forward", true, objDEmp);
            DetailView dvDocument = Application.CreateDetailView(os, "Document_DetailView_New", true, newDoc);

            //ShowViewParameters svp = new ShowViewParameters(dv);
            //svp.TargetWindow = DevExpress.ExpressApp.TargetWindow.NewModalWindow;
            //svp.CreatedView = dv;
            //svp.NewWindowTarget = NewWindowTarget.Default;
            dvDocument.ViewEditMode = ViewEditMode.Edit;
            e.ShowViewParameters.CreatedView = dvDocument;
            os.SetModified(newDoc);
            //Application.ShowViewStrategy.ShowView(e.ShowViewParameters, new ShowViewSource(Frame, actForwardDocument)); // null, null));
        }
        #endregion

        #region actGiaoViec_Execute
        private void actGiaoViec_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //Hiển thị lỗi quyền truy cập
            // bắt điều kiện quyền ở đây
            if(true)
            {
                os = Application.CreateObjectSpace();
                TaskExtra congViecMoi = os.CreateObject<TaskExtra>();
                congViecMoi.EmployeeCreated = os.FindObject<Employee>(new BinaryOperator("Oid", SecuritySystem.CurrentUserId));
                congViecMoi.Documents.Add(os.GetObject<Document>((Document)View.CurrentObject));
                congViecMoi.Description = ((Document)View.CurrentObject).Excerpt;
                congViecMoi.Subject = ((Document)View.CurrentObject).Excerpt;
                //congViecMoi.Save();
                DetailView dv = Application.CreateDetailView(os, "TaskExtra_DetailView_New", true, congViecMoi);
                dv.ViewEditMode = ViewEditMode.Edit;
                e.ShowViewParameters.CreatedView = dv;
                e.ShowViewParameters.CreateAllControllers = true;
                e.ShowViewParameters.TargetWindow = TargetWindow.Current;
            }
            else { 
                Application.ShowViewStrategy.ShowViewInPopupWindow(Application.CreateDashboardView(Application.CreateObjectSpace(), "AccessDeniedMessage", true), null, null, "OK", "");
            }
        }
        #endregion

        #region actTheodoi_Execute
        private void actTheodoi_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            os = Application.CreateObjectSpace();
            Document currentDoc = e.CurrentObject as Document; // View.CurrentObject as Document;
            Employee curUser = SecuritySystem.CurrentUser as Employee;
            //CriteriaOperator crit;
            DocumentEmployees objDEmp = null;
            foreach (DocumentEmployees docEmp in currentDoc.DocumentEmployees)
            {
                if (docEmp != null && docEmp.LinkEmployee != null && curUser != null && docEmp.LinkEmployee.Oid == curUser.Oid && docEmp.IsDirected && docEmp.IsCurrentDirected)
                {
                    objDEmp = os.FindObject<DocumentEmployees>(new BinaryOperator("Oid", docEmp.Oid));
                }
            }

            if (objDEmp != null)
            {
                objDEmp.IsFollow = true;
                objDEmp.Save();
                os.CommitChanges();
                //Hiển thị OK
                Application.ShowViewStrategy.ShowViewInPopupWindow(Application.CreateDashboardView(Application.CreateObjectSpace(), "ActionOkDashboard", true), null, null, "OK", "");
            }
            else
            {
                //Hiển thị lỗi quyền truy cập
                Application.ShowViewStrategy.ShowViewInPopupWindow(Application.CreateDashboardView(Application.CreateObjectSpace(), "AccessDeniedMessage", true), null, null, "OK", "");
            }
        }
        #endregion

        #region actChuyenXuly_Execute - Chuyển PGĐ xử lý
        private void actChuyenXuly_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if(e.SelectedChoiceActionItem != null)
            {
                Employee newEmp = e.SelectedChoiceActionItem.Data as Employee;
                Employee currentEmp = (Employee)SecuritySystem.CurrentUser;
                if (newEmp != null && currentEmp != null)
                {
                    Document curDoc = View.CurrentObject as Document;
                    if(curDoc != null)
                    {
                        //1. Bỏ chế độ isCurrentDirected
                        int idx = curDoc.DocumentEmployees.FindIndex(x => x.LinkEmployee.Oid == currentEmp.Oid && x.IsCurrentDirected);
                        if(idx > -1)
                        {
                            curDoc.DocumentEmployees[idx].IsCurrentDirected = false;
                            curDoc.DocumentEmployees[idx].DirectedContent = string.Format("Đã chuyển \"{0}\" xử lý!", newEmp.Title);
                            curDoc.DocumentEmployees[idx].DateDirected = DateTime.Now;
                            int directOrder = curDoc.DocumentEmployees[idx].DirectedOrder;

                            //2. Cập nhật người duyệt mới: add vao neu chua co
                            idx = curDoc.DocumentEmployees.FindIndex(x => x.LinkEmployee == newEmp);
                            if(idx > -1) //Cập nhật
                            {
                                curDoc.DocumentEmployees[idx].IsCurrentDirected = true;
                                curDoc.DocumentEmployees[idx].IsDirected = true;
                            }
                            else //Thêm người vào danh sách
                            {
                                os = Application.CreateObjectSpace();
                                DocumentEmployees docEmp = os.CreateObject<DocumentEmployees>();
                                docEmp.LinkDocument = os.GetObject(curDoc);
                                docEmp.LinkEmployee = os.GetObject(newEmp);
                                docEmp.IsDirected = true;
                                docEmp.IsCurrentDirected = true;
                                docEmp.DirectedOrder = directOrder + 1;
                                docEmp.Save();
                                os.CommitChanges();

                                //Hiển thị OK
                                Application.ShowViewStrategy.ShowViewInPopupWindow(Application.CreateDashboardView(Application.CreateObjectSpace(), "ActionOkDashboard", true), null, null, "OK", "");
                            }
                            curDoc.Save();
                            ObjectSpace.CommitChanges();
                        }
                    }
                }
            }
        }
        #endregion
    }
}
