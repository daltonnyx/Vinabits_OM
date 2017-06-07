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
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.SystemModule;

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ChangeNewEditorViewController : WebNewObjectViewController
    {
        public ChangeNewEditorViewController()
        {
            InitializeComponent();
            //TargetViewType = ViewType.DetailView; //Đưa cái này vào, nó bị Disable khi ở chế độ Listview
        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }

        #region Thay đổi giao diện tạo mới (New) của Document
        protected override void New(SingleChoiceActionExecuteEventArgs args)
        {
            Application.DetailViewCreating += Application_DetailViewCreating;
            base.New(args);
        }
        void Application_DetailViewCreating(object sender, DevExpress.ExpressApp.DetailViewCreatingEventArgs e)
        {
            if (e.Obj is Document && e.IsRoot)
            {
                e.ViewID = "Document_DetailView_New"; //Document_DetailView_Edit
                
            }
            if (e.Obj is TaskExtra && e.IsRoot)
            {
                e.ViewID = "TaskExtra_DetailView_New";

            }
        }
        #endregion

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            Application.DetailViewCreating -= Application_DetailViewCreating;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
