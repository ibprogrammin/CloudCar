Imports CloudCar.CCFramework.Model

Namespace CCContentManagement.CalendarModule

    Partial Public Class Calendar
        Inherits Page

        Private Const ShowIcon As Boolean = True
        Private Const ShowName As Boolean = False
        Private Const ShowTime As Boolean = False

        Protected Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadDateDropDowns()

                SelectYearDropDown.SelectedValue = Date.Now.Year.ToString
                SelectMonthDropDown.SelectedValue = Date.Now.Month.ToString

                LoadSchedule()
            End If

            Title = "Fitness Schedule"
        End Sub

        Private Sub LoadSchedule()
            DayRepeater.Visible = True
            DayRepeater.DataSource = CCFramework.ContentManagement.CalendarModule.ScheduleController.GetFutureScheduleDaysByMonthYear(Integer.Parse(SelectMonthDropDown.SelectedValue), Integer.Parse(SelectYearDropDown.SelectedValue))
            DayRepeater.DataBind()

            MyClassRepeater.Visible = False

            If Not Membership.GetUser Is Nothing Then
                MyBookingsButton.Visible = True
            End If

            BackToScheduleButton.Visible = False
        End Sub

        Private Sub LoadMySchedule()
            Dim UserName As String = Membership.GetUser.UserName
            Dim CurrentRegisteredUser As RegisteredUser = New CCFramework.Core.RegisteredUserController().GetByUserName(UserName)

            MyClassRepeater.Visible = True
            MyClassRepeater.DataSource = CCFramework.ContentManagement.CalendarModule.ScheduleController.GetFutureSignedUpProgramSchedules(CurrentRegisteredUser.UserID)
            MyClassRepeater.DataBind()

            DayRepeater.Visible = False

            MyBookingsButton.Visible = False
            BackToScheduleButton.Visible = True
        End Sub

        Protected Sub ChangeSelectedDate(ByVal Sender As Object, ByVal E As EventArgs)
            LoadSchedule()
        End Sub

        Private Const CountOfYears As Integer = 6
        Private Sub LoadDateDropDowns()
            For i As Integer = 0 To CountOfYears - 1
                Dim CurrentYear As Integer = Date.Now.Year + i

                SelectYearDropDown.Items.Add(New ListItem(CurrentYear.ToString, CurrentYear.ToString))
            Next

            For j As Integer = 0 To 11
                Dim CurrentMonth As Integer = ((Date.Now.Month + (j - 1)) Mod 12) + 1

                SelectMonthDropDown.Items.Add(New ListItem(MonthName(CurrentMonth), CurrentMonth.ToString))
            Next
        End Sub

        Protected Sub ProgramCalendarDayRender(ByVal Sender As Object, ByVal E As DayRenderEventArgs) Handles ProgramCalendar.DayRender
            'Dim CurrentDayStringBuilder As New StringBuilder

            'CurrentDayStringBuilder.Append(String.Format("{0}<br />", E.Day.Date.Day))

            'Dim CurrentSchedules As List(Of Schedule) = CalendarModule.ScheduleController.GetProgramSchedulesByDate(E.Day.Date)
            'Dim CurrentProgramController As New CalendarModule.ProgramController

            'For Each Item As Schedule In CurrentSchedules
            '    Dim CurrentProgram As Program = CurrentProgramController.GetElement(Item.ProgramId)

            '    CurrentDayStringBuilder.Append(String.Format("<a href=""/Programs/{0}.html"" class=""ProgramLink"">", CurrentProgram.Permalink))

            '    If ShowIcon Then
            '        CurrentDayStringBuilder.Append(String.Format("<img src=""/images/db/{0}/30/{1}"" alt=""{2}"" class=""ProgramIconImage"" />", CurrentProgram.IconImage, SMCore.PictureController.GetPicture(Integer.Parse(CurrentProgram.IconImage)).PictureFileName, CurrentProgram.Name))
            '    End If

            '    If ShowName Then
            '        CurrentDayStringBuilder.Append(String.Format("{0} Class - ", CurrentProgram.Name))
            '    End If

            '    If ShowTime Then
            '        CurrentDayStringBuilder.Append(String.Format("{0:h:mm tt}", Item.BookingDate))
            '    End If

            '    CurrentDayStringBuilder.Append(String.Format("</a>"))
            'Next

            'E.Cell.Text = CurrentDayStringBuilder.ToString
        End Sub

        Protected Sub DayRepeaterItemDataBound(ByVal Sender As Object, ByVal E As RepeaterItemEventArgs) Handles DayRepeater.ItemDataBound
            Dim CurrentDateField As HiddenField = CType(E.Item.FindControl("CurrentDateField"), HiddenField)

            If Not CurrentDateField Is Nothing Then
                Dim CurrentDate As Date = Date.Parse(CurrentDateField.Value)
                Dim CurrentSchedules As List(Of Schedule) = CCFramework.ContentManagement.CalendarModule.ScheduleController.GetProgramSchedulesByDate(CurrentDate)

                Dim ClassRepeater As Repeater = CType(E.Item.FindControl("ClassRepeater"), Repeater)

                ClassRepeater.DataSource = CurrentSchedules
                ClassRepeater.DataBind()
            End If
        End Sub

        Protected Sub ClassRepeaterItemDataBound(ByVal Sender As Object, ByVal E As RepeaterItemEventArgs)
            Dim CurrentCommandLinkButton As LinkButton = CType(E.Item.FindControl("CommandLinkButton"), LinkButton)

            Dim CurrentScheduleIdField As HiddenField = CType(E.Item.FindControl("ScheduleIdField"), HiddenField)

            If Not CurrentScheduleIdField Is Nothing Then
                Dim ScheduleId As Integer = Integer.Parse(CurrentScheduleIdField.Value)

                If Membership.GetUser Is Nothing Then
                    If CCFramework.ContentManagement.CalendarModule.ScheduleController.GetRemainingCapacity(ScheduleId) > 0 Then
                        CurrentCommandLinkButton.Text = "Sign Up"
                        CurrentCommandLinkButton.CommandName = "Book Non Member"
                        CurrentCommandLinkButton.CommandArgument = ScheduleId.ToString
                    Else
                        CurrentCommandLinkButton.Text = "Full"
                        CurrentCommandLinkButton.CommandName = ""
                        CurrentCommandLinkButton.Enabled = False
                    End If
                Else
                    Dim UserName As String = Membership.GetUser.UserName
                    Dim CurrentRegisteredUser As RegisteredUser = New CCFramework.Core.RegisteredUserController().GetByUserName(UserName)

                    If CCFramework.Core.RegisteredUserController.GetUserMembershipStatus(UserName) Then
                        If CCFramework.ContentManagement.CalendarModule.BookingController.CheckIfUserIsBooked(ScheduleId, CurrentRegisteredUser.UserID) Then
                            CurrentCommandLinkButton.Text = "Cancel"
                            CurrentCommandLinkButton.CommandName = "Cancel Booking"
                            CurrentCommandLinkButton.CommandArgument = ScheduleId.ToString

                            If Not CCFramework.ContentManagement.CalendarModule.ScheduleController.CanCancelSchedule(ScheduleId) Then
                                CurrentCommandLinkButton.Enabled = False
                            End If
                        Else
                            If CCFramework.ContentManagement.CalendarModule.ScheduleController.GetRemainingCapacity(ScheduleId) > 0 Then
                                CurrentCommandLinkButton.Text = "Sign Up"
                                CurrentCommandLinkButton.CommandName = "Book Member"
                                CurrentCommandLinkButton.CommandArgument = ScheduleId.ToString
                            Else
                                CurrentCommandLinkButton.Text = "Full"
                                CurrentCommandLinkButton.CommandName = ""
                                CurrentCommandLinkButton.Enabled = False
                            End If
                        End If
                    Else
                        CurrentCommandLinkButton.Text = "Expired"
                        CurrentCommandLinkButton.CommandName = ""
                        CurrentCommandLinkButton.Enabled = False
                    End If
                End If
            End If
        End Sub

        Protected Sub CommandLinkButtonClick(ByVal Sender As Object, ByVal E As CommandEventArgs)
            Select Case E.CommandName
                Case "Book Non Member"
                    If Session("ScheduleId") Is Nothing Then
                        Session.Add("ScheduleId", Integer.Parse(E.CommandArgument.ToString))
                    Else
                        Session("ScheduleId") = Integer.Parse(E.CommandArgument.ToString)
                    End If

                    'Response.Redirect("/Forms/Program-Sign-Up.html")
                    Response.Redirect("/Login.html")
                Case "Cancel Booking"
                    Dim UserName As String = Membership.GetUser.UserName
                    Dim CurrentRegisteredUser As RegisteredUser = New CCFramework.Core.RegisteredUserController().GetByUserName(UserName)
                    Dim ScheduleId As Integer = Integer.Parse(E.CommandArgument.ToString)

                    Dim CurrentBookingController As New CCFramework.ContentManagement.CalendarModule.BookingController()

                    CurrentBookingController.Delete(CurrentRegisteredUser.UserID, ScheduleId)

                    LoadSchedule()

                    CancelLabel.Visible = True
                    SignUpLabel.Visible = False
                    OneWorkoutLabel.Visible = False

                    ConfirmPopupExtender.PopupControlID = "CancelLabel"
                    ConfirmPopupExtender.CancelControlID = "CancelContinueButton"
                    ConfirmPopupExtender.Show()
                Case "Book Member"
                    Dim UserName As String = Membership.GetUser.UserName
                    Dim CurrentRegisteredUser As RegisteredUser = New CCFramework.Core.RegisteredUserController().GetByUserName(UserName)
                    Dim ScheduleId As Integer = Integer.Parse(E.CommandArgument.ToString)

                    Dim ExceededMaximumBookingsPerDay As Boolean = CCFramework.ContentManagement.CalendarModule.BookingController.ExceededMaximumBookingsPerDay(CurrentRegisteredUser.UserID, ScheduleId)

                    If Not ExceededMaximumBookingsPerDay Then
                        Dim RemainingCapacity As Integer = CCFramework.ContentManagement.CalendarModule.ScheduleController.GetRemainingCapacity(ScheduleId)

                        If RemainingCapacity > 0 Then
                            Dim CurrentBookingController As New CCFramework.ContentManagement.CalendarModule.BookingController()

                            CurrentBookingController.Create(CurrentRegisteredUser.UserID, ScheduleId, True)

                            LoadSchedule()

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
                Case Else
                    'Response.Write("Other")
            End Select
        End Sub

        Private Sub MyBookingsButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles MyBookingsButton.Click
            If Not Membership.GetUser Is Nothing Then
                LoadMySchedule()
            End If
        End Sub

        Private Sub BackToScheduleButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles BackToScheduleButton.Click
            LoadSchedule()
        End Sub

        Protected Function CheckSignedUp(ByVal ScheduleId As Integer, ByVal ClassName As String) As String
            If Not Membership.GetUser Is Nothing Then
                Dim UserName As String = Membership.GetUser.UserName
                Dim CurrentRegisteredUser As RegisteredUser = New CCFramework.Core.RegisteredUserController().GetByUserName(UserName)

                If CCFramework.ContentManagement.CalendarModule.BookingController.CheckIfUserIsBooked(ScheduleId, CurrentRegisteredUser.UserID) Then
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
End NameSpace