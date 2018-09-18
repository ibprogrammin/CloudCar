<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SnippetControl.ascx.vb" Inherits="CloudCar.CCControls.SnippetControl" %>

<div class="SnippetWrapper">
	<h3 class="Snippet"><asp:Label ID="lblTitle" runat="server" /></h3>
	<p class="Snippet"><asp:Label ID="lblContent" runat="server" /></p>
    <div style="text-align: right; width: 100%;"><asp:HyperLink ID="hlReadMore" runat="server" Text="Read more..." CssClass="Snippet" /></div>
</div>