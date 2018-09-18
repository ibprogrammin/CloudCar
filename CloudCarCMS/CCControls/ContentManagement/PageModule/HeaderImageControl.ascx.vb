Imports CloudCar.CCFramework.ContentManagement

Namespace CCControls.ContentManagement.PageModule

    Public Class HeaderImageControl
        Inherits UserControl

        Public Property PageId() As Integer
            Get
                Return CInt(ViewState("PageId"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("PageId") = Value

                LoadHeaderImage()
            End Set
        End Property

        Private Sub LoadHeaderImage()
            Dim CurrentImageId As Integer = ContentPageController.GetPageHeaderImageId(PageId)
            Dim CurrentPermalink As String = ContentPageController.GetPagePermalink(PageId)

            If Not CurrentImageId = 0 Then
                HeaderImage.Alt = ContentPageController.GetPageContentTitle(PageId)
                HeaderImage.Src = String.Format("/images/db/{0}/920/{1}.jpg", CurrentImageId, CurrentPermalink)
                HeaderImage.Visible = True
            End If
        End Sub

    End Class

End Namespace