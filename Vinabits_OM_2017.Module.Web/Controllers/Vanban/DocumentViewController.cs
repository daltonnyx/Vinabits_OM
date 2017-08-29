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
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl;

namespace Vinabits_OM_2017.Module.Web.Controllers.Vanban
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DocumentViewController : ViewController
    {
        public DocumentViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Document);
        }
        private string viewId;
        protected override void OnActivated()
        {
            base.OnActivated();
            if(this.View is ListView)
            viewId = this.View.Id;
            // Perform various tasks depending on the target View.
            if(Frame != null)
            {
                Frame.GetController<NewObjectViewController>().NewObjectAction.ProcessCreatedView += NewObjectAction_ProcessCreatedView;
            }
        }

        private void NewObjectAction_ProcessCreatedView(object sender, ActionBaseEventArgs e)
        {
            if(this.View is DetailView)
            {
                Document newDoc = this.View.CurrentObject as Document;
                if(viewId == "Document_ListView_DocIn")
                {
                    newDoc.InOutDocument = RadioButtonEnum.Value2;
                }
                else if(viewId == "Document_ListView_DocOut")
                {
                    newDoc.InOutDocument = RadioButtonEnum.Value1;
                }
                this.View.CurrentObject = newDoc;
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            if (Frame != null)
            {
                Frame.GetController<NewObjectViewController>().NewObjectAction.ProcessCreatedView -= NewObjectAction_ProcessCreatedView;
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void DocumentReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objectSpace = ReportDataProvider.ReportObjectSpaceProvider.CreateObjectSpace(typeof(ReportDataV2));
                IReportDataV2 reportData = objectSpace.FindObject<ReportDataV2>(CriteriaOperator.Parse("[DisplayName] = 'VanBanReport'"));
                string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
                Frame.GetController<MyWebReportServiceController>().ShowPreview(handle, null, null, false, null, false, e.ShowViewParameters);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
