using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.DC;

namespace Vinabits_OM_2017.Module.BusinessObjects
{
    [Persistent, XafDefaultProperty("FileName")]
    [XafDisplayName("Công việc - File kèm")]
    [ImageName("Action_FileAttachment_Attach")]
    public class TaskExtraFile : XPObject
    {
        public TaskExtraFile(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        //[Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        private TaskExtra taskextra;
        [Browsable(false)]
        [Association("TaskExtra-TaskExtraFiles", typeof(TaskExtra))]
        //[DataSourceCriteria("Oid = @This.TaskExtra.Oid")]
        public TaskExtra TaskExtra
        {
            get { return taskextra; }
            set
            {
                SetPropertyValue("TaskExtra", ref taskextra, value);
            }
        }

        [ImmediatePostData]
        [ModelDefault("PropertyEditorType", "Vinabits_BMS.Module.Web.PropertyEditors.ASPxUploadToFileUploadsEditors")]
        public FileUploads UpFile
        {
            get { return GetPropertyValue<FileUploads>("UpFile"); }
            set { SetPropertyValue<FileUploads>("UpFile", value); }
        }

        private bool uploadComplete = false;
        [NonPersistent, Browsable(false)]
        public bool UploadComplete
        {
          get
          {
            return uploadComplete;
          }
          set
          {
            if (uploadComplete == value)
              return;
            uploadComplete = value;
          }
        }
    }

}
