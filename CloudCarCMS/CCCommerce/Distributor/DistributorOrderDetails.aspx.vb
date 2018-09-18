Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Partial Public Class DistributorOrderDetails
    Inherits Page

    Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim OrderID As Integer
            Dim DistributorID As Integer = New CCFramework.Core.RegisteredUserController().GetByUserName(Membership.GetUser().UserName).UserID

            If Integer.TryParse(Request.QueryString("Order"), OrderID) And OrderController.IsDistributorsOrder(DistributorID, OrderID) Then
                LoadOrder(OrderID)

                phOrderDetails.Visible = True
            Else
                lblStatus.Text = "You do not have permission to view this order!"
                lblStatus.Visible = True
                phOrderDetails.Visible = False
            End If
        End If
    End Sub

    Private Sub LoadOrder(ByVal OrderID As Integer)
        Dim order As Order = New OrderController().GetElement(OrderID)
        Dim user As SimpleUser = New CCFramework.Core.SimpleUserController().GetElement(order.UserID)
        Dim sAddress As Address = New AddressController().GetElement(order.SAddressID)
        Dim bAddress As Address = New AddressController().GetElement(order.BAddressID)
        Dim sProvince As Province = New ProvinceController().GetElement(sAddress.ProvStateID)
        Dim bProvince As Province = New ProvinceController().GetElement(bAddress.ProvStateID)
        Dim sCountry As Country = New CountryController().GetElement(sProvince.CountryID)
        Dim bCountry As Country = New CountryController().GetElement(bProvince.CountryID)
        Dim orderItems As List(Of OrderItem) = New OrderItemController().GetOrderElements(order.ID)
        Dim profile As RegisteredUser

        hfOrderID.Value = OrderID.ToString

        litOrderID.Text = order.ID.ToString

        Try
            profile = New CCFramework.Core.RegisteredUserController().GetByUserID(user.ID)

            litUsername.Text = profile.UserName
            'hlUsername.NavigateUrl = "~/CCAdmin/Users.aspx?User=" & user.ID
        Catch ex As Exception
            litUsername.Text = "User is not registered"
        End Try

        litName.Text = user.FirstName & " " & user.MiddleName & " " & user.LastName
        hlEmailAddress.Text = user.Email
        hlEmailAddress.NavigateUrl = "mailto:" & user.Email
        litPhoneNumber.Text = user.PhoneNumber

        litShippingAddress.Text = sAddress.Address & " " & sAddress.City & ", " & sProvince.Name & "<br /> " & sCountry.Name & ", " & sAddress.PCZIP
        litBillingAddress.Text = bAddress.Address & " " & bAddress.City & ", " & bProvince.Name & "<br /> " & bCountry.Name & ", " & bAddress.PCZIP

        'hlEditSAddress.NavigateUrl = "~/CCAdmin/Commerce/Addresses.aspx?Address=" & sAddress.ID & "&Order=" & order.ID
        'hlEditBAddress.NavigateUrl = "~/CCAdmin/Commerce/Addresses.aspx?Address=" & bAddress.ID & "&Order=" & order.ID

        Dim db As New CommerceDataContext

        Dim orderItemDetails As List(Of OrderItemSummary) = OrderItemController.GetOrderItemsSummaryFunc(db, order.ID).ToList

        Dim orderShipped As Boolean = True

        For Each item In orderItemDetails
            If Not item.Shipped Then
                orderShipped = False
            End If
        Next

        If orderShipped Then
            txtOrderTrackingNumber.Enabled = False
            btnShipOrder.Enabled = False
        End If

        gvOrderItems.DataSource = orderItemDetails
        gvOrderItems.DataBind()

        Dim subTotal As Decimal = 0D

        For Each item In orderItemDetails
            subTotal += item.Price
        Next

        Dim shippingCharge As Decimal = order.ShipCharge
        Dim discount As Decimal = order.Discount
        Dim taxes As Decimal = CDec(((subTotal - discount) + shippingCharge) * order.TaxPercent)
        Dim total As Decimal = CDec((subTotal - discount) + shippingCharge + taxes)

        litSubtotal.Text = subTotal.ToString("C")
        litDiscount.Text = discount.ToString("C")
        litShippingCharge.Text = shippingCharge.ToString("C")
        litTaxRate.Text = order.TaxPercent.ToString("P")
        litTaxes.Text = taxes.ToString("C")
        litTotal.Text = total.ToString("C")

        litShipCompany.Text = order.ShipCompany
        litShipService.Text = order.ShipService
        litShipCharge.Text = order.ShipCharge.ToString
    End Sub

    Private Sub ShipOrder()
        Dim db As New CommerceDataContext

        Dim orderID As Integer
        Dim trackingNumber As String = txtOrderTrackingNumber.Text

        If Integer.TryParse(hfOrderID.Value, orderID) And Not trackingNumber = "" Then
            'Dim orderItems As List(Of OrderItem) = New Commerce.cOrder().GetOrderItems(orderID)

            Dim orderItems = OrderItemController.GetOrderItemsByOrderIdFunc(db, orderID) ' From oi In db.OrderItems Where oi.OrderID = orderID Select oi

            For Each item As OrderItem In orderItems
                item.Shipped = True
                item.ShippedDate = DateTime.Now
                item.TrackingNum = trackingNumber
            Next

            Dim order = OrderController.GetOrderByIdFunc(db, orderID) ' (From o In db.Orders Where o.ID = orderID Select o).FirstOrDefault
            order.ShipDate = Date.Now

            db.SubmitChanges()

            Dim user As SimpleUser = New CCFramework.Core.SimpleUserController().GetElement(order.UserID)

            If CCFramework.Core.Settings.SendTrackingEmails Then
                Dim shipMessage As String = "This is an automated message from " & CCFramework.Core.Settings.CompanyName & " to inform you that your order has been shipped. <br /><br />"
                shipMessage &= "The tracking number for this order is " & trackingNumber & "<br /><br />"
                shipMessage &= "Thank you for shopping with us"

                CCFramework.Core.SendEmails.Send(New Net.Mail.MailAddress(user.Email), CCFramework.Core.Settings.CompanyName & " - Your order has been shipped.", shipMessage)
            End If
        End If

        LoadOrder(orderID)
    End Sub

    Private Sub ShipItem(ByVal OrderItemId As Integer, ByVal trackingNumber As String)
        Dim db As New CommerceDataContext

        Dim orderID As Integer
        Dim shippedItem As Product

        If Integer.TryParse(hfOrderID.Value, orderID) And Not trackingNumber = "" Then
            Dim orderItems = OrderItemController.GetOrderItemsByOrderIdFunc(db, orderID) ' From oi In db.OrderItems Where oi.OrderID = orderID Select oi
            Dim order = OrderController.GetOrderByIdFunc(db, orderID) ' (From o In db.Orders Where o.ID = orderID Select o).FirstOrDefault

            Dim allItemsShipped As Boolean = True
            For Each item As OrderItem In orderItems
                If item.ID = OrderItemID Then
                    item.Shipped = True
                    item.ShippedDate = DateTime.Now
                    item.TrackingNum = trackingNumber

                    shippedItem = New ProductController().GetElement(item.ProductID)

                    Dim user As SimpleUser = New CCFramework.Core.SimpleUserController().GetElement(order.UserID)

                    If CCFramework.Core.Settings.SendTrackingEmails Then
                        Dim shipMessage As String = "This is an automated message from " & CCFramework.Core.Settings.CompanyName & " to inform you that your " & shippedItem.Name & " has been shipped. <br /><br />"
                        If Not trackingNumber = "" Then
                            shipMessage &= "The tracking number for this item is " & trackingNumber & "<br /><br />"
                        End If
                        shipMessage &= "Thank you for shopping with us."

                        CCFramework.Core.SendEmails.Send(New Net.Mail.MailAddress(user.Email), CCFramework.Core.Settings.CompanyName & " - your " & shippedItem.Name & " has been shipped.", shipMessage)
                    End If
                End If
                If Not item.Shipped Then
                    allItemsShipped = False
                End If
            Next

            If allItemsShipped Then
                order.ShipDate = Date.Now
            End If

            db.SubmitChanges()
        End If

        LoadOrder(orderID)
    End Sub

    Private Sub ShipOrderButtonCommand(ByVal Sender As Object, ByVal E As CommandEventArgs) Handles btnShipOrder.Command
        ShipOrder()
    End Sub

    Public Sub ShipItemButtonCommand(ByVal Sender As Object, ByVal E As CommandEventArgs)
        Dim TrackingNumber As String = CType(CType(Sender, LinkButton).Parent.FindControl("txtItemTNumber"), TextBox).Text

        ShipItem(Integer.Parse(E.CommandArgument.ToString), TrackingNumber)

        Response.Write(TrackingNumber)
    End Sub

    Private Sub PrintOrderButtonCommand(ByVal Sender As Object, ByVal E As CommandEventArgs) Handles btnPrintOrder.Command
        Dim OrderId As Integer = Integer.Parse(hfOrderID.Value)

        If Session("Order") Is Nothing Then
            Session.Add("Order", OrderId)
        Else
            Session("Order") = OrderId
        End If
        Response.Redirect("~/CCCommerce/Email/NewOrderInvoice.aspx")
    End Sub

End Class