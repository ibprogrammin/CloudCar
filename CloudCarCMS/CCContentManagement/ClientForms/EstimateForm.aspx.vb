Imports System.Net.Mail

Namespace CCContentManagement.ClientForms

    Partial Public Class EstimateForm
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load

        End Sub

        Private Sub SubmitButtonClick(ByVal Sender As System.Object, ByVal E As EventArgs) Handles btnSubmit.Click
            Dim CurrentQuoteMessage As String

            CurrentQuoteMessage = "<html><head><title>A Quote From " & FirstNameTextBox.Text & " " & LastNameTextBox.Text & "</title></head><body>"
            CurrentQuoteMessage &= "Name: " & FirstNameTextBox.Text & " " & LastNameTextBox.Text & "<br>"
            CurrentQuoteMessage &= "Email: " & EmailTextBox.Text & "<br>"
            CurrentQuoteMessage &= "Team Name: " & TeamNameTextBox.Text & "<br>"
            CurrentQuoteMessage &= "Sport: " & SportTextBox.Text & "<br>"
            CurrentQuoteMessage &= "Phone (home): " & PhoneTextBox.Text & "<br>"
            CurrentQuoteMessage &= "Phone (cell): " & CellPhoneTextBox.Text & "<br>"
            CurrentQuoteMessage &= "Age of Participants: " & AgeOfParticipantsTextBox.Text & "<br>"
            CurrentQuoteMessage &= "Number of Participants: " & NumberOfParticipantsTextBox.Text & "<br>"
            CurrentQuoteMessage &= "</body></html>"

            'lblStatus.Text &= CurrentQuoteMessage
            SendMail(CurrentQuoteMessage, "Team Training Quote message from " & FirstNameTextBox.Text & " " & LastNameTextBox.Text)

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

                lblStatus.Text = "Quote request successfully sent!"
            Catch Ex As Exception
                lblStatus.Text = "There was an error sending the quote"
            End Try

            lblStatus.Visible = True
        End Sub

        Private Sub ClearForm()
            FirstNameTextBox.Text = ""
            LastNameTextBox.Text = ""
            PhoneTextBox.Text = ""
            EmailTextBox.Text = ""
            CellPhoneTextBox.Text = ""
            TeamNameTextBox.Text = ""
            SportTextBox.Text = ""
            AgeOfParticipantsTextBox.Text = ""
            NumberOfParticipantsTextBox.Text = ""
        End Sub

    End Class
End NameSpace