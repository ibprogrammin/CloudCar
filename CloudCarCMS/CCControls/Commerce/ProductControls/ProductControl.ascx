<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProductControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.ProductControls.ProductControl" %>

<div>
    <div class="image">
        <a href="#">
            <img id="ProductImage" runat="server" title="" alt="" src="" class="" />
        </a>
    </div>
    <div class="name">
        <asp:HyperLink ID="NameHyperLink" runat="server" Text="Product Name" />
    </div>
    <div class="description">
        <asp:Label runat="server" ID="StatusMessageLabel" CssClass="product-sale-display" Text="Temporarily Sold Out" Visible="false" EnableViewState="False" />
        <asp:Literal runat="server" ID="DescriptionLiteral" /> 
        <asp:LinkButton runat="server" ID="FullDescriptionButton" Text="Read more &raquo;" Visible="false" />
    </div>
    <div class="price">
        <span class="price-new"><asp:Literal ID="PriceLiteral" runat="server" /></span>
        <span class="price-old"><asp:Literal runat="server" ID="ListPriceLiteral" /></span><br />
        
        <asp:Panel runat="server" ID="DiscountPanel" Visible="False" class="discount-display" />
        <span class="price-display"><asp:Literal runat="server" ID="SavingsLiteral" /></span>
    </div>
    <div class="cart">

        <asp:UpdatePanel runat="server" ID="ColorUpdatePanel">
            <ContentTemplate>
                <asp:Repeater runat="server" ID="ColorOptionsRepeater">
                    <ItemTemplate>
                        <asp:LinkButton ID="ColorLinkButton" runat="server" 
                            OnCommand="ColorSelectionClicked" 
                            CommandArgument='<%# Eval("Id") %>' 
                            CssClass="color-button"
                            Text='<%# if(CInt(Eval("Id")) = CurrentProductShoppingCartMediator.SelectedColor, "X", "") %>'
                            style='<%# String.Format("background-color: {0}", Eval("RGBColourCode")) %>' />
                    </ItemTemplate>
                </asp:Repeater>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:RadioButtonList ID="ColorRadioButton" runat="server" DataTextField="RGBColourCode" DataValueField="ID" CssClass="color-radio-button-list" Visible="False">
            <asp:ListItem Text="Colours" Value="" />
        </asp:RadioButtonList>

        <asp:DropDownList ID="SizeDropDown" runat="server" DataTextField="Name" DataValueField="ID" CssClass="size-drop-down">
            <asp:ListItem Text="Sizes" Value="" />
        </asp:DropDownList>
        
        <asp:TextBox ID="QuantityTextBox" runat="server" CssClass="quantity-text-box" size="4" />
        <br /><br />
        <asp:Button runat="server" ID="AddToCartButton" Text="Add To Cart" CausesValidation="true" ValidationGroup="CheckoutGroup" Enabled="true" ToolTip="Add this item to your Shopping Cart" CssClass="button" />

    </div>
    <div class="wishlist"><!--a onclick="addToWishList('43');">&#43;&nbsp;Add to Wish List</a--></div>
    <div class="compare"><!--a onclick="addToCompare('43');">&#43;&nbsp;Add to Compare</a--></div>
</div>


<div class="product-control" runat="server" visible="False">
    <div>
        <span></span>
        
        <br class="clear-both" />
    </div>
  

    <asp:HyperLink runat="server" ID="MoreImagesHyperLink" Text="+" Target="_blank" CssClass="more-images" ToolTip="View More Images For This Item" Visible="false" />
    <h4></h4>
    
    
    
    <p runat="server" visible="False">
        
    </p>
    
    
</div>

<asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="QuantityTextBox" ErrorMessage="Please enter the quantity of this item you would like to add to your shopping cart" Display="None" />
<ajax:TextBoxWatermarkExtender ID="tbwQuantity" runat="server" TargetControlID="QuantityTextBox" WatermarkText="Quantity" />
<ajax:ValidatorCalloutExtender ID="vceQuantity" runat="server" TargetControlID="rfvQuantity" PopupPosition="TopLeft"  />

<asp:Panel runat="server" ID="DescriptionPanel" CssClass="product-control-description" visible="false">
    <h3><asp:Label runat="server" ID="FullTitleLiteral" /></h3>
    <p><asp:Literal runat="server" ID="FullDescriptionLiteral" /></p>
</asp:Panel>

<ajax:AnimationExtender ID="aePopOut" runat="server" TargetControlID="FullDescriptionButton">
    <Animations>
        <OnMouseOver>
            <Sequence>
                <StyleAction AnimationTarget="DescriptionPanel" Attribute="display" Value="block"/>
                <Parallel AnimationTarget="DescriptionPanel" Duration=".2" Fps="30">
                    <FadeIn Duration=".2" />
                </Parallel>
            </Sequence>
        </OnMouseOver>
        <OnMouseOut>
            <Sequence AnimationTarget="DescriptionPanel">
                <Parallel AnimationTarget="DescriptionPanel" Duration=".2" Fps="30">
                    <FadeOut Duration=".2"/>
                </Parallel>
                <StyleAction Attribute="display" Value="none"/>
                <StyleAction Attribute="height" Value=""/>
                <StyleAction Attribute="width" Value="265px"/>
            </Sequence>
        </OnMouseOut>
    </Animations>
</ajax:AnimationExtender>