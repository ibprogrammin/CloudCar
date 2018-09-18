Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement

    Public Class ImageGalleryController

        Public Shared GetGalleryByIDFunc As Func(Of ContentDataContext, Integer, ImageGallery) = _
                CompiledQuery.Compile(Function(db As ContentDataContext, id As Integer) (From i In db.ImageGalleries Where i.ID = id Select i).FirstOrDefault)

        Public Shared GetAllGalleriesFunc As Func(Of ContentDataContext, IQueryable(Of ImageGallery)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) From i In db.ImageGalleries Select i)

        Private Shared GetFirstGalleryItemByGalleryIDFunc As Func(Of ContentDataContext, Integer, Integer) = _
                CompiledQuery.Compile(Function(db As ContentDataContext, GalleryID As Integer) (From i In db.ImageGalleryItems Where i.GalleryID = GalleryID Order By i.Order Select i).First.ImageID)

        Public Shared Function Create(ByVal Title As String, ByVal Description As String) As Integer
            Dim db As New ContentDataContext

            Dim ig As New ImageGallery

            ig.Title = Title
            ig.Description = Description

            db.ImageGalleries.InsertOnSubmit(ig)
            db.SubmitChanges()

            Dim Id As Integer = ig.ID

            ig = Nothing
            db = Nothing

            Return Id
        End Function

        Public Shared Sub Update(ByVal ID As Integer, ByVal Title As String, ByVal Description As String)
            Dim db As New ContentDataContext

            Dim ig As ImageGallery = GetGalleryByIDFunc(db, ID)

            If ig Is Nothing Then
                Throw New InvalidImageGalleryException()
            Else
                ig.Title = Title
                ig.Description = Description

                db.SubmitChanges()
            End If

            ig = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim db As New ContentDataContext

                Dim ig As ImageGallery = GetGalleryByIDFunc(db, ID)

                db.ImageGalleries.DeleteOnSubmit(ig)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetElements() As System.Linq.IQueryable(Of ImageGallery)
            Dim db As New ContentDataContext

            Dim ig As IQueryable(Of ImageGallery) = GetAllGalleriesFunc(db)

            If ig Is Nothing Then
                Throw New InvalidImageGalleryException()
            Else
                Return ig
            End If

            db = Nothing
        End Function

        Public Shared Function GetElement(ByVal ID As Integer) As ImageGallery
            Dim db As New ContentDataContext

            Dim ig As ImageGallery = GetGalleryByIDFunc(db, ID)

            If ig Is Nothing Then
                Throw New InvalidImageGalleryException()
            Else
                Return ig
            End If

            db = Nothing
        End Function

        Public Shared Function GetFirstGalleryImage(ByVal GalleryId As Integer) As Integer
            Dim db As New ContentDataContext

            Dim ImageId As Integer = GetFirstGalleryItemByGalleryIDFunc(db, GalleryId)

            Return ImageId
        End Function

    End Class

    Public Class InvalidImageGalleryException
        Inherits Exception

        Public Sub New()
            MyBase.New("The image gallery you are looking for does not exist")
        End Sub

    End Class

    Public Class ImageGalleryItemController
        Public Shared GetGalleryItemsByIDFunc As Func(Of ContentDataContext, Integer, ImageGalleryItem) = _
                CompiledQuery.Compile(Function(db As ContentDataContext, id As Integer) (From i In db.ImageGalleryItems Where i.ID = id Select i).FirstOrDefault)

        Public Shared GetAllGalleryItemsFunc As Func(Of ContentDataContext, IQueryable(Of ImageGalleryItem)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) From i In db.ImageGalleryItems Select i)

        Public Shared GetGalleryItemsByGalleryIDFunc As Func(Of ContentDataContext, Integer, IQueryable(Of ImageGalleryItem)) = _
                CompiledQuery.Compile(Function(db As ContentDataContext, GalleryID As Integer) From i In db.ImageGalleryItems Where i.GalleryID = GalleryID Select i)

        Public Shared Function Create(ByVal Title As String, ByVal Description As String, ByVal GalleryID As Integer, ByVal ImageID As Integer, ByVal Order As Integer) As Integer
            Dim db As New ContentDataContext

            Dim igi As New ImageGalleryItem

            igi.Title = Title
            igi.Description = Description
            igi.GalleryID = GalleryID
            igi.ImageID = ImageID
            igi.Order = Order

            db.ImageGalleryItems.InsertOnSubmit(igi)
            db.SubmitChanges()

            Dim Id As Integer = igi.ID

            igi = Nothing
            db = Nothing

            Return Id
        End Function

        Public Shared Sub Update(ByVal ID As Integer, ByVal Title As String, ByVal Description As String, ByVal GalleryID As Integer, ByVal ImageID As Integer, ByVal Order As Integer)
            Dim db As New ContentDataContext

            Dim igi As ImageGalleryItem = GetGalleryItemsByIDFunc(db, ID)

            If igi Is Nothing Then
                Throw New InvalidImageGalleryItemException()
            Else
                igi.Title = Title
                igi.Description = Description
                igi.GalleryID = GalleryID
                igi.Order = Order

                If ImageID = 0 And igi.ImageID > 0 Then

                ElseIf Not ImageID = 0 Then
                    igi.ImageID = ImageID
                Else
                    igi.ImageID = ImageID
                End If

                db.SubmitChanges()
            End If

            igi = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim db As New ContentDataContext

                Dim igi As ImageGalleryItem = GetGalleryItemsByIDFunc(db, ID)

                db.ImageGalleryItems.DeleteOnSubmit(igi)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetElements() As System.Linq.IQueryable(Of ImageGalleryItem)
            Dim db As New ContentDataContext

            Dim igi As IQueryable(Of ImageGalleryItem) = GetAllGalleryItemsFunc(db)

            If igi Is Nothing Then
                Throw New InvalidImageGalleryItemException()
            Else
                Return igi
            End If

            db = Nothing
        End Function

        Public Shared Function GetElement(ByVal ID As Integer) As ImageGalleryItem
            Dim db As New ContentDataContext

            Dim igi As ImageGalleryItem = GetGalleryItemsByIDFunc(db, ID)

            If igi Is Nothing Then
                Throw New InvalidImageGalleryItemException()
            Else
                Return igi
            End If

            db = Nothing
        End Function

        Public Shared Function GetGalleryItemsByGallery(ByVal GalleryID As Integer) As List(Of ImageGalleryItem)
            Dim db As New ContentDataContext

            Dim igi As List(Of ImageGalleryItem) = GetGalleryItemsByGalleryIDFunc(db, GalleryID).ToList

            If igi Is Nothing Then
                Throw New InvalidImageGalleryItemException()
            Else
                Return igi
            End If

            db = Nothing
        End Function

    End Class

    Public Class InvalidImageGalleryItemException
        Inherits Exception

        Public Sub New()
            MyBase.New("The image gallery item you are looking for does not exist")
        End Sub

    End Class

End Namespace