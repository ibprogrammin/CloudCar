<%@ Page Title="Address" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Addresses.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Addresses" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Address
        <i class="icon-home"></i>
    </h1><hr />
    
    <asp:HyperLink runat="server" ID="hlBackToOrder" Visible="false" Text="&laquo; Go back to Order" style="clear: left;" /><br /><br />
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" />
    
    <fieldset>
    
        <h2 class="form-heading-style">Details</h2>
        <br />
    
        <asp:HiddenField runat="server" ID="hfID" />

        <label>Address *</label>
        <asp:TextBox runat="server" ID="txtAddress" style="width: 650px;" />
        <br class="clear-both" /><br />
        
        <label>City *</label>
        <asp:TextBox runat="server" ID="txtCity" style="width: 650px;" />
        <br class="clear-both" /><br />
        
        <label>Postal Code *</label>
        <asp:TextBox runat="server" ID="txtPostalCode" style="width: 650px;" />
        <br class="clear-both" /><br />
        
        <label>Country *</label>
        <asp:DropDownList runat="server" ID="ddlCountry" DataTextField="Name" DataValueField="ID" AutoPostBack="true" style="width: 670px;" />
        <br class="clear-both" /><br />
        
        <label>Province *</label>
        <asp:DropDownList runat="server" ID="ddlProvince" DataTextField="Name" DataValueField="ID" style="width: 670px;" />
        <br class="clear-both" /><br />
    
    </fieldset>

    <br class="clear-both" />

    <asp:Button id="btnAdd" runat="server" CssClass="SaveButton" ValidationGroup="ItemValidation" CausesValidation="true" UseSubmitBehavior="true" Text="Save" />
    <asp:Button id="btnClear" runat="server" CssClass="DeleteButton" Text="Clear" />

    <br class="clear-both" /><br /><hr />

    <ajax:TextBoxWatermarkExtender ID="tbwAddress" runat="server" TargetControlID="txtAddress" WatermarkText="Address" WatermarkCssClass="WMText" />
    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress" Display="None" ErrorMessage="Please enter an Address" ValidationGroup="ItemValidation" />
    <ajax:ValidatorCalloutExtender ID="vceAddress" runat="server" TargetControlID="rfvAddress" PopupPosition="TopLeft" />
    
    <ajax:TextBoxWatermarkExtender ID="tbeCity" runat="server" TargetControlID="txtCity" WatermarkText="City" WatermarkCssClass="WMText" />
    <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" Display="None" ErrorMessage="Please enter a City" ValidationGroup="ItemValidation" />
    <ajax:ValidatorCalloutExtender ID="vceCity" runat="server" TargetControlID="rfvCity" PopupPosition="TopLeft" />
    
    <ajax:TextBoxWatermarkExtender ID="tbwPostalCode" runat="server" TargetControlID="txtPostalCode" WatermarkText="Postal Code" WatermarkCssClass="WMText" />
    <asp:RequiredFieldValidator ID="rfvPostalCode" runat="server" ControlToValidate="txtPostalCode" Display="None" ErrorMessage="Please enter a Postal Code" ValidationGroup="ItemValidation" />
    <ajax:ValidatorCalloutExtender ID="vcePostalCode" runat="server" TargetControlID="rfvPostalCode" PopupPosition="TopLeft" />        

    <asp:DataGrid runat="server" ID="dgAddress" AutoGenerateColumns="false" DataKeyField="ID" AllowPaging="true"
            AllowSorting="true" GridLines="None" CssClass="default-table" CellPadding="0" CellSpacing="0">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="" SortExpression="ID" HeaderStyle-Width="6%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnEdit" OnCommand="btnEdit_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Edit" Text="" CssClass="icon-edit edit-icon" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Address" HeaderText="Address" SortExpression="Address" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundColumn DataField="City" HeaderText="City" SortExpression="City" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
            <asp:TemplateColumn HeaderText="Province/State" SortExpression="Province" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# getProvinceName(CInt(Eval("ProvStateID"))) %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Country" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# getCountryName(CInt(Eval("ProvStateID"))) %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="PCZIP" HeaderText="Postal/Zip Code" SortExpression="PostalCode" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
            <asp:TemplateColumn ItemStyle-HorizontalAlign="right" HeaderStyle-Width="6%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete this Address?');" OnCommand="btnDelete_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="" CssClass="icon-trash delete-icon" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

</asp:Content>