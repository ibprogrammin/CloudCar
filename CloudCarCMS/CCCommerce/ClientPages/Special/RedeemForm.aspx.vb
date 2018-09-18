Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.ShoppingCart

Namespace CCCommerce.Special

    Partial Public Class RedeemForm
        Inherits Page

        Private Sub RedeemForm_Load(ByVal Sender As Object, ByVal E As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                If Not Request.QueryString("RC") Is Nothing And Not Request.QueryString("RC") = String.Empty Then
                    txtRedemptionCode.Text = Request.QueryString("RC")
                End If
            End If
        End Sub

        Private Sub btnSubmit_Command(ByVal Sender As Object, ByVal E As CommandEventArgs) Handles btnSubmit.Command
            Dim RedemptionCode As String = txtRedemptionCode.Text

            If Not RedemptionCode = String.Empty Then
                Dim RedemptionRequest As SalesRequest = SalesRequestController.GetRedemptionRequestByCode(RedemptionCode)

                If Not RedemptionRequest Is Nothing Then
                    Dim ShoppingCart As New ShoppingCartController()
                    Dim ShoppingCartId As Integer

                    If SalesRequestController.CheckCodeRedeemed(RedemptionCode) Then
                        lblError.Text = "Sorry! The code you entered has already been redeemed."
                        lblError.Visible = True
                    Else
                        If SalesRequestController.CheckCodeInUse(RedemptionCode) Then
                            ShoppingCart.Delete(RedemptionRequest.ShoppingCartId)
                        End If

                        If Not System.Web.Security.Membership.GetUser Is Nothing Then
                            Dim CurrentUser As RegisteredUser = New CCFramework.Core.RegisteredUserController().GetByUserName(System.Web.Security.Membership.GetUser.UserName)

                            If ShoppingCartController.CartHasProduct(CurrentUser.UserID, RedemptionRequest.ProductId) Then
                                lblError.Text = "You have already added that to your shopping cart."
                                lblError.Visible = True
                            Else
                                ShoppingCartId = ShoppingCart.Create(Session("SessionId").ToString, CurrentUser.UserID, RedemptionRequest.ProductId, CCFramework.Core.Settings.NoColorID, CCFramework.Core.Settings.NoSizeID, RedemptionRequest.Quantity)
                            End If
                        Else
                            Dim SessionId As String = Session("SessionId").ToString

                            If ShoppingCartController.CartHasProduct(SessionId, RedemptionRequest.ProductId) Then
                                lblError.Text = "You have already added that to your shopping cart."
                                lblError.Visible = True
                            Else
                                ShoppingCartId = ShoppingCart.Create(SessionId, -1, RedemptionRequest.ProductId, CCFramework.Core.Settings.NoColorID, CCFramework.Core.Settings.NoSizeID, RedemptionRequest.Quantity)
                            End If
                        End If

                        If Not ShoppingCartId = Nothing And ShoppingCartId > 0 Then
                            SalesRequestController.SetRedemptionRequestShoppingCart(RedemptionRequest.RequestKey.ToString, ShoppingCartId)

                            lblError.Text = "Thank you! The product has been added to your shopping cart."
                            lblError.Visible = True
                        End If
                    End If
                End If
            Else
                lblError.Text = "You didn't enter a code."
                lblError.Visible = True
            End If
        End Sub

    End Class

End NameSpace