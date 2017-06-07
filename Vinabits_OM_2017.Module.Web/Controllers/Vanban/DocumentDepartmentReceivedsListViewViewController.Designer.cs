namespace Vinabits_OM_2017.Module.Web.Controllers
{
    partial class DocumentDepartmentReceivedsListViewViewController
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
            this.actFilterTreeList = new DevExpress.ExpressApp.Actions.ParametrizedAction(this.components);
            // 
            // actFilterTreeList
            // 
            this.actFilterTreeList.Caption = "Lọc/tìm";
            this.actFilterTreeList.ConfirmationMessage = null;
            this.actFilterTreeList.Id = "actFilterTreeList";
            this.actFilterTreeList.NullValuePrompt = null;
            this.actFilterTreeList.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.actFilterTreeList.ShortCaption = null;
            this.actFilterTreeList.ToolTip = null;
            this.actFilterTreeList.Execute += new DevExpress.ExpressApp.Actions.ParametrizedActionExecuteEventHandler(this.actFilterTreeList_Execute);
            // 
            // DocumentDepartmentReceivedsListViewViewController
            // 
            this.Actions.Add(this.actFilterTreeList);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.ParametrizedAction actFilterTreeList;
    }
}
