Imports CloudCar.CCFramework.ContentManagement.CalendarModule
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCControls.ContentManagement.CalendarModule

    Public Class MyScheduleControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load

        End Sub

        Private Sub MyBookingsButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles MyBookingsButton.Click
            If Not Membership.GetUser Is Nothing Then
                LoadMySchedule()
            End If
        End Sub

        Private Sub LoadMySchedule()
            Dim UserName As String = Membership.GetUser.UserName
            Dim CurrentRegisteredUser As RegisteredUser = New RegisteredUserController().GetByUserName(UserName)

            MyClassRepeater.Visible = True
            MyClassRepeater.DataSource = ScheduleController.GetFutureSignedUpProgramSchedules(CurrentRegisteredUser.UserID)
            MyClassRepeater.DataBind()

            MyBookingsButton.Visible = False
        End Sub

        Public Sub CommandLinkButtonClick(ByVal Sender As Object, ByVal Args As CommandEventArgs)
            Select Case Args.CommandName
                Case "Book Non Member"
                    If Session("ScheduleId") Is Nothing Then
                        Session.Add("ScheduleId", Integer.Parse(Args.CommandArgument.ToString))
                    Else
                        Session("ScheduleId") = Integer.Parse(Args.CommandArgument.ToString)
                    End If

                    'Response.Redirect("/Forms/Program-Sign-Up.html")
                    Response.Redirect("/Login.html")
                Case "Cancel Booking"
                    Dim UserName As String = Membership.GetUser.UserName
                    Dim CurrentRegisteredUser As RegisteredUser = New RegisteredUserController().GetByUserName(UserName)
                    Dim ScheduleId As Integer = Integer.Parse(Args.CommandArgument.ToString)

                    Dim CurrentBookingController As New BookingController()

                    CurrentBookingController.Delete(CurrentRegisteredUser.UserID, ScheduleId)

                    'LoadSchedule()

                    CancelLabel.Visible = True
                    SignUpLabel.Visible = False
                    OneSessionLabel.Visible = False

                    ConfirmPopupExtender.PopupControlID = "CancelLabel"
                    ConfirmPopupExtender.CancelControlID = "CancelContinueButton"
                    ConfirmPopupExtender.Show()
                Case "Book Member"
                    Dim UserName As String = Membership.GetUser.UserName
                    Dim CurrentRegisteredUser As RegisteredUser = New RegisteredUserController().GetByUserName(UserName)
                    Dim ScheduleId As Integer = Integer.Parse(Args.CommandArgument.ToString)

                    Dim ExceededMaximumBookingsPerDay As Boolean = BookingController.ExceededMaximumBookingsPerDay(CurrentRegisteredUser.UserID, ScheduleId)

                    If Not ExceededMaximumBookingsPerDay Then
                        Dim RemainingCapacity As Integer = ScheduleController.GetRemainingCapacity(ScheduleId)

                        If RemainingCapacity > 0 Then
                            Dim CurrentBookingController As New BookingController()

                            CurrentBookingController.Create(CurrentRegisteredUser.UserID, ScheduleId, True)

                            'LoadSchedule()

                            SignUpLabel.Visible = True
                            CancelLabel.Visible = False
                            OneSessionLabel.Visible = False

                            ConfirmPopupExtender.PopupControlID = "SignUpLabel"
                            ConfirmPopupExtender.CancelControlID = "SignUpContinueButton"
                            ConfirmPopupExtender.Show()
                        End If
                    Else
                        SignUpLabel.Visible = False
                        CancelLabel.Visible = False
                        OneSessionLabel.Visible = True

                        ConfirmPopupExtender.PopupControlID = "OneWorkoutLabel"
                        ConfirmPopupExtender.CancelControlID = "OneWorkoutButton"
                        ConfirmPopupExtender.Show()
                    End If
                Case Else
                    'Response.Write("Other")
            End Select
        End Sub

        Private Sub SignUpContinueButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles SignUpContinueButton.Click
            ConfirmPopupExtender.Hide()
        End Sub

        Private Sub CancelContinueButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles CancelContinueButton.Click
            ConfirmPopupExtender.Hide()
        End Sub

        Protected Sub ClassRepeaterItemDataBound(ByVal Sender As Object, ByVal E As RepeaterItemEventArgs)
            Throw New NotImplementedException()
        End Sub
    End Class

End Namespace