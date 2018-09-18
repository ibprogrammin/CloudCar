Imports CloudCar.CCFramework
Imports CloudCar.CCFramework.ContentManagement.NewsModule
Imports CloudCar.CCFramework.Core

Namespace CCContentManagement.NewsModule

    Public Class NewsDetails
        Inherits RoutablePage

        Public ReadOnly Property Permalink() As String
            Get
                Return (From CurrentValues In RequestContext.RouteData.Values Where CurrentValues.Key = "permalink" Select New With {.id = CurrentValues.Value}).SingleOrDefault.id.ToString
            End Get
        End Property

        Private Sub LoadNews()
            Dim CurrentNews As Model.New = NewsController.GetItem(Permalink)

            If Not CurrentNews Is Nothing Then
                TitleLiteral.Text = CurrentNews.Title
                SubTitleLiteral.Text = CurrentNews.SubTitle
                PageDescriptionMeta.Attributes("content") = CurrentNews.Summary

                If Settings.EnableSSL = True And Settings.FullSSL Then
                    PageCanonicalMeta.Attributes("href") = String.Format("https://{0}/News/{1}", Settings.HostName, CurrentNews.Permalink)
                Else
                    PageCanonicalMeta.Attributes("href") = String.Format("http://{0}/News/{1}.html", Settings.HostName, CurrentNews.Permalink)
                End If

                Dim FirstParagraphIndex As Integer = CurrentNews.Details.IndexOf("<p>") + 3
                Dim PublishDataString As String = String.Format("<i>{0:dddd MMMM dd, yyyy}</i> - ", CurrentNews.PublishDate)

                ContentLiteral.Text = CurrentNews.Details.Insert(FirstParagraphIndex, PublishDataString)
                
                If CurrentNews.ImageId.HasValue Then
                    ThumbnailImage.ImageUrl = String.Format("/images/db/{0}/full/{1}.jpg", CurrentNews.ImageId, CurrentNews.Title.Replace(" ", "").Replace("'", ""))
                    ThumbnailImage.Visible = True
                End If

                Title = String.Format("{0}{1}", CurrentNews.Title, Settings.SiteTitle)

                EventBreadCrumbControl.PageId = CurrentNews.Id
            Else
                Throw New Exception("The News Id has not been set")
            End If
        End Sub

        Protected Overrides Sub OnInit(ByVal Args As EventArgs)
            LoadNews()

            MyBase.OnInit(Args)
        End Sub

    End Class

End Namespace