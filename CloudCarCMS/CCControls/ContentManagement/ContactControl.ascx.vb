Imports CloudCar.CCFramework.ContentManagement
Imports CloudCar.CCFramework.Core

Namespace CCControls.ContentManagement

    Partial Public Class ContactControl
        Inherits UserControl

        Public Sub SubmitButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles btnSubmit.Command
            SaveInquiry()
        End Sub

        Public Sub SaveInquiry()
            Dim Answer As Integer

            If Not Integer.TryParse(QuestionTextBox.Text, Answer) Then
                Answer = 0
            End If

            If Page.IsValid AndAlso (Answer = 9 OrElse QuestionTextBox.Text.ToLower = "nine") Then
                Dim CurrentInquiryName As String = ContactName.Text
                Dim CurrentInquiryEmail As String = ContactEmail.Text
                Dim CurrentInquiryMessage As String = ContactQuestion.Text

                Dim InquiryId As Integer = SalesInquiryController.Create(CurrentInquiryName, CurrentInquiryEmail, CurrentInquiryMessage, Date.Now, False)

                If Settings.SendNoticeEmails Then
                    Dim Subject As String = "There has been a new inquiry on your web site."

                    Dim Message As New StringBuilder

                    Message.Append("There has been a new inquiry on your web site. Find the message attached below or you can login to your administrative dashboard to view.")
                    Message.Append(String.Format("The following message has been posted through your web site: {0}" & vbNewLine, CurrentInquiryMessage))
                    Message.Append(String.Format("The message was posted by: {0} - <a href=""mailto:{1}"">{1}</a>", CurrentInquiryName, CurrentInquiryEmail))

                    SendEmails.SendNoticeMessage(Settings.AdminEmail, Subject, Message.ToString())
                End If

                Response.Redirect("/Home/thank-you.html")
            End If
        End Sub

    End Class

End Namespace