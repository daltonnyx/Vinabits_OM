<%@ Control Language="C#" CodeBehind="LogonNew.ascx.cs" ClassName="LogonNew" Inherits="Vinabits_OM_2017.Web.LogonNew" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v16.2, Version=16.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates.ActionContainers"
    TagPrefix="xaf" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v16.2, Version=16.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates.Controls"
    TagPrefix="xaf" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v16.2, Version=16.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Controls"
    TagPrefix="xaf" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v16.2, Version=16.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates"
    TagPrefix="xaf" %>
<%@ Register Assembly="DevExpress.Web.v16.2, Version=16.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<meta name="viewport" content="width=device-width, user-scalable=no, maximum-scale=1.0, minimum-scale=1.0">
<link href="asset/css/bootstrap.min.css" rel="stylesheet">
<link rel="stylesheet" href="asset/css/font-awesome.min.css">
<script type="asset/js/bootstrap.min.js"></script>
<script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.min.js"></script>
<style>
.alert {
    margin: 5px 20px;
    font-size: 11px;
    padding: 0px;
    border: 0px solid transparent;
    border-radius: 4px;
}

.form-signin
{
    max-width: 330px;
    padding: 15px;
    margin: 0 auto;
}
.form-signin .form-signin-heading, .form-signin .checkbox
{
    margin-bottom: 10px;
}
.form-signin .checkbox
{
    font-weight: normal;
}
.form-signin .form-control
{
    position: relative;
    font-size: 16px;
    height: auto;
    padding: 10px;
    -webkit-box-sizing: border-box;
    -moz-box-sizing: border-box;
    box-sizing: border-box;
}
.form-signin .form-control:focus
{
    z-index: 2;
}
.form-signin input[type="text"]
{
    margin-bottom: -1px;
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
}
.form-signin input[type="password"]
{
    margin-bottom: 10px;
    border-top-left-radius: 0;
    border-top-right-radius: 0;
}
.account-wall
{
    margin-top: 20px;
    padding: 40px 0px 20px 0px;
    background-color: #f7f7f7;
    -moz-box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.3);
    -webkit-box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.3);
    box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.3);
}
.login-title
{
    color: #555;
    font-size: 18px;
    font-weight: 400;
    display: block;
}
.profile-img
{
    width: 96px;
    height: 96px;
    margin: 0 auto 10px;
    display: block;
    -moz-border-radius: 50%;
    -webkit-border-radius: 50%;
    border-radius: 50%;
}
.need-help
{
    margin-top: 10px;
}
.new-account
{
    display: block;
    margin-top: 10px;
}</style>


<div class="container form-group" style="margin-top:30px">
<asp:Login ID="LoginUser" runat="server" EnableViewState="false" 
RenderOuterTable="false" 
    onauthenticate="LoginUser_Authenticate" 
    onloggedin="LoginUser_LoggedIn" 
    onloginerror="LoginUser_LoginError"
    >
    <LayoutTemplate>
        <asp:Panel ID="FrmLoginPanel" runat="server" DefaultButton="LoginButton">
        <div class="col-md-4 col-md-offset-4">
            <div style="width:100%; text-align:center;"><img src="Images/Logo.png" /></div>
            <div class="panel panel-default">

                <div class="panel-heading"><h3 class="panel-title"><strong>Sign in</strong>&nbsp; - OM 2017</h3>
                </div>

                <span class="alert alert-danger">
                    <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                </span>

                <div class="panel-body form-group">
                    <form data-toggle="validator" role="form" novalidate="true" id="FrmLoginUser">
                      <div class="form-group has-feedback">
                        <div style="margin-bottom: 12px" class="input-group form-group">
                            <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                            <asp:TextBox ID="UserName" runat="server" CssClass="form-control" placeholder="Username" data-focus="60px" required></asp:TextBox>
                        </div>
                                
                        <div style="margin-bottom: 12px" class="input-group form-group">
                            <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                            <asp:TextBox ID="Password" runat="server" CssClass="form-control passwordEntry" TextMode="Password" placeholder="Password" data-minlength="6" required></asp:TextBox>
                        </div>
                        <span class="glyphicon form-control-feedback" aria-hidden="true"></span>
                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" CssClass="btn btn-success" /> 
                    </div>
                    <hr style="margin-top:10px;margin-bottom:10px;" >
  
                    <div class="form-group">
                            <div style="font-size:85%">
                                Problem with your account, please call: 
                            <a href="#" onClick="$('#loginbox').hide(); $('#signupbox').show()">
                                05113 777779
                            </a>
                            </div>
                                    
                    </div> 
                </form>
            </div>
        </div>
    </div>
    </div>
    </asp:Panel>
    </LayoutTemplate>
</asp:Login>

</div>
<script type="asset/js/validator.min.js"></script>