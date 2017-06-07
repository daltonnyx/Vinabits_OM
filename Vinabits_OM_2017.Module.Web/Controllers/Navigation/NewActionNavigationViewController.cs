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
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.SystemModule;

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class NewActionNavigationViewController : ViewController
    {
        public NewActionNavigationViewController()
        {
            InitializeComponent();
            //TargetViewNesting = Nesting.Root;
        }
        protected override void OnActivated()
        {
            base.OnActivated();            
            //1. Navi check
            ShowNavigationItemController showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
            showNavigationItemController.CustomShowNavigationItem += showNavigationItemController_CustomShowNavigationItem;

            //1. Document Detailview
            if (View.Id == "Document_DetailView")
            {
                
            }
        }

        void showNavigationItemController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            ///1. Document: Click Nav hiển thị NEW Document
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "NewDocumentNav") //&& View.Id != "Document_DetailView_Edit"
            {
                SingleChoiceAction newObjectAction = Frame.GetController<NewObjectViewController>().NewObjectAction;
                newObjectAction.DoExecute(new ChoiceActionItem() { Data = typeof(Document) });
                e.Handled = true;
            }

            ///2. Task
            ///
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
