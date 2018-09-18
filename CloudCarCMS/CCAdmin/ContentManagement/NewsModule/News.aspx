<%@ Page Language="vb" ValidateRequest="false" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="News.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.NewsModule.News" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        News
        <i class="icon-bullhorn"></i>
        <asp:Button id="AddNewsButton" runat="server" CausesValidation="true" CssClass="SaveButton heading-button-new" Text="New" />
    </h1><hr />
    
    <asp:Label ID="StatusMessageLabel" runat="server" Text="" CssClass="status-message" Visible="false" />

    <asp:PlaceHolder ID="AddNewsPlaceHolder" runat="server" Visible="false">
    
        <div class="tab-container">
	        <ul class="tabs">
		        <li class="tab"><a href="#tab-details">Details</a></li>
		        <li class="tab"><a href="#tab-content">Content</a></li>
	        </ul>
            <div id="tab-details">
                <label>Title</label>
                <asp:HiddenField ID="NewsIdHiddenField" runat="server" />
                <asp:TextBox ID="TitleTextBox" runat="server" CssClass="form-text-box" />
                <asp:RequiredFieldValidator ID="rfvT" runat="server" ControlToValidate="TitleTextBox" ErrorMessage="Please enter a title." SetFocusOnError="true" Display="None" ValidationGroup="EN" />
                <ajax:ValidatorCalloutExtender ID="vceT" runat="server" TargetControlID="rfvT" />
                <br class="clear-both" /><br />
            
                <label>Sub Title</label>
                <asp:TextBox ID="SubTitleTextBox" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
                <br class="clear-both" /><br />

                <label>Permalink</label>
                <asp:TextBox ID="PermalinkTextBox" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="PermalinkTextBox" ErrorMessage="Please set a permalink" SetFocusOnError="true" Display="None" ValidationGroup="EN" />
                <br class="clear-both" /><br />

                <label>Publish Date</label>
                <asp:TextBox runat="server" ID="PublishDateTextBox" TextMode="SingleLine" CssClass="form-text-box" /><br />
                <ajax:CalendarExtender ID="ceDateCalendar" TargetControlID="PublishDateTextBox" runat="server" PopupPosition="BottomLeft" />
                <br class="clear-both" /><br />
            
                <label>Summary</label>
                <asp:TextBox ID="SummaryTextBox" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-text-area" />
                <br class="clear-both" /><br />
                
                <label style="min-height: 100px;">Image</label>
                <asp:HiddenField runat="server" ID="ImageIdHiddenField" />
                <asp:Label runat="server" ID="ImageLocationLabel" Visible="false" CssClass="display-message" />
                <div class="form-file-upload-display">
	                <div class="form-fake-upload">
		                <input type="text" name="imagefilename" readonly="readonly" />
	                </div>
                    <asp:FileUpload runat="server" ID="ImageFileUpload" ToolTip="Select the image for news or event" size="20" CssClass="form-real-upload" onchange="this.form.imagefilename.value = this.value;" /><br />
                </div>
                <br class="clear-both" />
            </div>
            <div id="tab-content">
                <textarea runat="server" id="DetailsTextArea" class="ck-editor" />
                <asp:RequiredFieldValidator ID="rfvD" runat="server" ControlToValidate="DetailsTextArea" ErrorMessage="Please enter some details." SetFocusOnError="true" Display="None" ValidationGroup="EN" />
                <ajax:ValidatorCalloutExtender ID="vceD" runat="server" TargetControlID="rfvD" />
            </div>
        </div>

        <asp:Button runat="server" ID="SubmitButton" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="SaveButton" />
        <asp:Button runat="server" ID="CancelButton" Text="Cancel" CausesValidation="false" CssClass="DeleteButton" />

        <br class="clear-both" /><br /><hr />

    </asp:PlaceHolder>
    
    <asp:GridView runat="server" 
            ID="NewsGridView" 
            AllowSorting="true" 
            AutoGenerateColumns="False" 
            DataSourceID="NewsDataSource" 
            DataKeyNames="Id" 
            GridLines="None" 
            CssClass="default-table" 
            CellPadding="0" 
            CellSpacing="0">
        <PagerStyle HorizontalAlign="Right" CssClass="default-table-pager" />
        <PagerSettings PageButtonCount="20" Mode="NumericFirstLast" Position="Bottom" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateField HeaderText="Title" SortExpression="Title">
                <ItemTemplate>
                    <asp:LinkButton ID="SelectButton" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="Select" Text='<%# Eval("Title") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PublishDate" HeaderText="Publish Date" SortExpression="PublishDate" DataFormatString="{0:F}" />
            <asp:BoundField DataField="SubmitDate" HeaderText="Submit Date" SortExpression="SubmitDate" DataFormatString="{0:D}" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-Width="6%">
                <ItemTemplate>
                    <asp:LinkButton ID="DeleteButton" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" Text="" CssClass="icon-trash delete-icon" />
                    <asp:LinkButton ID="ApproveButton" runat="server" CommandArgument='<%# Eval("Id") %>' commandname="Approve" Text="Approve" Visible='<%# NOT CBool(Eval("Approved")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <asp:SqlDataSource ID="NewsDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:MainConnectionString %>"
        SelectCommand="SELECT [Id], [Title], [PublishDate], [SubmitDate], [Details], [ImageId], [Approved] FROM [News] ORDER BY [PublishDate]" />

</asp:Content>