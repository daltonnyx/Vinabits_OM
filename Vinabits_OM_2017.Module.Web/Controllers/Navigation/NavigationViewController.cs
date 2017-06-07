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
using DevExpress.ExpressApp.Security.Strategy;
using Vinabits_OM_2017.Module.BusinessObjects;

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class NavigationViewController : ViewController
    {
        private ShowNavigationItemController navigationController;
        public NavigationViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewType = ViewType.Any;
            //TargetViewNesting = Nesting.Root;
        }

        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            if (navigationController == null)
            {
                navigationController = Frame.GetController<ShowNavigationItemController>();
                if (navigationController != null)
                {
                    navigationController.ItemsInitialized += NavigationController_ItemsInitialized;
                }
            }
        }

        private void NavigationController_ItemsInitialized(object sender, EventArgs e)
        {
            return;

            Employee currentUser = SecuritySystem.CurrentUser as Employee;
            if (currentUser != null)
            {
                bool hideDocumentInOut = true;
                foreach (EmployeeRole role in currentUser.EmployeeRoles)
                {
                    if (role.Name == "Officer")
                    {
                        hideDocumentInOut = false;
                    }
                }

                //Ép hideDocumentInOut = false ==> luôn luôn hiển thị (Dawaco thay đổi ý định), trước đây chỉ hiện với Officer
                hideDocumentInOut = false;

                if (hideDocumentInOut)
                {
                    HideItemByCaption(navigationController.ShowNavigationItemAction.Items, "Navi_Document_ListView_In");
                    HideItemByCaption(navigationController.ShowNavigationItemAction.Items, "Navi_Document_ListView_Out");
                }
            }
        }

        private void HideItemByCaption(ChoiceActionItemCollection items, string navigationItemId)
        {
            foreach (ChoiceActionItem item in items)
            {
                if (item.Id == navigationItemId)
                {
                    item.Active["InactiveForUsersRole"] = false;
                    return;
                }
                HideItemByCaption(item.Items, navigationItemId);
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            if (navigationController != null)
            {
                navigationController.ItemsInitialized -= NavigationController_ItemsInitialized;
                navigationController = null;
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
