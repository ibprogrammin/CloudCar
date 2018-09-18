<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Countries.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Countries" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Countries
        <i class="icon-flag"></i>
    </h1>
    <hr />
    
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" />
    <asp:HiddenField runat="server" ID="hfID" />

     <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
	    </ul>
        <div id="tab-details">

            <label>Name</label>
            <asp:TextBox runat="server" ID="txtName" CssClass="form-text-box" />
            <ajax:TextBoxWatermarkExtender ID="tbwName" runat="server" TargetControlID="txtName" WatermarkText="Country" />
            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" Display="None" ErrorMessage="Please enter a name for this country" ValidationGroup="ItemValidation" />
            <ajax:ValidatorCalloutExtender ID="vceName" runat="server" TargetControlID="rfvName" PopupPosition="TopLeft" />
            <br class="clear-both" /><br />
        
            <label>Code</label>
            <asp:TextBox runat="server" ID="txtCode" style="width: 100px;" />
            <ajax:TextBoxWatermarkExtender ID="tbwCode" runat="server" TargetControlID="txtCode" WatermarkText="Code" />
            <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtCode" Display="None" ErrorMessage="Please enter a valid Code for this Country" ValidationGroup="ItemValidation" />
            <ajax:ValidatorCalloutExtender ID="vceCode" runat="server" TargetControlID="rfvCode" PopupPosition="TopLeft" />
            <asp:RegularExpressionValidator ValidationExpression="\w{2}" ControlToValidate="txtCode" runat="server" ID="revCode" Display="None" ErrorMessage="Code must be 2 characters in length" ValidationGroup="ItemValidation" />
            <ajax:ValidatorCalloutExtender ID="vceCodeLength" runat="server" TargetControlID="revCode" PopupPosition="TopLeft" />
            <br class="clear-both" /><br />
        
            <label>Tax</label>
            <asp:TextBox runat="server" ID="txtTax" style="width: 100px;" />
            <ajax:TextBoxWatermarkExtender ID="tbwTax" runat="server" TargetControlID="txtTax" WatermarkText="Tax %" />
            <asp:RequiredFieldValidator ID="rfvTax" runat="server" ControlToValidate="txtTax" Display="None" ErrorMessage="Please enter a percentage tax value" ValidationGroup="ItemValidation" />
            <ajax:ValidatorCalloutExtender ID="vceTax" runat="server" TargetControlID="rfvTax" PopupPosition="TopLeft" />
            <br class="clear-both" />

        </div>
    </div>

    <br class="clear-both" />

    <asp:Button id="btnAdd" runat="server" CssClass="SaveButton" ValidationGroup="ItemValidation" CausesValidation="true" UseSubmitBehavior="true" Text="Save" />
    <asp:Button id="btnClear" runat="server" CssClass="DeleteButton" Text="Clear" />
    
    <br class="clear-both" /><hr />
    
    <asp:DataGrid runat="server" 
            ID="dgCountries" 
            AutoGenerateColumns="false" 
            DataKeyField="ID" 
            AllowSorting="true" 
            GridLines="None" 
            CssClass="default-table" 
            AllowPaging="true">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="Country" SortExpression="Name" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnEdit" OnCommand="btnEdit_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Edit" Text='<%# Eval("Name") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Code" HeaderText="Code" SortExpression="Code" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="center" />
            <asp:BoundColumn DataField="Tax" HeaderText="Tax" SortExpression="Tax" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="6%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete this Country?');" OnCommand="btnDelete_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="" CssClass="icon-trash delete-icon" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

</asp:Content>