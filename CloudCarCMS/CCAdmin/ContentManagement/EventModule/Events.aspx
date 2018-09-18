<%@ Page Title="Events" Language="vb" ValidateRequest="False" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Events.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.EventModule.Events" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Events
        <i class="icon-calendar"></i>
        <asp:Button id="AddEventButton" runat="server" CausesValidation="true" CssClass="SaveButton heading-button-new" Text="New" />
    </h1><hr />
    
    <asp:Label ID="StatusMessageLabel" runat="server" Text="" CssClass="status-message" Visible="false" />

    <asp:PlaceHolder ID="AddEventPlaceHolder" runat="server" Visible="false">
    
        <asp:HiddenField ID="EventIdHiddenField" runat="server" />
        
        <div class="tab-container">
	        <ul class="tabs">
		        <li class="tab"><a href="#tab-details">Details</a></li>
		        <li class="tab"><a href="#tab-content">Content</a></li>
	        </ul>
            <div id="tab-details">
                <label>Title</label>
                <asp:TextBox ID="TitleTextBox" runat="server" CssClass="form-text-box" />
                <asp:RequiredFieldValidator ID="rfvT" runat="server" ControlToValidate="TitleTextBox" ErrorMessage="Please enter a title." SetFocusOnError="true" Display="None" ValidationGroup="EN" />
                <ajax:ValidatorCalloutExtender ID="vceT" runat="server" TargetControlID="rfvT" />
                <br class="clear-both" /><br />
            
                <label>Permalink</label>
                <asp:TextBox ID="PermalinkTextBox" runat="server" TextMode="SingleLine" CssClass="form-text-box" />
                <br class="clear-both" /><br />

                <label>Date</label>
                <asp:TextBox runat="server" ID="EventDateTextBox" TextMode="SingleLine" CssClass="form-text-box" /><br />
                <ajax:CalendarExtender ID="ceDateCalendar" TargetControlID="EventDateTextBox" runat="server" PopupPosition="BottomLeft" />
                <br class="clear-both" /><br />
            
                <label>Time</label>
                <asp:TextBox ID="TimeTextBox" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-text-box" />
                <br class="clear-both" /><br />
            
                <label>Location</label>
                <asp:TextBox ID="LocationTextBox" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-text-box" />
                <br class="clear-both" /><br />
                
                <label style="min-height: 100px;">Image</label>
                <asp:HiddenField runat="server" ID="ImageIdHiddenField" />
                <asp:Label runat="server" ID="ImageLocationLabel" Visible="false" CssClass="display-message" style="margin-bottom: 10px;" />
                <div class="form-file-upload-display">
	                <div class="form-fake-upload">
		                <input type="text" name="imagefilename" readonly="readonly" />
	                </div>
                    <asp:FileUpload runat="server" ID="ImageFileUpload" ToolTip="Select the image for news or event" size="20" CssClass="form-real-upload" onchange="this.form.imagefilename.value = this.value;" /><br />
                </div>
                <br class="clear-both" /><br />
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
            ID="EventsGridView" 
            AllowSorting="true" 
            AutoGenerateColumns="False" 
            DataSourceID="EventDataSource" 
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
            <asp:BoundField DataField="DateAdded" HeaderText="Date Added" SortExpression="DateAdded" DataFormatString="{0:F}" />
            <asp:BoundField DataField="EventDate" HeaderText="Event Date" SortExpression="EventDate" DataFormatString="{0:D}" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-Width="6%">
                <ItemTemplate>
                    <asp:LinkButton ID="DeleteButton" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" Text="" cssclass="icon-trash delete-icon" />
                    <asp:LinkButton ID="ApproveButton" runat="server" CommandArgument='<%# Eval("Id") %>' commandname="Approve" Text="Approve" Visible='<%# NOT CBool(Eval("Approved")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <asp:SqlDataSource ID="EventDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:MainConnectionString %>"
        SelectCommand="SELECT [Id], [Title], [ImageId], [DateAdded], [EventDate], [Approved] FROM [Events] ORDER BY [DateAdded]" />

</asp:Content>