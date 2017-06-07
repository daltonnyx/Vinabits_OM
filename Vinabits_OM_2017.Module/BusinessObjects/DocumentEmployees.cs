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
using DevExpress.ExpressApp.Model;

namespace Vinabits_OM_2017.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Người nhận văn bản")]
    [ImageName("BO_Validation"), DeferredDeletion(Enabled = false)] //DeferredDeletion = false ===>> Xóa thẳng trong DB.
    public class DocumentEmployees : BaseObject
    {
        private Employee linkEmployee;
        private Document linkDocument;
        private DateTime dateRead;

        public DocumentEmployees(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
            if (Session.IsNewObject(this))
            {
                isFollow = false;
                isDirected = false;
                isCurrentDirected = false;
                dateRead = DateTime.MinValue;
            }
        }

        [DataSourceCriteria("IsActive"), ImmediatePostData()]
        [Association]
        public Employee LinkEmployee
        {
            get { return linkEmployee; }
            set { SetPropertyValue("LinkEmployee", ref linkEmployee, value); }
        }

        [Association, ImmediatePostData()]
        public Document LinkDocument
        {
            get { return linkDocument; }
            set { SetPropertyValue("LinkDocument", ref linkDocument, value); }
        }

        [XafDisplayName("Ngày xem")]
        public DateTime DateRead
        {
            get { return dateRead; }
            set { SetPropertyValue("DateRead", ref dateRead, value); }
        }

        #region Theo dõi người chỉ đạo văn bản
        private bool isDirected;
        [XafDisplayName("Quyền chỉ đạo văn bản?")]
        public bool IsDirected
        {
            get { return isDirected; }
            set
            {
                SetPropertyValue("IsDirected", ref isDirected, value);
            }
        }

        private bool isCurrentDirected;
        [XafDisplayName("Người chỉ đạo hiện tại?")]
        public bool IsCurrentDirected
        {
            get { return isCurrentDirected; }
            set
            {
                SetPropertyValue("IsCurrentDirected", ref isCurrentDirected, value);
            }
        }

        private DateTime dateDirected;
        [XafDisplayName("Ngày chỉ đạo")]
        public DateTime DateDirected
        {
            get { return dateDirected; }
            set { SetPropertyValue("DateDirected", ref dateDirected, value); }
        }

        private string directedContent;
        [XafDisplayName("Nội dung chỉ đạo"), Size(SizeAttribute.Unlimited)]
        public string DirectedContent
        {
            get { return directedContent; }
            set
            {
                SetPropertyValue("DirectedContent", ref directedContent, value);
            }
        }

        private int directedOrder;
        [XafDisplayName("Thứ tự chỉ đạo")]
        public int DirectedOrder
        {
            get { return directedOrder; }
            set
            {
                SetPropertyValue("DirectedOrder", ref directedOrder, value);
            }
        }
        #endregion

        [XafDisplayName("Ghi chú")]
        public string ReadNote
        {
            get
            {
                string note = "Chưa xem văn bản";
                if (this.DateRead != null && this.DateRead != DateTime.MinValue)
                    note = string.Format("Đã xem lúc {0} ngày {1}", this.DateRead.ToString("h:m tt"), this.DateRead.ToString("dd/MM/yyyy"));

                return note;
            }
        }

        private bool isFollow;
        [XafDisplayName("Theo dõi văn bản")]
        public bool IsFollow
        {
            get { return isFollow; }
            set { SetPropertyValue("IsFollow", ref isFollow, value); }
        }

        #region Parent Document để theo dõi đường đi của văn bản
        private Document parentDoc;
        [XafDisplayName("Văn bản gốc")]
        public Document ParentDoc
        {
            get { return parentDoc; }
            set
            {
                SetPropertyValue("ParentDoc", ref parentDoc, value);
            }
        }
        #endregion

        protected override void OnSaved()
        {
            base.OnSaved();
            //Cập nhật Document => isApproved = true
            if(this.directedContent != null && this.directedContent != string.Empty && !this.linkDocument.IsApproveed)
            {
                this.linkDocument.IsApproveed = true;
                this.linkDocument.Save();
                this.linkDocument.Session.CommitTransaction();
            }
        }
    }

}
