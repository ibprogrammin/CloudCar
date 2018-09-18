<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="EditMenuCategories.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.ClientCustom.EditMenuCategories" %>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

    <h1 style="float: left;">Menu Category</h1>
    <asp:LinkButton id="btnAddMenuCategory" runat="server" CssClass="GreenButton SerifStack" style="width: 260px; float: left; margin-left: 125px; margin-top: 8px;"><span class="GreenButton">New Category</span></asp:LinkButton>
    <br style="clear: both;" />

    <asp:Label ID="lblStatus" runat="server" CssClass="status-message" Visible="false" />

    <asp:PlaceHolder ID="phAddMenuCategory" runat="server" Visible="false">
        <h2 class="BoldLight SansStack">Add/Edit Menu Category</h2>
        <asp:HiddenField ID="hfMCID" runat="server" />
        <fieldset style="width: 530px;">
            <p>
                <label>Category</label><br />
                <asp:TextBox ID="txtCategory" runat="server" CssClass="TextBox" TextMode="SingleLine" />
            </p>
        </fieldset>
        <br />
        <asp:LinkButton id="btnAddMC" runat="server" CssClass="GreenButton SerifStack" ValidationGroup="Category" CausesValidation="true" style="width: 250px; float: left;"><span class="GreenButton">Add</span></asp:LinkButton>
        <asp:LinkButton id="btnCancelMC" runat="server" CssClass="RedButton SerifStack" style="margin-left: 20px; width: 250px; float: left;"><span class="RedButton">Cancel</span></asp:LinkButton>
        <br style="clear: both;" />
        
        <asp:RequiredFieldValidator ID="rfvC" runat="server" ControlToValidate="txtCategory" ErrorMessage="Please add a Category." SetFocusOnError="true" Display="None" ValidationGroup="Category" />
        <ajax:ValidatorCalloutExtender ID="vceC" runat="server" TargetControlID="rfvC" />
        
    </asp:PlaceHolder>
    <br />
    <asp:GridView runat="server" ID="gvMenuCategories" AutoGenerateColumns="False" DataKeyNames="id" AllowSorting="true" 
            Width="650px" GridLines="None" CssClass="ShoppingCart" CellPadding="0" CellSpacing="0">
        <HeaderStyle CssClass="TableHeader" />
        <RowStyle CssClass="SCRow" />
        <AlternatingRowStyle CssClass="SCAlternatingRow" />
        <Columns>
            <asp:TemplateField HeaderStyle-CssClass="HeaderLeftCell" ItemStyle-Width="20%">
                <ItemTemplate>
                    <asp:LinkButton ID="lbSelect" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="Select" Text="Edit" /><br />
                    <asp:LinkButton ID="lbDelete" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="DeleteItem" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this Menu Category? This cannot be undone.');" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="category" HeaderStyle-CssClass="HeaderCell" HeaderText="Category" />
            <asp:TemplateField HeaderStyle-CssClass="HeaderRightCell" ItemStyle-Width="10px"><ItemTemplate></ItemTemplate></asp:TemplateField>
        </Columns>
    </asp:GridView>
    

    <br /><br />

    <h1 style="float: left;">Menu Category Items</h1>
    <asp:LinkButton id="btnAddMenuCategoryItem" runat="server" CssClass="GreenButton SerifStack" style="width: 260px; float: left; margin-left: 25px; margin-top: 8px;"><span class="GreenButton">New Item</span></asp:LinkButton>
    <br style="clear: both;" />
    
    <asp:PlaceHolder ID="phAddMenuCategoryItem" runat="server" Visible="false">
        <h1 class="BoldLight SansStack">Add/Edit Menu Category Item</h1>
        <asp:HiddenField ID="hfMCIID" runat="server" />
        <fieldset style="width: 530px;">
            <p>
                <label>Category</label><br />
                <asp:DropDownList runat="server" ID="ddlMenuCategory" DataTextField="category" DataValueField="id" CssClass="SelectBox" />
            </p>
            <p>
                <label>Product</label><br />
                <asp:DropDownList runat="server" ID="ddlProduct" DataTextField="name" DataValueField="id" CssClass="SelectBox" />
            </p>
        </fieldset>
        <br />
        <asp:LinkButton id="btnAddMCI" runat="server" CssClass="GreenButton SerifStack" ValidationGroup="MCI" CausesValidation="true" style="width: 250px; float: left;"><span class="GreenButton">Add</span></asp:LinkButton>
        <asp:LinkButton id="btnCancelMCI" runat="server" CssClass="RedButton SerifStack" style="margin-left: 20px; width: 250px; float: left;"><span class="RedButton">Cancel</span></asp:LinkButton>
        <br style="clear: both;" />
        
        <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddlMenuCategory" ErrorMessage="Please select a category." SetFocusOnError="true" Display="None" ValidationGroup="MCI" />
        <ajax:ValidatorCalloutExtender ID="vceCategory" runat="server" TargetControlID="rfvCategory" />

        <asp:RequiredFieldValidator ID="rfvProduct" runat="server" ControlToValidate="ddlProduct" ErrorMessage="Please select a product." SetFocusOnError="true" Display="None" ValidationGroup="MCI" />
        <ajax:ValidatorCalloutExtender ID="vceProduct" runat="server" TargetControlID="rfvProduct" />
        
    </asp:PlaceHolder>
    <br />
    <asp:GridView runat="server" ID="gvMenuCategoryItems" AutoGenerateColumns="False" DataKeyNames="id" AllowSorting="true" 
            Width="650px" GridLines="None" CssClass="ShoppingCart" CellPadding="0" CellSpacing="0">
        <HeaderStyle CssClass="TableHeader" />
        <RowStyle CssClass="SCRow" />
        <AlternatingRowStyle CssClass="SCAlternatingRow" />
        <Columns>
            <asp:TemplateField HeaderStyle-CssClass="HeaderLeftCell" ItemStyle-Width="20%">
                <ItemTemplate>
                    <asp:LinkButton ID="lbSelect" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="Select" Text="Edit" /><br />
                    <asp:LinkButton ID="lbDelete" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="DeleteItem" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this Menu Item? This cannot be undone.');" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="category" HeaderStyle-CssClass="HeaderCell" HeaderText="Category" />
            <asp:BoundField DataField="product" HeaderStyle-CssClass="HeaderCell" HeaderText="Product" />
            <asp:TemplateField HeaderStyle-CssClass="HeaderRightCell" ItemStyle-Width="10px"><ItemTemplate></ItemTemplate></asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>