<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Testimonials.aspx.vb" Inherits="CloudCar.CCContentManagement.Testimonials" %>
<%@ Import Namespace="CloudCar.CCFramework.ContentManagement" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" EnableViewState="False" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" EnableViewState="False" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />
</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <CMS:HeaderControl runat="server" Id="PageHeaderControl" EnableViewState="False" />
    <hr />

    <CMS:NivoRotatorControl runat="server" ID="PageRotatorControl" EnableViewState="False" />
    <CMS:HeaderImageControl runat="server" Id="PageHeaderImageControl" EnableViewState="False" />
    <CMS:ContentControl runat="server" ID="PageContentControl" PageContentType="Primary" EnableViewState="False" />
    
    <CMS:TestimonialControl runat="server" />

    <br class="clear-both" />

    <asp:Repeater runat="server" ID="rptVideos">
        <HeaderTemplate>
            <h2>Videos</h2>
        </HeaderTemplate>
        <ItemTemplate>
            <div style="background-color: #fff; border: 0 solid #888; float: left; width: 236px; height: 255px; padding: 10px; margin-right: 20px; margin-left: 0px; margin-bottom: 15px;">
                <%# VideoController.GetPlayerHTML(CType(Eval("Player"), SMVideoType), CStr(Eval("VideoID")), 236, 168)%>
                <p><%#Eval("Title")%></p>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    
    <CMS:ChildPageMenuControl runat="server" id="PageChildMenuControl" Visible="False" EnableViewState="False" />

</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
    <CMS:ContentControl runat="server" ID="PageSecondaryContentControl" PageContentType="Secondary" EnableViewState="False" />

    <br class="clear-both" />

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <CMS:CallToActionControl runat="server" ID="PageCallToActionControl" />
    <CMS:BreadCrumbControl runat="server" ID="PageBreadCrumbControl" EnableViewState="False" />
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
    <CMS:ScriptControl runat="server" id="PageScriptControl" EnableViewState="False" />
</asp:Content>