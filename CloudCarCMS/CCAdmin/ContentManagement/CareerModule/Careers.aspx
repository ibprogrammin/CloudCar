<%@ Page Title="Careers" ValidateRequest="False" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Careers.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.CareerModule.Careers" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Careers
        <i class="icon-briefcase"></i>
    </h1><hr />
    
    <asp:Button runat="server" ID="NewButton" Text="New" CausesValidation="false" CssClass="GreenButton NewButton" />
    <asp:ListBox runat="server" ID="CareersListBox" DataValueField="Id" DataTextField="Title" AutoPostBack="true" CssClass="form-list-box" Rows="3" />
    
    <br class="clear-both" /><hr />

    <asp:Label runat="server" ID="StatusLabel" CssClass="status-message" Visible="false" />
    <asp:HiddenField runat="server" ID="CareerIdHiddenField" />

    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
            <li class="tab"><a href="#tab-content">Content</a></li>
	    </ul>
        <div id="tab-details">
            <label for="txtName">Title</label>
            <asp:TextBox runat="server" ID="TitleTextBox" CssClass="form-text-box" TabIndex="20" />
            <br class="clear-both" /><br />
        
            <label for="txtName">Department</label>
            <asp:DropDownList runat="server" ID="DepartmentDropDown" CssClass="form-select-box" TabIndex="20" />
            <br class="clear-both" /><br />
        
            <label for="txtName">Report To</label>
            <asp:TextBox runat="server" ID="ReportToTextBox" CssClass="form-text-box" TabIndex="20" />
            <br class="clear-both" /><br />
        
            <label for="txtName">Experience</label>
            <asp:TextBox runat="server" ID="ExperienceTextBox" CssClass="form-text-box" TabIndex="20" />
            <br class="clear-both" /><br />
        
            <label for="txtName">Level</label>
            <asp:TextBox runat="server" ID="LevelTextBox" CssClass="form-text-box" TabIndex="20" />
            <br class="clear-both" /><br />
        
            <label for="txtName">Job Reference Code</label>
            <asp:TextBox runat="server" ID="ReferenceCodeTextBox" CssClass="form-text-box" TabIndex="20" />
            <br class="clear-both" /><br />
        </div>
        <div id="tab-content">
            <textarea runat="server" id="DescriptionTextArea" class="ck-editor" />
        </div>
    </div>

    <br class="clear-both" />
    <asp:Button runat="server" ID="SaveButton" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="SaveButton" TabIndex="34" />
    <asp:Button runat="server" ID="DeleteButton" Text="Delete" CausesValidation="false" CssClass="DeleteButton" TabIndex="35" />
    <br /><br />

    <asp:RequiredFieldValidator ID="TitleValidator" runat="server" ControlToValidate="TitleTextBox" ErrorMessage="You should create a title for this career." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="TitleValidatorCallout" runat="server" TargetControlID="TitleValidator" />
    
    <asp:RequiredFieldValidator ID="DepartmentValidator" runat="server" ControlToValidate="DepartmentDropDown" ErrorMessage="You should select a department for this career." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="DepartmentValidatorCallout" runat="server" TargetControlID="DepartmentValidator" />
    
    <asp:RequiredFieldValidator ID="DescriptionValidator" runat="server" ControlToValidate="DescriptionTextArea" ErrorMessage="You should give a description to this career." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="DescriptionValidatorCallout" runat="server" TargetControlID="DescriptionValidator" />

</asp:Content>