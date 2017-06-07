using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Scheduler.Web;
using DevExpress.ExpressApp.Editors;
using DevExpress.Web;

namespace Vinabits_OM_2017.Module.Web.PropertyEditors
{
    [PropertyEditor(typeof(int), false)]
    public class ASPxTaskExtraStatusEditor : ASPxSchedulerStatusPropertyEditor
    {
        public ASPxTaskExtraStatusEditor(Type objectType, IModelMemberViewItem info) : base(objectType, info)
        {
        }

        protected override string GetPropertyDisplayValue()
        {
            return getCaption((int)PropertyValue);
        }

        protected override void SetupControl(WebControl control)
        {
            base.SetupControl(control);
            if (control is ASPxComboBox)
            {
                foreach (ListEditItem item in ((ASPxComboBox)control).Items)
                {
                    item.Text = getCaption((int)item.Value);
                }
            }
            else if (control is Label)
            {
                ((Label)control).Text = getCaption((int)PropertyValue);
            }
        }

        private string getCaption(int value)
        {
            string caption = string.Empty;
            switch (value)
            {
                case 0:
                    caption = "Chưa bắt đầu";
                    break;
                case 1:
                    caption = "Đang thực hiện";
                    break;
                case 2:
                    caption = "Tạm hoãn";
                    break;
                case 3:
                    caption = "Hoàn thành";
                    break;
                case 4:
                    caption = "Hủy bỏ";
                    break;
                default:
                    break;
            }
            return caption;
        }
    }
}
