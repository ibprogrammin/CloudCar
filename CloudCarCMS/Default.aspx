<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Home.Master" CodeBehind="Default.aspx.vb" Inherits="CloudCar.CloudCarDefault" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    
    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />

</asp:Content>

<asp:Content ContentPlaceHolderID="RotatorPlaceHolder" runat="server">
    <CMS:JShowOffRotatorControl runat="server" ID="TempRotatorControl" ScriptManagementControlId="PageScriptManagementControl" Visible="False" />
    <CMS:NivoRotatorControl runat="server" ID="NivoRotatorControl" ScriptManagementControlId="PageScriptManagementControl" Visible="False" />
    <CMS:CameraRotatorControl runat="server" ID="PageRotatorControl" ScriptManagementControlId="PageScriptManagementControl" Visible="True" />
</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    
    <CMS:HeaderImageControl runat="server" Id="PageHeaderImageControl" />
    <CMS:HeaderControl runat="server" Id="PageHeaderControl" Visible="False" />
    
    <Store:ClearanceControl ID="PageClearanceControl" runat="server" Count="10" DisplaySize="Large" />
	<Store:LatestControl ID="LatestControl1" runat="server" Count="10" DisplaySize="Large" />       
    <Store:BrandControl runat="server" />

    <CMS:ContentControl runat="server" ID="PageContentControl" PageContentType="Primary" />

    <CMS:ChildPageMenuControl runat="server" id="PageChildMenuControl" Visible="False" />

    <Store:QuickShopControl runat="Server" id="PageQuickShipControl" Visible="false" />
    <blog:SocialMediaControl runat="Server" id="PageSocialMediaControl" enabled="false" visible="false" />

</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    <CMS:ContentControl runat="server" ID="PageSecondaryContentControl" PageContentType="Secondary" />
</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <CMS:CallToActionControl runat="server" ID="PageCallToActionControl" />
    <CMS:BreadCrumbControl runat="server" ID="PageBreadCrumbControl" Visible="False" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ScriptContentPlaceHolder">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" Visible="false" />
    <CMS:ScriptControl runat="server" id="PageScriptControl" />
</asp:Content>