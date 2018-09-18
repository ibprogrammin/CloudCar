<%@ Page Title="View Distributor Orders" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="DistributorOrders.aspx.vb" Inherits="CloudCar.DistributorOrders" %>
<%@ Import Namespace="CloudCar.CCFramework.Commerce" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server" />

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <h1>Orders</h1>
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

    <asp:UpdatePanel runat="server" ID="upUpdate" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Always">
        <ContentTemplate>
        
    <table class="shopping-cart-details">
        <tr>
            <td>
                <asp:TextBox ID="txtSOrderNumber" runat="server" style="width: 60px;" />
            </td>
            <td>
                <asp:TextBox ID="txtSUserName" runat="server" style="width: 240px;" />
            </td>
            <td>
                <asp:DropDownList ID="ddlShipped" runat="server" AutoPostBack="false" style="width: 140px;">
                    <asp:ListItem Value="*" Text="Shipped" />
                    <asp:ListItem Value="0" Text="None" />
                    <asp:ListItem Value="1" Text="Partial" />
                    <asp:ListItem Value="2" Text="Complete" />
                </asp:DropDownList>
            </td>
            <td>
                <asp:LinkButton id="btnSearch" runat="server" CssClass="checkout-button" Text="Search" />
            </td>
        </tr>
    </table>

    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwOrderNum" TargetControlID="txtSOrderNumber" WatermarkText="#" />
    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwUserName" TargetControlID="txtSUserName" WatermarkText="Username" />

    <br /><asp:Label runat="server" ID="lblStatus" CssClass="status-message" /><br />
        
           
    <asp:DataGrid ID="gvOrders" runat="server" AutoGenerateColumns="False" DataKeyNames="id" PageSize="10" 
            AllowPaging="true" AllowSorting="True" Width="860px" GridLines="None" CssClass="" CellPadding="0" CellSpacing="0">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="SCPager" />
        <HeaderStyle CssClass="" />
        <Columns>
            <asp:TemplateColumn SortExpression="ID" HeaderText="Order">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("~/CCCommerce/Distributor/DistributorOrderDetails.aspx?Order={0}", Eval("ID"))  %>' Text='<%# String.Format("View ({0})", Eval("ID")) %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="OrderDate" HeaderText="Date" SortExpression="OrderDate" DataFormatString="{0:d}" />
            <asp:TemplateColumn HeaderText="User" SortExpression="User" >
                <ItemTemplate>
                    <asp:Literal runat="server" Text='<%# Eval("User") %>' /><br />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Authorized" SortExpression="ApprovalState">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# getApprovalState(CType(Eval("ApprovalState"),EApprovalState)) %>' />
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
            <div class="loading-box">
                <h3>Loading please wait...</h3>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
    <br class="clear-both" />

</asp:Content>
    
<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server"></asp:Content>