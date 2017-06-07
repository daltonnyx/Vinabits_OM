using System;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.Web;
using System.Collections.Generic;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Security;
using Vinabits_OM_2017.Module.BusinessObjects;

namespace Vinabits_OM_2017.Web {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWebWebApplicationMembersTopicAll.aspx
    public partial class Vinabits_OM_2017AspNetApplication : WebApplication {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule module2;
        private Vinabits_OM_2017.Module.Vinabits_OM_2017Module module3;
        private Vinabits_OM_2017.Module.Web.Vinabits_OM_2017AspNetModule module4;
        private DevExpress.ExpressApp.Security.SecurityModule securityModule1;
        private DevExpress.ExpressApp.AuditTrail.AuditTrailModule auditTrailModule;
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule objectsModule;
        private DevExpress.ExpressApp.Chart.ChartModule chartModule;
        private DevExpress.ExpressApp.Chart.Web.ChartAspNetModule chartAspNetModule;
        private DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule conditionalAppearanceModule;
        private DevExpress.ExpressApp.Dashboards.DashboardsModule dashboardsModule;
        private DevExpress.ExpressApp.Dashboards.Web.DashboardsAspNetModule dashboardsAspNetModule;
        private DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule htmlPropertyEditorAspNetModule;
        private DevExpress.ExpressApp.Notifications.NotificationsModule notificationsModule;
        private DevExpress.ExpressApp.Notifications.Web.NotificationsAspNetModule notificationsAspNetModule;
        private DevExpress.ExpressApp.PivotChart.PivotChartModuleBase pivotChartModuleBase;
        private DevExpress.ExpressApp.PivotChart.Web.PivotChartAspNetModule pivotChartAspNetModule;
        private DevExpress.ExpressApp.PivotGrid.PivotGridModule pivotGridModule;
        private DevExpress.ExpressApp.PivotGrid.Web.PivotGridAspNetModule pivotGridAspNetModule;
        private DevExpress.ExpressApp.ReportsV2.ReportsModuleV2 reportsModuleV2;
        private DevExpress.ExpressApp.ReportsV2.Web.ReportsAspNetModuleV2 reportsAspNetModuleV2;
        private DevExpress.ExpressApp.Scheduler.SchedulerModuleBase schedulerModuleBase;
        private DevExpress.ExpressApp.Scheduler.Web.SchedulerAspNetModule schedulerAspNetModule;
        private DevExpress.ExpressApp.Validation.ValidationModule validationModule;
        private DevExpress.ExpressApp.Security.SecurityStrategyComplex securityStrategyComplex1;
        private DevExpress.ExpressApp.Security.AuthenticationStandard authenticationStandard1;
        private DevExpress.ExpressApp.TreeListEditors.Web.TreeListEditorsAspNetModule treeListEditorsAspNetModule1;
        private DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase treeListEditorsModuleBase1;
        private DevExpress.ExpressApp.Validation.Web.ValidationAspNetModule validationAspNetModule;

        private bool ChangingPassword;

        public Vinabits_OM_2017AspNetApplication() {
            InitializeComponent();
            #region Vinabits: tham khảo đoạn code fixed lỗi đổi pass khi ở chế độ Middle Tier - tạm thời ngắt (lưu ý các Event và Class đi kèm bên dưới
            //this.securityModule1.CustomChangePasswordOnLogon += SecurityModule1_CustomChangePasswordOnLogon;
            #endregion

            LinkNewObjectToParentImmediately = false;
            DevExpress.ExpressApp.Web.Editors.ASPx.ASPxGridListEditor.AllowFilterControlHierarchy = true;
            DevExpress.ExpressApp.Web.Editors.ASPx.ASPxGridListEditor.MaxFilterControlHierarchyDepth = 3;
            DevExpress.ExpressApp.Web.Editors.ASPx.ASPxCriteriaPropertyEditor.AllowFilterControlHierarchyDefault = true;
            DevExpress.ExpressApp.Web.Editors.ASPx.ASPxCriteriaPropertyEditor.MaxHierarchyDepthDefault = 3;
        }

