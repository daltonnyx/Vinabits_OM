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
using Vinabits_OM_2017.Module.FunctionCriteriaOperator;

namespace Vinabits_OM_2017.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class FilterDataListViewController : ViewController
    {
        public FilterDataListViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            Employee curEmp = SecuritySystem.CurrentUser as Employee;
            if (SecuritySystem.CurrentUser != null)
            {
                curEmp = (Employee)ObjectSpace.GetObject(SecuritySystem.CurrentUser);
            }
            else if (SecuritySystem.CurrentUserId != null)
            {
                curEmp = ObjectSpace.FindObject<Employee>(new BinaryOperator("Oid", SecuritySystem.CurrentUserId));
            }
            ///Hạn chế số lượng bản ghi trả về: 
            /// - Chỉ người được phép có thể xem được văn bản
            /// - Dùng tốt hơn ở đây, để lọc ngay từ server

            //1. Messages - Filter tin nhắn 
            if ((View is ListView) && View.ObjectTypeInfo != null && (View.ObjectTypeInfo.Type == typeof(Messages)))
            {
                if (SecuritySystem.CurrentUser != null || SecuritySystem.CurrentUserId != null )
                    ((ListView)View).CollectionSource.Criteria["FilterMessage"] = CriteriaOperator.Parse("[EmployeeReceiveds][[Oid] = CurrentEmployeeOid()] Or [EmployeeSend.Oid] = CurrentEmployeeOid()");
            }

            //2. Document - Filter Văn bản
            if ((View is ListView) && View.ObjectTypeInfo != null && (View.ObjectTypeInfo.Type == typeof(Document)) && View.IsRoot)
            {
                if ((SecuritySystem.CurrentUser != null || SecuritySystem.CurrentUserId != null) && SecuritySystem.CurrentUserName != "admin" )
                {
                    ((ListView)View).CollectionSource.Criteria["FilterDocument"] =
                        CriteriaOperator.Parse("([DocumentEmployees][[LinkEmployee.Oid] = CurrentEmployeeOid() And [IsDirected]]) Or ([IsApproveed] And [DocumentEmployees][[LinkEmployee.Oid] = CurrentEmployeeOid()]) Or [EmployeeCreated.Oid] = CurrentEmployeeOid()");
                    //CriteriaOperator crit = new ContainsOperator("DocumentEmployees", CriteriaOperator.And(
                    //                                                            new BinaryOperator(new OperandProperty("LinkEmployee"), new OperandValue(curEmp), BinaryOperatorType.Equal),
                    //                                                            new BinaryOperator(new OperandProperty("IsDirected"), new OperandValue(true), BinaryOperatorType.Equal)));
                    //((ListView)View).CollectionSource.Criteria["FilterDocument"] = crit;
                }
            }

            //3. TaskExtra - Filter Công việc
            if ((View is ListView) && View.ObjectTypeInfo != null && (View.ObjectTypeInfo.Type == typeof(TaskExtra)))
            {
                if ((SecuritySystem.CurrentUser != null || SecuritySystem.CurrentUserId != null) && SecuritySystem.CurrentUserName != "admin")
                    ((ListView)View).CollectionSource.Criteria["FilterTaskExtra"] = CriteriaOperator.Parse("[EmployeeReceiveds][[Oid] = CurrentEmployeeOid()] Or [EmployeeCreated.Oid] = CurrentEmployeeOid() Or [TaskAssignedTo.Oid] = CurrentEmployeeOid() Or CurrentEmployeeIsRoot()");
            }

            //4. Nestlist DepartmentReceived - Filter danh sách phòng ban theo quyền hạn hiện tại
            //   a) Đối với Văn bản: Tổng Giám đốc, Giám đốc, Officer => ALL; Các Trưởng phòng cấp dưới => GĐ và các bộ phận thuộc Đơn vị, Nhân viên BT => trong phòng
            //   b) Đối với Công việc (giao việc): theo quyền...
            if ((View is ListView) && (View.Id == "Document_DepartmentReceiveds_ListView") && (View.ObjectTypeInfo.Type == typeof(Department)))
            {
                ListView lv = View as ListView;
                //lv.CollectionSource.BeginUpdateCriteria();
                lv.CollectionSource.Criteria["FilterDepartmentTree"] = CriteriaOperator.Parse("1 = 2");
                if (SecuritySystem.CurrentUser != null || SecuritySystem.CurrentUserId != null)
                {
                    if (curEmp != null && curEmp.Position != null && curEmp.Department != null)
                    {
                        //a) Giám đốc hoặc Văn thư, được gởi Văn bản toàn hệ thống
                        if (curEmp.EmployeeRoles.FindIndex(x => x.Name == "Officer" || x.Name == "Director") > -1)
                        {
                            //lv.CollectionSource.Criteria["FilterDepartmentTree"] = CriteriaOperator.And(CriteriaOperator.Parse("1 = 1"), new BinaryOperator("HRType", "Employee", BinaryOperatorType.NotEqual));
                            lv.CollectionSource.Criteria["FilterDepartmentTree"] = CriteriaOperator.Parse("1 = 1");
                        }
                        else
                        {
                            //b) Trưởng phòng/bộ phận: được gởi trong đơn vị => Department của TP, các Department cấp dưới (nhận Department này làm Parent) và các Department cùng cấp Cha
                            Employee curLeader = curEmp.Department.getLeader();
                            if (curLeader != null && curEmp.Position != null && curLeader.Position != null && curEmp.Position.PositionLevel >= curLeader.Position.PositionLevel)
                            {
                                Department depTmp = curEmp.Department;
                                if (curEmp.Department.ManagerDepartment != null)
                                    depTmp = curEmp.Department.ManagerDepartment;
                                //BinaryOperator capDuoi = new BinaryOperator("ListManagerDepartmentOid", curEmp.Department.Oid.ToString(), BinaryOperatorType.Like);
                                FunctionOperator subDepartments = new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("ListManagerDepartmentOid"), depTmp.Oid.ToString());
                                lv.CollectionSource.Criteria["FilterDepartmentTree"] = subDepartments;
                            }
                            //c) Nhân viên bình thường trong phòng => gởi trong phòng thôi
                            else {
                                FunctionOperator subDepartments = new FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("ListManagerDepartmentOid"), curEmp.Department.Oid.ToString());
                                lv.CollectionSource.Criteria["FilterDepartmentTree"] = subDepartments;
                            }
                        }
                    }
                }
                lv.CollectionSource.Sorting.Add(new DevExpress.Xpo.SortProperty("Position.PositionLevel", DevExpress.Xpo.DB.SortingDirection.Descending));
                lv.RefreshDataSource();
                //lv.CollectionSource.EndUpdateCriteria();
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
