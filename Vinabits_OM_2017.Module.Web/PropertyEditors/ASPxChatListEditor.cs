using DevExpress.ExpressApp.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using System.Collections;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Model;
using DevExpress.Web;
using Vinabits_OM_2017.Module.BusinessObjects;
using System.Web.UI.WebControls;

namespace Vinabits_OM_2017.Module.Web.PropertyEditors
{
    [ListEditor(typeof(NoteExtra),false)]
    public class ASPxChatListEditor : ASPxGridListEditor
    {
        public ASPxChatListEditor(IModelListView info) : base(info)
        {
        }

        protected override ASPxGridView CreateGridControl()
        {
            ASPxGridView control = base.CreateGridControl();
            control.SettingsContextMenu.Enabled = false;
            control.SettingsBehavior.AllowGroup = false;
            control.Settings.ShowColumnHeaders = false;
            control.SettingsBehavior.ProcessSelectionChangedOnServer = false;
            control.CssClass += "Chatbox_main_table";
            control.CommandButtonInitialize += Control_CommandButtonInitialize;
            control.ClientInstanceName = "__ChatBox_Grid_";
            control.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            control.Settings.VerticalScrollableHeight = 600;
            control.Width = Unit.Percentage(100);
            control.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
            return control;
        }

        private void Control_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if(e.ButtonType == ColumnCommandButtonType.SelectCheckbox) {
                e.Column.Width = Unit.Pixel(1);
                e.Column.ShowSelectCheckbox = false;
                e.Visible = false;
            }
            //e.Column.ShowSelectButton = false;
        }
    }
}
