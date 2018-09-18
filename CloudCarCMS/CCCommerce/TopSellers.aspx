<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="TopSellers.aspx.vb" Inherits="CloudCar.CCCommerce.TopSellers" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" EnableViewState="False" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" EnableViewState="False" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />
</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <h1><asp:Literal runat="server" ID="PageHeadingLiteral" Text="Category Name Here" /></h1>
    <hr />
    
    <section class="store-navigation">
        <Store:CategoriesControl ID="CategoriesControl1" runat="server" />
        <Store:QuickShopControl ID="QuickShopControl1" runat="server" Visible="False" />
        
        <CMS:NivoRotatorControl runat="server" ID="PageRotatorControl" EnableViewState="False" />
        <CMS:HeaderImageControl runat="server" Id="PageHeaderImageControl" EnableViewState="False" />
        <img src="/CCTemplates/Default/images/rotator.item.01.jpg" />

        <br class="clear-both" />
    </section>
    
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
    <Store:ShoppingProgressControl ID="ShoppingProgressControl1" runat="server" Progress="Browse" Visible="False" />
    
    <asp:HiddenField ID="lblDummy" runat="server" Value="" />

    <br class="clear-both" />
    
    <asp:Label runat="server" ID="StatusMessageLabel" />

    <asp:Repeater ID="ProductRepeater" runat="server">
        <ItemTemplate>
            <Store:ProductControl ProductID='<%# Eval("ID") %>' ID="pcProduct" runat="server" OnCartItemAdded="ProductControlCartItemAdded" OnAddMembership="ProductControlAddMembership" OnOutOfStock="ProductControlOutOfStock" />
        </ItemTemplate>
    </asp:Repeater>

    <br class="clear-both" />
    
    <span style="font-size: 12px;"><asp:Literal runat="server" ID="ProductCountLiteral" /></span><br /><br />

    <h3>Refine</h3>
    <label>Price: </label>
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
    <br />

    <label>Brand: </label>
    <asp:DropDownList runat="server" 
        ID="BrandDropDown" 
        DataValueField="Id"
        DataTextField="Name"
        AutoPostBack="true" 
        CssClass="refine-select-box" 
        OnSelectedIndexChanged="FilterDropDownSelectedIndexChanged" />
    <br />
    
    <label>Color: </label>
    <asp:DropDownList runat="server" 
        ID="ColorDropDown" 
        DataValueField="Id"
        DataTextField="Name"
        AutoPostBack="true" 
        CssClass="refine-select-box"
        OnSelectedIndexChanged="FilterDropDownSelectedIndexChanged" />
    <br /><br />
    
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
    <CMS:ContentControl runat="server" ID="PageContentControl" PageContentType="Primary" EnableViewState="False" />

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <CMS:ChildPageMenuControl runat="server" id="PageChildMenuControl" Visible="False" EnableViewState="False" />
    <CMS:ContentControl runat="server" ID="PageSecondaryContentControl" PageContentType="Secondary" EnableViewState="False" />
    <CMS:CallToActionControl runat="server" ID="PageCallToActionControl" />
    <CMS:BreadCrumbControl runat="server" ID="PageBreadCrumbControl" EnableViewState="False" />
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
    <CMS:ScriptControl runat="server" id="PageScriptControl" EnableViewState="False" />
</asp:Content>