using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Vinabits_OM_2017.Module.BusinessObjects;
using System.Collections.Generic;
using DevExpress.ExpressApp.Dashboards;

namespace Vinabits_OM_2017.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            DashboardsModule.AddDashboardData<DashboardData>(
                ObjectSpace, "MainDashboard", Properties.Resources.MainDashBoard);
            //Cập nhật Danh sách Department cấp trên: chỉ mở và cho chạy 1 lần
            /*
            List<Department> allDepartment = ObjectSpace.GetObjects<Department>().ToList();
            foreach(Department dep in allDepartment)
            {
                if(dep != null)
                {
                    dep.updateListManagerDepartmentOid();
                    dep.Save();
                }
            }
            ObjectSpace.CommitChanges();
            //*/

            #region EmployeeRole - Khoi tao Role/User cho EmployeeRole
            ///Phải có tài khoản Default (Default)
            EmployeeRole defaultEmployeeRole = ObjectSpace.FindObject<EmployeeRole>(
                new BinaryOperator("Name", "default"));
            if (defaultEmployeeRole == null)
            {
                defaultEmployeeRole = ObjectSpace.CreateObject<EmployeeRole>();
                defaultEmployeeRole.Name = "Default";
                defaultEmployeeRole.IsAdministrative = false;
                defaultEmployeeRole.Save();
            }

            ///Phải có tài khoản Thủ quỷ (Cashier)
            EmployeeRole cashierEmployeeRole = ObjectSpace.FindObject<EmployeeRole>(
                new BinaryOperator("Name", "Cashier"));
            if (cashierEmployeeRole == null)
            {
                cashierEmployeeRole = ObjectSpace.CreateObject<EmployeeRole>();
                cashierEmployeeRole.Name = "Cashier";
                cashierEmployeeRole.IsAdministrative = false;
                cashierEmployeeRole.Save();
            }
            ///Phải có tài khoản Giám đốc (Director)
            EmployeeRole directorEmployeeRole = ObjectSpace.FindObject<EmployeeRole>(
                new BinaryOperator("Name", "Director"));
            if (directorEmployeeRole == null)
            {
                directorEmployeeRole = ObjectSpace.CreateObject<EmployeeRole>();
                directorEmployeeRole.Name = "Director";
                directorEmployeeRole.IsAdministrative = false;
                directorEmployeeRole.Save();
            }

            ///Phải có tài khoản Văn thư (Officer)
            EmployeeRole officerEmployeeRole = ObjectSpace.FindObject<EmployeeRole>(
                new BinaryOperator("Name", "Officer"));
            if (officerEmployeeRole == null)
            {
                officerEmployeeRole = ObjectSpace.CreateObject<EmployeeRole>();
                officerEmployeeRole.Name = "Officer";
                officerEmployeeRole.IsAdministrative = false;
                officerEmployeeRole.Save();
            }
            

            EmployeeRole TaskerRole = ObjectSpace.FindObject<EmployeeRole>(
                new BinaryOperator("Name", "Tasker"));
            if (TaskerRole == null)
            {
                TaskerRole = ObjectSpace.CreateObject<EmployeeRole>();
                TaskerRole.Name = "Tasker";
                TaskerRole.IsAdministrative = false;
                TaskerRole.Save();
            }

            ///Administrator
            EmployeeRole adminEmployeeRole = ObjectSpace.FindObject<EmployeeRole>(
                new BinaryOperator("Name", SecurityStrategy.AdministratorRoleName));
            if (adminEmployeeRole == null)
            {
                adminEmployeeRole = ObjectSpace.CreateObject<EmployeeRole>();
                adminEmployeeRole.Name = SecurityStrategy.AdministratorRoleName;
                adminEmployeeRole.IsAdministrative = true;
                //SecuritySystemTypePermissionObject permissionObject = ObjectSpace.FindObject<SecuritySystemTypePermissionObject>(null);
                //permissionObject.MemberPermissions.Add(new SecuritySystemMemberPermissionsObject)  
                //adminEmployeeRole.TypePermissions.Add(permissionObject);
                adminEmployeeRole.Save();
            }

            Employee adminEmployee = ObjectSpace.FindObject<Employee>(
                new BinaryOperator("UserName", "admin"));
            if (adminEmployee == null)
            {
                adminEmployee = ObjectSpace.CreateObject<Employee>();
                adminEmployee.UserName = "admin";
                adminEmployee.SetPassword("admin");
                adminEmployee.EmployeeRoles.Add(adminEmployeeRole);
            }
            #endregion

            #region DocType - Loại văn bản
            DocType doctype = ObjectSpace.FindObject<DocType>(new BinaryOperator("Code", "congvan"));
            if (doctype == null)
            {
                doctype = ObjectSpace.CreateObject<DocType>();
                doctype.Code = "congvan";
                doctype.Title = "Công văn";
                doctype.Save();
            }

            doctype = ObjectSpace.FindObject<DocType>(new BinaryOperator("Code", "quyetdinh"));
            if (doctype == null)
            {
                doctype = ObjectSpace.CreateObject<DocType>();
                doctype.Code = "quyetdinh";
                doctype.Title = "Quyết định";
                doctype.Save();
            }

            doctype = ObjectSpace.FindObject<DocType>(new BinaryOperator("Code", "totrinh"));
            if (doctype == null)
            {
                doctype = ObjectSpace.CreateObject<DocType>();
                doctype.Code = "totrinh";
                doctype.Title = "Tờ trình";
                doctype.Save();
            }

            doctype = ObjectSpace.FindObject<DocType>(new BinaryOperator("Code", "thongbao"));
            if (doctype == null)
            {
                doctype = ObjectSpace.CreateObject<DocType>();
                doctype.Code = "thongbao";
                doctype.Title = "Thông báo";
                doctype.Save();
            }

            doctype = ObjectSpace.FindObject<DocType>(new BinaryOperator("Code", "baocao"));
            if (doctype == null)
            {
                doctype = ObjectSpace.CreateObject<DocType>();
                doctype.Code = "baocao";
                doctype.Title = "Báo cáo";
                doctype.Save();
            }
            #endregion

            #region Position - Vị trí/chức vụ
            Position position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Chủ tịch hội đồng quản trị"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Chủ tịch hội đồng quản trị";
                position.PositionLevel = 900;
                position.Save();
            }

            position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Ủy viên hội đồng quản trị"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Ủy viên hội đồng quản trị";
                position.PositionLevel = 800;
                position.Save();
            }

            position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Tổng giám đốc"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Tổng giám đốc";
                position.PositionLevel = 500;
                position.Save();
            }

            position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Phó tổng giám đốc"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Chủ tịch hội đồng quản trị văn";
                position.PositionLevel = 490;
                position.Save();
            }

            position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Giám đốc"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Giám đốc";
                position.PositionLevel = 400;
                position.Save();
            }

            position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Phó giám đốc"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Phó giám đốc";
                position.PositionLevel = 380;
                position.Save();
            }

            position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Trưởng ban"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Trưởng ban";
                position.PositionLevel = 300;
                position.Save();
            }

            position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Phó ban"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Phó ban";
                position.PositionLevel = 280;
                position.Save();
            }

            position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Trưởng phòng"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Trưởng phòng";
                position.PositionLevel = 200;
                position.Save();
            }

            position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Phó phòng"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Phó phòng";
                position.PositionLevel = 180;
                position.Save();
            }

            position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Chuyên viên"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Chuyên viên";
                position.PositionLevel = 80;
                position.Save();
            }

            position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Nhân viên"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Nhân viên";
                position.PositionLevel = 78;
                position.Save();
            }

            position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Kế toán viên"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Kế toán viên";
                position.PositionLevel = 78;
                position.Save();
            }

            position = ObjectSpace.FindObject<Position>(new BinaryOperator("Title", "Công nhân"));
            if (doctype == null)
            {
                position = ObjectSpace.CreateObject<Position>();
                position.Title = "Công nhân";
                position.PositionLevel = 70;
                position.Save();
            }
            #endregion

            #region Configuration
            Global.Config = Configuration.GetInstance(ObjectSpace);
            #endregion

            ObjectSpace.CommitChanges(); //This line persists created object(s).

        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
        private PermissionPolicyRole CreateDefaultRole() {
            PermissionPolicyRole defaultRole = ObjectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Default"));
            if(defaultRole == null) {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

				defaultRole.AddObjectPermission<PermissionPolicyUser>(SecurityOperations.Read, "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
				defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
				defaultRole.AddMemberPermission<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", "[Oid] = CurrentUserId()", SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
				defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);                
            }
            return defaultRole;
        }
    }
}