        #region Vinabits: tham khảo đoạn code fixed lỗi đổi pass khi ở chế độ Middle Tier
        private void SecurityModule1_CustomChangePasswordOnLogon(object sender, DevExpress.ExpressApp.Security.CustomChangePasswordOnLogonEventArgs e)
        {
            if (ChangingPassword) return;
            string newPass = e.LogonPasswordParameters.NewPassword;

            try
            {
                ChangingPassword = true;
                var method = sender.GetType().GetMethod("ChangePasswordOnLogon", System.Reflection.BindingFlags.NonPublic); //|| System.Reflection.BindingFlags.Instance
                method.Invoke(sender, new object[] { e.LogonPasswordParameters });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ChangingPassword = false;
            }

            var logonParameters = (AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters; // => AuthenticationStandardLogonParameters hoặc Employee (đang nghiên cứu)
            logonParameters.Password = newPass;

            e.Handled = true;

        }
        #endregion

        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            args.ObjectSpaceProvider = new XPObjectSpaceProvider(GetDataStoreProvider(args.ConnectionString, args.Connection), true); //Mặc định: true (hiện lỗi cross => recommend nhưng tính sau)
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }
        private IXpoDataStoreProvider GetDataStoreProvider(string connectionString, System.Data.IDbConnection connection) {
            System.Web.HttpApplicationState application = (System.Web.HttpContext.Current != null) ? System.Web.HttpContext.Current.Application : null;
            IXpoDataStoreProvider dataStoreProvider = null;
            if(application != null && application["DataStoreProvider"] != null) {
                dataStoreProvider = application["DataStoreProvider"] as IXpoDataStoreProvider;
            }
            else {
                if(!String.IsNullOrEmpty(connectionString)) {
                    connectionString = DevExpress.Xpo.XpoDefault.GetConnectionPoolString(connectionString);
                    dataStoreProvider = new ConnectionStringDataStoreProvider(connectionString, true);
                }
                else if(connection != null) {
                    dataStoreProvider = new ConnectionDataStoreProvider(connection);
                }
                if(application != null) {
                    application["DataStoreProvider"] = dataStoreProvider;
                }
            }
			return dataStoreProvider;
        }
        private void Vinabits_OM_2017AspNetApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
            //if (System.Diagnostics.Debugger.IsAttached)
            //{
                e.Updater.Update();
                e.Handled = true;
            //}
            //else {
            //    string message = "The application cannot connect to the specified database.";

