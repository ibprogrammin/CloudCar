<%@ Page Title="Products" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Products.aspx.vb" Inherits="CloudCar.CCContentManagement.CMSProductsPage" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

<div id="SideCallToActionWrapper" style="margin-left: 30px; margin-top: 10px; padding-bottom: 20px;">
    <asp:Literal runat="server" ID="litSideContent" />
</div>

<div style="margin-left: 20px; float: left; width: 680px;">

    <div style="width: 380px; margin-bottom: 0px; margin-left: 30px;">
        
        <img id="imgIcon" src="" runat="server" visible="false" style="float: right; margin-top: 10px;" alt="" />
        
        <h1><asp:Literal runat="server" ID="litCategoryTitle" /></h1>
        <asp:Literal runat="server" ID="litPageContent" />
        
    </div>
    
    <asp:Label runat="server" ID="lblStatus" />

    <asp:Repeater ID="rptProducts" runat="server">
        <ItemTemplate>
            <CMS:ProductControl ID="pcProduct" runat="server" ProductID='<%# Eval("ID") %>' />
        </ItemTemplate>
    </asp:Repeater>
</div>

<div style="margin-left: 40px; float: left; width: 640px; background-color: #F7F7F7; margin-top: 30px;">
    <asp:Literal runat="server" ID="litBottomContent" />
</div>

<br style="clear: both;" /><br />

<p style="margin-left: 50px;"><a href="/" class="BreadCrumb" title="Home">Home</a> &raquo; <a href="/Home/Products.html" title="Shop" class="BreadCrumb">Products</a> &raquo; <asp:Literal ID="litBreadCrumb" runat="server" /></p>

</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">

<div id="ImageRotatorImage">
    <div id="DescriptionWrapper">
        <h2 id="RedHeading" style="font-size: 36px;">Looking for a NEW view?</h2>
        <p class="Serif ItalicText">For over 25 years, the family's of Burlington and Hamilton alike have trusted their homes to the quality, 
           service and experience Brant Windows provides in <a href="/Home/burlington-replacement-windows.html" title="Replacement Windows in Burlington">replacement windows</a>, doors, siding and home renovations.</p>
        <a class="BlueLinks" href="/Home/Guarantee.html">See Our Guarantee<img class="BlueArrow" style="margin-left: 30px;" src="/images/design/blue_arrow.png" alt="" /></a>
    </div>
</div>

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server"></asp:Content>