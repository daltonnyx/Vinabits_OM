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
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using DevExpress.ExpressApp.TreeListEditors.Web;
using Vinabits_OM_2017.Module.BusinessObjects;
using DevExpress.Web.ASPxTreeList;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTreeList.Internal;
using DevExpress.Web.Internal.InternalCheckBox;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DocumentDepartmentReceivedsListViewViewController : ViewController
    {
        private double treeListFixedWidth;
        private ASPxGridView gridEmployees = null;
        private List<string> empOidStrFlag = new List<string>();
        private List<string> leaderOidStrUnSelected = new List<string>();
        private ASPxTreeList treeList;
        ASPxTreeListEditor treeListEditor;
        private bool isFirstLoad = true;
        DetailView dvOwner;
        Document curDoc;

        public DocumentDepartmentReceivedsListViewViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "Document_DepartmentReceiveds_ListView";
            //TargetViewNesting = Nesting.Nested;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            this.actFilterTreeList.Active["disable"] = false;
            

            
        }

        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            Frame.GetController<DevExpress.ExpressApp.Web.SystemModule.ASPxGridListEditorMemberLevelSecurityController>().Active[GetType().Name] = false;
        }

        protected override void OnViewChanging(DevExpress.ExpressApp.View view)
        {
            base.OnViewChanging(view);
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
           
            #region 1. Đánh dấu chọn các phòng có người
            treeListEditor = ((ListView)View).Editor as ASPxTreeListEditor;
            if (View.ObjectSpace.Owner is DetailView)
            {
                dvOwner = View.ObjectSpace.Owner as DetailView;
                //1. Trường hợp Owner là Document
                if (dvOwner.CurrentObject != null && dvOwner.CurrentObject is Document)
                {
                    curDoc = dvOwner.CurrentObject as Document;

                    //2. Tạo các Event handle, khởi tạo giá trị
                    if (treeListEditor != null)
                    {
                        treeList = treeListEditor.TreeList as ASPxTreeList;
                        if (treeList != null)
                        {
                            treeListFixedWidth = treeList.Width.Value - 5;
                            if (treeList.FocusedNode != null && treeList.FocusedNode.Level > 0)
                                treeListFixedWidth = treeList.Width.Value - 100 * treeList.FocusedNode.Level;
                            treeList.Width = new Unit(90, UnitType.Percentage); ;
                            treeList.Height = new Unit(100, UnitType.Percentage);
                            treeList.Settings.ScrollableHeight = 350;
                            treeList.SettingsBehavior.FocusNodeOnExpandButtonClick = false;
                            treeList.SettingsBehavior.ProcessFocusedNodeChangedOnServer = false;
                            treeList.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            treeList.SettingsPager.Visible = false;
                            treeList.SettingsPager.Summary.Visible = false;
                            treeList.SettingsPager.PageSize = 500;
                            treeList.SettingsSelection.Recursive = true;
                            treeList.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            treeList.Load += TreeListDepartmentReceived_Load;
                            treeList.SelectionChanged += TreeListDepartmentReceived_SelectionChanged;
                            
                        }

                    }
                }

                //2. Trường hợp Owner là DocumentEmployees (Phê duyệt/chỉ đạo)
                if (dvOwner.CurrentObject != null && dvOwner.CurrentObject is DocumentEmployees)
                {
                    DocumentEmployees curDocEmp = dvOwner.CurrentObject as DocumentEmployees;
                    if (curDocEmp != null && curDocEmp.LinkDocument != null)
                    {
                        Document curDoc = curDocEmp.LinkDocument as Document;
                        //Tạo các Event handle, khởi tạo giá trị
                        if (treeListEditor != null)
                        {
                            treeList = treeListEditor.TreeList as ASPxTreeList;
                            if (treeList != null)
                            {
                                treeListFixedWidth = treeList.Width.Value - 5;
                                treeList.Height = new Unit(100, UnitType.Percentage);
                                treeList.Settings.ScrollableHeight = 350;
                                treeList.SettingsPager.Visible = false;
                                treeList.SettingsPager.Summary.Visible = false;
                                treeList.SettingsPager.PageSize = 1000;
                                treeList.SettingsBehavior.FocusNodeOnExpandButtonClick = false;
                                treeList.SettingsBehavior.ProcessFocusedNodeChangedOnServer = false;
                                treeList.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                treeList.SettingsSelection.Recursive = true;
                                treeList.Load += TreeListDepartmentReceivedDocEmp_Load;
                                treeList.SelectionChanged += TreeListDepartmentReceived_SelectionChanged;
                                
                            }
                        }
                    }
                }
            }
            #endregion
        }

        #region TreeListDepartmentReceived_Load_Expanded - khi TreeList load: expanded all
        private void TreeListDepartmentReceived_Load_Expanded(object sender, EventArgs e)
        {
            //((ASPxTreeList)sender).ExpandAll();
            //((ASPxTreeList)sender).ExpandToLevel(1);
            treeList.Width = new Unit(treeListFixedWidth, UnitType.Percentage);
            treeList.Load -= TreeListDepartmentReceived_Load_Expanded;
        }
        #endregion

        #region TreeListDepartmentReceivedDocEmp_Load - Load cho trong hop DocumentEmployees
        private void TreeListDepartmentReceivedDocEmp_Load(object sender, EventArgs e)
        {
            treeList.Width = new Unit(treeListFixedWidth, UnitType.Percentage);

            if (isFirstLoad && View.ObjectSpace.Owner is DetailView)
            {
                //1. Tính toán Level của người đăng nhập hiện tại
                ///Đã sử dụng Crit chuẩn

                //2. Selected/chọn những Department/phòng tương ứng
                ((ASPxTreeList)sender).ExpandAll();
                refreshSelectedDepartment();
            }

            treeList.Load -= TreeListDepartmentReceivedDocEmp_Load;
        }

        private void refreshSelectedDepartment()
        {
            if (treeListEditor != null)
            {
                DetailView dvOwner = View.ObjectSpace.Owner as DetailView;
                if (dvOwner != null && dvOwner.CurrentObject != null && dvOwner.CurrentObject is DocumentEmployees)
                {
                    DocumentEmployees curDocEmp = dvOwner.CurrentObject as DocumentEmployees;
                    if (curDocEmp != null && curDocEmp.LinkDocument != null)
                    {
                        Document curDoc = curDocEmp.LinkDocument as Document;
                        if ((curDoc.DepartmentReceivedsSelected.Count > 0))
                        {
                            treeListEditor.SetControlSelectedObjects(curDoc.DepartmentReceivedsSelected.ToList<object>());
                            isFirstLoad = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region TreeListDepartmentReceived_SelectionChanged - Cho cả 2 trường hợp: Document và DocumentEmployee
        private void TreeListDepartmentReceived_SelectionChanged(object sender, EventArgs e)
        {
            //return;
            
            if (View.SelectedObjects != null && View.SelectedObjects.Count > 0)
            {
                DetailView dvo = View.ObjectSpace.Owner as DetailView;
                Document curDoc = null;
                if (dvo != null && dvo.CurrentObject != null && dvo.CurrentObject is Document)
                {
                    curDoc = dvo.CurrentObject as Document;
                    curDoc.EmployeeReceiveds.Clear();
                }
                else if (dvo != null && dvo.CurrentObject != null && dvo.CurrentObject is DocumentEmployees)
                {
                    DocumentEmployees curDocEmp = dvo.CurrentObject as DocumentEmployees;
                    if (curDocEmp != null && curDocEmp.LinkDocument != null)
                    {
                        curDoc = curDocEmp.LinkDocument as Document;
                    }
                }
                
                if (curDoc == null)
                {
                    ///check errror
                    return;
                }

                //a) Xác định danh sách các trưởng phòng ban đang được chọn
                List<Employee> lstLeaderSelected = new List<Employee>();
                foreach(object obj in View.SelectedObjects)
                {
                    if (obj is Department)
                    {
                        Department dep = obj as Department;
                        if(dep.Employees != null && dep.Employees.Count > 0) { 
                            Employee leader = dep.getLeader();
                        
                            if (leader != null)
                            {
                                lstLeaderSelected.Add(leader);
                            }
                        }
                        else if(dep.SubordinatesDepartments.Count > 0)
                        {
                            foreach(Department subdep in dep.SubordinatesDepartments)
                            {
                                Employee leader = subdep.getLeader();
                                if (leader != null)
                                {
                                    lstLeaderSelected.Add(leader);
                                }
                            }
                        }
                    }
                    else if(obj is Employee)
                    {
                        Employee emp = obj as Employee;
                        if(emp != null)
                            lstLeaderSelected.Add(emp);
                    }
                }
                lstLeaderSelected = lstLeaderSelected.Distinct(new EmployeeEqualityComparer()).ToList();
                foreach(Employee employeeToAdd in lstLeaderSelected)
                {
                    curDoc.EmployeeReceiveds.Add(employeeToAdd);
                }
                //View.ObjectSpace.CommitChanges();
                
                ///Không thực hiện 3 dòng sau để cải thiện tốc độ
                //curDoc.Reload();
                //dvo.Refresh();
                //dvo.RefreshDataSource();
                
                //Panel pan = dvo.Control as Panel;
                //ASPxGridView c = pan.FindControl("Document_EmployeeReceiveds_ListView") as ASPxGridView;

            }
                #region Old way
                /*
                IObjectSpace os = Application.CreateObjectSpace();
                #region 1. Duyệt danh sách department select, add vào danh sách Leader nhận nếu chưa có
                if (View.SelectedObjects != null && View.SelectedObjects.Count > 0)
                {
                    DetailView dvo = View.ObjectSpace.Owner as DetailView;
                    Document curDoc = null;
                    if (dvo != null && dvo.CurrentObject != null && dvo.CurrentObject is Document)
                    {
                        curDoc = dvo.CurrentObject as Document;
                    }
                    else if (dvo != null && dvo.CurrentObject != null && dvo.CurrentObject is DocumentEmployees)
                    {
                        DocumentEmployees curDocEmp = dvo.CurrentObject as DocumentEmployees;
                        if (curDocEmp != null && curDocEmp.LinkDocument != null)
                        {
                            curDoc = curDocEmp.LinkDocument as Document;
                        }
                    }

                    if (curDoc == null)
                    {
                        ///check errror
                        return;
                    }

                    /////1.1 Remove những obj ko được chọn (mới bỏ chọn?)
                    List<DocumentEmployees> objectsToDelete = new List<DocumentEmployees>();
                    foreach (Employee obj in curDoc.EmployeeReceiveds)
                    {
                        if (obj != null)
                        {
                            DocumentEmployees docEmpToRemoved = os.FindObject<DocumentEmployees>(
                                CriteriaOperator.And(new BinaryOperator("LinkDocument", os.GetObject(curDoc)),
                                                     new BinaryOperator("LinkEmployee", os.GetObject(obj))));
                            if (docEmpToRemoved != null && docEmpToRemoved.LinkEmployee != null && !View.SelectedObjects.Contains(docEmpToRemoved.LinkEmployee.Department))
                            {
                                objectsToDelete.Add(os.GetObject(docEmpToRemoved));
                            }
                        }
                    }
                    os.Delete(objectsToDelete);
                    os.CommitChanges();

                    //View.ObjectSpace.Delete(curDoc.EmployeeReceiveds);
                    //View.ObjectSpace.CommitChanges();
                    //curDoc.EmployeeReceiveds.Clear();

                    foreach (object objDep in View.SelectedObjects)
                    {
                        if (objDep is Department)
                        {
                            Department dep = objDep as Department;
                            if (dep != null && dep.Employees != null && dep.Employees.Count > 0 && View.ObjectSpace != null && View.ObjectSpace.Owner != null && View.ObjectSpace.Owner is DetailView)
                            {

                                if (dvo != null && dvo.CurrentObject != null && (dvo.CurrentObject is Document || dvo.CurrentObject is DocumentEmployees))
                                {

                                    Employee departmentLeader = dep.getLeader();
                                    if (curDoc != null && curDoc.DocumentEmployees != null) // && curDoc.DocumentEmployees.Count > 0
                                    {
                                        /////1.2 Add
                                        List<CriteriaOperator> andOperator = new List<CriteriaOperator>();
                                        andOperator.Add(new BinaryOperator("LinkDocument", os.GetObject(curDoc)));
                                        andOperator.Add(new BinaryOperator("LinkEmployee", os.GetObject(departmentLeader)));
                                        DocumentEmployees leaderDocEmp = os.FindObject<DocumentEmployees>(new GroupOperator(GroupOperatorType.And, andOperator));

                                        if (leaderDocEmp != null) //Đã có người này nhận văn bản, add vào list Flag nếu chưa có
                                        {
                                            if (!empOidStrFlag.Contains(departmentLeader.Oid.ToString()))
                                            {
                                                empOidStrFlag.Add(departmentLeader.Oid.ToString());
                                            }
                                        }
                                        else //Không tìm thấy? => chưa có, add vào
                                        {
                                            if (curDoc.DocumentEmployees == null)
                                            {
                                                //Vinabits: Error apply employee
                                                return;
                                            }
                                            //IObjectSpace os = Application.CreateObjectSpace();
                                            DocumentEmployees de = os.CreateObject<DocumentEmployees>();
                                            de.LinkDocument = os.FindObject<Document>(new BinaryOperator("Oid", curDoc.Oid));
                                            de.LinkEmployee = os.FindObject<Employee>(new BinaryOperator("Oid", departmentLeader.Oid));
                                            de.Save();
                                            //de.Session.CommitTransaction();
                                            os.CommitChanges();
                                            if (!empOidStrFlag.Contains(departmentLeader.Oid.ToString()))
                                            {
                                                empOidStrFlag.Add(departmentLeader.Oid.ToString());
                                            }
                                            curDoc.Reload();
                                            //Không cần add nữa
                                            //curDoc.DocumentEmployees.Add(de);
                                            //curDoc.Save();
                                            //curDoc.Session.CommitTransaction();
                                        }
                                    }

                                    //dvo.RefreshDataSource();
                                    //dvo.Refresh();
                                }
                            }
                        }// foreach selected
                    }
                }
                #endregion
                */
                #endregion Old way
            }
        #endregion

        #region TreeListDepartmentReceived_Load - Load cho truong hop Document (default)
        private void TreeListDepartmentReceived_Load(object sender, EventArgs e)
        {
            if (isFirstLoad && View.ObjectSpace.Owner is DetailView && treeList != null)
            {
                treeList.Width = new Unit(90, UnitType.Percentage); ;
                treeList.Height = new Unit(100, UnitType.Percentage);
                treeList.Settings.ScrollableHeight = 350;
                treeList.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                treeList.SettingsPager.Visible = false;
                //Expand rồi collapse đảm bảo mọi node được load ở server side hạn chế load ở client side
                treeList.ExpandAll();
                treeList.CollapseAll();
                treeList.SettingsPager.Summary.Visible = false;
                treeList.SettingsPager.PageSize = 500;
                treeList.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                /// Turn off this shit before select node
                

                //2. Selected/chọn những Department/phòng tương ứng
                DetailView dvOwner = View.ObjectSpace.Owner as DetailView;
                if (dvOwner != null && dvOwner.CurrentObject != null && dvOwner.CurrentObject is Document)
                {
                    Document curDoc = dvOwner.CurrentObject as Document;
                    if ((curDoc.EmployeeReceiveds.Count > 0))
                    {
                        ///((ASPxTreeList)sender).ExpandAll(); //New_Object => không expaned all
                        // Check những phòng ban có trong list
                        treeList.SettingsBehavior.ProcessSelectionChangedOnServer = false;
                        foreach (Employee emp in curDoc.EmployeeReceiveds.Where( emp => emp != null))
                        {
                            var node = treeList.FindNodeByKeyValue(emp.Oid.ToString());
                            if (node != null)
                            {
                                node.Selected = true;
                                if(node.ParentNode != null)
                                    node.ParentNode.Expanded = true;
                            }

                        }
                        treeList.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                    }
                }
                isFirstLoad = false;
                //Done turn it on again
                

            }
        }
        #endregion


        protected override void OnViewChanged()
        {
            base.OnViewChanged();
            isFirstLoad = true;
        }
        protected override void OnViewControlsDestroying()
        {
            base.OnViewControlsDestroying();
            
        }


        protected override void OnDeactivated()
        {
            if (treeList != null)
            {

                treeList = null;
              
            }

            base.OnDeactivated();
        }

        private void actFilterTreeList_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {
            string searchText = e.ParameterCurrentValue as String;
            if (!String.IsNullOrEmpty(searchText))
            {
                treeList.ExpandAll();
            }
        }
    }

    public class EmployeeEqualityComparer : IEqualityComparer<Employee>
    {
        public bool Equals(Employee x, Employee y)
        {
            return x.Oid.Equals(y.Oid);
        }

        public int GetHashCode(Employee obj)
        {
            return obj.Oid.GetHashCode();
        }
    }
}
