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

        private ASPxButton cancel;

        private ASPxButton pause;

        private ASPxTrackBar trackBar;

        private ASPxLabel statusLabel;

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

        protected bool IsCurrentCanUpdate
        {
            get
            {
                return CurrentTask.TaskAssignedTo.Oid == Guid.Parse(SecuritySystem.CurrentUserId.ToString());
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
            cancel = this.CreateCancelButton();
            pause = this.CreatePauseButton();
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
            container.Controls.Add(pause);
            container.Controls.Add(cancel);
            return container;
        }

        private ASPxButton CreatePauseButton()
        {
            ASPxButton control = new ASPxButton();
            control.Text = "Tạm dừng";
            control.AutoPostBack = false;
            control.Style.Add("background-color", "#FFEA00");
            control.Style.Add("color", "#fff");
            control.ClientSideEvents.Click = @"function(e) { 
                if(window.confirm('Bạn có muốn tạm dừng công việc này không?')) {
                    eval('_ChatBox_Callback_Panel_').PerformCallback('PAUSE');eval('__ChatBox_Grid_').Refresh(); 
                }
            }";
            if (!IsCurrentCanUpdate)
            {
                control.Enabled = false;
                control.ClientEnabled = false;
            }
            return control;
        }

        private ASPxButton CreateCancelButton()
        {
            ASPxButton control = new ASPxButton();
            control.Text = "Hủy";
            control.AutoPostBack = false;
            control.Style.Add("background-color", "#f44336");
            control.Style.Add("color", "#fff");
            control.ClientSideEvents.Click = @"function(e) { 
                if(window.confirm('Bạn có muốn HỦY công việc này không?')) {
                    eval('_ChatBox_Callback_Panel_').PerformCallback('CANCEL');eval('__ChatBox_Grid_').Refresh(); 
                }
            }";
            if (!IsCurrentCanUpdate)
            {
                control.Enabled = false;
                control.ClientEnabled = false;
            }
            return control;
        }


        protected ASPxButton CreateSubmitButton()
        {
            ASPxButton control = new ASPxButton();
            control.Text = "Cập nhật";
            control.AutoPostBack = false;
            control.Style.Add("background-color", "#00C853");
            control.Style.Add("color", "#fff");
            control.ClientSideEvents.Click = @"function(e) { eval('_ChatBox_Callback_Panel_').PerformCallback('UPDATE');eval('__ChatBox_Grid_').Refresh(); }";
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
            statusLabel = CreateStatuLabel();
            this.UpdateLabelText();
            this.UpdateLabelStyle();
            ASPxPanel trackBarPanel = new ASPxPanel();
            trackBarPanel.Width = Unit.Percentage(100);
            trackBarPanel.Controls.Add(this.CreateTrackbarLabel());
            trackBarPanel.Controls.Add(trackBar);
            if (!IsCurrentCanUpdate)
            {
                trackBar.Enabled = false;
            }
            return trackBarPanel;
        }

        protected ASPxLabel CreateStatuLabel()
        {
            ASPxLabel control = new ASPxLabel();
            control.Width = Unit.Pixel(178);
            control.Style.Add("display", "inline-block");
            control.Style.Add("position", "relative");
            control.Style.Add("top", "2.5em");
            control.Style.Add("margin-left", "1.5em");
            return control;
        }

        private void UpdateLabelText()
        {
            if(statusLabel != null)
            {
                statusLabel.Text = "Tình trạng: " + ASPxTaskExtraStatusEditor.getCaption(CurrentTask.Status);
            }
        }

        private void UpdateLabelStyle()
        {
            if (statusLabel != null)
            {
                statusLabel.Style.Add("font-style", "italic");
                switch(CurrentTask.Status)
                {
                    case 0:
                        statusLabel.Style.Add("color", "#999");break;
                    case 1:
                        statusLabel.Style.Add("color", "#000"); break;
                    case 2:
                        statusLabel.Style.Add("color", "#FFEB3B"); break;
                    case 3:
                        statusLabel.Style.Add("color", "green"); break;
                    case 4:
                        statusLabel.Style.Add("color", "red"); break;
                    default:
                        break;
                }
            }
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
                    if (e.Parameter == "UPDATE") {
                        this.UpdateTask(task, chat);
                    }
                    else if(e.Parameter == "PAUSE")
                    {
                        this.PauseTask(task, chat);
                    }
                    else if(e.Parameter == "CANCEL")
                    {
                        this.CancelTask(task, chat);
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

        private void CancelTask(TaskExtra task, NoteExtra chat)
        {
            if (CanUpdateTrackbar)
            {
                chat.Text += "<hr/>";
                chat.Text += string.Format("<em>Cập nhật Tình trạng: <span style=\"color:red;\">{0}</span></em>", "Hủy bỏ");
                task.Status = 4;
                task.PercentCompleted = 0;
                trackBar.Value = 0;
            }
        }

        private void PauseTask(TaskExtra task, NoteExtra chat)
        {
            if (CanUpdateTrackbar)
            {
                chat.Text += "<hr/>";
                chat.Text += string.Format("<em>Cập nhật Tình trạng: <span style=\"color:#FFEB3B;\">{0}</span></em>", "Tạm hoãn");
                task.Status = 2;
            }
        }

        private void UpdateTask(TaskExtra task, NoteExtra chat)
        {
            bool hasHorizonalLine = true;
            int trackValue = Convert.ToInt32(trackBar.Value);
            if (IsTrackBarChanged && CanUpdateTrackbar)
            {

                chat.Text += "<hr/>" + string.Format("<em>Cập nhật tiếp độ: {0}%</em>", trackBar.Value);
                hasHorizonalLine = false;
            }
            if (IsTrackBarChanged && trackValue > 0 && task.Status != 1)
            {
                chat.Text += hasHorizonalLine ? "<hr/>" : "<br/>";
                chat.Text += string.Format("<em>Cập nhật Tình trạng: {0}</em>", "Đang thực hiện");
                task.Status = 1;
            }
            else if (IsTrackBarChanged && trackValue == 100 && task.Status != 3)
            {
                chat.Text += hasHorizonalLine ? "<hr/>" : "<br/>";
                chat.Text += string.Format("<em>Cập nhật Tình trạng: {0}</em>", "Hoàn thành");
                task.Status = 3;
            }
            task.PercentCompleted = trackValue;
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
