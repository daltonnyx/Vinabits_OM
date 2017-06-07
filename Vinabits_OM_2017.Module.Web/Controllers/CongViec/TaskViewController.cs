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

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TaskViewController : ViewController
    {
        public TaskViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            //1. Navi check
            ShowNavigationItemController showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
            showNavigationItemController.CustomShowNavigationItem += showNavigationItemController_CustomShowNavigationItem;
            
        }
        
        void showNavigationItemController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "NewTaskExtraNav")
            {
                SingleChoiceAction newObjectAction = Frame.GetController<NewObjectViewController>().NewObjectAction;
                newObjectAction.DoExecute(new ChoiceActionItem() { Data = typeof(TaskExtra) });
                e.Handled = true;
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
            ShowNavigationItemController showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
            if (showNavigationItemController != null)
                showNavigationItemController.CustomShowNavigationItem -= showNavigationItemController_CustomShowNavigationItem;
            
            base.OnDeactivated();
            
        }
    }
}
