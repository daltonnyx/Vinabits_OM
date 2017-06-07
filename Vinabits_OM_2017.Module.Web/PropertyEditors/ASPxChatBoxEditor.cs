using DevExpress.ExpressApp.Web.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Model;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp;
using Vinabits_OM_2017.Module.BusinessObjects;

namespace Vinabits_OM_2017.Module.Web.PropertyEditors
{
    [PropertyEditor(typeof(string),false)]
    public class ASPxChatBoxEditor : WebPropertyEditor
    {
        private ASPxHtmlEditor htmlEditor;

        private ASPxCallbackPanel container;

        private ASPxButton submit;

        private ASPxTrackBar trackBar;

        private ASPxComboBox statusDropdown;

        private IObjectSpace os;

        public ASPxChatBoxEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model)
        {
        }

        private bool IsTrackBarChanged
        {
            get
            {
                return Convert.ToInt32(trackBar.Value) != CurrentTask.PercentCompleted;
            }
        }

        private bool CanUpdateTrackbar
        {
            get
            {
                return trackBar.Enabled;
            }
        }

        private bool CanUpdateStatusDropdown
        {
            get
            {
                return statusDropdown.Enabled;
            }
        }

        private bool IsStatusDropdownChanged
        {
            get
            {
                return Convert.ToInt32(statusDropdown.SelectedItem.Value) != CurrentTask.Status;
            }
        }

        protected override WebControl CreateEditModeControlCore()
        {
            return new ASPxLabel();
        }

        protected override WebControl CreateViewModeControlCore()
        {

            #region Submit Button
            submit = this.CreateSubmitButton();
            #endregion
            #region htmlEditor
            htmlEditor = CreateChatEditor();
            #endregion
            container = new ASPxCallbackPanel();
            container.ClientInstanceName = "_ChatBox_Callback_Panel_";
            container.Callback += Container_Callback;
            container.Controls.Add(htmlEditor);
            container.Controls.Add(this.CreateBottomPanel());
            container.Controls.Add(submit);
            
            return container;
        }

        protected bool IsCurrentCanUpdate
        {
            get
            {
                return CurrentTask.TaskAssignedTo.Oid == Guid.Parse(SecuritySystem.CurrentUserId.ToString());
            }
        }

        protected ASPxButton CreateSubmitButton()
        {
            ASPxButton control = new ASPxButton();
            control.Text = "Gửi";
            //submit.Click += Submit_Click;
            control.AutoPostBack = false;
            control.ClientSideEvents.Click = @"function(e) { eval('_ChatBox_Callback_Panel_').PerformCallback();eval('__ChatBox_Grid_').Refresh(); }";
            return control;
        }

        protected ASPxHtmlEditor CreateChatEditor()
        {
            ASPxHtmlEditor control = new ASPxHtmlEditor();
            control.ID = "ChatHtmlEditor";
            control.Width = Unit.Percentage(100);
            return control;
        }

        protected WebControl CreateBottomPanel()
        {
            trackBar = CreateTrackBar();
            statusDropdown = CreateStatusDropDown();
            ASPxPanel trackBarPanel = new ASPxPanel();
            trackBarPanel.Width = Unit.Percentage(100);
            trackBarPanel.Controls.Add(this.CreateTrackbarLabel());
            trackBarPanel.Controls.Add(trackBar);
            trackBarPanel.Controls.Add(statusDropdown);
            if (!IsCurrentCanUpdate)
            {
                trackBar.Enabled = false;
                statusDropdown.Enabled = false;
            }
            return trackBarPanel;
        }

