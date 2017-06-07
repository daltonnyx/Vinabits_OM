using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.Web;
using System.Web.UI.WebControls;

namespace Vinabits_OM_2017.Module.Web.PropertyEditors
{
    [PropertyEditor(typeof(Int32), "ASPxTrackBarToInt32Editors", false)]
    public class ASPxTrackBarEditors : WebPropertyEditor
    {
        private Int32 tbValue;

        public ASPxTrackBarEditors(Type objectType, IModelMemberViewItem info)
            : base(objectType, info)
        {
        }

        protected override void SetupControl(WebControl control)
        {
            base.SetupControl(control);
            if (control.GetType().Name == "ASPxPanel") //ASPxUploadControl
            {
                if (PropertyValue != null) //ViewEditMode == ViewEditMode.Edit && 
                {
                    tbValue = (Int32)PropertyValue;
                    //((ASPxUploadControl)control).NullText = ((FileUploads)PropertyValue).Name;
                }
            }
        }

        #region Thiết lập cho giao diện soạn thảo (Editor)
        #region Khởi tạo giao diện soạn thảo (Editor)
        private ASPxPanel editorSetup()
        {
            ASPxPanel editorPanel = new ASPxPanel();
            editorPanel.Width = Unit.Percentage(100);
            ASPxTrackBar trackbarPanel = new ASPxTrackBar();
            trackbarPanel.ID = "trackbarPanel";
            trackbarPanel.Width = Unit.Percentage(100);
            trackbarPanel.MinValue = 0;
            trackbarPanel.MaxValue = 100;
            trackbarPanel.LargeTickStartValue = 0;
            trackbarPanel.LargeTickEndValue = 100;
            trackbarPanel.LargeTickInterval = 10;
            trackbarPanel.SmallTickFrequency = 5;
            trackbarPanel.ScaleLabelHighlightMode = DevExpress.Web.ScaleLabelHighlightMode.AlongBarHighlight;
            trackbarPanel.ValueToolTipPosition = DevExpress.Web.ValueToolTipPosition.RightOrBottom;
            trackbarPanel.ScalePosition = DevExpress.Web.ScalePosition.LeftOrTop;
            trackbarPanel.Theme = "Youthful";
            trackbarPanel.ValueChanged += new EventHandler(trackbarPanel_ValueChanged); 
            editorPanel.Controls.Add(trackbarPanel);

            return editorPanel;
        }

        void trackbarPanel_ValueChanged(object sender, EventArgs e)
        {
            tbValue = Convert.ToInt32(((ASPxTrackBar)sender).Value);
            PropertyValue = tbValue;
        }

        private void editorRender(ref ASPxPanel editorPanel)
        {
            ASPxTrackBar trackbarPanel = (ASPxTrackBar)(editorPanel.FindControl("trackbarPanel"));
            if (trackbarPanel != null)
                trackbarPanel.Value = tbValue;
        }
        #endregion

        protected override WebControl CreateEditModeControlCore()
        {
            if (Editor == null)
                return editorSetup();
            return Editor;
        }

        protected override void ReadEditModeValueCore()
        {
            if (PropertyValue != null)
                tbValue = (Int32)PropertyValue;
            ASPxPanel editorPanel = (ASPxPanel)Editor;
            editorRender(ref editorPanel);
        }

        //protected override object GetControlValue()
        //{
        //    return tbValue;
        //}

        public override object ControlValue
        {
            get
            {
                return tbValue;
            }
        }
        
        #endregion

        //#region Thiết lập gia diện hiển thị (View)
        //private ASPxPanel viewSetup()
        //{
        //    ASPxPanel viewPanel = new ASPxPanel();
        //    return viewPanel;
        //}

        //private void viewRender(ref ASPxPanel viewPanel)
        //{
        //    ASPxLabel lblInfo = new ASPxLabel();

        //    if (PropertyValue != null) // && View != null
        //    {
        //        lblInfo.Text = (string)PropertyValue; ;
        //        lblInfo.EncodeHtml = false;
        //    }
        //    viewPanel.Controls.Add(lblInfo);
        //}

        //protected override WebControl CreateViewModeControlCore()
        //{
        //    return viewSetup();
        //}

        //protected override void ReadViewModeValueCore()
        //{
        //    base.ReadViewModeValueCore();
        //    if (PropertyValue != null)
        //    {
        //        ASPxPanel viewPanel = (ASPxPanel)InplaceViewModeEditor;
        //        if (viewPanel.Controls.Count == 0)
        //            viewRender(ref viewPanel);
        //        //this.CreateControl();
        //    }
        //}
        //#endregion

        protected override object GetControlValueCore()
        {
            return tbValue; //((ASPxUploadControl)Editor).FileName
        }
    }
}