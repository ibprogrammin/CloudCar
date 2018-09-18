Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging

Namespace handlers.Images

    Partial Public Class GetImage
        Inherits RoutablePage

        Public Const TopSellerIconFile As String = "~/CCTemplates/Default/Images/Required/top.seller.2.icon.png"
        Public Const SaleIconFile As String = "~/CCTemplates/Default/Images/Required/top.seller.2.icon.png"

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            Dim CurrentImageId As Integer = Integer.Parse((From v In RequestContext.RouteData.Values Where v.Key = "id" Select New With {.id = v.Value}).SingleOrDefault.id.ToString)
            Dim CurrentFileName As String = (From v In RequestContext.RouteData.Values Where v.Key = "filename" Select New With {.filename = v.Value}).SingleOrDefault.filename.ToString
            Dim CurrentRequestedSize As Object = (From v In RequestContext.RouteData.Values Where v.Key = "size" Select New With {.size = v.Value}).SingleOrDefault.size

            Dim MaxSize As Integer
            If Not Integer.TryParse(CurrentRequestedSize.ToString, MaxSize) Then
                MaxSize = Nothing
            End If

            'Try
            If Not CurrentImageId = Nothing Then
                Dim CurrentImage As Picture = PictureController.GetPicture(CurrentImageId)

                If Not CurrentImage Is Nothing Then
                    Dim CurrentContentType As String = CurrentImage.PictureContentType
                    Dim CurrentContentLength As Integer = CurrentImage.PictureContentLength
                    Dim CurrentImageData As Byte() = CurrentImage.PictureData.ToArray

                    Dim CurrentMemoryStream As New MemoryStream(CurrentImageData, 0, CurrentContentLength)
                    Dim CurrentResponseImage As Image = Image.FromStream(CurrentMemoryStream)
                    Dim CurrentOutputStream As New MemoryStream

                    Dim CurrentWidth As Integer
                    Dim CurrentHeight As Integer

                    If Not MaxSize = Nothing Then
                        If Not CurrentResponseImage.Width = CurrentResponseImage.Height Then
                            If CurrentResponseImage.Height > CurrentResponseImage.Width Then
                                CurrentHeight = MaxSize
                                CurrentWidth = CInt(CurrentResponseImage.Width * (CurrentHeight / CurrentResponseImage.Height))
                            Else
                                CurrentWidth = MaxSize
                                CurrentHeight = CInt(CurrentResponseImage.Height * (CurrentWidth / CurrentResponseImage.Width))
                            End If

                            CurrentResponseImage.RotateFlip(RotateFlipType.Rotate180FlipNone)
                            CurrentResponseImage.RotateFlip(RotateFlipType.Rotate180FlipNone)

                            CurrentResponseImage = CurrentResponseImage.GetThumbnailImage(CurrentWidth, CurrentHeight, NullImage, IntPtr.Zero)
                        Else
                            CurrentWidth = MaxSize
                            CurrentHeight = MaxSize

                            CurrentResponseImage.RotateFlip(RotateFlipType.Rotate180FlipNone)
                            CurrentResponseImage.RotateFlip(RotateFlipType.Rotate180FlipNone)

                            CurrentResponseImage = CurrentResponseImage.GetThumbnailImage(CurrentWidth, CurrentHeight, NullImage, IntPtr.Zero)
                        End If
                    End If

                    If CurrentFileName.EndsWith(".jpg") OrElse CurrentFileName.EndsWith(".jpeg") Then
                        CurrentContentType = "image/jpeg"
                    End If

                    If Not Request.QueryString("overlay") Is Nothing Then
                        Dim CurrentOverlayImage As Image

                        Select Case Request.QueryString("overlay")
                            Case "ts"
                                CurrentOverlayImage = Image.FromFile(Server.MapPath(TopSellerIconFile))
                            Case "sale"
                                CurrentOverlayImage = Image.FromFile(Server.MapPath(SaleIconFile))
                            Case Else
                                CurrentOverlayImage = Image.FromFile(Server.MapPath(TopSellerIconFile))
                        End Select

                        Dim ForegroundWidth As Integer
                        Dim ForegroundHeight As Integer

                        If CurrentOverlayImage.Height > CurrentOverlayImage.Width Then
                            ForegroundHeight = CInt(MaxSize * 0.3)
                            ForegroundWidth = CInt(CurrentOverlayImage.Width * (CurrentHeight / CurrentOverlayImage.Height) * 0.3)
                        Else
                            ForegroundWidth = CInt(MaxSize * 0.3)
                            ForegroundHeight = CInt(CurrentOverlayImage.Height * (CurrentWidth / CurrentOverlayImage.Width) * 0.3)
                        End If

                        Dim CurrentBitmap As New Bitmap(CurrentWidth, CurrentHeight)
                        Using gr As Graphics = Graphics.FromImage(CurrentBitmap)
                            gr.DrawImage(CurrentResponseImage, 0, 0, CurrentWidth, CurrentHeight)
                            gr.DrawImage(CurrentOverlayImage, 20, 0, ForegroundWidth, ForegroundHeight)
                        End Using

                        CurrentResponseImage = CurrentBitmap.GetThumbnailImage(CurrentWidth, CurrentHeight, NullImage, IntPtr.Zero)
                    End If

                    Dim CurrentImageCodec As ImageCodecInfo
                    Dim EncParams As New EncoderParameters(2)

                    Select Case CurrentContentType
                        Case "image/pjpeg", "image/jpeg"
                            CurrentImageCodec = ImageCodecInfo.GetImageEncoders().Where(Function(ic) ic.MimeType = "image/jpeg").FirstOrDefault
                            EncParams.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                        Case "image/png", "image/x-png"
                            If Image.IsAlphaPixelFormat(CurrentResponseImage.PixelFormat) Then
                                CurrentImageCodec = ImageCodecInfo.GetImageEncoders().Where(Function(ic) ic.MimeType = "image/png").FirstOrDefault
                                EncParams.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                            Else
                                CurrentImageCodec = ImageCodecInfo.GetImageEncoders().Where(Function(ic) ic.MimeType = "image/jpeg").FirstOrDefault
                                EncParams.Param(0) = New EncoderParameter(Encoder.Quality, 100L)

                                CurrentContentType = "image/jpeg"
                            End If
                        Case "image/gif"
                            CurrentImageCodec = ImageCodecInfo.GetImageEncoders().Where(Function(ic) ic.MimeType = "image/gif").FirstOrDefault
                            EncParams.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                        Case "image/bmp"
                            CurrentImageCodec = ImageCodecInfo.GetImageEncoders().Where(Function(ic) ic.MimeType = "image/bmp").FirstOrDefault
                            EncParams.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                        Case "image/tiff"
                            CurrentImageCodec = ImageCodecInfo.GetImageEncoders().Where(Function(ic) ic.MimeType = "image/tiff").FirstOrDefault
                            EncParams.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                        Case "image/tif"
                            CurrentImageCodec = ImageCodecInfo.GetImageEncoders().Where(Function(ic) ic.MimeType = "image/tif").FirstOrDefault
                            EncParams.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                        Case Else
                            CurrentImageCodec = ImageCodecInfo.GetImageEncoders().Where(Function(ic) ic.MimeType = "image/jpeg").FirstOrDefault
                            EncParams.Param(0) = New EncoderParameter(Encoder.Quality, 100L)
                    End Select

                    EncParams.Param(1) = New EncoderParameter(Encoder.Compression, 100L)

                    CurrentResponseImage.Save(CurrentOutputStream, CurrentImageCodec, EncParams)

                    Dim CurrentThumbnailData(Convert.ToInt32(CurrentOutputStream.Length)) As Byte

                    CurrentOutputStream.Position = 0
                    CurrentOutputStream.Read(CurrentThumbnailData, 0, Convert.ToInt32(CurrentOutputStream.Length))
                    CurrentOutputStream.Close()

                    RequestContext.HttpContext.Response.Clear()
                    RequestContext.HttpContext.Response.ContentType = CurrentContentType
                    RequestContext.HttpContext.Response.BinaryWrite(CurrentThumbnailData)
                    RequestContext.HttpContext.Response.AppendHeader("Expires", DateTime.Now.AddMonths(12).ToString("DDD, dd MMM yyyy hh:mm:ss"))

                    If IsGZipSupported() Then
                        Dim AcceptEncoding As String = Context.Request.Headers("Accept-Encoding")

                        If AcceptEncoding.Contains("deflate") Then
                            RequestContext.HttpContext.Response.Filter = New Compression.DeflateStream(Context.Response.Filter, Compression.CompressionMode.Compress)
                            RequestContext.HttpContext.Response.AppendHeader("Content-Encoding", "deflate")
                        Else
                            RequestContext.HttpContext.Response.Filter = New Compression.GZipStream(Context.Response.Filter, Compression.CompressionMode.Compress)
                            RequestContext.HttpContext.Response.AppendHeader("Content-Encoding", "gzip")
                        End If
                    End If

                    'Me.Response.AppendHeader("Content-Type", contentType)
                    'Me.Response.AppendHeader("Content-Length", output_stream.Length.ToString)
                    'HttpResponse Response = HttpContext.Current.Response

                    ' '' Allow proxy servers to cache encoded and unencoded versions separately
                    'Me.Response.AppendHeader("Vary", "Content-Encoding")

                    CurrentResponseImage.Dispose()
                    CurrentMemoryStream.Dispose()
                    CurrentOutputStream.Dispose()
                Else
                    Response.Redirect(Settings.NoImageUrl)
                End If
            End If
            'Catch CurrentException As Exception
            'Response.Redirect(Settings.NoImageUrl)
            'End Try
        End Sub

        Private Function NullImage() As Image.GetThumbnailImageAbort
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
End Namespace