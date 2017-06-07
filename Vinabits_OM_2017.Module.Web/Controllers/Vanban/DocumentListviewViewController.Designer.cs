namespace Vinabits_OM_2017.Module.Web.Controllers
{
    partial class DocumentListviewViewController
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
            this.actFeatureMarked = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.actReadMark = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.actUnreadMark = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.actFeatureUnmark = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // actFeatureMarked
            // 
            this.actFeatureMarked.Caption = "Đánh dấu theo dõi văn bản";
            this.actFeatureMarked.Category = "Edit";
            this.actFeatureMarked.ConfirmationMessage = null;
            this.actFeatureMarked.Id = "actFeatureMarked";
            this.actFeatureMarked.ToolTip = null;
            // 
            // actReadMark
            // 
            this.actReadMark.Caption = "Đánh dấu đã đọc";
            this.actReadMark.Category = "Edit";
            this.actReadMark.ConfirmationMessage = null;
            this.actReadMark.Id = "actReadMark";
            this.actReadMark.ToolTip = null;
            // 
            // actUnreadMark
            // 
            this.actUnreadMark.Caption = "Đánh dấu chưa đọc";
            this.actUnreadMark.Category = "Edit";
            this.actUnreadMark.ConfirmationMessage = null;
            this.actUnreadMark.Id = "actUnreadMark";
            this.actUnreadMark.ToolTip = null;
            // 
            // actFeatureUnmark
            // 
            this.actFeatureUnmark.Caption = "Hủy theo dõi văn bản";
            this.actFeatureUnmark.Category = "Edit";
            this.actFeatureUnmark.ConfirmationMessage = null;
            this.actFeatureUnmark.Id = "actFeatureUnmark";
            this.actFeatureUnmark.ToolTip = null;
            // 
            // DocumentListviewViewController
            // 
            this.Actions.Add(this.actFeatureMarked);
            this.Actions.Add(this.actReadMark);
            this.Actions.Add(this.actUnreadMark);
            this.Actions.Add(this.actFeatureUnmark);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction actFeatureMarked;
        private DevExpress.ExpressApp.Actions.SimpleAction actReadMark;
        private DevExpress.ExpressApp.Actions.SimpleAction actUnreadMark;
        private DevExpress.ExpressApp.Actions.SimpleAction actFeatureUnmark;
    }
}
