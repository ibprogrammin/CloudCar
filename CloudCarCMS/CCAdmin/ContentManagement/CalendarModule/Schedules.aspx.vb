Imports CloudCar.CCFramework.Model

Namespace CCAdmin.ContentManagement.CalendarModule

    Partial Public Class Schedules
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadDropDownSelections()
                LoadSchedules()
            End If
        End Sub

        Private Const CountOfYears As Integer = 6
        Private Sub LoadDropDownSelections()
            Dim CurrentProgramController As New CCFramework.ContentManagement.CalendarModule.ProgramController()

            ProgramsDropDownList.DataSource = CurrentProgramController.GetElements()
            ProgramsDropDownList.DataBind()

            For i As Integer = -CountOfYears To CountOfYears - 1
                Dim SetYear As Integer = Date.Now.Year + i

                YearDropDownList.Items.Add(New ListItem(SetYear.ToString, SetYear.ToString))
            Next

            For j As Integer = 1 To 12
                'Dim CurrentMonth As Integer = ((Date.Now.Month + (j - 1)) Mod 12) + 1

                MonthDropDownList.Items.Add(New ListItem(MonthName(j), j.ToString))
            Next

            Dim CurrentMonth As Integer = Date.Now.Month
            Dim CurrentYear As Integer = Date.Now.Year

            MonthDropDownList.SelectedValue = CurrentMonth.ToString
            YearDropDownList.SelectedValue = CurrentYear.ToString

        End Sub

        Protected Sub FilterScheduleList(ByVal Sender As Object, ByVal E As EventArgs)
            LoadSchedules()
        End Sub

        Private Sub LoadSchedules()
            Dim CurrentProgram As Integer = Integer.Parse(ProgramsDropDownList.SelectedValue)
            Dim CurrentMonth As Integer = Integer.Parse(MonthDropDownList.SelectedValue)
            Dim CurrentYear As Integer = Integer.Parse(YearDropDownList.SelectedValue)

            gvSchedules.DataSource = CCFramework.ContentManagement.CalendarModule.ScheduleController.GetSchedulesByMonthYearProgram(CurrentMonth, CurrentYear, CurrentProgram)
            'CurrentSchedulesController.GetElements()
            gvSchedules.DataBind()
        End Sub

        Private Sub SchedulesGridViewPageIndexChanged(ByVal Sender As Object, ByVal E As DataGridPageChangedEventArgs) Handles gvSchedules.PageIndexChanged
            gvSchedules.CurrentPageIndex = E.NewPageIndex

            LoadSchedules()
        End Sub

        Private Sub AddScheduleButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles AddScheduleButton.Click
            ClearForm()

            AddSchedulePlaceHolder.Visible = True
            RepeatScheduleFields.Visible = True
            LoadProgramsDropDownList()
        End Sub

        Private Sub LoadProgramsDropDownList()
            Dim CurrentProgramController As New CCFramework.ContentManagement.CalendarModule.ProgramController

            ProgramDropDown.DataSource = CurrentProgramController.GetElements()
            ProgramDropDown.DataBind()
        End Sub

        Private Sub SaveButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles SaveButton.Click
            If Page.IsValid Then
                Dim ProgramId As Integer = Integer.Parse(ProgramDropDown.SelectedValue)
                Dim ClassDate As DateTime = DateTime.Parse(DateTextBox.Text & " " & HourDropDown.SelectedValue & ":" & MinuteDropDown.SelectedValue & " " & TimeOfDayDropDown.SelectedValue)
                Dim ClassCapacity As Integer = Integer.Parse(CapacityTextBox.Text)
                Dim ClassFree As Boolean = False 'Boolean.Parse(FreeClassDropDown.SelectedValue)
                Dim ClassDuration As Integer = Integer.Parse(DurationDropDown.SelectedValue)
                Dim CurrentScheduleController As New CCFramework.ContentManagement.CalendarModule.ScheduleController

                Dim ScheduleId As Integer

                If Integer.TryParse(ScheduleIdHiddenField.Value, ScheduleId) Then
                    CurrentScheduleController.Update(ScheduleId, ProgramId, ClassCapacity, ClassFree, ClassDate, ClassDuration)
                Else
                    If RepeatList.SelectedValue = "Weekly" Then
                        Dim CurrentDate As DateTime = ClassDate

                        For i As Integer = 0 To (Integer.Parse(FrequencyTextBox.Text) - 1)
                            CurrentScheduleController.Create(ProgramId, ClassCapacity, ClassFree, CurrentDate, ClassDuration)

                            CurrentDate = CurrentDate.AddDays(7)
                        Next
                    Else
                        CurrentScheduleController.Create(ProgramId, ClassCapacity, ClassFree, ClassDate, ClassDuration)
                    End If
                End If

                lblStatus.Text = "The schedule has been saved successfully."
                lblStatus.Visible = True

                LoadSchedules()
            End If
        End Sub

        Protected Sub SchedulesGridEditCommand(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs) Handles gvSchedules.EditCommand
            Dim ScheduleId As Integer = Integer.Parse(gvSchedules.DataKeys(E.Item.ItemIndex).ToString)

            If Not ScheduleId = Nothing Then
                LoadProgramsDropDownList()

                AddSchedulePlaceHolder.Visible = True

                Dim CurrentScheduleController As New CCFramework.ContentManagement.CalendarModule.ScheduleController

                Dim CurrentSchedule As Schedule = CurrentScheduleController.GetElement(ScheduleId)

                ScheduleIdHiddenField.Value = CurrentSchedule.Id.ToString
                ProgramDropDown.SelectedValue = CurrentSchedule.ProgramId.ToString
                DateTextBox.Text = CurrentSchedule.BookingDate.ToString("MMMM d, yyyy")
                HourDropDown.SelectedValue = CurrentSchedule.BookingDate.ToString("%h")
                MinuteDropDown.SelectedValue = CurrentSchedule.BookingDate.ToString("mm")
                TimeOfDayDropDown.SelectedValue = CurrentSchedule.BookingDate.ToString("tt")
                DurationDropDown.SelectedValue = CurrentSchedule.Duration.ToString
                FreeClassDropDown.SelectedValue = CurrentSchedule.Free.ToString
                CapacityTextBox.Text = CurrentSchedule.Capacity.ToString

                SignUpRepeater.DataSource = CCFramework.ContentManagement.CalendarModule.ScheduleController.GetSignedUpUsers(ScheduleId)
                SignUpRepeater.DataBind()
            End If
        End Sub

        Protected Sub SchedulesGridCancelCommand(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs) Handles gvSchedules.CancelCommand
            Dim ScheduleId As Integer = Integer.Parse(gvSchedules.DataKeys(E.Item.ItemIndex).ToString)
            Response.Write(ScheduleId.ToString())
            If CCFramework.ContentManagement.CalendarModule.ScheduleController.CancelClass(ScheduleId) Then
                lblStatus.Text = String.Format("The class has been successfully canceled.")
                lblStatus.Visible = True

                LoadSchedules()
            Else
                lblStatus.Text = String.Format("There was a problem canceling the current class.")
                lblStatus.Visible = True
            End If
        End Sub

        Protected Sub CancelClassButtonCommand(ByVal Sender As Object, ByVal E As CommandEventArgs)
            Dim ScheduleId As Integer = Integer.Parse(E.CommandArgument.ToString)

            If CCFramework.ContentManagement.CalendarModule.ScheduleController.CancelClass(ScheduleId) Then
                lblStatus.Text = String.Format("The class has been successfully canceled.")
                lblStatus.Visible = True

                LoadSchedules()
            Else
                lblStatus.Text = String.Format("There was a problem canceling the current class.")
                lblStatus.Visible = True
            End If
        End Sub

        Protected Sub SchedulesGridDeleteCommand(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs) Handles gvSchedules.DeleteCommand
            Dim ScheduleId As Integer = Integer.Parse(gvSchedules.DataKeys(E.Item.ItemIndex).ToString)

            If Not ScheduleId = Nothing Then
                Dim CurrentScheduleController As New CCFramework.ContentManagement.CalendarModule.ScheduleController

                CurrentScheduleController.Delete(ScheduleId)

                lblStatus.Text = "The schedule was successfully deleted from the database."
                lblStatus.Visible = True

                LoadSchedules()
            End If
        End Sub

        Private Sub ClearButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles ClearButton.Click
            ClearForm()
        End Sub

        Private Sub ClearForm()
            ScheduleIdHiddenField.Value = Nothing
            ProgramDropDown.SelectedValue = Nothing
            DateTextBox.Text = ""
            HourDropDown.SelectedValue = Nothing
            MinuteDropDown.SelectedValue = Nothing
            TimeOfDayDropDown.SelectedValue = Nothing
            DurationDropDown.SelectedValue = Nothing
            FreeClassDropDown.SelectedValue = Nothing
            CapacityTextBox.Text = "20"
            SignUpRepeater.DataSource = Nothing
        End Sub

    End Class

End Namespace