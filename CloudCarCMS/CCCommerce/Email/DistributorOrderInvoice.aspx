<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Email.Master" CodeBehind="DistributorOrderInvoice.aspx.vb" Inherits="CloudCar.CCCommerce.Email.DistributorOrderInvoice" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Content ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

<h1>Invoice - Order Number ( <asp:Literal runat="server" ID="litOrderID" /> )</h1>

<p style="width: 620px;">The following order requires shipment. Go to <asp:HyperLink runat="server" ID="hlDistributorOrders" /> for more details.</p>

<br />

<asp:DataGrid runat="server" ID="dgOrderItems" Width="100%" AutoGenerateColumns="false" DataKeyField="ID" 
        AllowSorting="true" GridLines="None" CellPadding="0" CellSpacing="0">
    <HeaderStyle CssClass="HeaderStyle" />
    <ItemStyle CssClass="LineItem" />
    <Columns>
        <asp:TemplateColumn HeaderText="Item(s)" ItemStyle-Width="15%">
            <ItemTemplate>
                <asp:Image ID="Image1" runat="server" ImageUrl='<%# String.Format("http://{0}/images/db/{1}/60/{2}.jpg", Settings.HostName, Eval("ImageID"), Eval("Name")) %>' style="float: left;" BorderWidth="6" BorderColor="White" /><br />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Description" ItemStyle-Width="50%">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# String.Format("{0}{1}", Eval("Name"), ApplicationFunctions.DisplaySizeColor(CStr(Eval("Color")), CStr(Eval("Size")))) %>' />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:BoundColumn HeaderText="Quantity" DataField="Quantity" ItemStyle-Width="17%" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
        <asp:BoundColumn HeaderText="Total" DataField="Total" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" ItemStyle-Width="20%" />
    </Columns>
</asp:DataGrid>

<br /><br />

<table width="100%">
    <tr>
        <td width="60%" valign="top">
            
            <h2>Details</h2>

            <p><label>Customer Information (Name, Email)</label><br />
            <asp:Label runat="server" ID="litCustomerInformation" /></p>

            <p><label>Billing Address</label><br />
            <asp:Label runat="server" ID="lblBillingAddress" /></p>

            <p><label>Shipping Address</label><br />
            <asp:Label runat="server" ID="lblShippingAddress" /></p>

            <p><label>Shipping Information (Company, Service, Wight, Fee)</label><br />
            <asp:Label runat="server" ID="lblShipCompany" /> - <asp:Label runat="server" ID="lblShipService" /> - <asp:Label runat="server" ID="lblWeight" /> - <asp:Label runat="server" ID="lblShipCharge" /></p>

            <p><label>Payment Information (Method, Authorization Code, Amount)</label><br />
            <asp:Label runat="server" ID="lblBillMethod" /> - <asp:Label runat="server" ID="lblAuthCode" /> - <asp:Label runat="server" ID="lblBillingAmount" /></p>

            <br />
        
        </td>
        <td width="40%" valign="top">
            
            <h2>Totals</h2>

            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td><label>Items</label></td>
                    <td align="right"><asp:Literal runat="server" ID="litOrderItems" /></td>
                </tr>
                <tr>
                    <td><label>Subtotal</label></td>
                    <td align="right"><asp:Literal runat="server" ID="litSubTotal" /></td>
                </tr>
                <tr>
                    <td><label style="color: Red;">Discount</label></td>
                    <td align="right" style="color: Red;"><asp:Literal runat="server" ID="litDiscount" /></td>
                </tr>
                <tr>
                    <td><label>Shipping</label></td>
                    <td align="right"><asp:Literal runat="server" ID="litShipCharge" /></td>
                </tr>
                <tr>
                    <td><label>Taxes (<asp:Literal runat="server" ID="litTaxRate" />)</label></td>
                    <td align="right"><asp:Literal runat="server" ID="litTaxAmount" /></td>
                </tr>
                <tr>
                    <td><label>Total</label></td>
                    <td align="right"><asp:Literal runat="server" ID="litTotal" /></td>
                </tr>
            </table>
            
        </td>
    </tr>
</table>

</asp:Content>
