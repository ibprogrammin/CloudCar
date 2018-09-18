<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="JShowOffRotatorControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.PageModule.JShowOffRotatorControl" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:PlaceHolder runat="server" ID="RotatorPlaceHolder" Visible="false">
    
    <asp:Repeater runat="server" ID="RotatorRepeater">
        <HeaderTemplate>
            <div id="RotatorSpanWrapper"></div>
            <ul id="JShowOffRotator">
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <section class="rotator-section">
                <%# Server.HtmlDecode(CStr(Eval("Details")))%>
                </section>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
            
        </FooterTemplate>
    </asp:Repeater>
    <asp:Repeater runat="server" ID="RotatorLinksRepeater">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
            <!--%# If(Not Eval("subheading") = String.Empty, String.Format("<div id=""htmlcaption_{0}"" class=""nivo-html-caption"">{1}</div>", Eval("id"), Server.HtmlEncode(Eval("subheading"))), "")%-->
        </ItemTemplate>
        <FooterTemplate>
            
        </FooterTemplate>
    </asp:Repeater>

</asp:PlaceHolder>