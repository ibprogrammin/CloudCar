<%@ Page Title="Checkout" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Checkout.aspx.vb" Inherits="CloudCar.CCCommerce.Checkout" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.HtmlControls" Assembly="System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server" />

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <div class="breadcrumb">
        <a href="/">Home</a> &raquo; 
        <a href="/Shop/Index.html">Shop</a> &raquo; 
        <a href="/Shop/ShoppingCart.html">Shopping Cart</a> &raquo; 
        Checkout
    </div>

    <h1>Checkout</h1>
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    <Store:ShoppingProgressControl ID="ShoppingProgressControl1" runat="server" Progress="CheckOut" Visible="false" />
    
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" />

    <asp:HtmlIframe runat="server" id="ifVBVContents" visible="false" width="100%" height="600" style="border: 1px solid #666666; display: none;" />
    
    <asp:Label runat="server" ID="StatusMessageLabel" CssClass="shopping-status-message" Visible="false" />
    
    <p>Would you like to <a href="/Login.html">Login</a> or <a href="/Register.html">Register</a> before you continue?</p>

    <div class="checkout">
        <div id="checkout">
            <div class="checkout-heading">
                Step 1: Your Information
                <a href="#" class="right">Hide</a>
            </div>
            <div class="checkout-content">
                <div class="left">
                    <b>Email</b><br />
                    <asp:TextBox runat="server" ID="EmailAddressTextBox" tabindex="30" CssClass="large-field" />
                    <br /><br />
                </div>
                
                <div class="right">
                    <b>Phone Number</b><br />
                    <asp:TextBox runat="server" ID="PhoneNumberTextBox" tabindex="31" CssClass="large-field" />
                </div>
            </div>
        </div>
        <div id="billing">
            <div class="checkout-heading">
                Step 2: Billing Details
                <a href="#" class="right">Hide</a>
            </div>
            <div class="checkout-content">
                <div class="left">
                <asp:DropDownList runat="server" ID="BillingType" DataTextField="Type" DataValueField="ID" Visible="false">
                    <asp:ListItem Text="Default" Value="1" />
                    <asp:ListItem Text="Paypal" Value="2" />
                    <asp:ListItem Text="Bean Stream" Value="3" />
                    <asp:ListItem Text="Payza" Value="4" />
                    <asp:ListItem Text="Braintree" Value="5" />
                    <asp:ListItem Text="Dwolla" Value="6" />
                    <asp:ListItem Text="Authorize .Net" Value="7" />
                </asp:DropDownList>
                
                <b>Card Number</b><br />
                <asp:TextBox runat="server" ID="CreditCardNumberTextBox" tabindex="32" CssClass="large-field" />
                <br /><br />
                
                <b>CVD</b><br />
                <asp:TextBox runat="server" ID="CreditCardCvdTextBox" tabindex="33" CssClass="large-field" />
                <br /><br />
                
                <b>Name On Card</b><br />
                <asp:TextBox runat="server" ID="CreditCardNameTextBox" tabindex="34" CssClass="large-field" />
                <br /><br />
                
                <b>Expiry</b><br />
                <asp:DropDownList runat="server" ID="ExpiryMonthDropDown" tabindex="35">
                    <asp:ListItem Text="Month" Value="" />
                </asp:DropDownList>
                <asp:DropDownList runat="server" ID="ExpiryYearDropDown" tabindex="36">
                    <asp:ListItem Text="Year" Value="" />
                </asp:DropDownList>
                <br /><br />
                
                </div>    
                <div class="right">
                
                <img src="/CCTemplates/Default/Images/Required/visa.icon.jpg" alt="" />
                <img src="/CCTemplates/Default/Images/Required/mastercard.icon.jpg" alt="" />
                <img src="/CCTemplates/Default/Images/Required/american.express.icon.jpg" alt="" />
                <img src="/CCTemplates/Default/Images/Required/discover.icon.jpg" alt="" />
        
                <img src="/CCTemplates/Default/Images/Required/verisign-logo.gif" alt="" />

                <p style="font-size: 12px;">*Please note that if you are using a credit card which supports "Verified by Visa", or Master Card “Secure Code”, you will be directed to a third party website for Verification.</p>
                
                </div>
            </div>
        </div>
        <div id="shipping-address">
            <div class="checkout-heading">
                Step 3: Shipping Address 
                (<span><a href="/Shop/ShoppingCart.html" title="Modify shipping address" style="float: none !important;">Modify</a></span>)
                <a href="#" class="right">Hide</a>
            </div>
            <div class="checkout-content">
                <p><i><asp:Literal runat="server" ID="ShippingAddressLiteral" /></i></p><br />
                <p><i><asp:Literal runat="server" ID="ShippingDetailsLiteral" /></i></p>
            </div>
        </div>
        <asp:UpdatePanel ID="BillingAddressUpdatePanel" runat="server" RenderMode="Inline" UpdateMode="Always">
            <ContentTemplate>
                <div id="billing-address">
                    <div class="checkout-heading">
                        Step 4: Billing Address 
                        (Same as shipping address? <asp:CheckBox runat="server" ID="BillingSameAsShippingCheckBox" Checked="true" AutoPostBack="true" CssClass="shipping-check-box" />)
                        <a href="#" class="right">Hide</a>
                    </div>
                    <div class="checkout-content">
                        <asp:PlaceHolder runat="server" id="BillingAddressPlaceHolder" Visible="false">

                            <asp:TextBox runat="server" ID="BillingAddressTextBox" tabindex="37" CssClass="shipping-text-box" />
                            <asp:TextBox runat="server" ID="BillingCityTextBox" tabindex="38" CssClass="shipping-text-box" /><br />
                            <asp:DropDownList runat="server" ID="BillingProvinceDropDown" DataTextField="Name" DataValueField="ID" AutoPostBack="true" tabindex="39" CssClass="shipping-select-box" />
                            <asp:DropDownList runat="server" ID="BillingCountryDropDown" DataTextField="Name" DataValueField="ID" AutoPostBack="true" tabindex="40" CssClass="shipping-select-box" />
                            <asp:TextBox runat="server" ID="BillingPostalCodeTextBox" tabindex="41" CssClass="shipping-text-box" /><br />
                        
                            <ajax:TextBoxWatermarkExtender runat="server" ID="tbwBAddress" TargetControlID="BillingAddressTextBox" WatermarkText="Address" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvBAddress" ControlToValidate="BillingAddressTextBox" Display="None" ErrorMessage="Please enter in your billing address" ValidationGroup="CheckoutGroup" />
                            <ajax:ValidatorCalloutExtender runat="server" ID="vceBAddress" TargetControlID="rfvBAddress" />
                        
                            <ajax:TextBoxWatermarkExtender runat="server" ID="tbwBCity" TargetControlID="BillingCityTextBox" WatermarkText="City" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvBCity" ControlToValidate="BillingCityTextBox" Display="None" ErrorMessage="Please enter in your billing city" ValidationGroup="CheckoutGroup" />
                            <ajax:ValidatorCalloutExtender runat="server" ID="vceBCity" TargetControlID="rfvBCity" />
                        
                            <ajax:TextBoxWatermarkExtender runat="server" ID="tbwBPC" TargetControlID="BillingPostalCodeTextBox" WatermarkText="Postal/Zip Code" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvBPC" ControlToValidate="BillingPostalCodeTextBox" Display="None" ErrorMessage="Please enter in your billing postal/zip code" ValidationGroup="CheckoutGroup" />
                            <ajax:ValidatorCalloutExtender runat="server" ID="vceBPC" TargetControlID="rfvBPC" />
                        
                            <asp:RequiredFieldValidator runat="server" ID="rfvBCountry" ControlToValidate="BillingCountryDropDown" Display="None" ErrorMessage="Please enter in your billing country" ValidationGroup="CheckoutGroup" />
                            <ajax:ValidatorCalloutExtender runat="server" ID="vceBCountry" TargetControlID="rfvBCountry" />
                        
                            <asp:RequiredFieldValidator runat="server" ID="rfvBProvince" ControlToValidate="BillingProvinceDropDown" Display="None" ErrorMessage="Please enter in your billing province/state" ValidationGroup="CheckoutGroup" />
                            <ajax:ValidatorCalloutExtender runat="server" ID="vceBProvince" TargetControlID="rfvBProvince" />

                        </asp:PlaceHolder>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="confirm">
            <div class="checkout-heading">
                Step 5: Confirm Order (<asp:Literal runat="server" ID="OrderItemsLiteral" /> Items)
                <a href="#" class="right">Hide</a>
            </div>
            <div class="checkout-content">
                <table style="width: 100%;">
                    <tr>
                        <td><b>Subtotal</b></td>
                        <td style="text-align: right;" class="align-right"><asp:Literal runat="server" ID="SubTotalLiteral" /></td>
                    </tr>
                    <tr>
                        <td><b>Shipping</b></td>
                        <td style="text-align: right;" class="align-right"><asp:Literal runat="server" ID="ShippingChargeLiteral" /></td>
                    </tr>
                    <tr>
                        <td class="discount-label"><b>Discount</b></td>
                        <td style="text-align: right;" class="align-right discount-label">(<asp:Literal runat="server" ID="DiscountLiteral" Text="0.00" />)</td>
                    </tr>
                    <tr>
                        <td><b>Taxes (<asp:Literal runat="server" ID="TaxRateLiteral" />)</b></td>
                        <td style="text-align: right;" class="align-right"><asp:Literal runat="server" ID="TaxChargesLiteral" /></td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: right;">
                            <br />
                            <h2>
                                Total&nbsp;
                                <b class="price-label" style="float: right;"><asp:Literal runat="server" ID="TotalLiteral" /></b>
                            </h2><br />
                        </td>
                    </tr>
                </table>

            </div>
        </div>
    </div>
    
    <script type="text/javascript">

        $(document).ready(function () {
            $('#checkout .checkout-content').slideDown('slow');
            $('#billing .checkout-content').slideDown('slow');
            $('#shipping-address .checkout-content').slideDown('slow');
            $('#billing-address .checkout-content').slideDown('slow');
            $('#confirm .checkout-content').slideDown('slow');

            HideShowEnable('#checkout');
            HideShowEnable('#billing');
            HideShowEnable('#shipping-address');
            HideShowEnable('#billing-address');
            HideShowEnable('#confirm');
        });
        
        function HideShowEnable(element) {
            $(element + ' .checkout-heading a.right').live('click', function () {
                if ($(element + ' .checkout-content').is(":visible")) {
                    $(element + ' .checkout-content').slideUp('slow');
                    $(element + ' .checkout-heading a.right').text("Modify +");
                }                 
                else {            
                    $(element + ' .checkout-content').slideDown('slow');
                    $(element + ' .checkout-heading a.right').text("Hide");
                }
            });    
        }
        
    </script>

    <div class="buttons">
	    <div class="right">
	        <asp:Button runat="server" ID="PurchaseButton" Text="5. Purchase" CssClass="button" CausesValidation="true" ValidationGroup="CheckoutGroup" Enabled="true" tabindex="42" />
	    </div>
        <div class="left">
            <a href="/Shop/ShoppingCart.html" class="button">&laquo; Back to Shopping Cart</a>
        </div>
    </div>

    <p>Having problems with checkout? Give us a call at <b>1.905.390.0635</b></p>

    <corp:HelpButtonControl ImageUrl="/images/round.help.icon.png" ID="HelpButton1" runat="server" Height="20" Width="20" Visible="false" 
        HelpText="If you have finished your shopping for today, you are being asked to “checkout” and that will finalize your purchase. You will be able to review your purchase prior to checkout as well as make changes to quantity or product if you so choose."/>
    
    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwEmail" TargetControlID="EmailAddressTextBox" WatermarkText="Email" />
    <asp:RequiredFieldValidator runat="server" ID="rfvEmail" ControlToValidate="EmailAddressTextBox" Display="None" ErrorMessage="Please enter in your email address" ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceEmail" TargetControlID="rfvEmail" />
    
    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="EmailAddressTextBox" ErrorMessage="Your Email address is not in the correct format." SetFocusOnError="true"
        Display="None" ValidationGroup="CheckoutGroup" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceEmailRegex" TargetControlID="revEmail" />

    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwPN" TargetControlID="PhoneNumberTextBox" WatermarkText="Phone" />
    <asp:RequiredFieldValidator runat="server" ID="rfvPN" ControlToValidate="PhoneNumberTextBox" Display="None" ErrorMessage="Please enter in your phone number" ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePN" TargetControlID="rfvPN" />
    
    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwCCNumber" TargetControlID="CreditCardNumberTextBox" WatermarkText="Credit Card Number" />
    <asp:RequiredFieldValidator runat="server" ID="rfvCCNumber" ControlToValidate="CreditCardNumberTextBox" Display="None" ErrorMessage="Please enter in your credit card number" ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceCCNumber" TargetControlID="rfvCCNumber" />
    
    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwCCName" TargetControlID="CreditCardNameTextBox" WatermarkText="Name On Card" />
    <asp:RequiredFieldValidator runat="server" ID="rfvCCName" ControlToValidate="CreditCardNameTextBox" Display="None" ErrorMessage="Please enter the name found on your credit card" ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceCCName" TargetControlID="rfvCCName" />
    
    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwCCCVD" TargetControlID="CreditCardCvdTextBox" WatermarkText="CVD" />
    <asp:RequiredFieldValidator runat="server" ID="rfvCCCVD" ControlToValidate="CreditCardCvdTextBox" Display="None" ErrorMessage="Please enter in your credit card validation number" ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceCCCVD" TargetControlID="rfvCCCVD" />
    
    <asp:RequiredFieldValidator runat="server" ID="rfvExpMonth" ControlToValidate="ExpiryMonthDropDown" Display="None" ErrorMessage="Please enter your credit card expiration month." ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceExpMonth" TargetControlID="rfvExpMonth" />
    <asp:RequiredFieldValidator runat="server" ID="rfvExpYear" ControlToValidate="ExpiryYearDropDown" Display="None" ErrorMessage="Please enter your credit card expiration year." ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceExpYear" TargetControlID="rfvExpYear" />

    <!--ul>
        <li><a href="/Home/Privacy-Policy.html">Privacy Policy</a></li>
        <li><a href="/Home/Guarantee.html">Gaurantee</a></li>
        <li><a href="/Home/Returns.html">Returns</a></li>
        <li><a href="/Home/Security.html">Security</a></li>
    </ul-->

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" visible="false" />
</asp:Content>