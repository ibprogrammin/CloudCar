<%@ Page Title="View Store Orders" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Orders.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Orders" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>
<%@ Import Namespace="CloudCar.CCFramework.Commerce" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Orders
        <i class="icon-money"></i>
    </h1>
    <hr />

    <asp:UpdatePanel runat="server" ID="upUpdate" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Always">
        <ContentTemplate>
        
    <div class="search-bar">
        <asp:TextBox ID="txtSOrderNumber" runat="server" style="width: 60px;" />
        <asp:TextBox ID="txtSUserName" runat="server" style="width: 240px;" />
        <asp:DropDownList ID="ddlApproved" runat="server" AutoPostBack="false" style="width: 200px;">
            <asp:ListItem Value="*" Text="Authorization" />
            <asp:ListItem Value="0" Text="Pending" />
            <asp:ListItem Value="1" Text="Approved" />
            <asp:ListItem Value="2" Text="Declined" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlShipped" runat="server" AutoPostBack="false" style="width: 180px;">
            <asp:ListItem Value="*" Text="Shipped" />
            <asp:ListItem Value="0" Text="None" />
            <asp:ListItem Value="1" Text="Partial" />
            <asp:ListItem Value="2" Text="Complete" />
        </asp:DropDownList>
        <asp:Button id="btnSearch" runat="server" CssClass="OrangeButton" style="width: 140px;" Text="Search" />
    </div>

    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwOrderNum" TargetControlID="txtSOrderNumber" WatermarkText="#" />
    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwUserName" TargetControlID="txtSUserName" WatermarkText="Username" />

    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" />
           
    <asp:DataGrid ID="gvOrders" 
            runat="server" 
            AutoGenerateColumns="False" 
            DataKeyNames="id" 
            PageSize="10" 
            AllowPaging="true" 
            AllowSorting="True" 
            GridLines="None" 
            CssClass="default-table">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn SortExpression="ID" HeaderText="Order">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("~/CCAdmin/Commerce/OrderDetails.aspx?Order={0}", Eval("ID"))  %>' Text='<%# String.Format("View ({0})",Eval("ID")) %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="OrderDate" HeaderText="Date" SortExpression="OrderDate" DataFormatString="{0:d}" />
            <asp:TemplateColumn HeaderText="User" SortExpression="User" >
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("~/CCAdmin/Users.aspx?User={0}", Eval("UserID")) %>' Text='<%# Eval("User") %>' Enabled='<%# RegisteredUserController.IsUserRegistered(CInt(Eval("UserID"))) %>' /><br />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Authorized" SortExpression="ApprovalState">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# getApprovalState(CType(Eval("ApprovalState"), EApprovalState)) %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Shipped" SortExpression="Shipped">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# getShippedState(CInt(Eval("Shipped"))) %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Items" HeaderText="Item(s)" SortExpression="Items" />
            <asp:BoundColumn DataField="Total" DataFormatString="{0:c}" SortExpression="Total" HeaderText="Total" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
        </Columns>
    </asp:DataGrid>

    <br />
    <br />
    
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress AssociatedUpdatePanelID="upUpdate" runat="server" ID="upProgress" DynamicLayout="false" >
        <ProgressTemplate>
            <div style="display: table-cell; position: fixed; top: 50%; left: 40%; width: 400px; height: 80px; background-color: #FFF; border: 1px solid #F5F5F5; vertical-align: middle;"><h3 class="BoldDark" style="text-align: center; position: relative; top: 32px;">Loading please wait...</h3></div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>