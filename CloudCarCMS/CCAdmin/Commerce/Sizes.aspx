<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Sizes.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Sizes" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Sizes
        <i class="icon-resize-vertical"></i>
    </h1><hr />

    <asp:HyperLink runat="server" ID="hlBackToProduct" Text="&laquo; Back to Product" style="clear: left;" Font-Size="Small" Visible="false" />
    
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" />
    <asp:HiddenField runat="server" ID="hfSizeID" />

    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
            <li class="tab"><a href="#tab-content">Content</a></li>
		    <li class="tab"><a href="#tab-seo">SEO</a></li>
	    </ul>
        <div id="tab-details">

            <label>Size</label>
            <asp:TextBox runat="server" ID="txtName" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label>Abreviation</label>
            <asp:TextBox runat="server" ID="txtAbreviation" CssClass="form-text-box" />
            <br class="clear-both" />

        </div>
    </div>
    
    <br class="clear-both" />

    <asp:Button id="btnAdd" runat="server" CssClass="SaveButton" ValidationGroup="ItemValidation" CausesValidation="true" UseSubmitBehavior="true" Text="Save" />
    <asp:Button id="btnClear" runat="server" CssClass="DeleteButton" Text="Clear" />
    
    <br class="clear-both" /><hr />

    <ajax:TextBoxWatermarkExtender ID="tbwName" runat="server" TargetControlID="txtName" WatermarkText="Size" />
    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" Display="None" ErrorMessage="Please enter a name for this size" ValidationGroup="ItemValidation" />
    <ajax:ValidatorCalloutExtender ID="vceName" runat="server" TargetControlID="rfvName" PopupPosition="TopLeft" />
    
    <asp:RequiredFieldValidator ID="rfvAbreviation" runat="server" ControlToValidate="txtAbreviation" Display="None" ErrorMessage="Please enter a abreviation for this size" ValidationGroup="ItemValidation" />
    <ajax:ValidatorCalloutExtender ID="vceAbreviation" runat="server" TargetControlID="rfvAbreviation" PopupPosition="TopLeft" />

    <asp:DataGrid runat="server" 
            ID="dgSizes" 
            AutoGenerateColumns="false" 
            DataKeyField="ID" 
            AllowSorting="true" 
            AllowPaging="true" 
            GridLines="None" 
            CssClass="default-table" 
            CellPadding="0" 
            CellSpacing="0">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="Size" SortExpression="Name">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnEdit" OnCommand="EditButtonClick" CommandArgument='<%# Eval("ID") %>' CommandName="Edit" Text='<%# Eval("Name") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Abreviation" HeaderText="Abreviation" SortExpression="Abreviation" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" />
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-Width="6%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete this size?');" OnCommand="DeleteButtonClick" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="" CssClass="icon-trash delete-icon" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

</asp:Content>