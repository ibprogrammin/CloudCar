Imports CloudCar.CCFramework.Model
Imports System.Data.Linq
Imports CloudCar.CCFramework.Core

Namespace CCFramework.ContentManagement.NewsModule

    Public Class NewsController

        Public Shared GetNewsFunc As Func(Of ContentDataContext, IQueryable(Of Model.New)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      From e In CurrentDataContext.News Select e)

        Public Shared GetApprovedNewsFunc As Func(Of ContentDataContext, IQueryable(Of Model.New)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      From e In CurrentDataContext.News _
                                      Where e.Approved = True Select e _
                                      Order By e.PublishDate Ascending)

        Public Shared GetNewsByIdFunc As Func(Of ContentDataContext, Integer, Model.New) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) _
                                      (From e In CurrentDataContext.News Where e.Id = Id Select e).FirstOrDefault)

        Public Shared GetLatestNewsFunc As Func(Of ContentDataContext, Model.New) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      (From e In CurrentDataContext.News _
                                      Where e.Approved = True Select e _
                                      Order By e.PublishDate Ascending).FirstOrDefault)

        Public Shared GetNewsByPermalinkFunc As Func(Of ContentDataContext, String, Model.New) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Permalink As String) _
                                      (From e In CurrentDataContext.News Where e.Permalink Like Permalink Select e).FirstOrDefault)

        Public Shared GetNewsIdByPermalinkFunc As Func(Of ContentDataContext, String, Integer) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Permalink As String) _
                                      (From e In CurrentDataContext.News Where e.Permalink Like Permalink Select e.Id).FirstOrDefault)

        ''' <summary>
        ''' Creates the News with an image
        ''' </summary>
        ''' <param name="Title">The title.</param>
        ''' <param name="SubTitle">The sub title.</param>
        ''' <param name="Summary">The summary</param>
        ''' <param name="Permalink">The permalink for the news.</param>
        ''' <param name="Details">The news details</param>
        ''' <param name="ImageId">The image id</param>
        ''' <param name="PublishDate">The news publish date.</param>
        ''' <param name="Approved">if set to <c>true</c> [approved].</param>
        ''' <returns>An integer value representing the newly created news Id</returns>
        Public Shared Function Create(ByVal Title As String, ByVal SubTitle As String, ByVal Summary As String, ByVal Permalink As String, ByVal Details As String, ByVal ImageId As Integer, ByVal PublishDate As DateTime, ByVal Approved As Boolean) As Integer
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentNews As New Model.New
            Dim CurrentNewsId As Integer

            CurrentNews.Title = Title
            CurrentNews.SubTitle = SubTitle
            CurrentNews.Summary = Summary
            CurrentNews.Details = Details
            CurrentNews.ImageId = ImageId
            CurrentNews.SubmitDate = Date.Now
            CurrentNews.PublishDate = PublishDate
            CurrentNews.Approved = Approved
            CurrentNews.Permalink = Permalink

            CurrentDataContext.News.InsertOnSubmit(CurrentNews)
            CurrentDataContext.SubmitChanges()

            CurrentNewsId = CurrentNews.Id

            CurrentDataContext.Dispose()

            Return CurrentNewsId
        End Function

        ''' <summary>
        ''' Creates the News with out an image
        ''' </summary>
        ''' <param name="Title">The title.</param>
        ''' <param name="SubTitle">The sub title.</param>
        ''' <param name="Summary">The summary</param>
        ''' <param name="Permalink">The permalink for the news.</param>
        ''' <param name="Details">The news details</param>
        ''' <param name="PublishDate">The news publish date.</param>
        ''' <param name="Approved">if set to <c>true</c> [approved].</param>
        ''' <returns>An integer value representing the newly created news Id</returns>
        Public Shared Function Create(ByVal Title As String, ByVal SubTitle As String, ByVal Summary As String, ByVal Permalink As String, ByVal Details As String, ByVal PublishDate As DateTime, ByVal Approved As Boolean) As Integer
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentNews As New Model.New
            Dim CurrentNewsId As Integer

            CurrentNews.Title = Title
            CurrentNews.SubTitle = SubTitle
            CurrentNews.Summary = Summary
            CurrentNews.Details = Details
            CurrentNews.SubmitDate = Date.Now
            CurrentNews.PublishDate = PublishDate
            CurrentNews.Approved = Approved
            CurrentNews.Permalink = Permalink

            CurrentDataContext.News.InsertOnSubmit(CurrentNews)
            CurrentDataContext.SubmitChanges()

            CurrentNewsId = CurrentNews.Id

            CurrentDataContext.Dispose()

            Return CurrentNewsId
        End Function

        ''' <summary>
        ''' Updates the News with an image
        ''' </summary>
        ''' <param name="Id">The News Id</param>
        ''' <param name="Title">The title</param>
        ''' <param name="SubTitle">The sub title</param>
        ''' <param name="Summary">The summary</param>
        ''' <param name="Permalink">The permalink for the news</param>
        ''' <param name="Details">The news details</param>
        ''' <param name="ImageId">The image id</param>
        ''' <param name="PublishDate">The news publish date</param>
        Public Shared Sub Update(ByVal Id As Integer, ByVal Title As String, ByVal SubTitle As String, ByVal Summary As String, ByVal Permalink As String, ByVal Details As String, ByVal ImageId As Integer, ByVal PublishDate As DateTime)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentNews As Model.New

            CurrentNews = GetNewsByIdFunc(CurrentDataContext, Id)

            If CurrentNews Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("News does not exist")
            Else
                CurrentNews.Title = Title
                CurrentNews.SubTitle = SubTitle
                CurrentNews.Summary = Summary
                CurrentNews.Details = Details
                CurrentNews.ImageId = ImageId
                CurrentNews.SubmitDate = Date.Now
                CurrentNews.PublishDate = PublishDate
                CurrentNews.Permalink = Permalink


                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If
        End Sub

        ''' <summary>
        ''' Updates the News with an image
        ''' </summary>
        ''' <param name="Id">The News Id</param>
        ''' <param name="Title">The title</param>
        ''' <param name="SubTitle">The sub title</param>
        ''' <param name="Summary">The summary</param>
        ''' <param name="Permalink">The permalink for the news</param>
        ''' <param name="Details">The news details</param>
        ''' <param name="PublishDate">The news publish date</param>
        Public Shared Sub Update(ByVal Id As Integer, ByVal Title As String, ByVal SubTitle As String, ByVal Summary As String, ByVal Permalink As String, ByVal Details As String, ByVal PublishDate As DateTime)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentNews As Model.New

            CurrentNews = GetNewsByIdFunc(CurrentDataContext, Id)

            If CurrentNews Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("News does not exist")
            Else
                CurrentNews.Title = Title
                CurrentNews.SubTitle = SubTitle
                CurrentNews.Summary = Summary
                CurrentNews.Details = Details
                CurrentNews.SubmitDate = Date.Now
                CurrentNews.PublishDate = PublishDate
                CurrentNews.Permalink = Permalink

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If
        End Sub

        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Try
                Dim CurrentDataContext As New ContentDataContext
                Dim CurrentNews As Model.New

                CurrentNews = GetNewsByIdFunc(CurrentDataContext, Id)

                CurrentDataContext.News.DeleteOnSubmit(CurrentNews)
                CurrentDataContext.SubmitChanges()

                CurrentDataContext.Dispose()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Shared Sub RemovePicture(ByVal Id As Integer)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentNews As Model.New

            CurrentNews = GetNewsByIdFunc(CurrentDataContext, Id)

            If CurrentNews Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("News does not exist")
            Else
                CurrentNews.ImageId = 0

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If
        End Sub

        Public Shared Function GetItem(ByVal Id As Integer) As Model.New
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentNews As Model.New

            CurrentNews = GetNewsByIdFunc(CurrentDataContext, Id)

            If CurrentNews Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("News does not exist")
            Else
                GetItem = CurrentNews

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetItem(ByVal Permalink As String) As Model.New
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentNews As Model.New

            CurrentNews = GetNewsByPermalinkFunc(CurrentDataContext, Permalink)

            If CurrentNews Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("News does not exist")
            Else
                GetItem = CurrentNews

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetItems() As List(Of Model.New)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentNewsList As List(Of Model.New)

            CurrentNewsList = GetNewsFunc(CurrentDataContext).ToList

            If CurrentNewsList Is Nothing OrElse CurrentNewsList.Count < 1 Then
                CurrentDataContext.Dispose()

                Throw New Exception("There are currently no news articles")
            Else
                GetItems = CurrentNewsList

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetLatestNews() As Model.New
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentNews As Model.New

            CurrentNews = GetLatestNewsFunc(CurrentDataContext)

            If CurrentNews Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("News does not exist")
            Else
                GetLatestNews = CurrentNews

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetApprovedNews() As List(Of Model.New)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentNewsList As List(Of Model.New)

            CurrentNewsList = GetApprovedNewsFunc(CurrentDataContext).ToList

            If CurrentNewsList Is Nothing OrElse CurrentNewsList.Count < 1 Then
                CurrentDataContext.Dispose()

                Throw New Exception("There are currently no news articles")
            Else
                GetApprovedNews = CurrentNewsList

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Sub ApproveNews(ByVal Id As Integer)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentNews As Model.New = GetNewsByIdFunc(CurrentDataContext, Id)

            CurrentNews.Approved = True
            CurrentDataContext.SubmitChanges()

            CurrentDataContext.Dispose()
        End Sub

        Public Shared GetNewsTitleByIdFunc As Func(Of ContentDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) _
                                      (From e In CurrentDataContext.Events Where e.Id = Id Select e.Title).FirstOrDefault)

        Public Shared Function GetBreadCrumb(ByVal EventId As Integer) As String
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentNewsTitle As String = GetNewsTitleByIdFunc(CurrentDataContext, EventId)

            Dim BreadCrumbStringBuilder As New StringBuilder
            BreadCrumbStringBuilder.AppendFormat("<a href=""/"">Home</a> {0} ", Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("<a href=""/News/Index.html"">News</a> {0} ", Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("{0}", CurrentNewsTitle)

            Return BreadCrumbStringBuilder.ToString
        End Function

    End Class

End Namespace