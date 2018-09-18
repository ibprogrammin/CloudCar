<%@ Page Title="Order Details" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="OrderDetails.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.OrderDetails" %>

<asp:Content ContentPlaceHolderID="head" runat="server">

<style type="text/css">
    fieldset label { width: 120px; margin-right: 20px; }
    td label { width: 120px; margin: 0px;padding: 0px; }
    table td { font-size: 16px; height: 10px;vertical-align: top; padding: 5px; padding-left: 10px;  }
    table td h3 { padding: 0px !important; margin: 0px !important; padding-bottom: 5px !important; display: inline-block !important; }
    
    .cost-summary-table { float: right; width: 420px; margin-right: 50px; margin-top: 10px; }
    .cost-summary-table td { border-bottom: none; vertical-align: top; }
    .cost-summary-table td label { width: 160px; margin: 0px;padding: 0px; padding-top: 15px;}
    .cost-summary-table td.taxes { border-bottom: 1px solid #ddd; }
    .cost-summary-table .price { }
</style>

</asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

<h1 class="form-heading-style">
    Order <asp:Literal runat="server" ID="OrderIdLiteral" />
    <i class="icon-money"></i>
</h1>
<hr />

<div style="float: right; text-align: right; width: 400px; margin-right: 40px;">
    <asp:LinkButton runat="server" ID="ResendInvoiceButton" Text="Reissue Invoice" /> | <asp:LinkButton runat="server" ID="NotifyDistributorButton" Text="Notify Distributor" />
</div>

<asp:HyperLink runat="server" NavigateUrl="~/CCAdmin/Commerce/Orders.aspx" Text="&laquo; Back to Orders" style="clear: left; margin-left: 10px;" /><br /><br />


<div class="search-bar">
    <asp:TextBox ID="OrderAuthorizationNumberTextBox" runat="server" style="width: 120px;" />
    <ajax:TextBoxWatermarkExtender ID="tbwOAN" runat="server" TargetControlID="OrderAuthorizationNumberTextBox" WatermarkText="Auth #" />
    <asp:Button id="AuthorizeOrderButton" runat="server" CssClass="GreenButton" Text="Authorize" style="width: 190px;" />
    <asp:Button id="PrintOrderButton" runat="server" CssClass="OrangeButton" Text="Print" style="width: 170px;" />
    <asp:Button id="ReturnOrderButton" runat="server" CssClass="BlueButton" Text="Return" style="width: 180px; text-align: left;" OnClientClick="return confirm('Please note that you must manually process this return to the customer!');" />
    <asp:Button id="DeleteOrderButton" runat="server" CssClass="DeleteButton" Text="Delete" style="width: 180px;" OnClientClick="return confirm('Are you sure you want to delete this Order? This can't be undone!');" />
</div>

<asp:Label runat="server" ID="StatusMessageLabel" CssClass="status-message" Visible="False" />

<asp:HiddenField runat="server" ID="OrderIdHiddenField" />
    
<div class="tab-container">
	<ul class="tabs">
		<li class="tab"><a href="#tab-order">Order</a></li>
        <li class="tab"><a href="#tab-customer">Customer</a></li>
        <li class="tab"><a href="#tab-address">Address</a></li>
        <li class="tab"><a href="#tab-shipping">Shipping</a></li>
        <li class="tab"><a href="#tab-payment">Payment</a></li>
	</ul>
    <div id="tab-order">
        <asp:DataGrid ID="OrderItemsGridView" 
                runat="server" 
                AutoGenerateColumns="False" 
                DataKeyNames="id" 
                AllowPaging="False" 
                AllowSorting="False" 
                GridLines="None" 
                CssClass="default-table">
            <HeaderStyle CssClass="default-table-header" />
            <Columns>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="ShipButton" Text="Ship" OnCommand="ShipItemButtonCommand" CommandArgument='<%# Eval("ID") %>' CommandName="Ship" ForeColor="YellowGreen" Font-Bold="true" Visible='<%# CBool(Eval("Shipped")) <> true %>' /><br />
                        <asp:LinkButton runat="server" ID="DeleteButton" Text="Delete" OnCommand="DeleteItemButtonCommand" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" ForeColor="Red" Font-Bold="true" OnClientClick="return confirm('Are you sure you want to delete this Order Item? This can't be undone!');" /><br/>
                        <asp:LinkButton runat="server" ID="ReturnButton" Text='<%# if(CDec(Eval("Price")) < 0, "Charge", "Return") %>' OnCommand="ReturnItemButtonCommand" CommandArgument='<%# Eval("ID") %>' CommandName="Return" ForeColor="Blue" Font-Bold="true" OnClientClick="return confirm('Please note that you must manually process this return to the customer!');" />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn SortExpression="ID" HeaderText="Item(s)" HeaderStyle-CssClass="align-center" ItemStyle-CssClass="align-center">
                    <ItemTemplate>
                        <img runat="server" ID="ItemImage" src='<%# String.Format("/images/db/{0}/100/{1}.jpg", Eval("ImageID"), Eval("Name"))  %>' alt="" class="image-display-table" /><br />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Description" SortExpression="Name">
                    <ItemTemplate>
                        <asp:Literal runat="server" Text='<%# Eval("Name") %>' ID="litName" /><br />
                        <asp:Label runat="server" ID="ColorLabel" Text='<%# Eval("Colour") %>' /> / <asp:Label runat="server" ID="SizeLabel" Text='<%# Eval("Size") %>' /><br />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Tracking Number">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="ItemTrackingNumberTextBox" Width="200px" Visible='<%# CBool(Eval("Shipped")) <> true %>' style="position: relative; top: 10px;margin-bottom: -15px;" />
                        <ajax:TextBoxWatermarkExtender runat="server" ID="tbwItemTNumber" TargetControlID="ItemTrackingNumberTextBox" WatermarkText="Tracking #" />
                        <asp:Label runat="server" ID="ItemTrackingNumberLabel" Text='<%# String.Format("T# ( {0} )", Eval("TrackingNumber")) %>' Visible='<%# CBool(Eval("Shipped")) = true %>' />
                    </ItemTemplate>
                 </asp:TemplateColumn>
                <asp:BoundColumn DataField="Quantity" HeaderText="Qty." SortExpression="Quantity" HeaderStyle-CssClass="align-center" ItemStyle-CssClass="align-center" />
                <asp:BoundColumn DataField="UnitPrice" SortExpression="UnitPrice" HeaderText="Price" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                <asp:BoundColumn DataField="Price" SortExpression="Price" HeaderText="Total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
            </Columns>
        </asp:DataGrid>

        <table class="cost-summary-table default-table">
            <tr>
                <td><label>Sub Total</label></td>
                <td class="Price"><asp:Literal runat="server" ID="SubtotalLiteral" /></td>
            </tr>
            <tr>
                <td><label class="Discount">Discount</label></td>
                <td class="Price"><asp:Label runat="server" ID="DiscountLiteral" CssClass="Discount" /></td>
            </tr>
            <tr>
                <td><label>Shipping</label></td>
                <td class="Price"><asp:Literal runat="server" ID="SummaryShippingChargeLiteral" /></td>
            </tr>
            <tr>
                <td class="taxes"><label>Taxes ( <asp:Literal runat="server" ID="TaxRateLiteral" /> )</label></td>
                <td class="Price taxes"><asp:Literal runat="server" ID="TaxesLiteral" /></td>
            </tr>
            <tr>
                <td style="padding-top: 20px; padding-left: 10px;" class="Total">Total</td>
                <td style="padding-top: 30px;" class="Total Price"><asp:Label runat="server" ID="TotalLiteral" CssClass="TotalPrice" /></td>
            </tr>
        </table>
        
        <br class="clear-both" />
    </div>
    <div id="tab-customer">
        <table width="100%">
            <tr>
                <td colspan="2"><h3>Customer Information</h3></td>
            </tr>
            <tr>
                <td style="width: 80px;"><label>User</label></td>
                <td><asp:Hyperlink runat="server" ID="UserNameHyperLink" /></td>
            </tr>
            <tr>
                <td><label>Email</label></td>
                <td><asp:Hyperlink runat="server" ID="EmailAddressHyperLink" /></td>
            </tr>
            <tr>
                <td><label>Name</label></td>
                <td><asp:Literal runat="server" ID="NameLiteral" /></td>
            </tr>
            <tr>
                <td><label>Phone</label></td>
                <td><asp:Literal runat="server" ID="PhoneNumberLiteral" /></td>
            </tr>
        </table>
    </div>
    <div id="tab-address">
        <h3>Shipping Address (<asp:HyperLink runat="server" ID="EditShippingAddressHyperLink" Text="Edit" />)</h3><br />
        <p><asp:Literal runat="server" ID="ShippingAddressLiteral" /></p>
        
        <br />
        <h3>Billing Address (<asp:HyperLink runat="server" ID="EditBillingAddressHyperLink" Text="Edit" />)</h3><br />
        <p><asp:Literal runat="server" ID="BillingAddressLiteral" /></p>
    </div>
    <div id="tab-shipping">
        <table width="920">
            <tr>
                <td colspan="2" style="width: 520px;"><h3>Shipping Information</h3></td>
                <td><h3>Branch</h3></td>
            </tr>
            <tr>
                <td style="width: 80px;"><label>Company</label></td>
                <td><asp:Literal runat="server" ID="ShippingCompanyLiteral" /></td>
                <td><asp:Literal runat="server" ID="BranchInfoLiteral" /></td>
            </tr>
            <tr>
                <td><label>Service</label></td>
                <td><asp:Literal runat="server" ID="ShippingServiceLiteral" /></td>
                <td rowspan="2">
                    <asp:TextBox runat="server" ID="OrderTrackingNumberTextBox" Width="200px" />
                    <asp:Button id="ShipOrderButton" runat="server" CssClass="GreenButton" style="width: 216px; position: relative; margin-left: 10px;" Text="Ship Order" />
                    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwOTN" TargetControlID="OrderTrackingNumberTextBox" WatermarkText="Tracking Number" WatermarkCssClass="Watermark" />
                </td>
            </tr>
            <tr>
                <td><label>Charge</label></td>
                <td><asp:Literal runat="server" ID="ShippingChargeLiteral" /></td>
            </tr>
        </table>
    </div>
    <div id="tab-payment">
        <h3>Payment Information</h3><br />
    
        <label>Method</label>
        <asp:Label runat="server" ID="CardTypeLiteral" CssClass="display-message" style="width: 270px; float: left;" />
        <br class="clear-both" /><br />
    
        <label>Number (<asp:LinkButton runat="server" ID="ShowCreditCardButton" Text="Show" />)</label>
        <asp:Label runat="server" ID="CardNumberLiteral" CssClass="display-message" style="width: 270px;" />
        <br class="clear-both" /><br />
    
        <label>Charge Amount</label>
        <asp:Label runat="server" ID="CardChargeLiteral" CssClass="display-message" style="width: 270px;" />
        <br class="clear-both" /><br />
    
        <p><asp:Literal runat="server" ID="PaymentMessageLiteral" Visible="false" /></p>
        <br class="clear-both" />
    </div>
</div>



<fieldset ID="CreditCardPaymentInfoPlaceHolder" runat="server" style="width: 480px; float: left; margin-top: 20px; display: none;">
    
    
    
</fieldset>




<br class="clear-both" /><br />

<asp:Repeater runat="server" ID="AvailableDownloadsRepeater" Visible="False">
    <HeaderTemplate>
        <br class="clear-both" />
        <h2 class="">Available Downloads</h2><br />
        <table class="default-table">
            <thead>
                <td>Product</td>
                <td>Link</td>
                <td>File</td>
                <td style="text-align: right;">Downloads</td>
            </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><%# Eval("Name")%></td>
            <td><a href='<%# Eval("Link") %>' title='<%# Eval("Filename") %>'><%# Eval("Link") %></a></td>
            <td><%# Eval("FileName")%></td>
            <td style="text-align: right;"><%# Eval("Downloads")%></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>

</asp:Content>