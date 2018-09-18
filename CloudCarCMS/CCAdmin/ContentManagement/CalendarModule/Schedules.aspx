<%@ Page Title="Schedules" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Schedules.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.CalendarModule.Schedules" %>
<%@ Import Namespace="CloudCar.CCFramework.ContentManagement.CalendarModule" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
        .RadioButtonList { border: 0px solid #AAA; margin: 0px 0px; padding: 10px; padding-top: 0px; background-color: #FFF; }
        .RadioButtonList label { width: 120px; float: left; }
        .RadioButtonList input { float: left; margin-top: 15px; }
        
        .CheckBoxList { border: 0px solid #AAA; margin: 0px 0px; padding: 10px; padding-top: 0px; background-color: #FFF; }
        .CheckBoxList label { width: 120px; float: left; }
        .CheckBoxList input { float: left; margin-top: 15px; }
    </style>

</asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <asp:Button ID="AddScheduleButton" runat="server" UseSubmitBehavior="true" CssClass="SaveButton" style="float: right; width: 280px; margin-top: 30px;" Text="Add Schedule" />

    <img src="/CCTemplates/Admin/Images/icons/order.icon.png" alt="View Schedules" width="75" height="75" class="HeadingIcon" /><br />
    <h1 class="form-heading-style">
        Schedules
    </h1><hr />

    <asp:UpdatePanel runat="server" ID="upUpdate" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Always">
        <ContentTemplate>

    <br /><asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="False" /><br />
        
    <asp:PlaceHolder runat="server" ID="AddSchedulePlaceHolder" Visible="False">
        
    <fieldset>
        
        <h2 class="form-heading-style">Details</h2><br /><br />
        
        <asp:HiddenField runat="server" ID="ScheduleIdHiddenField" />
        
        <label>Program</label>
        <asp:DropDownList runat="server" ID="ProgramDropDown" DataValueField="Id" DataTextField="Name" ToolTip="Programs" style="width: 670px;" />
        <br class="clear-both" /><br />
        
        <label>Date</label>
        <asp:TextBox runat="server" ID="DateTextBox" ToolTip="Date" style="width: 574px;" />
        <asp:ImageButton runat="server" ID="CalendarButton" ImageUrl="/CCTemplates/Admin/images/icons/news.events.icon.png" ToolTip="Calendar" style="margin-top: -5px; margin-bottom: -20px; margin-left: 20px; width: 60px; height: 60px;" />
        <ajax:CalendarExtender runat="server" TargetControlID="DateTextBox" Format="MMMM d, yyyy" PopupButtonID="CalendarButton" />
        <br class="clear-both" /><br />
        
        <label>Time</label>
        <asp:DropDownList runat="server" ID="HourDropDown" style="width: 120px; margin-right: 20px;">
            <asp:ListItem Text="1" Value="1" />
            <asp:ListItem Text="2" Value="2" />
            <asp:ListItem Text="3" Value="3" />
            <asp:ListItem Text="4" Value="4" />
            <asp:ListItem Text="5" Value="5" />
            <asp:ListItem Text="6" Value="6" />
            <asp:ListItem Text="7" Value="7" />
            <asp:ListItem Text="8" Value="8" />
            <asp:ListItem Text="9" Value="9" />
            <asp:ListItem Text="10" Value="10" />
            <asp:ListItem Text="11" Value="11" />
            <asp:ListItem Text="12" Value="12" />
        </asp:DropDownList>
        <asp:DropDownList runat="server" ID="MinuteDropDown" style="width: 120px; margin-right: 20px;">
            <asp:ListItem Text="00" Value="00" />
            <asp:ListItem Text="15" Value="15" />
            <asp:ListItem Text="30" Value="30" />
            <asp:ListItem Text="45" Value="45" />
        </asp:DropDownList>
        <asp:DropDownList runat="server" ID="TimeOfDayDropDown" style="width: 90px;">
            <asp:ListItem Text="AM" Value="AM" />
            <asp:ListItem Text="PM" Value="PM" />
        </asp:DropDownList>
        <br class="clear-both" /><br />
        
        <label>Duration (mins.)</label>
        <asp:DropDownList runat="server" ID="DurationDropDown" style="width: 120px; margin-right: 20px;">
            <asp:ListItem Text="15" Value="15" />
            <asp:ListItem Text="30" Value="30" />
            <asp:ListItem Text="45" Value="45" />
            <asp:ListItem Text="60" Value="60" />
            <asp:ListItem Text="75" Value="75" />
            <asp:ListItem Text="90" Value="90" />
            <asp:ListItem Text="105" Value="105" />
            <asp:ListItem Text="120" Value="120" />
            <asp:ListItem Text="135" Value="135" />
            <asp:ListItem Text="150" Value="150" />
            <asp:ListItem Text="165" Value="165" />
            <asp:ListItem Text="180" Value="180" />
        </asp:DropDownList>
        <br class="clear-both" /><br />
        
        <!--label>Free Class</label-->
        <asp:DropDownList runat="server" ID="FreeClassDropDown" ToolTip="Date" style="width: 170px;" Visible="False">
            <asp:ListItem Value="True" Text="Yes" />
            <asp:ListItem Value="False" Text="No" Selected="True" />
        </asp:DropDownList>
        
        <label>Capacity</label>
        <asp:TextBox runat="server" ID="CapacityTextBox" ToolTip="Capacity" style="width: 150px;" Text="20" />
        <br class="clear-both" /><br />
        
        <label>Current Signups</label>
        <asp:Repeater runat="server" ID="SignUpRepeater">
            <HeaderTemplate>
                <table class="default-table" cellpadding="0" cellspacing="0" style="width: 650px;">
                    <thead class="default-table-header">
                        <tr>
                            <td>Name</td>
                            <td>Email</td>
                            <td>Phone</td>
                        </tr>
                    </thead>
           </HeaderTemplate> 
           <ItemTemplate>
                <tr>
                    <td><%#Eval("FirstName")%> <%#Eval("LastName")%></td>
                    <td><a href="mailto:<%#Eval("Email") %>"><%# Eval("Email") %></a></td>
                    <td><%#Eval("PhoneNumber")%></td>
                </tr>  
           </ItemTemplate>
           <FooterTemplate>
               </table>
           </FooterTemplate>
        </asp:Repeater>
        <br class="clear-both" /><br />
        
    </fieldset>
    
    <br/><br />
    
    <fieldset runat="server" id="RepeatScheduleFields" Visible="False">
        
        <h2 class="form-heading-style">Repeat Schedule</h2><br /><br />
        
        <label>Repeat</label>
        <asp:RadioButtonList runat="server" ID="RepeatList" RepeatLayout="Table" CssClass="RadioButtonList" RepeatColumns="4">
            <asp:ListItem Text="Never" Value="Never" Selected="True" />
            <asp:ListItem Text="Weekly" Value="Weekly" />
        </asp:RadioButtonList>
        <br class="clear-both" /><br />
        
        <!--label>Days</label-->
        <asp:CheckBoxList runat="server" ID="DaysOfWeekList" RepeatLayout="Table" CssClass="CheckBoxList" RepeatColumns="4" Visible="False">
            <asp:ListItem Text="Monday" Value="Monday" />
            <asp:ListItem Text="Tuesday" Value="Tuesday" />
            <asp:ListItem Text="Wednesday" Value="Wednesday" />
            <asp:ListItem Text="Thursday" Value="Thursday" />
            <asp:ListItem Text="Friday" Value="Friday" />
            <asp:ListItem Text="Saturday" Value="Saturday" />
            <asp:ListItem Text="Sunday" Value="Sunday" />
        </asp:CheckBoxList>
        
        <label>How many times?</label>
        <asp:TextBox runat="server" ID="FrequencyTextBox" ToolTip="Capacity" Text="1" style="width: 150px;" />
        <br class="clear-both" /><br />
        
    </fieldset>
    
    <br class="clear-both" />

    <asp:Button id="SaveButton" runat="server" CssClass="SaveButton" CausesValidation="true" ValidationGroup="product" TabIndex="34" Text="Save" />
    <asp:Button id="ClearButton" runat="server" CssClass="DeleteButton" TabIndex="34" Text="Clear" />

    <br class="clear-both" /><br /><br />
    
    </asp:PlaceHolder>
           
    <asp:DropDownList runat="server" ID="ProgramsDropDownList" DataValueField="Id" DataTextField="Name" OnSelectedIndexChanged="FilterScheduleList" AutoPostBack="True" style="margin-right: 20px;" />
    <asp:DropDownList runat="server" ID="MonthDropDownList" OnSelectedIndexChanged="FilterScheduleList" AutoPostBack="True" style="margin-right: 20px;" />
    <asp:DropDownList runat="server" ID="YearDropDownList" OnSelectedIndexChanged="FilterScheduleList" AutoPostBack="True" style="margin-right: 20px;" />
           
    <asp:DataGrid ID="gvSchedules" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" DataKeyField="Id" PageSize="10" 
            AllowPaging="true" AllowSorting="True" GridLines="None" CssClass="default-table" CellPadding="0" CellSpacing="0"
            OnDeleteCommand="SchedulesGridDeleteCommand" OnEditCommand="SchedulesGridEditCommand">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:ButtonColumn Text="Edit" ButtonType="LinkButton" CommandName="Edit" />
            <asp:TemplateColumn HeaderText="Program">
                <ItemTemplate>
                    <a href="/CAAdmin/ContentManagement/CalendarModule/ProgramDetails.aspx?Program=<%# Eval("Program.Id") %>"><%#Eval("Program.Name")%></a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="BookingDate" HeaderText="Date" DataFormatString="{0:MMMM dd, yyyy}" />
            <asp:BoundColumn DataField="BookingDate" HeaderText="Time" DataFormatString="{0:h:mm tt}" />
            <asp:BoundColumn DataField="Duration" HeaderText="Duration (mins.)" />
            <asp:BoundColumn DataField="Free" HeaderText="Free Class" Visible="False" />
            <asp:BoundColumn DataField="Capacity" HeaderText="Capacity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
            <asp:TemplateColumn HeaderText="Sign Ups" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <%# ScheduleController.GetScheduleBookingsCount(CInt(Eval("Id")))%>
                     (<a href="<%# String.Format("/CAAdmin/ContentManagement/CalendarModule/ScheduleSignups.aspx?ScheduleId={0}", Eval("Id")) %>" target="_blank">View</a>)
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="CancelClassButton" Text="Cancel Class" OnCommand="CancelClassButtonCommand" CommandArgument='<%#Eval("Id") %>' onClientClick="return confirm('Are you sure you want to cancel this class? All sign ups will be notified of cancelation!');" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:ButtonColumn Text="Cancel Class" ButtonType="LinkButton" CommandName="Cancel" Visible="False" />
            <asp:ButtonColumn Text="Delete" ButtonType="LinkButton" CommandName="Delete" Visible="False" />
        </Columns>
    </asp:DataGrid>

    <br />
    <br />
     <!--   -->
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress AssociatedUpdatePanelID="upUpdate" runat="server" ID="upProgress" DynamicLayout="false" >
        <ProgressTemplate>
            <div style="display: table-cell; position: fixed; top: 50%; left: 40%; width: 400px; height: 80px; background-color: #FFF; border: 1px solid #F5F5F5; vertical-align: middle;"><h3 class="BoldDark" style="text-align: center; position: relative; top: 32px;">Loading please wait...</h3></div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>