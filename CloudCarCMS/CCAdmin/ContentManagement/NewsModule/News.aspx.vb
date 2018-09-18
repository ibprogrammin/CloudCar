Imports CloudCar.CCFramework.ContentManagement.EventsModule
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework
Imports CloudCar.CCFramework.ContentManagement.NewsModule

Namespace CCAdmin.ContentManagement.NewsModule

    Partial Public Class News
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load

        End Sub

        Private Sub AddNewsbuttonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles AddNewsButton.Command
            AddNewsPlaceHolder.Visible = True
            NewsGridView.Visible = False
            AddNewsButton.Visible = False
        End Sub

        Private Sub CancelButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles CancelButton.Command
            AddNewsPlaceHolder.Visible = False
            NewsGridView.Visible = True
            AddNewsButton.Visible = True
            StatusMessageLabel.Visible = False

            NewsIdHiddenField.Value = ""
        End Sub

        Private Sub SubmitButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles SubmitButton.Command
            Dim CurrentNewsTitle As String = TitleTextBox.Text
            Dim CurrentNewsSubTitle As String = SubTitleTextBox.Text
            Dim CurrentNewsSummary As String = SummaryTextBox.Text
            Dim CurrentNewsDetails As String = DetailsTextArea.InnerText
            Dim CurrentNewsPublishDate As DateTime = DateTime.Parse(PublishDateTextBox.Text)
            Dim CurrentNewsPermalink As String = PermalinkTextBox.Text

            If Not NewsIdHiddenField.Value = "" Then
                Dim CurrentEventId As Integer = Integer.Parse(NewsIdHiddenField.Value)

                Dim CurrentImageId As Integer

                If Not ImageFileUpload.PostedFile.FileName = "" Then
                    Dim CurrentFileName As String = ImageFileUpload.FileName
                    Dim CurrentImageType As String = ImageFileUpload.PostedFile.ContentType
                    Dim CurrentImageLength As Integer = CInt(ImageFileUpload.PostedFile.InputStream.Length)
                    Dim CurrentImageData(CurrentImageLength) As Byte

                    ImageFileUpload.PostedFile.InputStream.Read(CurrentImageData, 0, CurrentImageLength)

                    CurrentImageId = PictureController.CreatePicture(CurrentFileName, CurrentImageType, CurrentImageLength, CurrentImageData)

                    NewsController.Update(CurrentEventId, CurrentNewsTitle, CurrentNewsSubTitle, CurrentNewsSummary, CurrentNewsPermalink, CurrentNewsDetails, CurrentImageId, CurrentNewsPublishDate)
                Else
                    NewsController.Update(CurrentEventId, CurrentNewsTitle, CurrentNewsSubTitle, CurrentNewsSummary, CurrentNewsPermalink, CurrentNewsDetails, CurrentNewsPublishDate)
                End If

                StatusMessageLabel.Text = String.Format("News {0} successfully updated!", CurrentEventId)
                StatusMessageLabel.Visible = True
            Else
                Dim CurrentImageId As Integer

                If Not ImageFileUpload.PostedFile.FileName = "" Then
                    Dim CurrentFileName As String = ImageFileUpload.FileName
                    Dim CurrentImageType As String = ImageFileUpload.PostedFile.ContentType
                    Dim CurrentImageLength As Integer = CInt(ImageFileUpload.PostedFile.InputStream.Length)
                    Dim CurrentImageData(CurrentImageLength) As Byte

                    ImageFileUpload.PostedFile.InputStream.Read(CurrentImageData, 0, CurrentImageLength)

                    CurrentImageId = PictureController.CreatePicture(CurrentFileName, CurrentImageType, CurrentImageLength, CurrentImageData)

                    NewsController.Create(CurrentNewsTitle, CurrentNewsSubTitle, CurrentNewsSummary, CurrentNewsPermalink, CurrentNewsDetails, CurrentImageId, CurrentNewsPublishDate, True)
                Else
                    NewsController.Create(CurrentNewsTitle, CurrentNewsSubTitle, CurrentNewsSummary, CurrentNewsPermalink, CurrentNewsDetails, CurrentNewsPublishDate, True)
                End If

                StatusMessageLabel.Text = String.Format("News successfully added!")
                StatusMessageLabel.Visible = True
            End If

            NewsGridView.DataBind()

            AddNewsPlaceHolder.Visible = False
            NewsGridView.Visible = True
            AddNewsButton.Visible = True

            NewsIdHiddenField.Value = ""
        End Sub

        Protected Sub NewsGridViewRowCommand(ByVal Sender As Object, ByVal Args As GridViewCommandEventArgs) Handles NewsGridView.RowCommand
            Dim CurrentEventId As Integer = Integer.Parse(Args.CommandArgument.ToString)

            Select Case Args.CommandName
                Case "Delete"
                    If NewsController.Delete(CurrentEventId) Then
                        StatusMessageLabel.Text = String.Format("News: {0} has been successfully deleted.", CurrentEventId)
                    End If

                    NewsGridView.DataBind()
                Case "Select"
                    Dim CurrentNews As Model.New = NewsController.GetItem(CurrentEventId)

                    With CurrentNews
                        NewsIdHiddenField.Value = .Id.ToString
                        TitleTextBox.Text = .Title
                        SubTitleTextBox.Text = .SubTitle
                        SummaryTextBox.Text = .Summary
                        DetailsTextArea.InnerText = .Details
                        PublishDateTextBox.Text = .PublishDate.ToString

                        If Not .ImageId.HasValue Then
                            ImageIdHiddenField.Value = .ImageId.ToString

                            ImageLocationLabel.Text = String.Format("/images/db/{0}/full/{1}", .ImageId, PictureController.GetPictureFilename(.ImageId.Value))
                            ImageLocationLabel.Visible = True
                        End If
                    End With

                    NewsGridView.Visible = False
                    AddNewsPlaceHolder.Visible = True
                    AddNewsButton.Visible = False
                Case "Approve"
                    NewsController.ApproveNews(CurrentEventId)

                    NewsGridView.DataBind()
            End Select
        End Sub

    End Class

End Namespace