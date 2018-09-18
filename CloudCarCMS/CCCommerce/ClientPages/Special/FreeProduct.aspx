<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="FreeProduct.aspx.vb" Inherits="CloudCar.CCCommerce.Special.FreeProduct" %>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

    <h1 class="ContentPageHeading">Get Your Free Boxes Now!</h1>

    <p>Enter the promo code in the field below and hit submit to add 30 free boxes to your shopping cart.</p><br /><br /><br /><br />

    <asp:Label runat="server" ID="lblError" CssClass="ErrorDisplay" Visible="false" ForeColor="Red" style="display: block; width: 370px;" />

    <asp:TextBox runat="server" ID="txtPromoCode" ValidationGroup="PromoCode" TabIndex="31" style="float: left;" />

    <asp:LinkButton id="btnSubmit" runat="server" CssClass="GreenButton SerifStack" CausesValidation="true" ValidationGroup="PromoCode" style="float: left; margin-left: 10px; position: relative; width: 260px; margin-top: 3px;" TabIndex="32"><span class="GreenButton">Add Products</span></asp:LinkButton>

    <br style="clear: both;" /><br /><br /><br /><br /><br /><br /><br />

</asp:Content>