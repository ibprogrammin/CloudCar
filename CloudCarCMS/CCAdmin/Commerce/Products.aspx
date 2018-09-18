<%@ Page Title="View Store Products" EnableViewStateMac="false" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Products.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Products" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Products
        <i class="icon-tags"></i>
    </h1>
    <hr />


<asp:UpdatePanel runat="server" ID="upUpdate" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Always">
    <ContentTemplate>

<div class="search-bar">
    <asp:TextBox ID="txtProductNumber" runat="server" style="width: 30px;" />
    <asp:TextBox ID="txtProductName" runat="server" style="width: 160px;" />
    <asp:DropDownList ID="ddlBrands" runat="server" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" style="width: 120px;">
        <asp:ListItem Value="" Text="Brand" />
    </asp:DropDownList>
    <asp:DropDownList ID="ddlCategories" runat="server" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" style="width: 120px;">
        <asp:ListItem Value="" Text="Category" />
    </asp:DropDownList>
    <asp:DropDownList ID="ddlActiveFilter" runat="server" style="width: 120px;">
        <asp:ListItem Value="" Text="Active" />
        <asp:ListItem Value="True" Text="Yes" />
        <asp:ListItem Value="False" Text="No" />
    </asp:DropDownList>
    <asp:Button id="btnSearch" runat="server" CssClass="OrangeButton" CausesValidation="true" Text="Search" style="width: 120px;" />
    <asp:Button runat="server" UseSubmitBehavior="true" PostBackUrl="~/CCAdmin/Commerce/ProductDetails.aspx" CssClass="SaveButton" style="float: right; width: 130px; margin-top: 0px; margin-bottom: 0px;" Text="New" />    
</div>

<ajax:TextBoxWatermarkExtender runat="server" ID="tbwPNo" TargetControlID="txtProductNumber" WatermarkText="#" WatermarkCssClass="Watermark" />
<ajax:TextBoxWatermarkExtender runat="server" ID="tbwPName" TargetControlID="txtProductName" WatermarkText="Name" WatermarkCssClass="Watermark" />

<asp:Label runat="server" ID="lblMessage" CssClass="status-message" />

<table class="default-table">
    <thead>
        <tr class="default-table-header">
            <td>Item</td>
            <td></td>
            <td></td>
            <td style="width: 150px;"></td>
            <td></td>
        </tr>
    </thead>
