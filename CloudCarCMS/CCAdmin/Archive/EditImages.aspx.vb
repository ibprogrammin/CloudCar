Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement.EventsModule
Imports CloudCar.CCFramework.Core

Partial Public Class EditImages
    Inherits Page

    Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load

    End Sub

    Private Sub UploadImage()
        Dim CurrentImageId As Integer

        If Not fuImage.PostedFile.FileName = "" Then
            Dim filename As String = fuImage.FileName
            Dim imgType As String = fuImage.PostedFile.ContentType
            Dim imgLength As Integer = fuImage.PostedFile.InputStream.Length
            Dim data(imgLength) As Byte
            fuImage.PostedFile.InputStream.Read(data, 0, imgLength)

            CurrentImageId = PictureController.CreatePicture(filename, imgType, imgLength, data)
        Else
            CurrentImageId = EventsController.GetItem(ID).ImageId
        End If
    End Sub

    Protected Sub gvImages_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvImages.RowCommand
        If e.CommandName = "DeleteImage" Then
            Dim CurrentImageId As Integer = Integer.Parse(e.CommandArgument.ToString)

            If PictureController.DeletePicture(CurrentImageId) Then
                lblStatus.Text = String.Format("Image: {0} has been successfully deleted.", CurrentImageId)
                lblStatus.Visible = True
            End If

            gvImages.DataBind()
        End If
    End Sub

    Private Sub btnUpload_Command(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles btnUpload.Command
        UploadImage()

        gvImages.DataBind()
    End Sub

    Protected Function hasRelationship(ByVal PictureID As Integer) As Boolean
        Dim db As New ContentDataContext

        Dim pic = (From p In db.Pictures Where p.PictureID = PictureID Select p).FirstOrDefault

        If pic.Links.Count > 0 Or pic.Testimonials.Count > 0 Or pic.Events.Count > 0 Then
            Return False
        Else
            Return True
        End If
    End Function

End Class