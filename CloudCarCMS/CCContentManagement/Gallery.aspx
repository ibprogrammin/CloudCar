<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Gallery.aspx.vb" Inherits="CloudCar.CCContentManagement.Gallery" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    
    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />

    <link rel="stylesheet" type="text/css" href="/CCTemplates/Default/Styles/jQueryLightBox/jquery.lightbox-0.5.css" media="screen" />

</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    
    <CMS:HeaderImageControl runat="server" Id="PageHeaderImageControl" />
    <CMS:HeaderControl runat="server" Id="PageHeaderControl" />
    <CMS:NivoRotatorControl runat="server" ID="PageRotatorControl" ScriptManagementControlId="PageScriptManagementControl" />
    <CMS:ContentControl runat="server" ID="PageContentControl" PageContentType="Primary" />

</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
    <CMS:ContentControl runat="server" ID="PageSecondaryContentControl" PageContentType="Secondary" />

    <asp:Repeater runat="server" ID="GalleriesRepeater">
        <ItemTemplate>
            <div id='<%# String.Format("Gallery{0}", CStr(Eval("Title")).Replace(" ", "")) %>' class="LightBoxGallery">
                <h2><asp:Literal runat="server" ID="litTitle" Text='<%# Eval("Title") %>' /></h2>
                <CMS:ImageGalleryControl runat="server" ID="igcGallery" GalleryID='<%# Eval("ID") %>' Category='<%# Eval("Title") %>'  />
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <CMS:ChildPageMenuControl runat="server" id="PageChildMenuControl" Visible="False" />
    
    <CMS:CallToActionControl runat="server" ID="PageCallToActionControl" />

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <CMS:BreadCrumbControl runat="server" ID="PageBreadCrumbControl" />
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
    <asp:Literal runat="server" ID="GalleryScriptLiteral" />
    <CMS:ScriptControl runat="server" id="PageScriptControl" />

</asp:Content>