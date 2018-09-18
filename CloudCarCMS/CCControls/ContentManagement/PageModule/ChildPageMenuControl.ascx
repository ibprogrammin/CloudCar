<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ChildPageMenuControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.PageModule.ChildPageMenuControl" %>

<asp:Panel id="MenuPanel" runat="server" visible="False" class="RightMenu">
    <h1>More Links</h1><br />
    <asp:Literal runat="server" ID="LinksLiteral" />
    
    <asp:Repeater runat="server" ID="LinksRepeater">
        <ItemTemplate>
            <h4><a href='<%#String.Format("/Home/{0}.html", Eval("Permalink")) %>' title='<%# Eval("pageTitle") %>'><%# Eval("breadcrumbTitle")%></a></h4><br />
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>