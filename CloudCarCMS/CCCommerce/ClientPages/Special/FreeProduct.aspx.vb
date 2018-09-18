Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.ShoppingCart
Imports CloudCar.CCFramework.Core

Namespace CCCommerce.Special

    Partial Public Class FreeProduct
        Inherits Page

        Private ReadOnly _freeProductId As Integer = Integer.Parse(ConfigurationManager.AppSettings("FreeProductID"))
        Private ReadOnly _freeProductQuantity As Integer = Integer.Parse(ConfigurationManager.AppSettings("FreeProductQuantity"))
        Private ReadOnly _freeProductCode As String = ConfigurationManager.AppSettings("FreeProductCode")

        Private ReadOnly _noColorId As Integer = Integer.Parse(ConfigurationManager.AppSettings("NoColorID"))
        Private ReadOnly _noSizeId As Integer = Integer.Parse(ConfigurationManager.AppSettings("NoSizeID"))

        Private Sub btnSubmit_Command(ByVal sender As Object, ByVal e As CommandEventArgs) Handles btnSubmit.Command
            Dim PromoCode As String = txtPromoCode.Text

            If Not PromoCode = String.Empty And PromoCode = _freeProductCode Then
                Dim ShoppingCart As New ShoppingCartController()

                If Not System.Web.Security.Membership.GetUser Is Nothing Then
                    Dim CurrentUser As RegisteredUser = New RegisteredUserController().GetByUserName(System.Web.Security.Membership.GetUser.UserName)

                    If ShoppingCartController.CartHasProduct(CurrentUser.UserID, _freeProductId) Then
                        lblError.Text = "You have already added that to your shopping cart."
                        lblError.Visible = True
                    Else
                        ShoppingCart.Create(Session("SessionId").ToString, CurrentUser.UserID, _freeProductId, _noColorId, _noSizeId, _freeProductQuantity)

                        lblError.Text = "Thank you! Your product has been added to your shopping cart."
                        lblError.Visible = True
                    End If
                Else
                    Dim SessionId As String = Session("SessionId").ToString

                    If ShoppingCartController.CartHasProduct(SessionId, _freeProductId) Then
                        lblError.Text = "You have already added that to your shopping cart."
                        lblError.Visible = True
                    Else
                        ShoppingCart.Create(SessionId, -1, _freeProductId, _noColorId, _noSizeId, _freeProductQuantity)

                        lblError.Text = "Thank you! Your product has been added to your shopping cart."
                        lblError.Visible = True
                    End If
                End If
            Else
                lblError.Text = "You didn't enter your promo code."
                lblError.Visible = True
            End If
        End Sub

    End Class
End NameSpace