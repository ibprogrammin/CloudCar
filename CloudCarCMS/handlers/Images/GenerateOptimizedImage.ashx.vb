Imports System.Web
Imports System.Web.Services
'Imports Geeks.ImageOptimizer.API

Public Class GenerateOptimizedImage
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        'context.Response.ContentType = "text/plain"
        'context.Response.Write("Hello World!")

        Dim ImageID As Integer = Integer.Parse(context.Request.QueryString("imageId"))

        'Try
        If Not IsNothing(ImageID) Then
            Dim productImage = CCFramework.Core.PictureController.GetPicture(ImageID)

            Dim contentType As String = productImage.PictureContentType
            Dim contentLength As Integer = productImage.PictureContentLength
            Dim data As Byte() = productImage.PictureData.ToArray

            Dim mem_stream As New System.IO.MemoryStream(data, 0, contentLength)
            Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(mem_stream)
            Dim output_stream As New System.IO.MemoryStream

            If Not context.Request.QueryString("maxSize") Is Nothing Then
                Dim MaxSize As System.Web.UI.WebControls.Unit = New Web.UI.WebControls.Unit(context.Request.QueryString("maxSize"))

                Select Case MaxSize.Type
                    Case UnitType.Pixel
                        If MaxSize.Value < image.Height And MaxSize.Value < image.Width Then
                            Dim width As Integer
                            Dim height As Integer
                            If image.Height >= image.Width Then
                                height = CInt(MaxSize.Value)
                                width = CInt(image.Width * (height / image.Height))
                            Else
                                width = CInt(MaxSize.Value)
                                height = CInt(image.Height * (width / image.Width))
                            End If

                            image.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)
                            image.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)

                            image = image.GetThumbnailImage(width, height, NullImage, System.IntPtr.Zero)
                        End If
                    Case UnitType.Percentage
                        Dim width As Integer = CInt(image.Width * MaxSize.Value)
                        Dim height As Integer = CInt(image.Height * MaxSize.Value)

                        image.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)
                        image.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)

                        image = image.GetThumbnailImage(width, height, NullImage, System.IntPtr.Zero)
                    Case Else
                End Select
            ElseIf Not context.Request.QueryString("maxWidth") Is Nothing Then
                Dim MaxSize As System.Web.UI.WebControls.Unit = New Web.UI.WebControls.Unit(context.Request.QueryString("maxSize"))

                Select Case MaxSize.Type
                    Case UnitType.Pixel
                        If MaxSize.Value < image.Height And MaxSize.Value < image.Width Then
                            Dim width As Integer
                            Dim height As Integer

                            width = CInt(MaxSize.Value)
                            height = CInt(image.Height * (width / image.Width))

                            image.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)
                            image.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)

                            image = image.GetThumbnailImage(width, height, NullImage, System.IntPtr.Zero)
                        End If
                    Case UnitType.Percentage
                        Dim width As Integer = CInt(image.Width * MaxSize.Value)
                        Dim height As Integer = CInt(image.Height * MaxSize.Value)

                        image.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)
                        image.RotateFlip(Drawing.RotateFlipType.Rotate180FlipNone)

                        image = image.GetThumbnailImage(width, height, NullImage, System.IntPtr.Zero)
                    Case Else
                End Select

            End If

            'Dim optimizer As New ImageOptimizer

            'image = optimizer.Optimize(image)

            Select Case contentType
                Case "image/pjpeg"
                    image.Save(output_stream, System.Drawing.Imaging.ImageFormat.Jpeg)
                Case "image/jpeg"
                    image.Save(output_stream, System.Drawing.Imaging.ImageFormat.Jpeg)
                Case "image/png"
                    image.Save(output_stream, System.Drawing.Imaging.ImageFormat.Png)
                Case "image/x-png"
                    image.Save(output_stream, System.Drawing.Imaging.ImageFormat.Png)
                Case "image/gif"
                    image.Save(output_stream, System.Drawing.Imaging.ImageFormat.Gif)
                Case "image/bmp"
                    image.Save(output_stream, System.Drawing.Imaging.ImageFormat.Bmp)
                Case "image/tiff"
                    image.Save(output_stream, System.Drawing.Imaging.ImageFormat.Tiff)
                Case "image/tif"
                    image.Save(output_stream, System.Drawing.Imaging.ImageFormat.Tiff)
                Case Else
                    image.Save(output_stream, System.Drawing.Imaging.ImageFormat.Jpeg)
            End Select

            Dim thumbnail_data(Convert.ToInt32(output_stream.Length)) As Byte

            output_stream.Position = 0
            output_stream.Read(thumbnail_data, 0, Convert.ToInt32(output_stream.Length))
            output_stream.Close()

            context.Response.Clear()
            context.Response.ContentType = contentType
            context.Response.BinaryWrite(thumbnail_data)

            'Me.Response.AppendHeader("Content-Type", contentType)
            'Me.Response.AppendHeader("Content-Length", output_stream.Length.ToString)
            'HttpResponse Response = HttpContext.Current.Response

            If IsGZipSupported() Then
                Dim AcceptEncoding As String = context.Request.Headers("Accept-Encoding")

                If AcceptEncoding.Contains("deflate") Then
                    context.Response.Filter = New System.IO.Compression.DeflateStream(context.Response.Filter, System.IO.Compression.CompressionMode.Compress)
                    context.Response.AppendHeader("Content-Encoding", "deflate")
                Else
                    context.Response.Filter = New System.IO.Compression.GZipStream(context.Response.Filter, System.IO.Compression.CompressionMode.Compress)
                    context.Response.AppendHeader("Content-Encoding", "gzip")
                End If
            End If

            '' Allow proxy servers to cache encoded and unencoded versions separately
            context.Response.AppendHeader("Vary", "Content-Encoding")

            image.Dispose()
            mem_stream.Dispose()
            output_stream.Dispose()

            image = Nothing
            mem_stream = Nothing
            output_stream = Nothing
        End If

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Function NullImage() As System.Drawing.Image.GetThumbnailImageAbort
        Return Nothing
    End Function

    Public Shared Function IsGZipSupported() As Boolean
        Dim AcceptEncoding As String = HttpContext.Current.Request.Headers("Accept-Encoding")

        If Not String.IsNullOrEmpty(AcceptEncoding) Then
            If AcceptEncoding.Contains("gzip") Or AcceptEncoding.Contains("deflate") Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

End Class