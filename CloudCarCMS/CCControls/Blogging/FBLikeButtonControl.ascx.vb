Namespace CCControls.Blogging

    Partial Public Class FBLikeButtonControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadLikeBoxControl()
            End If
        End Sub

        Private Sub LoadLikeBoxControl()
            Dim control As XElement = <iframe src=<%= "http://www.facebook.com/plugins/like.php?href=" & Server.HtmlEncode(Href) & "&layout=standard&show_faces=true&width=450&action=like&font=arial&colorscheme=light&height=30" %> scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:450px; height:30px;" allowTransparency="true"></iframe>

            litLikeButton.Text = control.ToString
        End Sub

        Public Property Href() As String
            Get
                If Not ViewState("Href") Is Nothing Then
                    Return CType(ViewState("Href"), String)
                Else
                    Return ""
                End If
            End Get
            Set(ByVal value As String)
                If Not ViewState("Href") Is Nothing Then
                    ViewState("Href") = value
                Else
                    ViewState.Add("Href", value)
                End If
            End Set
        End Property

    End Class

End Namespace