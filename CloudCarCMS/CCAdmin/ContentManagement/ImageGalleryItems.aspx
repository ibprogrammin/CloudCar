<%@ Page Title="Gallery Images" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="ImageGalleryItems.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.ImageGalleryItems" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Gallery Images
        <i class="icon-instagram"></i>
    </h1><hr />
    
    <asp:Label ID="lblStatus" runat="server" CssClass="status-message" Visible="false" />
    <asp:HiddenField ID="hfID" runat="server" />           

    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
		    <li class="tab"><a href="#tab-image">Image</a></li>
	    </ul>
        <div id="tab-details">
            <label>Title</label>
            <asp:TextBox ID="txtTitle" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label>Description</label>
            <asp:TextBox ID="txtDescription" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label>Gallery</label>
            <asp:DropDownList ID="ddlGallery" runat="server" DataValueField="ID" DataTextField="Title" CssClass="form-select-box" />
            <br class="clear-both" /><br />
        
            <label>Order</label>
            <asp:TextBox ID="txtOrder" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" />
        </div>
        <div id="tab-image">
            <label style="min-height: 110px;">
                <br />Image<br />
                <img runat="server" ID="imgImage" alt="Image Preview" visible="false" src="" class="image-display" />
            </label><br />
            <asp:HiddenField runat="server" ID="hfImageID" />
            <asp:Label runat="server" ID="lblImageLocation" ReadOnly="true" Visible="false" CssClass="display-message" />
            <div class="form-file-upload-display">
	            <div class="form-fake-upload">
		            <input type="text" name="headerfilename" readonly="readonly" /> <!-- browse button is here as background -->
	            </div>
                <asp:FileUpload runat="server" ID="fuImage" ToolTip="Select the header image for this product." size="20" CssClass="form-real-upload" onchange="this.form.headerfilename.value = this.value;" /><br />
            </div><br class="clear-both" /><br />
        </div>
    </div>

    <br class="clear-both" />
    
    <asp:Button ID="btnSave" Text="Save" CssClass="SaveButton" runat="server" CausesValidation="true" ValidationGroup="ValidateForm" />
    <asp:Button ID="btnClear" Text="Clear" CssClass="DeleteButton" runat="server" />

    <asp:RequiredFieldValidator ID="rfvT" runat="server" ControlToValidate="txtTitle" ErrorMessage="You should create a title for this image gallery." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceT" runat="server" TargetControlID="rfvT" />
    
    <asp:RequiredFieldValidator ID="rfvGallery" runat="server" ControlToValidate="ddlGallery" ErrorMessage="You should select a gallery for this image gallery item." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceGallery" runat="server" TargetControlID="rfvGallery" />
    
    <asp:RequiredFieldValidator ID="rfvOrder" runat="server" ControlToValidate="txtOrder" ErrorMessage="You should enter an order for this image gallery item." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceOrder" runat="server" TargetControlID="rfvOrder" />

    <br class="clear-both" />
    <hr />
    
    <asp:DataGrid runat="server" ID="gvItems" DataKeyField="id" AllowCustomPaging="false" DataKeyNames="ID" AllowSorting="True" AutoGenerateColumns="False" 
            PageSize="10" AllowPaging="true" GridLines="None" CssClass="default-table" CellPadding="0" CellSpacing="0">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="Title">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnSelect" OnCommand="btnSelect_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Select" Text='<%# Eval("Title") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn HeaderText="Description" DataField="Description" />
            <asp:BoundColumn HeaderText="Order" DataField="Order" HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" />
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDelete" OnCommand="btnDelete_Command" CommandArgument='<%# Eval("id") %>' CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you wish to delete this image gallery item? Your changes cannot be undone.');" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

</asp:Content>