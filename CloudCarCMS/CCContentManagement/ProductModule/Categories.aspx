<%@ Page Title="Categories" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Categories.aspx.vb" Inherits="CloudCar.CCContentManagement.CMSProductCategories" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">

<style type="text/css">
    #ImageRotatorImage { background-image:url("/images/design/estimate.header.jpg"); }
</style>

</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">


<asp:Repeater ID="rptCategories" runat="server">
    <ItemTemplate>
        <div class="ProductCategoryControl">
            <asp:Image runat="server" Style="height: 100px; max-width: 135px; max-height: 100px;" 
                ImageUrl='<%# String.Format("/images/db/{0}/135/{1}.jpg", GetCategoryImage(CInt(Eval("ID"))), Eval("permalink")) %>'
                Visible='<%# GetCategoryImage(CInt(Eval("ID"))) <> 0 %>' />
    	    <br /><br />
            <asp:HyperLink runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# String.Format("/Product/{0}.html", Eval("Permalink")) %>' />
            <asp:Literal runat="server" Text='<%# GetCategoryProducts(CInt(Eval("ID"))) %>' Visible="false" />
        </div>
    </ItemTemplate>
</asp:Repeater>

<asp:DataList ID="dlTopSellers" DataKeyField="ID" runat="server" RepeatColumns="3" RepeatDirection="Vertical" Width="100%" CellSpacing="0" CellPadding="0" Visible="false">
    <HeaderTemplate>
        <br />
        <hr class="MediumOrangeLine" style="float: left; clear: both;" />
        <h2 class="SerifStack" style="clear: both;">Top Sellers</h2>
    </HeaderTemplate>
    <ItemTemplate>
        <Store:ProductControl ProductID='<%# Eval("ID") %>' ID="pcProduct" runat="server" OnCartItemAdded="pcProduct_CartItemAdded" OnAddMembership="pcProduct_AddMembership" />
    </ItemTemplate>
</asp:DataList>
<asp:DataList ID="dlClearance" DataKeyField="ID" runat="server" RepeatColumns="3" RepeatDirection="Vertical" Width="100%" CellSpacing="0" CellPadding="0" Visible="false">
    <HeaderTemplate>
        <br />
        <hr class="MediumOrangeLine" style="float: left; clear: both;" />
        <h2 class="SerifStack" style="clear: both;">Clearance</h2>
    </HeaderTemplate>
    <ItemTemplate>
        <Store:ProductControl ProductID='<%# Eval("ID") %>' ID="pcProduct" runat="server" OnCartItemAdded="pcProduct_CartItemAdded" OnAddMembership="pcProduct_AddMembership" />
    </ItemTemplate>
</asp:DataList>
<asp:SqlDataSource runat="server" ID="sdsCategories" />
<asp:SqlDataSource runat="server" ID="sdsTopSellers" />
        
<asp:DataList ID="dlBrands" runat="server" DataKeyField="ID" RepeatColumns="6" RepeatDirection="Vertical" Width="100%" CellSpacing="10" CellPadding="0" Visible="false">
    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
    <HeaderTemplate>
        <br />
        <hr class="MediumOrangeLine" style="float: left; clear: both;" />
        <h2 class="SerifStack" style="clear: both;">Brands</h2>
    </HeaderTemplate>
    <ItemTemplate>
        <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("/Shop/Brand/{0}.html", Eval("Permalink")) %>'>
            <asp:Image runat="server" Style="margin-right: 10px; vertical-align: middle; float: left;" 
                ImageUrl='<%# String.Format("/images/db/{0}/100/{1}", Eval("LogoImageID"), Eval("permalink")) %>'
                Visible='<%# CInt(Eval("LogoImageID")) <> 0 %>' />
            <asp:Label runat="server" Text='<%# Eval("Name") %>' Visible='<%# CInt(Eval("LogoImageID")) = 0 %>' />
        </asp:HyperLink>
    </ItemTemplate>
</asp:DataList>
    
