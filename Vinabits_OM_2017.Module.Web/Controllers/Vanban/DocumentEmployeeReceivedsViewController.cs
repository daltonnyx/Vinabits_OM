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
using DevExpress.ExpressApp.Web;
using Vinabits_OM_2017.Module.BusinessObjects;
using DevExpress.ExpressApp.Web.Editors.ASPx;

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DocumentEmployeeReceivedsViewController : ViewController<ListView>
    {
        public DocumentEmployeeReceivedsViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Employee);
            TargetViewId = "Document_EmployeeReceiveds_ListView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            WebWindow.CurrentRequestWindow.PagePreRender += CurrentRequestWindow_PagePreRender;
        }

        private void CurrentRequestWindow_PagePreRender(object sender, EventArgs e)
        {
            if (Frame != null & Frame?.Template != null && View != null && View?.Editor is ASPxGridListEditor)
            {
                ASPxGridListEditor gridListEditor = (ASPxGridListEditor)View.Editor;
                gridListEditor.Grid.ClientInstanceName = "__OM_Document_EmployeeReceiveds__";
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
