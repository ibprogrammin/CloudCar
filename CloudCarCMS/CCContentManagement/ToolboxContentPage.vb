Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement
Imports CloudCar.CCControls.ContentManagement
Imports CloudCar.CCControls.ContentManagement.PageModule

Namespace CCContentManagement

    Public Class ContentPageTemplate
        Inherits RoutablePage

        Protected WithEvents KeywordsTag As HtmlGenericControl
        Protected WithEvents DescriptionTag As HtmlGenericControl
        Protected WithEvents CanonicalTag As HtmlGenericControl
        Protected WithEvents HeadingContainer As Literal
        Protected WithEvents BannerPanel As Panel
        Protected WithEvents ImageNivoRotatorControl As NivoRotatorControl
        Protected WithEvents HeadingImage As HtmlImage
        Protected WithEvents MainContentPlaceHolder As PlaceHolder
        Protected WithEvents SideMenuPanel As Panel
        Protected WithEvents LinksContainer As Literal
        Protected WithEvents SecondaryContent As Literal
        Protected WithEvents BreadCrumbContainer As Literal
        Protected WithEvents ScriptsContainer As Literal

        Private Const HeaderImageWidth As Integer = 920

        Protected _Permalink As String
        Protected _CurrentContentPage As ContentPage

        Public Property Permalink() As String
            Get
                Return _Permalink
            End Get
            Set(ByVal Value As String)
                _Permalink = Value
                _CurrentContentPage = ContentPageController.GetPageFromLink(_Permalink)
            End Set
        End Property

        Public Property CurrentContentPage() As ContentPage
            Get
                Return _CurrentContentPage
            End Get
            Set(ByVal Value As ContentPage)
                _CurrentContentPage = Value
            End Set
        End Property

        Protected Overridable Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Try
                    LoadContentPage()
                Catch Ex As Exception
                    SecondaryContent.Text = String.Format("Oops! There was an error loading the current page!")
                End Try
            End If
        End Sub

        Private Sub LoadContentPage()
            ImageNivoRotatorControl.PageID = CurrentContentPage.id

            If ImageNivoRotatorControl.FindControl("phRotator").Visible = True Then
                BannerPanel.Visible = True
            Else
                BannerPanel.Visible = False
            End If

            BreadCrumbContainer.Text = ContentPageController.GetBreadCrumb(CurrentContentPage.id, "Home")
            HeadingContainer.Text = CurrentContentPage.contentTitle
            ScriptsContainer.Text = CurrentContentPage.script
            SecondaryContent.Text = CurrentContentPage.secondaryContent

            If CurrentContentPage.headerImageID.HasValue And Not CurrentContentPage.headerImageID.Value = 0 Then
                HeadingImage.Alt = CurrentContentPage.contentTitle
                HeadingImage.Src = String.Format("/images/db/{0}/{1}/{2}", CurrentContentPage.headerImageID, HeaderImageWidth.ToString(), CCFramework.Core.PictureController.GetPictureFilename(CurrentContentPage.headerImageID.Value))
                HeadingImage.Visible = True
            End If

            KeywordsTag.Attributes("content") = CurrentContentPage.keywords
            DescriptionTag.Attributes("content") = CurrentContentPage.description

            If CCFramework.Core.Settings.EnableSSL = True And CCFramework.Core.Settings.FullSSL Then
                CanonicalTag.Attributes("href") = String.Format("https://{0}/{1}.html", CCFramework.Core.Settings.HostName, CurrentContentPage.permalink)
            Else
                CanonicalTag.Attributes("href") = String.Format("http://{0}/{1}.html", CCFramework.Core.Settings.HostName, CurrentContentPage.permalink)
            End If

            Dim NestedPages = ContentPageController.GetNestedPages(CurrentContentPage.id)
            If NestedPages.Count > 0 Then
                SideMenuPanel.Visible = True

                For Each item In NestedPages
                    LinksContainer.Text &= String.Format("<h4><a href=""/Home/{0}.html"" title=""{1}"">{2}</a></h4><br />", item.permalink, item.pageTitle, item.breadcrumbTitle)
                Next
            End If

            Title = CurrentContentPage.pageTitle & CCFramework.Core.Settings.SiteTitle
        End Sub

        Private Sub LoadContent()
            If CurrentContentPage.pageContent.Contains("<#!COMMENTBOX!#>") Or CurrentContentPage.pageContent.Contains("<#!NEWSBOX!#>") Then
                Dim CurrentSeperatorStrings As String() = {"<#!", "!#>"}
                Dim CurrentPageContent As String() = CurrentContentPage.pageContent.Split(CurrentSeperatorStrings, StringSplitOptions.None)

                For Each Item As String In CurrentPageContent
                    Select Case Item
                        Case "COMMENTBOX"
                            Dim CurrentCommentBox As ContactControl = CType(LoadControl("~/CCControls/ContentManagement/ContactControl.ascx"), ContactControl)
                            CurrentCommentBox.ID = "cbComments"
                            MainContentPlaceHolder.Controls.Add(CurrentCommentBox)
                        Case "NEWSBOX"
                            Dim CurrentNewsBox As RecentEventAndNewsControl = CType(LoadControl("~/CCControls/ContentManagement/RecentEventAndNewsControl.ascx"), RecentEventAndNewsControl)
                            CurrentNewsBox.ID = "cbNews"
                            MainContentPlaceHolder.Controls.Add(CurrentNewsBox)
                        Case Else
                            MainContentPlaceHolder.Controls.Add(New Literal() With {.Text = Item})
                    End Select
                Next
            Else
                MainContentPlaceHolder.Controls.Add(New Literal() With {.Text = CurrentContentPage.pageContent})
            End If
        End Sub

        'Make sure to recreate any custom controls on each request or they will be unable to post any data.

        Protected Overrides Sub OnInit(ByVal E As EventArgs)
            MyBase.OnInit(E)

            If _Permalink Is Nothing Then
                _Permalink = (From v In RequestContext.RouteData.Values Where v.Key = "permalink" Select New With {.id = v.Value}).SingleOrDefault.id.ToString
                _CurrentContentPage = ContentPageController.GetPageFromLink(_Permalink)
            End If

            LoadContent()
        End Sub

    End Class

End Namespace