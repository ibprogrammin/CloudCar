<%@ Page Title="Sales Package Request" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="RequestForm.aspx.vb" Inherits="CloudCar.CCCommerce.Forms.RequestForm" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Sales Package Request
    </h1><hr />
    
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />
    
    <fieldset>
    
        <h2 class="form-heading-style">Details</h2><br /><br />

        <label>Access Key *</label><br />
        <asp:TextBox runat="server" ID="txtAccessKey" style="width: 420px;" />
        <asp:RequiredFieldValidator ID="rfvAccessKey" runat="server" ControlToValidate="txtAccessKey" Display="None" ErrorMessage="Please enter your access key" ValidationGroup="ItemValidation" />
        <ajax:ValidatorCalloutExtender ID="vceAccessKey" runat="server" TargetControlID="rfvAccessKey" PopupPosition="TopLeft" />
        <br style="clear: both;" /><br />
        
        <label>Sales Rep Name *</label><br />
        <asp:TextBox runat="server" ID="txtSalesRepName" style="width: 420px;" />
        <asp:RequiredFieldValidator ID="rfvSalesRepName" runat="server" ControlToValidate="txtSalesRepName" Display="None" ErrorMessage="Please your name" ValidationGroup="ItemValidation" />
        <ajax:ValidatorCalloutExtender ID="vceSalesRepName" runat="server" TargetControlID="rfvSalesRepName" PopupPosition="TopLeft" />
        <br style="clear: both;" /><br />
        
        <label>Sales Rep Email *</label><br />
        <asp:TextBox runat="server" ID="txtSalesRepEmail" style="width: 420px;" />
        <asp:RequiredFieldValidator ID="rfvSalesRepEmail" runat="server" ControlToValidate="txtSalesRepEmail" Display="None" ErrorMessage="Please your email address" ValidationGroup="ItemValidation" />
        <ajax:ValidatorCalloutExtender ID="vceSalesRepEmail" runat="server" TargetControlID="rfvSalesRepEmail" PopupPosition="TopLeft" />
        <br style="clear: both;" /><br />
        
        <label>Package *</label><br />
        <asp:DropDownList runat="server" ID="ddlPackage" DataTextField="Name" DataValueField="ID" style="width: 440px;" />
        <asp:RequiredFieldValidator ID="rfvPackage" runat="server" ControlToValidate="ddlPackage" Display="None" ErrorMessage="Please select a package" ValidationGroup="ItemValidation" />
        <ajax:ValidatorCalloutExtender ID="vcePackage" runat="server" TargetControlID="rfvPackage" PopupPosition="TopLeft" />
        <br style="clear: both;" /><br />
        
        <h3>Customer</h3><br />
        
        <label>Name *</label><br />
        <asp:TextBox runat="server" ID="txtCustomerName" style="width: 420px;" />
        <asp:RequiredFieldValidator ID="rfvCustomerName" runat="server" ControlToValidate="txtCustomerName" Display="None" ErrorMessage="Please enter the customers name" ValidationGroup="ItemValidation" />
        <ajax:ValidatorCalloutExtender ID="vceCustomerName" runat="server" TargetControlID="rfvCustomerName" PopupPosition="TopLeft" />
        <br style="clear: both;" /><br />
        
        <label>Email *</label><br />
        <asp:TextBox runat="server" ID="txtCustomerEmail" style="width: 420px;" />
        <asp:RequiredFieldValidator ID="rfvCustomerEmail" runat="server" ControlToValidate="txtCustomerEmail" Display="None" ErrorMessage="Please enter the customers email address" ValidationGroup="ItemValidation" />
        <ajax:ValidatorCalloutExtender ID="vceCustomerEmail" runat="server" TargetControlID="rfvCustomerEmail" PopupPosition="TopLeft" />
        <br style="clear: both;" /><br />
        
    </fieldset>
    
    <br class="clear-both" /><br />

    <asp:Button id="btnAdd" runat="server" CssClass="Green" ValidationGroup="ItemValidation" CausesValidation="true" UseSubmitBehavior="true" Text="Save" />
    <asp:Button id="btnClear" runat="server" CssClass="RedButton" Text="Clear" />

</asp:Content>