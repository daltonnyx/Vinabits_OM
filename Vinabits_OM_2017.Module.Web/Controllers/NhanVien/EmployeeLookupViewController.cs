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

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class EmployeeLookupViewController : ViewController
    {
        private Document curDoc;
        SimpleAction actEmployeeReceiveAdd;
        public EmployeeLookupViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "Employee_LookupListView";
            //TargetViewType = ViewType.DetailView;
            TargetObjectType = typeof(Employee);


            #region Thêm nút lệnh ADD người nhận: REV PAUSE 07/5/2017 - Chưa add được vào danh sách
            //DialogCancel
            //Frame.GetController<DevExpress.ExpressApp.SystemModule.LinkDialogController>().CancelAction.Active.SetItemValue("RemoveCancelButton", false);
            actEmployeeReceiveAdd = new SimpleAction(this, "actEmployeeReceiveAdd", DevExpress.Persistent.Base.PredefinedCategory.PopupActions);
            actEmployeeReceiveAdd.Caption = "Thêm người nhận";
            
            actEmployeeReceiveAdd.Execute += ActEmployeeReceiveAdd_Execute;
            #endregion
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if(ObjectSpace != null && ObjectSpace.Owner != null)
            {
                DetailView dvDoc = ObjectSpace.Owner as DetailView;

                if (dvDoc != null && dvDoc.CurrentObject != null && dvDoc.CurrentObject is Document)
                {
                    //dvDoc.ObjectSpace.CommitChanges();
                    curDoc = dvDoc.CurrentObject as Document;
                }
            }
        }

        private void ActEmployeeReceiveAdd_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Application.ShowViewStrategy.ShowViewInPopupWindow(Application.CreateDashboardView(Application.CreateObjectSpace(), "AccessDeniedMessage", true), null, null, "Đồng ý", null);
            return;
            if (View.SelectedObjects != null && View.SelectedObjects.Count > 0)
            {
                if(curDoc != null || 1==1)
                {
                    foreach(Employee emp in View.SelectedObjects)
                    {
                        curDoc.EmployeeReceiveds.Add(curDoc.Session.GetObjectByKey<Employee>(emp.Oid));
                    }
                    //curDoc.Session.CommitTransaction();
                }
            }
        }

        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            if (actEmployeeReceiveAdd != null)
                actEmployeeReceiveAdd.Active["LookupWindowContext"] = Frame.Context == TemplateContext.LookupWindow;
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            if (actEmployeeReceiveAdd != null)
            {
                actEmployeeReceiveAdd.Execute -= ActEmployeeReceiveAdd_Execute;
            }
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
