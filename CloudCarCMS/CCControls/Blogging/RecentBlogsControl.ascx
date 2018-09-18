<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RecentBlogsControl.ascx.vb" Inherits="CloudCar.CCControls.Blogging.RecentBlogsControl" %>

<div class="postescontent">

    <asp:Repeater ID="rptEntries" runat="server">
        <ItemTemplate>
            <p><strong><a href="/Blog/<%# Eval("Permalink") %>.html" title="<%# Eval("Title") %> - <%# Eval("DatePosted","{0:g}") %>"><%# Eval("Title") %></a></strong><span>-by <%#Eval("Author.Name")%></span></p>
        </ItemTemplate>
    </asp:Repeater>

</div>