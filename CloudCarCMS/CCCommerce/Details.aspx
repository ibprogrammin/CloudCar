<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Details.aspx.vb" Inherits="CloudCar.CCCommerce.Details" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />
</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    
    <div runat="server" ID="StatusMessageLabel" Visible="false" class="warning"></div>

    <CMS:BreadCrumbControl runat="server" ID="PageBreadCrumbControl" EnableViewState="False" BreadCrumbType="StoreProductPage" />
    <br />
    <div class="product-info">
	    <div class="left">
            <a runat="server" ID="MoreImagesHyperLink" target="_blank" visible="False">More Images</a>
            <div class="image">
                <div class="pinterest">
                    <a id="PintrestAnchor" runat="server" href="" class="pin-it-button" count-layout="none">
                        <img src="//assets.pinterest.com/images/PinExt.png" alt="Pin It" />
                    </a>
                    <script type="text/javascript" src="//assets.pinterest.com/js/pinit.js"></script>
                </div>
                
                <asp:HiddenField runat="server" ID="DefaultImageHiddenField" Value="" />
                <asp:HiddenField runat="server" ID="DefaultSmallImageHiddenField" Value="" />
                <asp:HiddenField runat="server" ID="DefaultImageDescriptionHiddenField" Value="" />
            
            <asp:Repeater runat="server" ID="ImagesThumbnailRepeater">
                <HeaderTemplate>
                        <a id='zoom1' href="<%# DefaultImageHiddenField.Value%>" 
                            title="<%# DefaultImageDescriptionHiddenField.Value%>" 
                            class='cloud-zoom colorbox'
                            rel="position: 'inside', tint: '', tintOpacity: '', showTitle: false, softFocus: 'true', adjustX: -3, adjustY:-3, zoomWidth:350, zoomHeight:350">
                            <img id="image" src="<%# DefaultSmallImageHiddenField.Value%>" 
                                title="<%# DefaultImageDescriptionHiddenField.Value%>" 
                                alt="<%# DefaultImageDescriptionHiddenField.Value%>" />
                            <div class="zoomin"></div>
                        </a>
                    
                        <script type="text/javascript">
                            // Removes Title Tag from zoom icon
                            var elements = document.getElementsByTagName('a');
                            for (var i = 0, len = elements.length; i < len; i++) {
                                elements[i].removeAttribute('title');
                            }
                        </script>

                        <script type="text/javascript">
                            $('.colorbox').colorbox({
                                overlayClose: true,
                                opacity: 0.5
                            });
                        </script>

                    </div>
                    <div class="image-additional">
                </HeaderTemplate>
                <ItemTemplate>
                    <a href="<%# String.Format("/images/db/{0}/full/{1}", Eval("ImageID"), PictureController.GetPictureFilename(CInt(Eval("ImageID"))))%> "
                        class='cloud-zoom-gallery' 
                        title='<%# Eval("Description") %>'
                        rel="useZoom: 'zoom1', smallImage: '/images/db/<%# Eval("ImageID")%>/350/<%# PictureController.GetPictureFilename(CInt(Eval("ImageID")))%>'">
                        <img src='<%# String.Format("/images/db/{0}/80/{1}", Eval("ImageID"), PictureController.GetPictureFilename(CType(Eval("ImageID"), Integer)))%>' alt='<%# Eval("Description") %>' title='<%# Eval("Description") %>' />
                    </a> 
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
	    </div>
	    <div class="right"><!-- Right Side -->
	        <h1><asp:Literal runat="server" ID="ProductTitleLiteral"/></h1>
		    <div class="description">
		        <span>Brand:</span> <asp:Literal runat="server" ID="ProductBrandLiteral" /><br />
				<span>Product Code:</span> <asp:Literal runat="server" ID="ProductCodeLiteral" /><br />
				<span>Availability:</span> <asp:Literal runat="server" ID="AvailabilityLiteral" />
                <asp:Label runat="server" ID="SoldOutLabel" style="color: Red; font-size: 12px; font-weight: bold;" Text="Temporarily Sold Out" Visible="false" />
		    </div>
			<div id="tabs" class="htabs">
			    <a href="#tab-info" class="info selected" style="display: inline;">
			        <img src="/CCTemplates/Default/Images/info-icon.png" alt="">
			    </a> <!-- Tab info starts here -->
			    <a href="#tab-description" style="display: inline;">Description</a>
				<a href="#tab-review" style="display: inline;"><asp:Literal runat="server" ID="ReviewsLinkLiteral" /></a>
			</div>
	        <div id="tab-info" class="tab-content" style="display: block;"> <!-- Tab info START -->
				<div class="price">Price: 
                    <span class="price-old"><asp:Literal runat="server" ID="ProductListPrice" /></span> 
                    <span class="price-new"><asp:Literal runat="server" ID="ProductPriceLiteral" /><br></span>
				    
                </div>
			    <div class="cart">
			        <b>Size</b><br />
                    <asp:DropDownList ID="ProductSizeDropDownList" runat="server" DataTextField="Name" DataValueField="ID">
                        <asp:ListItem Text="Sizes" Value="" />
                    </asp:DropDownList><br /><br />
        
                    <b>Colour/Finish</b><br />
                    <asp:DropDownList ID="ProductColorDropDownList" runat="server" DataTextField="Name" DataValueField="ID">
                        <asp:ListItem Text="Colours" Value="" />
                    </asp:DropDownList><br /><br />
        
                    <div>
                        <b>Quantity</b>
			            <input type="button" class="quantity-arrow-subtract" onclick="var quantity_el = document.getElementById('ProductQuantityTextBox'); var quantity = quantity_el.value; if( !isNaN( quantity ) && quantity > 0 ) quantity_el.value--;return false;">
    					<asp:TextBox ID="ProductQuantityTextBox" ClientIDMode="Static" runat="server" size="1" />
                        <input type="button" class="quantity-arrow-add" onclick="var quantity_el = document.getElementById('ProductQuantityTextBox'); var quantity = quantity_el.value; if( !isNaN( quantity )) quantity_el.value++;return false;">

                        <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="ProductQuantityTextBox" ErrorMessage="Please enter a quantity for this product." Display="None" />
                        <ajax:TextBoxWatermarkExtender ID="tbwQuantity" runat="server" TargetControlID="ProductQuantityTextBox" WatermarkText="Qty." />
                        <ajax:ValidatorCalloutExtender ID="vceQuantity" runat="server" TargetControlID="rfvQuantity" PopupPosition="TopLeft"  />

					    &nbsp;
                        <asp:Button runat="server" ID="AddToCartButton" Text="Add To Cart" CssClass="button" CausesValidation="true" ValidationGroup="CheckoutGroup" Enabled="true" Visible="true" OnClick="AddToCartButtonClick" />
				    </div>
				    <div><span>&nbsp;&nbsp;&nbsp;</span></div>
				    <!--div>
					    <a onclick="addToWishList('49');">+&nbsp;Add to Wish List</a><br>
					    <a onclick="addToCompare('49');">+&nbsp;Add to Compare</a>
				    </div-->
			    </div>
                <div class="review">
				    <div>
				        <ajax:Rating runat="server" 
                            ID="AverageProductRating" 
                            Direction="LeftToRight" 
                            CurrentRating='<%# Eval("rating") %>' 
                            MaxRating="5" 
                            EmptyStarCssClass="star-empty" 
                            FilledStarCssClass="star-filled" 
                            CssClass="rating-star" 
                            StarCssClass="rating-item" 
                            WaitingStarCssClass="star-waiting" />
					    &nbsp;&nbsp;
					    <a onclick="$('a[href=\'#tab-review\']').trigger('click'); $('html, body').animate({scrollTop: $('#tabs').offset().top}, 900);">0 reviews</a>&nbsp;&nbsp;|&nbsp;&nbsp;
					    <a onclick="$('a[href=\'#tab-review\']').trigger('click'); $('html, body').animate({scrollTop: $('#review-title').offset().top}, 900);">Write a review</a>
				    </div>
                    <br style="clear: both;" />
				    <div class="share">
	                    <!-- AddThis Button BEGIN -->
	                    <div class="addthis_toolbox addthis_default_style " style="height: 25px;">
	                        <a class="addthis_button_facebook_like" <%= "fb:like:layout=""button_count"""%>></a>
	                        <a class="addthis_button_tweet"></a>
	                        <a class="addthis_button_google_plusone" <%= "g:plusone:size=""medium"""%>></a>
	                        <a class="addthis_counter addthis_pill_style"></a>
	                    </div>
                        <script type="text/javascript">var addthis_config = { "data_track_addressbar": false };</script>
	                    <script type="text/javascript" src="http://s7.addthis.com/js/300/addthis_widget.js#pubid=ra-4e9eade130d4e427"></script>
                    </div>
			    </div>
		    </div>
		    <div id="tab-description" class="tab-content" style="display: none;">
		        <asp:Literal runat="server" ID="ProductDescriptionLiteral" />
            </div>
            <div id="tab-review" class="tab-content" style="display: none;">
                <div id="review">
                    <asp:Repeater runat="server" ID="ReviewsRepeater">
                        <HeaderTemplate>
                            <div class="review-list">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <ajax:Rating runat="server" 
                                    ID="ratProduct" 
                                    Direction="LeftToRight" 
                                    CurrentRating='<%# Eval("rating") %>' 
                                    MaxRating="5" 
                                    style="float: right;"
                                    EmptyStarCssClass="star-empty" 
                                    FilledStarCssClass="star-filled" 
                                    CssClass="rating-star" 
                                    StarCssClass="rating-item" 
                                    WaitingStarCssClass="star-waiting" />
                            <div class="author"><b><%#Eval("Name")%></b></div>
                            <div class="text"><%#Eval("comment")%></div>
                            <!-- TODO Add: Was this review helpful to you? yes/no to show quality reviews at the top -->
                        </ItemTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>

                    <div class="content">There are no reviews for this product.</div>
                </div>
                
                <Store:AddProductReviewControl runat="server" ID="prcReview" />

            </div>
        </div>
    </div>

    <script type="text/javascript">
        $('#tabs a').tabs();
    </script> 

</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
    <store:RecomendedProductControl runat="server" id="DetailsRecomendedProductControl" />
    <Store:ShoppingProgressControl ID="ShoppingProgressControl1" runat="server" Progress="Browse" Visible="False" />

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" Visible="false" />
</asp:Content>