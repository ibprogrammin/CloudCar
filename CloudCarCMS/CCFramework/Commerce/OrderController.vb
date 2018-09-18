Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.PaymentProcessing

Namespace CCFramework.Commerce

    Public Class OrderController
        Inherits DataControllerClass

        Public Shared GetOrdersFunc As Func(Of CommerceDataContext, IQueryable(Of Order)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext) From o In db.Orders Select o)

        Public Shared GetOrderByIdFunc As Func(Of CommerceDataContext, Integer, Order) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, OrderId As Integer) _
                                              (From o In db.Orders Where o.ID = OrderId Select o).FirstOrDefault)

        Public Shared GetOrderLineTotalsFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Decimal)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, OrderId As Integer) _
                                            From o In db.OrderItems _
                                            Where o.OrderID = OrderId Select (o.Price * o.Quantity))


        Public Overloads Function Create(ByVal CustomerName As String, ByVal UserId As Integer, ByVal Discount As Decimal, ByVal TaxPercent As Double, ByVal PaymentType As EPaymentType, ByVal ApprovalState As EApprovalState, ByVal ShipCharge As Decimal, ByVal ShipCompany As String, ByVal ShipService As String, ByVal ShipAddressID As Integer, ByVal BillAddressID As Integer, ByVal AuthCode As String) As Integer
            Dim CurrentOrder As New Order
            Dim CurrentOrderId As Integer

            'item.CustomerName = CustomerName
            CurrentOrder.UserID = UserId
            CurrentOrder.OrderDate = DateTime.Now
            CurrentOrder.TaxPercent = TaxPercent
            CurrentOrder.PaymentType = PaymentType
            CurrentOrder.ApprovalState = ApprovalState
            CurrentOrder.ShipCharge = ShipCharge
            CurrentOrder.ShipCompany = ShipCompany
            CurrentOrder.ShipService = ShipService
            CurrentOrder.SAddressID = ShipAddressID
            CurrentOrder.BAddressID = BillAddressID
            CurrentOrder.Discount = Discount
            'item.PromoCode = PromoCode
            'item.AuthCode = AuthCode

            db.Orders.InsertOnSubmit(CurrentOrder)
            db.SubmitChanges()

            CurrentOrderId = CurrentOrder.ID

            Return CurrentOrderId
        End Function

        Public Overloads Function Create(ByVal CustomerName As String, ByVal UserId As Integer, ByVal Discount As Decimal, ByVal TaxPercent As Double, ByVal PaymentType As EPaymentType, ByVal ApprovalState As EApprovalState, ByVal ShipCharge As Decimal, ByVal ShipCompany As String, ByVal ShipService As String, ByVal ShipAddressId As Integer, ByVal BillAddressId As Integer, ByVal AuthCode As String, ByVal PromoCode As String, ByVal DistributorId As Integer) As Integer
            Dim CurrentOrder As New Order
            Dim CurrentOrderId As Integer

            'item.CustomerName = CustomerName
            CurrentOrder.UserID = UserId
            CurrentOrder.OrderDate = DateTime.Now
            CurrentOrder.TaxPercent = TaxPercent
            CurrentOrder.PaymentType = PaymentType
            CurrentOrder.ApprovalState = ApprovalState
            CurrentOrder.ShipCharge = ShipCharge
            CurrentOrder.ShipCompany = ShipCompany
            CurrentOrder.ShipService = ShipService
            CurrentOrder.SAddressID = ShipAddressId
            CurrentOrder.BAddressID = BillAddressId
            CurrentOrder.Discount = Discount
            CurrentOrder.PromoCode = PromoCode
            'item.AuthCode = AuthCode
            CurrentOrder.DistributorId = DistributorId

            db.Orders.InsertOnSubmit(CurrentOrder)
            db.SubmitChanges()

            CurrentOrderId = CurrentOrder.ID

            Return CurrentOrderId
        End Function

        Public Overloads Function Delete(ByVal OrderId As Integer) As Boolean
            Try
                Dim CurrentOrder As Order = GetOrderByIdFunc(db, OrderId)

                db.Orders.DeleteOnSubmit(CurrentOrder)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal OrderId As Integer, ByVal CustomerName As String, ByVal UserID As Integer, ByVal Discount As Decimal, ByVal TaxPercent As Double, ByVal PaymentType As EPaymentType, ByVal ApprovalState As EApprovalState, ByVal ShipCharge As Decimal, ByVal ShipCompany As String, ByVal ShipService As String, ByVal ShipAddressID As Integer, ByVal BillAddressID As Integer, ByVal AuthCode As String) As Boolean
            Dim CurrentOrder As Order = GetOrderByIdFunc(db, OrderId)

            If CurrentOrder Is Nothing Then
                Throw New Exception(String.Format("Order {0} does not exist.", OrderId))
            Else
                'item.CustomerName = CustomerName
                CurrentOrder.UserID = UserID
                CurrentOrder.OrderDate = DateTime.Now
                CurrentOrder.TaxPercent = TaxPercent
                CurrentOrder.PaymentType = PaymentType
                CurrentOrder.ApprovalState = ApprovalState
                CurrentOrder.ShipCharge = ShipCharge
                CurrentOrder.ShipCompany = ShipCompany
                CurrentOrder.ShipService = ShipService
                CurrentOrder.SAddressID = ShipAddressID
                CurrentOrder.BAddressID = BillAddressID
                CurrentOrder.Discount = Discount
                'item.PromoCode = PromoCode
                'item.AuthCode = AuthCode

                db.SubmitChanges()
            End If

            Return True
        End Function

        Public Overloads Function GetElement(ByVal OrderId As Integer) As Order
            Dim CurrentOrder As Order = GetOrderByIdFunc(db, OrderId)

            If CurrentOrder Is Nothing Then
                Throw New Exception(String.Format("Order {0} does not exist.", OrderId))
            Else
                Return CurrentOrder
            End If
        End Function

        Public Overloads Function GetElements() As List(Of Order)
            Dim CurrentOrders As List(Of Order) = GetOrdersFunc(db).ToList

            If CurrentOrders Is Nothing Then
                Throw New Exception("There are no Orders")
            Else
                Return CurrentOrders
            End If
        End Function

        Public Function GetOrderItems(ByVal OrderId As Integer) As List(Of OrderItem)
            Dim CurrentOrders = OrderItemController.GetOrderItemsByOrderIdFunc(db, OrderId).ToList

            If CurrentOrders Is Nothing OrElse CurrentOrders.Count < 1 Then
                Throw New Exception("There are no Order Items")
            Else
                Return CurrentOrders
            End If
        End Function

        Public Shared Function GetItemCount(ByVal OrderId As Integer) As Integer
            Dim CurrentDataContext As New CommerceDataContext

            Try
                GetItemCount = OrderItemController.GetOrderItemsQuantityByOrderIdFunc(CurrentDataContext, OrderId)
            Catch Ex As InvalidOperationException
                GetItemCount = 0
            End Try

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetOrderSubTotalFunc As Func(Of CommerceDataContext, Integer, Decimal) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, OrderId As Integer) _
                                            (From o In db.OrderItems _
                                            Where o.OrderID = OrderId _
                                            Select (o.Price * o.Quantity)).Sum)

        Public Shared Function GetOrderSubTotal(ByVal OrderId As Integer) As Decimal
            Dim CurrentDataContext As New CommerceDataContext

            GetOrderSubTotal = GetOrderSubTotalFunc(CurrentDataContext, OrderId)

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function SetOrderAuthorized(ByVal OrderId As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentOrder As Order = GetOrderByIdFunc(CurrentDataContext, OrderId)

            If CurrentOrder Is Nothing Then
                'Throw New Exception("Order " & OrderId.ToString & " does not exist.")

                CurrentDataContext.Dispose()

                Return False
            Else
                CurrentOrder.ApprovalState = EApprovalState.Approved
                CurrentOrder.ApprovalDate = Date.Now

                CurrentDataContext.SubmitChanges()

                'TODO send any emails for digital products.
                ProductDownloadController.CreateDownloadableOrderItems(OrderId)
                ProductDownloadController.SendDownloadLinks(OrderId)

                CurrentDataContext.Dispose()

                Return True
            End If
        End Function

        Public Shared Function ReturnOrder(ByVal OrderId As Integer) As Boolean
            Try
                Dim CurrentDataContext As New CommerceDataContext

                Dim CurrentOrder As Order = GetOrderByIdFunc(CurrentDataContext, OrderId)

                CurrentOrder.ShipCharge = -CurrentOrder.ShipCharge
                CurrentOrder.Discount = -CurrentOrder.Discount

                Dim CurrentOrderItems As List(Of OrderItem) = OrderItemController.GetOrderItemsByOrderIdFunc(CurrentDataContext, OrderID).ToList ' From oi In db.OrderItems Where oi.OrderID = OrderID Select oi

                For Each item In CurrentOrderItems
                    item.Price = -item.Price
                Next

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Shared Function DeleteOrder(ByVal OrderId As Integer) As Boolean
            Try
                Dim CurrentDataContext As New CommerceDataContext
                Dim CurrentOrderItemController As New OrderItemController
                Dim CurrentOrder As Order = GetOrderByIdFunc(CurrentDataContext, OrderId)

                CurrentOrderItemController.DeleteOrderItems(OrderId)

                Dim CurrentPaymentType As EPaymentType = CType(CurrentOrder.PaymentType, EPaymentType)

                Select Case CurrentPaymentType
                    Case EPaymentType.None
                    Case EPaymentType.CreditCard
                        Dim CurrentPayment = From p In CurrentDataContext.CreditCardPayments Where p.OrderID = OrderID Select p

                        CurrentDataContext.CreditCardPayments.DeleteAllOnSubmit(CurrentPayment)
                    Case EPaymentType.Paypal
                    Case EPaymentType.BeanStream
                        Dim CurrentPayment = From p In CurrentDataContext.BeanStreamPayments Where p.OrderID = OrderId Select p

                        CurrentDataContext.BeanStreamPayments.DeleteAllOnSubmit(CurrentPayment)
                    Case Else
                End Select

                'Dim CurrentOrder = GetOrderByIdFunc(CurrentDataContext, OrderID)

                CurrentOrderItemController.Dispose()

                CurrentDataContext.Orders.DeleteOnSubmit(CurrentOrder)
                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Shared GetMonthlySalesItems As Func(Of CommerceDataContext, IQueryable(Of MonthlySalesSummary)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                From o In db.Orders _
                                Join oi In db.OrderItems On o.ID Equals oi.OrderID _
                                Where o.ApprovalState = 1 And oi.Price > 0 _
                                Group By o.OrderDate.Year, o.OrderDate.Month Into Sales = Sum(oi.Price * oi.Quantity) _
                                Order By Year, Month _
                                Select New MonthlySalesSummary(Year, Month, Sales))

        Public Function GetMonthlySales(ByVal Count As Integer) As List(Of MonthlySalesSummary)
            Dim Items As List(Of MonthlySalesSummary) = GetMonthlySalesItems(db).ToList

            Dim Skip As Integer = Items.Count - Count

            If Items.Count - Count < Skip Then
                Skip = 0
            End If

            Items = Items.Skip(Skip).Take(Count).ToList

            Return Items
        End Function

        Public Shared Function GetMonthlySalesAsDictionary(Count As Integer) As Dictionary(Of String, Decimal)
            Dim CurrentDataContext As New CommerceDataContext

            Dim Items As List(Of MonthlySalesSummary) = GetMonthlySalesItems(CurrentDataContext).ToList

            Dim CurrentValues As New Dictionary(Of String, Decimal)

            For CurrentIndex As Integer = 0 To Count
                Dim CurrentMonthDate As Date = Date.Now.AddMonths(-CurrentIndex)
                Dim Month As String = String.Format("{0} {1}", MonthName(CurrentMonthDate.Month), CurrentMonthDate.Year.ToString)
                Dim Sales As Decimal = 0

                Sales = Items.Where(Function(f) f.Month = CurrentMonthDate.Month And f.Year = CurrentMonthDate.Year).Sum(Function(f) f.Sales)

                CurrentValues.Add(Month, Sales)
            Next CurrentIndex

            Return CurrentValues
        End Function

        Public Shared Function GetMonthlySalesFormatedForChart(Count As Integer) As String
            Dim CurrentValues As Dictionary(Of String, Decimal) = GetMonthlySalesAsDictionary(Count)
            Dim CurrentStringBuilder As New StringBuilder

            CurrentStringBuilder.Append("[['Month', 'Sales'],")

            For Each Item In CurrentValues.Reverse
                If Item.Equals(CurrentValues.First) Then
                    CurrentStringBuilder.AppendFormat("['{0}',{1}]", Item.Key, Item.Value)
                Else
                    CurrentStringBuilder.AppendFormat("['{0}',{1}],", Item.Key, Item.Value)
                End If
            Next

            CurrentStringBuilder.Append("]")

            Return CurrentStringBuilder.ToString
        End Function

        Public Shared GetDailySalesItems As Func(Of CommerceDataContext, IQueryable(Of SalesSummary)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                From o In db.Orders _
                                Join oi In db.OrderItems On o.ID Equals oi.OrderID _
                                Where o.ApprovalState = 1 And oi.Price > 0 _
                                Group By o.OrderDate.Year, o.OrderDate.Month, o.OrderDate.Day Into Sales = Sum(oi.Price * oi.Quantity) _
                                Order By Year, Month, Day _
                                Select New SalesSummary(Day, Month, Year, Sales))

        Public Shared GetDailySalesTotalFunc As Func(Of CommerceDataContext, Date, Decimal) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, CurrentDate As Date) _
                                (From oi In db.OrderItems _
                                join o In db.Orders On o.ID Equals oi.OrderID _
                                Where o.ApprovalState = 1 And oi.Price > 0 _
                                Group By OrderDate = o.OrderDate.Date Into Total = Sum(oi.Price * oi.Quantity) _
                                Where OrderDate = CurrentDate _
                                Select Total).FirstOrDefault)

        Public Function GetDailySales(ByVal Count As Integer) As List(Of SalesSummary)
            Dim Items As List(Of SalesSummary) = GetDailySalesItems(db).ToList

            Dim Skip As Integer = Items.Count - Count

            If Items.Count - Count < Skip Then
                Skip = 0
            End If

            Items = Items.Skip(Skip).Take(Count).ToList
            Return Items
        End Function

        Public Shared Function GetDailySalesAsDictionary(Count As Integer) As Dictionary(Of String, Decimal)
            Dim CurrentDataContext As New CommerceDataContext

            Dim Items As List(Of SalesSummary) = GetDailySalesItems(CurrentDataContext).ToList

            Dim CurrentValues As New Dictionary(Of String, Decimal)

            For CurrentIndex As Integer = 0 To Count
                Dim CurrentMonthDate As Date = Date.Now.AddDays(-CurrentIndex)
                Dim CurrentDate As String = String.Format("{0} {1} {2}", CurrentMonthDate.Day, MonthName(CurrentMonthDate.Month), CurrentMonthDate.Year.ToString)
                Dim Sales As Decimal = 0

                Sales = Items.Where(Function(f) f.Month = CurrentMonthDate.Month And f.Year = CurrentMonthDate.Year And f.Day = CurrentMonthDate.Day).Sum(Function(f) f.Sales)

                CurrentValues.Add(CurrentDate, Sales)
            Next CurrentIndex

            Return CurrentValues
        End Function

        Public Shared Function GetDailySalesFormatedForChart(Count As Integer) As String
            Dim CurrentValues As Dictionary(Of String, Decimal) = GetDailySalesAsDictionary(Count)
            Dim CurrentStringBuilder As New StringBuilder

            CurrentStringBuilder.Append("[['Date', 'Sales'],")

            For Each Item In CurrentValues.Reverse
                If Item.Equals(CurrentValues.First) Then
                    CurrentStringBuilder.AppendFormat("['{0}',{1}]", Item.Key, Item.Value)
                Else
                    CurrentStringBuilder.AppendFormat("['{0}',{1}],", Item.Key, Item.Value)
                End If
            Next

            CurrentStringBuilder.Append("]")

            Return CurrentStringBuilder.ToString
        End Function

        Public Shared GetSimpleOrderViewsFunc As Func(Of CommerceDataContext, Data.Linq.Table(Of EStore_SimpleOrderView)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) db.EStore_SimpleOrderViews)

        'Public Shared GetDistributorSimpleOrderViewsFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of EStore_SimpleOrderView)) = _
        '    CompiledQuery.Compile(Function(db As CommerceDataContext, DistributorID As Integer) _
        '                                From o In db.Orders _
        '                                Join a In db.Addresses On o.SAddressID Equals a.ID _
        '                                Join su In db.SimpleUsers On o.UserID Equals su.ID _
        '                                Let OrderItems = (From oi in db.OrderItems Where oi.OrderID = o.ID Select oi.Quantity).Sum, _
        '                                OITotal = (From oi in db.OrderItems Where oi.OrderID = o.ID Select New With { .Total = oi.Quantity * oi.Price }).Sum _
        '                                Select New EStore_SimpleOrderView With {.ID = o.ID, .Date = o.OrderDate, .UserID = o.UserID, .User = su.FirstName & " " & su.LastName, _
        '                                                 .ApprovalState = o.ApprovalState, .Email = su.Email, .Items = OrderItems, .SubTotal = .OITotal, _
        '                                                 .Taxes = o.TaxPercent * (.OITotal + o.ShipCharge), .Total = .SubTotal + .ShipCharge + .Taxes, .PaymentType = o.PaymentType, .ShipCharge = o.ShipCharge, .Shipped = })

        'Public Shared GetDistributorSimpleOrderViewsFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of EStore_SimpleOrderView)) = _
        '    CompiledQuery.Compile(Function(db As CommerceDataContext, DistributorID As Integer) _
        '                                From so In db.EStore_SimpleOrderViews _
        '                                Join o In db.Orders On o.ID Equals so.ID _
        '                                Join a In db.Addresses On o.SAddressID Equals a.ID _
        '                                Let DistributorUserID = (From z In db.FixedShippingZones Where db.CheckPrefix(a.PCZIP.Substring(0, 3), z.PrefixLow, z.PrefixHigh) Select z).FirstOrDefault.DistributorUserID _
        '                                Where DistributorUserID = DistributorID _
        '                                Select so)

        Public Shared IsDistributorsOrderFunc As Func(Of CommerceDataContext, Integer, Integer, Boolean) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, DistributorId As Integer, OrderId As Integer) _
                                    If((From o In db.Orders _
                                    Join a In db.Addresses On o.SAddressID Equals a.ID _
                                    Let DistributorUserID = FixedShippingZoneController.GetShippingZoneDistributor(a.PCZIP.Substring(0, 3)) _
                                    Where o.ID = OrderId And DistributorUserID = DistributorId _
                                    Select o.ID).Count > 0, True, False))

        Public Shared DistributorHasOrderFunc As Func(Of CommerceDataContext, Integer, Boolean) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, DistributorId As Integer) _
                                    If((From o In db.Orders _
                                    Join a In db.Addresses On o.SAddressID Equals a.ID _
                                    Let DistributorUserId = FixedShippingZoneController.GetShippingZoneDistributor(a.PCZIP.Substring(0, 3)) _
                                    Where DistributorUserId = DistributorId _
                                    Select o.ID).Count > 0, True, False))

        Public Shared GetOrderPrefixFunc As Func(Of CommerceDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, OrderID As Integer) _
                                    (From o In db.Orders Join a In db.Addresses On o.SAddressID Equals a.ID _
                                    Where o.ID = OrderID Select a.PCZIP.Substring(0, 3)).FirstOrDefault)

        Public Shared Function IsDistributorsOrder(ByVal DistributorID As Integer, ByVal OrderID As Integer) As Boolean
            Dim db As New CommerceDataContext
            Dim Prefix As String = GetOrderPrefixFunc(db, OrderID)

            If Not Prefix Is Nothing And Not Prefix = String.Empty Then
                Dim PrefixDistributorID As Integer = FixedShippingZoneController.GetShippingZoneDistributor(Prefix)

                If PrefixDistributorID = DistributorID Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        Public Shared Function DistributorHasOrder(ByVal DistributorId As Integer) As Boolean
            Dim HasOrders As Boolean = False

            If Settings.UseFixedRate Then
                Dim CurrentOrders As List(Of Order) = New OrderController().GetElements()

                For Each Item As Order In CurrentOrders
                    Dim Prefix As String = New AddressController().GetElement(Item.SAddressID).PCZIP.Substring(0, 3)
                    If FixedShippingZoneController.GetShippingZoneDistributor(Prefix) = DistributorId Then
                        HasOrders = True
                        Exit For
                    End If
                Next
            End If

            Return HasOrders
        End Function

        Public Shared Function DistributorsOrders(ByVal DistributorID As Integer) As List(Of EStore_SimpleOrderView)
            Dim db As New CommerceDataContext

            Dim orders As List(Of Order) = New OrderController().GetElements()
            Dim OrdersView As List(Of EStore_SimpleOrderView) = GetSimpleOrderViewsFunc(db).ToList

            For Each item As Order In orders
                Dim id As Integer = item.ID
                Dim prefix As String = New AddressController().GetElement(item.SAddressID).PCZIP.Substring(0, 3)
                If Not FixedShippingZoneController.GetShippingZoneDistributor(prefix) = DistributorID Then
                    OrdersView.Remove((From ov In OrdersView Where ov.ID = id Select ov).FirstOrDefault)
                End If
            Next

            Return OrdersView
        End Function

        Public Shared Function CheckOrderPermissions(ByVal OrderUserID As Integer, ByVal email As String) As Boolean
            Dim user As SimpleUser = New SimpleUserController().GetElement(OrderUserID)

            Dim username As String = Membership.GetUserNameByEmail(email)
            Dim UserIsAdmin As Boolean = False

            If Not username Is Nothing Then
                UserIsAdmin = Roles.IsUserInRole(username, "Administrator")
            End If

            If UserIsAdmin Or user.Email = email Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function CheckDistributorOrderPermissions(ByVal OrderUserID As Integer, ByVal username As String) As Boolean
            Dim userID As Integer = SimpleUserController.GetUserIDByUserName(username)

            If Roles.IsUserInRole(username, "Administrator") Or OrderUserID = userID Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared GetUnshippedOrderCountFunc As Func(Of CommerceDataContext, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                    (From o In db.Orders Where o.ShipDate Is Nothing AndAlso o.ApprovalState = 1 Select o).Count)

        Public Shared Function GetUnshippedOrderCount() As Integer
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentUnshippedOrderCount As Integer = GetUnshippedOrderCountFunc(CurrentDataContext)

            CurrentDataContext.Dispose()

            Return CurrentUnshippedOrderCount
        End Function

        Public Shared Function GetOrderWeight(ByVal OrderID As Integer) As Decimal
            Dim weight As Decimal = 0

            For Each item As OrderItem In New OrderItemController().GetOrderElements(OrderID)
                Dim currentProduct As New ProductController

                weight += currentProduct.GetElement(item.ProductID).Weight * item.Quantity
            Next

            Return weight
        End Function

        Public Shared GetEarliestOrderYearFunc As Func(Of CommerceDataContext, Integer) = _
                                    CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                    (From o In db.Orders Select o.OrderDate.Year Distinct).Min(Function(o) o))

        Public Shared GetLatestOrderYearFunc As Func(Of CommerceDataContext, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                    (From o In db.Orders Select o.OrderDate.Year Distinct).Max(Function(o) o))

        Public Shared Function GetEarliestOrderYear() As Integer
            Dim db As New CommerceDataContext

            Return GetEarliestOrderYearFunc(db)
        End Function

        Public Shared Function GetLatestOrderYear() As Integer
            Dim db As New CommerceDataContext

            Return GetLatestOrderYearFunc(db)
        End Function

        Public Shared Function GetOrderPickupAddress(ByVal AddressId As Integer) As String
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentAddress As Address = AddressController.GetAddressByIdFunc(CurrentDataContext, AddressId)

            Dim Prefix As String

            If CurrentAddress.Province.Country.Code = "CA" Then
                Prefix = CurrentAddress.PCZIP.Substring(0, 3)
            Else
                Prefix = CurrentAddress.PCZIP
            End If

            Dim CurrentDistributorId As Integer = FixedShippingZoneController.GetShippingZoneDistributor(Prefix)
            Dim CurrentDistributorUser As RegisteredUser = RegisteredUserController.GetRegisteredUserByUserIdFunc(CurrentDataContext, CurrentDistributorId)

            Dim PickupAddress As String

            If CurrentDistributorUser Is Nothing Then
                PickupAddress = "None"
            Else
                Dim CurrentDistributorAddress As Address = AddressController.GetAddressByIdFunc(CurrentDataContext, CurrentDistributorUser.AddressID)

                PickupAddress = CurrentDistributorAddress.Address & ", " & CurrentDistributorAddress.City & ", " & CurrentDistributorAddress.Province.Name & ", " & CurrentDistributorAddress.Province.Country.Name & ", " & CurrentDistributorAddress.PCZIP
            End If

            GetOrderPickupAddress = PickupAddress

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetOrderPickupAddress(ByVal Prefix As String) As String
            Dim CurrentStoreDataContext As New CommerceDataContext

            Dim DistributorId As Integer = FixedShippingZoneController.GetShippingZoneDistributor(Prefix)
            Dim DistributorUser As RegisteredUser = RegisteredUserController.GetRegisteredUserByUserIdFunc(CurrentStoreDataContext, DistributorId)

            Dim PickupAddress As String

            If DistributorUser Is Nothing Then
                PickupAddress = "None"
            Else
                Dim DistributorAddress As Address = AddressController.GetAddressByIdFunc(CurrentStoreDataContext, DistributorUser.AddressID)

                PickupAddress = DistributorAddress.Address & ", " & DistributorAddress.City & ", " & DistributorAddress.Province.Name & ", " & DistributorAddress.Province.Country.Name & ", " & DistributorAddress.PCZIP
            End If

            Return PickupAddress
        End Function

    End Class

    Public Class OrderItemController
        Inherits DataControllerClass

        Public Shared GetOrderItemsFunc As Func(Of CommerceDataContext, IQueryable(Of OrderItem)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext) From oi In db.OrderItems Select oi)

        Public Shared GetOrderItemByIdFunc As Func(Of CommerceDataContext, Integer, OrderItem) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, OrderItemId As Integer) _
                                              (From oi In db.OrderItems Where oi.ID = OrderItemId Select oi).FirstOrDefault)

        Public Shared GetOrderItemsByOrderIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of OrderItem)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, OrderId As Integer) From oi In db.OrderItems Where oi.OrderID = OrderId Select oi)

        Public Shared GetOrderItemsQuantityByOrderIdFunc As Func(Of CommerceDataContext, Integer, Integer) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, OrderId As Integer) _
                                              (From oi In db.OrderItems Where oi.OrderID = OrderId Select New With {.Quantity = oi.Quantity}).Sum(Function(oi) oi.Quantity))

        Public Shared GetOrderItemsSummaryFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of OrderItemSummary)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, OrderId As Integer) _
                                              (From oi In db.OrderItems Join p In db.Products On oi.ProductID Equals p.ID Join c In db.Colors On oi.ColorID Equals c.ID _
                                               Join s In db.Sizes On oi.SizeID Equals s.ID Where oi.OrderID = OrderId _
                                               Select New OrderItemSummary With {.ID = oi.ID, .ImageID = p.DefaultImageID, .Name = p.Name, .Quantity = oi.Quantity, .Colour = c.Name, _
                                                                .Size = s.Name, .Shipped = oi.Shipped, .ShippedDate = oi.ShippedDate, .UnitPrice = oi.Price, .Price = oi.Price * oi.Quantity, _
                                                                .TrackingNumber = oi.TrackingNum}))


        Public Shared GetOrderInvoiceItemsFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of InvoiceOrderItem)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, OrderId As Integer) _
                                From oi In db.OrderItems _
                                Join p In db.Products On p.ID Equals oi.ProductID _
                                Join c In db.Colors On c.ID Equals oi.ColorID _
                                Join s In db.Sizes On s.ID Equals oi.SizeID _
                                Where oi.OrderID = OrderId _
                                Select New InvoiceOrderItem With _
                                {.ID = oi.ID, .ProductID = oi.ProductID, .Quantity = oi.Quantity, .Name = p.Name, .Color = c.Name, .Size = s.Name, .Price = oi.Price, .ImageID = p.DefaultImageID})



        Public Overloads Function Create(ByVal OrderId As Integer, ByVal ProductId As Integer, ByVal ColorId As Integer, ByVal SizeId As Integer, ByVal Quantity As Integer, ByVal Price As Decimal, ByVal Shipped As Boolean, ByVal ShippedDate As DateTime, ByVal TrackingNum As String) As Integer
            Dim item As New OrderItem
            Dim itemId As Integer

            item.OrderID = OrderId
            item.ProductID = ProductId
            item.ColorID = ColorId
            item.SizeID = SizeId
            item.Quantity = Quantity
            item.Price = Price
            item.Shipped = Shipped
            If Not ShippedDate = Date.MinValue Then
                item.ShippedDate = ShippedDate
            End If
            item.TrackingNum = TrackingNum

            db.OrderItems.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = GetOrderItemByIDFunc(db, ID)

                db.OrderItems.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal OrderID As Integer, ByVal ProductID As Integer, ByVal ColorID As Integer, ByVal SizeID As Integer, ByVal Quantity As Integer, ByVal Price As Decimal, ByVal Shipped As Boolean, ByVal ShippedDate As DateTime, ByVal TrackingNum As String) As Boolean
            Dim item As OrderItem = GetOrderItemByIdFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("Order Item with ID: " & ID.ToString & " does not exist.")
            Else
                item.OrderID = OrderID
                item.ProductID = ProductID
                item.ColorID = ColorID
                item.SizeID = SizeID
                item.Quantity = Quantity
                item.Price = Price
                item.Shipped = Shipped
                If Not ShippedDate = Date.MinValue Then
                    item.ShippedDate = ShippedDate
                End If
                item.TrackingNum = TrackingNum

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As OrderItem
            Dim item As OrderItem = GetOrderItemByIDFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("Order Item " & ID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElements() As Collections.Generic.List(Of OrderItem)
            Dim items = GetOrderItemsFunc(db)

            If items Is Nothing Then
                Throw New Exception("There are no Order Items")
            Else
                Return items.ToList
            End If
        End Function

        Public Function GetOrderElements(ByVal OrderID As Integer) As Collections.Generic.List(Of OrderItem)
            Dim items = GetOrderItemsByOrderIDFunc(db, OrderID) ' From c In db.OrderItems Where c.OrderID = OrderID Select c

            If items Is Nothing Then
                Throw New Exception("There are no Order Items for order " & OrderID.ToString)
            Else
                Return items.ToList
            End If
        End Function

        Public Function DeleteOrderItems(ByVal OrderID As Integer) As Boolean
            Try
                Dim items = GetOrderItemsByOrderIDFunc(db, OrderID).ToList

                For Each item In items
                    ProductDownloadController.DeleteDownloadsByOrderItemID(item.ID)
                Next

                Dim deleteItems = GetOrderItemsByOrderIDFunc(db, OrderID)

                db.OrderItems.DeleteAllOnSubmit(items)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function ReturnItem(ByVal ItemId As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentOrderItem As OrderItem = GetOrderItemByIdFunc(CurrentDataContext, ItemId)

            If CurrentOrderItem Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("Order item: " & ItemId.ToString & " does not exist.")
            Else
                CurrentOrderItem.Price = -CurrentOrderItem.Price

                CurrentDataContext.SubmitChanges()

                CurrentDataContext.Dispose()
            End If

            Return True
        End Function

    End Class

    Public Class OrderTotals
        Public Items As Integer
        Public SubTotal As Decimal
        Public ShippingCharge As Decimal
        Public Discount As Decimal
        Public TaxRate As Decimal
        Public Tax As Decimal
        Public Total As Decimal

        Public Sub New(ByVal _items As Integer, ByVal _subTotal As Decimal, ByVal _shippingCharge As Decimal, ByVal _taxRate As Decimal, ByVal _discount As Decimal)
            Items = _items
            SubTotal = _subTotal
            ShippingCharge = _shippingCharge
            TaxRate = _taxRate
            Discount = _discount

            Tax = ((SubTotal + ShippingCharge) - Discount) * TaxRate

            Total = Math.Round(((SubTotal + ShippingCharge) - Discount) + Tax, 2)
        End Sub

    End Class

    Public Class SalesSummary
        Private _day As Integer
        Private _month As Integer
        Private _year As Integer
        Private _sales As Decimal

        Public Sub New(ByVal day As Integer, ByVal month As Integer, ByVal year As Integer, ByVal sales As Decimal)
            _day = day
            _month = month
            _year = year
            _sales = sales
        End Sub

        Public Property Day() As Integer
            Get
                Return _day
            End Get
            Set(ByVal value As Integer)
                _day = value
            End Set
        End Property

        Public Property Month() As Integer
            Get
                Return _month
            End Get
            Set(ByVal value As Integer)
                _month = value
            End Set
        End Property

        Public Property Year() As Integer
            Get
                Return _year
            End Get
            Set(ByVal value As Integer)
                _year = value
            End Set
        End Property

        Public Property Sales() As Decimal
            Get
                Return _sales
            End Get
            Set(ByVal value As Decimal)
                _sales = value
            End Set
        End Property

    End Class

    Public Class MonthlySalesSummary
        Private _month As Integer
        Private _year As Integer
        Private _sales As Decimal

        Public Sub New(ByVal year As Integer, ByVal month As Integer, ByVal sales As Decimal)
            _month = month
            _year = year
            _sales = sales
        End Sub

        Public Property Year() As Integer
            Get
                Return _year
            End Get
            Set(ByVal value As Integer)
                _year = value
            End Set
        End Property

        Public Property Month() As Integer
            Get
                Return _month
            End Get
            Set(ByVal value As Integer)
                _month = value
            End Set
        End Property

        Public Property Sales() As Decimal
            Get
                Return _sales
            End Get
            Set(ByVal value As Decimal)
                _sales = value
            End Set
        End Property

    End Class

    Public Class OrderItemSummary
        Private _itemID As Integer
        Private _imageID As Integer
        Private _name As String
        Private _quantity As Integer
        Private _colour As String
        Private _size As String
        Private _shipped As Boolean
        Private _shippedDate As DateTime?
        Private _unitPrice As Decimal
        Private _price As Decimal
        Private _trackingNumber As String

        Public Property ID() As Integer
            Get
                Return _itemID
            End Get
            Set(ByVal value As Integer)
                _itemID = value
            End Set
        End Property

        Public Property ImageID() As Integer
            Get
                Return _imageID
            End Get
            Set(ByVal value As Integer)
                _imageID = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Quantity() As Integer
            Get
                Return _quantity
            End Get
            Set(ByVal value As Integer)
                _quantity = value
            End Set
        End Property

        Public Property Colour() As String
            Get
                Return _colour
            End Get
            Set(ByVal value As String)
                _colour = value
            End Set
        End Property

        Public Property Size() As String
            Get
                Return _size
            End Get
            Set(ByVal value As String)
                _size = value
            End Set
        End Property

        Public Property Shipped() As Boolean
            Get
                Return _shipped
            End Get
            Set(ByVal value As Boolean)
                _shipped = value
            End Set
        End Property

        Public Property ShippedDate() As DateTime?
            Get
                Return _shippedDate
            End Get
            Set(ByVal value As DateTime?)
                _shippedDate = value
            End Set
        End Property

        Public Property UnitPrice() As Decimal
            Get
                Return _unitPrice
            End Get
            Set(ByVal value As Decimal)
                _unitPrice = value
            End Set
        End Property

        Public Property Price() As Decimal
            Get
                Return _price
            End Get
            Set(ByVal value As Decimal)
                _price = value
            End Set
        End Property

        Public Property TrackingNumber() As String
            Get
                Return _trackingNumber
            End Get
            Set(ByVal value As String)
                _trackingNumber = value
            End Set
        End Property

    End Class

    Public Class InvoiceOrderItem
        Private _id As Integer
        Private _productId As Integer
        Private _name As String
        Private _imageID As Integer
        Private _color As String
        Private _size As String
        Private _quantity As Integer
        Private _price As Decimal
        Private _total As Decimal

        Public Property ID() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property ProductID() As Integer
            Get
                Return _productId
            End Get
            Set(ByVal value As Integer)
                _productId = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property ImageID() As Integer
            Get
                Return _imageID
            End Get
            Set(ByVal value As Integer)
                _imageID = value
            End Set
        End Property

        Public Property Color() As String
            Get
                Return _color
            End Get
            Set(ByVal value As String)
                _color = value
            End Set
        End Property

        Public Property Size() As String
            Get
                Return _size
            End Get
            Set(ByVal value As String)
                _size = value
            End Set
        End Property

        Public Property Quantity() As Integer
            Get
                Return _quantity
            End Get
            Set(ByVal value As Integer)
                _quantity = value
            End Set
        End Property

        Public Property Price() As Decimal
            Get
                Return _price
            End Get
            Set(ByVal value As Decimal)
                _price = value
            End Set
        End Property

        Public ReadOnly Property Total() As Decimal
            Get
                Return (_price * _quantity)
            End Get
        End Property

    End Class

End Namespace