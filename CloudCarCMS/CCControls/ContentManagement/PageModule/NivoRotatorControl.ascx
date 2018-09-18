<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NivoRotatorControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.PageModule.NivoRotatorControl" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Panel runat="Server" ID="BannerPanel" Visible="False" CssClass="slider-wrapper">
    <div id="slider" class="nivoSlider">
    <asp:Repeater runat="server" ID="RotatorRepeater">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
            <%# If(Not Eval("linkurl") Is String.Empty,
                        String.Format("<a href=""{0}"" alt=""{1}"">",
                        Eval("linkurl"),
                        Server.HtmlEncode(CStr(Eval("title")))), "")%>
            <%# If(Not Eval("subheading") Is String.Empty,
                        String.Format("<img src=""{0}"" data-thumb=""{0}"" alt=""{1}"" title=""#htmlcaption_{2}"" />",
                                    PictureController.GetPictureLink(CInt(Eval("ImageID"))),
                                    Eval("title"), Eval("id")),
                        String.Format("<img src=""{0}"" data-thumb=""{0}"" alt=""{1}"" />",
                                      PictureController.GetPictureLink(CInt(Eval("ImageID"))),
                                      Server.HtmlEncode(CStr(Eval("title")))))%>
            <%# If(Not Eval("linkurl") Is String.Empty, "</a>", "")%>
        </ItemTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </asp:Repeater>
    </div>

    <asp:Repeater runat="server" ID="RotatorLinksRepeater" Visible="True">
        <HeaderTemplate></HeaderTemplate>
        <ItemTemplate>
            <%# If(Not Eval("subheading") Is String.Empty,
                    String.Format("<div id=""htmlcaption_{0}"" class=""nivo-html-caption"">{1}</div>",
                                  Eval("id"), Server.HtmlEncode(CStr(Eval("subheading")))), "")%>
        </ItemTemplate>
        <FooterTemplate></FooterTemplate>
    </asp:Repeater>
    
</asp:Panel>