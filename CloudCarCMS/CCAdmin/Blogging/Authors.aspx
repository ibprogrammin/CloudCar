<%@ Page Title="Authors" ValidateRequest="False" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Authors.aspx.vb" Inherits="CloudCar.CCAdmin.Blogging.Authors" %>
<%@ Register TagPrefix="SM" Namespace="CloudCar.CCControls.Validators" Assembly="CloudCarFramework" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Author
        <i class="icon-book"></i>
    </h1><hr />
    
    <asp:Button runat="server" ID="btnClear" Text="New" CausesValidation="false" CssClass="GreenButton NewButton" />       
    <asp:ListBox runat="server" ID="lbAuthors" DataValueField="id" DataTextField="Name" AutoPostBack="true" CssClass="form-list-box" Rows="3" />

    <br class="clear-both" /><hr />

    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />
    
    <asp:HiddenField runat="server" ID="hfAuthorID" />

    <div class="tab-container">
	    <ul class="tabs">
		    <li class="tab"><a href="#tab-details">Details</a></li>
		    <li class="tab"><a href="#tab-content">Content</a></li>
            <li class="tab"><a href="#tab-seo">SEO</a></li>
	    </ul>
        <div id="tab-details">
            <label for="txtName">Name</label>
            <asp:TextBox runat="server" ID="txtName" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <label for="txtAvatarURL">Avatar URL</label>
            <asp:TextBox runat="server" ID="txtAvatarURL" CssClass="form-text-box" />
            <br class="clear-both" />
        </div>
        <div id="tab-content">
            <textarea runat="server" id="txtBiography" class="ck-editor" />
        </div>
        <div id="tab-seo">
            <label for="txtBrowserTitle">Browser Title</label>
            <asp:TextBox runat="server" ID="txtBrowserTitle" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label for="txtPermalink">Permalink</label>
            <asp:TextBox runat="server" ID="txtPermalink" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label for="txtKeywords">Keywords</label>
            <asp:TextBox runat="server" ID="txtKeywords" CssClass="form-text-box" />
            <br class="clear-both" /><br />
    
            <label for="txtDescription">Description <br />(max. 260 characters)</label>
            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="5" MaxLength="260" CssClass="form-text-area" />
            <br class="clear-both" />
        </div>
    </div>
    
    <br class="clear-both" />

    <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="SaveButton" />
    <asp:Button runat="server" ID="btnDelete" Text="Delete" CausesValidation="false" CssClass="DeleteButton" OnClientClick="return confirm('Are you sure you wish to delete this author? Your changes cannot be undone.');" />

    <br class="clear-both" />

    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="You should give a name to this author." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceName" runat="server" TargetControlID="rfvName" />

    <asp:RequiredFieldValidator ID="rfvPL" runat="server" ControlToValidate="txtPermalink" ErrorMessage="You should give a permalink to this author." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vcePL" runat="server" TargetControlID="rfvPL" />
    <SM:PermalinkValidator ID="pvPermalink" runat="server" ControlToValidate="txtPermalink" PermalinkType="Author" ErrorMessage="The permalink you have choosen is not unique." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vcePermalinkValidator" runat="server" TargetControlID="pvPermalink" />

</asp:Content>