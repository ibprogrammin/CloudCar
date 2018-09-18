<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Videos.aspx.vb" Inherits="CloudCar.CCContentManagement.VideosModule.Videos" %>
<%@ Import Namespace="CloudCar.CCFramework.ContentManagement" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    
    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" EnableViewState="False" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" EnableViewState="False" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />

</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <CMS:HeaderControl runat="server" Id="PageHeaderControl" EnableViewState="False" />
    <CMS:NivoRotatorControl runat="server" ID="PageRotatorControl" EnableViewState="False" />
    <CMS:HeaderImageControl runat="server" Id="PageHeaderImageControl" EnableViewState="False" />
    <CMS:ContentControl runat="server" ID="PageContentControl" PageContentType="Primary" EnableViewState="False" />
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

    <CMS:ContentControl runat="server" ID="PageSecondaryContentControl" PageContentType="Secondary" EnableViewState="False" />
    <br class="clear-both" />

    <div class="featured-video-wrapper">
        <asp:Literal runat="server" ID="litFeaturedVideo" EnableViewState="False" />
        <h4>
            <asp:Literal runat="server" ID="litFeaturedTitle" EnableViewState="False" />
        </h4>
        <asp:Literal runat="server" ID="litFeaturedDetails" EnableViewState="False" />
    </div>

    <asp:Repeater runat="server" ID="rptVideos" EnableViewState="False">
        <HeaderTemplate>
            <div class="video-wrapper">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="small-video-wrapper">
                <%# VideoController.GetPlayerHTML(CType(Eval("Player"), SMVideoType), CStr(Eval("VideoId")), 200, 155)%>    
                <h4 class="video-title"><%#Eval("Title")%></h4>
            </div>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <div class="small-video-wrapper">
                <%# VideoController.GetPlayerHTML(CType(Eval("Player"), SMVideoType), CStr(Eval("VideoId")), 200, 155)%>    
                <h4 class="video-title"><%#Eval("Title")%></h4>
            </div>
            <br class="clear-both" />
        </AlternatingItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    
    <CMS:ChildPageMenuControl runat="server" id="PageChildMenuControl" Visible="False" EnableViewState="False" />
    
    <CMS:CallToActionControl runat="server" ID="PageCallToActionControl" />

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <CMS:BreadCrumbControl runat="server" ID="PageBreadCrumbControl" EnableViewState="False" />
</asp:Content>
<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
    <CMS:ScriptControl runat="server" id="PageScriptControl" EnableViewState="False" />
</asp:Content>