Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.PaymentProcessing

Namespace CCAdmin.Commerce.Print

    Partial Public Class PrintOrder
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            MyBase.Title = "Thank you for shopping with us - " & CCFramework.Core.Settings.CompanyName

            hlSupportEmail.Text = CCFramework.Core.Settings.SupportEmail
            hlSupportEmail.NavigateUrl = "mailto:" & CCFramework.Core.Settings.SupportEmail

            Dim OrderID As Integer
            'Dim Email As String = Request.QueryString("Email")

            If Integer.TryParse(Session("Order").ToString, OrderID) Then
                If OrderID > 0 Then
                    LoadOrder(OrderID)
                End If
            End If
        End Sub

        Private Sub LoadOrder(ByVal OrderID As Integer)
            Dim order As Order = New OrderController().GetElement(OrderID)

            Dim Customer As SimpleUser = New CCFramework.Core.SimpleUserController().GetElement(order.UserID)

            Dim sAddress As Address = New AddressController().GetElement(order.SAddressID)
            Dim sProvince As Province = New ProvinceController().GetElement(sAddress.ProvStateID)
            Dim sCountry As Country = New CountryController().GetElement(sProvince.CountryID)

            Dim bAddress As Address = New AddressController().GetElement(order.BAddressID)
            Dim bProvince As Province = New ProvinceController().GetElement(bAddress.ProvStateID)
            Dim bCountry As Country = New CountryController().GetElement(bProvince.CountryID)

            Dim orderTotal As New OrderTotals(OrderController.GetItemCount(order.ID), OrderController.GetOrderSubTotal(order.ID), order.ShipCharge, sProvince.Tax + sCountry.Tax, order.Discount)

            Dim paymentMethod As EPaymentType = CType(order.PaymentType, EPaymentType)

            litOrderID.Text = order.ID.ToString

            lblShippingAddress.Text = sAddress.Address & ", " & sAddress.City & ", " & sProvince.Name & ", " & sCountry.Name & ", " & sAddress.PCZIP
            lblBillingAddress.Text = bAddress.Address & ", " & bAddress.City & ", " & bProvince.Name & ", " & bCountry.Name & ", " & bAddress.PCZIP

            litCustomerInformation.Text = Customer.FirstName & " " & Customer.LastName & " - <a href=""mailto:" & Customer.Email & """>" & Customer.Email & "</a>"

            litOrderItems.Text = orderTotal.Items.ToString
            litSubTotal.Text = orderTotal.SubTotal.ToString("C")
            litDiscount.Text = orderTotal.Discount.ToString("C")
            litShipCharge.Text = orderTotal.ShippingCharge.ToString("C")
            litTaxRate.Text = orderTotal.TaxRate.ToString("P")
            litTaxAmount.Text = orderTotal.Tax.ToString("C")
            litTotal.Text = orderTotal.Total.ToString("C")

            lblShipCompany.Text = order.ShipCompany
            lblShipService.Text = order.ShipService
            lblShipCharge.Text = order.ShipCharge.ToString("C")

            Select Case paymentMethod
                Case EPaymentType.CreditCard
                    Try
                        Dim ccPayment As CreditCardPayment = New CreditCardPaymentController().GetOrderPayment(order.ID)
                        Dim ccType As CreditCardType = New CreditCardTypeController().GetElement(ccPayment.CCTypeID)

                        lblBillMethod.Text = ccType.Type
                        lblBillingAmount.Text = ccPayment.Charge.Value.ToString("C")
                    Catch ex As Exception
                        'The payment doesn't seem to exist
                    End Try

                Case EPaymentType.BeanStream
                    Try
                        Dim payment As BeanStreamPayment = New PaymentController().GetOrderPayment(order.ID)

                        lblBillMethod.Text = "Credit Card"
                        lblBillingAmount.Text = payment.Charge.ToString("C")
                        lblAuthCode.Text = payment.AuthCode
                    Catch ex As Exception
                        'The Payment doesn't seem to exist
                    End Try
                Case Else
                    'Payment not authorized
            End Select

            Dim db As New CommerceDataContext

            Dim orderItems As List(Of InvoiceOrderItem) = OrderItemController.GetOrderInvoiceItemsFunc(db, order.ID).ToList

            dgOrderItems.DataSource = orderItems
            dgOrderItems.DataBind()

            db.Dispose()
        End Sub

    End Class
End Namespace