<asp:ListView ID="lvProducts" runat="server" DataKeyNames="ID">
    <LayoutTemplate>
        <asp:PlaceHolder runat="server" id="itemPlaceholder" />
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("~/CCAdmin/Commerce/ProductDetails.aspx?Product={0}", Eval("ID"))  %>' Text="Edit" /><br />
                <asp:LinkButton id="btnMakeInactive" runat="server" OnCommand="btnMakeInactive_Command" Visible='<%# Eval("active") %>' CommandArgument='<%# Eval("ID") %>' Text="Disable" />
                <asp:LinkButton id="btnMakeActive" runat="server" OnCommand="btnMakeActive_Command" Visible='<%# NOT CBool(Eval("active")) %>' CommandArgument='<%# Eval("ID") %>' Text="Enable" />
            </td>
            <td>
                <div>
                    <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# String.Format("/images/db/{0}/90/{1}.jpg",Eval("DefaultImageID"), Eval("Name"))%>' AlternateText="" />
                </div>
                <br />
            </td>
            <td>
                <asp:Hyperlink ID="hlPName" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# String.Format("~/CCAdmin/Commerce/ProductDetails.aspx?Product={0}", Eval("ID")) %>' /><br />
                <asp:Label ID="lblPDescription" runat="server" Text='<%# GetDescription(CType(Eval("Description"), String)) %>' Width="99%" />
            </td>
            <td>
                <asp:Label ID="lblPPrice" runat="server" Text='<%# "Price <b>" & Eval("Price", "{0:C}") & "</b>" %>' ForeColor="Red" /><br />
                <b>Category</b> <asp:Label ID="lblPCategory" runat="server" Text='<%# Eval("Category.Name") %>' /><br />
                <b>Brand</b> <asp:Label ID="lblPBrand" runat="server" Text='<%# Eval("Brand.Name") %>' />
            </td>
            <td style="text-align: right;">
                <asp:Hyperlink runat="server" Text='<%# Eval("ID") %>' NavigateUrl='<%# String.Format("~/CCAdmin/Commerce/ProductDetails.aspx?Product={0}", Eval("ID")) %>' />
            </td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr>
            <td>
                <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("~/CCAdmin/Commerce/ProductDetails.aspx?Product={0}", Eval("ID"))  %>' Text="Edit" /><br />
                <asp:LinkButton id="btnMakeInactive" runat="server" OnCommand="btnMakeInactive_Command" Visible='<%# Eval("active") %>' CommandArgument='<%# Eval("ID") %>' Text="Disable" />
                <asp:LinkButton id="btnMakeActive" runat="server" OnCommand="btnMakeActive_Command" Visible='<%# NOT CBool(Eval("active")) %>' CommandArgument='<%# Eval("ID") %>' Text="Enable" />
            </td>
            <td>
                <div>
                    <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# String.Format("/images/db/{0}/90/{1}.jpg",Eval("DefaultImageID"), Eval("Name"))%>' AlternateText="" CssClass="image-display-table" />
                </div>
                <br />
            </td>
            <td>
                <asp:Hyperlink ID="hlPName" runat="server" Text='<%# Eval("Name") %>' NavigateUrl='<%# String.Format("~/CCAdmin/Commerce/ProductDetails.aspx?Product={0}", Eval("ID")) %>' /><br />
                <asp:Label ID="lblPDescription" runat="server" Text='<%# GetDescription(CType(Eval("Description"), String)) %>' Width="99%" />
            </td>
            <td>
                <asp:Label ID="lblPPrice" runat="server" Text='<%# "Price <b>" & Eval("Price", "{0:C}") & "</b>" %>' ForeColor="Red" /><br />
                <b>Category</b> <asp:Label ID="lblPCategory" runat="server" Text='<%# Eval("Category.Name") %>' /><br />
                <b>Brand</b> <asp:Label ID="lblPBrand" runat="server" Text='<%# Eval("Brand.Name") %>' />
            </td>
            <td style="text-align: right;">
                <asp:Hyperlink runat="server" Text='<%# Eval("ID") %>' NavigateUrl='<%# String.Format("~/CCAdmin/Commerce/ProductDetails.aspx?Product={0}", Eval("ID")) %>' />
            </td>
        </tr>
    </AlternatingItemTemplate>
</asp:ListView>
    <tr>
        <td colspan="5">
            <asp:DataPager ID="dpPagerBottom" runat="server" PageSize="10" PagedControlID="lvProducts" style="float: right; margin: 4px; font-size: 12px;">
                <Fields>
                    <asp:NextPreviousPagerField ButtonType="Link" ShowPreviousPageButton="true" ShowNextPageButton="false" ShowFirstPageButton="true" ShowLastPageButton="false" PreviousPageText="Previous" FirstPageText="&nbsp;&laquo;&nbsp;" />
                    <asp:NumericPagerField ButtonCount="10" ButtonType="Link" RenderNonBreakingSpacesBetweenControls="true" CurrentPageLabelCssClass="PagerStyle" />
                    <asp:NextPreviousPagerField ButtonType="Link" ShowNextPageButton="true" ShowLastPageButton="true" ShowFirstPageButton="false" ShowPreviousPageButton="false" NextPageText="Next" LastPageText="&nbsp;&raquo;&nbsp;" />
                </Fields>
            </asp:DataPager>            
        </td>
    </tr>
</table>


<asp:SqlDataSource ID="dsBrands" runat="server" ConnectionString="<%$ ConnectionStrings:MainConnectionString %>" />
<asp:SqlDataSource ID="dsCategories" runat="server" ConnectionString="<%$ ConnectionStrings:MainConnectionString %>" />
<asp:SqlDataSource ID="dsProducts" runat="server" ConnectionString="<%$ ConnectionStrings:MainConnectionString %>" />

    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress AssociatedUpdatePanelID="upUpdate" runat="server" ID="upProgress" DynamicLayout="false" >
    <ProgressTemplate>
        <div class="loading-box"><h3 style="text-align: center; position: relative; top: 32px;">Loading please wait...</h3></div>
    </ProgressTemplate>
</asp:UpdateProgress>

    <br class="clear-both"/><br />

</asp:Content>

<asp:Content ContentPlaceHolderID="cphBeforeContent" runat="server" />
<asp:Content ContentPlaceHolderID="cphAfterContent" runat="server" />
