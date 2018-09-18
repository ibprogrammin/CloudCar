<%@ Page Title="Product Reviews" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="ProductReviews.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.ProductReviews" %>
<%@ Import Namespace="CloudCar.CCFramework.Commerce" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Product Review
        <i class="icon-star"></i>
    </h1>
    <hr />
    
    <asp:PlaceHolder runat="server" ID="phDetails" Visible="false">
        
        <div class="tab-container">
	        <ul class="tabs">
		        <li class="tab"><a href="#tab-content">Details</a></li>
	        </ul>
            <div id="tab-content">
                <label>Name</label>
                <div class="display-message"><asp:Literal runat="server" ID="litName" /></div>
                <br class="clear-both" /><br />
            
                <label>Email</label>
                <div class="display-message"><asp:Literal runat="server" ID="litEmail" /></div>
                <br class="clear-both" /><br />
            
                <label>Portrait URL</label>
                <div class="display-message"><asp:Literal runat="server" ID="litAvatarURL" /></div>
                <br class="clear-both" /><br />
            
                <label>Product</label>
                <div class="display-message"><asp:Literal runat="server" ID="litProduct" Text=" " /></div>
                <br class="clear-both" /><br />
            
                <label>Comment</label>
                <div class="display-message"><asp:Literal runat="server" ID="litComment" /></div>
                <br class="clear-both" /><br />
            
                <p class="align-right"><asp:Literal runat="server" id="litDate" /></p>
            
            </div>
        </div>

        <br class="clear-both" />

    </asp:PlaceHolder>    

    <asp:DataGrid runat="server" 
            ID="gvProductReviews" 
            DataKeyField="id" 
            AllowCustomPaging="false" 
            DataKeyNames="id" 
            AllowSorting="True" 
            AutoGenerateColumns="False" 
            PageSize="12" 
            AllowPaging="true" 
            CssClass="default-table">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="Name">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnSelect" OnCommand="btnSelect_Command" CommandArgument='<%# Eval("id") %>' CommandName="Select" Text='<%# Eval("Name") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Product">
                <ItemTemplate>
                    <asp:Literal ID="Literal1" runat="server" Text='<%# ProductController.GetProductName(CInt(Eval("ProductID"))) %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn HeaderText="Comment" DataField="comment" />
            <asp:BoundColumn HeaderText="Date" DataField="timestamp" DataFormatString="{0:d}" />
            <asp:BoundColumn HeaderText="Rating" DataField="rating" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" />
            <asp:TemplateColumn ItemStyle-HorizontalAlign="right" ItemStyle-Width="6%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDelete" OnCommand="btnDelete_Command" CommandArgument='<%# Eval("id") %>' CommandName="Delete" Text="" CssClass="icon-trash delete-icon" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

</asp:Content>