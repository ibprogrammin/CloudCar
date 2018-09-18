<%@ Page Title="Navigation" ValidateRequest="False" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="MenuItems.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.MenuItems" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Navigation
        <i class="icon-screenshot"></i>
    </h1><hr />

    <asp:Label ID="lblStatus" runat="server" CssClass="status-message" Visible="false" />
    <asp:HiddenField ID="hfID" runat="server" />

    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
            <li class="tab"><a href="#tab-settings">Settings</a></li>
	    </ul>
        <div id="tab-details">
            <label>Title</label>
            <asp:TextBox ID="txtTitle" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <label>URL</label>
            <asp:TextBox ID="txtURL" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <label>Description</label>
            <asp:TextBox ID="txtDetails" runat="server" TextMode="MultiLine" Rows="3" MaxLength="550" CssClass="form-text-area" />
            <br class="clear-both" />
        </div>
        <div id="tab-settings">
            <label>Parent Page</label>
            <asp:DropDownList runat="server" ID="ddlParentPage" DataValueField="ID" DataTextField="Title" CssClass="form-select-box" />
            <br class="clear-both" /><br />
            
            <label>Order</label>
            <asp:TextBox ID="txtOrder" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <label>Css Class</label>
            <asp:TextBox ID="txtCssClass" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <label>Menu</label>
            <asp:TextBox ID="txtMenu" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <label>Icon Image URL</label>
            <asp:TextBox ID="txtIconImageURL" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" />
        </div>
    </div>
    
    <br class="clear-both" />
    
    <asp:Button ID="btnAdd" Text="Save" CssClass="SaveButton" runat="server" CausesValidation="true" ValidationGroup="Item" />
    <asp:Button ID="btnCancel" Text="Clear" CssClass="DeleteButton" runat="server" />
    
    <asp:RequiredFieldValidator ID="rfvT" runat="server" ControlToValidate="txtTitle" ErrorMessage="Please add a Title." SetFocusOnError="true" Display="None" ValidationGroup="Item" />
    <ajax:ValidatorCalloutExtender ID="vceT" runat="server" TargetControlID="rfvT" />

    <asp:RequiredFieldValidator ID="rfvURL" runat="server" ControlToValidate="txtURL" ErrorMessage="Please enter a URL" SetFocusOnError="true" Display="None" ValidationGroup="Item" />
    <ajax:ValidatorCalloutExtender ID="vceURL" runat="server" TargetControlID="rfvURL" />
    
    <asp:RequiredFieldValidator ID="rfvParentPage" runat="server" ControlToValidate="ddlParentPage" ErrorMessage="Please select a parent page" SetFocusOnError="true" Display="None" ValidationGroup="Item" />
    <ajax:ValidatorCalloutExtender ID="vceParenPage" runat="server" TargetControlID="rfvParentPage" />
    
    <asp:RequiredFieldValidator ID="rfvOrder" runat="server" ControlToValidate="txtOrder" ErrorMessage="Please enter an order value" SetFocusOnError="true" Display="None" ValidationGroup="Item" />
    <ajax:ValidatorCalloutExtender ID="vceOrder" runat="server" TargetControlID="rfvOrder" />
    
    <br class="clear-both" /><hr />

    <asp:DataGrid runat="server" 
            ID="gvMenuItems" 
            DataKeyField="id" 
            AllowCustomPaging="false" 
            DataKeyNames="ID" 
            AllowSorting="True" 
            AutoGenerateColumns="False" 
            PageSize="10" 
            AllowPaging="true" 
            CssClass="default-table">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnSelect" OnCommand="btnSelect_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Select" Text="Select" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn HeaderText="Title" DataField="Title" />
            <asp:TemplateColumn HeaderText="URL">
                <ItemTemplate>
                    <asp:HyperLink runat="server" Text='<%# Eval("URL") %>' NavigateUrl='<%# Eval("URL") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn HeaderText="Order" DataField="Order" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="center" />
            <asp:BoundColumn HeaderText="Css Class" DataField="CssClass" />
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-Width="6%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDelete" OnCommand="btnDelete_Command" CommandArgument='<%# Eval("id") %>' CommandName="Delete" Text="" OnClientClick="return confirm('Are you sure you wish to delete this menu item? Your changes cannot be undone.');" CssClass="icon-trash delete-icon" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

</asp:Content>