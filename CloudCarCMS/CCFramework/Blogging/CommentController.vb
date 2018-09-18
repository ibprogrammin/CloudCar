Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Blogging

    Public Class CommentController

        Public Shared GetAllCommentsFunc As Func(Of BlogDataContext, IQueryable(Of Comment)) = _
            CompiledQuery.Compile(Function(db As BlogDataContext) _
                                      From c In db.Comments Select c)

        Public Shared GetCommentFunc As Func(Of BlogDataContext, Integer, Comment) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, id As Integer) _
                                      (From c In db.Comments Where c.id = id Select c).SingleOrDefault())

        Public Shared GetBlogCommentsFunc As Func(Of BlogDataContext, Integer, IQueryable(Of Comment)) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, id As Integer) _
                                      From c In db.Comments Where c.BlogId = id Select c)

        Public Shared GetApprovedBlogCommentsFunc As Func(Of BlogDataContext, Integer, IQueryable(Of Comment)) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, id As Integer) _
                                      From c In db.Comments Where c.BlogId = id And c.Approved = True Order By c.DatePosted Descending Select c)

        Public Shared GetCommentEmailFunc As Func(Of BlogDataContext, String, Comment) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, email As String) _
                                      (From c In db.Comments Where c.Email Like email Select c).SingleOrDefault())

        Public Shared GetLatestCommentFunc As Func(Of BlogDataContext, Comment) = _
            CompiledQuery.Compile(Function(db As BlogDataContext) _
                                      (From c In db.Comments Order By c.DatePosted Descending Select c).FirstOrDefault())

        Public Shared Function CreateComment(ByVal BlogID As Integer, ByVal Name As String, ByVal Email As String, ByVal Comment As String, ByVal Url As String) As Integer
            Dim db As New BlogDataContext

            Dim blogComment As New Comment

            blogComment.BlogId = BlogID
            blogComment.Name = Name
            blogComment.Email = Email
            blogComment.Comment = Comment
            blogComment.Url = Url
            blogComment.Approved = True

            db.Comments.InsertOnSubmit(blogComment)
            db.SubmitChanges()

            CreateComment = blogComment.id

            db.Dispose()
        End Function

        Public Shared Function CreateComment(ByVal comment As Comment) As Integer
            Dim db As New BlogDataContext

            db.Comments.InsertOnSubmit(comment)
            db.SubmitChanges()

            CreateComment = comment.id

            db.Dispose()
        End Function

        Public Shared Sub UpdateComment(ByVal ID As Integer, ByVal BlogID As Integer, ByVal Name As String, ByVal Email As String, ByVal Comment As String, ByVal Url As String)
            Dim db As New BlogDataContext

            Dim blogComment As Comment = GetCommentFunc(db, ID)

            If blogComment Is Nothing Then
                Throw New InvalidCommentException()
            Else
                blogComment.BlogId = BlogID
                blogComment.Name = Name
                blogComment.Email = Email
                blogComment.Comment = Comment
                blogComment.Url = Url

                db.SubmitChanges()
            End If

            blogComment = Nothing
            db = Nothing
        End Sub

        Public Shared Function GetComment() As System.Linq.IQueryable(Of Comment)
            Dim db As New BlogDataContext

            Dim blogComment = GetAllCommentsFunc(db)

            If blogComment Is Nothing Then
                Throw New InvalidCommentException()
            Else
                GetComment = blogComment
            End If

            db = Nothing
        End Function

        Public Shared Function GetComment(ByVal CommentID As Integer) As Comment
            Dim db As New BlogDataContext

            Dim blogComment As Comment = GetCommentFunc(db, CommentID)

            If blogComment Is Nothing Then
                Throw New InvalidCommentException()
            Else
                GetComment = blogComment
            End If

            db = Nothing
        End Function

        Public Shared Function GetComment(ByVal Email As String) As Comment
            Dim db As New BlogDataContext

            Dim blogComment As Comment = GetCommentEmailFunc(db, Email)

            If blogComment Is Nothing Then
                Throw New InvalidCommentException()
            Else
                GetComment = blogComment
            End If

            db = Nothing
        End Function

        Public Shared Function GetCommentsInBlog(ByVal BlogID As Integer) As List(Of Comment)
            Dim db As New BlogDataContext

            Dim blogComments As List(Of Comment) = GetBlogCommentsFunc(db, BlogID).ToList()

            If blogComments Is Nothing Then
                Throw New InvalidCommentException()
            Else
                GetCommentsInBlog = blogComments
            End If

            db = Nothing
        End Function

        Public Shared Function DeleteBlogComments(ByVal BlogId As Integer) As Boolean
            Dim CurrentBlogDataContext As New BlogDataContext

            Dim BlogComments As List(Of Comment) = GetBlogCommentsFunc(CurrentBlogDataContext, BlogId).ToList

            CurrentBlogDataContext.Comments.DeleteAllOnSubmit(BlogComments.AsEnumerable())
            CurrentBlogDataContext.SubmitChanges()

            DeleteBlogComments = True

            CurrentBlogDataContext.Dispose()
        End Function

        Public Class InvalidCommentException
            Inherits Exception

            Public Sub New()
                MyBase.New("The comment you are looking for does not exist")
            End Sub

        End Class

    End Class

End Namespace