<%@ Page Title="Videos" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Videos.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.Videos" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Videos
        <i class="icon-film"></i>
    </h1><hr />

    <asp:Button runat="server" ID="btnClear" Text="New" CausesValidation="false" CssClass="GreenButton NewButton" />       
    <asp:ListBox runat="server" ID="lbVideos" DataValueField="id" DataTextField="Title" AutoPostBack="true" Rows="3" CssClass="form-list-box" />
    
    <br class="clear-both" /><hr />

    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />
    <asp:HiddenField runat="server" ID="hfVideoID" />

    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
            <li class="tab"><a href="#tab-content">Content</a></li>
		    <li class="tab"><a href="#tab-seo">SEO</a></li>
	    </ul>
        <div id="tab-details">
            <asp:Literal ID="litCurrentVideo" runat="server" Visible="false" />

            <label for="ddlPlayerType">Player</label>
            <asp:DropDownList runat="server" ID="ddlPlayerType" CssClass="form-select-box">
                <asp:ListItem Text="Select" Value="" />
                <asp:ListItem Text="You Tube" Value="1" />
                <asp:ListItem Text="Vimeo" Value="2" />
            </asp:DropDownList>
            <br class="clear-both" /><br />
        
            <label for="txtVideoID">Video ID</label>
            <asp:TextBox runat="server" ID="txtVideoID" CssClass="form-text-box" />
            <br class="clear-both" /><br />

            <label for="txtTitle">Title</label>
            <asp:TextBox runat="server" ID="txtTitle" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
        </div>
        <div id="tab-content">
            <textarea runat="server" id="DetailsTextArea" class="ck-editor" />
        </div>
        <div id="tab-seo">
            <label for="txtKeywords">Keywords</label>
            <asp:TextBox runat="server" ID="txtKeywords" CssClass="form-text-box" />
	        <br class="clear-both" /><br />
        
            <label for="txtDescription">Description</label>
            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="4" CssClass="form-text-area" />
	        <br class="clear-both" />
        </div>
    </div>

    <br class="clear-both" />

    <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="SaveButton" />
    <asp:Button runat="server" ID="btnDelete" Text="Delete" CausesValidation="false" CssClass="DeleteButton" OnClientClick="return confirm('Are you sure you wish to delete this video? Your changes cannot be undone.');" />

    <br class="clear-both" /><hr />
    
    <asp:RequiredFieldValidator ID="rfvT" runat="server" ControlToValidate="txtTitle" ErrorMessage="You should create a title for this video." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceT" runat="server" TargetControlID="rfvT" />

    <asp:RequiredFieldValidator ID="rfvVID" runat="server" ControlToValidate="txtVideoID" ErrorMessage="You should enter a video ID." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceVID" runat="server" TargetControlID="rfvVID" />
    
    <asp:RequiredFieldValidator ID="rfvPlayer" runat="server" ControlToValidate="ddlPlayerType" ErrorMessage="You should select a player for this video." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vcePlayer" runat="server" TargetControlID="rfvPlayer" />

</asp:Content>