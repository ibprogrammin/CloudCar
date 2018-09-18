<%@ Page Title="Shopping Cart" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="ShoppingCart.aspx.vb" Inherits="CloudCar.CCCommerce.ShoppingCartPage" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    
</asp:Content>

<asp:Content ContentPlaceHolderID="MessagePlaceHolder" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    <asp:ScriptManagerProxy runat="server">
        <Services>
            <asp:ServiceReference Path="~/services/PromoCodeService.asmx" />
        </Services>
    </asp:ScriptManagerProxy>
    
    <Store:ShoppingProgressControl ID="ShoppingProgressControl1" runat="server" Progress="ShoppingCart" Visible="false" />

<asp:UpdatePanel runat="server" ID="ShoppingCartUpdatePanel" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Conditional">
    <ContentTemplate>
        
    <div class="breadcrumb">
        <a href="/">Home</a> &raquo; <a href="/Shop/Index.html">Shop</a> &raquo; Shopping Cart
    </div>

    <h1>Your Shopping Cart&nbsp;<asp:Literal runat="server" ID="CartWeightLiteral" Visible="false" Text="0.0" /></h1>
    <div runat="server" ID="StatusMessageLabel" Visible="false"></div>

    <asp:PlaceHolder runat="server" ID="ShoppingCartPlaceHolder">
    
    <asp:Repeater runat="server" ID="ShoppingCartItemsRepeater">
        <HeaderTemplate>
            <div class="cart-info">
            <table>
                <thead>
                    <td class="image">Product</td>
                    <td class="name"></td>
                    <!--td>Description</td>
                    <td>Details (Colour/Size)</td-->
                    <td class="quantity">Quantity</td>
                    <td class="total">Total</td>
                    <td class="AlignCenter" style="width: 70px;">
                        <asp:LinkButton 
                            ID="EmptyCartButton" 
                            runat="server" 
                            CssClass="shopping-cart-empty-button" 
                            Text="" 
                            ToolTip="Empty the contents of your shopping cart"
                            OnClientClick="return confirm('Are you sure you wish to clear your shopping cart?');" 
                            OnClick="EmptyCartButtonClick"
                            CausesValidation="False" />
                    </td>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="image">
                    <img runat="server" src='<%# String.Format("/images/db/{0}/70/{1}.jpg", Eval("DefaultImageID"),Eval("Name")) %>' alt='<%# Eval("Name") %>' /><br />
                </td>
                <td class="name">
                    <a href="<%# String.Format("/Shop/{0}/{1}.html", Eval("CategoryPermalink"), Eval("Permalink"))%>" title='<%#Eval("Name")%>'>
                        <%#Eval("Name")%>
                    </a>
                    <div></div>
                    <small><%# ApplicationFunctions.DisplaySizeColor(CStr(Eval("Color")), CStr(Eval("Size")))%></small>
                </td>
                <td class="quantity">
                    <asp:HiddenField runat="server" ID="ShoppingCartIdHiddenField" Value='<%# Eval("ShoppingCartId") %>' />
                    <asp:TextBox runat="server" ID="QuantityTextBox" Text='<%# Eval("Quantity") %>' size="1" /> 
                    &nbsp;<%# Eval("PricingUnit")%><br />
                    <asp:CompareValidator Type="Integer" Operator="DataTypeCheck" ControlToValidate="QuantityTextBox" runat="server" ID="cvQuantity" Display="None" ErrorMessage="Please enter a valid number." />
                    <ajax:ValidatorCalloutExtender runat="server" ID="vceQuantity" TargetControlID="cvQuantity" />
                    
                </td>
                <td class="total">
                    <%#Eval("Total", "${0:n2}")%>
                </td>
                <td style="text-align: center;">
                    <asp:LinkButton runat="server" ID="DeleteItemButton" Text="Remove" CommandArgument='<%# Eval("ShoppingCartId") %>' CommandName="Delete" OnCommand="DeleteItemButtonCommand" />
                </td>
            </tr>
        </ItemTemplate>
        <SeparatorTemplate>
                
        </SeparatorTemplate>
        <FooterTemplate>
                    </tbody>
                </table>
            </div>
        </FooterTemplate>
    </asp:Repeater>

    <div class="cart-total">
        <asp:HiddenField runat="server" ID="TotalHiddenField" />
        <asp:LinkButton runat="server" ID="UpdateCartButton" Text="Update" CssClass="button" />
        <table id="total">
            <tr>
                <td class="right" style="font-size: 24px;"><b>Sub Total</b>&nbsp;</td>
                <td class="right" style="font-size: 24px;"><asp:Label ID="TotalLiteral" runat="server" Text="$0.00" CssClass="price-label" /></td>
            </tr>
        </table>
    </div>
        
    <div class="checkout">
        <div id="shipping-estimate" onkeypress="javascript:return WebForm_FireDefaultButton(event, 'ctl00_SecondContentPlaceHolder_ShippingOptionsButton');">
            <div class="checkout-heading">
                Step 1: Enter your address to get a shipping estimate
                <img alt="Step 1 Complete" 
                class="check-image" 
                src="/CCTemplates/Default/Images/Required/check.mark.png" 
                runat="server" 
                id="CheckStepOneImage" 
                visible="False" />
                <a href="#" class="right">Hide</a>
            </div>
            <div class="checkout-content">
                <asp:Label ID="Label1" runat="server" Font-Size="Medium" Text="Please enter your address before proceeding to checkout." style="padding-left: 20px;" Visible="false" />
        
                <asp:TextBox id="AddressTextBox" runat="server" tabindex="37" CssClass="shipping-text-box" AutoCompleteType="HomeStreetAddress" autocomplete="HomeStreetAddress" />
                &nbsp;
                <asp:TextBox ID="CityTextBox" runat="server" tabindex="38" CssClass="shipping-text-box" AutoCompleteType="HomeCity" />
                &nbsp;
                <asp:DropDownList runat="server" ID="ProvinceDropDown" DataTextField="Name" DataValueField="ID" AutoPostBack="true" tabindex="39" CssClass="shipping-select-box" />
                &nbsp;
                <asp:DropDownList runat="server" ID="CountryDropDown" DataTextField="Name" DataValueField="ID" AutoPostBack="true" tabindex="40" CssClass="shipping-select-box" />
                &nbsp;
                <asp:TextBox runat="server" ID="PostalCodeTextBox" tabindex="41" CssClass="shipping-text-box" AutoCompleteType="HomeZipCode" />
                
                <br /><br />
        
                <asp:Button runat="server" id="ShippingOptionsButton" Text="Estimate Shipping" CausesValidation="true" ValidationGroup="SearchAddress" tabindex="42" CssClass="button" />
            </div>
        </div>
        <div id="shipping-method">
            <div class="checkout-heading">
                Step 2: Choose A Shipping Method
                <img alt="Step 2 Complete" 
                    class="check-image" 
                    src="/CCTemplates/Default/Images/Required/check.mark.png" 
                    runat="server" 
                    id="CheckStepTwoImage" 
                    visible="False" />
                <a href="#" class="right">Hide</a>
            </div>
            <div class="checkout-content">
                <asp:HiddenField runat="server" ID="SelectedRateHiddenField" />

                <asp:Label runat="server" ID="ShippingRateLabel" Visible="false" />
                <asp:RadioButtonList runat="server" ID="ShippingOptionsRadioList" Visible="false" AutoPostBack="true" TabIndex="43" Font-Size="12px" />
                <br />
                <table class="cart-total" style="width: 100%;">
                    <tr>
                        <td class="right">
                            <b>Charge</b>&nbsp;
                            <asp:Literal ID="ShippingChargesLiteral" runat="server" Text="0.00" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="promo-code">
            <div class="checkout-heading">
                Step 3: Enter A Promo Code (Optional)
                <img id="spanPromoCode" 
                    alt="Step 3 Complete" 
                    style="visibility: hidden;"
                    class="check-image" 
                    src="/CCTemplates/Default/Images/Required/check.mark.png" />
                <a href="#" class="right">Hide</a>
            </div>
            <div class="checkout-content">
                <asp:TextBox ID="PromoCodeTextBox" runat="Server" size="6" tabindex="44" MaxLength="8" /><!-- onblur="CheckPromoCode()" -->
                &nbsp;&nbsp;&nbsp;
                <asp:Label runat="server" ID="PromoDiscountLiteral" Text="(0.00)" CssClass="discount-label" />
        
                <input type="button" id="PromoCodeButton" class="button right"  onclick="CheckPromoCode();" value="Apply" tabindex="45" />
            </div>
        </div>
    </div>
    <br />
    <div class="buttons">
	    <div class="right">
	        <asp:Button runat="server" ID="CheckoutButton" Text="4. Secure Checkout" CausesValidation="true" ValidationGroup="CheckoutGroup" Enabled="true" TabIndex="46" CssClass="button" />    
	    </div>
	    <div class="left">
	        <asp:LinkButton runat="server" ID="ContinueShoppingButton" PostBackUrl="~/CCCommerce/Categories.aspx" Text="&laquo; Continue Shopping" CssClass="button" />
	    </div>
	</div>
        
    <script type="text/javascript">

        $(document).ready(function () {
            $('#shipping-estimate .checkout-content').slideDown('slow');
            $('#shipping-method .checkout-content').slideDown('slow');
            $('#promo-code .checkout-content').slideDown('slow');

            HideShowEnable('#shipping-estimate');
            HideShowEnable('#shipping-method');
            HideShowEnable('#promo-code');
        });

        function HideShowEnable(element) {
            $(element + ' .checkout-heading a').live('click', function () {
                if ($(element + ' .checkout-content').is(":visible")) {
                    $(element + ' .checkout-content').slideUp('slow');
                    $(element + ' .checkout-heading a').text("Modify +");
                }
                else {
                    $(element + ' .checkout-content').slideDown('slow');
                    $(element + ' .checkout-heading a').text("Hide");
                }
            });
        }

    </script>

    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwAddress" TargetControlID="AddressTextBox" WatermarkText="Address" />
    <asp:RequiredFieldValidator runat="server" ID="rfvAddress" ControlToValidate="AddressTextBox" Display="None" ErrorMessage="Please enter in your address" ValidationGroup="SearchAddress" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceAddress" TargetControlID="rfvAddress" />

    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwCity" TargetControlID="CityTextBox" WatermarkText="City" />
    <asp:RequiredFieldValidator runat="server" ID="rfvCity" ControlToValidate="CityTextBox" Display="None" ErrorMessage="Please enter in your city" ValidationGroup="SearchAddress" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceCity" TargetControlID="rfvCity" />

    <asp:RequiredFieldValidator runat="server" ID="rfvCountry" ControlToValidate="CountryDropDown" Display="None" ErrorMessage="Please select your country" ValidationGroup="SearchAddress" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceCountry" TargetControlID="rfvCountry" />

    <asp:RequiredFieldValidator runat="server" ID="rfvProvince" ControlToValidate="ProvinceDropDown" Display="None" ErrorMessage="Please enter in your province/state" ValidationGroup="SearchAddress" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceProvince" TargetControlID="rfvProvince" />

    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwPostalCode" TargetControlID="PostalCodeTextBox" WatermarkText="Postal Code/Zip" />
    <asp:RequiredFieldValidator runat="server" ID="rfvPostalCode" ControlToValidate="PostalCodeTextBox" Display="None" ErrorMessage="Please enter in your Postal/Zip Code" ValidationGroup="SearchAddress" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePostalCode" TargetControlID="rfvPostalCode" />

    <asp:RequiredFieldValidator runat="server" ID="rfvSOptions" ControlToValidate="ShippingOptionsRadioList" Display="None" ErrorMessage="Please select a shipping method" ValidationGroup="CheckoutGroup" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceSOptions" TargetControlID="rfvSOptions" />

    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwPromoCode" TargetControlID="PromoCodeTextBox" WatermarkText="ie. 2d41f3" />

    </asp:PlaceHolder>

    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress AssociatedUpdatePanelID="ShoppingCartUpdatePanel" runat="server" ID="upProgress">
    <ProgressTemplate>
        <div class="loading-box">
            <h3>Loading please wait...</h3>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

    <br class="clear-both" />
    <p>Having problems with checkout? Give us a call at <b>1.905.390.0635</b></p>

    <corp:HelpButtonControl ImageUrl="/images/round.help.icon.png" ID="HelpButton1" Visible="false" 
    runat="server" HelpTextId="1" Height="20" Width="20" HelpText="You can now buy all our products online, with accessibility 24 hours, our doors never close. </br> To obtain a shipping quote you must have filled out your address correctly in registration and hit the GO button. From the rates drop down menu you are able to select your shipping method and cost. To specify the quantity of an item you can enter a numerical value in the text box, and then you must click “update” for the changes to take effect. "/>

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server"></asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ScriptContentPlaceHolder">
    
<CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" Visible="False" />

<script type="text/javascript">
    var PromoCodeTextBox;
    var PromoDiscountLiteral;
    var TotalLiteral;
    var TotalHiddenField;
    var SelectedRateHiddenField;

    function LoadControls() {
        PromoCodeTextBox = document.getElementById("<%=PromoCodeTextBox.ClientID %>");
        PromoDiscountLiteral = document.getElementById("<%=PromoDiscountLiteral.ClientID %>");
        TotalHiddenField = document.getElementById("<%=TotalHiddenField.ClientID %>");
        SelectedRateHiddenField = document.getElementById("<%=SelectedRateHiddenField.ClientID %>");
        TotalLiteral = document.getElementById("<%=TotalLiteral.ClientID %>");
    }

    function CheckPromoCode() {
        LoadControls();

        if (PromoCodeTextBox.value.length > 0) {
            document.getElementById("spanPromoCode").style.visibility = "hidden";

            var currentTotal = TotalHiddenField.value;

            window.CloudCar.PromoCodeService.PromoCodeDiscount(PromoCodeTextBox.value, parseFloat(currentTotal.replace("$", "")), OnPromoCodeDiscount);
        }
        else {
            PromoCodeTextBox.className = "shipping-text-box promo-code-unavailable";
        }
    }

    function OnPromoCodeDiscount(discount) {
        var subTotal = parseFloat(TotalHiddenField.value);
        var shippingRate = parseFloat(SelectedRateHiddenField.value);
        var currentTotal = 0.0;
        
        if(shippingRate == null || isNaN(shippingRate)) {
            currentTotal = subTotal;
        }
        else {
            currentTotal = subTotal + shippingRate;
        }

        if (discount > 0) {
            document.getElementById("spanPromoCode").style.visibility = "visible";

            PromoCodeTextBox.className = "shipping-text-box promo-code-available";

            PromoDiscountLiteral.innerHTML = "(" + discount.toFixed(2) + ")";
            TotalLiteral.innerHTML = "$" + (Math.round((parseFloat(currentTotal) - discount) * 100) / 100).toFixed(2);
        }
        else {
            document.getElementById("spanPromoCode").style.visibility = "hidden";

            PromoCodeTextBox.className = "shipping-text-box promo-code-unavailable";

            PromoDiscountLiteral.innerHTML = "(0.00)";
            TotalLiteral.innerHTML = "$" + (Math.round((parseFloat(currentTotal)) * 100) / 100).toFixed(2);

            PromoCodeTextBox.focus();
            PromoCodeTextBox.select();
        }
    }
</script>

</asp:Content>