



using DevExpress.Xpo;

namespace Vinabits_OM_2017.Module.BusinessObjects
{
    public interface IMultiUpload
    {
        XPCollection FileAttachments
        {
            get;
        }

        string MultiUpload
        {
            get; set;
        }
    }
}