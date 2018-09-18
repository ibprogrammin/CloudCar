<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="ProgramList.aspx.vb" Inherits="CloudCar.CCContentManagement.CalendarModule.ProgramList" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta runat="server" id="KeywordsTag" name="keywords" content="" />
    <meta runat="server" id="DescriptionTag" name="description" content="" />
    
    <link rel="canonical" href="" runat="server" id="CanonicalTag" />
</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <asp:Literal runat="server" ID="HeadingContainer" Visible="False" />

    <asp:Panel id="BannerPanel" runat="server">
        <CMS:NivoRotatorControl runat="server" ID="ImageNivoRotatorControl" />
    </asp:Panel>
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    <img runat="server" id="HeadingImage" src="" alt="" visible="false" />
    
    <asp:PlaceHolder runat="server" ID="MainContentPlaceHolder" />

    <br /><br /><br/>

    <asp:Panel id="SideMenuPanel" runat="server" visible="false">
        <h1 class="SerifStack">More Links</h1><br />
        <asp:Literal runat="server" ID="LinksContainer" />
    </asp:Panel>
</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    
    <asp:Literal runat="server" ID="SecondaryContent" />
    
    <asp:Repeater runat="server" ID="ProgramsRepeater" Visible="False">
        <ItemTemplate>
            <a id="<%# Eval("Permalink") %>" name="<%#Eval("Permalink") %>"></a>
            <div class="CurrentProgramsWrapper">
                <h1><asp:Literal runat="server" Text='<%# Eval("Name") %>' /></h1>
                <div><img src="<%# PictureController.GetPictureLink(Eval("IconImage")) %>" alt="<%#Eval("Name") %>" title="<%# Eval("Name") %>" /></div>
                
                <br/><asp:Literal runat="server" Text='<%# Eval("Content") %>' />
                <br class="clear-both"/>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    
    
    
    <div class="bread-crumb-holder"><asp:Literal runat="server" ID="BreadCrumbContainer" /></div>
    
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <asp:Literal runat="server" ID="ScriptsContainer" />
</asp:Content>