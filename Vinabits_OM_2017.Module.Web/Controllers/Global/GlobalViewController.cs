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
using DevExpress.ExpressApp.Web.Templates.ActionContainers;
using DevExpress.Web;
using DevExpress.ExpressApp.Security;

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class GlobalViewController : ChangePasswordController
    {
        private NavigationActionContainer navigationActionContainer;
        private ASPxNavBar navBarControl;
        public GlobalViewController()
        {
            InitializeComponent();
            TargetViewType = ViewType.Any;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            ///Để dành thực hiện các thao tác Global
            ////....
            IsChangePasswordSupported = false;
            if (SecuritySystem.Instance is ISupportChangePasswordOption)
            {
                IsChangePasswordSupported = ((ISupportChangePasswordOption)SecuritySystem.Instance).IsSupportChangePassword;
                //IsChangePasswordSupported = true;
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
