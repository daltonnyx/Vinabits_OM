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
using Vinabits_OM_2017.Module.BusinessObjects;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;

namespace Vinabits_OM_2017.Module.Web.Controllers.CongViec
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TaskFileUploadViewController : ViewController<ListView>
    {
        public TaskFileUploadViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(TaskExtraFile);
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            WebWindow.CurrentRequestWindow.PagePreRender += CurrentRequestWindow_PagePreRender;
        }

        private void CurrentRequestWindow_PagePreRender(object sender, EventArgs e)
        {
            // Để thay đổi thuộc tính cho Grid trong ListView thì bắt buộc phải hook ở event này để toàn bộ Control được khởi tạo xong.
            if (Frame != null & Frame.Template != null && View != null && View.Editor is ASPxGridListEditor)
            {
                ASPxGridListEditor gridListEditor = (ASPxGridListEditor)View.Editor;
                gridListEditor.Grid.ClientInstanceName = "__Files__Uploaded__";
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            if (WebWindow.CurrentRequestWindow != null)
                WebWindow.CurrentRequestWindow.PagePreRender -= CurrentRequestWindow_PagePreRender;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
