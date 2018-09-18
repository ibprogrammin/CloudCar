<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Categories.aspx.vb" Inherits="CloudCar.CCCommerce.Categories" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    
    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" EnableViewState="False" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" EnableViewState="False" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />
</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <CMS:BreadCrumbControl runat="server" ID="PageBreadCrumbControl" EnableViewState="False" />
    <CMS:HeaderControl runat="server" Id="PageHeaderControl" EnableViewState="False" />
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    <CMS:NivoRotatorControl runat="server" ID="PageRotatorControl" EnableViewState="False" />
    <CMS:HeaderImageControl runat="server" Id="PageHeaderImageControl" EnableViewState="False" />
    
    <Store:ShoppingProgressControl ID="ShoppingProgressControl1" runat="server" Progress="Browse" Visible="False" />
    
    <asp:Label runat="server" ID="lblStatus" />
    
    <Store:LatestControl runat="server" Count="10" DisplaySize="Large" />
    <Store:ClearanceControl runat="server" Count="10" DisplaySize="Large" />
    
    <Store:BrandControl runat="server" visible="True" />

    <CMS:ContentControl runat="server" ID="PageContentControl" PageContentType="Primary" EnableViewState="False" />
    <CMS:ChildPageMenuControl runat="server" id="PageChildMenuControl" Visible="False" EnableViewState="False" />
    
    <CMS:CallToActionControl runat="server" ID="PageCallToActionControl" />
    <br class="clear-both" />

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <CMS:ContentControl runat="server" ID="PageSecondaryContentControl" PageContentType="Secondary" EnableViewState="False" />
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
    <CMS:ScriptControl runat="server" id="PageScriptControl" EnableViewState="False" />
</asp:Content>