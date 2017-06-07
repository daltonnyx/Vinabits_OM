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
    public partial class TaskDetailViewController : ViewController
    {
        public TaskDetailViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "TaskExtra_DetailView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            TaskExtra task = View.CurrentObject as TaskExtra;
            Employee currentUser = SecuritySystem.CurrentUser as Employee;
            if(task != null && currentUser != null && task.LastNote != null)
            {
                TrackEmployee track = View.ObjectSpace.FindObject<TrackEmployee>(CriteriaOperator.And(
                    new BinaryOperator("ObjectType", task.LastNote.GetType().Name),
                    new BinaryOperator("ObjOid", task.LastNote.Oid),
                    new BinaryOperator("EmpAttack", currentUser.Oid),
                    new BinaryOperator("TypeAttack", EmpAttackType.t0_read)
                ));
                if(track == null)
                {
                    track = View.ObjectSpace.CreateObject<TrackEmployee>();
                    track.ObjectType = task.LastNote.GetType().Name;
                    track.ObjOid = task.LastNote.Oid;
                    track.EmpAttack = currentUser.Oid;
                    track.TypeAttack = EmpAttackType.t0_read;
                    View.ObjectSpace.CommitChanges();
                }
            }

        }
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
