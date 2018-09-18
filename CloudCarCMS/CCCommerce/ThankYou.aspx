<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="ThankYou.aspx.vb" Inherits="CloudCar.CCCommerce.ThankYou" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <h1><asp:Literal runat="server" ID="FirstNameLiteral" />, Thank You For Purchasing! <span class="float-right" style="margin-right: 65px;">#<asp:Literal runat="server" ID="OrderIdLiteral" /></span></h1>
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    <Store:ShoppingProgressControl ID="ShoppingProgressControl1" runat="server" Progress="ThankYou" />
    
    <div class="checkout-summary-container">
        <h2>Your Details</h2>
        <p>
            <asp:Literal runat="server" ID="CustomerInformationLiteral" />
        </p><br />
    </div>
    
    <div class="checkout-info-container" style="border-bottom: none;">
        <p>
            Your purchase has been sent to our warehouse and will be processed immediately. You will
            get a copy of the invoice sent to your email address shortly. If you notice any discrepancies 
            with the following details, please contact us at <asp:HyperLink runat="server" ID="hlSupportEmail" />.
        </p><br />
        <p>
            We appreciate your time today and once again <b style="color: #FF0023;">Thank You</b>.
        </p>
    
    </div>
    
    <br class="clear-both" />

    <h3 style="margin-left: 10px;">Summary - <asp:Literal runat="server" ID="OrderItemsLiteral" /> Item(s)</h3>

    <asp:Repeater runat="server" ID="OrderItemsRepeater">
        <HeaderTemplate>
            <table class="shopping-cart-table">
                <thead>
                    <td>Item(s)</td>
                    <td>Description</td>
                    <td class="align-right">Quantity</td>
                    <td class="align-right">Total</td>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Image runat="server" ImageUrl='<%# String.Format("/images/db/{1}/60/{2}.jpg", Settings.HostName, Eval("ImageID"), Eval("Name")) %>' style="float: left; padding: 6px; background-color: #fff;" /><br />
                </td>
                <td>
                    <asp:Label runat="server" Text='<%# String.Format("{0} {1}", Eval("Name"), ApplicationFunctions.DisplaySizeColor(CStr(Eval("Color")), CStr(Eval("Size")))) %>' />
                </td>
                <td class="align-right"><%# Eval("Quantity")%></td>
                <td class="align-right"><span class="price-label"><%# String.Format("{0:C}", Eval("Total"))%></span></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    
    <div class="checkout-summary-container">
        <table>
            <tr>
                <td><b>Subtotal</b></td>
                <td class="align-right"><asp:Literal runat="server" ID="SubTotalLiteral" /></td>
            </tr>
            <tr>
                <td><b>Shipping</b></td>
                <td class="align-right"><asp:Literal runat="server" ID="ShippingChargeLiteral" /></td>
            </tr>
            <tr>
                <td class="discount-label"><b>Discount</b></td>
                <td class="align-right discount-label">(<asp:Literal runat="server" ID="DiscountLiteral" Text="0.00" />)</td>
            </tr>
            <tr>
                <td><b>Taxes (<asp:Literal runat="server" ID="TaxRateLiteral" />)</b></td>
                <td class="align-right"><asp:Literal runat="server" ID="TaxChargesLiteral" /></td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                    <h2>
                        Total
                        <b class="price-label" style="float: right;"><asp:Literal runat="server" ID="TotalLiteral" /></b>
                    </h2><br />
                </td>
            </tr>
        </table>
        
        <p>
            <b>Payment Method</b><br />
            <asp:Literal runat="server" ID="BillingMethodLiteral" /><br /><br />
            
            <b>Authorization Code</b><br />
            <asp:Literal runat="server" ID="AuthorizationCodeLiteral" /><br /><br />
            
            <asp:Literal runat="server" ID="BillingAmountLiteral" Visible="False" />
        </p>
    </div>

    <div class="checkout-info-container" style="border-bottom: none;">

        <h4>Billing Address</h4>
        <p><i><asp:Literal runat="server" ID="BillingAddressLiteral" /></i></p><br />

        <h4>Shipping Address</h4>
        <p><i><asp:Literal runat="server" ID="ShippingAddressLiteral" /></i></p><br />
        
        <h4>Courior</h4>
        <p><i><asp:Literal runat="server" ID="ShippingDetailsLiteral" /></i></p><br />

    </div>
    
    
    <!--p>
        <a href="/Home/Privacy-Policy.html">Privacy Policy</a>&nbsp;&nbsp;&nbsp; - &nbsp;&nbsp;&nbsp;
        <a href="/Home/Guarantee.html">Guarantee</a>&nbsp;&nbsp;&nbsp; - &nbsp;&nbsp;&nbsp;
        <a href="/Home/Returns.html">Returns</a>&nbsp;&nbsp;&nbsp; - &nbsp;&nbsp;&nbsp;
        <a href="/Home/Security.html">Security</a>
    </p-->
	
    <br class="clear-both" /><br />
    <p>Having problems with checkout? Give us a call at <b>1.905.390.0635</b></p>

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <div class="bread-crumb-holder">
        <a href="/">Home</a> &raquo; <a href="/Store/Shop.html">Shop</a> &raquo; Thank You
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
</asp:Content>