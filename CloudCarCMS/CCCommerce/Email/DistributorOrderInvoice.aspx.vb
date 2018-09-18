Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.PaymentProcessing

Namespace CCCommerce.Email

    Partial Public Class DistributorOrderInvoice
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            MyBase.Title = "An order is awaiting shipment - " & CCFramework.Core.Settings.CompanyName

            hlDistributorOrders.Text = "http://" & CCFramework.Core.Settings.HostName & "/Distributor/DistributorOrders.aspx"
            hlDistributorOrders.NavigateUrl = "http://" & CCFramework.Core.Settings.HostName & "/Distributor/DistributorOrders.aspx"

            Dim OrderId As Integer
            Dim DistributorId As Integer

            If Integer.TryParse(Request.QueryString("Order"), OrderId) And Integer.TryParse(Request.QueryString("Distributor"), DistributorId) Then
                If OrderId > 0 Then
                    LoadOrder(OrderId, DistributorId)
                End If
            End If
        End Sub

        Private Sub LoadOrder(ByVal OrderId As Integer, ByVal DistributorId As Integer)
            Dim CurrentOrder As Order = New OrderController().GetElement(OrderID)

            If OrderController.IsDistributorsOrder(DistributorID, CurrentOrder.ID) Then
                Dim sAddress As Address = New AddressController().GetElement(CurrentOrder.SAddressID)
                Dim sProvince As Province = New ProvinceController().GetElement(sAddress.ProvStateID)
                Dim sCountry As Country = New CountryController().GetElement(sProvince.CountryID)

                Dim bAddress As Address = New AddressController().GetElement(CurrentOrder.BAddressID)
                Dim bProvince As Province = New ProvinceController().GetElement(bAddress.ProvStateID)
                Dim bCountry As Country = New CountryController().GetElement(bProvince.CountryID)

                Dim Customer As SimpleUser = New CCFramework.Core.SimpleUserController().GetElement(CurrentOrder.UserID)

                'lblCustomerName.Text = user.LastName & ", " & user.FirstName & " " & user.MiddleName
                'hlEmailAddress.NavigateUrl = "mailto:" & user.Email
                'hlEmailAddress.Text = user.Email
                'lblPhoneNumber.Text = user.PhoneNumber

                Dim orderTotal As New OrderTotals(OrderController.GetItemCount(CurrentOrder.ID), OrderController.GetOrderSubTotal(CurrentOrder.ID), CurrentOrder.ShipCharge, sProvince.Tax + sCountry.Tax, CurrentOrder.Discount)

                Dim paymentMethod As EPaymentType = CType(CurrentOrder.PaymentType, EPaymentType)

                litOrderID.Text = CurrentOrder.ID.ToString

                lblShippingAddress.Text = sAddress.Address & ", " & sAddress.City & ", " & sProvince.Name & ", " & sCountry.Name & ", " & sAddress.PCZIP
                lblBillingAddress.Text = bAddress.Address & ", " & bAddress.City & ", " & bProvince.Name & ", " & bCountry.Name & ", " & bAddress.PCZIP

                litCustomerInformation.Text = Customer.FirstName & " " & Customer.LastName & " - <a href=""mailto:" & Customer.Email & """>" & Customer.Email & "</a> - " & Customer.PhoneNumber

                litOrderItems.Text = orderTotal.Items.ToString
                litSubTotal.Text = orderTotal.SubTotal.ToString("C")
                litDiscount.Text = orderTotal.Discount.ToString("C")
                litShipCharge.Text = orderTotal.ShippingCharge.ToString("C")
                litTaxRate.Text = orderTotal.TaxRate.ToString("P")
                litTaxAmount.Text = orderTotal.Tax.ToString("C")
                litTotal.Text = orderTotal.Total.ToString("C")

                lblShipCompany.Text = CurrentOrder.ShipCompany
                lblShipService.Text = CurrentOrder.ShipService
                lblShipCharge.Text = CurrentOrder.ShipCharge.ToString("C")
                lblWeight.Text = OrderController.GetOrderWeight(CurrentOrder.ID) & "Kg"

                Dim db As New CommerceDataContext

                Dim orderItems As List(Of InvoiceOrderItem) = OrderItemController.GetOrderInvoiceItemsFunc(db, CurrentOrder.ID).ToList

                dgOrderItems.DataSource = orderItems
                dgOrderItems.DataBind()

                db.Dispose()
            End If
        End Sub

    End Class
End NameSpace