Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Blogging

    Public Class AuthorController

        Public Shared GetAllAuthorsFunc As Func(Of BlogDataContext, IQueryable(Of Author)) = _
            CompiledQuery.Compile(Function(db As BlogDataContext) _
                                      From a In db.Authors Select a)

        Public Shared GetAuthorFunc As Func(Of BlogDataContext, Integer, Author) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, id As Integer) _
                                      (From a In db.Authors Where a.ID = id Select a).SingleOrDefault())

        Public Shared GetAuthorPermalinkFunc As Func(Of BlogDataContext, String, Author) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, permalink As String) _
                                      (From a In db.Authors Where a.Permalink Like permalink Select a).SingleOrDefault())

        Public Shared GetAuthorByNameFunc As Func(Of BlogDataContext, String, Author) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, Name As String) _
                                      (From a In db.Authors Where a.Name Like Name Select a).SingleOrDefault())

        Public Shared GetPermalinkExistsFunc As Func(Of BlogDataContext, String, Integer) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, permalink As String) (From a In db.Authors Where a.Permalink Like permalink Select a).Count)

        Public Shared GetPermalinkExistsUpdateFunc As Func(Of BlogDataContext, String, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, permalink As String, AuthorId As Integer) (From a In db.Authors Where a.Permalink Like permalink And Not a.ID = AuthorId Select a).Count)

        Public Shared Function Create(ByVal Name As String, ByVal Biography As String, ByVal AvatarURL As String, ByVal Title As String, ByVal Keywords As String, ByVal Description As String, ByVal Permalink As String, Optional ByVal UserID As Integer = 0) As Integer

            Dim db As New BlogDataContext

            Dim author As New Author

            author.Name = Name
            author.Biography = Biography
            author.AvatarURL = AvatarURL
            author.Title = Title
            author.Keywords = Keywords
            author.Description = Description
            author.Permalink = Permalink
            author.UserID = UserID

            db.Authors.InsertOnSubmit(author)
            db.SubmitChanges()

            Create = author.ID

            db.Dispose()
        End Function

        Public Shared Function Create(ByVal author As Author) As Integer
            Dim db As New BlogDataContext

            db.Authors.InsertOnSubmit(author)
            db.SubmitChanges()

            Create = author.ID

            db.Dispose()
        End Function

        Public Shared Sub Update(ByVal ID As Integer, ByVal Name As String, ByVal Biography As String, ByVal AvatarURL As String, ByVal Title As String, ByVal Keywords As String, ByVal Description As String, ByVal Permalink As String, Optional ByVal UserID As Integer = 0)
            Dim db As New BlogDataContext

            Dim author As Author = GetAuthorFunc(db, ID)

            If author Is Nothing Then
                Throw New InvalidAuthorException()
            Else
                author.Name = Name
                author.Biography = Biography
                author.AvatarURL = AvatarURL
                author.Title = Title
                author.Keywords = Keywords
                author.Description = Description
                author.Permalink = Permalink
                author.UserID = UserID

                db.SubmitChanges()
            End If

            author = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal AuthorID As Integer) As Boolean
            Try
                Dim db As New BlogDataContext

                Dim author = GetAuthorFunc(db, AuthorID)

                db.Authors.DeleteOnSubmit(author)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetElements() As System.Linq.IQueryable(Of Author)
            Dim db As New BlogDataContext

            Dim author = GetAllAuthorsFunc(db)

            If author Is Nothing Then
                Throw New InvalidAuthorException()
            Else
                Return author
            End If

            db = Nothing
        End Function

        Public Shared Function GetElement(ByVal AuthorID As Integer) As Author
            Dim db As New BlogDataContext

            Dim author As Author = GetAuthorFunc(db, AuthorID)

            If author Is Nothing Then
                Throw New InvalidAuthorException()
            Else
                Return author
            End If

            db = Nothing
        End Function

        Public Shared Function GetElement(ByVal Name As String) As Author
            Dim db As New BlogDataContext

            Dim author As Author = GetAuthorByNameFunc(db, Name)

            If author Is Nothing Then
                Throw New InvalidAuthorException()
            Else
                Return author
            End If

            db = Nothing
        End Function

        Public Shared Function GetAuthorFromLink(ByVal Permalink As String) As Author
            Dim db As New BlogDataContext

            Dim author As Author = GetAuthorPermalinkFunc(db, Permalink)

            If author Is Nothing Then
                Throw New InvalidAuthorException()
            Else
                Return author
            End If

            db = Nothing
        End Function

        Public Shared Function HasPermalink(ByVal Permalink As String) As Boolean
            Dim db As New BlogDataContext

            Dim pageCount As Integer = GetPermalinkExistsFunc(db, Permalink)

            If pageCount = 0 Then
                Return False
            Else
                Return True
            End If

            db = Nothing
        End Function

        Public Shared Function HasPermalink(ByVal Permalink As String, ByVal AuthorId As Integer) As Boolean
            Dim db As New BlogDataContext

            Dim pageCount As Integer = GetPermalinkExistsUpdateFunc(db, Permalink, AuthorId)

            If pageCount = 0 Then
                Return False
            Else
                Return True
            End If

            db = Nothing
        End Function

        Public Class InvalidAuthorException
            Inherits Exception

            Public Sub New()
                MyBase.New("The author you are looking for does not exist")
            End Sub

        End Class

    End Class

End Namespace