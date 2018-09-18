Imports CloudCar.CCFramework.Model

Namespace CCControls

    Partial Public Class LinkControl
        Inherits UserControl
        Private _linkId As Integer

        Public Property LinkID() As Integer
            Get
                Return _linkId
            End Get
            Set(ByVal value As Integer)
                _linkId = value
            End Set
        End Property

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If Not _linkId = Nothing Then
                Dim link As Link = CCFramework.ContentManagement.LinksController.GetLink(_linkId)
                Dim url As String

                hlLink.Text = link.LinksTitle.Replace(vbNewLine, "<br />")

                If link.LinkURL.StartsWith("http://") Or link.LinkURL.StartsWith("https://") Then
                    url = link.LinkURL
                Else
                    url = "http://" & link.LinkURL
                End If

                If link.PictureID.HasValue Then
                    imgLinkImage.Src = "/images/db/" & link.PictureID & "/650/" & link.Picture.PictureFileName
                    imgLinkImage.Alt = link.LinksTitle

                    hlImageLink.HRef = url
                    hlImageLink.Title = link.LinksTitle
                Else
                    imgLinkImage.Visible = False
                    hlImageLink.Visible = False
                End If

                hlLink.NavigateUrl = url

                lblDescription.Text = link.LinkNotes

            End If
        End Sub

    End Class

End Namespace