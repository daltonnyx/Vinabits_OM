using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web;
using DevExpress.Web;
using System.ServiceModel;
using DevExpress.ExpressApp.Security.ClientServer.Wcf;
using DevExpress.ExpressApp.Security.ClientServer;

namespace Vinabits_OM_2017.Web {
    public class Global : System.Web.HttpApplication {
        public Global() {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e) {
			SecurityAdapterHelper.Enable(DevExpress.ExpressApp.Security.Adapters.ReloadPermissionStrategy.CacheOnFirstAccess); //Vinabits: them firstAccess
            ASPxWebControl.CallbackError += new EventHandler(Application_Error);
			WebApplication.EnableMultipleBrowserTabsSupport = true;
        }
        protected void Session_Start(Object sender, EventArgs e) {
            Tracing.LogName = "C:\\Logs\\OMlogfile";
            Tracing.Initialize();
            WebApplication.SetInstance(Session, new Vinabits_OM_2017AspNetApplication());
            //WebApplication.Instance.SwitchToNewStyle(); //!!!!!!!!!!!
            #region Wcf service
            //         string connectionString = string.Format("net.tcp://{0}:2551/OMServer", ConfigurationManager.AppSettings["ServerDBIP"]);
            //WcfSecuredDataServerClient clientDataServer = new WcfSecuredDataServerClient(
            //	WcfDataServerHelper.CreateNetTcpBinding(), new EndpointAddress(connectionString));
            //Session["DataServerClient"] = clientDataServer;
            //ServerSecurityClient securityClient = 
            //	new ServerSecurityClient(clientDataServer, new ClientInfoFactory());
            //         securityClient.IsSupportChangePassword = true;

            //         WebApplication.Instance.Security = securityClient;
            //WebApplication.Instance.CreateCustomObjectSpaceProvider += 
            //	delegate(object sender2, CreateCustomObjectSpaceProviderEventArgs e2) {
            //	e2.ObjectSpaceProvider = new DataServerObjectSpaceProvider(clientDataServer, securityClient);
            //};
            #endregion
            DevExpress.ExpressApp.Web.Templates.DefaultVerticalTemplateContentNew.ClearSizeLimit();
            ////Đổi giao diện cho OM
            WebApplication.Instance.Settings.DefaultVerticalTemplateContentPath = "DefaultOMVertical.ascx"; 
            //if (!ConfigurationManager.AppSettings.AllKeys.Contains("LogonNew")) 
            WebApplication.Instance.Settings.LogonTemplateContentPath = "LogonNew.ascx";

            WebApplication.Instance.SwitchToNewStyle();
            #region DB connect
            if(ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
            if(System.Diagnostics.Debugger.IsAttached && WebApplication.Instance.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
            #endregion
            WebApplication.Instance.Setup();
			WebApplication.Instance.Start();
		}
        protected void Application_BeginRequest(Object sender, EventArgs e) {
        }
        protected void Application_EndRequest(Object sender, EventArgs e) {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
        }
        protected void Application_Error(Object sender, EventArgs e) {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        protected void Session_End(Object sender, EventArgs e) {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
			//WcfSecuredDataServerClient clientDataServer = 
			//	(WcfSecuredDataServerClient)Session["DataServerClient"];
			//if (clientDataServer != null) {
			//	clientDataServer.Close();
			//	Session["DataServerClient"] = null;
			//}
        }
        protected void Application_End(Object sender, EventArgs e) {
        }
        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
        }
        #endregion
    }
}
