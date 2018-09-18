<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SimpleProductControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.ProductControls.SimpleProductControl" %>

<div class="product-control simple-product-control">
    <div>
        <span></span>
        <img id="ProductImage" runat="server" alt="" src="" class="" />
        <br class="clear-both" />
    </div>
    <h3>
        <asp:HyperLink ID="NameHyperLink" runat="server" Text="Product Name" /><br />
        <span><asp:Literal ID="PriceLiteral" runat="server" /></span>
    </h3>
    <p><asp:Literal runat="server" ID="ShortDescriptionLiteral" /></p>
</div>