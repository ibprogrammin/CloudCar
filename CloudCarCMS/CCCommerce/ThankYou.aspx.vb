Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Commerce.PaymentProcessing

Namespace CCCommerce
    Partial Public Class ThankYou
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Settings.StoreEnabled Then
                Response.Redirect(Settings.StoreDisabledPage)
            End If

            MyBase.Title = String.Format("Thank you for shopping with us - {0}", Settings.CompanyName)

            hlSupportEmail.Text = Settings.SupportEmail
            hlSupportEmail.NavigateUrl = String.Format("mailto:{0}", Settings.SupportEmail)

            Dim OrderId As Integer
            Dim EmailAddress As String = Session("EmailAddress").ToString

            If Integer.TryParse(CStr(Session("OrderId")), OrderId) AndAlso Not String.IsNullOrEmpty(EmailAddress) Then
                If OrderId > 0 Then
                    LoadOrder(OrderId, EmailAddress)

                    Session.Remove("OrderId")
                    Session.Remove("EmailAddress")
                Else
                    Response.RedirectToRoute("RouteShop")
                End If
            Else
                Response.RedirectToRoute("RouteShop")
            End If
        End Sub

        Private Sub LoadOrder(ByVal OrderId As Integer, ByVal EmailAddress As String)
            Dim CurrentOrderController As New OrderController
            
            Dim CurrentOrder As Order = CurrentOrderController.GetElement(OrderId)

            If OrderController.CheckOrderPermissions(CurrentOrder.UserID, EmailAddress) Then
                Dim CurrentSimpleUserController As New SimpleUserController
                Dim CurrentAddressController As New AddressController

                Dim Customer As SimpleUser = CurrentSimpleUserController.GetElement(CurrentOrder.UserID)

                Dim CurrentShippingAddress As Address = CurrentAddressController.GetElement(CurrentOrder.SAddressID)
                Dim CurrentShippingProvince As Province = CurrentShippingAddress.Province
                Dim CurrentShippingCountry As Country = CurrentShippingAddress.Province.Country

                Dim CurrentBillingAddress As Address = CurrentAddressController.GetElement(CurrentOrder.BAddressID)
                Dim CurrentBillingProvince As Province = CurrentBillingAddress.Province
                Dim CurrentBillingCountry As Country = CurrentBillingAddress.Province.Country

                Dim CurrentOrderTotals As New OrderTotals(OrderController.GetItemCount(CurrentOrder.ID), OrderController.GetOrderSubTotal(CurrentOrder.ID), CurrentOrder.ShipCharge, CurrentShippingProvince.Tax + CurrentShippingCountry.Tax, CurrentOrder.Discount)

                Dim CurrentPaymentMethod As EPaymentType = CType(CurrentOrder.PaymentType, EPaymentType)

                OrderIdLiteral.Text = CurrentOrder.ID.ToString

                ShippingAddressLiteral.Text = String.Format("{0}, {1}, {2}, {3}, {4}", CurrentShippingAddress.Address, CurrentShippingAddress.City, CurrentShippingProvince.Name, CurrentShippingCountry.Name, CurrentShippingAddress.PCZIP)
                BillingAddressLiteral.Text = String.Format("{0}, {1}, {2}, {3}, {4}", CurrentBillingAddress.Address, CurrentBillingAddress.City, CurrentBillingProvince.Name, CurrentBillingCountry.Name, CurrentBillingAddress.PCZIP)

                CustomerInformationLiteral.Text = String.Format("{0} {1} <br /> <a href=""mailto:{2}"">{2}</a>", Customer.FirstName, Customer.LastName, Customer.Email)
                FirstNameLiteral.Text = Customer.FirstName

                OrderItemsLiteral.Text = CurrentOrderTotals.Items.ToString
                SubTotalLiteral.Text = CurrentOrderTotals.SubTotal.ToString("C")
                DiscountLiteral.Text = CurrentOrderTotals.Discount.ToString("C")
                ShippingChargeLiteral.Text = CurrentOrderTotals.ShippingCharge.ToString("C")
                TaxRateLiteral.Text = CurrentOrderTotals.TaxRate.ToString("P")
                TaxChargesLiteral.Text = CurrentOrderTotals.Tax.ToString("C")
                TotalLiteral.Text = CurrentOrderTotals.Total.ToString("C")

                ShippingDetailsLiteral.Text = String.Format("{0} {1} - {2}", CurrentOrder.ShipCompany, CurrentOrder.ShipService, CurrentOrder.ShipCharge.ToString("C"))

                CurrentSimpleUserController.Dispose()
                CurrentAddressController.Dispose()

                Select Case CurrentPaymentMethod
                    Case EPaymentType.CreditCard
                        Try
                            Dim CurrentCreditCardPaymentController As New CreditCardPaymentController
                            Dim CurrentCreditCardTypeController As New CreditCardTypeController

                            Dim CurrentCreditCardPayment As CreditCardPayment = CurrentCreditCardPaymentController.GetOrderPayment(CurrentOrder.ID)
                            Dim CurrentCreditCardType As CreditCardType = CurrentCreditCardTypeController.GetElement(CurrentCreditCardPayment.CCTypeID)

                            BillingMethodLiteral.Text = CurrentCreditCardType.Type
                            BillingAmountLiteral.Text = CurrentCreditCardPayment.Charge.Value.ToString("C")

                            CurrentCreditCardPaymentController.Dispose()
                            CurrentCreditCardTypeController.Dispose()
                        Catch CurrentException As Exception
                            'The payment doesn't seem to exist
                        End Try

                    Case EPaymentType.BeanStream
                        Try
                            Dim CurrentBeanStreamPaymentController As New PaymentController
                            Dim CurrentBeanStreamPayment As BeanStreamPayment = CurrentBeanStreamPaymentController.GetOrderPayment(CurrentOrder.ID)

                            BillingMethodLiteral.Text = "Credit Card"
                            BillingAmountLiteral.Text = CurrentBeanStreamPayment.Charge.ToString("C")
                            AuthorizationCodeLiteral.Text = CurrentBeanStreamPayment.AuthCode
                        Catch CurrentException As Exception
                            'The Payment doesn't seem to exist
                        End Try
                End Select

                Dim CurrentDataContext As New CommerceDataContext

                Dim CurrentOrderItems As List(Of InvoiceOrderItem) = OrderItemController.GetOrderInvoiceItemsFunc(CurrentDataContext, CurrentOrder.ID).ToList

                OrderItemsRepeater.DataSource = CurrentOrderItems
                OrderItemsRepeater.DataBind()

                CurrentDataContext.Dispose()
            Else
                Response.RedirectToRoute("RouteShop")
            End If

            CurrentOrderController.Dispose()
        End Sub


    End Class

End Namespace