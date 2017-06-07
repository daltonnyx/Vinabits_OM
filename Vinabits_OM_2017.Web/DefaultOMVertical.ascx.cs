using System;
using System.Collections.Generic;
using System.Web.UI;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Templates.ActionContainers;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.SystemModule;

namespace Vinabits_OM_2017.Web
{
    public partial class DefaultOMVertical : TemplateContent, IXafPopupWindowControlContainer, IXafSecurityActionContainerHolder, IHeaderImageControlContainer
    {
        static DefaultOMVertical()
        {
            AdditionalClass = "sizeLimit";
        }
        public static void ClearSizeLimit() { AdditionalClass = ""; }
        public static string AdditionalClass { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Page.ClientScript.RegisterClientScriptResource(typeof(WebWindow), "DevExpress.ExpressApp.Web.Resources.JScripts.XafNavigation.js");
            Page.ClientScript.RegisterClientScriptResource(typeof(WebWindow), "DevExpress.ExpressApp.Web.Resources.JScripts.XafFooter.js");
            Page.ClientScript.RegisterClientScriptResource(typeof(WebWindow), "DevExpress.ExpressApp.Web.Resources.JScripts.DefaultVerticalTemplate.js");
            if (WebWindow.CurrentRequestWindow != null)
            {
                WebWindow.CurrentRequestWindow.PagePreRender += new EventHandler(CurrentRequestWindow_PagePreRender);
            }

            IModelApplicationNavigationItems modelApplicationNavigationItems = (IModelApplicationNavigationItems)WebApplication.Instance.Model;
            bool showNavigationOnStart = ((IModelRootNavigationItemsWeb)modelApplicationNavigationItems.NavigationItems).ShowNavigationOnStart;
            if (!showNavigationOnStart && !navigation.CssClass.Contains("xafHidden"))
            {
                navigation.CssClass += " xafHidden";
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        private void CurrentRequestWindow_PagePreRender(object sender, EventArgs e)
        {
            WebWindow window = (WebWindow)sender;
            window.RegisterStartupScript("Init", "Init();");
        }
        protected override void OnUnload(EventArgs e)
        {
            if (WebWindow.CurrentRequestWindow != null)
            {
                WebWindow.CurrentRequestWindow.PagePreRender -= new EventHandler(CurrentRequestWindow_PagePreRender);
            }
            base.OnUnload(e);
        }
        public override void SetStatus(ICollection<string> statusMessages)
        {
        }
        public override IActionContainer DefaultContainer
        {
            get
            {
                if (mainMenu != null)
                {
                    return mainMenu.FindActionContainerById("View");
                }
                return null;
            }
        }
        public override object ViewSiteControl
        {
            get { return VSC; }
        }
        public XafPopupWindowControl XafPopupWindowControl
        {
            get { return PopupWindowControl; }
        }
        public override void BeginUpdate()
        {
            base.BeginUpdate();
            this.SAC.BeginUpdate();
            this.mainMenu.BeginUpdate();
            this.SearchAC.BeginUpdate();
        }
        public override void EndUpdate()
        {
            this.SAC.EndUpdate();
            this.mainMenu.EndUpdate();
            this.SearchAC.EndUpdate();
            base.EndUpdate();
        }
        ActionContainerHolder IXafSecurityActionContainerHolder.SecurityActionContainerHolder { get { return SAC; } }

        public ThemedImageControl HeaderImageControl { get { return TIC; } }
    }
}
