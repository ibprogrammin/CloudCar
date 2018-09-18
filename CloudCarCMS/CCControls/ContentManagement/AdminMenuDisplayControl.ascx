<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminMenuDisplayControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.AdminMenuDisplayControl" %>
<asp:Repeater runat="server" ID="rptMenuItems">
    <ItemTemplate>
        <a href="<%# Eval("URL") %>" class="menu-button" title="<%# Eval("Details") %>"><%#Eval("Title")%></a>
    </ItemTemplate>
</asp:Repeater>