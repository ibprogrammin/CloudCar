<%@ Page Title="Links" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Links.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.Links" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Links
        <i class="icon-link"></i>
        <asp:Button id="btnAddLink" runat="server" CssClass="SaveButton heading-button-new" CausesValidation="true" Text="New" />
    </h1><hr />
    
    <asp:Label ID="lblStatus" runat="server" CssClass="status-message" Visible="false" />
    <asp:PlaceHolder ID="phAddLink" runat="server" Visible="false">
    
    <asp:HiddenField ID="hfID" runat="server" />

    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
            <li class="tab"><a href="#tab-content">Content</a></li>
	    </ul>
        <div id="tab-details">
            <img runat="server" ID="imgLinkImage" alt="Image Preview" visible="false" src="" class="image-display" style="margin-left: 10px; margin-bottom: 30px;" />
            
            <label>Title</label>
            <asp:TextBox ID="txtTitle" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <label>URL</label>
            <asp:TextBox ID="txtURL" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
            <br class="clear-both" /><br />

            <label style="min-height: 100px;">Image</label>
            <asp:HiddenField runat="server" ID="hfPictureID" />
            <asp:Label runat="server" ID="lblFileName" Visible="false" CssClass="display-message" />
            <div class="form-file-upload-display">
	            <div class="form-fake-upload">
		            <input type="text" name="imagefilename" readonly="readonly" />
	            </div>
                <asp:FileUpload runat="server" ID="fuPicture" ToolTip="Select the image for this link" size="20" CssClass="form-real-upload" onchange="this.form.imagefilename.value = this.value;" /><br />
            </div>
            <br class="clear-both" />
            
        </div>
        <div id="tab-content">
            <textarea runat="server" id="DescriptionTextArea" class="ck-editor" />
        </div>
    </div>
        
    <br class="clear-both" />
        
    <asp:Button ID="btnAdd" Text="Save" CssClass="SaveButton" runat="server" CausesValidation="true" ValidationGroup="File" />
    <asp:Button ID="btnCancel" Text="Clear" CssClass="DeleteButton" runat="server" />
        
    <asp:RequiredFieldValidator ID="rfvT" runat="server" ControlToValidate="txtTitle" ErrorMessage="Please add a Title." SetFocusOnError="true" Display="None" ValidationGroup="Link" />
    <ajax:ValidatorCalloutExtender ID="vceT" runat="server" TargetControlID="rfvT" />

    <asp:RequiredFieldValidator ID="rfvURL" runat="server" ControlToValidate="txtURL" ErrorMessage="Please Enter a URL." SetFocusOnError="true" Display="None" ValidationGroup="Link" />
    <ajax:ValidatorCalloutExtender ID="vceURL" runat="server" TargetControlID="rfvURL" />
        
    <br class="clear-both" /><hr />
        
    </asp:PlaceHolder>
    
    <asp:GridView runat="server" 
            ID="gvLinks" 
            AutoGenerateColumns="False" 
            DataSourceID="dsLinks" 
            DataKeyNames="LinksID" 
            AllowSorting="true" 
            AllowPaging="true" 
            CssClass="default-table">
        <PagerStyle HorizontalAlign="Right" CssClass="default-table-pager" />
        <PagerSettings PageButtonCount="20" Mode="NumericFirstLast" Position="Bottom" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateField HeaderText="Title" SortExpression="LinksTitle">
                <ItemTemplate>
                    <asp:LinkButton ID="lbSelect" runat="server" CommandArgument='<%# Eval("LinksID") %>' CommandName="SelectLink" Text='<%# Eval("LinksTitle") %>' /><br />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="LinkURL" HeaderText="URL" SortExpression="LinkURL" />
            <asp:BoundField DataField="LinkNotes" HeaderText="Description" SortExpression="LinkNotes" ItemStyle-Width="325px" Visible="false" />
            <asp:TemplateField SortExpression="LinksID">
                <ItemTemplate>
                    <asp:LinkButton ID="lbDelete" runat="server" CommandArgument='<%# Eval("LinksID") %>' CommandName="DeleteLink" Text="Delete" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <asp:SqlDataSource ID="dsLinks" runat="server" ConnectionString="<%$ ConnectionStrings:MainConnectionString %>" SelectCommand="SELECT [LinksID], [LinksTitle], [LinkURL], [LinkNotes], [PictureID] FROM [Link]" />

</asp:Content>