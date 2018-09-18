Imports System.Net.Mail
Imports CloudCar.CCFramework.Model

Namespace CCContentManagement.ClientForms

    Partial Public Class SignUpForm
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            'If Not Session("ScheduleId") Is Nothing Then
            '    Dim ScheduleId As Integer = Integer.Parse(Session("ScheduleId"))

            '    Dim CurrentScheduleController As New CalendarModule.ScheduleController
            '    Dim CurrentSchedule As Schedule = CurrentScheduleController.GetElement(ScheduleId)

            '    lblStatus.Text = CurrentSchedule.Program.Name & " - " & CurrentSchedule.BookingDate.ToString("dddd MMM dd, yyyy h:mm tt")
            '    lblStatus.Visible = True
            'End If
        End Sub

        Private Sub SubmitButtonClick(ByVal Sender As System.Object, ByVal E As EventArgs) Handles btnSubmit.Click
            Dim CurrentQuoteMessage As String

            CurrentQuoteMessage = "<html><head><title>A Quote From " & FirstNameTextBox.Text & " " & LastNameTextBox.Text & "</title></head><body>"
            CurrentQuoteMessage &= "Name: " & FirstNameTextBox.Text & " " & LastNameTextBox.Text & "<br>"
            CurrentQuoteMessage &= "Email: " & EmailTextBox.Text & "<br>"
            CurrentQuoteMessage &= "Phone (home): " & PhoneTextBox.Text & "<br>"

            If Not Session("ScheduleId") Is Nothing Then
                Dim ScheduleId As Integer = CInt(Session("ScheduleId"))

                Dim CurrentScheduleController As New CCFramework.ContentManagement.CalendarModule.ScheduleController
                Dim CurrentSchedule As Schedule = CurrentScheduleController.GetElement(ScheduleId)
                CurrentQuoteMessage &= "Class: " & CurrentSchedule.Program.Name & " - " & CurrentSchedule.BookingDate.ToString("dddd MMM dd, yyyy h:mm tt") & "<br>"
            End If

            CurrentQuoteMessage &= "</body></html>"

            SendMail(CurrentQuoteMessage, "Non Member Sign Up by " & FirstNameTextBox.Text & " " & LastNameTextBox.Text)

            lblStatus.Visible = True
            ClearForm()
        End Sub

        Private Sub ResetButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnReset.Click
            ClearForm()
        End Sub

        Private Sub SendMail(ByVal Message As String, ByVal Subject As String)
            Dim CurrentEmailMessage As MailMessage
            CurrentEmailMessage = New MailMessage(New MailAddress(CCFramework.Core.Settings.AdminEmail), New MailAddress(CCFramework.Core.Settings.AdminEmail))

            CurrentEmailMessage.Subject = Subject
            CurrentEmailMessage.BodyEncoding = Encoding.UTF8
            CurrentEmailMessage.IsBodyHtml = True
            CurrentEmailMessage.Body = Message

            Try
                Dim CurrentClient As SmtpClient = New SmtpClient(CCFramework.Core.Settings.SMTPHost)
                CurrentClient.Send(CurrentEmailMessage)

                lblStatus.Text = "Sign up request successfully sent!"
            Catch Ex As Exception
                lblStatus.Text = "There was an error sending the request"
            End Try

            lblStatus.Visible = True
        End Sub

        Private Sub ClearForm()
            FirstNameTextBox.Text = ""
            LastNameTextBox.Text = ""
            PhoneTextBox.Text = ""
            EmailTextBox.Text = ""
        End Sub

    End Class
End NameSpace