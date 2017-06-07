using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinabits_OM_2017.Module.BusinessObjects;

namespace Vinabits_OM_2017.Module.Web.PropertyEditors
{
    [PropertyEditor(typeof(Vinabits_OM_2017.Module.BusinessObjects.DocumentSigneesOrganization), "OMWebLookupEditor", false)]
    public class OMWebLookupEditor : ASPxLookupPropertyEditor
    {
        private const string STR_NullText = "";
        public OMWebLookupEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) {
        }
        protected override void CreateControlItems(object currentSelectedObject, System.Collections.ArrayList list, ASPxComboBox control, DevExpress.ExpressApp.Web.Editors.WebLookupEditorHelper helper)
        {
            base.CreateControlItems(currentSelectedObject, list, control, helper);
            control.IncrementalFilteringMode = DevExpress.Web.IncrementalFilteringMode.Contains;
            control.DropDownStyle = DevExpress.Web.DropDownStyle.DropDown;
            control.NullText = STR_NullText;
            var nullTextItem = control.Items.FindByText(this.NullText);
            if (nullTextItem != null)
            {
                nullTextItem.Text = STR_NullText;
            }
        }

        protected override void OnValueStored()
        {
            base.OnValueStored();
            //Tạo mới Nơi phát hành nếu chưa tồn tại
            if (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.Edit)
            {
                ASPxComboBox control = this.DropDownEdit.Controls[0] as ASPxComboBox;

                if (control.Text == control.Value.ToString())
                {
                    Document doc = View.CurrentObject as Document;
                    UnitOfWork uow = new UnitOfWork(doc.Session.DataLayer);

                    DocumentSigneesOrganization currentOrganization = uow.FindObject<DocumentSigneesOrganization>(new BinaryOperator("Title", control.Text));
                    if (currentOrganization == null)
                    {
                        currentOrganization = new DocumentSigneesOrganization(uow);
                        currentOrganization.Title = control.Text;
                        currentOrganization.Save();
                        uow.CommitTransaction();
                    }
                    doc.DocOrganization = doc.Session.GetObjectByKey<DocumentSigneesOrganization>(currentOrganization.Oid);
                    doc.Save();
                    doc.Session.CommitTransaction();
                }
            }
        }
    }
}
