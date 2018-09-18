<%@ Page Title="Store Checkout" Language="vb" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/CCMasterPages/Mobile.Master" CodeBehind="MobileCheckout.aspx.vb" Inherits="CloudCar.CCCommerce.Mobile.MobileCheckout" %>

<asp:Content ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">
   
    <div>
    
        <h1>Checkout</h1>
        <p>Having problems with checkout? Give us a call at <b>1.800.276.4428</b></p>
        
        <ul>
            <li><a href="/Home/Privacy-Policy.html">Privacy Policy</a></li>
            <li><a href="/Home/Guarantee.html">Guarantee</a></li>
            <li><a href="/Home/Returns.html">Returns</a></li>
            <li><a href="/Home/Security.html">Security</a></li>
        </ul>

    </div>
    
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />

    <iframe runat="server" id="ifVBVContents" visible="false" width="100%" height="600" style="border: 1px solid #666666; display: none;"></iframe>

    <div>
        <h3>1. Order Summary</h3>
    	
        <table class="OrderSummaryTable" cellspacing="0">
    	    <tr>
        	    <td><label>Total Items</label></td>
                <td align="right"><asp:Literal runat="server" ID="litOrderItems" /></td>
            </tr>
            <tr>
        	    <td class="Subtotal LightBorder"><label class="Subtotal">Subtotal</label></td>
                <td align="right" class="Price Subtotal LightBorder"><asp:Literal runat="server" ID="litSubTotal" /></td>
            </tr>
            <tr>
        	    <td><label>Shipping</label></td>
                <td align="right" class="Price"><asp:Literal runat="server" ID="litShipCharge" /></td>
            </tr>
            <tr>
        	    <td><label class="Discount">Discount</label></td>
                <td align="right" class="Discount">(<asp:Literal runat="server" ID="litDiscount" Text="0.00" />)</td>
            </tr>
            <tr>
        	    <td class="MediumBorder"><label>Taxes (<asp:Literal runat="server" ID="litTaxRate" />)</label></td>
                <td align="right" class="Price MediumBorder"><asp:Literal runat="server" ID="litTaxAmount" /></td>
            </tr>
            <tr>
        	    <td><label class="Total">Total</label></td>
                <td align="right" class="Total Price"><asp:Literal runat="server" ID="litTotal" /></td>
            </tr>
	    </table>
        
        <br />
        
	</div>

    <div class="ContactInformationControl">
    	<h3>2. Contact Information</h3>
        
        <label>Email</label><br />
        <asp:TextBox runat="server" ID="txtEmail" style="width: 96%;" tabindex="30" />
        
        <br />
        
        <label>Phone</label><br />
        <asp:TextBox runat="server" ID="txtPN" style="width: 96%;" tabindex="31" />
        
        <br />
        
        <h3>3. Shipping Address</h3>        
        <p><i><asp:Literal runat="server" ID="litSAddress" /></i></p>
        <p runat="server" visible="false"><asp:HyperLink runat="server" NavigateUrl="~/Store/ShoppingCart.aspx" Text="Change Shipping Address" /></p>

        <h3>4. Billing Address</h3>
        
        <label>Same as shipping address? <asp:CheckBox runat="server" ID="ckbSameAsShipping" Checked="true" CssClass="CheckBox" AutoPostBack="true" onchange="aspnetForm.submit();" /></label>
        
        <br />
        
        <asp:PlaceHolder runat="server" id="tblBillingAddress" Visible="false">

            <label>Address</label><br />
            <asp:TextBox runat="server" ID="txtBAddress" style="width: 96%;" tabindex="37" />
        
            <br />
        
            <label>City</label><br />
            <asp:TextBox runat="server" ID="txtBCity" style="width: 96%;" tabindex="38" />
            
            <br />
            
            <label>Country</label><br />
            <asp:DropDownList runat="server" ID="ddlBCountry" DataTextField="Name" DataValueField="ID" style="width: 96%;" tabindex="39" />
            
            <br />
            
            <label>Province/State</label><br />
            <asp:DropDownList runat="server" ID="ddlBProvince" DataTextField="Name" DataValueField="ID" style="width: 96%;" tabindex="40" />

            <br />
            
            <label>Postal Code</label><br />
            <asp:TextBox runat="server" ID="txtBPC" style="width: 96%;" tabindex="41" />                
            
            <ajax:TextBoxWatermarkExtender runat="server" ID="tbwBAddress" TargetControlID="txtBAddress" WatermarkText="Street Address (Number, Name, Apt/Unit)" WatermarkCssClass="Watermark" />
            <asp:RequiredFieldValidator runat="server" ID="rfvBAddress" ControlToValidate="txtBAddress" Display="None" ErrorMessage="Please enter in your billing address" ValidationGroup="CheckoutGroup" />
            <ajax:ValidatorCalloutExtender runat="server" ID="vceBAddress" TargetControlID="rfvBAddress" />
            
            <asp:RequiredFieldValidator runat="server" ID="rfvBCity" ControlToValidate="txtBCity" Display="None" ErrorMessage="Please enter in your billing city" ValidationGroup="CheckoutGroup" />
            <ajax:ValidatorCalloutExtender runat="server" ID="vceBCity" TargetControlID="rfvBCity" />
            
            <asp:RequiredFieldValidator runat="server" ID="rfvBPC" ControlToValidate="txtBPC" Display="None" ErrorMessage="Please enter in your billing postal/zip code" ValidationGroup="CheckoutGroup" />
            <ajax:ValidatorCalloutExtender runat="server" ID="vceBPC" TargetControlID="rfvBPC" />
            
            <asp:RequiredFieldValidator runat="server" ID="rfvBCountry" ControlToValidate="ddlBCountry" Display="None" ErrorMessage="Please enter in your billing country" ValidationGroup="CheckoutGroup" />
            <ajax:ValidatorCalloutExtender runat="server" ID="vceBCountry" TargetControlID="rfvBCountry" />
            
            <asp:RequiredFieldValidator runat="server" ID="rfvBProvince" ControlToValidate="ddlBProvince" Display="None" ErrorMessage="Please enter in your billing province/state" ValidationGroup="CheckoutGroup" />
            <ajax:ValidatorCalloutExtender runat="server" ID="vceBProvince" TargetControlID="rfvBProvince" />

        </asp:PlaceHolder>

        <br />
        
    </div>

    <div class="PaymentInformationControl">
    	<h3>5. Payment Information</h3>
        
        <asp:DropDownList runat="server" ID="ddlCardType" DataTextField="Type" DataValueField="ID" Visible="false" />       
        
        <label>Credit Card Number</label><br />
        <asp:TextBox runat="server" ID="txtCCNumber" style="width: 96%;" tabindex="32" />
        
        <br />
        
        <label>Validation Number</label><br />
        <asp:TextBox runat="server" ID="txtCCCVD" style="width: 33%;" tabindex="33" />
        
        <br />
        
        <label>Name on Card</label><br />
        <asp:TextBox runat="server" ID="txtCCName" style="width: 96%" tabindex="34" />
        
        <br />
        
        <label>Expiry Date</label><br />
        <asp:DropDownList runat="server" ID="ddlExpMonth" style="width: 47%; margin-right: 4%;" tabindex="35">
            <asp:ListItem Text="Month" Value="" />
        </asp:DropDownList>
        <asp:DropDownList runat="server" ID="ddlExpYear" style="width: 47%;" tabindex="36">
            <asp:ListItem Text="Year" Value="" />
        </asp:DropDownList>
        
        <br />
        
        <img src="/images/icons/visa.icon.jpg" style="margin-top: 35px;" alt="Visa" />
        <img src="/images/icons/mastercard.icon.jpg" style="margin-top: 35px; margin-left: 10px;" alt="Mastercard" />
        <img src="/images/icons/american.express.icon.jpg" style="margin-top: 35px; margin-left: 10px;" alt="American Express" />
        <img src="/images/icons/discover.icon.jpg" style="margin-top: 40px; margin-left: 10px;" alt="Discover" />
        
        <br /><br />
        
        <p style="font-size: 11px; line-height: 18px;">*<b>Note</b> If you are using a credit card which supports "Verified by Visa", or Master Card “Secure Code”, you will be directed to a third party website for Verification.</p>
    </div>
    
    <asp:RequiredFieldValidator runat="server" ID="rfvEmail" ControlToValidate="txtEmail" Display="None" ErrorMessage="Please enter in your email address" ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceEmail" TargetControlID="rfvEmail" />
    
    <asp:RequiredFieldValidator runat="server" ID="rfvPN" ControlToValidate="txtPN" Display="None" ErrorMessage="Please enter in your phone number" ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePN" TargetControlID="rfvPN" />
    
    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwCCNumber" TargetControlID="txtCCNumber" WatermarkText="e.g. 0000 0000 0000 0000" WatermarkCssClass="Watermark" />
    <asp:RequiredFieldValidator runat="server" ID="rfvCCNumber" ControlToValidate="txtCCNumber" Display="None" ErrorMessage="Please enter in your credit card number" ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceCCNumber" TargetControlID="rfvCCNumber" />
    
    <asp:RequiredFieldValidator runat="server" ID="rfvCCName" ControlToValidate="txtCCName" Display="None" ErrorMessage="Please enter the name found on your credit card" ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceCCName" TargetControlID="rfvCCName" />
    
    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwCCCVD" TargetControlID="txtCCCVD" WatermarkText="e.g. 000" WatermarkCssClass="Watermark" />
    <asp:RequiredFieldValidator runat="server" ID="rfvCCCVD" ControlToValidate="txtCCCVD" Display="None" ErrorMessage="Please enter in your credit card validation number" ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceCCCVD" TargetControlID="rfvCCCVD" />
    
    <asp:RequiredFieldValidator runat="server" ID="rfvExpMonth" ControlToValidate="ddlExpMonth" Display="None" ErrorMessage="Please enter your credit card expiration month." ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceExpMonth" TargetControlID="rfvExpMonth" />
    <asp:RequiredFieldValidator runat="server" ID="rfvExpYear" ControlToValidate="ddlExpYear" Display="None" ErrorMessage="Please enter your credit card expiration year." ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceExpYear" TargetControlID="rfvExpYear" />
    
    <center><asp:Button runat="server" ID="btnPurchase" CssClass="btnPurchase" CausesValidation="true" ValidationGroup="CheckoutGroup" Enabled="true" tabindex="42" /></center>
    
    <br />

</asp:Content>

<asp:Content ContentPlaceHolderID="cphBeforeContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="cphAfterContent" runat="server"></asp:Content>