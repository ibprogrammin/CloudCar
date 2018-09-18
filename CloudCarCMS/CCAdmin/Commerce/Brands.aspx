<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Brands.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Brands" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Brands
        <i class="icon-eye-open"></i>
    </h1>
    <hr />
    
    <asp:HyperLink runat="server" ID="hlBackToProduct" Text="&laquo; Back to Product" style="clear: left;" Font-Size="Small" Visible="false" />
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" />
    
    <asp:HiddenField runat="server" ID="hfBrandID" />

    <div class="tab-container">
	    <ul class="tabs">
		    <li class="tab"><a href="#tab-details">Details</a></li>
            <li class="tab"><a href="#tab-seo">SEO</a></li>
	    </ul>
        <div id="tab-details">
            <label>Name *</label>
            <asp:TextBox runat="server" ID="txtName" CssClass="form-text-box" />
            <ajax:TextBoxWatermarkExtender ID="tbwName" runat="server" TargetControlID="txtName" WatermarkText="Name" />
            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" Display="None" ErrorMessage="Please enter a name" ValidationGroup="ItemValidation" />
            <ajax:ValidatorCalloutExtender ID="vceName" runat="server" TargetControlID="rfvName" PopupPosition="TopLeft" />
            <br class="clear-both" /><br />
        
            <label>URL</label>
            <asp:TextBox runat="server" ID="txtLink" CssClass="form-text-box" />
            <ajax:TextBoxWatermarkExtender ID="tbwLink" runat="server" TargetControlID="txtLink" WatermarkText="URL" />
            <br class="clear-both" /><br />

            <label style="min-height: 100px;">
                Logo Image *
                <asp:Image runat="server" ID="imgLogo" Visible="false" CssClass="image-display" />
            </label>
            <asp:HiddenField runat="server" ID="hfLogoImageID" />
            <asp:Label runat="server" ID="lblLogoFileLocation" ReadOnly="true" Visible="false" CssClass="display-message" style="margin-bottom: 10px;" />
            <div class="form-file-upload-display">
	            <div class="form-fake-upload">
		            <input type="text" name="logofilename" readonly="readonly" /> <!-- browse button is here as background -->
	            </div>
                <asp:FileUpload runat="server" ID="fuLogoImage" ToolTip="Select the logo for this brand." size="20" CssClass="form-real-upload" onchange="this.form.logofilename.value = this.value;" /><br />
            </div>
            <br class="clear-both" />
        </div>
        <div id="tab-seo">
            <label>Permalink *</label>
            <asp:TextBox runat="server" ID="txtPermalink" CssClass="form-text-box" />
            <ajax:TextBoxWatermarkExtender ID="tbwePermalink" runat="server" TargetControlID="txtPermalink" WatermarkText="Permalink" />
            <asp:RequiredFieldValidator ID="rfvPermalink" runat="server" ControlToValidate="txtPermalink" Display="None" ErrorMessage="Please specify a unique permalink for this brand" ValidationGroup="ItemValidation" />
            <ajax:ValidatorCalloutExtender ID="vcePermalink" runat="server" TargetControlID="rfvPermalink" PopupPosition="TopLeft" />
            <br class="clear-both" /><br />

            <label>Description *</label>
            <asp:TextBox runat="server" ID="txtDescription" Rows="4" TextMode="MultiLine" CssClass="form-text-area" />
            <ajax:TextBoxWatermarkExtender ID="tbwDescription" runat="server" TargetControlID="txtDescription" WatermarkText="Description" />
            <br class="clear-both" /><br />
    
            <label>Keywords</label>
            <asp:TextBox runat="server" ID="txtKeywords" Rows="4" TextMode="MultiLine" CssClass="form-text-area" />
            <ajax:TextBoxWatermarkExtender ID="tbwKeywords" runat="server" TargetControlID="txtKeywords" WatermarkText="Keywords" />
            <br class="clear-both" />
        </div>
    </div>

    <br class="clear-both" />

    <asp:Button id="btnAdd" runat="server" CssClass="SaveButton" ValidationGroup="ItemValidation" CausesValidation="true" UseSubmitBehavior="true" Text="Save" />
    <asp:Button id="btnClear" runat="server" CssClass="DeleteButton" Text="Clear" />
    
    <br style="clear: both;" /><br /><hr />
    

    <asp:DataGrid runat="server" 
            ID="dgBrands" 
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
            <asp:TemplateColumn HeaderText="Brand" SortExpression="Name" HeaderStyle-Width="12%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnEdit" OnCommand="btnEdit_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Edit" Text='<%# Eval("Name") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="Permalink" SortExpression="Premalink">
                <ItemTemplate>
                    <a href="<%# String.Format("/Shop/Brand/{0}.html", Server.UrlDecode(CStr(Eval("Permalink"))))%>" target="_blank"><%# String.Format("/Shop/Brand/{0}.html", Server.UrlDecode(CStr(Eval("Permalink"))))%></a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="Description" HeaderText="Description" SortExpression="Description" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="8%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete this Brand?');" OnCommand="btnDelete_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="" class="icon-trash delete-icon" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

    <br />

</asp:Content>