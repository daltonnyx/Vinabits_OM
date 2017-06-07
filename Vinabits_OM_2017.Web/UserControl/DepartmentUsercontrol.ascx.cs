using DevExpress.ExpressApp.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.ExpressApp;
using Vinabits_OM_2017.Module.BusinessObjects;

namespace Vinabits_OM_2017.Web.UserControl
{
    public partial class DepartmentUsercontrol : System.Web.UI.UserControl, IComplexControl
    {
        public void Refresh()
        {
            //throw new NotImplementedException();
        }

        public void Setup(IObjectSpace objectSpace, XafApplication application)
        {
            departmentTreeList.DataSourceID = string.Empty;
            departmentTreeList.DataSource = objectSpace.GetObjects<Department>();
            departmentTreeList.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

    }
}