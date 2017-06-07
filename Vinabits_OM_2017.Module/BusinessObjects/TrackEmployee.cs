using System;
using DevExpress.Xpo;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace Vinabits_OM_2017.Module.BusinessObjects
{
    [Persistent, DefaultProperty("DateCreated")]
    [XafDisplayName("Lịch sử hệ thống")]
    [ImageName("BO_Audit_ChangeHistory")]
    public class TrackEmployee : XPObject
    {
        public TrackEmployee()
            : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public TrackEmployee(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }

        private DateTime dateCreated;
        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { SetPropertyValue("DateCreated", ref dateCreated, value); }
        }

        private Guid objectOid;
        public Guid ObjOid
        {
            get { return objectOid; }
            set { SetPropertyValue("ObjectOid", ref objectOid, value); }
        }

        private string objectType;
        public string ObjectType
        {
            get
            {
                return objectType;
            }
            set { SetPropertyValue("ObjectType", ref objectType, value); }
        }

        private Guid empAttack;
        public Guid EmpAttack
        {
            get { return empAttack; }
            set { SetPropertyValue("EmpAttack", ref empAttack, value); }
        }

        private EmpAttackType typeAttack;
        public EmpAttackType TypeAttack
        {
            get { return typeAttack; }
            set { SetPropertyValue("TypeAttack", ref typeAttack, value); }
        }
    }

    public enum EmpAttackType
    {
        [XafDisplayName("Read")]
        t0_read,
        [XafDisplayName("Write")]
        t1_write,
        [XafDisplayName("New")]
        t2_new
    }
}