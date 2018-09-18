<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BlogThumbnailsControl.ascx.vb" Inherits="CloudCar.CCControls.Blogging.BlogThumbnailsControl" %>
<asp:Repeater runat="server" ID="rptEntries">
    <ItemTemplate>
        <div class="postesimg"><a href="/Blog/<%# Eval("Permalink") %>.html" title="<%# Eval("Title") %> - <%# Eval("DatePosted","{0:g}") %>"><img src="<%# Eval("ThumnailImageLink") %>" alt="" width="113" height="112" /></a></div>
    </ItemTemplate>
</asp:Repeater>