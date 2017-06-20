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
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using Vinabits_OM_2017.Module.BusinessObjects;

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class NestListActionChangeViewController : ViewController
    {
        private ASPxGridListEditor gridListEditor;
        private ListViewProcessCurrentObjectController processCurrentObjectController;
        public NestListActionChangeViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            this.TargetViewNesting = Nesting.Nested;
            this.TargetViewType = ViewType.ListView;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            #region 1. ListView là danh sách trực thuộc
            if (View is ListView && !View.IsRoot && this.ObjectSpace != null && this.ObjectSpace.Owner != null && this.ObjectSpace.Owner is DetailView)
            {
                DetailView dv = this.ObjectSpace.Owner as DetailView;
                if (dv == null)
                    return;
                if ((dv.Id == "Document_DetailView" || dv.Id == "Document_DetailView_Edit") && 
                    (View.ObjectTypeInfo.Type.ToString() == "Vinabits_OM_2017.Module.BusinessObjects.Employee" || View.ObjectTypeInfo.Type == typeof(Vinabits_OM_2017.Module.BusinessObjects.DocumentEmployees)))
                {
                    Frame.GetController<LinkUnlinkController>().LinkAction.Caption = "Thêm người nhận";
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.Caption = "Bỏ chọn";
                    Frame.GetController<LinkUnlinkController>().LinkAction.Active.SetItemValue("ViewAllowLink", true);
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.Active.SetItemValue("ViewAllowUnlink", true);
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("DisableDeleteOnNestListview", false);
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Active.SetItemValue("DisableNewItemsOnNestListview", false);
                    Frame.GetController<ListViewController>().EditAction.Active.SetItemValue("DisableEditItemsOnNestListview", false);

                }
                else if (View.ObjectTypeInfo.Type.ToString() == "Vinabits_OM_2017.Module.BusinessObjects.DocumentFile")
                {
                    Frame.GetController<LinkUnlinkController>().LinkAction.Active.SetItemValue("DisableLinkOnNestListview", false);
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.Active.SetItemValue("DisableUnLinkOnNestListview", false);
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("EnableDeleteOnNestListview", true);
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Active.SetItemValue("EnableNewItemsOnNestListview", true);
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Caption = "Click để upload file văn bản";
                    Frame.GetController<ListViewController>().EditAction.Active.SetItemValue("EnableEditItemsOnNestListview", true);
                }
                else if (View.ObjectTypeInfo.Type == typeof(DevExpress.ExpressApp.Security.Strategy.SecuritySystemTypePermissionObject)
                      || View.ObjectTypeInfo.Type == typeof(DevExpress.ExpressApp.Security.Strategy.SecuritySystemMemberPermissionsObject)
                      || View.ObjectTypeInfo.Type == typeof(DevExpress.ExpressApp.Security.Strategy.SecuritySystemObjectPermissionsObject)) 
                {
                    Frame.GetController<LinkUnlinkController>().LinkAction.Caption = "Chọn/thêm";
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.Caption = "Bỏ chọn";
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("DisableDeleteOnNestListview", true);
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Active.SetItemValue("DisableNewItemsOnNestListview", true);
                    Frame.GetController<ListViewController>().EditAction.Active.SetItemValue("DisableEditItemsOnNestListview", true);
                }
                else
                {
                    Frame.GetController<LinkUnlinkController>().LinkAction.Caption = "Chọn/thêm";
                    Frame.GetController<LinkUnlinkController>().UnlinkAction.Caption = "Bỏ chọn";
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.SetItemValue("DisableDeleteOnNestListview", false);
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Active.SetItemValue("DisableNewItemsOnNestListview", false);
                    Frame.GetController<ListViewController>().EditAction.Active.SetItemValue("DisableEditItemsOnNestListview", false);
                }
                //Global:
                //Cho phép tạo mới tại Property
                if (View.Id.ToString().Contains("LookupListView"))
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Active.SetItemValue("DisableNewItemsOnNestListview", true);
                    Frame.GetController<NewObjectViewController>().NewObjectAction.ToolTip = "Thêm mới";
                }
                Frame.GetController<WebResetViewSettingsController>().ResetViewSettingsAction.Active.SetItemValue("DisableResetOnNestListview", false);
                Frame.GetController<ExportController>().ExportAction.Active.SetItemValue("DisableExportOnNestListview", false);

                #region 2. Không cho click xem detail
                processCurrentObjectController = Frame.GetController<ListViewProcessCurrentObjectController>();
                if (processCurrentObjectController != null)
                {
                    processCurrentObjectController.CustomProcessSelectedItem += ProcessCurrentObjectController_CustomProcessSelectedItem;
                }
                #endregion
            }
            #endregion


            #region Hiển thị text cho nhóm chuẩn lại (ko có theo mặc định: Count ...)
            if (((DevExpress.ExpressApp.ListView)View).Editor is ASPxGridListEditor)
            {
                gridListEditor = ((ASPxGridListEditor)((DevExpress.ExpressApp.ListView)View).Editor);
                if (gridListEditor != null)
                {
                    gridListEditor.ControlsCreated += GridListEditor_ControlsCreated;
                }
            }
            #endregion
        }

        private void GridListEditor_ControlsCreated(object sender, EventArgs e)
        {

            if (gridListEditor != null && gridListEditor.Grid != null)
            {
                gridListEditor.Grid.CustomGroupDisplayText += GridListEditor_CustomGroupDisplayText;
            }
        }

        private void ProcessCurrentObjectController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            e.Handled = true;
            if (e.InnerArgs.CurrentObject is Document && !View.IsRoot)
            {
                DetailView docSimpleView = Application.CreateDetailView(View.ObjectSpace, "DocumentSimple_DetailView", false, e.InnerArgs.CurrentObject);
                e.InnerArgs.ShowViewParameters.CreatedView = docSimpleView;
            }
            else {
                var act = sender as ListViewProcessCurrentObjectController;
                if (act != null)
                {
                    act.Active.SetItemValue("DisableThisAction", false);
                }
                
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            //Hiển thị text cho nhóm chuẩn lại (ko có theo mặc định: Count ...) ==> Move to Actived
            
        }

        private void GridListEditor_CustomGroupDisplayText(object sender, DevExpress.Web.ASPxGridViewColumnDisplayTextEventArgs e)
        {
            //ASPxGridView grid = sender as ASPxGridView;
            //grid.Settings.ShowColumnHeaders = false;
            //grid.Settings.ShowGroupedColumns = false;
            //e.Column.Grid.Settings.ShowTitlePanel = false;
            e.Column.Caption = e.DisplayText;
            if (e.Column.Grid != null && e.Column.Grid.GroupSummary.Count > 0)
            {
                e.Column.Grid.GroupSummary[0].DisplayFormat = "{0}";
                ///e.Column.Grid.GroupSummary[0].SummaryType = DevExpress.Data.SummaryItemType.None;
                //e.Column.Grid.GroupSummary[0].ValueDisplayFormat
                e.Column.Grid.GroupSummary[0].ShowInGroupFooterColumn = "1";
            }

            //throw new NotImplementedException();
            e.DisplayText = "";
            //e.Value = null;
            //gridListEditor.Grid.CustomGroupDisplayText -= GridListEditor_CustomGroupDisplayText;
        }

        protected override void OnDeactivated()
        {
            if (gridListEditor != null)
            {
                gridListEditor.ControlsCreated -= GridListEditor_ControlsCreated;
            }
            if (processCurrentObjectController != null)
            {
                processCurrentObjectController.CustomProcessSelectedItem -= ProcessCurrentObjectController_CustomProcessSelectedItem;
            }
            base.OnDeactivated();
        }

    }
}
