<%@ Page Title="Web Forms" ValidateRequest="False" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="CustomIntakeForms.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.FormModule.CustomIntakeForms" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

<h1 class="form-heading-style">
    Web Forms
    <i class="icon-list"></i>
</h1><hr />

<asp:Button runat="server" ID="ClearButton" Text="New" CausesValidation="false" CssClass="GreenButton NewButton" />       
<asp:ListBox runat="server" ID="FormsListBox" DataValueField="Id" DataTextField="Name" AutoPostBack="true" CssClass="form-list-box" Rows="3" />

<br class="clear-both" /><hr />

<asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />
<asp:HiddenField runat="server" ID="FormIdHiddenField" />

<div class="tab-container">
	<ul class="tabs">
		<li class="tab"><a href="#tab-details">Form Settings</a></li>
		<li class="tab"><a href="#tab-newfield">Field Settings</a></li>
        <li class="tab"><a href="#tab-fields">Fields</a></li>
		<li class="tab"><a href="#tab-data">View Submissions</a></li>
	</ul>
	<div id="tab-details">
        
        <asp:Literal ID="CurrentFormLink" runat="server" Visible="false" />

        <label>Title</label>
        <asp:textbox runat="server" id="FormNameTextBox" CssClass="form-text-box" />
        <br class="clear-both" /><br />

        <label>Details</label>
        <asp:TextBox runat="server" id="DetailsTextBox" TextMode="MultiLine" Rows="3" CssClass="form-text-area" />
        <br class="clear-both" /><br />

        <label>Completion Message</label>
        <asp:TextBox runat="server" ID="CompletionMessageTextBox" TextMode="MultiLine" Rows="3" CssClass="form-text-area" />
        <br class="clear-both" /><br />

        <label>Permalink</label>
        <asp:textbox runat="server" id="PermalinkTextBox" CssClass="form-text-box" />
        <br class="clear-both" /><br />
        
        <label>Submit Button Label</label>
        <asp:textbox runat="server" id="CallToActionLabelTextBox" CssClass="form-text-box" />
        <br class="clear-both" /><br />
        
        <label style="min-height: 100px;">Picture<br />
            <img runat="server" ID="PictureImage" alt="Default Image Preview" visible="false" src="" class="image-display" />
        </label>
        <asp:HiddenField runat="server" ID="PictureImageIdHiddenField" />
        <asp:Label runat="server" ID="PictureLocationLabel" ReadOnly="true" Visible="false" CssClass="display-message" style="margin-bottom: 10px;" />
        <div class="form-file-upload-display">
	        <div class="form-fake-upload">
		        <input type="text" name="picturefilename" readonly="readonly" /> <!-- browse button is here as background -->
	        </div>
            <asp:FileUpload runat="server" ID="PictureFileUpload" ToolTip="Select the header image for this product." size="20" CssClass="form-real-upload" onchange="this.form.picturefilename.value = this.value;" /><br />
        </div><br />
        <br class="clear-both" /><br />

        <label>Cc Admin</label>
        <asp:CheckBox runat="server" ID="CcAdminCheckBox" CssClass="form-check-box" />
        <br class="clear-both" /><br />

        <asp:Button runat="server" ID="SaveFormButton" Text="Save" CssClass="SaveButton" />
        <asp:Button runat="server" ID="DeleteFormButton" Text="Delete" CssClass="DeleteButton" />
        
        <br class="clear-both" /><br />
	</div>
    <div id="tab-fields">
        <asp:Repeater runat="server" ID="FormFieldRepeater">
            <HeaderTemplate>
                <table class="default-table">
                    <thead class="default-table-header">
                        <tr>
                            <td>Label</td>
                            <td>Details</td>
                            <td></td>
                            <td></td>
                            <td>Default</td>
                            <td></td>
                            <td></td>
                            <td>Index</td>
                        </tr>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("Label")%></td>
                    <td><%# Eval("Details")%></td>
                    <td><%# Eval("DataType") %></td>
                    <td><%# Eval("ControlType")%></td>
                    <td><%# Eval("DefaultValues")%></td>
                    <td><%# Eval("ValidationExpression")%></td>
                    <td><%# Eval("Watermark")%></td>
                    <td><%# Eval("FieldIndex")%></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
        </asp:Repeater>
    </div>
    <div id="tab-newfield">
        <asp:HiddenField runat="server" ID="NewFieldIdHiddenField" />

        <label>Label</label>
        <asp:TextBox runat="server" ID="NewFieldLabelTextBox" CssClass="form-text-box" />
        <br class="clear-both" /><br />

        <label>Details</label>
        <asp:TextBox runat="server" ID="NewFieldDetailsTextBox" CssClass="form-text-box" />
        <br class="clear-both" /><br />

        <label>Data Type</label>
        <asp:DropDownList runat="server" ID="NewFieldDataTypeDropDown" CssClass="form-select-box">
            <asp:ListItem Text="Whole Number" Value="1" />
            <asp:ListItem Text="Decimal/Dollar" Value="2" />
            <asp:ListItem Text="Text" Value="3" />
            <asp:ListItem Text="True/False" Value="4" />
            <asp:ListItem Text="Yes/No" Value="4" />
        </asp:DropDownList>
        <br class="clear-both" /><br />

        <asp:UpdatePanel runat="server" ID="ControlTypeUpdatePanel" ChildrenAsTriggers="true">
            <ContentTemplate>
        
                <label>Control Type</label>
                <asp:DropDownList runat="server" ID="NewFieldControlTypeDropDown" AutoPostBack="true" CssClass="form-select-box">
                    <asp:ListItem Text="Text Box" Value="1" />
                    <asp:ListItem Text="Text Area" Value="2" />
                    <asp:ListItem Text="Check Box" Value="3" />
                    <asp:ListItem Text="Radio Button List" Value="4" />
                    <asp:ListItem Text="Drop Down List" Value="5" />
                </asp:DropDownList>
                <br class="clear-both" /><br />
        
                <label>Field Options</label>
                <div class="float-left">
                <asp:PlaceHolder runat="server" ID="FieldOptionsPlaceHolder" Visible="False" /><br />
                <asp:Button runat="server" ID="AddFieldOptionButton" Visible="False" Text="Add Option" CssClass="SaveButton" />
                <asp:Button runat="server" ID="RemoveFieldOptionButton" Visible="False" Text="Remove Option" CssClass="DeleteButton" />
                </div>
                <br class="clear-both" /><br />

            </ContentTemplate>
        </asp:UpdatePanel>

        <label>Default</label>
        <asp:TextBox runat="server" ID="NewFieldDefaultsTextBox" CssClass="form-text-box" />
        <br class="clear-both" /><br />

        <label>Validation (REGEX)</label>
        <asp:DropDownList runat="server" ID="NewFieldValidationDropDown">
            <asp:ListItem Text="None" Value="0" />
            <asp:ListItem Text="Numeric" Value="1" />
            <asp:ListItem Text="Email" Value="2" />
            <asp:ListItem Text="Postal Code" Value="3" />
            <asp:ListItem Text="Zip Code" Value="4" />
            <asp:ListItem Text="Phone Number" Value="5" />
            <asp:ListItem Text="Address" Value="6" />
            <asp:ListItem Text="Custom" Value="7" />
        </asp:DropDownList>
        <asp:TextBox runat="server" ID="NewFieldCustomValidationTextBox" />
        <br class="clear-both" /><br />

        <label>Watermark</label>
        <asp:TextBox runat="server" ID="NewFieldWatermarkTextBox" CssClass="form-text-box" />
        <br class="clear-both" /><br />

        <label>Index</label>
        <asp:TextBox runat="server" ID="NewFieldIndexTextBox" CssClass="form-text-box" />
        <br class="clear-both" /><br />

        <asp:Button runat="server" ID="AddFieldButton" Text="Add Field" CssClass="SaveButton" />
        <br class="clear-both" /><br />
	</div>
    <div id="tab-data">
	    <asp:DataGrid runat="server" ID="FormDataDisplayDataGrid" CssClass="default-table">
            <HeaderStyle CssClass="default-table-header" />
        </asp:DataGrid>
	</div>
</div>

</asp:Content>