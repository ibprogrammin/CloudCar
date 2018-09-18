<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Features.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.PropertyModule.Features" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">Features</h1><br class="clear-both" /><br />
    
    <asp:Button runat="server" ID="btnNew" Text="New" CausesValidation="false" CssClass="GreenButton NewButton" />
    <asp:ListBox runat="server" ID="lbFeatures" DataValueField="Id" DataTextField="Name" AutoPostBack="true" style="width: 780px; height: 97px; float: left;" Rows="3" />
    
    <br class="clear-both" /><br />
    <hr /><br />
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" /><br class="clear-both" />
    
    <fieldset>
        <h2 class="form-heading-style">Content</h2><br />
    
        <asp:HiddenField runat="server" ID="hfFeatureId" />
    
        <label for="txtName">Name</label>
        <asp:TextBox runat="server" ID="txtName" style="width: 650px;" TabIndex="20" />
        <br class="clear-both" /><br />
    
        <label for="txtDetails">Details</label><br class="clear-both" />
        <textarea runat="server" id="DetailsTextArea" class="ck-editor" />
        <br class="clear-both" /><br />
    
    </fieldset>

    <br class="clear-both" />
    <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="SaveButton" TabIndex="34" />
    <asp:Button runat="server" ID="btnDelete" Text="Delete" CausesValidation="false" CssClass="DeleteButton" TabIndex="35" />
    <br /><br />

    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="You should create a name for this feature." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceName" runat="server" TargetControlID="rfvName" />

</asp:Content>