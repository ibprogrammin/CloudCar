<%@ Page Title="Call To Action" ValidateRequest="False" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="CallToAction.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.CallToActionModule.CallToAction" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Call To Action
        <i class="icon-bell"></i>
        <asp:Button id="NewButton" runat="server" CausesValidation="true" CssClass="SaveButton heading-button-new" Text="New" />
    </h1><hr />
    
    <asp:Label ID="StatusMessageLabel" runat="server" Text="" CssClass="status-message" Visible="false" />
    
    <asp:PlaceHolder ID="DetailsFormPlaceHolder" runat="server" Visible="false">
        
        <asp:HiddenField runat="server" id="CallToActionIdHiddenField" />
        
        <div class="tab-container">
	        <ul class="tabs">
		        <li class="tab"><a href="#tab-details">Details</a></li>
		        <li class="tab"><a href="#tab-content">Content</a></li>
	        </ul>
            <div id="tab-details">
                <label for="HeadingTextBox">Heading</label>
                <asp:TextBox runat="server" ID="HeadingTextBox" CssClass="form-text-box" />
                <asp:RequiredFieldValidator ID="HeadingValidator" runat="server" ControlToValidate="HeadingTextBox"
                    ErrorMessage="Please enter a Heading" SetFocusOnError="true" Display="None" ValidationGroup="FAQ" />
                <ajax:ValidatorCalloutExtender ID="HeadingValidatorCallout" runat="server" TargetControlID="HeadingValidator" />
                <br class="clear-both" /><br />

                <label for="ButtonTextTextBox">Button Text</label>
                <asp:TextBox runat="server" ID="ButtonTextTextBox" CssClass="form-text-box" />
                <asp:RequiredFieldValidator ID="ButtonTextValidator" runat="server" ControlToValidate="ButtonTextTextBox"
                    ErrorMessage="Please enter a Heading" SetFocusOnError="true" Display="None" ValidationGroup="FAQ" />
                <ajax:ValidatorCalloutExtender ID="ButtonTextValidatorCallout" runat="server" TargetControlID="ButtonTextValidator" />
                <br class="clear-both" /><br />
            
                <label for="LinkUrlTextBox">Link Url</label>
                <asp:TextBox runat="server" ID="LinkUrlTextBox" CssClass="form-text-box" />
                <br class="clear-both" /><br />

                <label for="ImageUrlTextBox">Image Url</label>
                <asp:TextBox runat="server" ID="ImageUrlTextBox" CssClass="form-text-box" />
                <br class="clear-both" /><br />
            </div>
            <div id="tab-content">
                <textarea runat="server" id="DetailsTextArea" class="ck-editor" />
            </div>
        </div>
        
        <br class="clear-both" />

        <asp:Button runat="server" ID="SaveButton" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="SaveButton" />
        <asp:Button runat="server" ID="CancelButton" Text="Clear" CausesValidation="false" CssClass="DeleteButton" />

        <br class="clear-both" /><hr />

    </asp:PlaceHolder>
    
    <asp:Repeater runat="server" ID="CallToActionRepeater">
        <HeaderTemplate>
            <table class="default-table">
                <thead class="default-table-header">
                    <td>Heading</td>
                    <td>Button Text</td>
                    <td>Link Url</td>
                    <td></td>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:LinkButton Text='<%# Eval("Heading") %>' runat="server" ItemId='<%# Eval("Id") %>' OnClick="SelectItemButtonClick" />
                </td>
                <td><%# Eval("ButtonText")%></td>
                <td><a href='<%# Eval("LinkUrl")%>'><%# Eval("LinkUrl")%></a></td>
                <td>
                    <asp:LinkButton ID="DeleteItemButton" runat="server" ItemId='<%# Eval("Id") %>' OnClick="DeleteItemButtonClick" Text="" CssClass="icon-trash delete-icon" />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>

</asp:Content>