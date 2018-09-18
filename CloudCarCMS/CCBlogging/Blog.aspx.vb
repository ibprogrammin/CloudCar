Imports CloudCar.CCFramework.Blogging
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCBlogging

    Partial Public Class BlogPage
        Inherits RoutablePage

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Dim permalink As String = (From v In RequestContext.RouteData.Values Where v.Key = "permalink" Select New With {.id = v.Value}).SingleOrDefault.id.ToString

                LoadBlogEntry(permalink)
            End If
        End Sub

        Private Sub LoadBlogEntry(ByVal Permalink As String)
            Dim CurrentBlog As Blog = BlogController.GetBlogFromLink(Permalink)

            accAddComments.BlogID = CurrentBlog.id

            litTitle.Text = CurrentBlog.Title
            litAuthorDescription.Text = CurrentBlog.Author.Biography
            imgAuthorPortrait.Src = CurrentBlog.Author.AvatarURL
            litBlogContent.Text = CurrentBlog.BlogContent
            litDatePosted.Text = String.Format("{0:dddd MMMM dd, yyyy}", CurrentBlog.DatePosted)
            litSubHeading.Text = CurrentBlog.SubHeading

            If Settings.EnableSSL And Settings.FullSSL Then
                fblbControl.Href = String.Format("https://{0}/Blog/{1}.html", Settings.HostName, CurrentBlog.Permalink)
                fbccComments.Href = String.Format("https://{0}/Blog/{1}.html", Settings.HostName, CurrentBlog.Permalink)
            Else
                fblbControl.Href = String.Format("http://{0}/Blog/{1}.html", Settings.HostName, CurrentBlog.Permalink)
                fbccComments.Href = String.Format("http://{0}/Blog/{1}.html", Settings.HostName, CurrentBlog.Permalink)
            End If

            If Settings.EnableSSL = True And Settings.FullSSL Then
                PageCanonicalMeta.Attributes("href") = String.Format("https://{0}/Blog/{1}.html", Settings.HostName, CurrentBlog.Permalink)
            Else
                PageCanonicalMeta.Attributes("href") = String.Format("http://{0}/Blog/{1}.html", Settings.HostName, CurrentBlog.Permalink)
            End If

            If Not CurrentBlog.ImageLink = String.Empty And Not CurrentBlog.ImageLink = "" Then
                imgHeadingImage.Visible = True
                imgHeadingImage.Src = CurrentBlog.ImageLink
                imgHeadingImage.Alt = CurrentBlog.Title
            End If

            PageKeywordsMeta.Attributes("content") = CurrentBlog.Keywords
            PageDescriptionMeta.Attributes("content") = CurrentBlog.Description

            BreadCrumbLiteral.Text = TextFunctions.StripShortString(CurrentBlog.Title, 36)

            hlAuthor.HRef = String.Format("/Blog/{0}.html#Author", CurrentBlog.Permalink)
            hlComments.HRef = String.Format("/Blog/{0}.html#Comments", CurrentBlog.Permalink)

            hlAuthor.Title = String.Format("The author of {0}", CurrentBlog.Title)
            hlComments.Title = String.Format("View {0} comments", CurrentBlog.Title)

            MyBase.Title = CurrentBlog.Title & Settings.SiteTitle

            Dim db As New BlogDataContext
            Dim comments = CCFramework.Blogging.CommentController.GetApprovedBlogCommentsFunc(db, CurrentBlog.id)

            rptComments.DataSource = comments
            rptComments.DataBind()
        End Sub

    End Class
End Namespace