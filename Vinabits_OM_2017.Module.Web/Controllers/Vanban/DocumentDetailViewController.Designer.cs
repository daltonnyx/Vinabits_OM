namespace Vinabits_OM_2017.Module.Web.Controllers
{
    partial class DocumentDetailViewController
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
            this.actGiaoViec = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.actTheodoi = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.actChiDaoVanBan = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.actForwardDocument = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.actChuyenXuly = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // actGiaoViec
            // 
            this.actGiaoViec.Caption = "Giao việc theo văn bản";
            this.actGiaoViec.ConfirmationMessage = null;
            this.actGiaoViec.Id = "actGiaoViecTheoVB";
            this.actGiaoViec.ToolTip = null;
            this.actGiaoViec.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.actGiaoViec_Execute);
            // 
            // actTheodoi
            // 
            this.actTheodoi.Caption = "Đánh dấu & theo dõi";
            this.actTheodoi.ConfirmationMessage = null;
            this.actTheodoi.Id = "actTheodoiVanban";
            this.actTheodoi.ToolTip = null;
            this.actTheodoi.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.actTheodoi_Execute);
            // 
            // actChiDaoVanBan
            // 
            this.actChiDaoVanBan.Caption = "Phê duyệt/chỉ đạo";
            this.actChiDaoVanBan.ConfirmationMessage = null;
            this.actChiDaoVanBan.Id = "actChiDaoVanBan";
            this.actChiDaoVanBan.TargetObjectsCriteria = "![IsApproveed] And [DocumentEmployees][[LinkEmployee.Oid] = CurrentEmployeeOid() " +
    "And [IsDirected] And [IsCurrentDirected]]";
            this.actChiDaoVanBan.ToolTip = null;
            this.actChiDaoVanBan.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.actChiDaoVanBan_Execute);
            // 
            // actForwardDocument
            // 
            this.actForwardDocument.Caption = "Chuyển tiếp văn bản";
            this.actForwardDocument.ConfirmationMessage = "Chức năng sẽ photo/tạo mới văn bản cho đơn vị, bạn đồng ý thực hiện?";
            this.actForwardDocument.Id = "actForwardDocument";
            this.actForwardDocument.ToolTip = null;
            this.actForwardDocument.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.actForwardDocument_Execute);
            // 
            // actChuyenXuly
            // 
            this.actChuyenXuly.Caption = "Chuyển PGĐ xử lý";
            this.actChuyenXuly.ConfirmationMessage = null;
            this.actChuyenXuly.DefaultItemMode = DevExpress.ExpressApp.Actions.DefaultItemMode.LastExecutedItem;
            this.actChuyenXuly.Id = "actChuyenXuly";
            this.actChuyenXuly.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.actChuyenXuly.ShowItemsOnClick = true;
            this.actChuyenXuly.TargetObjectsCriteria = "![IsApproveed] And [DocumentEmployees][[LinkEmployee.Oid] = CurrentEmployeeOid() " +
    "And [IsDirected] And [IsCurrentDirected]]";
            this.actChuyenXuly.ToolTip = null;
            this.actChuyenXuly.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.actChuyenXuly_Execute);
            // 
            // DocumentDetailViewController
            // 
            this.Actions.Add(this.actGiaoViec);
            this.Actions.Add(this.actTheodoi);
            this.Actions.Add(this.actChiDaoVanBan);
            this.Actions.Add(this.actForwardDocument);
            this.Actions.Add(this.actChuyenXuly);
            this.TargetObjectType = typeof(Vinabits_OM_2017.Module.BusinessObjects.Document);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction actGiaoViec;
        private DevExpress.ExpressApp.Actions.SimpleAction actTheodoi;
        private DevExpress.ExpressApp.Actions.SimpleAction actChiDaoVanBan;
        private DevExpress.ExpressApp.Actions.SimpleAction actForwardDocument;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction actChuyenXuly;
    }
}
