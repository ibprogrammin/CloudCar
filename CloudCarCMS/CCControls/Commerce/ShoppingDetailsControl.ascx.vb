Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce.ShoppingCart

Namespace CCControls.Commerce

    Partial Public Class ShoppingDetailsControl
        Inherits UserControl

        Public Event ShoppingCartModified(ByVal Sender As Object, ByVal E As EventArgs)

        Protected Overrides Sub OnLoad(ByVal E As EventArgs)
            If Not Page.IsPostBack Then
                RaiseEvent ShoppingCartModified(Me, E)
            End If
        End Sub

        Public Sub ShoppingDetailsControlShoppingCartModified(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.ShoppingCartModified
            LoadDetails()
        End Sub

        Friend Sub LoadDetails()
            Dim Total As Decimal
            Dim Quantity As Integer

            If Not Membership.GetUser Is Nothing Then
                Dim CurrentRegisteredUserController As New RegisteredUserController
                Dim CurrentRegisteredUser As RegisteredUser = CurrentRegisteredUserController.GetByUserName(Membership.GetUser.UserName)
                CurrentRegisteredUserController.Dispose()

                Dim PriceLevel As EPriceLevel = CType(CurrentRegisteredUser.PriceLevel, EPriceLevel)

                Total = ShoppingCartController.GetSubTotal(CurrentRegisteredUser.UserID, PriceLevel)
                Quantity = ShoppingCartController.GetItemCount(CurrentRegisteredUser.UserID)

                If Quantity > 0 Then
                    Dim CurrentDataContext As New CommerceDataContext
                    MiniCartItemsRepeater.DataSource = ShoppingCartHelper.GetShoppingCartSummaryItemsByUserId(CurrentDataContext, CurrentRegisteredUser.UserID).ToList
                    MiniCartItemsRepeater.DataBind()
                    CurrentDataContext.Dispose()

                    CartMiniItemsLabel.Visible = True
                    CartMiniTotalLabel.Visible = True
                    CartMiniButtonsLabel.Visible = True

                    TotalLiteral.Text = String.Format("{0:C}", Total)
                Else
                    CartEmptyLabel.Visible = True
                End If
            Else
                Dim CurrentSessionId As String = Session("SessionId").ToString

                Total = ShoppingCartController.GetSubTotal(CurrentSessionId, EPriceLevel.Regular)
                Quantity = ShoppingCartController.GetItemCount(CurrentSessionId)

                If Quantity > 0 Then
                    Dim CurrentDataContext As New CommerceDataContext
                    MiniCartItemsRepeater.DataSource = ShoppingCartHelper.GetShoppingCartSummaryItemsBySessionId(CurrentDataContext, CurrentSessionId).ToList
                    MiniCartItemsRepeater.DataBind()
                    CurrentDataContext.Dispose()

                    CartMiniItemsLabel.Visible = True
                    CartMiniTotalLabel.Visible = True
                    CartMiniButtonsLabel.Visible = True

                    TotalLiteral.Text = String.Format("{0:C}", Total)
                Else
                    CartEmptyLabel.Visible = True
                End If
            End If

            litItems.Text = Quantity.ToString
            litTotal.Text = Total.ToString("C")
        End Sub

    End Class

End Namespace