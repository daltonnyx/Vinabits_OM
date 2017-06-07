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
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Security;

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class RootActionChangeViewController : ViewController
    {
        public RootActionChangeViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            this.TargetViewNesting = Nesting.Root;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            #region 1. ListView là danh sách ROOT
            if (View is ListView && View.IsRoot)
            {
                if (View.ObjectTypeInfo.Type.ToString() == "Vinabits_OM_2017.Module.BusinessObjects.Document") //View.Id == "Document_ListView" && 
                {
                    Frame.GetController<ListViewController>().EditAction.Active.SetItemValue("DisableEditItemsOnNestListview", false);
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Active.SetItemValue("NewDocOnListviewRoot", true);
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Caption = "Nhập văn bản mới";
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Caption = "Xóa văn bản";
                    Frame.GetController<ModificationsController>().SaveAction.Caption = "Lưu";
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Caption = "Lưu & đóng";
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Caption = "Lưu & tạo mới";
                }
                else if (View.ObjectTypeInfo.Type.ToString() == "Vinabits_OM_2017.Module.BusinessObjects.TaskExtra")
                {
                    Frame.GetController<ListViewController>().EditAction.Active.SetItemValue("DisableEditItemsOnNestListview", false);
                }
                else
                {
                    Frame.GetController<ListViewController>().EditAction.Active.SetItemValue("EnableEditItemsOnNestListview", true);
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Caption = "Thêm mới";
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Caption = "Xóa";
                    Frame.GetController<ModificationsController>().SaveAction.Caption = "Lưu";
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Caption = "Lưu & đóng";
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Caption = "Lưu & tạo mới";
                }
                Frame.GetController<ModificationsController>().CancelAction.Caption = "Bỏ qua";
                Frame.GetController<ModificationsController>().CancelAction.ToolTip = "Hủy tác vụ";

            }
            #endregion
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

        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            //1. Hiển thị Username đang login
            MyDetailsController target = Frame.GetController<MyDetailsController>();
            if (target != null && target.MyDetailsAction != null)
                target.MyDetailsAction.Caption = SecuritySystem.CurrentUserName;
        }
    }
}
