<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Links.aspx.vb" Inherits="CloudCar.CCContentManagement.Links" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    
    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />

</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <CMS:HeaderImageControl runat="server" Id="PageHeaderImageControl" />
    <CMS:HeaderControl runat="server" Id="PageHeaderControl" />
    <CMS:NivoRotatorControl runat="server" ID="PageRotatorControl" />
    <CMS:ContentControl runat="server" ID="PageContentControl" PageContentType="Primary" />
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
    <CMS:ContentControl runat="server" ID="PageSecondaryContentControl" PageContentType="Secondary" />

    <br class="clear-both" />

    <asp:Repeater ID="LinksRepeater" runat="server" DataSourceID="LinksDataSource">
        <ItemTemplate>
            <Corp:LinkControl LinkID='<%# Eval("LinksID") %>' runat="server" /><br /><br />
        </ItemTemplate>
    </asp:Repeater>

    <asp:SqlDataSource ID="LinksDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:MainConnectionString %>" SelectCommand="SELECT [LinksID] FROM [Link]" />
    
    <CMS:ChildPageMenuControl runat="server" id="PageChildMenuControl" Visible="False" />
    
    <CMS:HeaderImageControl runat="server" Id="HeaderImageControl1" />

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <CMS:BreadCrumbControl runat="server" ID="PageBreadCrumbControl" />
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
    <CMS:ScriptControl runat="server" id="PageScriptControl" />
</asp:Content>