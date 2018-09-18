Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Blogging

    Public Class BlogController

        Public Shared GetAllBlogsFunc As Func(Of BlogDataContext, IQueryable(Of Blog)) = _
            CompiledQuery.Compile(Function(db As BlogDataContext) _
                                      From b In db.Blogs Select b)

        Public Shared GetBlogFunc As Func(Of BlogDataContext, Integer, Blog) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, id As Integer) _
                                      (From b In db.Blogs Where b.id = id Select b).SingleOrDefault())

        Public Shared GetBlogPermalinkFunc As Func(Of BlogDataContext, String, Blog) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, permalink As String) _
                                      (From b In db.Blogs Where b.Permalink Like permalink Select b).SingleOrDefault())

        Public Shared GetBlogTitleFunc As Func(Of BlogDataContext, String, Blog) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, title As String) _
                                      (From b In db.Blogs Where b.Title Like title Select b).SingleOrDefault())

        Public Shared GetBlogsByAuthorFunc As Func(Of BlogDataContext, Integer, IQueryable(Of Blog)) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, AuthorID As Integer) _
                                      From b In db.Blogs Where b.AuthorID = AuthorID Order By b.DatePosted Descending Select b)

        Public Shared GetLatestBlogFunc As Func(Of BlogDataContext, Blog) = _
            CompiledQuery.Compile(Function(db As BlogDataContext) _
                                      (From b In db.Blogs Where b.Live = True Order By b.DatePosted Descending Select b).FirstOrDefault())

        Public Shared GetLiveBlogsFunc As Func(Of BlogDataContext, IQueryable(Of Blog)) = _
            CompiledQuery.Compile(Function(db As BlogDataContext) _
                                      From b In db.Blogs Where b.Live = True Select b)

        Public Shared GetPermalinkExistsFunc As Func(Of BlogDataContext, String, Integer) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, permalink As String) (From b In db.Blogs Where b.Permalink Like permalink Select b).Count)

        Public Shared GetPermalinkExistsUpdateFunc As Func(Of BlogDataContext, String, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, permalink As String, blogId As Integer) (From b In db.Blogs Where b.Permalink Like permalink And Not b.id = blogId Select b).Count)

        Public Shared GetBlogCategoriesFunc As Func(Of BlogDataContext, String, Integer, IQueryable(Of String)) = _
            CompiledQuery.Compile(Function(db As BlogDataContext, Prefix As String, Count As Integer) _
                                      (From b In db.Blogs Where SqlClient.SqlMethods.Like(b.Category.ToLower, Prefix.ToLower & "%") Select b.Category Order By Category Take Count))

        Public Shared Function CreateBlog(ByVal AuthorID As Integer, ByVal Title As String, ByVal SubHeading As String, ByVal BlogContent As String, ByVal PermaLink As String, ByVal Keywords As String, ByVal Description As String, ByVal ContentSummary As String, ByVal Category As String, ByVal HeadingImageURL As String, ByVal ThumbnailImageURL As String, ByVal Live As Boolean) As Integer
            Dim db As New BlogDataContext

            Dim blog As New Blog

            blog.AuthorID = AuthorID
            blog.Title = Title
            blog.SubHeading = SubHeading
            blog.BlogContent = BlogContent
            blog.Keywords = Keywords
            blog.Description = Description
            blog.ContentSummary = ContentSummary
            blog.Permalink = PermaLink
            blog.Category = Category
            blog.DatePosted = Date.Now
            blog.ImageLink = HeadingImageURL
            blog.ThumnailImageLink = ThumbnailImageURL
            blog.Live = Live

            db.Blogs.InsertOnSubmit(blog)
            db.SubmitChanges()

            CreateBlog = blog.id

            db.Dispose()
        End Function

        Public Shared Function CreateBlog(ByVal blog As Blog) As Integer
            Dim db As New BlogDataContext

            db.Blogs.InsertOnSubmit(blog)
            db.SubmitChanges()

            CreateBlog = blog.id

            db.Dispose()
        End Function

        Public Shared Sub UpdateBlog(ByVal ID As Integer, ByVal AuthorID As Integer, ByVal Title As String, ByVal SubHeading As String, ByVal BlogContent As String, ByVal PermaLink As String, ByVal Keywords As String, ByVal Description As String, ByVal ContentSummary As String, ByVal Category As String, ByVal HeadingImageURL As String, ByVal ThumbnailImageURL As String, ByVal Live As Boolean)
            Dim db As New BlogDataContext

            Dim blog As Blog = GetBlogFunc(db, ID)

            If blog Is Nothing Then
                Throw New InvalidBlogException()
            Else
                blog.AuthorID = AuthorID
                blog.Title = Title
                blog.SubHeading = SubHeading
                blog.BlogContent = BlogContent
                blog.Keywords = Keywords
                blog.Description = Description
                blog.ContentSummary = ContentSummary
                blog.Permalink = PermaLink
                blog.Category = Category
                blog.ImageLink = HeadingImageURL
                blog.ThumnailImageLink = ThumbnailImageURL
                blog.Live = Live

                db.SubmitChanges()
            End If

            blog = Nothing
            db = Nothing
        End Sub

        Public Shared Function DeleteBlog(ByVal BlogId As Integer) As Boolean
            Try
                Dim db As New BlogDataContext

                Dim blog = GetBlogFunc(db, BlogID)

                db.Blogs.DeleteOnSubmit(blog)
                db.SubmitChanges()

                Return True
            Catch Ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetBlog() As List(Of Blog)
            Dim CurrentDataContext As New BlogDataContext

            Dim Blogs As List(Of Blog) = GetAllBlogsFunc(CurrentDataContext).ToList

            If Blogs Is Nothing Then
                Throw New InvalidBlogException()
            Else
                GetBlog = Blogs
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetBlog(ByVal BlogId As Integer) As Blog
            Dim db As New BlogDataContext

            Dim blog As Blog = GetBlogFunc(db, BlogId)

            If blog Is Nothing Then
                Throw New InvalidBlogException()
            Else
                GetBlog = blog
            End If

            db = Nothing
        End Function

        Public Shared Function GetBlog(ByVal Title As String) As Blog
            Dim db As New BlogDataContext

            Dim blog As Blog = GetBlogTitleFunc(db, Title)

            If blog Is Nothing Then
                Throw New InvalidBlogException()
            Else
                GetBlog = blog
            End If

            db = Nothing
        End Function

        Public Shared Function GetBlogsByAuthor(ByVal AuthorID As Integer) As System.Linq.IQueryable(Of Blog)
            Dim db As New BlogDataContext

            Dim blogs = GetBlogsByAuthorFunc(db, AuthorID)

            If blogs Is Nothing Then
                Throw New InvalidBlogException()
            Else
                Return blogs
            End If

            db = Nothing
        End Function

        Public Shared Function GetLiveBlogs() As List(Of Blog)
            Dim db As New BlogDataContext

            Dim blog = GetLiveBlogsFunc(db).ToList

            If blog Is Nothing Then
                Throw New InvalidBlogException()
            Else
                Return blog
            End If

            'db = Nothing
        End Function

        Public Shared Function GetBlogFromLink(ByVal Permalink As String) As Blog
            Dim db As New BlogDataContext

            Dim blog As Blog = GetBlogPermalinkFunc(db, Permalink)

            If blog Is Nothing Then
                Throw New InvalidBlogException()
            Else
                GetBlogFromLink = blog
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

        Public Shared Function HasPermalink(ByVal Permalink As String, ByVal blogId As Integer) As Boolean
            Dim db As New BlogDataContext

            Dim pageCount As Integer = GetPermalinkExistsUpdateFunc(db, Permalink, blogId)

            If pageCount = 0 Then
                Return False
            Else
                Return True
            End If

            db = Nothing
        End Function

        Public Class InvalidBlogException
            Inherits Exception

            Public Sub New()
                MyBase.New("The blog entry you are looking for does not exist")
            End Sub

        End Class

    End Class

End Namespace