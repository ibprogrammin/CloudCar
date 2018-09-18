Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement

    Public Class ImageRotatorController

        Public Shared Function Create(ByVal Title As String, ByVal SubHeading As String, ByVal Details As String, ByVal LinkURL As String, ByVal ItemOrder As Integer, ByVal ImageID As Integer, ByVal PageID As Integer) As Integer
            Dim db As New ContentDataContext

            Dim item As New ImageRotator

            item.title = Title
            item.subheading = SubHeading
            item.details = Details
            item.linkurl = LinkURL
            item.order = ItemOrder
            item.imageID = ImageID
            item.pageID = PageID

            db.ImageRotators.InsertOnSubmit(item)
            db.SubmitChanges()

            Create = item.id

            db.Dispose()
        End Function

        Public Shared Sub Update(ByVal ID As Integer, ByVal Title As String, ByVal SubHeading As String, ByVal Details As String, ByVal LinkURL As String, ByVal ItemOrder As Integer, ByVal ImageID As Integer, ByVal PageID As Integer)
            Dim db As New ContentDataContext
            Dim item As ImageRotator

            item = GetElement(db, ID)

            If item Is Nothing Then
                Throw New InvalidImageRotatorException()
            Else
                item.title = Title
                item.subheading = SubHeading
                item.details = Details
                item.linkurl = LinkURL
                item.order = ItemOrder
                item.imageID = ImageID
                item.pageID = PageID

                db.SubmitChanges()
            End If

            item = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim db As New ContentDataContext

                Dim item As ImageRotator = GetElement(db, ID)

                db.ImageRotators.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetPageTopOrder(ByVal PageID As Integer) As Integer
            Try
                Dim db As New ContentDataContext

                Return GetPageTopOrderFunc(db, PageID)
            Catch ex As Exception
                Return 0
            End Try
        End Function

        Public Shared Function GetPageOrderAvailable(ByVal PageID As Integer, ByVal Order As Integer, ByVal RotatorID As Integer) As Boolean
            'Try
            Dim db As New ContentDataContext

            Return GetPageOrderAvailableFunc(db, PageID, Order, RotatorID)
            'Catch ex As Exception
            'Return False
            'End Try
        End Function

        Public Shared GetElement As Func(Of ContentDataContext, Integer, ImageRotator) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, ID As Integer) (From i In db.ImageRotators Where i.id = ID Select i).SingleOrDefault)

        Public Shared GetElements As Func(Of ContentDataContext, IQueryable(Of ImageRotator)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) From i In db.ImageRotators Select i)

        Public Shared GetElementByPage As Func(Of ContentDataContext, Integer, IQueryable(Of ImageRotator)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, PageID As Integer) From i In db.ImageRotators Where i.pageID = PageID Select i)

        Public Shared GetPageTopOrderFunc As Func(Of ContentDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, pageId As Integer) (From i In db.ImageRotators Where i.pageID = pageId Select i.order).Max)

        Public Shared GetPageOrderAvailableFunc As Func(Of ContentDataContext, Integer, Integer, Integer, Boolean) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, pageId As Integer, order As Integer, rotatorID As Integer) _
                                      If((From i In db.ImageRotators Where i.pageID = pageId And i.order = order And Not i.id = rotatorID Select i).FirstOrDefault Is Nothing, True, False))

        Public Class InvalidImageRotatorException
            Inherits Exception

            Public Sub New()
                MyBase.New("The image rotator item you are looking for does not exist")
            End Sub

        End Class

    End Class

End Namespace