Imports CloudCar.CCFramework.Commerce
Imports System.ComponentModel

Namespace CCControls.Commerce

    Partial Public Class AddProductReviewControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            'ratRating.Attributes.Add("onclick", "return false;")

            If Not Page.IsPostBack Then
                'rcRecaptcha.PublicKey = Settings.ReCaptchaPublicKey
                'rcRecaptcha.PrivateKey = Settings.ReCaptchaPrivateKey
            End If
        End Sub

        Public Property ProductId() As Integer
            Get
                Return CInt(ViewState("ProductId"))
            End Get
            Set(ByVal Value As Integer)
                If CInt(ViewState("ProductId")) = Nothing Then
                    ViewState.Add("ProductId", Value)
                Else
                    ViewState("ProductId") = Value
                End If
            End Set
        End Property

        Private Sub CreateReviewButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles CreateReviewbutton.Click
            Dim CurrentName As String = NameTextBox.Text
            Dim CurrentEmail As String = EmailTextBox.Text
            Dim CurrentUrl As String = UrlTextBox.Text
            Dim CurrentComment As String = CommentTextBox.Text
            Dim CurrentRating As Integer = CInt(ProductRatingControl.CurrentRating)

            If Page.IsValid Then
                Dim CurrentProductReviewController As New ProductReviewController

                Dim CurrentReviewId As Integer = CurrentProductReviewController.Create(CurrentName, CurrentEmail, CurrentUrl, CurrentComment, CurrentRating, ProductId)

                If Not CurrentReviewId = Nothing And Not CurrentReviewId = 0 Then
                    MessageLabel.Text = String.Format("Your review was successfully submitted. Thank You!")
                    MessageLabel.Visible = True
                End If
            End If
        End Sub

    End Class

End Namespace