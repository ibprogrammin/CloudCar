<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="EventDetails.aspx.vb" Inherits="CloudCar.CCContentManagement.EventModule.EventDetails" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />
</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <h1><asp:Literal runat="server" ID="TitleLiteral" /></h1>
    
    <h5>
        <asp:Literal runat="server" ID="StartDateLiteral" /> 
        <asp:Literal runat="server" ID="TimeLiteral" />
    </h5><br />

    <asp:Image runat="server" 
        ID="ThumbnailImage" 
        Visible="false" 
        CssClass="image-border event-image" />
    <asp:Literal runat="server" ID="LocationLiteral" /><br /><br />
    <asp:Literal runat="server" ID="ContentLiteral" /><br />

</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <CMS:BreadCrumbControl runat="server" ID="EventBreadCrumbControl" BreadCrumbType="EventPage" />
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" ID="PageScriptManagementControl" />
</asp:Content>