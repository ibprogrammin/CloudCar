<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AlertMessageControl.ascx.vb" Inherits="CloudCar.CCControls.Admin.AlertMessageControl" %>
<asp:Repeater runat="server" ID="MessageRepeater">
    <HeaderTemplate>
        <div class="notice-panel">
            <label></label>
        </div>
        <ul class="notice-list">
    </HeaderTemplate>
    <ItemTemplate>
        <li>
            <img src="/CCTemplates/Admin/Images/icons/cc.testimonial.icon.dark.png" style="margin-bottom: -3px; margin-right: 5px; width: 18px; height: 18px;" alt="" />
            <%# Eval("ActivityDate", "{0:ddd MMM dd, yyyy} at {0:h:mm tt}")%> - <%# Eval("ActivityMessage")%>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>