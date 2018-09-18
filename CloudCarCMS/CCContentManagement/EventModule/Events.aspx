<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Events.aspx.vb" Inherits="CloudCar.CCContentManagement.EventModule.Events" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">

    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />
</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <CMS:HeaderControl runat="server" Id="PageHeaderControl" />
    <CMS:NivoRotatorControl runat="server" ID="PageRotatorControl" />
    <CMS:HeaderImageControl runat="server" Id="PageHeaderImageControl" />
    <CMS:ContentControl runat="server" ID="PageContentControl" PageContentType="Primary" />
    
    <CMS:EventsControl runat="server" id="PageEventsControl" Visible="False" />
    <CMS:EventsCalendar runat="server" id="PageEventCalendar" />
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
    <CMS:ContentControl runat="server" ID="PageSecondaryContentControl" PageContentType="Secondary" />
    
    <CMS:ChildPageMenuControl runat="server" id="PageChildMenuControl" Visible="False" />
    
</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <CMS:CallToActionControl runat="server" ID="PageCallToActionControl" />
    <CMS:BreadCrumbControl runat="server" ID="PageBreadCrumbControl" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
    <CMS:ScriptControl runat="server" id="PageScriptControl" />
</asp:Content>
