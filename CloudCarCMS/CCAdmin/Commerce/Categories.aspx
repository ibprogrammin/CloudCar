<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Categories.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Categories" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Categories
        <i class="icon-sitemap"></i>
    </h1>
    <hr />

    <asp:HyperLink runat="server" ID="hlBackToProduct" Text="&laquo; Back to Product" style="clear: left;" Visible="false" /><br />
    
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" />

    <asp:HiddenField runat="server" ID="hfCategoryID" />
    
    <div class="tab-container">
	    <ul class="tabs">
		    <li class="tab"><a href="#tab-details">Details</a></li>
		    <li class="tab"><a href="#tab-content">Content</a></li>
            <li class="tab"><a href="#tab-seo">SEO</a></li>
	    </ul>
        <div id="tab-details">
            <label>Name</label>
            <asp:TextBox runat="server" ID="txtName" CssClass="form-text-box" />
            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" Display="None" ErrorMessage="Please enter a name for this category" ValidationGroup="ItemValidation" />
            <ajax:ValidatorCalloutExtender ID="vceName" runat="server" TargetControlID="rfvName" PopupPosition="TopLeft" />
            <br class="clear-both" />
        </div>
        <div id="tab-content">
            <textarea runat="server" id="DetailsTextArea" class="ck-editor" />
        </div>
        <div id="tab-seo">
            <label>Browser Title</label>
            <asp:TextBox runat="server" ID="txtBrowserTitle" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label>Permalink</label>
            <asp:HiddenField runat="server" ID="HiddenField1" />
            <asp:TextBox runat="server" ID="txtPermalink" CssClass="form-text-box" />
            <asp:RequiredFieldValidator ID="rfvPermalink" runat="server" ControlToValidate="txtPermalink" Display="None" ErrorMessage="Please specify a permalnk for this category" ValidationGroup="ItemValidation" />
            <ajax:ValidatorCalloutExtender ID="vcePermalink" runat="server" TargetControlID="rfvPermalink" PopupPosition="TopLeft" />
            <br class="clear-both" /><br />

            <label>Description</label>
            <asp:TextBox runat="server" ID="txtDescription" Rows="4" TextMode="MultiLine" CssClass="form-text-area" />
            <br class="clear-both" /><br />
        
            <label>Keywords</label>
            <asp:TextBox runat="server" ID="txtKeywords" Rows="4" TextMode="MultiLine" CssClass="form-text-area" />
            <br class="clear-both" />
        </div>
    </div>

    <br class="clear-both" />

    <asp:Button id="btnAdd" runat="server" CssClass="SaveButton" ValidationGroup="ItemValidation" CausesValidation="true" UseSubmitBehavior="true" Text="Save" />
    <asp:Button id="btnClear" runat="server" CssClass="DeleteButton" Text="Clear" />

    <br class="clear-both" /><br /><hr />
    
    <asp:DataGrid runat="server" ID="dgCategories" AutoGenerateColumns="false" DataKeyField="ID" AllowSorting="true" 
            GridLines="None" CssClass="default-table" CellPadding="0" CellSpacing="0" AllowPaging="true">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="Category" SortExpression="Name" HeaderStyle-Width="12%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnEdit" OnCommand="btnEdit_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Edit" Text='<%# Eval("Name") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Link" SortExpression="Premalink">
                <ItemTemplate>
                    <a href='<%# String.Format("/Shop/{0}.html", Server.UrlDecode(CStr(Eval("Permalink"))))%>' target="_blank"><%# String.Format("/Shop/{0}.html", Server.UrlDecode(CStr(Eval("Permalink"))))%></a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Description" HeaderText="Description" SortExpression="Description" />
            <asp:TemplateColumn ItemStyle-HorizontalAlign="right" HeaderStyle-Width="8%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete this Category?');" OnCommand="btnDelete_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="" CssClass="icon-trash delete-icon" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

</asp:Content>