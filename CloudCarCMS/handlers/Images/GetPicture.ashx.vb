Imports System.Web
Imports System.Web.Services
Imports CloudCar.CCFramework.Model

Public Class GetPicture
    Implements IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim arrContent As Byte()
        Dim thumbContent As Byte()

        If context.Request.Item("PictureID") Is Nothing Then
            context.Response.Redirect("/images/No-photo-Available.jpg")
        Else
            Try
                Dim PictureID As Integer
                Dim db As New ContentDataContext
                Dim picture As Picture
                Dim conType As String
                Dim size As Integer
                Dim mStream As System.IO.MemoryStream
                Dim thumbnail As System.Drawing.Image
                Dim img As System.Drawing.Bitmap
                Dim ostream As System.IO.MemoryStream

                If context.Request.Item("Size") Is Nothing Then
                    size = 0
                Else
                    size = CInt(context.Request.Item("Size"))
                End If

                PictureID = CInt(context.Request.Item("PictureID"))
                picture = (From p In db.Pictures Where p.PictureID = PictureID Select p).Single

                arrContent = picture.PictureData.ToArray
                conType = picture.PictureContentType
                mStream = New System.IO.MemoryStream(arrContent, 0, picture.PictureContentLength)

                img = New System.Drawing.Bitmap(mStream)

                If context.Request.Item("Height") Is Nothing Then
                    If Not context.Request.Item("Width") Is Nothing Then
                        Dim width As Integer
                        Dim height As Integer

                        width = CInt(context.Request.Item("Width"))
                        height = CInt(img.Height * (width / img.Width))

                        img.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)
                        img.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)
                        thumbnail = img.GetThumbnailImage(width, height, NullImage, New System.IntPtr)

                        ostream = New System.IO.MemoryStream
                        thumbnail.Save(ostream, System.Drawing.Imaging.ImageFormat.Png)
                        ReDim thumbContent(CInt(ostream.Length))
                        ostream.Position = 0
                        ostream.Read(thumbContent, 0, Convert.ToInt32(ostream.Length))
                        ostream.Close()
                        context.Response.Clear()
                        context.Response.ContentType = "image/png"
                        context.Response.Cache.SetCacheability(HttpCacheability.Public)
                        context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(15))
                        context.Response.Cache.SetMaxAge(New TimeSpan(0, 15, 0))
                        context.Response.BinaryWrite(thumbContent)
                        thumbnail.Dispose()
                    ElseIf (img.Width <= size And img.Height <= size) Or size = 0 Then
                        ostream = New System.IO.MemoryStream
                        img.Save(ostream, System.Drawing.Imaging.ImageFormat.Png)

                        ReDim thumbContent(CInt(ostream.Length))
                        ostream.Position = 0
                        ostream.Read(thumbContent, 0, Convert.ToInt32(ostream.Length))
                        ostream.Close()

                        context.Response.Clear()
                        context.Response.ContentType = "image/png"
                        context.Response.Cache.SetCacheability(HttpCacheability.Public)
                        context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(15))
                        context.Response.Cache.SetMaxAge(New TimeSpan(0, 15, 0))
                        context.Response.BinaryWrite(thumbContent)
                    Else
                        Dim width As Integer
                        Dim height As Integer
                        If img.Height > img.Width Then
                            height = size
                            width = CInt(img.Width * (height / img.Height))
                        Else
                            width = size
                            height = CInt(img.Height * (width / img.Width))
                        End If
                        img.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)
                        img.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)
                        thumbnail = img.GetThumbnailImage(width, height, NullImage, New System.IntPtr)

                        ostream = New System.IO.MemoryStream
                        thumbnail.Save(ostream, System.Drawing.Imaging.ImageFormat.Png)
                        ReDim thumbContent(CInt(ostream.Length))
                        ostream.Position = 0
                        ostream.Read(thumbContent, 0, Convert.ToInt32(ostream.Length))
                        ostream.Close()
                        context.Response.Clear()
                        context.Response.ContentType = "image/png"
                        context.Response.Cache.SetCacheability(HttpCacheability.Public)
                        context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(15))
                        context.Response.Cache.SetMaxAge(New TimeSpan(0, 15, 0))
                        context.Response.BinaryWrite(thumbContent)
                        thumbnail.Dispose()
                    End If
                Else
                    Dim width As Integer
                    Dim height As Integer

                    height = CInt(context.Request.Item("Height"))
                    width = CInt(img.Width * (height / img.Height))

                    img.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)
                    img.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)
                    thumbnail = img.GetThumbnailImage(width, height, NullImage, New System.IntPtr)

                    ostream = New System.IO.MemoryStream
                    thumbnail.Save(ostream, System.Drawing.Imaging.ImageFormat.Png)
                    ReDim thumbContent(CInt(ostream.Length))
                    ostream.Position = 0
                    ostream.Read(thumbContent, 0, Convert.ToInt32(ostream.Length))
                    ostream.Close()
                    context.Response.Clear()
                    context.Response.ContentType = "image/png"
                    context.Response.Cache.SetCacheability(HttpCacheability.Public)
                    context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(15))
                    context.Response.Cache.SetMaxAge(New TimeSpan(0, 15, 0))
                    context.Response.BinaryWrite(thumbContent)
                    thumbnail.Dispose()
                End If

                img.Dispose()
                img = Nothing
                thumbnail = Nothing
                ostream.Dispose()
                ostream = Nothing
                db = Nothing
                picture = Nothing
                mStream = Nothing
            Catch ex As Exception
                context.Response.Redirect("/images/No-photo-Available.jpg")
            End Try
        End If

        arrContent = Nothing
        thumbContent = Nothing
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Function NullImage() As System.Drawing.Image.GetThumbnailImageAbort
        Return Nothing
    End Function

End Class