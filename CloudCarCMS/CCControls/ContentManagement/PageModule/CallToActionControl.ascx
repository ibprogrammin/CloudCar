<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CallToActionControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.PageModule.CallToActionControl" %>
<div class="generic-call-to-action">
    <h3><asp:Literal runat="server" ID="HeadingLiteral" /></h3>
    <asp:Literal runat="server" ID="DetailsLiteral" />
    <a id="LinkUrlAnchor" runat="server">
        <asp:Literal runat="server" ID="ButtonTextLiteral" />
    </a>
</div>