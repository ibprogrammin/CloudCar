<%@ Page Title="File Upload" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="UploadFiles.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.UploadFiles" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        File Upload
        <i class="icon-cloud-upload"></i>
    </h1><hr />

    <asp:Label runat="server" ID="lblStatus" Text="" Visible="false" CssClass="status-message" />
    <asp:HiddenField runat="server" ID="hfFileID" Value="" />
           
    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
	    </ul>
        <div id="tab-details">
            
            <label>Title</label>
            <asp:TextBox runat="server" ID="txtTitle" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <label>Description</label>
            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="3" CssClass="form-text-area" />
            <br class="clear-both" /><br />
            
            <label style="min-height: 130px;">File</label>
            <asp:HiddenField ID="hfFile" runat="server" Value="" />
            <asp:HiddenField ID="hfPath" runat="server" Value="" />
            <asp:Label runat="server" ID="lblFilename" Visible="false" CssClass="display-message" />
            <div class="form-file-upload-display">
                <div class="form-fake-upload">
	                <input type="text" name="imagefilename" readonly="readonly" />
                </div>
                <asp:FileUpload runat="server" ID="fuFile" ToolTip="Select an image for this testimonial" size="20" CssClass="form-real-upload" onchange="this.form.imagefilename.value = this.value;" /><br />
            </div>
            Please keep file size under 5 MB.
            <br class="clear-both" />
            
            <label>Enabled</label>
            <asp:CheckBox runat="server" ID="ckbEnabled" Checked="true" Text="" CssClass="form-check-box" />
            <br class="clear-both" /><br />
         </div>   
    </div>
        
    <br class="clear-both" />
                
    <asp:Button ID="btnAdd" Text="Save" CssClass="SaveButton" runat="server" CausesValidation="true" ValidationGroup="File" />
    <asp:Button ID="btnCancel" Text="Clear" CssClass="DeleteButton" runat="server" />
                
    <asp:RequiredFieldValidator runat="server" ID="rfvTitle" ControlToValidate="txtTitle" Text="Please enter a title" Display="None" ValidationGroup="File" />
    <ajax:ValidatorCalloutExtender runat="server" id="vceTitle" TargetControlID="rfvTitle" PopupPosition="BottomLeft" />
                
    <br class="clear-both" /><hr />

    <asp:DataGrid runat="server" ID="dgUploadedFiles" DataKeyField="id" DataKeyNames="id" AllowPaging="true" AllowSorting="True" AutoGenerateColumns="False" 
            PageSize="10" GridLines="None" CssClass="default-table" CellPadding="0" CellSpacing="0">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="File">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnSelect" OnCommand="btnSelect_Command" CommandArgument='<%# Eval("id") %>' CommandName="Select" Text='<%# Eval("Title") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn HeaderText="Description" DataField="Description" />
            <asp:TemplateColumn HeaderText="Path">
                <ItemTemplate>
                    <a href='<%# String.Format("{0}{1}", Eval("Path"), Eval("Filename")) %>'><asp:Literal runat="server" Text='<%# String.Format("{0}{1}", Eval("Path"), Eval("Filename")) %>' /></a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Enabled" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="ckbChecked" Checked='<%# Eval("Enabled") %>' Enabled="false" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-Width="6%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDelete" OnCommand="btnDelete_Command" CommandArgument='<%# Eval("id") %>' CommandName="Delete" Text="" CssClass="icon-trash delete-icon" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

</asp:Content>