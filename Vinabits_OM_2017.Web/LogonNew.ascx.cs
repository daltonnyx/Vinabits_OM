using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Utils;

namespace Vinabits_OM_2017.Web
{
    public partial class LogonNew : TemplateContent //, IXafPopupWindowControlContainer
    {
        public override void SetStatus(ICollection<string> statusMessages)
        {
        }
        public override IActionContainer DefaultContainer
        {
            get { return null; }
        }
        public override object ViewSiteControl
        {
            get {
                //return viewSiteControl;
                return null;
            }
        }

        /*
        public XafPopupWindowControl XafPopupWindowControl
        {
            get { return PopupWindowControl; }
        }
        public override void BeginUpdate()
        {
            base.BeginUpdate();
            this.PopupActions.BeginUpdate();
        }
        public override void EndUpdate()
        {
            this.PopupActions.EndUpdate();
            base.EndUpdate();
        }*/

        Exception lastLoginException = null;
        protected void LoginUser_Authenticate(object sender, System.Web.UI.WebControls.AuthenticateEventArgs e)
        {
            lastLoginException = null;
            bool authenticated = false;
            try
            {
                using (IObjectSpace os = WebApplication.Instance.CreateObjectSpace())
                {
                    /*
                    AuthenticationStandard authentication = (AuthenticationStandard)((SecurityStrategyBase)WebApplication.Instance.Security).Authentication;
                    Guard.ArgumentNotNull(authentication, "authentication");
                    */
                    /*
                    AuthenticationStandardLogonParameters logonParameters = (AuthenticationStandardLogonParameters)authentication.LogonParameters;
                    Guard.ArgumentNotNull(logonParameters, "logonParameters");
                    */
                    /*
                    AuthenticationStandardLogonParameters logonParameters = SecuritySystem.LogonParameters as AuthenticationStandardLogonParameters;
                    logonParameters.UserName = LoginUser.UserName;
                    logonParameters.Password = LoginUser.Password;
                    
                    authenticated = authentication.Authenticate(os) != null;//Dennis: You can use a custom authentication algorithm here;
                    */
                    if (!SecuritySystem.IsAuthenticated)
                    {
                        AuthenticationStandardLogonParameters parameters = SecuritySystem.LogonParameters as AuthenticationStandardLogonParameters;
                        parameters.UserName = LoginUser.UserName;
                        parameters.Password = LoginUser.Password;
                        try
                        {
                            ((Vinabits_OM_2017AspNetApplication)WebApplication.Instance).Logon();
                        }
                        catch (Exception ex)
                        {
                            //this.Error.Text = ex.Message;
                            LoginUser.FailureText = ex.Message;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                authenticated = false;
                lastLoginException = exp;
            }
            e.Authenticated = authenticated;
        }
        protected void LoginUser_LoggedIn(object sender, EventArgs e)
        {
            //WebApplication.Instance.Start();
            //OR
            using (IObjectSpace os = WebApplication.Instance.CreateObjectSpace())
            {
                SecuritySystem.Logon(os); //Da load o tren
                
            }
        }
        protected void LoginUser_LoginError(object sender, EventArgs e)
        {
            if (lastLoginException != null)
            {
                //DoSomething(lastLoginException);
                if(LoginUser != null)
                    LoginUser.FailureText = "Login Failed!";
            }
        }

    }

    public class AuthenticationStandardForFullyCustomLogin : AuthenticationStandard
    {
        public override bool AskLogonParametersViaUI
        {
            get
            {
                return false;
            }
        }
    }
}
