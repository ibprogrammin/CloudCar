<%@ Page Title="Programs" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="ProgramDetails.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.CalendarModule.ProgramDetails" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <img src="/CCTemplates/Admin/Images/icons/product.icon.png" alt="Edit your programs" width="75" height="75" class="HeadingIcon" /><br />
    <h1 class="form-heading-style">
        Edit Program ( <asp:Literal runat="server" ID="litProgramId" Text="New" />  )
    </h1><hr />

    <asp:HiddenField runat="server" ID="hfProgramId" />

    <asp:HyperLink ID="dlBackToPrograms" runat="server" NavigateUrl="~/CCAdmin/ContentManagement/CalendarModule/Programs.aspx" Text="&laquo; Back To Programs" />
    <br class="clear-both" /><br/>

    <asp:Label runat="server" ID="lblMessage" CssClass="status-message" Visible="False" />

    <fieldset>
        
        <h2 class="form-heading-style">Details</h2><br /><br />
        
        <asp:Literal ID="litProgramLink" runat="server" Visible="false" />
        
        <label>Name</label>
        <asp:TextBox runat="server" ID="txtName" ToolTip="Program Name" style="width: 650px;" />
        <br class="clear-both" /><br />
        
        <label>Content</label><br class="clear-both" />
        <textarea runat="server" id="ContentTextArea" class="ck-editor" />
        <br class="clear-both" /><br />
        
        
        <label style="min-height: 100px;">
            Icon Image<br />
            <img runat="server" ID="imgIconImage" alt="Icon Image Preview" visible="false" src="" class="image-display" />
        </label><br />
        <asp:HiddenField runat="server" ID="hfIconImage" />
        <asp:Label runat="server" ID="lblIconImageLocation" ReadOnly="true" Visible="false" CssClass="display-message" style="margin-bottom: 10px;" />
        <div class="form-file-upload-display">
	        <div class="form-fake-upload">
		        <input type="text" name="iconfilename" readonly="readonly" /> <!-- browse button is here as background -->
	        </div>
            <asp:FileUpload runat="server" ID="fuIconImageImage" ToolTip="Select the icon for this program." size="20" CssClass="form-real-upload" onchange="this.form.iconfilename.value = this.value;" /><br />
        </div><br />
        <span style="font-size: 12px; font-weight: bold;">Recomended Dimensions: W (80px) x H (120px)</span>
        <br class="clear-both" /><hr /><br />
        

	    <br class="clear-both" /><br />
        
    </fieldset>

    <br class="clear-both" /><br />
    
    <fieldset>
    
        <h2 class="form-heading-style">SEO</h2><br /><br />
        
        <label>Browser Title</label>
        <asp:TextBox runat="server" ID="txtBrowserTitle" ToolTip="Browser Title" style="width: 650px;" />
        <br class="clear-both" /><br />
            
        <label>Permalink</label>
        <asp:TextBox runat="server" ID="txtPermalink" ToolTip="Permalink" style="width: 650px;" />
        <br class="clear-both" /><br />
        
        <label>Keywords</label>
        <asp:TextBox runat="server" ID="txtKeywords" TextMode="MultiLine" Rows="2" ToolTip="Product Keywords" style="width: 650px;" />
        <br class="clear-both" /><br />
        
        <label>Description</label>
        <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="6" ToolTip="Product Description" style="width: 650px;" />
        <br class="clear-both" /><br />      

    </fieldset>

    <asp:PlaceHolder runat="server" ID="phProperties" Visible="False">

    <asp:UpdatePanel runat="server" ID="upProgramProperties" RenderMode="Inline" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Conditional">
        <ContentTemplate>
        
            <fieldset style="float: left; width: 458px; margin-top: 40px; margin-right: 20px;">

                <h3>Latest Program Schedule (<asp:HyperLink ID="hlAddSchedule" runat="server" NavigateUrl="~/CCAdmin/ContentManagement/CalendarModule/Schedules.aspx" Text="Add" />)</h3>
                <p></p>

                <asp:DataGrid runat="server" 
                        ID="dtgLatestSchedule" 
                        AutoGenerateColumns="false" 
                        Width="99%" 
                        CellPadding="0" 
                        CellSpacing="0" 
                        GridLines="None" 
                        CssClass="default-table">
                    <Columns>
                        <asp:BoundColumn DataField="BookingDate" SortExpression="Date" HeaderText="Date" DataFormatString="{0:MMMM dd, yyyy}" />
                        <asp:BoundColumn DataField="BookingDate" SortExpression="Time" HeaderText="Time" DataFormatString="{0:h:mm tt}" />
                        <asp:BoundColumn DataField="Capacity" SortExpression="Capacity" HeaderText="Capacity" />
                        <asp:BoundColumn DataField="Free" SortExpression="Free" HeaderText="Free" />
                    </Columns>
                </asp:DataGrid>

                <br class="clear-both" /><br />
        
            </fieldset>
        
            <fieldset style="float: left; width: 459px; margin-top: 40px;">
        
                <h3>Instructors (<asp:HyperLink ID="hlNewInstructor" runat="server" NavigateUrl="~/CCAdmin/ContentManagement/CalendarModule/InstructorDetails.aspx?instructor=new" Text="New" />)</h3>
                <p>Associate an instructor with this program</p>
                
                <asp:Label runat="server" ID="lblInstructorMessage" CssClass="status-message" Visible="false" style="width: 500px; margin-left: 10px;" />
                
                <asp:DropDownList runat="server" ID="ddlAddInstructor" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ID" style="width: 140px; float: left; margin-left: 10px; margin-right: 10px;">
                    <asp:ListItem Text="Please Select..." Value="" />
                </asp:DropDownList>

                <asp:Button id="btnAddInstructor" runat="server" CssClass="GreenButton" Text="Add" style="margin-left: 10px; float: left; width: 100px;" />

                <asp:DataGrid runat="server" 
                        ID="dtgInstructors" 
                        AutoGenerateColumns="false" 
                        Width="99%" 
                        CellPadding="0" 
                        CellSpacing="0" 
                        GridLines="None" 
                        CssClass="default-table">
                    <Columns>
                        <asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Name" />
                        <asp:TemplateColumn ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDeleteInstructor" runat="server" OnCommand="InstructorsDataGridDeleteCommand" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" Text="Delete" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>

                <br class="clear-both" /><br />

            </fieldset>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddInstructor" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upProgramProperties">
        <ProgressTemplate>
            <div class="loading-box"><h4 style="text-align: center; position: relative; top: 32px;">Please wait while we store your selection...</h4></div>
        </ProgressTemplate>
    </asp:UpdateProgress> 

    </asp:PlaceHolder>

    <br class="clear-both" /><br /><br />

    <asp:Button id="btnSave" runat="server" CssClass="SaveButton" CausesValidation="true" ValidationGroup="product" TabIndex="34" Text="Save" />
    <asp:Button id="btnDelete" runat="server" CssClass="DeleteButton" OnClientClick="return confirm('Are you sure you want to delete this Program? This cannot be undone!');" TabIndex="34" Text="Delete" />

    <br class="clear-both" />

</asp:Content>