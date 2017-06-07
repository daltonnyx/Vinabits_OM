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
using DevExpress.ExpressApp.Web.Templates.ActionContainers.Menu;
using DevExpress.ExpressApp.Web.Templates.ActionContainers;

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ChangeEditorViewController : WebModificationsController
    {
        public ChangeEditorViewController()
        {
            InitializeComponent();
            TargetViewType = ViewType.DetailView;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            //Add client script to ActionBase Item
            //SOLUTION: https://www.devexpress.com/Support/Center/Question/Details/S32806
            SaveAction.SetClientScript(@"var scroll = window.setInterval(function(){
                                                window.scrollBy(0,-30);
                                                if(window.pageYOffset <= 0)
                                                    clearInterval(scroll);
                                        }, 1000 / 60);"
                                       , true);
        }


        #region Overide để thay đổi giao diện Detail khi edit Document
        protected override void ExecuteEdit(SimpleActionExecuteEventArgs args)
        {
            if (View.Id == "Document_DetailView" && args.CurrentObject.GetType() == typeof(Document))
            {
                IObjectSpace os = Application.CreateObjectSpace();
                DetailView dv = Application.CreateDetailView(os, (IModelDetailView)Application.FindModelView("Document_DetailView_New"), true, os.GetObject(View.CurrentObject));
                dv.ViewEditMode = ViewEditMode.Edit;
                args.ShowViewParameters.CreatedView = dv;
                os.SetModified(dv.CurrentObject);
            }
            else if (View.Id == "TaskExtra_DetailView" && args.CurrentObject.GetType() == typeof(TaskExtra))
            {
                IObjectSpace os = Application.CreateObjectSpace();
                DetailView dv = Application.CreateDetailView(os, (IModelDetailView)Application.FindModelView("TaskExtra_DetailView_New"), true, os.GetObject(View.CurrentObject));
                dv.ViewEditMode = ViewEditMode.Edit;
                args.ShowViewParameters.CreatedView = dv;

            }
            else {
                base.ExecuteEdit(args);
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
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
