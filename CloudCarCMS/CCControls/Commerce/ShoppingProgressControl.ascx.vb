Imports System.ComponentModel

Namespace CCControls.Commerce

    Public Class ShoppingProgressControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadProgressBar()
            End If
        End Sub

        Private Sub LoadProgressBar()
            Select Case Progress
                Case ShoppingProgress.Browse
                    BrowseStoreButton.CssClass = "progress-browse-button-active"
                    ShoppingCartButton.CssClass = "progress-shopping-cart-button"
                    CheckoutButton.CssClass = "progress-checkout-button"
                    ThankYouButton.CssClass = "progress-thank-you-button"

                    BrowseStoreButton.NavigateUrl = "/Shop/Index.html"
                    ShoppingCartButton.NavigateUrl = "/Shop/ShoppingCart.html"
                    CheckoutButton.NavigateUrl = ""
                    ThankYouButton.NavigateUrl = ""
                Case ShoppingProgress.ShoppingCart
                    BrowseStoreButton.CssClass = "progress-browse-button-visited"
                    ShoppingCartButton.CssClass = "progress-shopping-cart-button-active"
                    CheckoutButton.CssClass = "progress-checkout-button"
                    ThankYouButton.CssClass = "progress-thank-you-button"

                    BrowseStoreButton.NavigateUrl = "/Shop/Index.html"
                    ShoppingCartButton.NavigateUrl = "/Shop/ShoppingCart.html"
                    CheckoutButton.NavigateUrl = "/Shop/Checkout.html"
                    ThankYouButton.NavigateUrl = ""
                Case ShoppingProgress.CheckOut
                    BrowseStoreButton.CssClass = "progress-browse-button-visited"
                    ShoppingCartButton.CssClass = "progress-shopping-cart-button-visited"
                    CheckoutButton.CssClass = "progress-checkout-button-active"
                    ThankYouButton.CssClass = "progress-thank-you-button"

                    BrowseStoreButton.NavigateUrl = "/Shop/Index.html"
                    ShoppingCartButton.NavigateUrl = "/Shop/ShoppingCart.html"
                    CheckoutButton.NavigateUrl = "/Shop/Checkout.html"
                    ThankYouButton.NavigateUrl = ""
                Case ShoppingProgress.ThankYou
                    BrowseStoreButton.CssClass = "progress-browse-button-visited"
                    ShoppingCartButton.CssClass = "progress-shopping-cart-button-visited"
                    CheckoutButton.CssClass = "progress-checkout-button-visited"
                    ThankYouButton.CssClass = "progress-thank-you-button-active"

                    BrowseStoreButton.NavigateUrl = "/Shop/Index.html"
                    ShoppingCartButton.NavigateUrl = "/Shop/ShoppingCart.html"
                    CheckoutButton.NavigateUrl = "/Shop/Checkout.html"
                    ThankYouButton.NavigateUrl = "/Shop/ThankYou.html"
                Case Else
                    BrowseStoreButton.CssClass = "progress-browse-button"
                    ShoppingCartButton.CssClass = "progress-shopping-cart-button"
                    CheckoutButton.CssClass = "progress-checkout-button"
                    ThankYouButton.CssClass = "progress-thank-you-button"

                    BrowseStoreButton.NavigateUrl = "/Shop/Index.html"
                    ShoppingCartButton.NavigateUrl = ""
                    CheckoutButton.NavigateUrl = ""
                    ThankYouButton.NavigateUrl = ""
            End Select
        End Sub

        <Bindable(True)> _
        <Category("Attributes")> _
        <DefaultValue("")> _
        <Localizable(True)> _
        Public Property Progress As ShoppingProgress
            Get
                Return _Progress
            End Get
            Set(ByVal Value As ShoppingProgress)
                _Progress = Value
            End Set
        End Property
        Private _Progress As ShoppingProgress

        Protected Overrides Sub OnInit(ByVal Args As EventArgs)
            Page.RegisterRequiresControlState(Me)
            MyBase.OnInit(Args)
        End Sub

        Protected Overrides Sub LoadControlState(ByVal SavedState As Object)
            Dim Value As Pair = CType(SavedState, Pair)
            Progress = CType(Value.Second, ShoppingProgress)
        End Sub

        Protected Overrides Function SaveControlState() As Object
            Dim BaseState As Object = MyBase.SaveControlState()
            Return New Pair(BaseState, Progress)
        End Function

    End Class

    Public Enum ShoppingProgress
        Browse = 0
        ShoppingCart = 1
        CheckOut = 2
        ThankYou = 3
    End Enum

End Namespace