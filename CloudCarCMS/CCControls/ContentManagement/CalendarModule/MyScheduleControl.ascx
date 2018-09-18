<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MyScheduleControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.CalendarModule.MyScheduleControl" %>
<%@ Import Namespace="CloudCar.CCFramework.ContentManagement.CalendarModule" %>

<div class="float-right" style="margin-right: 20px; margin-bottom: 10px; padding-top: 10px; width:340px; text-align:right;">
    <asp:LinkButton runat="server" ID="MyBookingsButton" Text="View My Training" CssClass="CalendarMyClassLink" Visible="False" />
    | <a href="javascript:window.print();" class="CalendarMyClassLink">Print</a>
</div>

<asp:Repeater runat="server" ID="MyClassRepeater" OnItemDataBound="ClassRepeaterItemDataBound">
    <HeaderTemplate>
        <table class="ScheduleTable" cellpadding="0" cellspacing="0">
            <tr class="ScheduleHeading">
                <td>Date</td>
                <td>Length</td>
                <td>Meeting</td>
                <td>Capacity</td>
                <td></td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr class="SignedUp">
            <td><%#Eval("BookingDate", "{0:dddd MMM dd, yyyy h:mm tt}")%></td>
            <td><%#Eval("Duration")%> mins.</td>
            <td><a href="#" class="Qtip" title="<%#Eval("Program.Name")%>"><%#Eval("Program.Name")%> <div style="display: none;"><%#Eval("Program.Content")%></div></a></td>
            <td><%# ScheduleController.GetRemainingCapacity(CInt(Eval("Id")))%></td>
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

<asp:Label runat="server" ID="OneSessionLabel" Visible="false" CssClass="PopUpStyle">

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
