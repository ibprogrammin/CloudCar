<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Details.aspx.vb" Inherits="CloudCar.CCContentManagement.CMSDetails" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
<style type="text/css">
    #ImageRotatorImage { background-image:url("/images/design/nice.view_03.jpg"); }
</style>
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

<div class="DefaultDiv">

    <img src="" alt="" id="imgMainProduct" runat="server" width="450" height="500" class="image-display" style="float: right;" />

    <div style="width: 325px; margin-left: 20px;">
        <h1 style="font-size: 60px; line-height: 60px;"><asp:Literal runat="server" ID="litProductTitle"/></h1><br />
        <p style="font-family: Times New Roman, Serif; font-size: 12px; line-height: 24px; color: #666;"><asp:Literal runat="server" ID="litProductDescription" /></p><br />
        <p><a href="/" class="BreadCrumb" title="Home">Home</a> &raquo; <a href="/Home/Products.html" title="Products" class="BreadCrumb">Products</a> &raquo; <asp:Literal ID="litBreadCrumb" runat="server" /></p>
    </div>

    <br style="clear: both;" />
    
    <div class="ProductContent" runat="server" visible="false">
        
        <a runat="server" ID="hlMoreImages" target="_blank">More Images</a>

        <h3>Reviews</h3>
        <asp:Repeater runat="server" ID="rptReviews">
            <ItemTemplate>
                <div style="background-color: #FCFCFC; padding: 10px; min-height: 70px; margin-top: 20px;">
                    <ajax:Rating runat="server" ID="ratProduct" Direction="LeftToRight" CurrentRating='<%# Eval("rating") %>' MaxRating="5" style="float: right;"
                        EmptyStarCssClass="StarEmpty" FilledStarCssClass="StarFilled" CssClass="RatingStar" StarCssClass="RatingItem" WaitingStarCssClass="StarWaiting" />
                    <label><%#Eval("Name")%></label>
                    <p style="width: 400px; font-size: 12px; margin-left: 5px;"><%#Eval("comment")%></p>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    
        <br /><br />
        <Store:AddProductReviewControl runat="server" ID="prcReview" />
    
    </div>

</div>
<br style="clear: both;" /><br /><br />

<div id="SmallCallToActionWrapper">
    
    <h2 id="CallToActionHeader" style="font-size: 36px; line-height: 36px; width: 300px; float: left; margin-left: 20px; margin-top: 45px; margin-bottom: 0px;">
        I like what I see! <br />Please tell me more.</h2>
    
    <div id="C2AButtonWrapper" style="margin-top: 35px;">
        <h3 class="C2AText" style="font-size: 21px;">Want to <b style="color:#FF862E;">learn</b> more about the prices and features of our <b style="color:#FF862E;">great</b> line up of windows and doors.</h3>
        
        <a id="GreenButton" href="/Home/Estimate.html">
            <h3>Get an Estimate!</h3>
            <span>(it's 100% free)</span>
        </a>
    </div> 
</div>

</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
<div id="ImageRotatorImage">
    <div id="DescriptionWrapper">
        <h2 id="RedHeading" style="font-size: 36px;">Looking for a NEW view?</h2>
        <p class="Serif ItalicText">Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor.</p>
        <a class="BlueLinks" href="/Home/Guarantee.html">See Our Guarantee<img class="BlueArrow" style="margin-left: 30px;" src="/images/design/blue_arrow.png" alt="" /></a>
    </div>
</div>
</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server"></asp:Content>