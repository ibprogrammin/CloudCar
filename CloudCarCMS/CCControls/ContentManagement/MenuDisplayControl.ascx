<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MenuDisplayControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.MenuDisplayControl" %>

<asp:Repeater runat="server" ID="HeaderMenuRepeater" Visible="false">
    <HeaderTemplate>
        <div id="menu" class="clearfix">
	        <ul>
		        <li><a href="/"><i class="icon-home"></i></a></li>
    </HeaderTemplate>
    <ItemTemplate>
        <li class="primary">
            <asp:HiddenField runat="server" ID="MenuItemIdHiddenField" Value='<%# Eval("ID") %>'/>
            <a href="<%# Eval("URL") %>" class="<%# Eval("CssClass") %>" title="<%# Eval("Details") %>"><%#Eval("Title")%></a>
        <asp:Repeater runat="server" ID="SubMenuRepeater">
            <HeaderTemplate>
                <div>
                    <ul>
            </HeaderTemplate>
            <ItemTemplate>
	            <li><a href="<%# Eval("URL") %>" class="<%# Eval("CssClass") %>" title="<%# Eval("Details") %>"><%#Eval("Title")%></a></li>
            </ItemTemplate>
            <FooterTemplate>
                    </ul>
                </div>
            </FooterTemplate>
        </asp:Repeater>
        </li>
    </ItemTemplate>
    <FooterTemplate>
            </ul>
        </div>
    </FooterTemplate>
</asp:Repeater>

<asp:Repeater runat="server" ID="rptFooterMenu" Visible="false">
    <HeaderTemplate>
        <div class="column info">
            <%#If(Me.Attributes.Item("Title") = String.Empty, "", String.Format("<h3>{0}</h3>", Me.Attributes.Item("Title")))%>
            <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><a href="<%# Eval("URL") %>" class="<%# Eval("CssClass") %>" title="<%# Eval("Details") %>"><%#Eval("Title")%></a></li>
    </ItemTemplate>
    <FooterTemplate>
            </ul>
        </div>
    </FooterTemplate>
</asp:Repeater>