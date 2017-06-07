<%@ Control Language="C#" CodeBehind="DefaultOM.ascx.cs" ClassName="DefaultOM" Inherits="Vinabits_OM_2017.Web.DefaultOM"%>
<%@ Register Assembly="DevExpress.Web.v16.2, Version=16.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v16.2, Version=16.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates.ActionContainers"
    TagPrefix="cc2" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v16.2, Version=16.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates"
    TagPrefix="cc3" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v16.2, Version=16.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Controls"
    TagPrefix="cc4" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v16.2, Version=16.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.ExpressApp.Web.Templates.Controls"
    TagPrefix="tc" %>
<link href="asset/css/bootstrap.min.css" rel="stylesheet">
<link href="asset/css/template.css" rel="stylesheet">
<link rel="stylesheet" href="asset/css/font-awesome.min.css">
<script type="asset/js/bootstrap.min.js"></script>
<script type="text/javascript" src="//code.jquery.com/jquery-1.10.2.min.js"></script>

<div class="HorizontalTemplate BodyBackColor">
     <cc3:XafUpdatePanel ID="UPPopupWindowControl" runat="server">
        <cc4:XafPopupWindowControl runat="server" ID="PopupWindowControl" />     
    </cc3:XafUpdatePanel>
    <dx:ASPxGlobalEvents ID="GE" ClientSideEvents-EndCallback="AdjustSize" runat="server" />
    <table id="MT" border="0" width="100%" cellpadding="0" cellspacing="0" class="dxsplControl_<%= BaseXafPage.CurrentTheme %>">
        <tbody>
            <tr>
                <td style="vertical-align: top; height: 10px;" class="dxsplPane_<%= BaseXafPage.CurrentTheme %>">
                    <div id="HorizontalTemplateHeader" class="HorizontalTemplateHeader" style="width: 100%;">
                        <table cellpadding="0" cellspacing="0" border="0" class="Top" width="100%">
                            <tr>
                                <td class="Logo">
                                    <asp:HyperLink runat="server" NavigateUrl="#" ID="LogoLink">
                                        <cc4:ThemedImageControl ID="TIC" ImageName="Logo.png" DefaultThemeImageLocation="Images"
                                            runat="server" BorderWidth="0px" />
                                    </asp:HyperLink>
                                </td>
                                <td class="Security">
                                    <cc3:XafUpdatePanel ID="UPSAC" runat="server" UpdatePanelForASPxGridListCallback="False">
                                        <cc2:ActionContainerHolder runat="server" ID="SAC" ContainerStyle="Links" CssClass="Security" ShowSeparators="True" >
                                            <ActionContainers>
                                                <cc2:WebActionContainer ContainerId="Notifications" IsDropDown="false" />
                                                <cc2:WebActionContainer ContainerId="Security" IsDropDown="false" />
                                            </ActionContainers>
                                        </cc2:ActionContainerHolder>
                                    </cc3:XafUpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <cc3:XafUpdatePanel ID="UPNTAC" UpdatePanelForASPxGridListCallback="False" runat="server">
                            <cc2:NavigationTabsActionContainer ID="NTAC" runat="server" ContainerId="ViewsNavigation"
                                CssClass="NavigationTabsActionContainer">
                                <spaceaftertabstemplate>
                                    <cc2:ActionContainerHolder ID="VN" runat="server" ContainerStyle="Links" CssClass="TabsContainer">
                                        <ActionContainers>
                                            <cc2:WebActionContainer ContainerId="RootObjectsCreation" IsDropDown="false" />
                                            <cc2:WebActionContainer ContainerId="Appearance" IsDropDown="false" />
                                            <cc2:WebActionContainer ContainerId="Search" IsDropDown="false" />
                                            <cc2:WebActionContainer ContainerId="FullTextSearch" IsDropDown="false" />
                                        </ActionContainers>
                                    </cc2:ActionContainerHolder>
                                </spaceaftertabstemplate>
                            </cc2:NavigationTabsActionContainer>
                        </cc3:XafUpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <table id="MRC" style="width: 100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td id="LPcell" style="min-width: 0px; width: 200px; vertical-align: top; display:none;">
                                <div id="LP" class="LeftPane">
                                    <cc3:XafUpdatePanel ID="UPLP" runat="server">
                                        <cc2:ActionContainerHolder ID="VTC" runat="server" Orientation="Vertical"
                                            BorderWidth="0px" ContainerStyle="Links" ShowSeparators="False" >
                                            <ActionContainers>
                                                <cc2:WebActionContainer ContainerId="Tools" IsDropDown="false" />
                                            </ActionContainers>
                                        </cc2:ActionContainerHolder>
                                        <cc2:ActionContainerHolder ID="DAC" runat="server" Orientation="Vertical"
                                            BorderWidth="0px" ContainerStyle="Links" ShowSeparators="False" >
                                            <ActionContainers>
                                                <cc2:WebActionContainer ContainerId="Diagnostic" IsDropDown="false" />
                                            </ActionContainers>
                                        </cc2:ActionContainerHolder>
                                        <br />
                                    </cc3:XafUpdatePanel>
                                </div>
                            </td>
                            <td id="separatorCell" style="width: 6px; border-bottom-style: none; border-top-style: none; display: none"
                                class="dxsplVSeparator_<%= BaseXafPage.CurrentTheme %> dxsplPane_<%= BaseXafPage.CurrentTheme %>">
                                <div id="separatorButton" class="dxsplVSeparatorButton_<%= BaseXafPage.CurrentTheme %>" onmouseover="OnMouseEnter('separatorButton')"
                                    onmouseout="OnMouseLeave('separatorButton')" onclick="OnClick('LPcell', 'separatorImage', true)">
                                    <div id="separatorImage" style="width: 6px;" class="dxWeb_splVCollapseBackwardButton_<%= BaseXafPage.CurrentTheme %>">
                                    </div>
                                </div>
                            </td>
                            <td style="vertical-align: top;">
                                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <cc3:XafUpdatePanel ID="UPTB" runat="server">
                                                    <cc2:ActionContainerHolder CssClass="ACH MainToolbar" runat="server" ID="TB" ContainerStyle="ToolBar" Orientation="Horizontal">
                                                        <Menu Width="100%" ItemAutoWidth="False" ClientInstanceName="mainMenu">
                                                            <BorderTop BorderStyle="None" />
                                                            <BorderLeft BorderStyle="None" />
                                                            <BorderRight BorderStyle="None" />
                                                        </Menu>
                                                        <ActionContainers>
                                                            <cc2:WebActionContainer ContainerId="ObjectsCreation" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="Edit" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="RecordEdit" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="View" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="Export" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="Reports" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="Filters" IsDropDown="false" />                                                            
                                                        </ActionContainers>
                                                    </cc2:ActionContainerHolder>
                                                </cc3:XafUpdatePanel>
                                                <cc3:XafUpdatePanel ID="UPVH" runat="server" UpdatePanelForASPxGridListCallback="False">
                                                    <table id="VH" border="0" cellpadding="0" cellspacing="0" class="MainContent" width="100%">
                                                        <tr>
                                                            <td class="ViewHeader">
                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="ViewHeader">
                                                                    <tr>
                                                                        <td class="ViewImage">
                                                                            <cc4:ViewImageControl ID="VIC" runat="server" />
                                                                        </td>
                                                                        <td class="ViewCaption">
                                                                            <h1>
                                                                                <cc4:ViewCaptionControl ID="VCC" runat="server" />
                                                                            </h1>
                                                                            <cc2:NavigationHistoryActionContainer ID="VHC" runat="server" CssClass="NavigationHistoryLinks"
                                                                                ContainerId="ViewsHistoryNavigation" Delimiter=" / " />
                                                                        </td>
                                                                        <td align="right">
                                                                            <cc2:ActionContainerHolder runat="server" ID="RNC" ContainerStyle="Links" Orientation="Horizontal"
                                                                                UseLargeImage="True" CssClass="RecordsNavigationContainer">
                                                                                <Menu Width="100%" ItemAutoWidth="False" HorizontalAlign="Right" />
                                                                                <ActionContainers>
                                                                                    <cc2:WebActionContainer ContainerId="RecordsNavigation" IsDropDown="false" />
                                                                                </ActionContainers>
                                                                            </cc2:ActionContainerHolder>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </cc3:XafUpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <cc3:XafUpdatePanel ID="UPEMA" runat="server" UpdatePanelForASPxGridListCallback="False">
                                                    <cc2:ActionContainerHolder runat="server" ID="EMA" ContainerStyle="Links" Orientation="Horizontal" CssClass="EditModeActions">
                                                        <Menu Width="100%" ItemAutoWidth="False" HorizontalAlign="Right" />
                                                        <ActionContainers>
                                                            <cc2:WebActionContainer ContainerId="Save" IsDropDown="false" />
                                                            <cc2:WebActionContainer ContainerId="UndoRedo" IsDropDown="false" />
                                                        </ActionContainers>
                                                    </cc2:ActionContainerHolder>
                                                </cc3:XafUpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="CP" style="overflow: auto; width: 100%;">
                                                    <table border="0" cellpadding="0" cellspacing="0" class="MainContent" width="100%">
                                                        <tr class="Content">
                                                            <td class="Content">
                                                                <cc3:XafUpdatePanel ID="UPEI" runat="server" UpdatePanelForASPxGridListCallback="False">
                                                                    <tc:ErrorInfoControl ID="ErrorInfo" Style="margin: 10px 0px 10px 0px" runat="server">
                                                                    </tc:ErrorInfoControl>
                                                                </cc3:XafUpdatePanel>
                                                                <cc3:XafUpdatePanel ID="UPVSC" runat="server" UpdatePanelForASPxGridListCallback="False">
                                                                    <cc4:ViewSiteControl ID="VSC" runat="server" />
                                                                    <cc2:ActionContainerHolder runat="server" ID="EditModeActions2" ContainerStyle="Links"
                                                                        Orientation="Horizontal" CssClass="EditModeActions">
                                                                        <Menu Width="100%" ItemAutoWidth="False" HorizontalAlign="Right" Paddings-PaddingTop="15px" />
                                                                        <ActionContainers>
                                                                            <cc2:WebActionContainer ContainerId="Save" IsDropDown="false" />
                                                                            <cc2:WebActionContainer ContainerId="UndoRedo" IsDropDown="false" />
                                                                        </ActionContainers>
                                                                    </cc2:ActionContainerHolder>
                                                                </cc3:XafUpdatePanel>
                                                                <div id="Spacer" class="Spacer">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <cc3:XafUpdatePanel ID="UPQC" runat="server" UpdatePanelForASPxGridListCallback="False">
                                                                    <cc2:QuickAccessNavigationActionContainer CssClass="Links NavigationLinks" ID="QC" runat="server"
                                                                        ContainerId="ViewsNavigation" PaintStyle="Caption" ShowSeparators="True" />
                                                                </cc3:XafUpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 20px; vertical-align: bottom" class="BodyBackColor">
                    <cc3:XafUpdatePanel ID="UPIMP" runat="server" UpdatePanelForASPxGridListCallback="False">
                        <asp:Literal ID="InfoMessagesPanel" runat="server" Text="" Visible="False"></asp:Literal>
                    </cc3:XafUpdatePanel>
                    <div id="Footer" class="Footer">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <div class="FooterCopyright">
                                        <cc4:AboutInfoControl ID="AIC" runat="server">Copyright text</cc4:AboutInfoControl>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</div>
