<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Colours.aspx.vb" Inherits="CloudCar.CCCommerce.Colours" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" EnableViewState="False" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" EnableViewState="False" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />
</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <CMS:BreadCrumbControl runat="server" ID="PageBreadCrumbControl" EnableViewState="False" BreadCrumbType="StoreColorPage" />

    <h1><asp:Literal runat="server" ID="PageHeadingLiteral" Text="Category Name Here" /></h1>
    <CMS:NivoRotatorControl runat="server" ID="RcNivoRotator" />
    
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
    <Store:ShoppingProgressControl ID="ShoppingProgressControl1" runat="server" Progress="Browse" Visible="false" />
    <asp:HiddenField ID="lblDummy" runat="server" Value="" />
    <asp:Label runat="server" ID="StatusMessageLabel" />
    
    <div class="product-filter"><!-- Product Filter -->
        <div class="display"><b>Display:</b> List <b>/</b> <a onclick="display('grid');">Grid</a></div>

        <div class="limit">
            <b>Price: </b>
            <asp:DropDownList runat="server" 
                ID="PriceRangeDropDown" 
                AutoPostBack="true" 
                CssClass="refine-select-box"
                OnSelectedIndexChanged="FilterDropDownSelectedIndexChanged">
                <asp:ListItem Text="All" Value="0" />
                <asp:ListItem Text="$0-$2.99" Value="0-2.99" />
                <asp:ListItem Text="$3.00-$3.59" Value="3.00-3.59" />
                <asp:ListItem Text="$3.60-$3.99" Value="3.60-3.99" />
                <asp:ListItem Text="$4.00 and up" Value="4.00" />
            </asp:DropDownList>
        </div>
                
        <div class="limit">
            <b>Brand: </b>
            <asp:DropDownList runat="server" 
                ID="BrandDropDown" 
                DataValueField="Id"
                DataTextField="Name"
                AutoPostBack="true" 
                CssClass="refine-select-box"
                OnSelectedIndexChanged="FilterDropDownSelectedIndexChanged" />
        </div>

    </div>

    <asp:Repeater ID="ProductRepeater" runat="server">
        <HeaderTemplate>
            <div class="product-list">
        </HeaderTemplate>
        <ItemTemplate>
            <Store:ProductControl ProductID='<%# Eval("ID") %>' ID="pcProduct" runat="server" OnCartItemAdded="ProductControlCartItemAdded" OnAddMembership="ProductControlAddMembership" OnOutOfStock="ProductControlOutOfStock" />
        </ItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    
    <br class="clear-both" />
    
    <div class="pagination">
	    <div class="links" style="display: none;">
	        <b>1</b>  
	        <a href="pagelink">2</a>  
	        <a href="pagelink">&gt;</a> 
	        <a href="pagelink">&gt;|</a> 
	    </div>
	    <div class="results"><asp:Literal runat="server" ID="ProductCountLiteral" /></div>
	</div> 

    <ajax:ModalPopupExtender runat="server" ID="mpeConfirm" PopupControlID="lblDisplaySelectedProducts" 
        TargetControlID="lblDummy" DropShadow="true" BackgroundCssClass="PopupBackground" CancelControlID="btnContinueShopping">
    </ajax:ModalPopupExtender>

    <asp:Label runat="server" ID="lblDisplaySelectedProducts" Visible="false" CssClass="ModalStyle" style="width: 325px; padding: 20px; height: 250px;">

        <asp:UpdatePanel ID="udpInnerUpdatePanel" runat="server">
            <ContentTemplate>

        <h2>Thank You</h2>
        <p>Your selection has been added to the shopping cart.</p>
    
        <br style="clear: both;" />
    
        <asp:LinkButton runat="server" ID="btnContinueShopping">&laquo; Continue Shopping</asp:LinkButton><br /><br />
        <asp:HyperLink runat="server" ID="hlGoToCart" Text="Browse Your Shopping Cart &raquo;" NavigateUrl="/Shop/ShoppingCart.html" />

        <br style="clear: both;" />

            </ContentTemplate>
        </asp:UpdatePanel>

    </asp:Label>
    
    <asp:Literal ID="PageContentLiteral" runat="server" />
    
    <script type="text/javascript">
        function display(view) {
            if (view == 'list') {
                $('.product-grid').attr('class', 'product-list');

                $('.product-list > div').each(function (index, element) {
                    html = '<div class="right">';

                    var price = $(element).find('.price').html();

                    if (price != null) {
                        html += '<div class="price">' + price + '</div>';
                    }

                    var rating = $(element).find('.rating').html();

                    if (rating != null) {
                        html += '<div class="rating">' + rating + '</div>';
                    }

                    html += '</div>';

                    html += '<div class="left">';

                    var image = $(element).find('.image').html();

                    if (image != null) {
                        html += '<div class="image">' + image + '</div>';
                    }

                    html += '  <div class="name">' + $(element).find('.name').html() + '</div>';
                    html += '  <div class="description">' + $(element).find('.description').html() + '</div>';

                    html += '  <div class="cart">' + $(element).find('.cart').html() + '</div>';
                    html += '  <div class="wishlist">' + $(element).find('.wishlist').html() + '</div>';
                    html += '  <div class="compare">' + $(element).find('.compare').html() + '</div>';



                    html += '</div>';


                    $(element).html(html);
                });

                $('.display').html('&nbsp;&nbsp;<a onclick="display(\'grid\');"><img src="/CCTemplates/Default/Images/grid.png"></a>&nbsp;<img src="/CCTemplates/Default/Images/list-active.png">');

                $.cookie('display', 'list');
            } else {
                $('.product-list').attr('class', 'product-grid');

                $('.product-grid > div').each(function (index, element) {
                    html = '';

                    var image = $(element).find('.image').html();

                    if (image != null) {
                        html += '<div class="image">' + image + '</div>';
                    }

                    html += '<div class="name">' + $(element).find('.name').html() + '</div>';
                    html += '<div class="description">' + $(element).find('.description').html() + '</div>';

                    var price = $(element).find('.price').html();

                    if (price != null) {
                        html += '<div class="price">' + price + '</div>';
                    }

                    /* var rating = $(element).find('.rating').html();
                
                    if (rating != null) {
                        html += '<div class="rating">' + rating + '</div>';
                    } */

                    html += '<div class="cart">' + $(element).find('.cart').html() + '</div>';
                    html += '<div class="wishlist">' + $(element).find('.wishlist').html() + '</div>';
                    html += '<div class="compare">' + $(element).find('.compare').html() + '</div>';

                    $(element).html(html);
                });

                $('.display').html('&nbsp;&nbsp;<img src="/CCTemplates/Default/Images/grid-active.png">&nbsp;<a onclick="display(\'list\');"><img src="/CCTemplates/Default/Images/list.png"></a>');

                $.cookie('display', 'grid');
            }
        }

        view = $.cookie('display');

        if (view) {
            display(view);
        } else {
            display('list');
        }
    </script> 

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
</asp:Content>