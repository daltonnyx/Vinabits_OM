using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using System.Text.RegularExpressions;
using System.Net.Mail;
using DevExpress.ExpressApp.DC;

namespace Vinabits_OM_2017.Module.BusinessObjects
{
    [XafDisplayName("Lịch công việc")]
    [ImageName("Bo_Event")]
    [DefaultProperty("Subject")]
    [DefaultClassOptions]
    public class EventExtra : Event
    {
        public EventExtra(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            individual = this.Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);

        }

        [VisibleInDetailView(false)]
        [NonPersistent]
        public string Display
        {
            get
            {
                return string.Format("Tiêu đề:{0} \r\nNgười tham dự:{1} \r\nĐịa điểm:{2}", this.Subject, this.Participants, this.Location); 
            }
        }

        [XafDisplayName("Thành phần tham dự")]
        [Size(512)]
        public string Participants
        {
            get; set;
        }

        private bool isPublic = false;

        [NonPersistent]
        [XafDisplayName("Công việc chung")]
        public bool IsPublic
        {
            get
            {
                if (individual == null)
                    return true;
                return isPublic;
            }
            set
            {
                isPublic = value;
                OnChanged("Individual");
            }

        }
        
        private Employee individual; 
        [Browsable(false)]
        public Employee Individual
        {
            get
            {
                return individual;
            }
            set
            {
                SetPropertyValue<Employee>("Individual", ref individual, value);
            }
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if (this.IsLoading)
                return;
            if (propertyName != "Individual")
                return;
            if (!isPublic)
            {
                individual = this.Session.FindObject<Employee>(new BinaryOperator("Oid", SecuritySystem.CurrentUserId));
            }
            else
            {
                individual = null;
            }
        }


        protected override void OnSaving()
        {

            
            base.OnSaving();
        }

    }
}
