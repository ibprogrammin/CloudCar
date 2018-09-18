Imports CloudCar.CCFramework.Model

Namespace CCControls

    Partial Public Class UploadPicture
        Inherits UserControl


        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load

        End Sub

        Public Property MaxSize() As Integer
            Get
                Return Integer.Parse(hfMaxSize.Value)
            End Get
            Set(ByVal value As Integer)
                hfMaxSize.Value = value.ToString
            End Set
        End Property

        Public Property PictureID() As Integer
            Get
                Return Integer.Parse(hfPictureID.Value)
            End Get
            Set(ByVal value As Integer)
                hfPictureID.Value = value.ToString
            End Set
        End Property

        Public Sub Hide()
            mpeImageUpload.Hide()
        End Sub

        Public Sub Show()
            mpeImageUpload.Show()
        End Sub

        Private Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
            Dim db As New ContentDataContext
            Dim filename As String
            Dim extension As String
            Dim imgType As String
            Dim imgLength As Integer
            Dim arrContent As Byte()
            Dim picture As Picture

            filename = fuImage.PostedFile.FileName
            extension = filename.Substring(filename.LastIndexOf(".")).ToLower
            imgType = fuImage.PostedFile.ContentType
            imgLength = CInt(fuImage.PostedFile.InputStream.Length)
            ReDim arrContent(imgLength)
            fuImage.PostedFile.InputStream.Read(arrContent, 0, imgLength)

            If PictureID = 0 Then
                picture = New Picture
                db.Pictures.InsertOnSubmit(picture)
            Else
                picture = (From p In db.Pictures Where p.PictureID = PictureID Select p).Single
            End If

            picture.PictureContentLength = imgLength
            picture.PictureContentType = imgType
            picture.PictureData = arrContent
            picture.PictureFileName = filename

            db.SubmitChanges()

            PictureID = picture.PictureID

            picture = Nothing

            RaiseEvent ImageSaved(PictureID)

            arrContent = Nothing
            db = Nothing
            picture = Nothing
        End Sub

        Public Event ImageSaved(ByVal ImageID As Integer)
    End Class

End Namespace