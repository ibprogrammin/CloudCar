<%@ Page Title="" Language="vb" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Properties.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.PropertyModule.Properties" %>
<%@ Register TagPrefix="SM" Namespace="CloudCar.CCControls.Validators" Assembly="CloudCarFramework" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <asp:Button runat="server" ID="btnNew" Text="New" CausesValidation="false" CssClass="SaveButton" style="float: right;" TabIndex="18" />
    <h1 class="form-heading-style">Properties</h1><br class="clear-both" />

    <asp:ListBox runat="server" ID="lbProperties" Visible="false" DataValueField="Id" DataTextField="Title" AutoPostBack="true" style="width: 780px; height: 97px; float: left;" Rows="3" />

    <hr /><br />
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" /><br style="clear: both;" />

    <asp:PlaceHolder runat="server" ID="phPropertyList" Visible="false">
        
        <asp:DataGrid ID="dgProperties" runat="server" AutoGenerateColumns="False" DataKeyNames="id" PageSize="10" 
            AllowPaging="true" AllowSorting="True" GridLines="None" CssClass="default-table" CellPadding="0" CellSpacing="0">
            <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
            <HeaderStyle CssClass="default-table-header" />
            <Columns>
                <asp:TemplateColumn HeaderText="Property" SortExpression="Title">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" Text='<%# Eval("Title") %>' OnCommand="dgProperties_Select" CommandArgument='<%# Eval("Id") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="ListingId" HeaderText="Listing" SortExpression="ListingId" />
                <asp:TemplateColumn HeaderText="City" SortExpression="Address.City">
                    <ItemTemplate>
                        <asp:Literal runat="server" Text='<%# String.Format("{0}, {1}, {2}", Eval("Address.Address"), Eval("Address.City"), Eval("Address.Province.Name"))%>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Active" SortExpression="Active">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" Checked='<%# Eval("Active") %>' OnCheckedChanged="SetPropertyActive" AutoPostBack="True" PropertyId='<%#Eval("Id") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Vacancy" SortExpression="Vacancy">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" Checked='<%# Eval("Vacancy") %>' OnCheckedChanged="SetPropertyVacant" AutoPostBack="True" PropertyId='<%#Eval("Id") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="Price" DataFormatString="{0:c}" SortExpression="Price" HeaderText="Price" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="Price" />
            </Columns>
        </asp:DataGrid>
        
        <br class="clear-both" /><br /><br /><br />
            
    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="phPropertyDetails" Visible="false">
    
        <asp:LinkButton runat="server" ID="lbShowList" Text="&laquo; Return to List" style="margin-left: 20px;" TabIndex="19" /><br /><br />

        <fieldset>
            <h2 class="form-heading-style">Content</h2><br /><br />

            <asp:HiddenField runat="server" ID="hfPropertyId" />
                   
            <asp:Literal ID="litPropertyLink" runat="server" Visible="false" />
            
            <label for="txtTitle">Title</label>
            <asp:TextBox runat="server" ID="txtTitle" style="width: 650px;" TabIndex="20" />
            <br class="clear-both" /><br />
        
            <label for="txtListingNumber">Listing Number</label>
            <asp:TextBox runat="server" ID="txtListingNumber" style="width: 650px;" TabIndex="21" />
            <br class="clear-both" /><br />
            
            <label for="txtDetails">Details</label><br class="clear-both" />
            <textarea runat="server" id="DetailsTextArea" class="ck-editor" />
            <br class="clear-both" /><br />
            
            <label for="txtTestimonial">Testimonial</label><br class="clear-both" />
            <textarea runat="server" id="TestimonialTextArea" class="ck-editor" />
            <br class="clear-both" /><br />
            
        </fieldset>
        
        <br class="clear-both" /><br />
        
        <fieldset>
            <h2 class="form-heading-style">Address</h2><br /><br />
            
            <asp:HiddenField runat="server" ID="hfAddressId" />
            
            <label for="txtAddress">Address</label>
            <asp:TextBox runat="server" ID="txtAddress" style="width: 650px;" TabIndex="24" />
            <br class="clear-both" /><br />

            <label for="txtCity">City</label>
            <asp:TextBox runat="server" ID="txtCity" style="width: 650px;" TabIndex="25" />
            <br class="clear-both" /><br />
            
            <label for="ddlCountry">Country</label>
            <asp:DropDownList runat="server" ID="ddlCountry" DataValueField="Id" DataTextField="Name" style="width: 670px;" TabIndex="26" AutoPostBack="true" />
            <br class="clear-both" /><br />
            
            <label for="ddlProvince">Province</label>
            <asp:DropDownList runat="server" ID="ddlProvince" DataValueField="Id" DataTextField="Name" style="width: 670px;" TabIndex="27" />
            <br class="clear-both" /><br />
            
            <label for="txtPostalCode">Postal Code</label>
            <asp:TextBox runat="server" ID="txtPostalCode" style="width: 650px;" TabIndex="28" />
            <br class="clear-both" /><br />
            
        </fieldset>
        
        <br class="clear-both" /><br />
        
        <fieldset>
        
            <h2 class="form-heading-style">SEO Content</h2><br /><br />
            
            <label for="txtBrowserTitle">Browser Title</label>
            <asp:TextBox runat="server" ID="txtBrowserTitle" style="width: 650px;" TabIndex="29" />
            <br class="clear-both" /><br />
        
            <asp:HiddenField runat="server" ID="hfPermalink" Value="" />
            <label for="txtPermalink">Permalink</label>
            <asp:TextBox runat="server" ID="txtPermalink" style="width: 650px;" TabIndex="30" />
            <br class="clear-both" /><br />
            
            <label for="txtKeywords">Keywords</label>
            <asp:TextBox runat="server" ID="txtKeywords" style="width: 650px;" TabIndex="31" />
            <br class="clear-both" /><br />
            
            <label for="txtDescription">Description</label>
            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="4" style="width: 650px;" TabIndex="32" />
            <br class="clear-both" /><br />
            
        </fieldset>
        
        <br class="clear-both" /><br />
        
        <fieldset>
            <h2 class="form-heading-style">Details</h2><br /><br />
            
            <label for="txtPrice">Price</label>
            <asp:TextBox runat="server" ID="txtPrice" style="width: 350px;" TabIndex="33" />
            <br style="clear: both;" /><br />
            
            <label for="txtBedrooms">Bedrooms</label>
            <asp:TextBox runat="server" ID="txtBedrooms" style="width: 350px;" TabIndex="34" />
            <br class="clear-both" /><br />
            
            <label for="txtBathrooms">Bathrooms</label>
            <asp:TextBox runat="server" ID="txtBathrooms" style="width: 350px;" TabIndex="35" />
            <br class="clear-both" /><br />
            
            <label for="txtListingLink">Listing Link</label>
            <asp:TextBox runat="server" ID="txtListingLink" style="width: 350px;" TabIndex="36" />
            <br class="clear-both" /><br />
            
            <label for="ckbActive">Active</label>
            <asp:CheckBox runat="server" id="ckbActive" TabIndex="37" />
            <br class="clear-both" /><br />
            
            <label for="ckbVacancy">Vacancies</label>
            <asp:CheckBox runat="server" id="ckbVacancy" TabIndex="38" />
            <br class="clear-both" /><br />
            
        </fieldset>
            
        <br class="clear-both" />
        
        <asp:PlaceHolder runat="server" ID="phAddtionalDetails" Visible="false">
            
        <fieldset style="float: left; width: 560px; margin-top: 20px; margin-right: 20px;">
            
            <h3>Property Images</h3>
            <p>Add images to this property. <b>Dimensions: W (280px) x H (209px)</b></p>
            
            <asp:HiddenField ID="hfGalleryId" runat="server" /> 
            
            <div style="background-color: lemonchiffon; padding: 5px 0px;">
                <label>Gallery (<asp:HyperLink runat="server" NavigateUrl="~/CCAdmin/ContentManagement/ImageGalleries.aspx" Text="New" />)</label>
                <asp:DropDownList runat="server" ID="ImageGalleryDropDown" DataTextField="Title" DataValueField="Id" AutoPostBack="True" />
                <br class="clear-both" />
            </div>
            <br />
            
            <asp:Label runat="server" ID="lblImageMessage" Visible="false" />          
            
            <label>Title</label>
            <asp:TextBox ID="txtImageTitle" runat="server" TextMode="SingleLine" style="width: 250px;" TabIndex="39" />
            <br class="clear-both" /><br />
            
            <label>Description</label>
            <asp:TextBox ID="txtImageDescription" runat="server" TextMode="SingleLine" style="width: 250px;" TabIndex="40" />
            <br class="clear-both" /><br />
            
            <label>Order</label>
            <asp:TextBox ID="txtImageOrder" runat="server" TextMode="SingleLine" style="width: 200px; margin-right: 10px;" TabIndex="41" />
            <asp:Button runat="server" ID="btnOrderUp" Text="+" CssClass="form-up-down-button" />
            <asp:Button runat="server" ID="btnOrderDown" Text="-" CssClass="form-up-down-button" />
            <ajax:NumericUpDownExtender ID="nudOrder" runat="server" TargetControlID="txtImageOrder" Minimum="1" Maximum="6" TargetButtonUpID="btnOrderUp" TargetButtonDownID="btnOrderDown" />
            <br class="clear-both" />
            
            <label style="min-height: 100px;">
                <br />Image<br />
                <img runat="server" ID="imgImage" alt="Image Preview" visible="false" src="" class="image-display" />
            </label><br />
            <asp:HiddenField runat="server" ID="hfImageID" />
            <asp:Label runat="server" ID="lblImageLocation" Visible="false" CssClass="display-message" />
            <div class="form-file-upload-display-small">
	            <div class="form-fake-upload-small">
		            <input type="text" name="headerfilename" readonly="readonly" /> <!-- browse button is here as background -->
	            </div>
                <asp:FileUpload runat="server" ID="fuImage" ToolTip="Select the header image for this product." size="20" CssClass="form-real-upload" onchange="this.form.headerfilename.value = this.value;" TabIndex="42" /><br />
            </div>
            
            <asp:Button id="btnAddPropertyImage" runat="server" CssClass="SaveButton" Text="Add" CausesValidation="true" ValidationGroup="PropertyImage" style="margin-right: 40px; margin-top: 15px; float: right; width: 270px;" TabIndex="43" />
            
            <asp:RequiredFieldValidator ID="rfvImageTitle" runat="server" ControlToValidate="txtImageTitle" ErrorMessage="Please set a title for this property image" SetFocusOnError="true" Display="None" ValidationGroup="PropertyImage" />
            <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="rfvImageTitle" />
        
            <asp:RequiredFieldValidator ID="rfvImageOrder" runat="server" ControlToValidate="txtImageOrder" ErrorMessage="You should create a title for this property." SetFocusOnError="true" Display="None" ValidationGroup="PropertyImage" />
            <ajax:ValidatorCalloutExtender ID="vceImageOrder" runat="server" TargetControlID="rfvImageOrder" />
            <asp:RegularExpressionValidator ID="rfvImageOrderNumeric" runat="server" ControlToValidate="txtImageOrder" ErrorMessage="Please use only unsigned numbers when specifying an order" ValidationExpression="^\d+$" ValidationGroup="PropertyImage" />
            <ajax:ValidatorCalloutExtender ID="vceImageOrderNumeric" runat="server" TargetControlID="rfvImageOrderNumeric" />
            
            <asp:RequiredFieldValidator ID="rfvPropertyImage" runat="server" ControlToValidate="fuImage" ErrorMessage="Please select an image to use for this property" SetFocusOnError="true" Display="None" ValidationGroup="PropertyImage" />
            <ajax:ValidatorCalloutExtender ID="vcePropertyImage" runat="server" TargetControlID="rfvPropertyImage" />
            
            <br class="clear-both" />
            
            <asp:DataGrid runat="server" ID="dgPropertyImages" AutoGenerateColumns="false" Width="99%" CellPadding="0" CellSpacing="0" GridLines="None" CssClass="default-table">
                <Columns>
                    <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" HeaderText="Image">
                        <ItemTemplate>
                            <asp:Image runat="server" id="imgPropertyImage" ImageUrl='<%# String.Format("/images/db/{0}/60/{1}.jpg", Eval("ImageID"), CStr(Eval("Title")).Replace(" ", "")) %>' CssClass="image-display-table" /><br />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:BoundColumn HeaderText="Title" DataField="Title" />
                    <asp:BoundColumn HeaderText="Order" DataField="Order" />
                    <asp:TemplateColumn ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnRemoveImage" runat="server" OnCommand="dgPropertyImages_Remove" CommandArgument='<%# Eval("Id") %>' CommandName="RemoveImage" Text="Remove" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
            
            <br class="clear-both" /><br />
            
        </fieldset>
            
        <fieldset style="float: left; width: 360px; margin-top: 20px;">
            
            <h3>Features (<asp:HyperLink ID="hlAddFeature" runat="server" NavigateUrl="~/CCAdmin/ContentManagement/PropertyModule/Features.aspx" Text="New" />)</h3>
            <p>Add features to this property.</p>
            
            <asp:Label runat="server" ID="lblFeatureMessage" Visible="false" />
            
            <asp:DropDownList runat="server" ID="ddlFeatures" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ID" style="width: 140px; float: left; margin-left: 10px; margin-right: 10px;" TabIndex="44">
                <asp:ListItem Text="Feature" Value="" />
            </asp:DropDownList>
            
            <asp:Button id="btnAddFeature" runat="server" CssClass="SaveButton" Text="Add" style="margin-left: 10px; float: left; width: 170px;" TabIndex="45" />

            <asp:DataGrid runat="server" ID="dgPropertyFeatures" AutoGenerateColumns="false" Width="99%" CellPadding="0" CellSpacing="0" GridLines="None" CssClass="default-table">
                <Columns>
                    <asp:TemplateColumn SortExpression="FeatureId" HeaderText="Name" ItemStyle-Width="60%" ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <asp:Literal ID="litFeatureId" runat="server" Text='<%# getFeatureName(CInt(Eval("FeatureId"))) %>' />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnRemoveFeature" runat="server" OnCommand="dgPropertyFeatures_Remove" CommandArgument='<%# Eval("Id") %>' CommandName="RemoveFeature" Text="Remove" />
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
            
            <br class="clear-both" /><br />
            
        </fieldset>
                
        </asp:PlaceHolder>
        
        <br class="clear-both" /><br />
        
        <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="SaveButton" TabIndex="46" />
        <asp:Button runat="server" ID="btnDelete" Text="Delete" CausesValidation="false" CssClass="DeleteButton" TabIndex="47" />
        
        <br /><br />

        <asp:RequiredFieldValidator ID="rfvPT" runat="server" ControlToValidate="txtTitle" ErrorMessage="You should create a title for this property." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vcePT" runat="server" TargetControlID="rfvPT" />

        <asp:RequiredFieldValidator ID="rfvPL" runat="server" ControlToValidate="txtPermalink" ErrorMessage="You should create a permalink for this property." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vcePL" runat="server" TargetControlID="rfvPL" />
        <SM:PermalinkValidator ID="pvPermalink" runat="server" ControlToValidate="txtPermalink" PermalinkType="Properties" ErrorMessage="The permalink you have choosen is not unique." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vcePermalinkValidator" runat="server" TargetControlID="pvPermalink" />

        <asp:RequiredFieldValidator ID="rfvBedrooms" runat="server" ControlToValidate="txtBedrooms" ErrorMessage="Please set the number of bedrooms for this property" SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vceBedrooms" runat="server" TargetControlID="rfvBedrooms" />
        <asp:RegularExpressionValidator ID="rfvBedroomsNumeric" runat="server" ControlToValidate="txtBedrooms" ErrorMessage="Please use only numbers when specifying the amount of bedrooms" ValidationExpression="^\d+$" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vceBedroomsNumeric" runat="server" TargetControlID="rfvBedroomsNumeric" />
        
        <asp:RequiredFieldValidator ID="rfvBathrooms" runat="server" ControlToValidate="txtBathrooms" ErrorMessage="Please set the number of bathrooms for this property" SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vceBathrooms" runat="server" TargetControlID="rfvBathrooms" />
        <asp:RegularExpressionValidator ID="rfvBathroomsNumeric" runat="server" ControlToValidate="txtBathrooms" ErrorMessage="Please use only numbers when specifying the amount of bathrooms" ValidationExpression="^\d+$" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vceBathroomsNumeric" runat="server" TargetControlID="rfvBathroomsNumeric" />
        
        <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtPrice" ErrorMessage="Please set a price for this property" SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vcePrice" runat="server" TargetControlID="rfvPrice" />
        <asp:RegularExpressionValidator ID="rfvPriceNumeric" runat="server" ControlToValidate="txtPrice" ErrorMessage="Please use a numberical value when specifying the price of this property" ValidationExpression="^([0-9]*|\d*\.\d{1}?\d*)$" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vcePriceNumeric" runat="server" TargetControlID="rfvPriceNumeric" />

        <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress" Display="None" ErrorMessage="Please set an address for this property" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vceAddress" runat="server" TargetControlID="rfvAddress" />
        
        <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" Display="None" ErrorMessage="Please set a city for this property" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vceCity" runat="server" TargetControlID="rfvCity" />
        
        <asp:RequiredFieldValidator ID="rfvPostalCode" runat="server" ControlToValidate="txtPostalCode" Display="None" ErrorMessage="Please set a postal code for this property" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vcePostalCode" runat="server" TargetControlID="rfvPostalCode" PopupPosition="TopLeft" />
        
        <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ControlToValidate="ddlCountry" Display="None" ErrorMessage="Please select a country for this property" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vceCountry" runat="server" TargetControlID="rfvCountry" />
        
        <asp:RequiredFieldValidator ID="rfvProvince" runat="server" ControlToValidate="ddlProvince" Display="None" ErrorMessage="Please select a province for this property" ValidationGroup="ValidateForm" />
        <ajax:ValidatorCalloutExtender ID="vceProvince" runat="server" TargetControlID="rfvProvince" />

    </asp:PlaceHolder>

</asp:Content>