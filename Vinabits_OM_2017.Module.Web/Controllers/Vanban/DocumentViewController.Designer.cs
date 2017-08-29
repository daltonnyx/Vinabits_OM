namespace Vinabits_OM_2017.Module.Web.Controllers.Vanban
{
    partial class DocumentViewController
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
            this.DocumentReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // DocumentReport
            // 
            this.DocumentReport.Caption = "Tạo báo cáo";
            this.DocumentReport.Category = "Edit";
            this.DocumentReport.ConfirmationMessage = null;
            this.DocumentReport.Id = "DocumentReport";
            this.DocumentReport.TargetObjectType = typeof(Vinabits_OM_2017.Module.BusinessObjects.Document);
            this.DocumentReport.ToolTip = null;
            this.DocumentReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DocumentReport_Execute);
            // 
            // DocumentViewController
            // 
            this.Actions.Add(this.DocumentReport);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction DocumentReport;
    }
}