        protected ASPxComboBox CreateStatusDropDown()
        {
            ASPxComboBox control = new ASPxComboBox();
            control.Items.Add(new ListEditItem("--Chọn tình trạng--", -1));
            control.Items.Add(new ListEditItem("Chưa bắt đầu", 0));
            control.Items.Add(new ListEditItem("Đang thực hiện", 1));
            control.Items.Add(new ListEditItem("Tạm hoãn", 2));
            control.Items.Add(new ListEditItem("Hoàn thành", 3));
            control.Items.Add(new ListEditItem("Hủy bỏ", 4));
            control.EnableCallbackMode = false;
            control.Width = Unit.Pixel(178);
            control.Style.Add("display", "inline-block");
            control.Style.Add("position", "relative");
            control.Style.Add("top", "2.5em");
            control.Style.Add("margin-left", "1.5em");
            control.SelectedIndex = control.Items.IndexOfValue(CurrentTask.Status.ToString());
            return control;
        }

        protected ASPxLabel CreateTrackbarLabel()
        {
            ASPxLabel trackBarLabel = new ASPxLabel();
            trackBarLabel.Text = "Tiến độ: ";
            trackBarLabel.Style.Add("position", "relative");
            trackBarLabel.Style.Add("top", "2em");
            return trackBarLabel;
        }

        protected ASPxTrackBar CreateTrackBar()
        {
            ASPxTrackBar control = new ASPxTrackBar();
            control.CssClass = "center";
            control.MinValue = 0;
            control.MaxValue = 100;
            control.Step = 1;
            control.LargeTickInterval = 10;
            control.SmallTickFrequency = 5;
            control.ScaleLabelHighlightMode = DevExpress.Web.ScaleLabelHighlightMode.AlongBarHighlight;
            control.ValueToolTipPosition = DevExpress.Web.ValueToolTipPosition.RightOrBottom;
            control.ScalePosition = DevExpress.Web.ScalePosition.LeftOrTop;
            control.Theme = "Youthful";
            control.Style.Add("display", "inline-block");
            //trackBar.TextField = "Tiến độ";
            control.Width = Unit.Pixel(240);
            control.Value = CurrentTask.PercentCompleted;
            return control;
        }

        private void Container_Callback(object sender, CallbackEventArgsBase e)
        {
            if (htmlEditor != null && this.View.CurrentObject is TaskExtra)
            {
                if (os == null)
                {
                    os = this.View.ObjectSpace;
                }
                try
                {
                    NoteExtra chat = os.CreateObject<NoteExtra>();
                    TaskExtra task = this.CurrentTask;
                    chat.EmployeeCreated = GetCurrentUserInObjectSpace();
                    chat.DateCreated = DateTime.Now;
                    chat.Text = htmlEditor.Html;
                    bool hasHorizonalLine = true;
                    if (IsTrackBarChanged && CanUpdateTrackbar) { 
                    
                        chat.Text += "<hr/>" + string.Format("<em>Cập nhật tiếp độ: {0}%</em>", trackBar.Value);
                        task.PercentCompleted = Convert.ToInt32(trackBar.Value);
                        hasHorizonalLine = false;
                    }
                    if(IsStatusDropdownChanged && CanUpdateStatusDropdown)
                    {
                         chat.Text += hasHorizonalLine ? "<hr/>" : "<br/>";
                         chat.Text += string.Format("<em>Cập nhật Tình trạng: {0}</em>", statusDropdown.SelectedItem.Text);
                        task.Status = Convert.ToInt32(statusDropdown.SelectedItem.Value);
                    }
                    chat.TaskNote = task;
                    os.CommitChanges();
                }
                catch (Exception)
                {

                }
                finally
                {
                    htmlEditor.Html = string.Empty;

                }
            }
        }

        private TaskExtra CurrentTask
        {
            get
            {
                return this.View.CurrentObject as TaskExtra;
            }
        }

        protected Employee GetCurrentUserInObjectSpace()
        {
            return this.os?.GetObject<Employee>((Employee)SecuritySystem.CurrentUser);
        }        

        protected override object GetControlValueCore()
        {
            return ((ASPxHtmlEditor)InplaceViewModeEditor.FindControl("WebControl")).Html;
        }

        protected override void ReadEditModeValueCore()
        {
            
        }
    }
}
