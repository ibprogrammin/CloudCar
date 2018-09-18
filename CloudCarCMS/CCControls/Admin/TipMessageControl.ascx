<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TipMessageControl.ascx.vb" Inherits="CloudCar.CCControls.Admin.TipMessageControl" %>
<asp:Repeater runat="server" ID="MessageRepeater">
    <HeaderTemplate>
        <div class="tip-panel">
            <label></label>
        </div>
        <ul class="tip-list">
    </HeaderTemplate>
    <ItemTemplate>
        <li>
            <img src="/CCTemplates/Admin/Images/icons/cc.testimonial.icon.dark.png" style="margin-bottom: -3px; margin-right: 5px; width: 18px; height: 18px;" alt="" />
            <%# Eval("ActivityMessage")%>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>