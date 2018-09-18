<%@ Page Title="Schedule" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="ScheduleDetails.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.CalendarModule.ScheduleDetails" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <img src="/CCTemplates/Admin/Images/icons/product.icon.png" alt="Edit schedule" width="75" height="75" class="HeadingIcon" /><br />
    <h1 class="form-heading-style">
        Edit Schedule ( <asp:Literal runat="server" ID="litScheduleId" Text="New" />  )
    </h1><hr />

    <asp:HiddenField runat="server" ID="hfInstructorID" />
    
    <table class="default-table" cellspacing="0">
        <tr>
            <td width="40%">
                <asp:HyperLink ID="dlBackToSchedules" runat="server" NavigateUrl="~/CCAdmin/ContentManagement/CalendarModule/Schedules.aspx" Text="&laquo; Back To Schedules" />
            </td>
            <td>
            </td>
            <td></td>
            <td></td>
        </tr>
    </table>

    <asp:Label runat="server" ID="lblMessage" CssClass="status-message" Visible="False" />

    <fieldset>
        
        <h2 class="form-heading-style">Details</h2><br /><br />
        
        <label>Name</label>
        <asp:TextBox runat="server" ID="txtName" ToolTip="Instructor Name" style="width: 650px;" />
        <br class="clear-both" /><br />
        
        <label>Specialty</label>
        <asp:TextBox runat="server" ID="txtSpecialty" ToolTip="Specialty" style="width: 650px;" />
        <br class="clear-both" /><br />
        
        <label>Bio</label><br class="clear-both" />
        <textarea runat="server" id="BiographyTextArea" class="ck-editor" />
        <br class="clear-both" /><hr /><br />
        
        
        <label style="min-height: 100px;">
            Profile Image<br />
            <img runat="server" ID="imgProfileImage" alt="Profile Image Preview" visible="false" src="" class="image-display" />
        </label><br />
        <asp:HiddenField runat="server" ID="hfProfileImage" />
        <asp:Label runat="server" ID="lblProfileImageLocation" ReadOnly="true" Visible="false" CssClass="display-message" style="margin-bottom: 10px;" />
        <div class="form-file-upload-display">
	        <div class="form-fake-upload">
		        <input type="text" name="profilefilename" readonly="readonly" /> <!-- browse button is here as background -->
	        </div>
            <asp:FileUpload runat="server" ID="fuProfileImage" ToolTip="Select the profile image for this instructor." size="20" CssClass="form-real-upload" onchange="this.form.profilefilename.value = this.value;" /><br />
        </div><br />
        <span style="font-size: 12px; font-weight: bold;">Recomended Dimensions: W (80px) x H (120px)</span>
        <br class="clear-both" /><br />
        
    </fieldset>

    <asp:PlaceHolder runat="server" ID="phProperties" Visible="False">

    <asp:UpdatePanel runat="server" ID="upInstructorProperties" RenderMode="Inline" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Conditional">
        <ContentTemplate>
        
            <fieldset style="float: left; width: 550px; margin-top: 40px; margin-right: 20px;">
        
                <h3>Programs (<asp:HyperLink ID="hlNewProgram" runat="server" NavigateUrl="~/CCAdmin/ContentManagement/CalendarModule/ProgramDetails.aspx?program=new" Text="New" />)</h3>
                <p>Associate a program to this instructor</p>
                
                <asp:DropDownList runat="server" ID="ddlAddProgram" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ID" style="width: 140px; float: left; margin-left: 10px; margin-right: 10px;">
                    <asp:ListItem Text="Please Select..." Value="" />
                </asp:DropDownList>

                <asp:Button id="btnAddProgram" runat="server" CssClass="GreenButton" Text="Add" style="margin-left: 10px; float: left; width: 100px;" />

                <asp:DataGrid runat="server" ID="dtgInstructorPrograms" AutoGenerateColumns="false" Width="99%" CellPadding="0" CellSpacing="0" 
                        GridLines="None" CssClass="default-table">
                    <Columns>
                        <asp:BoundColumn DataField="Name" SortExpression="Name" HeaderText="Name" />
                        <asp:TemplateColumn ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDeleteProgram" runat="server" OnCommand="InstructorsDataGridDeleteCommand" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" Text="Delete" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>

                <br class="clear-both" /><br />

            </fieldset>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddProgram" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upInstructorProperties">
        <ProgressTemplate>
            <div class="loading-box"><h4 style="text-align: center; position: relative; top: 32px;">Please wait while we store your selection...</h4></div>
        </ProgressTemplate>
    </asp:UpdateProgress> 

    </asp:PlaceHolder>
    
    <br class="clear-both" /><br /><br />

    <asp:Button id="btnSave" runat="server" CssClass="SaveButton" CausesValidation="true" ValidationGroup="product" TabIndex="34" Text="Save" />
    <asp:Button id="btnDelete" runat="server" CssClass="DeleteButton" OnClientClick="return confirm('Are you sure you want to delete this Schedule? This cannot be undone!');" TabIndex="34" Text="Delete" />

    <br class="clear-both" /><br /><br />

</asp:Content>