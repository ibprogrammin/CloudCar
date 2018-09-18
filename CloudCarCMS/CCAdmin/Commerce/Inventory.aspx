<%@ Page Title="Inventory" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Inventory.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Inventory" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <!-- TODO Make this form a cascading drop down, and check the available colours and sizes after the product is selected. -->

    <h1 class="form-heading-style">
        Inventory
        <i class="icon-barcode"></i>
    </h1><hr />

    <asp:HyperLink runat="server" ID="hlBackToProduct" Text="&laquo; Back to Product" class="clear-left" Visible="false" />
    
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" />
    <asp:HiddenField runat="server" ID="hfInventoryID" />

    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
	    </ul>
        <div id="tab-details">
    
            <label>Product</label>
            <asp:DropDownList runat="server" ID="ddlProduct" DataTextField="Name" DataValueField="ID" CssClass="form-select-box" />
            <br class="clear-both" /><br />
        
            <label>Colour</label>
            <asp:DropDownList runat="server" ID="ddlColor" DataTextField="Name" DataValueField="ID" CssClass="form-select-box" />
            <br class="clear-both" /><br />
        
            <label>Size</label>
            <asp:DropDownList runat="server" ID="ddlSize" DataTextField="Name" DataValueField="ID" CssClass="form-select-box" />
            <br class="clear-both" /><br />
        
            <label>Quantity</label>
            <asp:TextBox runat="server" ID="txtQuantity" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" />

        </div>
    </div>
    
    <br class="clear-both" />
    
    <asp:Button id="btnAdd" runat="server" CssClass="SaveButton" ValidationGroup="ItemValidation" CausesValidation="true" Text="Save" />
    <asp:Button id="btnClear" runat="server" CssClass="DeleteButton" Text="Clear" />
    
    <br class="clear-both" /><hr />
    
    <asp:RequiredFieldValidator ID="rfvProduct" runat="server" ControlToValidate="ddlProduct" Display="None" ErrorMessage="Please select a product" ValidationGroup="ItemValidation" />
    <ajax:ValidatorCalloutExtender ID="vceProduct" runat="server" TargetControlID="rfvProduct" PopupPosition="TopLeft" />

    <asp:RequiredFieldValidator ID="rfvColor" runat="server" ControlToValidate="ddlColor" Display="None" ErrorMessage="Please select a color" ValidationGroup="ItemValidation" />
    <ajax:ValidatorCalloutExtender ID="vceColor" runat="server" TargetControlID="rfvColor" PopupPosition="TopLeft" />

    <asp:RequiredFieldValidator ID="rfvSize" runat="server" ControlToValidate="ddlSize" Display="None" ErrorMessage="Please select a size" ValidationGroup="ItemValidation" />
    <ajax:ValidatorCalloutExtender ID="vceSize" runat="server" TargetControlID="rfvSize" PopupPosition="TopLeft" />
    
    <ajax:TextBoxWatermarkExtender runat="server" ID="tbweQuantity" TargetControlID="txtQuantity" WatermarkText="Qty." />
    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity" Display="None" ErrorMessage="Please enter a quantity" ValidationGroup="ItemValidation" />
    <ajax:ValidatorCalloutExtender ID="vceQuantity" runat="server" TargetControlID="rfvQuantity" PopupPosition="TopLeft" />

    <asp:DataGrid runat="server" ID="dgInventory" AutoGenerateColumns="false" DataKeyField="ID" AllowSorting="true" GridLines="None" 
            CssClass="default-table" CellPadding="0" CellSpacing="0" AllowPaging="true">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn ItemStyle-Width="6%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnEdit" OnCommand="btnEdit_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Edit" Text="" CssClass="icon-edit edit-icon" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Product" HeaderText="Name" SortExpression="Product" />
            <asp:BoundColumn DataField="Size" HeaderText="Size" SortExpression="Size" />
            <asp:BoundColumn DataField="Color" HeaderText="Color" SortExpression="Color" />
            <asp:TemplateColumn HeaderText="Quantity" SortExpression="Quantity" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("Quantity") %>' Font-Size="18px" Font-Bold="true" Font-Italic="true" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Modify" SortExpression="Quantity">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnAddInventory" CssClass="AddButton" InventoryID='<%# Eval("ID") %>' OnCommand="btnAddInventory_Command" />
                    <br /><asp:TextBox runat="server" ID="txtAdjustInventory" Text='0' CssClass="inventory-box" />
                    <asp:LinkButton runat="server" ID="btnSubtractInventory" CssClass="SubtractButton" InventoryID='<%# Eval("ID") %>' OnCommand="btnSubtractInventory_Command" />
                    <asp:RangeValidator MaximumValue="2147483647" MinimumValue="-2147483647" SetFocusOnError="true" Type="Integer" ControlToValidate="txtAdjustInventory" runat="server" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderStyle-Width="6%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnUpdate" OnClientClick="return confirm('Are you sure you want to update the quantity of this Inventory?');" OnCommand="btnUpdate_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Update" Text="Update" Visible="false" Enabled="false" />
                    <asp:LinkButton runat="server" ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete this Inventory?');" OnCommand="btnDelete_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="" CssClass="icon-trash delete-icon" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

    <asp:UpdatePanel runat="server" ID="upInventory" EnableViewState="true" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress AssociatedUpdatePanelID="upInventory" runat="server" ID="upProgress" DynamicLayout="false" >
        <ProgressTemplate>
            <div class="loading-box"><h3>Loading please wait...</h3></div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>