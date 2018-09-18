Imports CloudCar.CCFramework.ContentManagement.NewsModule
Imports CloudCar.CCFramework

Namespace CCControls.ContentManagement.NewsModule

    Public Class NewsDetailsControl
        Inherits UserControl

        Public Property NewsId() As Integer
            Get
                Return CInt(ViewState("NewsId"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("NewsId") = Value
                LoadNews()
            End Set
        End Property

        Private Sub LoadNews()
            If Not NewsId = Nothing Then
                Dim CurrentNews As Model.New = NewsController.GetItem(NewsId)

                TitleLiteral.Text = CurrentNews.Title
                SubTitleLiteral.Text = CurrentNews.SubTitle
                ContentLiteral.Text = CurrentNews.Details

                If CurrentNews.ImageId.HasValue Then
                    ThumbnailImage.ImageUrl = String.Format("/images/db/{0}/200/{1}.jpg", CurrentNews.ImageId, CurrentNews.Title.Replace(" ", ""))
                    ThumbnailImage.Visible = True
                End If
            Else
                Throw New Exception("The News Id has not been set")
            End If
        End Sub

    End Class

End Namespace