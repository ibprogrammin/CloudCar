<%@ Page Title="Colors" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Colours.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Colours" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Colors
        <i class="icon-tint"></i>
    </h1>
    <hr />
    
    <asp:HyperLink runat="server" ID="hlBackToProduct" Text="&laquo; Back to Product" style="clear: left;" Font-Size="Small" Visible="false" />
    
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" />
    <asp:HiddenField runat="server" ID="hfColorID" />
    
    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
	    </ul>
        <div id="tab-details">
        
            <label>Color</label>
            <asp:TextBox runat="server" ID="txtColor" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label>Color Code (RGB HEX)</label>
            <asp:TextBox runat="server" ID="txtColourCode" CssClass="form-text-box" />
            <br class="clear-both" />

        </div>
    </div>
    
    <br class="clear-both" />

    <asp:Button id="btnAdd" runat="server" CssClass="SaveButton" ValidationGroup="ItemValidation" CausesValidation="true" UseSubmitBehavior="true" Text="Save" />
    <asp:Button id="btnClear" runat="server" CssClass="DeleteButton" Text="Clear" />
    
    <br class="clear-both" /><hr />
    
    <ajax:TextBoxWatermarkExtender ID="tbwColor" runat="server" TargetControlID="txtColor" WatermarkText="Colour Name" />
    <asp:RequiredFieldValidator runat="server" ID="rfvColor" ControlToValidate="txtColor" Display="None" Text="Please enter a color" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceColor" TargetControlID="rfvColor" PopupPosition="TopLeft" />

    <asp:DataGrid runat="server" 
            ID="dgColors" 
            AutoGenerateColumns="false" 
            DataKeyField="ID" 
            AllowSorting="true" 
            AllowPaging="true" 
            CssClass="default-table" 
            CellPadding="0" 
            CellSpacing="0">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" 
                Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="Color" SortExpression="Name">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnEdit" OnCommand="EditButtonClick" CommandArgument='<%# Eval("ID") %>' CommandName="Edit" Text='<%# Eval("Name") %>' CausesValidation="False" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="RGBColourCode" HeaderText="Code" />
            <asp:TemplateColumn HeaderText="Preview" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <%# String.Format("<div class=""display-color-preview"" style=""background-color: {0};""></div>", Eval("RGBColourCode"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete this Colour?');" OnCommand="DeleteButtonClick" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" CausesValidation="false" Text="" CssClass="icon-trash delete-icon" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

</asp:Content>