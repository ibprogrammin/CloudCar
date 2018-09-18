<%@ Page Title="Testimonials" Language="vb" AutoEventWireup="false" ValidateRequest="False" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Testimonials.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.Testimonials" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Testimonials
        <i class="icon-quote-left"></i>
    </h1><hr />

    <asp:Button runat="server" ID="btnClear" Text="New" CausesValidation="false" CssClass="GreenButton NewButton" />       
    <asp:ListBox runat="server" ID="lbTestimonials" DataValueField="id" DataTextField="Author" AutoPostBack="true" CssClass="form-list-box" Rows="3" />
    
    <br class="clear-both" /><hr />
    
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />
    <asp:HiddenField runat="server" ID="hfTestimonialID" />
    
    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
            <li class="tab"><a href="#tab-content">Content</a></li>
	    </ul>
        <div id="tab-details">

            <label for="txtAuthor">Client Names</label>
            <asp:TextBox runat="server" ID="txtAuthor" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <label style="min-height: 100px;">
                <img runat="server" ID="imgTestimonialImage" alt="Testimonial Image Preview" visible="false" src="" class="image-display" style="float: right; margin-top: -10px;" />
                Image
            </label>
            <asp:HiddenField runat="server" ID="hfImageID" />
            <asp:Label runat="server" ID="lblFileName" Visible="false" CssClass="display-message" />
            <div class="form-file-upload-display">
                <div class="form-fake-upload">
	                <input type="text" name="imagefilename" readonly="readonly" />
                </div>
                <asp:FileUpload runat="server" ID="fuImage" ToolTip="Select an image for this testimonial" size="20" CssClass="form-real-upload" onchange="this.form.imagefilename.value = this.value;" /><br />
            </div>
            <br class="clear-both" /><br />
        </div>
        <div id="tab-content">
            <textarea runat="server" id="QuoteTextArea" class="ck-editor" />
        </div>
    </div>
    
    <br class="clear-both" />
    
    <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="SaveButton" />
    <asp:Button runat="server" ID="btnDelete" Text="Delete" CausesValidation="false" CssClass="DeleteButton" />

    <br class="clear-both" /><hr />

    <asp:RequiredFieldValidator ID="rfvPT" runat="server" ControlToValidate="txtAuthor" ErrorMessage="Please enter an author." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vcePT" runat="server" TargetControlID="rfvPT" />

</asp:Content>