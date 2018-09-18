Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCControls.Blogging

    Partial Public Class LatestBlogControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadLatestBlog()
            End If
        End Sub

        Private Sub LoadLatestBlog()
            Dim db As New BlogDataContext

            Dim blog As Blog = CCFramework.Blogging.BlogController.GetLatestBlogFunc(db)

            If Not blog Is Nothing Then
                litHeading.Text = blog.Title
                litDetails.Text = TextFunctions.StripShortString(blog.ContentSummary, 190)

                hlBlogLinkBottom.HRef = "/Blog/" & blog.Permalink & ".html"
                hlBlogLinkHeading.HRef = "/Blog/" & blog.Permalink & ".html"

                hlBlogLinkBottom.Title = blog.Title
                hlBlogLinkHeading.Title = blog.Title
            End If
        End Sub

    End Class

End Namespace