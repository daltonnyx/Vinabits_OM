using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxHtmlEditor;
using System.Web;

namespace Vinabits_OM_2017.Module.Web.PropertyEditors
{
    [PropertyEditor(typeof(String), "ASPxHtmlToStringEditors", false)]
    public class ASPxHtmlEditors : WebPropertyEditor
    {
        private string txtHTML;

        public ASPxHtmlEditors(Type objectType, IModelMemberViewItem info)
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
                    txtHTML = (string)PropertyValue;
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
            ASPxHtmlEditor htmlPanel = new ASPxHtmlEditor();
            htmlPanel.ID = "htmlPanel";
            htmlPanel.Height = Unit.Pixel(250);
            htmlPanel.Width = Unit.Percentage(100);
            htmlPanel.Html = "<div style=\"font-size: 11px;\"></div>";
            string appDir = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            string appUrl = HttpContext.Current.Request.Url.Host;
            string imgDir = string.Format("{0}\\{1}", appDir, "Images\\editors");
            if (!System.IO.Directory.Exists(imgDir))
                System.IO.Directory.CreateDirectory(imgDir);

            if (System.IO.Directory.Exists(imgDir))
            {
                htmlPanel.SettingsDialogs.InsertImageDialog.SettingsImageUpload.UploadFolder = "/Images/editors/";  //"./Images/editors"; // imgDir; // UploadImageFolder
            }
            htmlPanel.HtmlChanged += new EventHandler<EventArgs>(htmlPanel_HtmlChanged);
            editorPanel.Controls.Add(htmlPanel);

            return editorPanel;
        }

        void htmlPanel_HtmlChanged(object sender, EventArgs e)
        {
            txtHTML = ((ASPxHtmlEditor)sender).Html;
            PropertyValue = txtHTML;
        }

        private void editorRender(ref ASPxPanel editorPanel)
        {
            ASPxHtmlEditor htmlPanel = (ASPxHtmlEditor)(editorPanel.FindControl("htmlPanel"));
            if (htmlPanel != null)
                htmlPanel.Html = txtHTML;
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
                txtHTML = (string)PropertyValue;
            ASPxPanel editorPanel = (ASPxPanel)Editor;
            editorRender(ref editorPanel);
        }

        //protected override object GetControlValue()
        //{
        //    return txtHTML;
        //}

        public override object ControlValue
        {
            get
            {
                return txtHTML;
            }
        }
        #endregion

        #region Thiết lập gia diện hiển thị (View)
        private ASPxPanel viewSetup()
        {
            ASPxPanel viewPanel = new ASPxPanel();
            return viewPanel;
        }

        private void viewRender(ref ASPxPanel viewPanel)
        {
            ASPxLabel lblInfo = new ASPxLabel();

            if (PropertyValue != null) // && View != null
            {
                lblInfo.Text = (string)PropertyValue; ;
                lblInfo.EncodeHtml = false;
            }
            viewPanel.Controls.Add(lblInfo);
        }

        protected override WebControl CreateViewModeControlCore()
        {
            return viewSetup();
        }

        protected override void ReadViewModeValueCore()
        {
            base.ReadViewModeValueCore();
            if (PropertyValue != null)
            {
                ASPxPanel viewPanel = (ASPxPanel)InplaceViewModeEditor;
                if (viewPanel.Controls.Count == 0)
                    viewRender(ref viewPanel);
                //this.CreateControl();
            }
        }
        #endregion

        protected override object GetControlValueCore()
        {
            return txtHTML; //((ASPxUploadControl)Editor).FileName
        }
    }
}