<%@ Page Title="Image Gallery" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="ImageGalleries.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.ImageGalleries" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Image Gallery
        <i class="icon-instagram"></i>
    </h1><hr />

    <asp:Button runat="server" ID="btnClear" Text="New" CausesValidation="false" CssClass="GreenButton NewButton" />       
    <asp:ListBox runat="server" ID="lbImageGalleries" DataValueField="id" DataTextField="Title" AutoPostBack="true" CssClass="form-list-box" Rows="3" />
    
    <br class="clear-both" />
    <hr />

    <asp:Label ID="lblStatus" runat="server" CssClass="status-message" Visible="false" />
    
    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
        </ul>
        <div id="tab-details">
            
            <asp:HiddenField ID="hfID" runat="server" />           
        
            <label>Title</label>
            <asp:TextBox ID="txtTitle" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label>Description</label>
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-text-area" />
            <br class="clear-both" /><br />
        
        </div>
    </div>
    
    <br class="clear-both" />
    
    <asp:Button ID="btnSave" Text="Save" CssClass="SaveButton" runat="server" CausesValidation="true" ValidationGroup="ValidateForm" />
    <asp:Button ID="btnDelete" Text="Delete" CssClass="DeleteButton" runat="server" OnClientClick="return confirm('Are you sure you wish to delete this image gallery? Your changes cannot be undone.');" />
    
    <br class="clear-both" /><hr />

    <asp:RequiredFieldValidator ID="rfvT" runat="server" ControlToValidate="txtTitle" ErrorMessage="You should create a title for this image gallery." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceT" runat="server" TargetControlID="rfvT" />

</asp:Content>