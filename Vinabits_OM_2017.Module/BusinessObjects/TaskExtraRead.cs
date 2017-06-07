using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.DC;

namespace Vinabits_OM_2017.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Công việc - Lịch sử xem/xử lý")]
    [ImageName("BO_Validation")]
    public class TaskExtraRead : BaseObject
    {
        private DateTime dateread;
        private Employee empread;
        private TaskExtra taskread;

        public TaskExtraRead(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        public DateTime DateRead
        {
            get { return dateread; }
            set { SetPropertyValue("DateRead", ref dateread, value); }
        }

        [DataSourceCriteria("IsActive")]
        public Employee EmpRead
        {
            get { return empread; }
            set { empread = value; }
        }

        public TaskExtra TaskRead
        {
            get { return taskread; }
            set { taskread = value; }
        }
    }

}
