Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement

    Public Class FileUploadController

        Public Shared GetAllFileUploadsFunc As Func(Of ContentDataContext, IQueryable(Of FileUpload)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) _
                                      From f In db.FileUploads Select f)

        Public Shared GetFileUploadByIDFunc As Func(Of ContentDataContext, Integer, FileUpload) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, id As Integer) _
                                      (From f In db.FileUploads Where f.ID = id Select f).FirstOrDefault)

        Public Shared GetAllEnabledFileUploadsFunc As Func(Of ContentDataContext, IQueryable(Of FileUpload)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) _
                                      From f In db.FileUploads Where f.Enabled = True Select f)

        Public Shared Function Create(ByVal FileName As String, ByVal Path As String, ByVal Title As String, ByVal Description As String, ByVal Enabled As Boolean) As Integer
            Dim db As New ContentDataContext

            Dim item As New FileUpload

            item.Filename = FileName
            item.Path = Path
            item.Title = Title
            item.Description = Description
            item.Enabled = Enabled

            db.FileUploads.InsertOnSubmit(item)
            db.SubmitChanges()

            Dim id As Integer = item.id

            item = Nothing
            db = Nothing

            Return id
        End Function

        Public Shared Function Create(ByVal item As FileUpload) As Integer
            Dim db As New ContentDataContext

            db.FileUploads.InsertOnSubmit(item)
            db.SubmitChanges()

            db = Nothing

            Return item.ID
        End Function

        Public Shared Sub Update(ByVal ID As Integer, ByVal FileName As String, ByVal Path As String, ByVal Title As String, ByVal Description As String, ByVal Enabled As Boolean)
            Dim db As New ContentDataContext

            Dim item As FileUpload = GetFileUploadByIDFunc(db, ID)

            If item Is Nothing Then
                Throw New InvalidFileUploadException()
            Else
                item.Filename = FileName
                item.Path = Path
                item.Title = Title
                item.Description = Description
                item.Enabled = Enabled

                db.SubmitChanges()
            End If

            item = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim db As New ContentDataContext
                Dim item As FileUpload = GetFileUploadByIDFunc(db, ID)
                Dim filename As String = item.Path & item.Filename

                System.IO.File.Delete(HttpContext.Current.Server.MapPath(filename))

                db.FileUploads.DeleteOnSubmit(item)
                db.SubmitChanges()

                item = Nothing
                db = Nothing

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetElement() As System.Linq.IQueryable(Of FileUpload)
            Dim db As New ContentDataContext

            Dim items = GetAllFileUploadsFunc(db)

            If items Is Nothing Then
                Throw New InvalidFileUploadException()
            Else
                GetElement = items
            End If

            items = Nothing
            db = Nothing
        End Function

        Public Shared Function GetElement(ByVal ID As Integer) As FileUpload
            Dim db As New ContentDataContext

            Dim item As FileUpload = GetFileUploadByIDFunc(db, ID)

            If item Is Nothing Then
                Throw New InvalidFileUploadException()
            Else
                GetElement = item
            End If

            item = Nothing
            db = Nothing
        End Function

        Public Shared Function GetEnabledFileUploads() As System.Linq.IQueryable(Of FileUpload)
            Dim db As New ContentDataContext

            Dim items = GetAllEnabledFileUploadsFunc(db)

            If items Is Nothing Then
                Throw New InvalidFileUploadException()
            Else
                GetEnabledFileUploads = items
            End If

            items = Nothing
            db = Nothing
        End Function

        Public Shared Sub SetEnabled(ByVal ID As Integer)
            Dim db As New ContentDataContext

            Dim item As FileUpload = GetFileUploadByIDFunc(db, ID)

            If item Is Nothing Then
                Throw New InvalidFileUploadException()
            Else
                item.Enabled = True

                db.SubmitChanges()
            End If

            item = Nothing
            db = Nothing
        End Sub

        Public Class InvalidFileUploadException
            Inherits Exception

            Public Sub New()
                MyBase.New("The file you are looking for does not exist")
            End Sub

        End Class

    End Class

End Namespace