Imports CloudCar.CCFramework.Model

Namespace CCControls

    Partial Public Class SocialMedia
        Inherits UserControl

        Protected Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadBlog()
            End If
        End Sub

        Private Sub LoadBlog()
            Dim LatestBlog As Blog = CCFramework.Blogging.BlogController.GetLiveBlogs.OrderByDescending(Function(t) t.DatePosted).FirstOrDefault

            If Not LatestBlog Is Nothing Then
                litBlogTitle.Text = LatestBlog.Title
                litAuthor.Text = LatestBlog.Author.Name
                litBlogSummary.Text = LatestBlog.ContentSummary
                litDataPosted.Text = LatestBlog.DatePosted.ToLongDateString

                If Not LatestBlog.ImageLink = Nothing And Not LatestBlog.ImageLink = String.Empty Then
                    imgHeadingImage.ImageUrl = LatestBlog.ImageLink
                    imgHeadingImage.Visible = True
                End If

                hlBlogTitle.HRef = "/Blog/" & LatestBlog.Permalink & ".html"
                hlJumpToBlog.HRef = "/Blog/" & LatestBlog.Permalink & ".html"
                'hlImageLink.HRef = "/Blog/" & latestBlog.Permalink & ".html"

                hlBlogTitle.Title = LatestBlog.Title
                hlJumpToBlog.Title = LatestBlog.Title
                'hlImageLink.Title = latestBlog.Title
            End If
        End Sub

    End Class

End Namespace