<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SubMenuControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.SubMenuControl" %>

<asp:Repeater runat="server" ID="rptSubMenu">
    <HeaderTemplate><ul id="RightSubMenu"></HeaderTemplate>
    <ItemTemplate>
        <li><a href="/Home/<%# Eval("Link") %>.html"><%#Eval("Title")%></a></li>
    </ItemTemplate>
    <FooterTemplate></ul></FooterTemplate>
</asp:Repeater>