Namespace CCContentManagement.ClientForms
    Public Partial Class GrantsForm
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load

        End Sub

        Private Sub SubmitButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles SubmitButton.Click
            Dim Name As String = NameTextBox.Text
            Dim Email As String = EmailTextBox.Text
            Dim Details As String = DetailsTextBox.Text

            If Not GrantApplicationFileUpload.PostedFile Is Nothing Then
                Dim CurrentApplicationFile As HttpPostedFile = GrantApplicationFileUpload.PostedFile

                If CurrentApplicationFile.ContentLength > 0 Then
                    Dim CurrentFileName As String
                    Dim FileId As Integer

                    CurrentFileName = CurrentApplicationFile.FileName
                    CurrentApplicationFile.SaveAs(Server.MapPath(CCFramework.Core.Settings.FileUploadPath & CurrentFileName))

                    FileId = CCFramework.ContentManagement.FileUploadController.Create(CurrentFileName, CCFramework.Core.Settings.FileUploadPath, Title, Details, False)

                    Dim CurrentBodyText As New StringBuilder

                    CurrentBodyText.Append(String.Format("You have a new application from {0}<br />", Name))
                    CurrentBodyText.Append(String.Format("Their email address is <a href=""mailto:{0}"">{0}</a><br />", Email))
                    CurrentBodyText.Append(String.Format("They left the following message: <br /> {0}<br />", Details))

                    Dim AdministratorEmail As New Net.Mail.MailAddress(CCFramework.Core.Settings.AdminEmail)
                    Dim CurrentAttachment As New Net.Mail.Attachment(HttpContext.Current.Server.MapPath(CCFramework.Core.Settings.FileUploadPath & CurrentFileName))

                    Dim AttachmentList As New List(Of Net.Mail.Attachment)
                    AttachmentList.Add(CurrentAttachment)

                    CCFramework.Core.SendEmails.Send(AdministratorEmail, String.Format("New Grant Application Submitted by {0}", Name), CurrentBodyText.ToString, AttachmentList)

                    'To delete file from the server
                    'IO.File.Delete(HttpContext.Current.Server.MapPath(CurrentFileName))
                    'SMContentManagement.FileUploadController.Delete(FileId)
                End If
            End If

        End Sub

    End Class
End NameSpace