<Store:QuickShopControl runat="server" ID="qssQuickShop" Visible="false" />



<div id="SideCallToActionWrapper" style="height: 595px; margin-left: 30px;">

    <h2 id="SmallCallToActionHeader">Check out these great products?</h2><br /><br />
    <p>Take a look at our wide product selection and don't be afraid to give us a call if you have any questions. We have a great deal of vinyl and alumminum windows, doors and siding products for you to choose, from some of the most well known and reputable manufacturers in the industry. Name's such as Gentek, Lexa and MDL are all known to bring a great quality product into your home. Combined with our quality of service it's a match that can't be beat.</p><br />
    <p>Here you will find a great deal of information on our product lines, combined with easily viewable images for you to get an idea of the kind of quality we provide. All our products are backed by a manufacturers warranty and our own lifetime warranty on all our installation work. You can find more information on the manufacturers warranty by browsing the specific product and going to the bottom of the page. If you require any more information or have any questions don't hessitate to call us and speak to one of our salesmen.</p>

</div>


<div id="WindowsWrapper">        
    <h1 class="LargeBlueHeading"><a href="/Home/Products/Windows.html" class="ProductCategoryLink">Windows</a></h1>        
    <p class="ProductCategoryParagraph">Your perfect window is waiting with our large selection of quality products that will be sure to suit you style and your budget.</p>        
    <div class="SmallImageContainer">            
        <img alt="" src="/images/design/windows_img.png" />
    </div>
</div>        

<div id="DoorsWrapper">        
    <h1 class="LargeBlueHeading"><a href="/Home/Products/Doors.html" class="ProductCategoryLink">Doors</a></h1>
    <p class="ProductCategoryParagraph">Your homes first impression is made simply with a knock, so with our array of luxury doors your impression will surely be met with awe.</p>        
    <div class="SmallImageContainer">
        <img alt="" src="/images/design/doors_img.png" />
    </div>    
</div>

<div id="SidingWrapper">        
    <h1 class="LargeBlueHeading"><a href="/Home/Products/Siding.html" class="ProductCategoryLink">Siding</a></h1>
    <p class="ProductCategoryParagraph">Our skilled workers have been completeing home renovations with persision, and throughoness for years. We pride ourselves on your level of satisfaction.</p>        
    <div class="SmallImageContainer">
        <img alt="" src="/images/design/siding_img.png" />
    </div>
</div>

<div id="RenovationWrapper" style="margin-bottom: 0px; ">       
    <h1 class="LargeBlueHeading"><a href="/Home/Products/Renovations.html" class="ProductCategoryLink">Renovations</a></h1>
    <p class="ProductCategoryParagraph">Our skilled workers have been completeing home renovations with persision, and throughoness for years. We pride ourselves on your level of satisfaction.</p>        
    <div class="SmallImageContainer">
        <img alt="" src="/images/design/siding_img.png" />
    </div>
</div>







<br style="clear: both;" /><br />

<p><a href="/" class="BreadCrumb" style="margin-left: 50px;">Home</a> &raquo; Products</p>


    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">

    <CMS:NivoRotatorControl runat="server" ID="RcNivoRotator" />

    <div id="ImageRotatorImage">
        <div id="DescriptionWrapper">
            <h2 id="RedHeading" style="font-size: 36px;">Renovating?</h2>
            <p class="Serif ItalicText">We have a vast selection of top quality products for you to browse through. From <a href="/Home/burlington-replacement-windows.html" title="Replacement Windows in Burlington">replacement windows</a> and <a href="/Home/Products/Doors.html" title="Doors in Burlington">doors</a> to siding and evestrough 
            and everything in between. Take your time! Look through our home renovation products online or visit our Burlington showroom and talk to one of our 
            renovation consultants..</p><br />
            <a class="BlueLinks" href="/Home/Guarantee.html">See Our Guarantee<img class="BlueArrow" style="margin-left: 30px;" src="/images/design/blue_arrow.png" alt="" /></a>
            
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server"></asp:Content>