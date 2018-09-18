<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="PropertySearch.aspx.vb" Inherits="CloudCar.CCContentManagement.PropertyModule.PropertySearch" %>
<%@ Import Namespace="CloudCar.CCFramework.ContentManagement" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">

    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />

</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <CMS:HeaderControl runat="server" Id="PageHeaderControl" />
    <CMS:NivoRotatorControl runat="server" ID="PageRotatorControl" />
    <CMS:HeaderImageControl runat="server" Id="PageHeaderImageControl" />
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

    <CMS:ContentControl runat="server" ID="PageContentControl" PageContentType="Primary" />

    <br class="clear-both" />

    <asp:PlaceHolder runat="server" ID="SearchPlaceHolder" Visible="false">

        <h2>Browse Some Great Properties</h2><br />

        <label>City</label>
        <asp:DropDownList runat="server" ID="CityDropDown" />

        <label>Price Range</label>
        <asp:DropDownList runat="server" ID="PriceLowDropDown" />
        <asp:DropDownList runat="server" ID="PriceHighDropDown" />

        <label>Bedrooms</label>
        <asp:TextBox runat="server" ID="BedroomsTextBox" />

        <label>Bathrooms</label>
        <asp:TextBox runat="server" ID="BathroomsTextBox" />

        <asp:Button runat="server" ID="SearchButton" Text="Search" ToolTip="Search for a property" />

    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="ResultsPlaceHolder" Visible="false">

        <h3><asp:Literal runat="server" ID="ResultsLiteral" /> Properties Matched Your Query</h3>

        <asp:Repeater runat="server" ID="PropertyResultsRepeater">
            <HeaderTemplate>
                <ul>
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <img src="<%# String.Format("/images/db/{0}/140/{1}.jpg",  ImageGalleryController.GetFirstGalleryImage(CType(Eval("ImageGalleryId"), Integer)), Eval("Permalink")) %>" alt="<%# Eval("Title") %>" />
                    <h3><a href="/Property/<%# Eval("Permalink") %>.html"><%# Eval("Title") %></a></h3>
                    <p><%#Eval("Address.Address")%><br /><%# String.Format("{0}, {1}", Eval("Address.City"), Eval("Address.Province.Name"))%><br />
                    <a href="/Property/<%# Eval("Permalink") %>.html">/Property/<%# Eval("Permalink") %>.html</a></p>
                </li>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <li>
                    <img src="<%# String.Format("/images/db/{0}/140/{1}.jpg",  ImageGalleryController.GetFirstGalleryImage(CType(Eval("ImageGalleryId"), Integer)), Eval("Permalink")) %>" alt="<%# Eval("Title") %>" />
                    <h3><a href="/Property/<%# Eval("Permalink") %>.html"><%# Eval("Title") %></a></h3>
                    <p><%#Eval("Address.Address")%><br /><%# String.Format("{0}, {1}", Eval("Address.City"), Eval("Address.Province.Name"))%><br />
                    <a href="/Property/<%# Eval("Permalink") %>.html">/Property/<%# Eval("Permalink") %>.html</a></p>
                </li>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>

    </asp:PlaceHolder>

    <br class="clear-both" />
    <CMS:ChildPageMenuControl runat="server" id="PageChildMenuControl" Visible="False" />
</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <CMS:ContentControl runat="server" ID="PageSecondaryContentControl" PageContentType="Secondary" />
    <CMS:BreadCrumbControl runat="server" ID="PageBreadCrumbControl" />
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
    <CMS:ScriptControl runat="server" id="PageScriptControl" />
</asp:Content>