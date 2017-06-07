<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DepartmentUsercontrol.ascx.cs" Inherits="Vinabits_OM_2017.Web.UserControl.DepartmentUsercontrol" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v16.2, Version=16.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v16.2, Version=16.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.ExpressApp.Web.v16.2, Version=16.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.ExpressApp.Web" tagprefix="cc1" %>

<dx:ASPxTreeList ID="departmentTreeList" runat="server" Height="309px" Width="428px" AutoGenerateColumns="False" DataSourceID="dsDepartment">
    <Columns>
        <dx:TreeListTextColumn FieldName="Office" VisibleIndex="2">
        </dx:TreeListTextColumn>
        <dx:TreeListTextColumn FieldName="Title" VisibleIndex="1">
        </dx:TreeListTextColumn>
    </Columns>
    <settingsbehavior autoexpandallnodes="True" />
    <settingspager mode="ShowPager">
    </settingspager>
    <settingsselection enabled="True" />
    <settingsdatasecurity allowdelete="False" allowedit="False" allowinsert="False" />
</dx:ASPxTreeList>

<cc1:XafWebDesignDataSource ID="dsDepartment" runat="server" ObjectTypeName="Vinabits_OM_2017.Module.BusinessObjects.Department" />


