using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Hosting;
using System.Web.UI.HtmlControls;
using DevExpress.Xpo;
using Vinabits_OM_2017.Module.BusinessObjects;
using Vinabits_OM_2017.Module;
using System.Web;

namespace Vinabits_OM_2017.Module.Web.PropertyEditors
{
    [PropertyEditor(typeof(string), false)]
    public class MultiFileUploadEditor : WebPropertyEditor
    {

        IMultiUpload current;

        ListPropertyEditor fileList;

        public MultiFileUploadEditor(Type objectType, IModelMemberViewItem info) : base(objectType, info) { }

        protected override System.Web.UI.WebControls.WebControl CreateViewModeControlCore()
        {
            System.Web.UI.WebControls.WebControl control = new System.Web.UI.WebControls.WebControl(System.Web.UI.HtmlTextWriterTag.Div);
            return control;
        }
        ASPxUploadControl control;

        public FileUploads FileUpload
        {
            get { return fileuploads; }
            set { fileuploads = value; }
        }

        FileUploads fileuploads;

        protected override System.Web.UI.WebControls.WebControl CreateEditModeControlCore()
        {
            control = new ASPxUploadControl();
            control.UploadMode = UploadControlUploadMode.Advanced;
            control.AdvancedModeSettings.EnableFileList = true;
            control.AdvancedModeSettings.EnableMultiSelect = true;
            control.AdvancedModeSettings.EnableDragAndDrop = true;
            control.ValidationSettings.AllowedFileExtensions = new string[] { ".jpg", ".png", ".psd", ".pdf", ".gif", ".docx", ".doc",
                                                   ".xls", ".xlsx", ".txt", ".zip", ".rar", ".fla", ".swf" };
            control.ShowProgressPanel = true;
            control.ShowUploadButton = true;
            control.UploadButton.Text = "Tải lên";
            control.NullText = "Click để upload file Văn bản";
            control.NullTextStyle.Font.Bold = true;
            control.FileUploadComplete += control_FileUploadComplete;
            // Bắt event ở client để re-render GridView
            // Tên của control được thay đổi ở file FileUploadViewController.cs
            control.ClientSideEvents.FilesUploadComplete = @"function(e) {
                eval('__Files__Uploaded__').Refresh();
                var initialYOffset = window.pageYOffset;
               
                    var scroll = window.setInterval(function(){
                        window.scrollBy(0,50);
                    },1000/60);
                var clearScroll = window.setInterval(function(){
                    if(window.pageYOffset >= initialYOffset + 2000 || window.pageYOffset >= body.clientHeight - window.innerHeight) {
                            window.clearInterval(scroll);
                            window.clearInterval(clearScroll);
                    }
                },500);
            }"; 
            current = this.CurrentObject as IMultiUpload;
            return control;
        }


        void control_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.IsValid)
            {
                SaveFile(e.UploadedFile);
                //Lưu file vào đường dẫn tạm sau đó add file vào FileDinhkems của object hiện tại
            }
        }

        protected string SaveFile(UploadedFile uploadedFile)
        {
            //IObjectSpace os = ((DevExpress.ExpressApp.DetailView)(View)).ObjectSpace;
            Session sess = (View.ObjectSpace as DevExpress.ExpressApp.Xpo.XPObjectSpace).Session;

            fileuploads = new FileUploads(sess);
            string appDir = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            string appUrl = "./";

            if (uploadedFile.IsValid)
            {
                //fileName = string.Format("{0}{1}", MapPath("~/"), uploadedFile.FileName);
                fileuploads.Name = uploadedFile.FileName;
                fileuploads.FileName = uploadedFile.FileName;
                fileuploads.FileSize = uploadedFile.ContentLength;
                fileuploads.FileType = uploadedFile.ContentType; //PostedFile
                fileuploads.FileExt = System.IO.Path.GetExtension(uploadedFile.FileName);

                if (Global.Config == null)
                    Global.Config = Configuration.GetInstance(this.View.ObjectSpace);
                string savePath;// = string.Format("{0}/{1}", appDir, Global.Config.UploadPath);
                //string saveUrl = appUrl + "/" + Global.Config.UploadPath;

                savePath = fileuploads.newFullPathRender(fileuploads.FileName, View.CurrentObject != null ? View.CurrentObject.GetType().Name : "");
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(@savePath);

                //Thêm số thứ tự vào tên file để tránh trùng lặp file
                if (File.Exists(string.Format("{0}{1}", savePath, fileuploads.FileName)))
                {
                    int i = 1;
                    string tmpFileName = string.Format("{0}.{1}{2}", fileuploads.FileName.Substring(0, fileuploads.FileName.Length - fileuploads.FileExt.Length), i, fileuploads.FileExt);
                    while (File.Exists(string.Format("{0}{1}", savePath, tmpFileName)))
                    {
                        i++;
                        tmpFileName = string.Format("{0}.{1}{2}", fileuploads.FileName.Substring(0, fileuploads.FileName.Length - fileuploads.FileExt.Length), i, fileuploads.FileExt);
                    }
                    fileuploads.FileName = tmpFileName;
                    //File.Delete(string.Format("{0}{1}", savePath, fileuploads.FileName));
                }

                uploadedFile.SaveAs(string.Format("{0}{1}", savePath, fileuploads.FileName)); //uncomment this line
                //fileuploads.FileRealPath = savePath;
                if (HttpContext.Current != null)
                    appUrl = HttpContext.Current.Request.Url.Authority;
                fileuploads.FileUrl = string.Format("{0}{1}{2}", appUrl, fileuploads.FileRealPath.Replace(appDir, ""), fileuploads.FileName);
                fileuploads.Save();
                if(CurrentObject is Document) { 
                    DocumentFile docFile = new DocumentFile(sess);
                    docFile.DocFile = fileuploads;
                    docFile.Document = View.CurrentObject as Document;
                    docFile.UploadComplete = true;
                    docFile.Save();
                    current.FileAttachments.Add(docFile);
                }
                else if (CurrentObject is TaskExtra)
                {
                    TaskExtraFile taskFile = new TaskExtraFile(sess);
                    taskFile.UpFile = fileuploads;
                    taskFile.TaskExtra = View.CurrentObject as TaskExtra;
                    taskFile.UploadComplete = true;
                    taskFile.Save();
                    current.FileAttachments.Add(taskFile);
                }
                //sess.CommitTransaction();
                //fileuploads.Session.CommitTransaction();
            }
            return fileuploads.Name;
        }


        protected override void ReadEditModeValueCore()
        {
            ViewItem item = this.View.FindItem("FileDinhKems");
            fileList = (ListPropertyEditor)item;
            
        }

        private void AddFileToInstance()
        {

        }

        protected override object GetControlValueCore()
        {
            return this.PropertyValue;
        }

        
    }
}