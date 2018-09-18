Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Core

    Public Class PictureController

        Private Const PictureDirectory As String = "/images/db"

        Public Shared GetPicturesFunc As Func(Of ContentDataContext, IQueryable(Of Picture)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      From p In CurrentDataContext.Pictures Select p)

        Public Shared GetPictureByIdFunc As Func(Of ContentDataContext, Integer, Picture) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, PictureId As Integer) _
                (From p In CurrentDataContext.Pictures Where p.PictureID = PictureId).FirstOrDefault)

        Public Shared Function CreatePicture(ByVal filename As String, ByVal imgType As String, ByVal imgLength As Integer, ByVal data As Byte()) As Integer
            Dim db As New ContentDataContext

            Dim pictureID As Integer
            Dim picture As New Picture

            picture.PictureContentLength = imgLength
            picture.PictureContentType = imgType
            picture.PictureData = data
            picture.PictureFileName = filename

            db.Pictures.InsertOnSubmit(picture)
            db.SubmitChanges()

            pictureID = picture.PictureID

            picture = Nothing
            data = Nothing
            db = Nothing

            Return pictureID
        End Function

        Public Shared Sub UpdatePicture(ByVal PictureID As Integer, ByVal filename As String, ByVal imgType As String, ByVal imgLength As Integer, ByVal data As Byte())
            Dim db As New ContentDataContext
            Dim pic As Picture = GetPictureByIDFunc(db, PictureID)

            If pic Is Nothing Then
                Throw New InvalidPictureException
            Else
                pic.PictureContentLength = imgLength
                pic.PictureContentType = imgType
                pic.PictureData = data
                pic.PictureFileName = filename

                db.SubmitChanges()
            End If

            pic = Nothing
            db = Nothing
        End Sub

        Public Shared Function DeletePicture(ByVal PictureId As Integer) As Boolean
            Try
                Dim db As New ContentDataContext

                Dim pic = GetPictureByIDFunc(db, PictureId)

                db.Pictures.DeleteOnSubmit(pic)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetPicture(ByVal PictureId As Integer) As Picture
            Dim db As New ContentDataContext

            Dim pic As Picture = GetPictureByIdFunc(db, PictureId)

            If pic Is Nothing Then
                Throw New InvalidPictureException
            Else
                GetPicture = pic
            End If

            db.Dispose()
        End Function

        Public Shared Function GetPictures() As List(Of Picture)
            Dim db As New ContentDataContext

            Dim pics As List(Of Picture) = GetPicturesFunc(db).ToList

            If pics Is Nothing Then
                Throw New InvalidPictureException
            Else
                GetPictures = pics
            End If

            db.Dispose()
        End Function

        Public Shared Function GetPictureIDs() As List(Of Integer)
            Dim CurrentDataContext As New ContentDataContext
            Dim PictureIdList As List(Of Integer)

            PictureIdList = GetPicturesFunc(CurrentDataContext).Select(Function(p) p.PictureID).ToList

            If PictureIdList Is Nothing Then
                Throw New InvalidPictureException
            Else
                GetPictureIDs = PictureIdList
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Class InvalidPictureException
            Inherits Exception

            Public Sub New()
                MyBase.New("The picture you are looking for does not exist")
            End Sub

        End Class

        Public Shared GetPictureFileNameByIdFunc As Func(Of ContentDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, PictureId As Integer) _
                                      (From p In db.Pictures Where p.PictureID = PictureId Select p).FirstOrDefault.PictureFileName)

        Public Shared GetPictureInPageCountFunc As Func(Of ContentDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, PictureID As Integer) (From p In db.ContentPages Where p.headerImageID = PictureID Or SqlClient.SqlMethods.Like(p.pageContent, "%/images/db/" & PictureID & "%")).Count)

        Public Shared GetPictureInBlogCountFunc As Func(Of BlogDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, PictureID As Integer) (From b In db.Blogs Where SqlClient.SqlMethods.Like(b.ContentSummary, "%/images/db/" & PictureID & "%") Or SqlClient.SqlMethods.Like(b.ImageLink, "%/images/db/" & PictureID & "%") Or SqlClient.SqlMethods.Like(b.BlogContent, "%/images/db/" & PictureID & "%")).Count)

        Public Shared GetPictureInAuthorCountFunc As Func(Of BlogDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, PictureID As Integer) (From a In db.Authors Where SqlClient.SqlMethods.Like(a.AvatarURL, "%/images/db/" & PictureID & "%")).Count)

        Public Shared GetPictureInBrandCountFunc As Func(Of CommerceDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, PictureID As Integer) (From b In db.Brands Where b.LogoImageID = PictureID).Count)

        Public Shared GetPictureInCategoryCountFunc As Func(Of CommerceDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, PictureID As Integer) (From c In db.Categories Where SqlClient.SqlMethods.Like(c.Details, "%/images/db/" & PictureID & "%")).Count)

        Public Shared GetPictureInEventCountFunc As Func(Of ContentDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, ImageId As Integer) (From en In db.Events Where en.ImageId = ImageId Or SqlClient.SqlMethods.Like(en.Details, "%/images/db/" & ImageId & "%")).Count)

        Public Shared GetPictureInImageRotatorCountFunc As Func(Of ContentDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, PictureID As Integer) (From ir In db.ImageRotators Where ir.imageID = PictureID).Count)

        Public Shared GetPictureInImageGalleryCountFunc As Func(Of ContentDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, PictureID As Integer) (From ig In db.ImageGalleryItems Where ig.ImageID = PictureID).Count)

        Public Shared GetPictureInLinksCountFunc As Func(Of ContentDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, PictureID As Integer) (From l In db.Links Where l.PictureID = PictureID).Count)

        Public Shared GetPictureInProductCountFunc As Func(Of CommerceDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, PictureID As Integer) (From p In db.Products Where p.DefaultImageID = PictureID Or p.HeaderImageID = PictureID Or SqlClient.SqlMethods.Like(p.Details, "%/images/db/" & PictureID & "%")).Count)

        Public Shared GetPictureInProductColorCountFunc As Func(Of CommerceDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, PictureID As Integer) (From c In db.ProductColors Where c.ImageID = PictureID).Count)

        Public Shared GetPictureInProductImageCountFunc As Func(Of CommerceDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, PictureID As Integer) (From i In db.ProductImages Where i.ImageID = PictureID).Count)

        Public Shared GetPictureInTestimonialCountFunc As Func(Of ContentDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, PictureID As Integer) (From t In db.Testimonials Where t.ImageID = PictureID).Count)

        Public Shared Function GetPictureFilename(ByVal Id As Integer) As String
            Dim CurrentDataContext As New ContentDataContext

            Return GetPictureFileNameByIdFunc(CurrentDataContext, Id)
        End Function

        Public Shared Function GetUnusedPictures() As List(Of Picture)
            Dim DeletePictures As New List(Of Picture)

            For Each Item As Picture In GetPictures()
                Dim UsageCount As Integer = GetPictureUsageCount(Item.PictureID)

                If UsageCount = 0 Then
                    DeletePictures.Add(Item)
                End If
            Next

            Return DeletePictures
        End Function

        Public Shared Function GetUsedPictures() As List(Of Picture)
            Dim db1 As New ContentDataContext
            Dim db2 As New BlogDataContext
            Dim db3 As New CommerceDataContext

            Dim DeletePictures As New List(Of Picture)

            For Each Item As Picture In GetPictures()
                Dim UsageCount As Integer = GetPictureUsageCount(Item.PictureID)

                If UsageCount > 0 Then
                    DeletePictures.Add(Item)
                End If
            Next

            Return DeletePictures
        End Function

        Public Shared Function GetPictureUsageCount(ByVal ImageId As Integer) As Integer
            Dim db1 As New ContentDataContext
            Dim db2 As New BlogDataContext
            Dim db3 As New CommerceDataContext

            Dim UsageCount As Integer

            UsageCount += GetPictureInPageCountFunc(db1, ImageId)
            UsageCount += GetPictureInBlogCountFunc(db2, ImageId)
            UsageCount += GetPictureInAuthorCountFunc(db2, ImageId)
            UsageCount += GetPictureInBrandCountFunc(db3, ImageId)
            UsageCount += GetPictureInCategoryCountFunc(db3, ImageId)
            UsageCount += GetPictureInEventCountFunc(db1, ImageId)
            UsageCount += GetPictureInImageRotatorCountFunc(db1, ImageId)
            UsageCount += GetPictureInImageGalleryCountFunc(db1, ImageId)
            UsageCount += GetPictureInLinksCountFunc(db1, ImageId)
            UsageCount += GetPictureInProductCountFunc(db3, ImageId)
            UsageCount += GetPictureInProductColorCountFunc(db3, ImageId)
            UsageCount += GetPictureInProductImageCountFunc(db3, ImageId)
            UsageCount += GetPictureInTestimonialCountFunc(db1, ImageId)

            Return UsageCount
        End Function

        Public Shared Function GetPictureCount() As Integer
            Dim CurrentDataContext As New ContentDataContext

            Return GetPicturesFunc(CurrentDataContext).Count
        End Function

        Public Shared Function DeleteUnusedPictures() As Boolean
            Dim DeletePictures As List(Of Picture) = GetUnusedPictures()

            Dim db As New ContentDataContext

            db.Pictures.DeleteAllOnSubmit(DeletePictures)
            db.SubmitChanges()

            Return True
        End Function

        Public Shared Function GetWaistedImageSpace() As Long
            Dim DeletePictures As List(Of Picture) = GetUnusedPictures()

            Dim WaistedSpace As Long = 0

            For Each item In DeletePictures
                WaistedSpace += CType(item.PictureContentLength, Long)
            Next

            Return WaistedSpace
        End Function

        Public Shared Function GetPictureLink(ByVal ImageId As Integer, Optional ByVal MaxSize As Integer = -1) As String
            If ImageId = Nothing Then
                Return String.Empty
            Else
                If MaxSize = -1 Then
                    Return String.Format("{0}/{1}/full/{2}", PictureDirectory, ImageId, GetPictureFilename(ImageId))
                Else
                    Return String.Format("{0}/{1}/{2}/{3}", PictureDirectory, ImageId, MaxSize, GetPictureFilename(ImageId))
                End If
            End If
        End Function

    End Class

End Namespace