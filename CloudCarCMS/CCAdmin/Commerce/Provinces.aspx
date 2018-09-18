<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Provinces.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Provinces" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Regions
        <i class="icon-map-marker"></i>
    </h1><hr />
    
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" />
    <asp:HiddenField runat="server" ID="hfID" />

    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
	    </ul>
        <div id="tab-details">

            <label>Name</label>
            <asp:TextBox runat="server" ID="txtName" CssClass="form-text-box" />
            <ajax:TextBoxWatermarkExtender ID="tbwName" runat="server" TargetControlID="txtName" WatermarkText="Province/State" />
            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" Display="None" ErrorMessage="Please enter a name for this province/state" ValidationGroup="ItemValidation" />
            <ajax:ValidatorCalloutExtender ID="vceName" runat="server" TargetControlID="rfvName" PopupPosition="TopLeft" />
            <br class="clear-both" /><br />
        
            <label>Code</label>
            <asp:TextBox runat="server" ID="txtCode" style="width: 100px;" />
            <ajax:TextBoxWatermarkExtender ID="tbwCode" runat="server" TargetControlID="txtCode" WatermarkText="Code" />
            <br class="clear-both" /><br />
        
            <label>Tax</label>
            <asp:TextBox runat="server" ID="txtTax" style="width: 100px;" />
            <ajax:TextBoxWatermarkExtender ID="tbwTax" runat="server" TargetControlID="txtTax" WatermarkText="Tax %" />
            <asp:RequiredFieldValidator ID="rfvTax" runat="server" ControlToValidate="txtTax" Display="None" ErrorMessage="Please enter a percentage tax value" ValidationGroup="ItemValidation" />
            <ajax:ValidatorCalloutExtender ID="vceTax" runat="server" TargetControlID="rfvTax" PopupPosition="TopLeft" />
            <br class="clear-both" /><br />
        
            <label>Country</label>
            <asp:DropDownList runat="server" ID="ddlCountry" DataTextField="Name" DataValueField="ID" CssClass="form-select-box" />
            <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ControlToValidate="ddlCountry" Display="None" ErrorMessage="Please select a country" ValidationGroup="ItemValidation" />
            <ajax:ValidatorCalloutExtender ID="vceCountry" runat="server" TargetControlID="rfvCountry" PopupPosition="TopLeft" />
            <br class="clear-both" />

         </div>   
    </div>
    
    <br class="clear-both" />

    <asp:Button id="btnAdd" runat="server" CssClass="SaveButton" ValidationGroup="ItemValidation" CausesValidation="true" UseSubmitBehavior="true" Text="Save" />
    <asp:Button id="btnClear" runat="server" CssClass="DeleteButton" Text="Clear" />
    
    <br class="clear-both" /><hr />    

    <asp:GridView runat="server" 
            ID="RegionGridView" 
            AutoGenerateColumns="false" 
            DataKeyField="ID" 
            AllowSorting="true" 
            AllowPaging="true" 
            GridLines="None" 
            CssClass="default-table">
        <PagerStyle HorizontalAlign="Right" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateField ItemStyle-Width="25%" SortExpression="Name" HeaderText="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="12%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnEdit" OnCommand="btnEdit_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Edit" Text='<%# Eval("Name") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Code" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="CountryName" HeaderText="Country" SortExpression="CountryID" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Tax" HeaderText="Tax" SortExpression="Tax" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="8%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete this Country?');" OnCommand="btnDelete_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="" CssClass="icon-trash delete-icon" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>