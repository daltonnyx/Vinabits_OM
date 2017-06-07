namespace Vinabits_OM_2017.Module.Web.Controllers
{
    partial class EventViewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.EventScheduleReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // EventScheduleReport
            // 
            this.EventScheduleReport.Caption = "Tạo lịch làm việc";
            this.EventScheduleReport.ConfirmationMessage = null;
            this.EventScheduleReport.Id = "EventScheduleReport";
            this.EventScheduleReport.ImageName = "BO_Event";
            this.EventScheduleReport.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.EventScheduleReport.ToolTip = null;
            this.EventScheduleReport.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.EventScheduleReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.EventScheduleReport_Execute);
            // 
            // EventViewController
            // 
            this.Actions.Add(this.EventScheduleReport);
            this.TargetObjectType = typeof(Vinabits_OM_2017.Module.BusinessObjects.EventExtra);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction EventScheduleReport;
    }
}
