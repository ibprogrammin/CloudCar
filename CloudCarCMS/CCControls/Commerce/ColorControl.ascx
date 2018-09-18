<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ColorControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.ColorControl" %>
<asp:Repeater runat="server" ID="ColorRepeater">
    <ItemTemplate>
        <a href="<%# String.Format("/Shop/Color/{0}.html", Eval("Name"))%>"><%# Eval("Name")%></a>
    </ItemTemplate>
</asp:Repeater>