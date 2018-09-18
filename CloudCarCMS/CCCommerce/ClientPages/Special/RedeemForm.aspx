<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="RedeemForm.aspx.vb" Inherits="CloudCar.CCCommerce.Special.RedeemForm" %>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

    <h1 class="ContentPageHeading">Redeem Your Free Product Now!</h1><br /><br /><br />

    <asp:Label runat="server" ID="lblError" CssClass="status-message" Visible="false" ForeColor="Red" style="display: block; width: 960px;" />
    
    <p>Enter the redemtion code in the field below and hit submit to add your free product to the shopping cart.</p><br /><br /><br /><br />

    <asp:TextBox runat="server" ID="txtRedemptionCode" ValidationGroup="Form" TabIndex="31" style="float: left;" />

    <asp:LinkButton id="btnSubmit" runat="server" CssClass="GreenButton SerifStack" CausesValidation="true" ValidationGroup="Form" style="float: left; margin-left: 10px; position: relative; width: 260px; margin-top: 3px;" TabIndex="32"><span class="GreenButton">Add Products</span></asp:LinkButton>

    <br style="clear: both;" /><br /><br /><br /><br /><br /><br /><br />

</asp:Content>