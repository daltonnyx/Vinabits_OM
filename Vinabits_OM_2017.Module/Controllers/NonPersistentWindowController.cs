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
using System.ComponentModel;

namespace Vinabits_OM_2017.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class NonPersistentWindowController : WindowController
    {
        public NonPersistentWindowController()
        {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
        }
        private object CurrentUserId;
        protected override void OnActivated()
        {
            base.OnActivated();
            CurrentUserId = SecuritySystem.CurrentUserId;
            // Perform various tasks depending on the target Window.
            Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
        }

        private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
        {
            if(e.ObjectSpace is NonPersistentObjectSpace)
                ((NonPersistentObjectSpace)e.ObjectSpace).ObjectsGetting += ObjectSpace_ObjectsGetting;
        }

        private void ObjectSpace_ObjectsGetting(object sender, ObjectsGettingEventArgs e)
        {
            BindingList<VanBanReport> objects = new BindingList<VanBanReport>();
            IObjectSpace objspc = Application.CreateObjectSpace();
            IList<Document> vanban = objspc.GetObjects<Document>(new ContainsOperator("DocumentEmployees", CriteriaOperator.Parse("LinkEmployee.Oid = ?", CurrentUserId)));
            foreach(Document doc in vanban)
            {
                objects.Add(new VanBanReport() {
                    DateCreated = doc.DateCreated,
                    DocId = doc.DocId,
                    DocOrganization = doc.DocOrganization?.Title,
                    DocSaveId = doc.DocSaveId,
                    DocSignees = doc.DocSignees?.Title,
                    Doctype = doc.Doctype.Title,
                    EmployeeReceiveds = string.Join(", ",doc.EmployeeReceiveds.Select<Employee, string>(emp => emp.Fullname)),
                    Excerpt = doc.Excerpt,
                    InOutDocument = doc.InOutDocument,
                    Result = ""
                });
            }
            e.Objects = objects;
        }

        protected override void OnDeactivated()
        {
            Application.ObjectSpaceCreated -= Application_ObjectSpaceCreated;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
