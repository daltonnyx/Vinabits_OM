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
    [XafDisplayName("Lịch sử xem/xử lý văn bản")]
    [ImageName("BO_Validation")]
    public class DocumentRead : BaseObject
    {
        private DateTime dateread;
        private Employee empread;
        private Document docread;

        public DocumentRead(Session session)
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

        public Document DocRead
        {
            get { return docread; }
            set { SetPropertyValue("DocRead", ref docread, value); }
        }
    }

}
