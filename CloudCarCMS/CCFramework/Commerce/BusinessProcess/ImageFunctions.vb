Namespace CCFramework.Core

    Public Class ImageFunctions

        Public Shared Function UploadImage(ByRef fuImage As WebControls.FileUpload) As Integer
            Dim pictureId As Integer

            If Not fuImage.PostedFile.FileName = "" Then
                Dim filename As String = fuImage.FileName
                Dim imgType As String = fuImage.PostedFile.ContentType
                Dim imgLength As Integer = CInt(fuImage.PostedFile.InputStream.Length)
                Dim data(imgLength) As Byte

                fuImage.PostedFile.InputStream.Read(data, 0, imgLength)

                pictureId = PictureController.CreatePicture(filename, imgType, imgLength, data)
            End If

            Return pictureId
        End Function

        Public Shared Function UploadImage(ByRef fuImage As HttpPostedFile) As Integer
            Dim pictureId As Integer

            If Not fuImage.FileName = "" Then
                Dim filename As String = fuImage.FileName
                Dim imgType As String = fuImage.ContentType
                Dim imgLength As Integer = CInt(fuImage.InputStream.Length)
                Dim data(imgLength) As Byte

                fuImage.InputStream.Read(data, 0, imgLength)

                pictureId = PictureController.CreatePicture(filename, imgType, imgLength, data)
            End If

            Return pictureId
        End Function

    End Class

End Namespace