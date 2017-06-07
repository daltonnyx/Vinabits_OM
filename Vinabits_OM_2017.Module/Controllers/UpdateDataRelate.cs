using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using Vinabits_OM_2017.Module.BusinessObjects;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.Persistent.BaseImpl;

namespace Vinabits_OM_2017.Module.Controllers
{
    public partial class UpdateDataRelate : ViewController
    {
        public UpdateDataRelate()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            //2. Cập nhật quá trình open Task/Message/Note
            Employee emp = null;
            if (SecuritySystem.CurrentUser != null)
            {
                emp = (Employee)SecuritySystem.CurrentUser;
            }
            else if (SecuritySystem.CurrentUserId != null)
            {
                emp = SecuritySystem.LogonObjectSpace.FindObject<Employee>(new BinaryOperator("Oid", SecuritySystem.CurrentUserId));
            }
            if (emp != null && Application != null && View.CurrentObject != null && View is DetailView &&
                (View.CurrentObject is TaskExtra || View.CurrentObject is NoteExtra || View.CurrentObject is Messages)
                )
            {
                string strCrit = string.Format("ObjectType = '{0}' And ObjOid = {{{1}}} And EmpAttack = {{{2}}} And TypeAttack = {3}",
                                                                        View.CurrentObject.GetType().Name, ((BaseObject)View.CurrentObject).Oid, emp.Oid, 0);
                TrackEmployee trackEmp = emp.Session.FindObject<TrackEmployee>(CriteriaOperator.Parse(strCrit));
                if (trackEmp == null)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    TrackEmployee track = os.CreateObject<TrackEmployee>();
                    track.DateCreated = DateTime.Now;
                    track.ObjectType = View.CurrentObject.GetType().Name;
                    track.ObjOid = ((BaseObject)View.CurrentObject).Oid;
                    track.EmpAttack = emp.Oid;
                    track.TypeAttack = EmpAttackType.t0_read;

                    if (track.ObjOid != null && track.ObjOid != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    {
                        track.Save();
                        os.CommitChanges();
                    }
                }

                ///OLD WAY
                /*
                //SqlConnection connection = (SqlConnection)Application.Connection;
                SqlConnection connection = (SqlConnection)((ConnectionProviderSql)(((DevExpress.Xpo.Helpers.BaseDataLayer)(((BaseObject)SecuritySystem.CurrentUser).Session.DataLayer)).ConnectionProvider)).Connection;
                if (connection != null)
                {
                    using (IDataLayer dataLayer = new SimpleDataLayer(MSSqlConnectionProvider.CreateProviderFromConnection(connection, DevExpress.Xpo.DB.AutoCreateOption.SchemaAlreadyExists)))
                    {
                        using (ExplicitUnitOfWork euow = new ExplicitUnitOfWork(dataLayer))
                        {
                            TrackEmployee track = new TrackEmployee(euow);
                            Employee emp = track.Session.GetObjectByKey<Employee>(track.Session.GetKeyValue(SecuritySystem.CurrentUser));

                            XPCollection<TrackEmployee> tracks = new XPCollection<TrackEmployee>(euow);
                            string strCrit = string.Format("ObjectType = '{0}' And ObjOid = {{{1}}} And EmpAttack = {{{2}}} And TypeAttack = {3}",
                                                                        View.CurrentObject.GetType().Name, ((BaseObject)View.CurrentObject).Oid, emp.Oid, 0);
                                                                                                                                        //EmpAttackType.t0_read ~ 0
                            tracks.Criteria = CriteriaOperator.Parse(strCrit);
                            tracks.Load();

                            if (tracks == null || tracks.Count <= 0)
                            {
                                track.DateCreated = DateTime.Now;
                                track.ObjectType = View.CurrentObject.GetType().Name;
                                track.ObjOid = ((BaseObject)View.CurrentObject).Oid;
                                track.EmpAttack = emp.Oid;
                                track.TypeAttack = EmpAttackType.t0_read;

                                if (track.ObjOid != null && track.ObjOid != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                {
                                    track.Save();
                                    euow.CommitChanges();
                                }
                            }
                            tracks.Dispose();
                            euow.Dispose();
                        }
                        //dataLayer.Dispose();
                    }
                }
                */
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }


    }
}
