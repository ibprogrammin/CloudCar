<%@ Page Title="" Language="vb" ValidateRequest="false" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="ProductDetails.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.ProductDetails" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        label { margin-left: 10px; }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        <asp:TextBox runat="server" ID="ProductNameTextBox" CssClass="h1-editable-text-box" ToolTip="Edit Product Name" />
        <asp:Literal runat="server" ID="litProductID" Text="" Visible="false" />
        <i class="icon-tags"></i>
    </h1><hr />

    <div class="search-bar">
        <asp:HyperLink ID="dlBackToProducts" runat="server" NavigateUrl="~/CCAdmin/Commerce/Products.aspx" Text="" CssClass="BlueButton return-button" />
        <asp:DropDownList runat="server" ID="ddlPActive" ToolTip="Active" TabIndex="33" CssClass="active-drop-down">
            <asp:ListItem Text="Active" Value="True" />
            <asp:ListItem Text="Inactive" Value="False" />
        </asp:DropDownList>
        <asp:Button id="btnDeleteProduct" runat="server" CssClass="DeleteButton" OnClientClick="return confirm('Are you sure you want to delete this Product? This cannot be undone!');" TabIndex="34" Text="Delete" />
        <asp:Button id="btnSavePTop" runat="server" CssClass="SaveButton" CausesValidation="true" ValidationGroup="product" TabIndex="34" Text="Save" />
    </div>
    
    <asp:Label runat="server" ID="lblMessage" CssClass="status-message" />

    <asp:HiddenField runat="server" ID="hfProductID" />

    <div class="tab-container">
	    <ul class="tabs">
		    <li class="tab"><a href="#tab-content">Content</a></li>
            <li class="tab"><a href="#tab-multimedia">Multimedia</a></li>
            <li class="tab"><a href="#tab-sales-info">Sales Info</a></li>
            <li class="tab"><a href="#tab-prices">Prices</a></li>
		    <li class="tab"><a href="#tab-shipping">Shipping</a></li>
            <li class="tab"><a href="#tab-colors">Colours</a></li>
            <li class="tab"><a href="#tab-sizes">Sizes</a></li>
            <li class="tab"><a href="#tab-recomendations">Recomendations</a></li>
            <li class="tab"><a href="#tab-seo">SEO</a></li>
	    </ul>
        <div id="tab-content">
            <textarea runat="server" id="DetailsTextArea" class="ck-editor" />
            <br class="clear-both" />
            
            <a href="#" class="next-tab" rel="1">next</a>
            <br class="clear-both" />
        </div>
        <div id="tab-multimedia">
            <label style="min-height: 110px;">
                Default Image <br />
                <img runat="server" ID="imgPDI" alt="Default Image Preview" visible="false" src="" class="image-display" />
            </label>
            <asp:HiddenField runat="server" ID="hfPDIID" />
            <asp:Label runat="server" ID="lblDefaultImageLocation" ReadOnly="true" Visible="false" CssClass="display-message" />
            <div class="form-file-upload-display">
	            <div class="form-fake-upload">
		            <input type="text" name="defaultimagefilename" readonly="readonly" /> <!-- browse button is here as background -->
	            </div>
                <asp:FileUpload runat="server" ID="fuPDefaultImage" ToolTip="Select the default image for this product." size="20" CssClass="form-real-upload" onchange="this.form.defaultimagefilename.value = this.value;" /><br />
            </div><br />
            <span class="image-requirements">Recomended Dimensions: W (258px) x H (195px)</span>
            <br class="clear-both" /><hr />       
            
            
            <label style="min-height: 110px;">
                Header Image<br />
                <img runat="server" ID="imgPHI" alt="Default Image Preview" visible="false" src="" class="image-display" />
            </label>
            <asp:HiddenField runat="server" ID="hfPHeaderImageID" />
            <asp:Label runat="server" ID="lblHeaderImageLocation" ReadOnly="true" Visible="false" CssClass="display-message" />
            <div class="form-file-upload-display">
	            <div class="form-fake-upload">
		            <input type="text" name="headerfilename" readonly="readonly" /> <!-- browse button is here as background -->
	            </div>
                <asp:FileUpload runat="server" ID="fuPHeaderImage" ToolTip="Select the header image for this product." size="20" CssClass="form-real-upload" onchange="this.form.headerfilename.value = this.value;" /><br />
            </div><br />
            <span class="image-requirements">Recomended Dimensions: W (970px) x H (565px)</span>
            <br class="clear-both" /><hr />
        
            <label style="min-height: 130px;">Digital Product File</label>
            <asp:HiddenField runat="server" ID="hfDigitalMediaFile" />
            <asp:DropDownList runat="server" ID="ddlIsDigitalMedia" ToolTip="Is this a digital product" style="width: 140px; margin-right: 510px;">
                <asp:ListItem Text="No" Value="False" Selected="True" />
                <asp:ListItem Text="Yes" Value="True" />
            </asp:DropDownList>
            <asp:Label runat="server" ID="lblDigitalFileLocation" ReadOnly="true" Visible="false" CssClass="display-message" /><br />
            <div class="form-file-upload-display">
	            <div class="form-fake-upload">
		            <input type="text" name="digitalfilename" readonly="readonly" /> <!-- browse button is here as background -->
	            </div>
                <asp:FileUpload runat="server" ID="fuDigitalMediaFile" ToolTip="Upload the digital file for this product." size="20" CssClass="form-real-upload" onchange="this.form.digitalfilename.value = this.value;" /><br />
            </div>
	        <br class="clear-both" /><br />
            
            <a href="#" class="prev-tab" rel="0">prev</a>
            <a href="#" class="next-tab" rel="2">next</a>
            <br class="clear-both" />
        </div>
        <div id="tab-sales-info">
            <label>SKU</label>
            <asp:TextBox runat="server" ID="txtPSKU" ToolTip="SKU" MaxLength="50" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label>Brand (<asp:HyperLink runat="server" ID="hlAddBrand" Text="New" NavigateUrl="~/CCAdmin/Commerce/Brands.aspx" />)</label>
            <asp:DropDownList runat="server" ID="ddlPBrand" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ID" ToolTip="Brand" CssClass="form-select-box">
                <asp:ListItem Text="Brand" Value="" />
            </asp:DropDownList>
            <br class="clear-both" /><br />
        
            <label>Category (<asp:HyperLink runat="server" ID="hlAddCategory" Text="New" NavigateUrl="~/CCAdmin/Commerce/Categories.aspx" />)</label>
            <asp:DropDownList runat="server" ID="ddlPCategory" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ID" ToolTip="Category" CssClass="form-select-box">
                <asp:ListItem Text="Category" Value="" />
            </asp:DropDownList>
            <br class="clear-both" /><br />

            <label>Track Product Inventory</label>
            <asp:DropDownList runat="server" ID="ddlTrackInventory" ToolTip="Track Inventory" style="width: 140px;">
                <asp:ListItem Text="No" Value="False" />
                <asp:ListItem Text="Yes" Value="True" />
            </asp:DropDownList>
            <br class="clear-both" /><br />
    
            <label>Top Selling Item</label>
            <asp:DropDownList runat="server" ID="ddlTopSeller" ToolTip="Top Seller" style="width: 140px;">
                <asp:ListItem Text="No" Value="False" />
                <asp:ListItem Text="Yes" Value="True" />
            </asp:DropDownList>
            <br class="clear-both" /><br />
    
            <label>Clearance Item</label>
            <asp:DropDownList runat="server" ID="ddlClearance" ToolTip="Clearance Item" style="width: 140px;">
                <asp:ListItem Text="No" Value="False" />
                <asp:ListItem Text="Yes" Value="True" />
            </asp:DropDownList>
            <br class="clear-both" /><br />
            
            <a href="#" class="prev-tab" rel="1">prev</a>
            <a href="#" class="next-tab" rel="3">next</a>
            <br class="clear-both" />
        </div>
        <div id="tab-prices">
            <label>Pricing Unit</label>
            <asp:TextBox runat="server" ID="PricingUnitTextBox" ToolTip="Pricing Unit" style="width: 120px;" />
            <br class="clear-both" /><br />

            <label>Cost</label>
            <asp:TextBox runat="server" ID="txtPCost" ToolTip="Cost" style="width: 120px;" />
            <br class="clear-both" /><br />
            
            <label>List Price</label>
            <asp:TextBox runat="server" ID="ListPriceTextBox" ToolTip="List Price" style="width: 120px;" />
            <br class="clear-both" /><br />

            <label>Retail Price</label>
            <asp:TextBox runat="server" ID="txtPPrice" ToolTip="Price" style="width: 120px;" />
            <br class="clear-both" /><br />
            
            <label>Wholesale Price</label>
            <asp:TextBox runat="server" ID="txtPWarehousePrice" ToolTip="Warehouse Price" style="width: 120px;" />
            <br class="clear-both" /><br />
            
            <label>Associate Price</label>
            <asp:TextBox runat="server" ID="txtPAssociatePrice" ToolTip="Associate Price" style="width: 120px;" />
            <br class="clear-both" /><br />
            
            <!--label>Discount %</label-->
            <asp:TextBox runat="server" ID="DiscountTextBox" ToolTip="Discount Percent" style="width: 120px;" Visible="false" />

            <a href="#" class="prev-tab" rel="2">prev</a>
            <a href="#" class="next-tab" rel="4">next</a>
            <br class="clear-both" />
        </div>
        <div id="tab-shipping">
            <label>Weight (Kg)</label>
            <asp:TextBox runat="server" ID="txtPWeight" ToolTip="Weight" style="width: 120px;" />
            <br class="clear-both" /><br />
        
            <label>Length (cm)</label>
            <asp:TextBox runat="server" ID="txtPLength" ToolTip="Length" style="width: 120px;" />
            <br class="clear-both" /><br />
        
            <label>Width (cm)</label>
            <asp:TextBox runat="server" ID="txtPWidth" ToolTip="Width" style="width: 120px;" />
            <br class="clear-both" /><br />
        
            <label>Height (cm)</label>
            <asp:TextBox runat="server" ID="txtPHeight" ToolTip="Height" style="width: 120px;" />
            <br class="clear-both" /><br />
            
            <a href="#" class="prev-tab" rel="3">prev</a>
            <a href="#" class="next-tab" rel="5">next</a>
            <br class="clear-both" />
        </div>
        <div id="tab-colors">
            <asp:UpdatePanel runat="server" ID="ColorsUpdatePanel" RenderMode="Inline" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Conditional">
                <ContentTemplate>

                    <h3>Colours (<asp:HyperLink ID="hlAddColor" runat="server" NavigateUrl="~/CCAdmin/Commerce/Colours.aspx" Text="New" />)</h3>
                    <p>Add a colour option to this product.</p>
                
                    <div class="search-bar">
                        <asp:DropDownList runat="server" ID="ddlPAddColour" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ID" style="width: 190px; float: left; margin-left: 0px; margin-right: 10px;">
                            <asp:ListItem Text="Colour" Value="" />
                        </asp:DropDownList>
                
                        <div class="form-file-upload-display-medium">
	                        <div class="form-fake-upload-medium">
		                        <input type="text" name="colorfilename" readonly="readonly" /> <!-- browse button is here as background -->
	                        </div>
	                        <asp:FileUpload runat="server" ID="fuPAddColorImage" ToolTip="Select an image for this colour." size="20" CssClass="form-real-upload" onchange="this.form.colorfilename.value = this.value;" /><br />
                        </div>
                
                        <asp:Button id="btnAddColor" runat="server" CssClass="SaveButton" Text="Add" style="margin-left: 10px; float: left; width: 140px;" />
                    </div>

                    <asp:DataGrid runat="server" 
                            ID="dtgPAddingColors" 
                            AutoGenerateColumns="false" 
                            CellPadding="0" 
                            CellSpacing="0" 
                            GridLines="None" 
                            CssClass="default-table">
                        <HeaderStyle CssClass="default-table-header" />
                        <Columns>
                            <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" HeaderText="Image">
                                <ItemTemplate>
                                    <asp:Image runat="server" id="imgSColor" ImageUrl='<%# String.format("/images/db/{0}/60/{1}.jpg",Eval("ImageID"), getColorName(CType(Eval("ColorID"), Integer)).Replace(" ", "")) %>' CssClass="image-display-table" /><br />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="ColorID" HeaderText="Name" ItemStyle-Width="60%" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Literal runat="server" Text='<%# getColorName(CType(Eval("ColorID"), Integer)) %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDeleteColor" runat="server" OnCommand="dtgPAddingColors_Delete" CommandArgument='<%# Eval("ColorID") %>' CommandName="DeleteColor" Text="" CssClass="icon-trash delete-icon" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                
                    <asp:DataGrid runat="server" 
                            ID="dtgPSelectedColors" 
                            AutoGenerateColumns="false" 
                            CellPadding="0" 
                            CellSpacing="0" 
                            GridLines="None" 
                            CssClass="default-table">
                        <HeaderStyle CssClass="default-table-header" />
                        <Columns>
                            <asp:BoundColumn Visible="false" DataField="ID" />
                            <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" HeaderText="Image">
                                <ItemTemplate>
                                    <asp:Image runat="server" id="imgSColor" ImageUrl='<%# String.format("/images/db/{0}/60/{1}.jpg",Eval("ImageID"), getColorName(CType(Eval("ColorID"), Integer)).Replace(" ", "")) %>' CssClass="image-display-table" /><br />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="ColorID" HeaderText="Name" ItemStyle-Width="60%" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Literal ID="Literal2" runat="server" Text='<%# getColorName(CType(Eval("ColorID"), Integer)) %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDeleteColor" runat="server" OnCommand="dtgPSelectedColors_Delete" CommandArgument='<%# Eval("ColorID") %>' CommandName="DeletColor" Text="" CssClass="icon-trash delete-icon" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                
                    <br class="clear-both" /><br />

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnAddColor" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="ColorsUpdatePanel">
                <ProgressTemplate>
                    <div class="loading-box">
                        <h4 style="text-align: center; position: relative; top: 32px;">Please wait while we store your selection...</h4>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <a href="#" class="prev-tab" rel="4">prev</a>
            <a href="#" class="next-tab" rel="6">next</a>
            <br class="clear-both" />
        </div>
        <div id="tab-sizes">
            <asp:UpdatePanel runat="server" ID="SizesUpdatePanel" RenderMode="Inline" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Conditional">
                <ContentTemplate>

                    <h3>Sizes (<asp:HyperLink ID="hlAddSize" runat="server" NavigateUrl="~/CCAdmin/Commerce/Sizes.aspx" Text="New" />)</h3>
                    <p>Add a size for this product.</p>
                
                    <div class="search-bar">
                        <asp:DropDownList runat="server" ID="ddlPAddSize" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ID" style="width: 680px; float: left; margin-left: 10px;">
                            <asp:ListItem Text="Size" Value="" />
                        </asp:DropDownList>
                        <asp:Button id="btnAddSize" runat="server" Text="Add" CssClass="SaveButton" />
                    </div>

                    <asp:DataGrid runat="server" 
                            ID="dtgPAddingSize" 
                            AutoGenerateColumns="false" 
                            GridLines="None" 
                            CssClass="default-table">
                        <HeaderStyle CssClass="default-table-header" />
                        <Columns>
                            <asp:TemplateColumn SortExpression="SizeID" HeaderText="Name" ItemStyle-Width="80%" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Literal runat="server" Text='<%# getSizeName(CType(Eval("SizeID"), Integer)) %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDeleteSize" runat="server" OnCommand="dtgPAddingSize_Delete" CommandArgument='<%# Eval("SizeID") %>' CommandName="DeleteSize" Text="" CssClass="icon-trash delete-icon" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>

                    <asp:DataGrid runat="server" 
                            ID="dtgSelectedSizes" 
                            AutoGenerateColumns="false" 
                            GridLines="None" 
                            CssClass="default-table">
                        <HeaderStyle CssClass="default-table-header" />
                        <Columns>
                            <asp:BoundColumn Visible="false" DataField="ID" />
                            <asp:TemplateColumn SortExpression="SizeID" HeaderText="Name" ItemStyle-Width="80%" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Literal runat="server" Text='<%# Eval("Name") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDeleteSize" runat="server" OnCommand="dtgPSelectedSizes_Delete" CommandArgument='<%# Eval("ID") %>' CommandName="DeleteSize" Text="" CssClass="icon-trash delete-icon" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
        
                    <br class="clear-both" /><br />

                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="SizesUpdatePanel">
                <ProgressTemplate>
                    <div class="loading-box">
                        <h4 style="text-align: center; position: relative; top: 32px;">Please wait while we store your selection...</h4>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <a href="#" class="prev-tab" rel="5">prev</a>
            <a href="#" class="next-tab" rel="7">next</a>
            <br class="clear-both" />
        </div>
        <div id="tab-recomendations">
            <asp:UpdatePanel runat="server" ID="RecomendationUpdatePanel" RenderMode="Inline" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Conditional">
                <ContentTemplate>

                    <h3>Recomended Products</h3>
                    <p>Add products to display as recomendations when purchasing this product</p>
                    
                    <div class="search-bar">
                        <asp:DropDownList runat="server" ID="RecomendedProductDropDown" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ID" style="width: 680px; float: left; margin-left: 10px;">
                            <asp:ListItem Text="Recomendation" Value="" />
                        </asp:DropDownList>
                        <asp:Button id="AddRecomendedProductButton" runat="server" Text="Add" CssClass="SaveButton" />
                    </div>

                    <asp:DataGrid runat="server" 
                            ID="AddingRecomendedProductDataGrid" 
                            AutoGenerateColumns="false" 
                            GridLines="None" 
                            CssClass="default-table">
                        <HeaderStyle CssClass="default-table-header" />
                        <Columns>
                            <asp:TemplateColumn SortExpression="RecomendedProductId" HeaderText="Name" ItemStyle-Width="80%" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Literal runat="server" Text='<%# Eval("Product1.Name")%>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="DeleteRecomendedProductButton" runat="server" OnCommand="AddingRecomendedProductDataGridDelete" CommandArgument='<%# Eval("RecomendedProductId")%>' CommandName="DeleteRecomendation" Text="" CssClass="icon-trash delete-icon" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    
                    <asp:DataGrid runat="server" 
                            ID="SelectingRecomendedProductDataGrid" 
                            AutoGenerateColumns="false" 
                            GridLines="None" 
                            CssClass="default-table">
                        <HeaderStyle CssClass="default-table-header" />
                        <Columns>
                            <asp:TemplateColumn SortExpression="RecomendedProductId" HeaderText="Name" ItemStyle-Width="80%" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Literal runat="server" Text='<%# Eval("Product1.Name")%>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="DeleteRecomendedProductButton" runat="server" OnCommand="SelectingRecomendedProductDataGridDelete" CommandArgument='<%# Eval("RecomendedProductId")%>' CommandName="DeleteRecomendation" Text="" CssClass="icon-trash delete-icon" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    
                    <br class="clear-both" /><br />

                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="RecomendationUpdatePanel">
                <ProgressTemplate>
                    <div class="loading-box">
                        <h4 style="text-align: center; position: relative; top: 32px;">Please wait while we store your selection...</h4>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <a href="#" class="prev-tab" rel="6">prev</a>
            <a href="#" class="next-tab" rel="8">next</a>
            <br class="clear-both" />
        </div>
        <div id="tab-seo">
            <label>Browser Title</label>
            <asp:TextBox runat="server" ID="txtBrowserTitle" ToolTip="Browser Title" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <label>Permalink</label>
            <asp:TextBox runat="server" ID="txtPermalink" ToolTip="Permalink" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label>Keywords</label>
            <asp:TextBox runat="server" ID="txtPKeywords" TextMode="MultiLine" Rows="2" ToolTip="Product Keywords" CssClass="form-text-area" />
            <br class="clear-both" /><br />
        
            <label>Description</label>
            <asp:TextBox runat="server" ID="txtPDescription" TextMode="MultiLine" Rows="6" ToolTip="Product Description" CssClass="form-text-area" />
            <br class="clear-both" /><br />      
        
            <label>Image Alt Tag</label>
            <asp:TextBox runat="server" ID="txtAlternateTag" ToolTip="Image Alt Tag" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <a href="#" class="prev-tab" rel="7">prev</a>
            <br class="clear-both" />
        </div>
    </div>

    <!-- TODO This needs to become a recuring billing setting -->
    <asp:DropDownList runat="server" ID="ddlIsMembership" Width="95%" Visible="false">
        <asp:ListItem Text="No" Value="False" Selected="True" />
        <asp:ListItem Text="Yes" Value="True" />
    </asp:DropDownList>


    <asp:UpdatePanel runat="server" ID="upColorsSizes" RenderMode="Inline" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Conditional">
        <ContentTemplate>
            <!--TODO Implement the Product Availability feature -->
            <table runat="server" id="tblProductAvailability" visible="false" cellpadding="0" cellspacing="0" class="default-table" style="float: left; width: 957px; margin-left: 0px; border-color: #DDD; background-color: #F5F5F5; margin-top: 20px;">
                <tr>
                    <td colspan="2"><h3 style="margin: 10px;">Restrict Product Availability</h3></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:DropDownList runat="server" ID="ddlProductAvailability" ToolTip="Restrict Product Availability" style="width: 460px; margin-left: 10px;">
                            <asp:ListItem Text="Yes" Value="True" />
                            <asp:ListItem Text="No" Value="False" Selected="True" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox runat="server" ID="txtAreaPrefixLow" style="width: 242px; margin-bottom: 6px; float: left; margin-left: 10px;" />
                        <asp:TextBox runat="server" ID="txtAreaPrefixHigh" style="width: 242px; margin-bottom: 6px; float: left; margin-left: 10px;" />
                        <asp:TextBox runat="server" ID="txtAreaDescription" style="width: 242px; margin-bottom: 6px; float: left; margin-left: 10px;" />
                        <asp:LinkButton id="btnAddArea" runat="server" CssClass="GreenButton" style="margin-top: 2px; margin-left: 12px; float: left; width: 100px;"><span class="GreenButton">Add</span></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-left: 4px; padding-bottom: 5px;">
                        <asp:DataGrid runat="server" ID="dgAvailableAreas" AutoGenerateColumns="false" Width="99%" CellPadding="0" CellSpacing="0" GridLines="None" CssClass="ProductDisplayTable">
                            <Columns>
                                <asp:BoundColumn DataField="PrefixLow" SortExpression="PrefixLow" HeaderText="Prefix Low" ItemStyle-VerticalAlign="Middle" />
                                <asp:BoundColumn DataField="PrefixHigh" SortExpression="PrefixHigh" HeaderText="Prefix High" ItemStyle-VerticalAlign="Middle" />
                                <asp:BoundColumn DataField="Description" SortExpression="Description" HeaderText="Description" ItemStyle-VerticalAlign="Middle" />
                                <asp:TemplateColumn ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnDeleteArea" runat="server" OnCommand="dgAvailableAreas_Delete" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="Delete" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
        
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upColorsSizes">
        <ProgressTemplate>
            <div class="loading-box">
                <h4>Please wait while we store your selection...</h4>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress> 

    <br class="clear-both" />

    <asp:Button id="btnSavePBottom" runat="server" CssClass="SaveButton" CausesValidation="true" ValidationGroup="product" TabIndex="34" Text="Save" Visible="false" />
    
    <asp:RequiredFieldValidator runat="server" ID="rfvPName" ControlToValidate="ProductNameTextBox" ErrorMessage="Please enter a name" Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePName" TargetControlID="rfvPName" PopupPosition="TopRight" />

    <asp:RequiredFieldValidator runat="server" ID="rfvPermalink" ControlToValidate="txtPermalink" ErrorMessage="Please enter a permalink" Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePermalink" TargetControlID="rfvPermalink" PopupPosition="TopRight" />

    <asp:RequiredFieldValidator runat="server" ID="rfvPBrand" ControlToValidate="ddlPBrand" ErrorMessage="Please select a brand" Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePBrand" TargetControlID="rfvPBrand" PopupPosition="TopRight" />

    <asp:RequiredFieldValidator runat="server" ID="rfvTopSeller" ControlToValidate="ddlTopSeller" ErrorMessage="Please select if this is a top selling item." Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceTopSeller" TargetControlID="rfvTopSeller" PopupPosition="TopRight" />

    <asp:RequiredFieldValidator runat="server" ID="rfvClearance" ControlToValidate="ddlClearance" ErrorMessage="Please select if this is a clearance item." Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceClearance" TargetControlID="rfvClearance" PopupPosition="TopRight" />

    <asp:RequiredFieldValidator runat="server" ID="rfvIsDigitalMedia" ControlToValidate="ddlIsDigitalMedia" ErrorMessage="Please select if this is a digital media item." Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceIsDigitalMedia" TargetControlID="rfvIsDigitalMedia" PopupPosition="TopRight" />

    <asp:RequiredFieldValidator runat="server" ID="rfvTrackInventory" ControlToValidate="ddlTrackInventory" ErrorMessage="Please select if you want to track inventory." Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceTrackInventory" TargetControlID="rfvTrackInventory" PopupPosition="TopRight" />

    <asp:RequiredFieldValidator runat="server" ID="rfvPCategory" ControlToValidate="ddlPCategory" ErrorMessage="Please select a category" Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePCategory" TargetControlID="rfvPCategory" PopupPosition="TopRight" />

    <asp:RequiredFieldValidator runat="server" ID="rfvPDescription" ControlToValidate="txtPDescription" ErrorMessage="Please leave a description." Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePDescription" TargetControlID="rfvPDescription" PopupPosition="TopRight" />

    <ajax:TextBoxWatermarkExtender ID="tbeCost" runat="server" TargetControlID="txtPCost" WatermarkText="0.00" WatermarkCssClass="Watermark" />
    <asp:RequiredFieldValidator runat="server" ID="rfvPCost" ControlToValidate="txtPCost" ErrorMessage="Please enter a cost in decimal format (9.99)." Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePCost" TargetControlID="rfvPCost" PopupPosition="TopRight" />

    <ajax:TextBoxWatermarkExtender ID="tbePrice" runat="server" TargetControlID="txtPPrice" WatermarkText="0.00" WatermarkCssClass="Watermark" />
    <asp:RequiredFieldValidator runat="server" ID="rfvPPrice" ControlToValidate="txtPPrice" ErrorMessage="Please enter a price in decimal format (9.99)." Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePPrice" TargetControlID="rfvPPrice" PopupPosition="TopRight" />

    <ajax:TextBoxWatermarkExtender ID="tbeWarehousePrice" runat="server" TargetControlID="txtPWarehousePrice" WatermarkText="0.00" WatermarkCssClass="Watermark" />
    <ajax:TextBoxWatermarkExtender ID="tbeAssociatePrice" runat="server" TargetControlID="txtPAssociatePrice" WatermarkText="0.00" WatermarkCssClass="Watermark" />

    <ajax:TextBoxWatermarkExtender ID="tbe04" runat="server" TargetControlID="txtPWeight" WatermarkText="0.0" WatermarkCssClass="Watermark" />
    <asp:RequiredFieldValidator runat="server" ID="rfvPWeight" ControlToValidate="txtPWeight" ErrorMessage="Please enter a weight in decimal format (9.99)." Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePWeight" TargetControlID="rfvPWeight" PopupPosition="TopRight" />

    <ajax:TextBoxWatermarkExtender ID="tbe05" runat="server" TargetControlID="txtPLength" WatermarkText="0.0" WatermarkCssClass="Watermark" />
    <asp:RequiredFieldValidator runat="server" ID="rfvPLength" ControlToValidate="txtPLength" ErrorMessage="Please enter a length in decimal format (9.99)." Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePLength" TargetControlID="rfvPLength" PopupPosition="TopRight" />

    <ajax:TextBoxWatermarkExtender ID="tbe06" runat="server" TargetControlID="txtPWidth" WatermarkText="0.0" WatermarkCssClass="Watermark" />
    <asp:RequiredFieldValidator runat="server" ID="rfvPWidth" ControlToValidate="txtPWidth" ErrorMessage="Please enter a width in decimal format (9.99)." Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePWidth" TargetControlID="rfvPWidth" PopupPosition="TopRight" />

    <ajax:TextBoxWatermarkExtender ID="tbe07" runat="server" TargetControlID="txtPHeight" WatermarkText="0.0" WatermarkCssClass="Watermark" />
    <asp:RequiredFieldValidator runat="server" ID="rfvPHeight" ControlToValidate="txtPHeight" ErrorMessage="Please enter a height in decimal format (9.99)." Display="None" ValidationGroup="product" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vcePHeight" TargetControlID="rfvPHeight" PopupPosition="TopRight" />

</asp:Content>