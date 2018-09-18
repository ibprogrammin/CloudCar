Imports CloudCar.CCFramework.ContentManagement.EventsModule
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework

Namespace CCAdmin.ContentManagement.EventModule

    Partial Public Class Events
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load

        End Sub

        Private Sub AddEventbuttonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles AddEventButton.Command
            AddEventPlaceHolder.Visible = True
            EventsGridView.Visible = False
            AddEventButton.Visible = False
        End Sub

        Private Sub CancelButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles CancelButton.Command
            AddEventPlaceHolder.Visible = False
            EventsGridView.Visible = True
            AddEventButton.Visible = True
            StatusMessageLabel.Visible = False

            EventIdHiddenField.Value = ""
        End Sub

        Private Sub SubmitButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles SubmitButton.Command
            Dim CurrentEventTitle As String = TitleTextBox.Text
            Dim CurrentEventTime As String = TimeTextBox.Text
            Dim CurrentEventLocation As String = LocationTextBox.Text
            Dim CurrentEventDetails As String = DetailsTextArea.InnerText
            Dim CurrentEventDate As DateTime = DateTime.Parse(EventDateTextBox.Text)
            Dim CurrentEventPermalink As String = PermalinkTextBox.Text

            If Not EventIdHiddenField.Value = String.Empty Then
                Dim CurrentEventId As Integer = Integer.Parse(EventIdHiddenField.Value)

                Dim CurrentImageId As Integer

                If Not ImageFileUpload.PostedFile.FileName = "" Then
                    Dim CurrentFileName As String = ImageFileUpload.FileName
                    Dim CurrentImageType As String = ImageFileUpload.PostedFile.ContentType
                    Dim CurrentImageLength As Integer = CInt(ImageFileUpload.PostedFile.InputStream.Length)
                    Dim CurrentImageData(CurrentImageLength) As Byte

                    ImageFileUpload.PostedFile.InputStream.Read(CurrentImageData, 0, CurrentImageLength)

                    CurrentImageId = PictureController.CreatePicture(CurrentFileName, CurrentImageType, CurrentImageLength, CurrentImageData)

                    EventsController.Update(CurrentEventId, CurrentEventTitle, CurrentEventPermalink, CurrentEventTime, CurrentEventLocation, CurrentEventDetails, CurrentImageId, CurrentEventDate)
                Else
                    EventsController.Update(CurrentEventId, CurrentEventTitle, CurrentEventPermalink, CurrentEventTime, CurrentEventLocation, CurrentEventDetails, CurrentEventDate)
                End If

                StatusMessageLabel.Text = String.Format("Event {0} successfully updated!", CurrentEventId)
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

                    EventsController.Create(CurrentEventTitle, CurrentEventPermalink, CurrentEventTime, CurrentEventLocation, CurrentEventDetails, CurrentImageId, CurrentEventDate, True)
                Else
                    EventsController.Create(CurrentEventTitle, CurrentEventPermalink, CurrentEventTime, CurrentEventLocation, CurrentEventDetails, CurrentEventDate, True)
                End If

                StatusMessageLabel.Text = "Event successfully added!"
                StatusMessageLabel.Visible = True
            End If

            EventsGridView.DataBind()

            AddEventPlaceHolder.Visible = False
            EventsGridView.Visible = True
            AddEventButton.Visible = True

            EventIdHiddenField.Value = ""
        End Sub

        Protected Sub EventsGridViewRowCommand(ByVal Sender As Object, ByVal Args As GridViewCommandEventArgs) Handles EventsGridView.RowCommand
            Dim CurrentEventId As Integer = Integer.Parse(Args.CommandArgument.ToString)

            Select Case Args.CommandName
                Case "Delete"
                    If EventsController.Delete(CurrentEventId) Then
                        StatusMessageLabel.Text = String.Format("Event: {0} has been successfully deleted.", CurrentEventId)
                    End If

                    EventsGridView.DataBind()
                Case "Select"
                    Dim CurrentEvent As Model.Event = EventsController.GetItem(CurrentEventId)

                    With CurrentEvent
                        EventIdHiddenField.Value = .Id.ToString
                        TitleTextBox.Text = .Title
                        TimeTextBox.Text = .Time
                        LocationTextBox.Text = .Location
                        DetailsTextArea.InnerText = .Details
                        EventDateTextBox.Text = .EventDate.ToString

                        If Not .ImageId Is Nothing AndAlso .ImageId.HasValue Then
                            ImageIdHiddenField.Value = .ImageId.ToString

                            ImageLocationLabel.Text = String.Format("/images/db/{0}/full/{1}", .ImageId, PictureController.GetPictureFilename(.ImageId.Value))
                            ImageLocationLabel.Visible = True
                        End If
                    End With

                    EventsGridView.Visible = False
                    AddEventPlaceHolder.Visible = True
                    AddEventButton.Visible = False
                Case "Approve"
                    EventsController.ApproveEvent(CurrentEventId)

                    EventsGridView.DataBind()
            End Select
        End Sub

    End Class

End Namespace