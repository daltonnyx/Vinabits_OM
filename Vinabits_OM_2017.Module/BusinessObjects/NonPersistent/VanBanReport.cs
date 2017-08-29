using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinabits_OM_2017.Module.BusinessObjects
{
    [DomainComponent, VisibleInReportsAttribute]
    public class VanBanReport
    {

        [XafDisplayName("Ngày nhập")]
        public DateTime DateCreated
        {
            get;
            set;
        }
        
        [XafDisplayName("Số VB đến")]
        public string DocSaveId
        {
            get;
            set;
        }

        [XafDisplayName("Số hiệu")]
        public string DocId
        {
            get;
            set;
        }

        [XafDisplayName("Loại văn bản")]
        public string Doctype
        {
            get;
            set;
        }

        
        [XafDisplayName("Chiều văn bản")] //, ImmediatePostData()
        public RadioButtonEnum InOutDocument
        {
            get;
            set;
        }

        
        [XafDisplayName("Người ký")]
        public string DocSignees
        {
            get;
            set;
        }
        
        [XafDisplayName("Nơi phát hành")]
        public string DocOrganization
        {
            get;
            set;
        }

        [XafDisplayName("Trích yếu")]
        public string Excerpt
        {
            get;
            set;
        }
        
        [XafDisplayName("Người nhận")]
        public string EmployeeReceiveds
        {
            get;
            set;
        }

        [XafDisplayName("Báo cáo")]
        public string Result
        {
            get;set;
        }
    }
}
