
Imports CloudCar.CCFramework.ContentManagement.CalendarModule
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCControls.ContentManagement.CalendarModule

    Public Class ScheduleCalendarControl
        Inherits UserControl

        Private Const ShowIcon As Boolean = False
        Private Const ShowName As Boolean = True
        Private Const ShowTime As Boolean = True

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load

        End Sub

        Protected Sub ProgramCalendarDayRender(ByVal Sender As Object, ByVal Args As DayRenderEventArgs) Handles ProgramCalendar.DayRender
            Dim CurrentSchedules As List(Of Schedule) = ScheduleController.GetProgramSchedulesByDate(Args.Day.Date)
            Dim CurrentProgramController As New ProgramController

            For Each Item As Schedule In CurrentSchedules
                Dim CurrentControl As New HyperLink()

                Dim CurrentProgram As Program = CurrentProgramController.GetElement(Item.ProgramId)
                Dim ScheduleId As Integer = Item.Id
                Dim CurrentCommand As String = ""

                CurrentControl.ID = String.Format("ScheduleButton{0}", ScheduleId)
                CurrentControl.Text = ""

                If ShowIcon Then
                    'CurrentControl.Text = String.Format("<img src=""/images/db/{0}/30/{1}"" alt=""{2}"" class=""ProgramIconImage"" />", CurrentProgram.IconImage, SMCore.PictureController.GetPicture(Integer.Parse(CurrentProgram.IconImage)).PictureFileName, CurrentProgram.Name)
                    'CurrentDayStringBuilder.Append(String.Format("<img src=""/images/db/{0}/30/{1}"" alt=""{2}"" class=""ProgramIconImage"" />", CurrentProgram.IconImage, SMCore.PictureController.GetPicture(Integer.Parse(CurrentProgram.IconImage)).PictureFileName, CurrentProgram.Name))
                End If

                If ShowName Then
                    CurrentControl.Text &= String.Format("{0} Class", CurrentProgram.Name)
                End If

                If ShowTime Then
                    CurrentControl.Text &= String.Format(" <br /> {0:h:mm tt}", Item.BookingDate)
                End If

                If Membership.GetUser Is Nothing Then
                    If ScheduleController.GetRemainingCapacity(ScheduleId) > 0 Then
                        CurrentControl.Text &= " <br /> Join"
                        CurrentCommand = "Book Non Member"
                        CurrentControl.CssClass = "CalendarButtonBook"
                    Else
                        CurrentControl.Text &= " <br /> Full"
                        CurrentControl.Enabled = False
                        CurrentControl.CssClass = "CalendarButtonFull"
                    End If
                Else
                    Dim UserName As String = Membership.GetUser.UserName
                    Dim CurrentRegisteredUser As RegisteredUser = New RegisteredUserController().GetByUserName(UserName)

                    If RegisteredUserController.GetUserMembershipStatus(UserName) Then
                        If BookingController.CheckIfUserIsBooked(ScheduleId, CurrentRegisteredUser.UserID) Then
                            CurrentControl.Text &= " <br /> Cancel"
                            CurrentCommand = "Cancel Booking"
                            CurrentControl.CssClass = "CalendarButtonCancel"

                            If Not ScheduleController.CanCancelSchedule(ScheduleId) Then
                                CurrentControl.Enabled = False
                            End If
                        Else
                            If ScheduleController.GetRemainingCapacity(ScheduleId) > 0 Then
                                CurrentControl.Text &= " <br /> Join"
                                CurrentCommand = "Book Member"
                                CurrentControl.CssClass = "CalendarButtonBook"
                            Else
                                CurrentControl.Text &= " <br /> Full"
                                CurrentControl.Enabled = False
                                CurrentControl.CssClass = "CalendarButtonFull"
                            End If
                        End If
                    Else
                        CurrentControl.Text &= " <br /> Expired"
                        CurrentControl.Enabled = False
                    End If
                End If

                CurrentControl.NavigateUrl = Page.ClientScript.GetPostBackClientHyperlink(CalendarLinkButton, String.Format("{0},{1}", ScheduleId.ToString, CurrentCommand), True)

                Args.Cell.Controls.Add(CurrentControl)
            Next

        End Sub

        Public Sub CalendarButtonClick(ByVal Sender As Object, ByVal Args As EventArgs)
            Dim CurrentEventArguments As String = Request.Form("__EVENTARGUMENT")

            Dim CurrentScheduleId As Integer = Integer.Parse(CurrentEventArguments.Split(","c).First)
            Dim CommandName As String = CurrentEventArguments.Split(","c).Last

            Select Case CommandName
                Case "Book Non Member"
                    If Session("ScheduleId") Is Nothing Then
                        Session.Add("ScheduleId", CurrentScheduleId)
                    Else
                        Session("ScheduleId") = CurrentScheduleId
                    End If

                    Response.Redirect("/Login.html")
                Case "Cancel Booking"
                    Dim UserName As String = Membership.GetUser.UserName
                    Dim CurrentRegisteredUser As RegisteredUser = New RegisteredUserController().GetByUserName(UserName)
                    Dim ScheduleId As Integer = CurrentScheduleId

                    Dim CurrentBookingController As New BookingController()

                    CurrentBookingController.Delete(CurrentRegisteredUser.UserID, ScheduleId)

                    'LoadSchedule()

                    CancelLabel.Visible = True
                    SignUpLabel.Visible = False
                    OneWorkoutLabel.Visible = False

                    ConfirmPopupExtender.PopupControlID = "CancelLabel"
                    ConfirmPopupExtender.CancelControlID = "CancelContinueButton"
                    ConfirmPopupExtender.Show()
                Case "Book Member"
                    Dim UserName As String = Membership.GetUser.UserName
                    Dim CurrentRegisteredUser As RegisteredUser = New RegisteredUserController().GetByUserName(UserName)
                    Dim ScheduleId As Integer = CurrentScheduleId

                    Dim ExceededMaximumBookingsPerDay As Boolean = BookingController.ExceededMaximumBookingsPerDay(CurrentRegisteredUser.UserID, ScheduleId)

                    If Not ExceededMaximumBookingsPerDay Then
                        Dim RemainingCapacity As Integer = ScheduleController.GetRemainingCapacity(ScheduleId)

                        If RemainingCapacity > 0 Then
                            Dim CurrentBookingController As New BookingController()

                            CurrentBookingController.Create(CurrentRegisteredUser.UserID, ScheduleId, True)

                            'LoadSchedule()

                            SignUpLabel.Visible = True
                            CancelLabel.Visible = False
                            OneWorkoutLabel.Visible = False

                            ConfirmPopupExtender.PopupControlID = "SignUpLabel"
                            ConfirmPopupExtender.CancelControlID = "SignUpContinueButton"
                            ConfirmPopupExtender.Show()
                        End If
                    Else
                        SignUpLabel.Visible = False
                        CancelLabel.Visible = False
                        OneWorkoutLabel.Visible = True

                        ConfirmPopupExtender.PopupControlID = "OneWorkoutLabel"
                        ConfirmPopupExtender.CancelControlID = "OneWorkoutButton"
                        ConfirmPopupExtender.Show()
                    End If
            End Select
        End Sub

        Protected Function CheckSignedUp(ByVal ScheduleId As Integer, ByVal ClassName As String) As String
            If Not Membership.GetUser Is Nothing Then
                Dim UserName As String = Membership.GetUser.UserName
                Dim CurrentRegisteredUser As RegisteredUser = New RegisteredUserController().GetByUserName(UserName)

                If BookingController.CheckIfUserIsBooked(ScheduleId, CurrentRegisteredUser.UserID) Then
                    CheckSignedUp = " " & ClassName
                Else
                    CheckSignedUp = ""
                End If
            Else
                CheckSignedUp = ""
            End If
        End Function

        Private Sub SignUpContinueButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles SignUpContinueButton.Click
            ConfirmPopupExtender.Hide()
        End Sub

        Private Sub CancelContinueButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles CancelContinueButton.Click
            ConfirmPopupExtender.Hide()
        End Sub

    End Class
End Namespace