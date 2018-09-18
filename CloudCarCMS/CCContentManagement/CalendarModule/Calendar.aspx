<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableTheming="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Calendar.aspx.vb" Inherits="CloudCar.CCContentManagement.CalendarModule.Calendar" %>
<%@ Import Namespace="CloudCar.CCFramework.ContentManagement.CalendarModule" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server" />

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server"/>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

    <h1 style="clear: right;">Workout Schedule</h1>
    
    <p style="color: #555;">Members please login to reserve your workout days and time slots. Please note that you may only book one workout per day.</p><br/>
    
    <div class="float-right" style="margin-right: 80px; padding-top: 10px; width:340px; text-align:right;">
        <asp:LinkButton runat="server" ID="MyBookingsButton" Text="View My Classes" CssClass="CalendarMyClassLink" Visible="False" />
        <asp:LinkButton runat="server" ID="BackToScheduleButton" Text="Back to Schedule" CssClass="CalendarMyClassLink" Visible="False" />
         | <a href="javascript:window.print();" class="CalendarMyClassLink">Print</a>
    </div>
    
    <asp:DropDownList runat="server" ID="SelectMonthDropDown" AutoPostBack="True" OnSelectedIndexChanged="ChangeSelectedDate" CssClass="CalendarSelect" />
    <asp:DropDownList runat="server" ID="SelectYearDropDown" AutoPostBack="True" OnSelectedIndexChanged="ChangeSelectedDate" CssClass="CalendarSelect" />
    
    <asp:Repeater runat="server" ID="DayRepeater">
        <HeaderTemplate>
            <table class="ScheduleTable" cellpadding="0" cellspacing="0">
        </HeaderTemplate>
        <ItemTemplate>

            <tr class="parent ScheduleHeading" id="<%# "" & Eval("Date","{0:dd}") %>">
                <td>
                    <asp:HiddenField runat="server" ID="CurrentDateField" Value='<%#Eval("Date") %>'/>
                    <%#Eval("Date", "{0:dddd MMM dd, yyyy}")%>
                </td>
                <td>Length</td>
                <td width="25%">Class</td>
                <td>Capacity</td>
                <td><a href="#">Show/Hide</a></td>
            </tr>

            <asp:Repeater runat="server" ID="ClassRepeater" OnItemDataBound="ClassRepeaterItemDataBound">
                <ItemTemplate>
                    <tr class="<%# "child-" & Eval("BookingDate", "{0:dd}") & CheckSignedUp(integer.parse(Eval("Id")),"SignedUp") %>">
                        <td><%#Eval("BookingDate", "{0:h:mm tt}")%></td>
                        <td><%#Eval("Duration")%> mins.</td>
                        <td><a href="#" class="Qtip" title="<%#Eval("Program.Name")%>"><%#Eval("Program.Name")%> <div style="display: none;"><%#Eval("Program.Content")%></div></a></td>
                        <td><%# ScheduleController.GetRemainingCapacity(Integer.Parse(Eval("Id")))%></td>
                        <td>
                            <asp:HiddenField runat="server" ID="ScheduleIdField" Value='<%# Eval("Id")%>'/>
                            <asp:LinkButton runat="server" ID="CommandLinkButton" OnCommand="CommandLinkButtonClick" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>

        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    
    <asp:Repeater runat="server" ID="MyClassRepeater" OnItemDataBound="ClassRepeaterItemDataBound">
        <HeaderTemplate>
            <table class="ScheduleTable" cellpadding="0" cellspacing="0">
                <tr class="ScheduleHeading">
                    <td>Date</td>
                    <td>Length</td>
                    <td>Class</td>
                    <td>Capacity</td>
                    <td></td>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="SignedUp">
                <td><%#Eval("BookingDate", "{0:dddd MMM dd, yyyy h:mm tt}")%></td>
                <td><%#Eval("Duration")%> mins.</td>
                <td><a href="#" class="Qtip" title="<%#Eval("Program.Name")%>"><%#Eval("Program.Name")%> <div style="display: none;"><%#Eval("Program.Content")%></div></a></td>
                <td><%# ScheduleController.GetRemainingCapacity(Integer.Parse(Eval("Id")))%></td>
                <td>
                    <asp:HiddenField runat="server" ID="ScheduleIdField" Value='<%#Eval("Id")%>'/>
                    <asp:LinkButton runat="server" ID="CommandLinkButton" OnCommand="CommandLinkButtonClick" />
                </td>
            </tr>    
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    
    
    <asp:Calendar runat="server" 
                    ID="ProgramCalendar" 
                    DayNameFormat="Full"
                    NextPrevFormat="ShortMonth"
                    CellPadding="0"
                    CellSpacing="5"
                    ShowGridLines="False"
                    CssClass="DefaultCalendar" 
                    UseAccessibleHeader="True"
                    BorderWidth="0"
                    Visible="False">
        <NextPrevStyle CssClass="NextPreviousButtons" ForeColor="#1658DA" />
        <TitleStyle CssClass="CalendarTitle" />
        <DayHeaderStyle CssClass="DayHeader" />
        <TodayDayStyle CssClass="TodayDay" />
        <DayStyle CssClass="CalendarDay" />
        <OtherMonthDayStyle CssClass="OtherMonthDay" />
    </asp:Calendar>
    
    <asp:HiddenField runat="server" ID="DummyField"/>
    
    <ajax:ModalPopupExtender runat="server" ID="ConfirmPopupExtender" PopupControlID="SignUpLabel" 
         TargetControlID="DummyField" DropShadow="false" BackgroundCssClass="PopUpBackgroundStyle" CancelControlID="SignUpContinueButton">
    </ajax:ModalPopupExtender>

    <asp:Label runat="server" ID="SignUpLabel" Visible="false" CssClass="PopUpStyle">

        <asp:UpdatePanel ID="InnerUpdatePanel" runat="server">
            <ContentTemplate>

                <h1>Thank You</h1>
                <p>You have been signed up to the selected class please note our policy located on the current page.</p>
                
                <br style="clear: both;" />
                
                <asp:LinkButton runat="server" ID="SignUpContinueButton" Text="Continue &raquo;" /><br /><br />

                <br style="clear: both;" />

            </ContentTemplate>
        </asp:UpdatePanel>

    </asp:Label>

    <asp:Label runat="server" ID="CancelLabel" Visible="false" CssClass="PopUpStyle">

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <h1>Thank You</h1>
                <p>You have been removed from the selected class.</p>
                
                <br style="clear: both;" />
                
                <asp:LinkButton runat="server" ID="CancelContinueButton" Text="Continue &raquo;" /><br /><br />

                <br style="clear: both;" />

            </ContentTemplate>
        </asp:UpdatePanel>

    </asp:Label>

    <asp:Label runat="server" ID="OneWorkoutLabel" Visible="false" CssClass="PopUpStyle">

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>

                <h1>We're Sorry</h1>
                <p>You can only book one workout per day.</p>
                
                <br style="clear: both;" />
                
                <asp:LinkButton runat="server" ID="OneWorkoutButton" Text="Continue &raquo;" /><br /><br />

                <br style="clear: both;" />

            </ContentTemplate>
        </asp:UpdatePanel>

    </asp:Label>
    
</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server"/>
<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    
</asp:Content>