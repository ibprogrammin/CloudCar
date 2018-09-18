<%@ Page Title="Shopping Cart" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Mobile.Master" CodeBehind="MobileShoppingCart.aspx.vb" Inherits="CloudCar.CCCommerce.Mobile.MobileShoppingCart" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Content ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

<div>

    <h1><asp:Literal runat="server" ID="litCategoryTitle" Text="Your Shopping Cart" /></h1>

    <p>Having problems with checkout?<br /> Give us a call at 1.800.276.4428</p>

    <ul>
        <li><a href="/Home/Privacy-Policy.html">Privacy Policy</a></li>
        <li><a href="/Home/Guarantee.html">Guarantee</a></li>
        <li><a href="/Home/Returns.html">Returns</a></li>
        <li><a href="/Home/Security.html">Security</a></li>
    </ul>

</div>

<asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />

<asp:PlaceHolder runat="server" ID="phSC">

<asp:Repeater runat="server" ID="rptSCItems">
    <HeaderTemplate>
        <table class="OrderSummaryTable" cellspacing="0">
        	<thead>
            	<tr>
                	<td>Item(s)</td>
                    <td>Details</td>
                    <td align="right">Total</td>
                </tr>
            </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><a href="<%# String.Format("/product/{0}/{1}.html", Eval("Category"), Eval("Permalink"))%>" title='<%#Eval("Name")%>'><%#Eval("Name")%></a></td>
            <td>
        	    <b>Qty.</b> &nbsp;&nbsp;<asp:Label runat="server" Text='<%# Eval("Quantity") %>' />&nbsp;|&nbsp;<b>Size/Colour</b> - <%# ApplicationFunctions.DisplaySizeColor(CStr(Eval("Color")), CStr(Eval("Size")))%>
            </td>
            <td class="Price" align="right"><%#Eval("Total", "{0:n2}")%></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>

<hr />

<table cellspacing="0" class="OrderSummaryTable">
    <tr>
    	<td><label class="Total">Total</label></td>
        <td align="right" class="Total Price" colspan="2"><asp:Label ID="litTotal" runat="server" Text="$999.99" CssClass="Price" /></td>
    </tr>
</table>

<div>
	<h3>1. Shipping Address</h3>

    <p>Please enter your address before proceeding to checkout.</p>
    
    <label>Address</label><br />
    <asp:TextBox id="txtAddress" runat="server" tabindex="37" style="width: 96%;" />
    
    <br />
    
    <label>City</label><br />
    <asp:TextBox ID="txtCity" runat="server" tabindex="38" style="width: 96%;" />
    
    <br />
    
    <label>Country</label><br />
    <asp:DropDownList runat="server" ID="ddlCountry" DataTextField="Name" DataValueField="ID" tabindex="39" style="width: 98%;" />
    
    <br />
    
    <label>Province/State</label><br />
    <asp:DropDownList runat="server" ID="ddlProvince" DataTextField="Name" DataValueField="ID" tabindex="40" style="width: 98%;" />

    <br />
    
    <label>Postal Code/Zip Code</label><br />
    <asp:TextBox runat="server" ID="txtPostalCode" tabindex="41" style="width: 96%;" />
</div>

<div>

    <label>Promo Code</label><br />
    <asp:TextBox ID="txtPromoCode" runat="Server" tabindex="44" MaxLength="8" style="width: 33%;" />
    
</div>

<br style="clear: both;" /><br />

<center><asp:Button runat="server" ID="btnCheckout" CssClass="btnCheckout" CausesValidation="true" ValidationGroup="Checkout" Enabled="true" TabIndex="45" /></center>

<br style="clear: both;" />

<asp:RequiredFieldValidator runat="server" ID="rfvAddress" ControlToValidate="txtAddress" Display="None" ErrorMessage="Please enter in your address" ValidationGroup="Checkout" />
<ajax:ValidatorCalloutExtender runat="server" ID="vceAddress" TargetControlID="rfvAddress" />

<asp:RequiredFieldValidator runat="server" ID="rfvCity" ControlToValidate="txtCity" Display="None" ErrorMessage="Please enter in your city" ValidationGroup="Checkout" />
<ajax:ValidatorCalloutExtender runat="server" ID="vceCity" TargetControlID="rfvCity" />

<asp:RequiredFieldValidator runat="server" ID="rfvCountry" ControlToValidate="ddlCountry" Display="None" ErrorMessage="Please select your country" ValidationGroup="Checkout" />
<ajax:ValidatorCalloutExtender runat="server" ID="vceCountry" TargetControlID="rfvCountry" />

<asp:RequiredFieldValidator runat="server" ID="rfvProvince" ControlToValidate="ddlProvince" Display="None" ErrorMessage="Please enter in your province/state" ValidationGroup="Checkout" />
<ajax:ValidatorCalloutExtender runat="server" ID="vceProvince" TargetControlID="rfvProvince" />

<asp:RequiredFieldValidator runat="server" ID="rfvPostalCode" ControlToValidate="txtPostalCode" Display="None" ErrorMessage="Please enter in your Postal/Zip Code" ValidationGroup="Checkout" />
<ajax:ValidatorCalloutExtender runat="server" ID="vcePostalCode" TargetControlID="rfvPostalCode" />

</asp:PlaceHolder>


</asp:Content>

<asp:Content ContentPlaceHolderID="cphBeforeContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="cphAfterContent" runat="server"></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphScripts">



</asp:Content>
