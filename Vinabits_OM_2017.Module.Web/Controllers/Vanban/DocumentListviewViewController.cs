using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Vinabits_OM_2017.Module.BusinessObjects;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using DevExpress.Data;

namespace Vinabits_OM_2017.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DocumentListviewViewController : ViewController
    {
        public DocumentListviewViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Document);
            TargetViewType = ViewType.ListView;
            TargetViewNesting = Nesting.Root;
        }
        
        protected override void OnActivated()
        {
            base.OnActivated();
            View.SelectionChanged += View_SelectionChanged;
            this.actReadMark.Execute += ActReadMark_Execute;
            this.actUnreadMark.Execute += ActUnreadMark_Execute;
            this.actFeatureUnmark.Execute += ActFeatureUnmark_Execute;
            this.actFeatureMarked.Execute += ActFeatureMarked_Execute;
            refreshActionState();
        }

        private void ActUnreadMark_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ReadUnread(false, e);
        }

        private void ActReadMark_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ReadUnread(true, e);
        }

        private void ActFeatureUnmark_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            FeatureMarked(false, e);
        }

        private void View_SelectionChanged(object sender, EventArgs e)
        {
            refreshActionState();
        }

        private void refreshActionState()
        {
            if (View != null && View.SelectedObjects != null && View.SelectedObjects.Count > 0)
            {
                this.actFeatureMarked.Enabled.SetItemValue("buttonStateFeature", true);
                this.actFeatureUnmark.Enabled.SetItemValue("buttonStateFeatureUnMark", true);
                this.actReadMark.Enabled.SetItemValue("buttonStateRead", true);
                this.actUnreadMark.Enabled.SetItemValue("buttonStateUnread", true);
            }
            else
            {
                this.actFeatureMarked.Enabled.SetItemValue("buttonStateFeature", false);
                this.actFeatureUnmark.Enabled.SetItemValue("buttonStateFeatureUnMark", false);
                this.actReadMark.Enabled.SetItemValue("buttonStateRead", false);
                this.actUnreadMark.Enabled.SetItemValue("buttonStateUnread", false);
            }
        }

        private void ActFeatureMarked_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            FeatureMarked(true, e);
        }

        private void FeatureMarked(bool isFeature, SimpleActionExecuteEventArgs e)
        {
            Employee curEmp = SecuritySystem.CurrentUser as Employee;
            foreach (Document selectDoc in e.SelectedObjects)
            {
                CriteriaOperator crit = CriteriaOperator.And(new BinaryOperator("LinkEmployee.Oid", curEmp.Oid), new BinaryOperator("LinkDocument.Oid", selectDoc.Oid));
                DocumentEmployees docEmp = View.ObjectSpace.FindObject<DocumentEmployees>(crit);
                if (docEmp != null)
                {
                    //The Action's state is updated when objects in Views change their values. This work is done by the ActionsCriteriaViewController
                    docEmp.IsFollow = isFeature;
                    docEmp.Save();
                    docEmp.Session.CommitTransaction();
                    ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (listEditor != null)
                        listEditor.UnselectAll();
                    //View.SelectedObjects.Clear();
                    //View.Refresh();
                }
            }
        }

        private void ReadUnread(bool isRead, SimpleActionExecuteEventArgs e)
        {
            Employee curEmp = SecuritySystem.CurrentUser as Employee;
            foreach (Document selectDoc in e.SelectedObjects)
            {
                CriteriaOperator crit = CriteriaOperator.And(new BinaryOperator("LinkEmployee.Oid", curEmp.Oid), new BinaryOperator("LinkDocument.Oid", selectDoc.Oid));
                DocumentEmployees docEmp = View.ObjectSpace.FindObject<DocumentEmployees>(crit);
                if (docEmp != null)
                {
                    //The Action's state is updated when objects in Views change their values. This work is done by the ActionsCriteriaViewController
                    docEmp.DateRead = isRead ? (docEmp.DateRead > DateTime.MinValue ? docEmp.DateRead : DateTime.Now): DateTime.MinValue;
                    docEmp.Save();
                    docEmp.Session.CommitTransaction();
                    ASPxGridListEditor listEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (listEditor != null)
                        listEditor.UnselectAll();
                    //View.SelectedObjects.Clear();
                    //View.Refresh();
                }
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            // Thêm số thứ tự
            ListView dv = (ListView)View;
            ASPxGridListEditor gridListEditor = dv.Editor as ASPxGridListEditor;
            if (gridListEditor != null)
            {
                ASPxGridView gridView = gridListEditor.Grid;
                // reset the VisibleIndxe start from 1, 0 will be use for ItemNo      
                for (int j = gridView.VisibleColumns.Count - 1; j >= 0; j--)
                {
                    if (gridView.VisibleColumns[j].VisibleIndex > 0)
                    {
                        gridView.VisibleColumns[j].VisibleIndex = gridView.VisibleColumns[j].VisibleIndex + 1;
                    }
                }
                // to add the ItemNo for each row                 
                if (gridView.Columns["itemNo"] == null)
                {
                    GridViewDataTextColumn itemNo = new GridViewDataTextColumn();
                    itemNo.Caption = "STT";
                    itemNo.ReadOnly = true;
                    itemNo.UnboundType = UnboundColumnType.String;
                    itemNo.Width = 32;
                    gridView.Columns.Add(itemNo);
                    itemNo.VisibleIndex = 1;
                }
                gridView.CustomColumnDisplayText += GridView_CustomColumnDisplayText; ;
            }
        }

        private void GridView_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.Caption == "STT")
            {
                ASPxGridView gridView = (ASPxGridView)sender;
                e.DisplayText = (e.VisibleIndex + 1).ToString();
            }
        }

        protected override void OnDeactivated()
        {
            this.actReadMark.Execute -= ActReadMark_Execute;
            this.actUnreadMark.Execute -= ActUnreadMark_Execute;
            this.actFeatureMarked.Execute -= ActFeatureMarked_Execute;
            this.actFeatureUnmark.Execute -= ActFeatureUnmark_Execute;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
