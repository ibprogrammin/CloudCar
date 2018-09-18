Imports CloudCar.CCFramework.Core.BeanStreamBilling
Imports CloudCar.CCFramework.Core.Shipping
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.PaymentProcessing
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce.ShoppingCart
Imports CloudCar.CCFramework.Commerce.BusinessProcess.PaymentProcessing

Namespace CCFramework.Commerce.BusinessProcess

    Public Class Purchase
        Private ReadOnly _SessionId As String
        Private ReadOnly _UserId As Integer
        Private ReadOnly _NewOrderId As Integer
        Private ReadOnly _UserSignedIn As Boolean

        Private _UserEmailAddress As String
        Private _UserPhoneNumber As String
        Private _BillingName As BillingName

        Private _BillingAddress As Address
        Private _ShippingAddressId As Integer
        Private _ShippingAddress As Address
        Private _Province As Province
        Private _Country As Country

        Private _PaymentType As EPaymentType
        Private ReadOnly _UserPriceLevel As EPriceLevel
        Private ReadOnly _Discount As Decimal
        Private _SelectedRate As ShippingOption
        Private _TaxRate As Decimal
        Private _OrderTotal As OrderTotals


        Public Sub New(ByVal PaymentType As EPaymentType, ByVal SessionId As String, ByVal SignedIn As Boolean, ByVal SelectedRate As ShippingOption, ByVal Discount As Decimal, ByVal BillAddressId As Integer, ByVal ShipAddressId As Integer, ByVal BillingName As String, ByVal Email As String, ByVal Phone As String)
            _BillingName = New BillingName(BillingName)
            _UserEmailAddress = Email
            _UserPhoneNumber = Phone

            _SessionId = SessionId
            _UserSignedIn = SignedIn
            _UserId = GetUserId()

            If _UserSignedIn Then
                _UserPriceLevel = RegisteredUserController.GetUserPriceLevel(_UserId)
            Else
                _UserPriceLevel = EPriceLevel.Regular
            End If

            SetPaymentType(PaymentType)
            SetAddress(BillAddressId, ShipAddressId)

            _Discount = Discount

            _SelectedRate = SelectedRate

            SetTaxRate(_Province.Tax + _Country.Tax)

            'pOrderTotoal = GetOrderTotal()

            _NewOrderId = CreateOrder()

            GetShoppingCartItems()
        End Sub

        Public Sub SetPaymentType(ByVal PaymentType As EPaymentType)
            _PaymentType = PaymentType
        End Sub

        Public Sub SetAddress(ByVal BillAddressId As Integer, ByVal ShippingAddressId As Integer)
            Dim CurrentAddressController As New AddressController

            _ShippingAddressId = ShippingAddressId
            _ShippingAddress = CurrentAddressController.GetElement(ShippingAddressId)
            _BillingAddress = CurrentAddressController.GetElement(BillAddressId)
            _Province = _ShippingAddress.Province
            _Country = _ShippingAddress.Province.Country

            CurrentAddressController.Dispose()
        End Sub

        Public Function UpdateBillingAddress(ByVal Address As String, ByVal City As String, ByVal PostalZipCode As String, ByVal ProvinceId As Integer, ByVal CountryId As Integer) As Boolean
            If Not _BillingAddress.ID = _ShippingAddressId Then
                Dim CurrentAddressController As New AddressController
                Dim CurrentAddressUpdated As Boolean = CurrentAddressController.Update(_BillingAddress.ID, Address, City, PostalZipCode, ProvinceId, CountryId)
                CurrentAddressController.Dispose()

                Return CurrentAddressUpdated
            Else
                Return False
            End If
        End Function

        Private Function GetUserId() As Integer
            Dim UserId As Integer

            If _UserSignedIn Then
                UserId = SimpleUserController.GetUserIdByUserName(Membership.GetUser.UserName)
            Else
                Dim CurrentSimpleUserController As New SimpleUserController
                UserId = CurrentSimpleUserController.Create(_BillingName.First, _BillingName.Middle, _BillingName.Last, _UserEmailAddress, _UserPhoneNumber)
                CurrentSimpleUserController.Dispose()
            End If

            Return UserId
        End Function

        Public Function UpdateUser(ByVal BillingName As String, ByVal Email As String, ByVal PhoneNumber As String) As Boolean
            If Membership.GetUser Is Nothing Then
                _BillingName = New BillingName(BillingName)
                _UserEmailAddress = Email
                _UserPhoneNumber = PhoneNumber

                Dim CurrentSimpleUserController As New SimpleUserController
                Dim CurrentSimpleUserUpdated As Boolean = CurrentSimpleUserController.Update(_UserId, _BillingName.First, _BillingName.Middle, _BillingName.Last, Email, PhoneNumber)
                CurrentSimpleUserController.Dispose()

                Return CurrentSimpleUserUpdated
            Else
                Return False
            End If
        End Function

        Private Sub SetTaxRate(ByVal TaxRate As Decimal)
            _TaxRate = TaxRate
        End Sub

        Private Function GetOrderTotal() As OrderTotals
            Dim CurrentOrderTotal As OrderTotals

            CurrentOrderTotal = New OrderTotals(OrderController.GetItemCount(_NewOrderId), OrderController.GetOrderSubTotal(_NewOrderId), _SelectedRate.Total, _TaxRate, _Discount)

            'If pSignedIn Then
            '    CurrentOrderTotal = New OrderTotals(ShoppingCartController.GetItemCount(pUserID), SMCommerce.ShoppingCartController.GetSubTotal(pUserID, pPriceLevel), pSelectedRate.Total, pTaxRate, pDiscount)
            'Else
            '    CurrentOrderTotal = New OrderTotals(ShoppingCartController.GetItemCount(pSessionID), SMCommerce.ShoppingCartController.GetSubTotal(pSessionID, EPriceLevel.Regular), pSelectedRate.Total, pTaxRate, pDiscount)
            'End If

            Return CurrentOrderTotal
        End Function

        Private Function GetShoppingCartItems() As List(Of Model.ShoppingCart)
            Dim CurrentShoppingCartController As New ShoppingCartController

            Dim ShoppingCartItems As List(Of Model.ShoppingCart)

            If _UserSignedIn Then
                ShoppingCartItems = CurrentShoppingCartController.GetShoppingCartItems(_UserId)
            Else
                ShoppingCartItems = CurrentShoppingCartController.GetShoppingCartItems(_SessionId)
            End If

            GetShoppingCartItems = ShoppingCartItems

            CurrentShoppingCartController.Dispose()
        End Function

        Private Sub MoveShoppingCartItemsToOrderItems()
            Dim CurrentOrderItemController As New OrderItemController()

            For Each ShoppingCartItem In GetShoppingCartItems()
                Dim Price As Decimal = ProductController.GetPrice(_UserPriceLevel, ShoppingCartItem.ProductID)

                If SalesRequestController.CheckShoppingCartItemIsSalesRequest(ShoppingCartItem.ID) Then
                    Dim CurrentRequestCode As Guid = SalesRequestController.GetRedemptionCodeByShoppingCartId(ShoppingCartItem.ID)

                    SalesRequestController.SetRedemptionRequestRedeemed(CurrentRequestCode.ToString, _NewOrderId)
                End If

                CurrentOrderItemController.Create(_NewOrderId, ShoppingCartItem.ProductID, ShoppingCartItem.ColorID, ShoppingCartItem.SizeID, ShoppingCartItem.Quantity, Price, False, Date.MinValue, "")

                If ProductController.DoesProductTrackInventory(ShoppingCartItem.ProductID) Then
                    Dim CurrentInventoryController As New InventoryController()

                    CurrentInventoryController.RemoveProductInventory(ShoppingCartItem.ProductID, ShoppingCartItem.ColorID, ShoppingCartItem.SizeID, ShoppingCartItem.Quantity)
                    CurrentInventoryController.Dispose()
                End If
            Next

            CurrentOrderItemController.Dispose()
        End Sub

        Private Sub ClearShoppingCartItems()
            Dim CurrentShoppingCartController As New ShoppingCartController

            For Each item In GetShoppingCartItems()
                CurrentShoppingCartController.Delete(item.ID)
            Next

            CurrentShoppingCartController.Dispose()
        End Sub

        Private Function CreateOrder() As Integer
            Dim CurrentOrderController As New OrderController

            CreateOrder = CurrentOrderController.Create(_BillingName.Full, _UserId, _Discount, _TaxRate, _PaymentType, EApprovalState.Pending, _
                                                _SelectedRate.Total, _SelectedRate.Company, _SelectedRate.Service, _ShippingAddressId, _BillingAddress.ID, "")

            CurrentOrderController.Dispose()
        End Function

        'TODO encapsulate into a seperate class eg. Credit Card info class
        Private _CreditCardNumber As String
        Private _ExpiryYear As Integer
        Private _ExpiryMonth As Integer
        Private _CreditCardValidationNumber As Integer

        Public Sub SetCreditCardDetails(ByVal CreditCardNumber As String, ByVal ExpiryYear As Integer, ByVal ExpiryMonth As Integer, ByVal CreditCardValidationNumber As Integer)
            _CreditCardNumber = CreditCardNumber
            _ExpiryYear = ExpiryYear
            _ExpiryMonth = ExpiryMonth
            _CreditCardValidationNumber = CreditCardValidationNumber
        End Sub

        'TODO encapsulate into a seperate class eg. billing info class
        Private _Recurring As Boolean = False
        Private _BillingPeriod As Char
        Private _BillingIncrement As Integer
        Private _FirstBilling As String 'Set to MMDDYYYY
        Private _SecondBilling As String 'Set to MMDDYYYY
        Private _RecurringCharge As Boolean
        Private _RecurringExpiry As String 'Set to MMDDYYYY
        Private _BillAtEndMonth As Boolean
        Private _ApplyTax1 As Boolean
        Private _ApplyTax2 As Boolean

        Public Sub SetRecuringPayment(ByVal BillingPeriod As BillingPeriod, ByVal BillingIncrement As Integer)
            _Recurring = True
            _BillingPeriod = ConvertBillingPeriodToChar(BillingPeriod)
            _BillingIncrement = BillingIncrement

            _FirstBilling = Date.Now.ToString("MMddyyyy")
            _SecondBilling = ""

            _RecurringCharge = True
            _RecurringExpiry = ""

            _BillAtEndMonth = False

            _BillAtEndMonth = False
            _ApplyTax1 = False
            _ApplyTax2 = False
        End Sub

        Public Function ProcessPayment(Optional ByVal Retry As Boolean = False) As PaymentStatusMessage
            If Not Retry Then
                MoveShoppingCartItemsToOrderItems()
            End If

            _OrderTotal = GetOrderTotal()

            Dim StatusMessage As PaymentStatusMessage = Nothing
            Dim CurrentPaymentAuthorized As Boolean = False
            Dim CurrentPaymentUpdated As Boolean = False
            Dim CurrentOrderAuthorized As Boolean = False

            Dim Cryptography As New EasyCryptography
            Dim EncryptionKey As String = Cryptography.GenerateEncryptionKey()
            Dim CreditCardNumber As String = Cryptography.Encrypt(_CreditCardNumber, EncryptionKey)
            Dim CreditCardExpirationDate As Date = New Date(_ExpiryYear, _ExpiryMonth, 1)

            Dim CurrentPaymentController As New PaymentController

            Select Case _PaymentType
                Case EPaymentType.None
                    _PaymentId = CurrentPaymentController.Create(_NewOrderId, _BillingName.Full, CreditCardExpirationDate.Month.ToString, CreditCardExpirationDate.Year.ToString.Substring(2), CreditCardNumber, EncryptionKey, _CreditCardValidationNumber.ToString, _OrderTotal.Total, PaymentStatus.Approved, 0, "Processed Successfully")
                    CurrentPaymentAuthorized = CurrentPaymentController.SetPaymentAuthorized(_PaymentId, "Manualy Process", DateTime.Now.ToString, -1, PaymentStatus.Approved, _OrderTotal.Total)

                    StatusMessage = New PaymentStatusMessage(True, "Success - No Payment Processing Engine", False, "", -1)
                Case EPaymentType.CreditCard
                    StatusMessage = New PaymentStatusMessage(False, "Error - No Payment Processing Engine", False, "", -1)
                Case EPaymentType.BeanStream
                    Dim TransactionDetails As BeanStreamTransaction

                    TransactionDetails = New BeanStreamTransaction(_OrderTotal.Total, _BillingName.Full, _UserPhoneNumber, _BillingAddress.Address, "", _BillingAddress.City, _Province.Code, _
                                                                    _BillingAddress.PCZIP, _Country.Code, _UserEmailAddress, _NewOrderId.ToString, _CreditCardNumber, _BillingName.Full, _
                                                                    CreditCardExpirationDate.Month, Integer.Parse(CreditCardExpirationDate.Year.ToString.Substring(2)), _CreditCardValidationNumber)

                    If _Recurring Then
                        TransactionDetails.SetRecurring(_Recurring, _BillingPeriod, _BillingIncrement, _FirstBilling, _SecondBilling, _RecurringCharge, _RecurringExpiry, _BillAtEndMonth, _ApplyTax1, _ApplyTax2)
                    End If

                    Dim BeanStreamPayment As New BeanStreamClient(TransactionDetails)
                    Dim ReturnMessage As BeanStreamReturnMessage

                    ReturnMessage = BeanStreamPayment.ProcessRequest()

                    Select Case ReturnMessage.Status
                        Case PaymentStatus.Approved
                            If Retry Then
                                CurrentPaymentUpdated = CurrentPaymentController.Update(_PaymentId, _NewOrderId, _BillingName.Full, CreditCardExpirationDate.Month.ToString, CreditCardExpirationDate.Year.ToString.Substring(2), CreditCardNumber, EncryptionKey, _CreditCardValidationNumber.ToString, _OrderTotal.Total, PaymentStatus.Approved, ReturnMessage.MessageID, ReturnMessage.MessageText)
                            Else
                                _PaymentId = CurrentPaymentController.Create(_NewOrderId, _BillingName.Full, CreditCardExpirationDate.Month.ToString, CreditCardExpirationDate.Year.ToString.Substring(2), CreditCardNumber, EncryptionKey, _CreditCardValidationNumber.ToString, _OrderTotal.Total, PaymentStatus.Approved, ReturnMessage.MessageID, ReturnMessage.MessageText)
                            End If

                            CurrentPaymentAuthorized = CurrentPaymentController.SetPaymentAuthorized(_PaymentId, ReturnMessage.AuthorizationCode, ReturnMessage.ProcessDate, CInt(ReturnMessage.TransactionID), PaymentStatus.Approved, ReturnMessage.ChargeAmount)

                            StatusMessage = New PaymentStatusMessage(True, ReturnMessage.Message, False, "", _PaymentId)
                        Case Else
                            'Handle Verified by Visa
                            If ReturnMessage.ResponseType = "R" Then
                                If Retry Then
                                    CurrentPaymentUpdated = CurrentPaymentController.Update(_PaymentId, _NewOrderId, _BillingName.Full, CreditCardExpirationDate.Month.ToString, CreditCardExpirationDate.Year.ToString.Substring(2), CreditCardNumber, EncryptionKey, _CreditCardValidationNumber.ToString, _OrderTotal.Total, ReturnMessage.Status, ReturnMessage.MessageID, ReturnMessage.MessageText)
                                Else
                                    _PaymentId = CurrentPaymentController.Create(_NewOrderId, _BillingName.Full, CreditCardExpirationDate.Month.ToString, CreditCardExpirationDate.Year.ToString.Substring(2), CreditCardNumber, EncryptionKey, _CreditCardValidationNumber.ToString, _OrderTotal.Total, ReturnMessage.Status, ReturnMessage.MessageID, ReturnMessage.MessageText)
                                End If

                                ' TODO: Send back the return message and the authorize page if this is a vbv purchase
                                StatusMessage = New PaymentStatusMessage(False, ReturnMessage.Message, True, ReturnMessage.PageContents, _PaymentId)
                            Else
                                If Retry Then
                                    CurrentPaymentUpdated = CurrentPaymentController.Update(_PaymentId, _NewOrderId, _BillingName.Full, CreditCardExpirationDate.Month.ToString, CreditCardExpirationDate.Year.ToString.Substring(2), CreditCardNumber, EncryptionKey, _CreditCardValidationNumber.ToString, _OrderTotal.Total, ReturnMessage.Status, ReturnMessage.MessageID, ReturnMessage.MessageText)
                                Else
                                    _PaymentId = CurrentPaymentController.Create(_NewOrderId, _BillingName.Full, CreditCardExpirationDate.Month.ToString, CreditCardExpirationDate.Year.ToString.Substring(2), CreditCardNumber, EncryptionKey, _CreditCardValidationNumber.ToString, _OrderTotal.Total, ReturnMessage.Status, ReturnMessage.MessageID, ReturnMessage.MessageText)
                                End If

                                StatusMessage = New PaymentStatusMessage(False, ReturnMessage.Message, False, "", _PaymentId)
                            End If
                    End Select
                Case EPaymentType.Paypal
                    Dim CurrentPaypalClient As New PayPalClient
                    With CurrentPaypalClient
                        .BuyerEmail = _UserEmailAddress
                        .AccountEmail = Settings.PayPalAccountEmailAddress
                        .Amount = _OrderTotal.Total
                        .LogoUrl = Settings.LogoImageUrl
                        .ItemName = ""
                        .InvoiceNo = _NewOrderId.ToString
                        .SuccessUrl = "/Shop/ThankYou.html"
                        .CancelUrl = "/Shop/Checkout.html"
                        .FirstName = _BillingName.First
                        .LastName = _BillingName.Last
                        .PostalCode = _BillingAddress.PCZIP
                        .City = _BillingAddress.City
                        .Country = _BillingAddress.Province.Country.Code
                        .State = _BillingAddress.Province.Code
                        .Address1 = _BillingAddress.Address
                        .Address2 = ""
                    End With

                    CurrentPaymentAuthorized = CurrentPaypalClient.ProcessRequest()

                    StatusMessage = New PaymentStatusMessage(CurrentPaymentAuthorized, CurrentPaypalClient.LastResponse, False, "", _PaymentId)
                Case EPaymentType.AuthorizeDotNet
                    Dim CurrentAuthorizeDotNetClient As New AuthorizeDotNetClient(Settings.AuthorizeDotNetLoginId, Settings.AuthorizeDotNetTransactionKey, Settings.TestMode)
                    CurrentAuthorizeDotNetClient.SetCustomerDetails(_BillingName.First, _BillingName.Last, _BillingAddress.Address, _BillingAddress.Province.Code, _BillingAddress.PCZIP)
                    CurrentAuthorizeDotNetClient.SetOrderDetails(_OrderTotal.Total, "")
                    CurrentAuthorizeDotNetClient.SetCreditCardDetails(_CreditCardNumber.Replace(" ", ""), String.Format("{0}{1}", _ExpiryMonth, _ExpiryYear)) '"05", "2015"

                    Dim ReturnMessage As AuthorizeDotNetResponseMessage = CurrentAuthorizeDotNetClient.ProcessRequest()

                    If AuthorizeDotNetClient.CheckResponseSuccess(ReturnMessage) Then
                        If Retry Then
                            CurrentPaymentUpdated = CurrentPaymentController.Update(_PaymentId, _NewOrderId, _BillingName.Full, CreditCardExpirationDate.Month.ToString, CreditCardExpirationDate.Year.ToString.Substring(2), CreditCardNumber, EncryptionKey, _CreditCardValidationNumber.ToString, _OrderTotal.Total, PaymentStatus.Approved, ReturnMessage.TransactionId, ReturnMessage.ResultDescription)
                        Else
                            _PaymentId = CurrentPaymentController.Create(_NewOrderId, _BillingName.Full, CreditCardExpirationDate.Month.ToString, CreditCardExpirationDate.Year.ToString.Substring(2), CreditCardNumber, EncryptionKey, _CreditCardValidationNumber.ToString, _OrderTotal.Total, PaymentStatus.Approved, ReturnMessage.TransactionId, ReturnMessage.ResultDescription)
                        End If

                        CurrentPaymentAuthorized = CurrentPaymentController.SetPaymentAuthorized(_PaymentId, ReturnMessage.AuthorizationCode, DateTime.Now.ToString, ReturnMessage.TransactionId, PaymentStatus.Approved, ReturnMessage.Amount)
                    Else
                        CurrentPaymentAuthorized = False
                    End If

                    StatusMessage = New PaymentStatusMessage(CurrentPaymentAuthorized, ReturnMessage.ResultDescription, False, "", _PaymentId)
                Case EPaymentType.Payza
                Case EPaymentType.OptimalPayments
                    'Integer.Parse(ddlCardType.SelectedValue)

                    Dim CurrentOptimalPaymentClient As New OptimalPaymentsClient(_NewOrderId.ToString, OptimalPayments.NetBanx.CardTypeV1.VI, _BillingAddress.PCZIP)

                    CurrentOptimalPaymentClient.SetClientName(_BillingName.First, _BillingName.Middle, _BillingName.Last)
                    CurrentOptimalPaymentClient.SetCreditCardDetails(CreditCardNumber, CreditCardExpirationDate.Month, CreditCardExpirationDate.Year, _CreditCardValidationNumber)
                    CurrentOptimalPaymentClient.SetOrderDetails(_NewOrderId, _OrderTotal.Total)

                    CurrentPaymentAuthorized = CurrentOptimalPaymentClient.ProcessPayment()

                    StatusMessage = New PaymentStatusMessage(CurrentPaymentAuthorized, "", False, "", _PaymentId)
                    'TODO Implement Card type display when using optimal payments
                    'CurrentOptimalPaymentClient.SimpleCharge(_NewOrderId.ToString, _OrderTotal.Total, CreditCardNumber, CreditCardExpirationDate, Integer.Parse(ddlCardType.SelectedValue), _CreditCardValidationNumber.ToString, _BillingName.First, _BillingName.Last, _BillingAddress.PCZIP)
                Case EPaymentType.Braintree
                    Dim CurrentBrainTreeClient As New BraintreeClient()
                    CurrentBrainTreeClient.SetGateway(Settings.BraintreeMerchantId, Settings.BraintreePublicKey, Settings.BraintreePrivateKey)
                    CurrentBrainTreeClient.SetRequest(_OrderTotal.Total, _CreditCardNumber.Replace(" ", ""), _ExpiryMonth.ToString, _ExpiryYear.ToString)

                    Dim ReturnMessage As String = CurrentBrainTreeClient.ProcessRequest()

                    If ReturnMessage.StartsWith("Success!") Then
                        CurrentPaymentAuthorized = True

                        If Retry Then
                            'CurrentPaymentUpdated = CurrentPaymentController.Update(_PaymentId, _NewOrderId, _BillingName.Full, CreditCardExpirationDate.Month.ToString, CreditCardExpirationDate.Year.ToString.Substring(2), CreditCardNumber, EncryptionKey, _CreditCardValidationNumber.ToString, _OrderTotal.Total, PaymentStatus.Approved, ReturnMessage.MessageID, ReturnMessage.MessageText)
                        Else
                            '_PaymentId = CurrentPaymentController.Create(_NewOrderId, _BillingName.Full, CreditCardExpirationDate.Month.ToString, CreditCardExpirationDate.Year.ToString.Substring(2), CreditCardNumber, EncryptionKey, _CreditCardValidationNumber, _OrderTotal.Total, PaymentStatus.Approved, ReturnMessage.MessageID, ReturnMessage.MessageText)
                        End If

                        'CurrentPaymentAuthorized = CurrentPaymentController.SetPaymentAuthorized(_PaymentId, ReturnMessage.AuthorizationCode, ReturnMessage.ProcessDate, ReturnMessage.TransactionID, PaymentStatus.Approved, ReturnMessage.ChargeAmount)
                    Else
                        CurrentPaymentAuthorized = False
                    End If

                    StatusMessage = New PaymentStatusMessage(CurrentPaymentAuthorized, ReturnMessage, False, "", _PaymentId)
                Case EPaymentType.Moneris
                    Dim CurrentMonerisClient As New MonerisClient(Settings.MonerisStoreId, Settings.MonerisApiToken)
                    CurrentMonerisClient.SetAvsInformation(_BillingAddress.Address.Substring(0, _BillingAddress.Address.IndexOf(" "c)), _BillingAddress.Address.Substring(_BillingAddress.Address.IndexOf(" "c)), _BillingAddress.PCZIP)
                    CurrentMonerisClient.SetCvdInformation(_CreditCardValidationNumber.ToString)
                    CurrentMonerisClient.SetOrderDetails(_NewOrderId.ToString, _OrderTotal.Total.ToString)
                    CurrentMonerisClient.SetCreditCardDetails(_CreditCardNumber.Replace(" ", ""), _ExpiryYear.ToString.Substring(1, 2) & _ExpiryMonth.ToString)

                    If CurrentMonerisClient.ProcessRequest() Then
                        If Retry Then
                            CurrentPaymentUpdated = CurrentPaymentController.Update(_PaymentId, _NewOrderId, _BillingName.Full, CreditCardExpirationDate.Month.ToString, CreditCardExpirationDate.Year.ToString.Substring(2), CreditCardNumber, EncryptionKey, _CreditCardValidationNumber.ToString, _OrderTotal.Total, PaymentStatus.Approved, CInt(CurrentMonerisClient.GetReceipt.GetStatusCode), CurrentMonerisClient.GetReceipt.GetMessage)
                        Else
                            _PaymentId = CurrentPaymentController.Create(_NewOrderId, _BillingName.Full, CreditCardExpirationDate.Month.ToString, CreditCardExpirationDate.Year.ToString.Substring(2), CreditCardNumber, EncryptionKey, _CreditCardValidationNumber.ToString, _OrderTotal.Total, PaymentStatus.Approved, CInt(CurrentMonerisClient.GetReceipt.GetStatusCode), CurrentMonerisClient.GetReceipt.GetMessage)
                        End If

                        CurrentPaymentAuthorized = CurrentPaymentController.SetPaymentAuthorized(_PaymentId, CurrentMonerisClient.GetReceipt.GetAuthCode, CurrentMonerisClient.GetReceipt.GetTransDate, CInt(CurrentMonerisClient.GetReceipt.GetTxnNumber), PaymentStatus.Approved, CDec(CurrentMonerisClient.GetReceipt.GetTransAmount))
                    Else
                        CurrentPaymentAuthorized = False
                    End If

                    StatusMessage = New PaymentStatusMessage(CurrentPaymentAuthorized, CurrentMonerisClient.GetReceipt.GetMessage, False, "", _PaymentId)
                Case Else
                    StatusMessage = New PaymentStatusMessage(False, "Error - No Payment Processing Engine", False, "", -1)
            End Select

            CurrentPaymentController.Dispose()

            If CurrentPaymentAuthorized Then
                ClearShoppingCartItems()

                CurrentOrderAuthorized = OrderController.SetOrderAuthorized(_NewOrderId)
            End If

            Return StatusMessage
        End Function

        Public Function RetryPayment() As PaymentStatusMessage

            Return ProcessPayment(True)

        End Function

        Private Function ConvertBillingPeriodToChar(ByVal BillingPeriod As BillingPeriod) As Char
            Select Case BillingPeriod
                Case BillingPeriod.Days
                    Return "D"c
                Case BillingPeriod.Weeks
                    Return "W"c
                Case BillingPeriod.Months
                    Return "M"c
                Case BillingPeriod.Years
                    Return "Y"c
                Case Else
                    Return "M"c
            End Select
        End Function

        Private _PaymentId As Integer = Nothing
        Public ReadOnly Property PaymentId() As Integer
            Get
                Return _PaymentId
            End Get
        End Property

        Public ReadOnly Property OrderId() As Integer
            Get
                Return _NewOrderId
            End Get
        End Property

    End Class

    Public Class BillingName
        Public First As String
        Public Last As String
        Public Middle As String
        Public Full As String

        Public Sub New(ByVal _name As String)
            Full = _name

            Dim names As String() = _name.Split(" "c)

            Dim firstName As String = names(0)
            Dim lastName As String = names(names.Count - 1)
            Dim middleName As String = ""

            If names.Count > 2 Then
                For Each item In names
                    If Not item = names(0) And Not item = names(names.Count - 1) Then
                        middleName &= item & " "
                    End If
                Next
            End If

            First = firstName
            Middle = middleName
            Last = lastName
        End Sub

        Public Sub New(ByVal _first As String, ByVal _middle As String, ByVal _last As String)
            First = _first
            Middle = _middle
            Last = _last
            If Not Middle = "" Then
                Full = _first & " " & _middle & " " & _last
            Else
                Full = _first & " " & _last
            End If

        End Sub

    End Class

    Public Class PaymentStatusMessage
        Private pApproved As Boolean
        Private pMessage As String
        Private pIsVBV As Boolean
        Private pVBVResponse As String
        Private pPaymentID As Integer

        Public Sub New(ByVal _approved As Boolean, ByVal _message As String, ByVal _isVBV As Boolean, ByVal _vbvResponse As String, ByVal _paymentID As Integer)
            pApproved = _approved
            pMessage = _message
            pIsVBV = _isVBV
            pVBVResponse = _vbvResponse
            pPaymentID = _paymentID
        End Sub

        Public Property Approved() As Boolean
            Get
                Return pApproved
            End Get
            Set(ByVal value As Boolean)
                pApproved = value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return pMessage
            End Get
            Set(ByVal value As String)
                pMessage = value
            End Set
        End Property

        Public Property IsVBV() As Boolean
            Get
                Return pIsVBV
            End Get
            Set(ByVal value As Boolean)
                pIsVBV = value
            End Set
        End Property

        Public Property VBVResponse() As String
            Get
                Return pVBVResponse
            End Get
            Set(ByVal value As String)
                pVBVResponse = value
            End Set
        End Property

        Public Property PaymentID() As Integer
            Get
                Return pPaymentID
            End Get
            Set(ByVal value As Integer)
                pPaymentID = value
            End Set
        End Property

    End Class

    Public Enum BillingPeriod
        Days = 1
        Weeks = 2
        Months = 3
        Years = 4
    End Enum

End Namespace