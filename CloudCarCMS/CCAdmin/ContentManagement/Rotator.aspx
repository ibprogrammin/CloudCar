<%@ Page Title="Image Slider" ValidateRequest="False" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Rotator.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.Rotator" %>
<%@ Register TagPrefix="SM" Namespace="CloudCar.CCControls.Validators" Assembly="CloudCarFramework" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">
    
    <h1 class="form-heading-style">
        Image Slider
        <i class="icon-desktop"></i>
    </h1>
    <hr />
    
    <asp:Button runat="server" ID="btnClear" Text="New" CausesValidation="false" CssClass="GreenButton NewButton" />
    <asp:ListBox runat="server" ID="lbRotator" DataValueField="id" DataTextField="title" AutoPostBack="true" CssClass="form-list-box" Rows="3" />
    
    <br class="clear-both" />
    <hr />

    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />
 
    <asp:HiddenField runat="server" ID="hfItemID" />
    
    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
		    <li class="tab"><a href="#tab-image">Image</a></li>
            <li class="tab"><a href="#tab-content">Content</a></li>
	    </ul>
        <div id="tab-details">
            <label for="txtTitle">Title</label>
            <asp:TextBox runat="server" ID="txtTitle" CssClass="form-text-box" />
            <br class="clear-both" /><br />

            <label for="txtSubHeading">Sub Heading</label>
            <asp:TextBox runat="server" ID="txtSubHeading" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <label for="ddlPage">Page</label>
            <asp:DropDownList runat="server" ID="ddlPage" DataTextField="ContentTitle" DataValueField="ID" ToolTip="Page" CssClass="form-select-box" />
            <br class="clear-both" /><br />

            <label for="txtLinkURL">Link URL</label>
            <asp:TextBox runat="server" ID="txtLinkURL" CssClass="form-text-box" />
            <br class="clear-both" /><br />

            <label for="txtOrder">Order</label>
            <asp:TextBox runat="server" ID="txtOrder" style="width: 200px; margin-right: 10px;" />
            <asp:Button runat="server" ID="btnOrderUp" Text="+" CssClass="form-up-down-button" />
            <asp:Button runat="server" ID="btnOrderDown" Text="-" CssClass="form-up-down-button clear-right" />
            <ajax:NumericUpDownExtender ID="nudOrder" runat="server" TargetControlID="txtOrder" Minimum="1" Maximum="6" TargetButtonUpID="btnOrderUp" TargetButtonDownID="btnOrderDown" />
            <br class="clear-both" /><br />
        </div>
        <div id="tab-image">
            <img runat="server" ID="imgRotatorImage" alt="Image Preview" visible="false" src="" class="image-display image-rotator-display" /><br />

            <label for="fuImage" style="min-height: 100px;">Image</label>
            <asp:HiddenField runat="server" ID="hfImageID" />
            <asp:Label runat="server" ID="lblImageLocation" Visible="false" CssClass="display-message" />
            <div class="form-file-upload-display">
	            <div class="form-fake-upload">
		            <input type="text" name="imagefilename" readonly="readonly" />
	            </div>
                <asp:FileUpload runat="server" ID="fuImage" ToolTip="Select the image for this rotator item" size="20" CssClass="form-real-upload" onchange="this.form.imagefilename.value = this.value;" /><br />
            </div><br />
            <span style="font-size: 12px; font-weight: bold;">Dimensions: W (920px) x H (318px)</span>
            <asp:LinkButton ID="btnGetImage" runat="server" Text="Get Image" OnCommand="GetImageButtonClick" Visible="false" />    
            <br class="clear-both" /><br />
        </div>
        <div id="tab-content">
            <textarea runat="server" id="DetailsTextArea" class="ck-editor" />
        </div>
    </div>
    
    <br class="clear-both" />

    <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="SaveButton" />
    <asp:Button runat="server" ID="btnDelete" Text="Delete" CausesValidation="false" CssClass="DeleteButton" />

    <br class="clear-both" /><hr />


<asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" ErrorMessage="You should create a title." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
<ajax:ValidatorCalloutExtender ID="vceTitle" runat="server" TargetControlID="rfvTitle" />

<asp:RequiredFieldValidator ID="rfvPage" runat="server" ControlToValidate="ddlPage" ErrorMessage="You should select a page." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
<ajax:ValidatorCalloutExtender ID="vcePage" runat="server" TargetControlID="rfvPage" />

<asp:RequiredFieldValidator ID="rfvOrder" runat="server" ControlToValidate="txtOrder" ErrorMessage="Please set the order of this item." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
<ajax:ValidatorCalloutExtender ID="vceOrder" runat="server" TargetControlID="rfvOrder" />
<SM:RotatorOrderValidator ID="ovOrder" runat="server" ControlToValidate="txtOrder" PageControl="ddlPage" RotatorIDControl="hfItemID" ErrorMessage="The order you have selected is already being used." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
<ajax:ValidatorCalloutExtender ID="vceOVOrder" runat="server" TargetControlID="ovOrder" />

</asp:Content>