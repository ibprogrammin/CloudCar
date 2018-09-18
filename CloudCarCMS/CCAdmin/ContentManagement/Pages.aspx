<%@ Page Title="Pages" Language="vb" AutoEventWireup="false" ValidateRequest="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Pages.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.Pages" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        label input {display: inline; float: none !important; margin: 0px 5px;}
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">
    
    <div class="edit-buttons-panel">
        <asp:Button id="NewButton" runat="server" CssClass="SaveButton" CausesValidation="false" style="float: right; width: 140px; position:relative; z-index: 9;" Text="New" ToolTip="Create a new page" />
        <asp:Button runat="server" ID="btnDelete" Text="Delete" CausesValidation="false" CssClass="DeleteButton" OnClientClick="return confirm('Are you sure you wish to delete this page? Your changes cannot be undone.');" style="width: 140px; float: right; margin-right: 10px; margin-left: 0px;" Enabled="False" />
        <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="SaveButton" style="width: 140px; float: right; margin-left: 0px; margin-right: 10px;" Enabled="False" />
        <asp:Button id="BackButton" runat="server" CssClass="BlueButton" CausesValidation="false" style="float: right; margin-left: 0px; margin-right: 10px; width: 45px; z-index: 8;" Text="" ToolTip="Return to page list" Enabled="False" />
    </div>

    <h1 class="form-heading-style">
        Pages
        <i class="icon-file-text"></i>
        <!--img src="/CCTemplates/Admin/Images/icons/cc.pages.icon.dark.png" alt="Pages" /-->
    </h1>
    <hr />
    
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" style="float: left; padding: 12px;" />
    
    <asp:PlaceHolder runat="server" ID="PagesListPlaceHolder" Visible="False">
        
        <asp:GridView runat="server" 
                ID="PagesList" 
                AutoGenerateColumns="False" 
                DataKeyNames="Id" 
                AllowSorting="true" 
                AllowPaging="true" 
                GridLines="None" 
                CssClass="default-table" 
                CellPadding="0" 
                CellSpacing="0" 
                PageSize="12">
            <PagerStyle HorizontalAlign="Right" CssClass="default-table-pager" />
            <PagerSettings PageButtonCount="20" Mode="NumericFirstLast" Position="Bottom" />
            <HeaderStyle CssClass="default-table-header" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <i class="icon-file-text" style="font-size: 21px;"></i>
                        <!--img src="/CCTemplates/Admin/Images/icons/cc.pages.icon.dark.png" alt="<%# Eval("contentTitle") %>" width="25" height="25" /-->
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Page" HeaderStyle-CssClass="TableHeaderText">
                    <ItemTemplate>
                        <asp:LinkButton ID="SelectPageButton" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="SelectPage" Text='<%# Eval("contentTitle") %>' /><br />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="URL" HeaderStyle-CssClass="TableHeaderText">
                    <ItemTemplate>
                        <span class="Highlight"><%# String.Format("/Home/{0}.html", Eval("Permalink")) %></span> (<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# String.Format("/Home/{0}.html", Eval("Permalink")) %>' Text="View" />)
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:LinkButton ID="DeletePageButton" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="DeletePage" Text="" CssClass="icon-trash delete-icon" ToolTip="Delete the current page permanently" OnClientClick="return confirm('Are you sure you want to delete this content page permanently?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </asp:PlaceHolder>
    
    <asp:PlaceHolder runat="server" ID="PageEditPlaceHolder" Visible="False">
    
        <asp:HiddenField runat="server" ID="hfPageId" />
        
        <div class="tab-container">
	        <ul class="tabs">
		        <li class="tab"><a href="#tab-details">Page Details</a></li>
		        <li class="tab"><a href="#tab-primary-content">Primary Content</a></li>
                <li class="tab"><a href="#tab-secondary-content">Secondary Content</a></li>
                <li class="tab"><a href="#tab-scripts">Scripts</a></li>
                <li class="tab"><a href="#tab-seo">SEO</a></li>
		        <li class="tab"><a href="#tab-settings">Menu</a></li>
	        </ul>
            <div id="tab-details">
                <asp:Literal ID="litCurrentPage" runat="server" Visible="false" />

                <label for="txtPageTitle">Heading (Show? <asp:CheckBox runat="server" ID="ShowHeadingCheckBox" />) *</label>
                <asp:TextBox runat="server" ID="txtContentTitle" CssClass="form-text-box" />
                <br class="clear-both" /><br />
                
                <label for="CallToActionDropDown">Call To Action</label>
                <asp:DropDownList runat="server" ID="CallToActionDropDown" DataTextField="Heading" DataValueField="Id" CssClass="form-select-box" />
	            <br class="clear-both" /><br />
                
                <a href="#" class="next-tab" rel="1">next</a>
                <br class="clear-both" />
            </div>
            <div id="tab-primary-content">
                
                <textarea runat="server" 
                    clientidmode="Static" 
                    id="PageContentTextArea" 
                    class="ck-editor" />
                <br class="clear-both" />
                
                <a href="#" class="prev-tab" rel="0">prev</a>
                <a href="#" class="next-tab" rel="2">next</a>
                <br class="clear-both" />
            </div>
            <div id="tab-secondary-content">
                <textarea runat="server" clientidmode="Static" id="SecondaryPageContentTextArea" class="ck-editor" />
                <br class="clear-both" />
                <a href="#" class="prev-tab" rel="1">prev</a>
                <a href="#" class="next-tab" rel="3">next</a>
                <br class="clear-both" />
            </div>
            <div id="tab-scripts">
                <asp:TextBox runat="server" ID="txtScript" TextMode="MultiLine" CssClass="form-text-area-full" />
                <br class="clear-both" /><br />
                <a href="#" class="prev-tab" rel="2">prev</a>
                <a href="#" class="next-tab" rel="4">next</a>
                <br class="clear-both" />
            </div>
            <div id="tab-seo">
                <label for="txtPageTitle">Browser Title *</label>
	            <asp:TextBox runat="server" ID="txtPageTitle" CssClass="form-text-box" />
                <br style="clear: both;" /><br />

                <asp:HiddenField runat="server" ID="hfPermalink" Value="" />
                <label for="txtPermalink">Permalink *</label>
                <asp:TextBox runat="server" ID="txtPermalink" CssClass="form-text-box" />
	            <br style="clear: both;"/><br />
        
                <label for="txtKeywords">Keywords</label>
                <asp:TextBox runat="server" ID="txtKeywords" CssClass="form-text-box" />
	            <br style="clear: both;"/><br />
        
                <label for="txtDescription">Description</label>
                <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="4" CssClass="form-text-box" />
	            <br style="clear: both;"/><br />
        
                <label for="txtBreadcrumbTitle">Breadcrumb Title *</label>
                <asp:TextBox runat="server" ID="txtBreadcrumbTitle" CssClass="form-text-box" />
	            <br style="clear: both;"/><br />
                <a href="#" class="prev-tab" rel="3">prev</a>
                <a href="#" class="next-tab" rel="5">next</a>
                <br class="clear-both" />
            </div>
            <div id="tab-settings">
                <label for="">Parent Page</label>
                <asp:DropDownList runat="server" ID="ddlParentPage" DataTextField="ContentTitle" DataValueField="id" CssClass="form-select-box" />
                <br class="clear-both" /><br />
        
                <label for="ddlMenu">Menu</label>
                <asp:DropDownList runat="server" ID="ddlMenu" DataTextField="menu" DataValueField="id" CssClass="form-select-box" />
	            <br class="clear-both" /><br />
        
                <label for="txtMenuOrder">Display Order</label>
                <asp:TextBox runat="server" ID="txtMenuOrder" Text="1" CssClass="form-text-box" />
	            <br class="clear-both" /><br />
        
                <label for="ckbSubMenu">Show in Sub Menu</label>
                <asp:CheckBox runat="server" id="ckbSubMenu" CssClass="form-check-box" />
                <br class="clear-both" /><br />
                
                <a href="#" class="prev-tab" rel="4">Prev</a>
                <br class="clear-both" />
            </div>
        </div>

        <asp:RequiredFieldValidator ID="rfvPT" runat="server" ControlToValidate="txtPageTitle" ErrorMessage="You should create a title for this page." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vcePT" runat="server" TargetControlID="rfvPT" />

        <asp:RequiredFieldValidator ID="rfvPC" runat="server" ControlToValidate="PageContentTextArea" ErrorMessage="You should add some content to this page." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vcePC" runat="server" TargetControlID="rfvPC" />

        <asp:RequiredFieldValidator ID="rfvPL" runat="server" ControlToValidate="txtPermalink" ErrorMessage="You should create a permalink for this page." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vcePL" runat="server" TargetControlID="rfvPL" />
        <CC:PermalinkValidator ID="pvPermalink" runat="server" ControlToValidate="txtPermalink" PermalinkType="ContentPage" ErrorMessage="The permalink you have choosen is not unique." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vcePermalinkValidator" runat="server" TargetControlID="pvPermalink" />
        
        <asp:RequiredFieldValidator ID="rfvMenuTitle" runat="server" ControlToValidate="txtBreadcrumbTitle" ErrorMessage="You should create a breadcrumb title for this page." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vceMenuTitle" runat="server" TargetControlID="rfvMenuTitle" />

        <asp:RequiredFieldValidator ID="rfvMenu" runat="server" ControlToValidate="ddlMenu" ErrorMessage="You should select a menu for this page." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vceMenu" runat="server" TargetControlID="rfvMenu" />

        <asp:RequiredFieldValidator ID="rfvMO" runat="server" ControlToValidate="txtMenuOrder" ErrorMessage="You should select a position in the menu for this page." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vceMO" runat="server" TargetControlID="rfvMO" />
    
    </asp:PlaceHolder>

</asp:Content>