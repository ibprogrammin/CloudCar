Imports CloudCar.CCFramework.Model

Namespace CCAdmin.ContentManagement.CalendarModule

    Partial Public Class ScheduleSignups
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadSchedule()
            End If
        End Sub

        Private Sub LoadSchedule()
            Dim ScheduleId As Integer = Integer.Parse(Request.QueryString("ScheduleId"))

            If Not ScheduleId = Nothing Then
                Dim CurrentScheduleController As New CCFramework.ContentManagement.CalendarModule.ScheduleController

                Dim CurrentSchedule As Schedule = CurrentScheduleController.GetElement(ScheduleId)

                ScheduleIdHiddenField.Value = CurrentSchedule.Id.ToString
                ProgramLabel.Text = CurrentSchedule.Program.Name
                DateLabel.Text = CurrentSchedule.BookingDate.ToString("MMMM d, yyyy")
                TimeLabel.Text = CurrentSchedule.BookingDate.ToString("%h:mm tt")
                DurationLabel.Text = CurrentSchedule.Duration & " Minutes"
                Capacity.Text = CurrentSchedule.Capacity.ToString

                SignUpRepeater.DataSource = CCFramework.ContentManagement.CalendarModule.ScheduleController.GetSignedUpUsers(ScheduleId)
                SignUpRepeater.DataBind()
            End If
        End Sub

    End Class
End Namespace