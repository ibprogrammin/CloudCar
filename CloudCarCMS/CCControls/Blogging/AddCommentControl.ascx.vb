Imports CloudCar.CCFramework.Blogging
Imports CloudCar.CCFramework.Core

Namespace CCControls.Blogging

    Partial Public Class AddCommentControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            'If Not Page.IsPostBack Then
            'CommentRecaptchaControl.PublicKey = Settings.ReCaptchaPublicKey
            'CommentRecaptchaControl.PrivateKey = Settings.ReCaptchaPrivateKey
            'End If
        End Sub

        Public Property BlogId() As Integer
            Get
                Return CInt(ViewState("BlogId"))
            End Get
            Set(ByVal Value As Integer)
                If ViewState("BlogId") = Nothing Then
                    ViewState.Add("BlogId", value)
                Else
                    ViewState("BlogId") = value
                End If
            End Set
        End Property

        Private Sub CreateCommentButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles CreateCommentButton.Command
            If Page.IsValid Then
                Dim CurrentName As String = NameTextBox.Text
                Dim CurrentEmail As String = EmailTextBox.Text
                Dim CurrentUrl As String = UrlTextBox.Text
                Dim CurrentComment As String = CommentTextBox.Text

                CommentController.CreateComment(BlogId, CurrentName, CurrentEmail, CurrentComment, CurrentUrl)
            End If
        End Sub

    End Class

End Namespace