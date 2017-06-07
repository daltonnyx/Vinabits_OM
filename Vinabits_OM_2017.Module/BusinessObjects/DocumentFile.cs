using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.DC;

namespace Vinabits_OM_2017.Module.BusinessObjects
{
    [Persistent, XafDefaultProperty("FileName")]
    [XafDisplayName("File văn bản")]
    [ImageName("Action_FileAttachment_Attach")]
    public class DocumentFile : XPObject
    {
        public DocumentFile(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        [Browsable(false)]
        public string FileName
        {
            get { return DocFile == null ? "" : DocFile.FileName; }
        }

        //[Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        private Document document;
        [Browsable(false)]
        [Association("Document-DocumentFiles", typeof(Document))]
        [DataSourceCriteria("Oid = @This.Document.Oid")]
        public Document Document
        {
            get { return document; }
            set
            {
                SetPropertyValue("Document", ref document, value);
            }
        }

        [ImmediatePostData, XafDisplayName("Văn bản đính kèm")]
        [ModelDefault("PropertyEditorType", "Vinabits_OM_2017.Module.Web.PropertyEditors.ASPxUploadToFileUploadsEditors")]
        public FileUploads DocFile
        {
            get { return GetPropertyValue<FileUploads>("DocFile"); }
            set { SetPropertyValue<FileUploads>("DocFile", value); }
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
