<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ScheduleCalendarControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.CalendarModule.ScheduleCalendarControl" %>

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
                Visible="True">
    <NextPrevStyle CssClass="NextPreviousButtons" ForeColor="#1658DA" />
    <TitleStyle CssClass="CalendarTitle" />
    <DayHeaderStyle CssClass="DayHeader" />
    <TodayDayStyle CssClass="TodayDay" />
    <DayStyle CssClass="CalendarDay" />
    <OtherMonthDayStyle CssClass="OtherMonthDay" />
</asp:Calendar>
    
<asp:LinkButton ID="CalendarLinkButton" runat="server" CssClass="hide" OnClick="CalendarButtonClick" />
    
<asp:HiddenField runat="server" ID="DummyField"/>
    
<ajax:ModalPopupExtender runat="server" ID="ConfirmPopupExtender" PopupControlID="SignUpLabel" 
        TargetControlID="DummyField" DropShadow="false" BackgroundCssClass="PopUpBackgroundStyle" CancelControlID="SignUpContinueButton">
</ajax:ModalPopupExtender>

<asp:Label runat="server" ID="SignUpLabel" Visible="false" CssClass="PopUpStyle">

    <asp:UpdatePanel ID="InnerUpdatePanel" runat="server">
        <ContentTemplate>

            <h1>Thank You</h1>
            <p>You have been signed up for the selected meeting.</p>
                
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
            <p>You have been removed from the selected meeting.</p>
                
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
            <p>You can only book one session per day.</p>
                
            <br style="clear: both;" />
                
            <asp:LinkButton runat="server" ID="OneWorkoutButton" Text="Continue &raquo;" /><br /><br />

            <br style="clear: both;" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Label>