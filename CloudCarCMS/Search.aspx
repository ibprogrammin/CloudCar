<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Search.aspx.vb" Inherits="CloudCar.Search" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />
</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <CMS:BreadCrumbControl runat="server" ID="PageBreadCrumbControl" />
    <CMS:HeaderControl runat="server" Id="PageHeaderControl" />
    <CMS:NivoRotatorControl runat="server" ID="PageRotatorControl" />
    <CMS:HeaderImageControl runat="server" Id="PageHeaderImageControl" />
    
    <CMS:ContentControl runat="server" ID="PageContentControl" PageContentType="Primary" />
    
    <asp:Literal runat="server" ID="StatusMessageLiteral" />
    
    <br />

    <asp:Repeater runat="server" ID="rptProducts" Visible="false">
        <HeaderTemplate>
            <h2>Product Results</h2>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="search-item">
                <%# If(Not Eval("ImageUrl") Is Nothing, String.Format("<img src=""{0}"" />", Eval("ImageUrl")), "")%>
                <b><asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Eval("Title") %>' NavigateUrl='<%# Eval("URL") %>' /></b>
                <p><asp:Literal ID="Literal1" runat="server" Text='<%# Eval("ShortDescription") %>' /><br />
                <asp:Label ID="Label1" runat="server" Text='<%# String.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, Eval("URL")) %>' /></p>
            </div>
        </ItemTemplate>
        <FooterTemplate></FooterTemplate>
    </asp:Repeater>
    
    <asp:Repeater runat="server" ID="rptContent" Visible="false">
        <HeaderTemplate>
            <h2>Content</h2>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="search-item">
                <h3><asp:HyperLink ID="HyperLink2" runat="server" Text='<%# Eval("Title") %>' NavigateUrl='<%# Eval("URL") %>' /></h3>
                <p><asp:Literal ID="Literal2" runat="server" Text='<%# Eval("ShortDescription") %>' /><br />
                <asp:Label ID="Label2" runat="server" Text='<%# String.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, Eval("URL")) %>' /></p>
            </div>
        </ItemTemplate>
        <FooterTemplate></FooterTemplate>
    </asp:Repeater>
    
    <asp:Repeater runat="server" ID="rptBlogs" Visible="false">
        <HeaderTemplate>
            <h2>Blogs</h2>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="search-item">
                <h3><asp:HyperLink ID="HyperLink3" runat="server" Text='<%# Eval("Title") %>' NavigateUrl='<%# Eval("URL") %>' /></h3>
                <p><asp:Literal ID="Literal3" runat="server" Text='<%# Eval("ShortDescription") %>' /><br />
                <asp:Label ID="Label3" runat="server" Text='<%# String.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, Eval("URL")) %>' /></p>
            </div>
        </ItemTemplate>
        <FooterTemplate></FooterTemplate>
    </asp:Repeater>
    
    <asp:Repeater runat="server" ID="rptMultimedia" Visible="false">
        <HeaderTemplate>
            <h2>Media</h2>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="search-item">
                <h3><asp:HyperLink ID="HyperLink4" runat="server" Text='<%# Eval("Title") %>' NavigateUrl='<%# Eval("URL") %>' /></h3>
                <p><asp:Label ID="Label4" runat="server" Text='<%# String.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, Eval("URL")) %>' /></p>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    
    <br class="clear-both" /><br />

    <p><asp:Literal runat="server" ID="litResults" /></p>
    
    <CMS:ChildPageMenuControl runat="server" id="PageChildMenuControl" Visible="False" />

</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
    <CMS:ContentControl runat="server" ID="PageSecondaryContentControl" PageContentType="Secondary" />

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <CMS:CallToActionControl runat="server" ID="PageCallToActionControl" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
    <CMS:ScriptControl runat="server" id="PageScriptControl" />
</asp:Content>