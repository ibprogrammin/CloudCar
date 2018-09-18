<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EventsCalendar.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.EventsModule.EventsCalendar" %>

<asp:Calendar runat="server" 
                ID="EventsCalendar" 
                DayNameFormat="Full"
                NextPrevFormat="ShortMonth"
                CellPadding="0"
                CellSpacing="5"
                ShowGridLines="False"
                CssClass="DefaultCalendar" 
                UseAccessibleHeader="True"
                BorderWidth="0"
                Visible="True">
    <NextPrevStyle CssClass="NextPreviousButtons" ForeColor="#adfafd" />
    <TitleStyle CssClass="CalendarTitle" />
    <DayHeaderStyle CssClass="DayHeader" />
    <TodayDayStyle CssClass="TodayDay" />
    <DayStyle CssClass="CalendarDay" />
    <OtherMonthDayStyle CssClass="OtherMonthDay" />
</asp:Calendar>