            //    if (e.CompatibilityError != null && e.CompatibilityError.Exception != null)
            //    {
            //        message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
            //    }
            //    throw new InvalidOperationException(message);
            //}
		}
        private void InitializeComponent() {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule();
            this.auditTrailModule = new DevExpress.ExpressApp.AuditTrail.AuditTrailModule();
            this.objectsModule = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
            this.chartModule = new DevExpress.ExpressApp.Chart.ChartModule();
            this.chartAspNetModule = new DevExpress.ExpressApp.Chart.Web.ChartAspNetModule();
            this.conditionalAppearanceModule = new DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule();
            this.dashboardsModule = new DevExpress.ExpressApp.Dashboards.DashboardsModule();
            this.dashboardsAspNetModule = new DevExpress.ExpressApp.Dashboards.Web.DashboardsAspNetModule();
            this.htmlPropertyEditorAspNetModule = new DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule();
            this.notificationsModule = new DevExpress.ExpressApp.Notifications.NotificationsModule();
            this.notificationsAspNetModule = new DevExpress.ExpressApp.Notifications.Web.NotificationsAspNetModule();
            this.pivotChartModuleBase = new DevExpress.ExpressApp.PivotChart.PivotChartModuleBase();
            this.pivotChartAspNetModule = new DevExpress.ExpressApp.PivotChart.Web.PivotChartAspNetModule();
            this.pivotGridModule = new DevExpress.ExpressApp.PivotGrid.PivotGridModule();
            this.pivotGridAspNetModule = new DevExpress.ExpressApp.PivotGrid.Web.PivotGridAspNetModule();
            this.reportsModuleV2 = new DevExpress.ExpressApp.ReportsV2.ReportsModuleV2();
            this.reportsAspNetModuleV2 = new DevExpress.ExpressApp.ReportsV2.Web.ReportsAspNetModuleV2();
            this.schedulerModuleBase = new DevExpress.ExpressApp.Scheduler.SchedulerModuleBase();
            this.schedulerAspNetModule = new DevExpress.ExpressApp.Scheduler.Web.SchedulerAspNetModule();
            this.validationModule = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.validationAspNetModule = new DevExpress.ExpressApp.Validation.Web.ValidationAspNetModule();
            this.module3 = new Vinabits_OM_2017.Module.Vinabits_OM_2017Module();
            this.module4 = new Vinabits_OM_2017.Module.Web.Vinabits_OM_2017AspNetModule();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.securityStrategyComplex1 = new DevExpress.ExpressApp.Security.SecurityStrategyComplex();
            this.authenticationStandard1 = new DevExpress.ExpressApp.Security.AuthenticationStandard();
            this.treeListEditorsAspNetModule1 = new DevExpress.ExpressApp.TreeListEditors.Web.TreeListEditorsAspNetModule();
            this.treeListEditorsModuleBase1 = new DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // auditTrailModule
            // 
            this.auditTrailModule.AuditDataItemPersistentType = typeof(DevExpress.Persistent.BaseImpl.AuditDataItemPersistent);
            // 
            // dashboardsModule
            // 
            this.dashboardsModule.DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.DashboardData);
            // 
            // notificationsModule
            // 
            this.notificationsModule.CanAccessPostponedItems = false;
            this.notificationsModule.NotificationsRefreshInterval = System.TimeSpan.Parse("00:05:00");
            this.notificationsModule.NotificationsStartDelay = System.TimeSpan.Parse("00:00:05");
            this.notificationsModule.ShowDismissAllAction = false;
            this.notificationsModule.ShowNotificationsWindow = true;
            this.notificationsModule.ShowRefreshAction = false;
            // 
            // pivotChartModuleBase
            // 
            this.pivotChartModuleBase.DataAccessMode = DevExpress.ExpressApp.CollectionSourceDataAccessMode.Client;
            this.pivotChartModuleBase.ShowAdditionalNavigation = false;
            // 
            // reportsModuleV2
            // 
            this.reportsModuleV2.EnableInplaceReports = true;
            this.reportsModuleV2.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportDataV2);
            this.reportsModuleV2.ReportStoreMode = DevExpress.ExpressApp.ReportsV2.ReportStoreModes.XML;
            // 
            // reportsAspNetModuleV2
            // 
            this.reportsAspNetModuleV2.ReportViewerType = DevExpress.ExpressApp.ReportsV2.Web.ReportViewerTypes.HTML5;
            // 
            // validationModule
            // 
            this.validationModule.AllowValidationDetailsAccess = true;
            this.validationModule.IgnoreWarningAndInformationRules = false;
            // 
            // securityStrategyComplex1
            // 
            this.securityStrategyComplex1.Authentication = this.authenticationStandard1;
            this.securityStrategyComplex1.RoleType = typeof(Vinabits_OM_2017.Module.BusinessObjects.EmployeeRole);
            this.securityStrategyComplex1.UserType = typeof(Vinabits_OM_2017.Module.BusinessObjects.Employee);
            // 
            // authenticationStandard1
            // 
            this.authenticationStandard1.LogonParametersType = typeof(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters);
            // 
            // Vinabits_OM_2017AspNetApplication
            // 
            this.ApplicationName = "Vinabits OM 2017";
            this.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.auditTrailModule);
            this.Modules.Add(this.objectsModule);
            this.Modules.Add(this.chartModule);
            this.Modules.Add(this.conditionalAppearanceModule);
            this.Modules.Add(this.dashboardsModule);
            this.Modules.Add(this.notificationsModule);
            this.Modules.Add(this.pivotChartModuleBase);
            this.Modules.Add(this.pivotGridModule);
            this.Modules.Add(this.reportsModuleV2);
            this.Modules.Add(this.schedulerModuleBase);
            this.Modules.Add(this.validationModule);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.chartAspNetModule);
            this.Modules.Add(this.dashboardsAspNetModule);
            this.Modules.Add(this.htmlPropertyEditorAspNetModule);
            this.Modules.Add(this.notificationsAspNetModule);
            this.Modules.Add(this.pivotChartAspNetModule);
            this.Modules.Add(this.pivotGridAspNetModule);
            this.Modules.Add(this.reportsAspNetModuleV2);
            this.Modules.Add(this.schedulerAspNetModule);
            this.Modules.Add(this.validationAspNetModule);
            this.Modules.Add(this.module4);
            this.Modules.Add(this.securityModule1);
            this.Modules.Add(this.treeListEditorsModuleBase1);
            this.Modules.Add(this.treeListEditorsAspNetModule1);
            this.Security = this.securityStrategyComplex1;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.Vinabits_OM_2017AspNetApplication_DatabaseVersionMismatch);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }

    #region Vinabits: tham khảo đoạn code fixed lỗi đổi pass khi ở chế độ Middle Tier - tam ngắt
    /*
    public class CustomChangePasswordController : ChangePasswordController
    {
        protected override void ChangePassword(ChangePasswordParameters parameters)
        {
            var newPassword = parameters.NewPassword;
            base.ChangePassword(parameters);
            var logonParameters = (AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters; //AuthenticationStandardLogonParameters hoặc Employee
            logonParameters.Password = newPassword; // Debug thử, nên dùng .SetPassword()
        }
    }
    */
    #endregion

    #region Vinabits: tham khảo đoạn code fixed lỗi đổi pass khi ở chế độ Middle Tier: cái này ngăn không cho đổi pass của chính chủ nhân đang login để tránh BUG - tạm ngắt
    /*
    public class CustomResetPasswordController : ResetPasswordController
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            var logonParameters = (Employee)SecuritySystem.LogonParameters;
            this.Actions["ResetPassword"].TargetObjectsCriteria = "[Oid] <> CurrentUserId()";

            if(this.View != null && this.View.CurrentObject != null && logonParameters.UserName.ToLower() == ((Employee)this.View.CurrentObject).UserName.ToLower())
            {
                this.Actions["ResetPassword"].Active["CurrentUser"] = false;
            }
            else
            {
                this.Actions["ResetPassword"].Active["CurrentUser"] = true;
            }
        }
    }
    */
    #endregion
}
