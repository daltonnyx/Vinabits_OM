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
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.ReportsV2.Web;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using Vinabits_OM_2017.Module.BusinessObjects;

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class EventViewController : ViewController
    {
        public EventViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
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
            if(View is DetailView)
            {
                ((DetailView)View).FindItem("IsPublic").ControlCreated += EventViewController_ControlCreated;
                ((DetailView)View).FindItem("StartOn").ControlCreated += EventViewController_ControlCreated1;
                ((DetailView)View).FindItem("EndOn").ControlCreated += EventViewController_ControlCreated1;
            }   
        }

        protected override void OnViewControlsDestroying()
        {
            if (View is DetailView)
            {
                ((DetailView)View).FindItem("IsPublic").ControlCreated -= EventViewController_ControlCreated;
                ((DetailView)View).FindItem("StartOn").ControlCreated -= EventViewController_ControlCreated1;
                ((DetailView)View).FindItem("EndOn").ControlCreated -= EventViewController_ControlCreated1;
            }

            base.OnViewControlsDestroying();
        }

        private void EventViewController_ControlCreated1(object sender, EventArgs e)
        {
            ((ASPxDateTimePropertyEditor)sender).Editor.TimeSectionProperties.Visible = true;
            ((ASPxDateTimePropertyEditor)sender).Editor.TimeSectionProperties.TimeEditProperties.EditFormatString = "HH:mm";
            ((ASPxDateTimePropertyEditor)sender).Editor.TimeSectionProperties.OkButtonText = "Xong";
            ((ASPxDateTimePropertyEditor)sender).Editor.TimeSectionProperties.ShowCancelButton = false;
        }

        private void EventViewController_ControlCreated(object sender, EventArgs e)
        {
            //((ASPxPropertyEditor)sender).Model
            Employee emp = SecuritySystem.CurrentUser as Employee;
            var roleExists = emp.EmployeeRoles.Where(role => role.Name == "Tasker").Count() > 0;
            if(!roleExists) { 
                ((ASPxPropertyEditor)sender).Model.SetValue<bool>("AllowEdit", false);
            }
            else
            {
                ((ASPxPropertyEditor)sender).Model.SetValue<bool>("AllowEdit", true);
            }
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void EventScheduleReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objectSpace = ReportDataProvider.ReportObjectSpaceProvider.CreateObjectSpace(typeof(ReportDataV2));
                IReportDataV2 reportData = objectSpace.FindObject<ReportDataV2>(CriteriaOperator.Parse("[DisplayName] = 'SchedulerReport'"));
                string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
                Frame.GetController<MyWebReportServiceController>().ShowPreview(handle, null, null, false, null, false, e.ShowViewParameters);
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class MyWebReportServiceController : WebReportServiceController
    {
        public void ShowPreview(string reportContainerHandle, ReportParametersObjectBase parametersObject, CriteriaOperator criteria, bool canApplyCriteria, SortProperty[] sortProperty, bool canApplySortProperty, ShowViewParameters showViewParameters)
        {
            base.ShowReportPreviewCore(reportContainerHandle, parametersObject, criteria, canApplyCriteria, sortProperty, canApplySortProperty, showViewParameters);
        }
    }
}
