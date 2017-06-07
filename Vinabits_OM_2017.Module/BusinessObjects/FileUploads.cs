using System;
using System.ComponentModel;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.DC;
using System.IO;

namespace Vinabits_OM_2017.Module.BusinessObjects
{
    [Persistent, XafDefaultProperty("FileName")]
    [XafDisplayName("File uploads")]
    [ImageName("Action_FileAttachment_Attach")]
    public class FileUploads : XPLiteObject
    {
        private string filename = String.Empty;
        private string name = String.Empty;
        private string fileext = String.Empty;
        private string filetype = String.Empty;
        private string filerealpath = String.Empty;
        private string fileurl = String.Empty;
        private long filesize = 0; //in byte
        private DateTime dateuploaded = DateTime.Now;

        public FileUploads(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
            if (Session.IsNewObject(this))
            {
                dateuploaded = DateTime.Now;
            }
        }

        [Key(AutoGenerate = true), Persistent, Browsable(false)]
        public long Oid { get; set; }

        [Size(1024)]
        public string Name
        {
            get { return name; }
            set { SetPropertyValue("Name", ref name, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string FileName
        {
            get { return filename; }
            set { SetPropertyValue("FileName", ref filename, value); }
        }

        [Size(1024)]
        public string FileType
        {
            get { return filetype; }
            set { SetPropertyValue("FileType", ref filetype, value); }
        }

        public string FileExt
        {
            get { return fileext; }
            set { SetPropertyValue("FileExt", ref fileext, value); }
        }

        //[Size(32000)]
        [Browsable(false)]
        public string FileRealPath
        {
            get { return filerealpath; }
            set { SetPropertyValue("FileRealPath", ref filerealpath, value); }
        }

        //[Size(32000)]
        public string FileUrl
        {
            get { return fileurl; }
            set { SetPropertyValue("FileUrl", ref fileurl, value); }
        }

        public long FileSize
        {
            get { return filesize; }
            set { SetPropertyValue("FileSize", ref filesize, value); }
        }

        [Browsable(false)]
        public DateTime DateUploaded
        {
            get { return dateuploaded; }
            set { SetPropertyValue("DateUploaded", ref dateuploaded, value); }
        }

        public string newFullPathRender(string fileName, string objectTypeName)
        {
            string appDir = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            if(appDir == null)
            {
                appDir = System.Configuration.ConfigurationManager.AppSettings["UploadRealPath"];
            }

            string appUrl = "./";
            if (Global.Config == null)
                Global.Config = Configuration.GetInstance(Session);
            string savePath = string.Format("{0}/{1}", appDir, Global.Config.UploadPath);
            string saveUrl = appUrl + "/" + Global.Config.UploadPath;

            //2. Người upload: Oid
            savePath = string.Format("{0}{1}/", savePath, DevExpress.ExpressApp.SecuritySystem.CurrentUserId.ToString());
            saveUrl = string.Format("{0}{1}/", saveUrl, DevExpress.ExpressApp.SecuritySystem.CurrentUserId.ToString());
            //3. Thể loại upload: Messages/Hopdong...
            savePath = string.Format("{0}{1}/", savePath, objectTypeName);
            saveUrl = string.Format("{0}{1}/", saveUrl, objectTypeName);
            this.FileRealPath = savePath;
            this.FileUrl = string.Format("{0}{1}", saveUrl, Path.GetFileName(fileName));

            return savePath;
        }
    }

}
