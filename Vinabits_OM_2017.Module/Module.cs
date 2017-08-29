using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Data.Filtering;
using Vinabits_OM_2017.Module.FunctionCriteriaOperator;
using DevExpress.ExpressApp.ReportsV2;
using Vinabits_OM_2017.Module.Reports;
using Vinabits_OM_2017.Module.BusinessObjects;

namespace Vinabits_OM_2017.Module {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class Vinabits_OM_2017Module : ModuleBase {
        public Vinabits_OM_2017Module() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;

            CurrentEmployeeOidOperator.Register();
            CurrentEmployeeUserNameOperator.Register();
            CurrentEmployeeOperator.Register();
            CurrentEmployeeIsRootOperator.Register();
            CurrentDepartmentOidOperator.Register();
            if (CriteriaOperator.GetCustomFunction(CheckIsReadOperator.OperatorName) == null)
            {
                CriteriaOperator.RegisterCustomFunction(new CheckIsReadOperator());
            }

            if (CriteriaOperator.GetCustomFunction(DocumentCheckIsReadOperator.OperatorName) == null)
            {
                CriteriaOperator.RegisterCustomFunction(new DocumentCheckIsReadOperator());
            }

            if (CriteriaOperator.GetCustomFunction(DocumentCheckIsFollowOperator.OperatorName) == null)
            {
                CriteriaOperator.RegisterCustomFunction(new DocumentCheckIsFollowOperator());
            }
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            PredefinedReportsUpdater predefinedReportsUpdater =
               new PredefinedReportsUpdater(Application, objectSpace, versionFromDB);
            predefinedReportsUpdater.AddPredefinedReport<SchedulerReport>("SchedulerReport", typeof(EventExtra));
            predefinedReportsUpdater.AddPredefinedReport<Reports.VanBanReport>("VanBanReport", typeof(BusinessObjects.VanBanReport));
            return new ModuleUpdater[] { updater, predefinedReportsUpdater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
        }
		protected override IEnumerable<Type> GetDeclaredExportedTypes() {
			List<Type> result = new List<Type>(base.GetDeclaredExportedTypes());
			result.AddRange(new Type[] { typeof(PermissionPolicyUser), typeof(PermissionPolicyRole) });
			return result;
		}
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            var ti = typesInfo.FindTypeInfo(typeof(DevExpress.Persistent.BaseImpl.BaseObject));
            var bi = (DevExpress.ExpressApp.DC.BaseInfo)ti.FindMember("Oid");
            var attr = bi.FindAttribute<System.ComponentModel.BrowsableAttribute>();
            bi.RemoveAttribute(attr);
            bi.AddAttribute(new DevExpress.Persistent.Base.VisibleInDetailViewAttribute(false));
            bi.AddAttribute(new DevExpress.Persistent.Base.VisibleInListViewAttribute(false));
            bi.AddAttribute(new DevExpress.Persistent.Base.VisibleInLookupListViewAttribute(false));
            typesInfo.RefreshInfo(ti);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
    }
}
