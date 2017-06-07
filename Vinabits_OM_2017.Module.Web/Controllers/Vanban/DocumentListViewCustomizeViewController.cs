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
using DevExpress.ExpressApp.Web.Editors.ASPx;
using Vinabits_OM_2017.Module.BusinessObjects;
using DevExpress.Web;
using System.Web.UI.WebControls;
using DevExpress.ExpressApp.Model;

namespace Vinabits_OM_2017.Module.Web.Controllers
{ 
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DocumentListViewCustomizeViewController : ViewController<ListView>
    {
        public DocumentListViewCustomizeViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Document);
            TargetViewNesting = Nesting.Root;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            ASPxGridListEditor gridListEditor = View.Editor as ASPxGridListEditor;
            if(gridListEditor != null)
            {
                ASPxGridView gridView = gridListEditor.Grid;
                gridView.Settings.UseFixedTableLayout = true;
                gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                gridView.Width = Unit.Percentage(100);
                gridView.Style.Add("min-width", "1024px");
                foreach (WebColumnBase column in gridView.Columns)
                {
                    IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                    if (columnInfo != null)
                    {
                        IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                        if (columnInfo.Model.Index == -1)
                            column.Width = Unit.Percentage(0);
                        else
                            column.Width = Unit.Percentage(modelColumn.Width);
                    }
                }
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
