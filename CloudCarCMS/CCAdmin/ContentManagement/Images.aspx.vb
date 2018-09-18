Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement.NewsModule
Imports CloudCar.CCFramework.ContentManagement.EventsModule

Namespace CCAdmin.ContentManagement

    Partial Public Class Images
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load

        End Sub

        Private Sub UploadImage()
            Dim pictureId As Integer

            If Not fuImage.PostedFile.FileName = "" Then
                Dim filename As String = fuImage.FileName
                Dim imgType As String = fuImage.PostedFile.ContentType
                Dim imgLength As Integer = CInt(fuImage.PostedFile.InputStream.Length)
                Dim data(imgLength) As Byte
                fuImage.PostedFile.InputStream.Read(data, 0, imgLength)

                pictureId = CCFramework.Core.PictureController.CreatePicture(filename, imgType, imgLength, data)
            Else
                pictureId = EventsController.GetItem(ID).ImageId.Value
            End If
        End Sub

        Protected Sub rptImages_ItemCommend(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptImages.ItemCommand
            If e.CommandName = "DeleteImage" Then
                Dim ID As Integer = Integer.Parse(e.CommandArgument.ToString)

                If CCFramework.Core.PictureController.DeletePicture(ID) Then
                    lblStatus.Text = "Image: " & ID & " has been successfully deleted."
                    lblStatus.Visible = True
                End If

                rptImages.DataBind()
            End If
        End Sub

        Private Sub btnUpload_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnUpload.Command
            UploadImage()

            rptImages.DataBind()
        End Sub

        Protected Function hasRelationship(ByVal ImageId As Integer) As Boolean
            Dim db As New ContentDataContext

            Dim pic = (From p In db.Pictures Where p.PictureID = ImageId Select p).FirstOrDefault

            If pic.Links.Count > 0 Or pic.Testimonials.Count > 0 Or pic.Events.Count > 0 Then
                Return False
            Else
                Return True
            End If
        End Function

    End Class
End Namespace