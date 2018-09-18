Imports CloudCar.CCFramework.Core.BeanStreamBilling
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.PaymentProcessing
Imports CloudCar.CCFramework.Core
Imports System.Net.Mail

Namespace CCAdmin.Commerce

    Partial Public Class OrderDetails
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Dim OrderId As Integer

                If Integer.TryParse(Request.QueryString("Order"), OrderId) Then
                    LoadOrder(OrderId)
                End If
            End If
        End Sub

        Private Sub LoadOrder(ByVal OrderId As Integer)
            Dim CurrentOrderController As New OrderController
            Dim CurrentRegisteredUserController As New RegisteredUserController
            Dim CurrentSimpleUserController As New SimpleUserController
            Dim CurrentAddressController As New AddressController
            
            Dim CurrentOrder As Order = CurrentOrderController.GetElement(OrderId)
            Dim CurrentSimpleUser As SimpleUser = CurrentSimpleUserController.GetElement(CurrentOrder.UserID)
            Dim CurrentShippingAddress As Address = CurrentAddressController.GetElement(CurrentOrder.SAddressID)
            Dim CurrentBillingAddress As Address = CurrentAddressController.GetElement(CurrentOrder.BAddressID)
            Dim CurrentShippingProvince As Province = CurrentShippingAddress.Province
            Dim CurrentBillingProvince As Province = CurrentBillingAddress.Province
            Dim CurrentShippingCountry As Country = CurrentShippingAddress.Province.Country
            Dim CurrentBillingCountry As Country = CurrentBillingAddress.Province.Country
            Dim CurrentRegisteredUser As RegisteredUser

            OrderIdHiddenField.Value = OrderId.ToString
            OrderIdLiteral.Text = CurrentOrder.ID.ToString

            Try
                CurrentRegisteredUser = CurrentRegisteredUserController.GetByUserID(CurrentSimpleUser.ID)

                UserNameHyperLink.Text = CurrentRegisteredUser.UserName
                UserNameHyperLink.NavigateUrl = String.Format("~/CCAdmin/Users.aspx?User={0}", CurrentSimpleUser.ID)
            Catch CurrentException As Exception
                UserNameHyperLink.Text = String.Format("User is not registered")
            End Try

            NameLiteral.Text = String.Format("{0} {1} {2}", CurrentSimpleUser.FirstName, CurrentSimpleUser.MiddleName, CurrentSimpleUser.LastName)
            EmailAddressHyperLink.Text = CurrentSimpleUser.Email
            EmailAddressHyperLink.NavigateUrl = String.Format("mailto:{0}", CurrentSimpleUser.Email)
            PhoneNumberLiteral.Text = CurrentSimpleUser.PhoneNumber

            ShippingAddressLiteral.Text = String.Format("{0} {1}, {2}<br />{3}, {4}", CurrentShippingAddress.Address, CurrentShippingAddress.City, CurrentShippingProvince.Name, CurrentShippingCountry.Name, CurrentShippingAddress.PCZIP)
            BillingAddressLiteral.Text = String.Format("{0} {1}, {2}<br />{3}, {4}", CurrentBillingAddress.Address, CurrentBillingAddress.City, CurrentBillingProvince.Name, CurrentBillingCountry.Name, CurrentBillingAddress.PCZIP)

            EditShippingAddressHyperLink.NavigateUrl = String.Format("~/CCAdmin/Commerce/Addresses.aspx?Address={0}&Order={1}", CurrentShippingAddress.ID, CurrentOrder.ID)
            EditBillingAddressHyperLink.NavigateUrl = String.Format("~/CCAdmin/Commerce/Addresses.aspx?Address={0}&Order={1}", CurrentBillingAddress.ID, CurrentOrder.ID)

            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentOrderItemDetails As List(Of OrderItemSummary) = OrderItemController.GetOrderItemsSummaryFunc(CurrentDataContext, CurrentOrder.ID).ToList

            Dim CurrentOrderShipped As Boolean = True

            For Each CurrentItem In From OrderItemDetail In CurrentOrderItemDetails Where Not OrderItemDetail.Shipped
                CurrentOrderShipped = False
            Next

            If CurrentOrderShipped Then
                OrderTrackingNumberTextBox.Enabled = False
                ShipOrderButton.Enabled = False
            End If

            OrderItemsGridView.DataSource = CurrentOrderItemDetails
            OrderItemsGridView.DataBind()

            Dim CurrentPaymentType As EPaymentType = CType(CurrentOrder.PaymentType, EPaymentType)

            Select Case CurrentPaymentType
                Case EPaymentType.None
                Case EPaymentType.CreditCard
                    Dim CurrentCreditCardPaymentController As New CreditCardPaymentController
                    Dim CurrentCreditCardTypeController As New CreditCardTypeController

                    Try
                        Dim CurrentCreditCardPayment As CreditCardPayment = CurrentCreditCardPaymentController.GetOrderPayment(CurrentOrder.ID)
                        Dim CurrentCreditCardType As CreditCardType = CurrentCreditCardTypeController.GetElement(CurrentCreditCardPayment.CCTypeID)

                        CardTypeLiteral.Text = CurrentCreditCardType.Type
                        CardNumberLiteral.Text = CurrentCreditCardPayment.CCNumber
                        If CurrentCreditCardPayment.Charge.HasValue Then
                            CardChargeLiteral.Text = CurrentCreditCardPayment.Charge.Value.ToString("C")
                        End If
                        CreditCardPaymentInfoPlaceHolder.Visible = True
                    Catch CurrentException As Exception
                        StatusMessageLabel.Text = String.Format("There was an error loading the payment information for this order.")
                        StatusMessageLabel.Visible = True
                    End Try

                    CurrentCreditCardTypeController.Dispose()
                    CurrentCreditCardPaymentController.Dispose()
                Case EPaymentType.Paypal
                Case EPaymentType.BeanStream
                    Dim CurrentBeanStreamPaymentController As New PaymentController

                    Try
                        Dim CurrentCreditCardPayment As BeanStreamPayment = CurrentBeanStreamPaymentController.GetOrderPayment(CurrentOrder.ID)
                        Dim CurrentEasyCryptography As New EasyCryptography()

                        Dim CurrentTransactionState As PaymentStatus = CType(CurrentCreditCardPayment.TransactionState, PaymentStatus)

                        CardTypeLiteral.Text = String.Format("Credit Card ({0})", PaymentStatusFunctions.GetPaymentStatusMessage(CurrentTransactionState))
                        CardNumberLiteral.Text = CCFunctions.HideCardNumber(CurrentEasyCryptography.Decrypt(CurrentCreditCardPayment.CCNumber, CurrentCreditCardPayment.CCEncryptionKey))
                        CardChargeLiteral.Text = CurrentCreditCardPayment.Charge.ToString("C")
                        
                        If Not CurrentTransactionState = PaymentStatus.Approved Then
                            PaymentMessageLiteral.Text = CurrentCreditCardPayment.MessageText.Replace("+", " ")
                            PaymentMessageLiteral.Visible = True
                        End If

                        If Not String.IsNullOrEmpty(CurrentCreditCardPayment.AuthCode) Then
                            OrderAuthorizationNumberTextBox.Text = CurrentCreditCardPayment.AuthCode
                            OrderAuthorizationNumberTextBox.Enabled = False
                            AuthorizeOrderButton.Enabled = False
                        End If

                        CreditCardPaymentInfoPlaceHolder.Visible = True
                    Catch CurrentException As Exception
                        StatusMessageLabel.Text = String.Format("There was an error loading the payment information for this order.")
                        StatusMessageLabel.Visible = True
                    End Try

                    CurrentBeanStreamPaymentController.Dispose()
            End Select

            'For Each item In CurrentOrderItemDetails
            'CurrentOrderSubTotal += item.Price
            'Next

            Dim CurrentOrderSubTotal As Decimal = OrderController.GetOrderSubTotal(CurrentOrder.ID)
            Dim CurrentShippingCharge As Decimal = CurrentOrder.ShipCharge
            Dim CurrentDiscount As Decimal = CurrentOrder.Discount
            Dim CurrentTaxes As Decimal = CDec(((CurrentOrderSubTotal - CurrentDiscount) + CurrentShippingCharge) * CurrentOrder.TaxPercent)
            Dim CurrentOrderTotal As Decimal = (CurrentOrderSubTotal - CurrentDiscount) + CurrentShippingCharge + CurrentTaxes

            SubtotalLiteral.Text = CurrentOrderSubTotal.ToString("C")
            DiscountLiteral.Text = CurrentDiscount.ToString("C")
            SummaryShippingChargeLiteral.Text = CurrentShippingCharge.ToString("C")
            TaxRateLiteral.Text = CurrentOrder.TaxPercent.ToString("P")
            TaxesLiteral.Text = CurrentTaxes.ToString("C")
            TotalLiteral.Text = CurrentOrderTotal.ToString("C")

            ShippingCompanyLiteral.Text = CurrentOrder.ShipCompany
            ShippingServiceLiteral.Text = CurrentOrder.ShipService
            ShippingChargeLiteral.Text = CurrentOrder.ShipCharge.ToString

            Dim PickupAddress As String = OrderController.GetOrderPickupAddress(CurrentOrder.SAddressID)
            BranchInfoLiteral.Text = PickupAddress

            Dim CurrentDownloadableProducts As List(Of ProductDownloadDetail) = ProductDownloadController.GetProductDownloadDetailsByOrderID(CurrentDataContext, OrderId).ToList
            If Not CurrentDownloadableProducts Is Nothing AndAlso CurrentDownloadableProducts.Count > 0 Then
                AvailableDownloadsRepeater.Visible = True
                AvailableDownloadsRepeater.DataSource = CurrentDownloadableProducts
                AvailableDownloadsRepeater.DataBind()
            End If

            CurrentOrderController.Dispose()
            CurrentRegisteredUserController.Dispose()
            CurrentSimpleUserController.Dispose()
            CurrentAddressController.Dispose()
        End Sub

        Private Sub ReturnOrder(ByVal OrderId As Integer)
            If OrderController.ReturnOrder(OrderId) Then
                LoadOrder(OrderId)

                StatusMessageLabel.Visible = True
                StatusMessageLabel.Text = String.Format("The order has been successfully returned! Thank you.")
            Else
                StatusMessageLabel.Visible = True
                StatusMessageLabel.Text = String.Format("We're sorry! There has been an error returning the order.")
            End If
        End Sub

        Private Sub DeleteOrder(ByVal OrderId As Integer)
            If OrderController.DeleteOrder(OrderId) Then
                StatusMessageLabel.Visible = True
                StatusMessageLabel.Text = String.Format("The order has been successfully deleted! Thank you.")
            Else
                StatusMessageLabel.Visible = True
                StatusMessageLabel.Text = String.Format("We're sorry! There has been an error deleting the order.")
            End If
        End Sub

        Private Sub DeleteItem(ByVal ItemId As Integer)
            Dim CurrentOrderItemController As New OrderItemController
            Dim DeleteItem As Boolean = CurrentOrderItemController.Delete(ItemId)
            CurrentOrderItemController.Dispose()

            If DeleteItem Then
                StatusMessageLabel.Text = String.Format("The line item has been successfully deleted.")
                StatusMessageLabel.Visible = True
            End If
        End Sub

        Private Sub ReturnItem(ByVal ItemId As Integer)
            Dim ReturnItem As Boolean = OrderItemController.ReturnItem(ItemId)

            If ReturnItem Then
                StatusMessageLabel.Text = String.Format("The line item has been successfully returned.")
                StatusMessageLabel.Visible = True
            End If
        End Sub

        Private Sub ShipOrder()
            Dim CurrentOrderId As Integer
            Dim CurrentTrackingNumber As String = OrderTrackingNumberTextBox.Text

            If Integer.TryParse(OrderIdHiddenField.Value, CurrentOrderId) AndAlso Not String.IsNullOrEmpty(CurrentTrackingNumber) Then
                Dim CurrentDataContext As New CommerceDataContext

                Dim CurrentOrderItems = OrderItemController.GetOrderItemsByOrderIdFunc(CurrentDataContext, CurrentOrderId)

                For Each CurrentOrderItem As OrderItem In CurrentOrderItems
                    CurrentOrderItem.Shipped = True
                    CurrentOrderItem.ShippedDate = DateTime.Now
                    CurrentOrderItem.TrackingNum = CurrentTrackingNumber
                Next

                Dim CurrentOrder = OrderController.GetOrderByIdFunc(CurrentDataContext, CurrentOrderId)
                CurrentOrder.ShipDate = Date.Now

                CurrentDataContext.SubmitChanges()

                Dim CurrentSimpleUserController As New SimpleUserController
                Dim CurrentSimpleUser As SimpleUser = CurrentSimpleUserController.GetElement(CurrentOrder.UserID)
                CurrentSimpleUserController.Dispose()

                If CCFramework.Core.Settings.SendTrackingEmails Then
                    Dim MailSubject As String = String.Format("{0} - Your order has been shipped.", CCFramework.Core.Settings.CompanyName)
                    Dim MailMessage As New StringBuilder

                    MailMessage.AppendFormat("This is an automated message from {0} to inform you that your order has been shipped. <br /><br />", CCFramework.Core.Settings.CompanyName)
                    If Not CurrentTrackingNumber = "None" Then
                        MailMessage.AppendFormat("The tracking number for this order is <b>{0}</b><br /><br />", CurrentTrackingNumber)
                    End If
                    MailMessage.Append("Thank you for shopping with us")

                    SendEmails.Send(New MailAddress(CurrentSimpleUser.Email), MailSubject, MailMessage.ToString)
                End If

                StatusMessageLabel.Text = String.Format("This order has been shipped with tracking #{0}", CurrentTrackingNumber)
                StatusMessageLabel.Visible = True

                CurrentDataContext.Dispose()
            End If

            LoadOrder(CurrentOrderId)
        End Sub

        Private Sub ShipItem(ByVal OrderItemId As Integer, ByVal TrackingNumber As String)
            Dim CurrentOrderId As Integer
            Dim CurrentShippedItem As Product

            If Integer.TryParse(OrderIdHiddenField.Value, CurrentOrderId) AndAlso Not String.IsNullOrEmpty(TrackingNumber) Then
                Dim CurrentDataContext As New CommerceDataContext

                Dim CurrentOrderItems = OrderItemController.GetOrderItemsByOrderIdFunc(CurrentDataContext, CurrentOrderId)
                Dim CurrentOrder = OrderController.GetOrderByIdFunc(CurrentDataContext, CurrentOrderId)

                Dim AllItemsShipped As Boolean = True
                For Each CurrentOrderItem As OrderItem In CurrentOrderItems
                    If CurrentOrderItem.ID = OrderItemId Then
                        CurrentOrderItem.Shipped = True
                        CurrentOrderItem.ShippedDate = DateTime.Now
                        CurrentOrderItem.TrackingNum = TrackingNumber

                        CurrentShippedItem = New ProductController().GetElement(CurrentOrderItem.ProductID)

                        Dim CurrentSimpleUserController As New SimpleUserController
                        Dim CurrentSimpleUser As SimpleUser = CurrentSimpleUserController.GetElement(CurrentOrder.UserID)
                        CurrentSimpleUserController.Dispose()

                        If CCFramework.Core.Settings.SendTrackingEmails Then
                            Dim MailSubject As String = String.Format("{0} - Your order has been shipped.", CCFramework.Core.Settings.CompanyName)
                            Dim MailMessage As New StringBuilder

                            MailMessage.AppendFormat("This is an automated message from {0} to inform you that your {1} has been shipped. <br /><br />", CCFramework.Core.Settings.CompanyName, CurrentShippedItem.Name)
                            If Not TrackingNumber = "None" Then
                                MailMessage.AppendFormat("The tracking number for this item is <b>{0}</b><br /><br />", TrackingNumber)
                            End If
                            MailMessage.Append("Thank you for shopping with us")

                            SendEmails.Send(New MailAddress(CurrentSimpleUser.Email), MailSubject, MailMessage.ToString)
                        End If
                    End If
                    If Not CurrentOrderItem.Shipped Then
                        AllItemsShipped = False
                    End If
                Next

                If AllItemsShipped Then
                    CurrentOrder.ShipDate = Date.Now
                End If

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If

            LoadOrder(CurrentOrderId)
        End Sub

        Private Sub ShipOrderButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles ShipOrderButton.Command
            ShipOrder()
        End Sub

        Private Sub ReturnOrderButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles ReturnOrderButton.Command
            Dim CurrentOrderId As Integer

            If Integer.TryParse(OrderIdHiddenField.Value, CurrentOrderId) Then
                ReturnOrder(CurrentOrderId)
            Else
                StatusMessageLabel.Visible = True
                StatusMessageLabel.Text = String.Format("Ooops! That order is invalid.")
            End If
        End Sub

        Private Sub DeleteOrderButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles DeleteOrderButton.Command
            Dim CurrentOrderId As Integer

            If Integer.TryParse(OrderIdHiddenField.Value, CurrentOrderId) Then
                DeleteOrder(CurrentOrderId)
            Else
                StatusMessageLabel.Visible = True
                StatusMessageLabel.Text = String.Format("Ooops! That order is invalid.")
            End If
        End Sub

        Public Sub ShipItemButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs)
            Dim CurrentTrackingNumber As String = CType(CType(Sender, LinkButton).Parent.FindControl("ItemTrackingNumberTextBox"), TextBox).Text

            ShipItem(Integer.Parse(Args.CommandArgument.ToString), CurrentTrackingNumber)
            'Response.Write(CurrentTrackingNumber)
        End Sub

        Public Sub ReturnItemButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs)
            ReturnItem(Integer.Parse(Args.CommandArgument.ToString))

            Dim OrderId As Integer = Integer.Parse(OrderIdHiddenField.Value)

            LoadOrder(OrderId)
        End Sub

        Public Sub DeleteItemButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs)
            DeleteItem(Integer.Parse(Args.CommandArgument.ToString))

            Dim OrderId As Integer = Integer.Parse(OrderIdHiddenField.Value)

            LoadOrder(OrderId)
        End Sub

        Private Sub AuthorizeOrderButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles AuthorizeOrderButton.Command
            Dim CurrentOrderId As Integer = Integer.Parse(OrderIdHiddenField.Value)

            Dim CurrentOrderController As New OrderController
            Dim CurrentOrder As Order = CurrentOrderController.GetElement(CurrentOrderId)
            Dim CurrentPaymentType As EPaymentType = CType(CurrentOrder.PaymentType, EPaymentType)
            CurrentOrderController.Dispose()

            Dim PaymentAuthorized As Boolean = False

            Select Case CurrentPaymentType
                Case EPaymentType.CreditCard
                Case EPaymentType.BeanStream
                    Dim CurrentBeanStreamPaymentController As New PaymentController
                    Dim CurrentPayment As BeanStreamPayment = CurrentBeanStreamPaymentController.GetOrderPayment(CurrentOrderId)
                    PaymentAuthorized = CurrentBeanStreamPaymentController.SetPaymentAuthorized(CurrentPayment.ID, OrderAuthorizationNumberTextBox.Text, "", CurrentOrderId, PaymentStatus.Approved, CurrentPayment.Charge)

                    CurrentBeanStreamPaymentController.Dispose()
                Case Else
            End Select

            If PaymentAuthorized Then
                OrderController.SetOrderAuthorized(CurrentOrderId)
            End If

            AuthorizeOrderButton.Enabled = False
            OrderAuthorizationNumberTextBox.Enabled = False

        End Sub

        Private Sub PrintOrderButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles PrintOrderButton.Command
            Dim CurrentOrderId As Integer = Integer.Parse(OrderIdHiddenField.Value)

            If Session("Order") Is Nothing Then
                Session.Add("Order", CurrentOrderId)
            Else
                Session("Order") = CurrentOrderId
            End If

            Response.Redirect("~/CCAdmin/Commerce/Print/PrintOrder.aspx")
        End Sub

        Private Sub ShowCreditCardButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles ShowCreditCardButton.Command
            'TODO Create a table and log in the database whenever someone shows a credit card number Date, Time, UesrID and OrderID :D
            Dim CurrentOrderId As Integer

            If Integer.TryParse(OrderIdHiddenField.Value, CurrentOrderId) Then
                Dim CurrentBeanStreamPaymentController As New PaymentController

                Dim CurrentCreditCardPayment As BeanStreamPayment = CurrentBeanStreamPaymentController.GetOrderPayment(CurrentOrderId)
                Dim CurrentEasyCryptography As New EasyCryptography()

                CardNumberLiteral.Text = CurrentEasyCryptography.Decrypt(CurrentCreditCardPayment.CCNumber, CurrentCreditCardPayment.CCEncryptionKey)

                CurrentBeanStreamPaymentController.Dispose()
            End If
        End Sub

        Private Sub NotifyDistributorButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles NotifyDistributorButton.Click
            SendDistributorInvoice()
        End Sub

        Private Sub ResendInvoiceButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles ResendInvoiceButton.Click
            SendUserInvoice()
        End Sub

        Private Sub SendUserInvoice()
            Dim CurrentOrderId As Integer = Integer.Parse(OrderIdHiddenField.Value)

            Dim CurrentSimpleUserController As New SimpleUserController
            Dim CurrentOrderController As New OrderController

            Dim CurrentOrder As Order = CurrentOrderController.GetElement(CurrentOrderId)

            Dim CurrentUserEmailAddress As String = CurrentSimpleUserController.GetElement(CurrentOrder.UserID).Email

            CurrentSimpleUserController.Dispose()
            CurrentOrderController.Dispose()

            SendEmails.SendUserInvoice(CurrentOrderId, CurrentUserEmailAddress, Context)

            StatusMessageLabel.Text = String.Format("The invoice has been sent to email: {0}. They should recieve it shortly", CurrentUserEmailAddress)
            StatusMessageLabel.Visible = True
        End Sub

        Private Sub SendDistributorInvoice()
            Dim CurrentOrderId As Integer = Integer.Parse(OrderIdHiddenField.Value)

            SendEmails.SendDistributorInvoice(CurrentOrderId, Context)

            StatusMessageLabel.Text = String.Format("The invoice has been sent to the distributor. They should recieve it shortly.")
            StatusMessageLabel.Visible = True
        End Sub

    End Class

End Namespace