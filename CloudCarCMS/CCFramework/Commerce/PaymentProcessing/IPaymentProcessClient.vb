Namespace CCFramework.Commerce.PaymentProcessing

    Public Interface IPaymentProcessClient
        Sub SetCreditCardDetails(CardNumber As String, ExpiryMonth As Integer, ExpiryYear As Integer, Cvd As Integer)
        Sub SetCreditCardDetails(CardDetails As CreditCardDetails)
        Function GetCreditCardDetails() As CreditCardDetails

        Sub SetOrderDetails(OrderId As Integer, Amount As Decimal)

        Function GetClientName() As ClientName
        Sub SetClientName(Name As ClientName)
        Sub SetClientName(First As String, Middle As String, Last As String)

        Sub SetPaymentReceipt(OrderId As Integer, Amount As Decimal, AuthorizationCode As String, TransactionId As String, TransactionDate As String)
        Function GetPaymentReceipt() As PaymentReceipt

        Function ProcessPayment() As Boolean
        Function ProcessReturn() As Boolean
    End Interface

    Public MustInherit Class PaymentProcessClient
        Implements IPaymentProcessClient

        Friend ClientOrderId As Integer
        Friend ClientAmount As Decimal

        Friend ClientName As ClientName
        Friend ClientCardDetails As CreditCardDetails
        Friend ClientPaymentReceipt As PaymentReceipt
        
        Public MustOverride Function ProcessPayment() As Boolean Implements IPaymentProcessClient.ProcessPayment
        Public MustOverride Function ProcessReturn() As Boolean Implements IPaymentProcessClient.ProcessReturn

        Public Function GetClientName() As ClientName Implements IPaymentProcessClient.GetClientName
            Return ClientName
        End Function

        Public Sub SetClientName(ByVal Name As ClientName) Implements IPaymentProcessClient.SetClientName
            ClientName = Name
        End Sub

        Public Sub SetClientName(ByVal First As String, ByVal Middle As String, ByVal Last As String) Implements IPaymentProcessClient.SetClientName
            ClientName = New ClientName

            ClientName.First = First
            ClientName.Middle = Middle
            ClientName.Last = Last
        End Sub

        Public Sub SetPaymentReceipt(ByVal OrderId As Integer, ByVal Amount As Decimal, ByVal AuthorizationCode As String, ByVal TransactionId As String, ByVal TransactionDate As String) Implements IPaymentProcessClient.SetPaymentReceipt
            ClientPaymentReceipt = New PaymentReceipt(OrderId, Amount, AuthorizationCode, TransactionId, TransactionDate)
        End Sub

        Public Function GetPaymentReceipt() As PaymentReceipt Implements IPaymentProcessClient.GetPaymentReceipt
            Return ClientPaymentReceipt
        End Function

        Public Sub SetCreditCardDetails(ByVal CardNumber As String, ByVal ExpiryMonth As Integer, ByVal ExpiryYear As Integer, ByVal Cvd As Integer) Implements IPaymentProcessClient.SetCreditCardDetails
            ClientCardDetails = New CreditCardDetails(CardNumber, ExpiryMonth, ExpiryYear, Cvd)
        End Sub

        Public Sub SetCreditCardDetails(CardDetails As CreditCardDetails) Implements IPaymentProcessClient.SetCreditCardDetails
            ClientCardDetails = CardDetails
        End Sub

        Public Function GetCreditCardDetails() As CreditCardDetails Implements IPaymentProcessClient.GetCreditCardDetails
            Return ClientCardDetails
        End Function

        Public Sub SetOrderDetails(OrderId As Integer, Amount As Decimal) Implements IPaymentProcessClient.SetOrderDetails
            ClientOrderId = OrderId
            ClientAmount = Amount
        End Sub

    End Class

    Public Class CreditCardDetails
        Private _CreditCardNumber As String
        Private _ExpiryDateMonth As Integer
        Private _ExpiryDateYear As Integer
        Private _Cvd As Integer

        Public Sub New(CreditCardNumber As String, ExpiryDateMonth As Integer, ExpiryDateYear As Integer, Cvd As Integer)
            _CreditCardNumber = CreditCardNumber
            _ExpiryDateMonth = ExpiryDateMonth
            _ExpiryDateYear = ExpiryDateYear
            _CVD = Cvd
        End Sub

        Public Property CreditCardNumber As String
            Get
                Return _CreditCardNumber
            End Get
            Set(Value As String)
                _CreditCardNumber = Value.Replace(" "c, "")
            End Set
        End Property

        Public Property ExpiryDateMonth As Integer
            Get
                Return _ExpiryDateMonth
            End Get
            Set(Value As Integer)
                _ExpiryDateMonth = Value
            End Set
        End Property

        Public Property ExpiryDateYear As Integer
            Get
                Return _ExpiryDateYear
            End Get
            Set(Value As Integer)
                _ExpiryDateYear = Value
            End Set
        End Property

        Public Property Cvd As Integer
            Get
                Return _Cvd
            End Get
            Set(Value As Integer)
                _Cvd = Value
            End Set
        End Property

    End Class

    Public Class PaymentReceipt
        Private _OrderId As Integer
        Private _Amount As Decimal
        Private _AuthorizationCode As String
        Private _TransactionId As String
        Private _TransactionDate As String

        Public Sub New(OrderId As Integer, Amount As Decimal, AuthorizationCode As String, TransactionId As String, TransactionDate As String)
            _OrderId = OrderId
            _Amount = Amount
            _AuthorizationCode = AuthorizationCode
            _TransactionId = TransactionId
            _TransactionDate = TransactionDate
        End Sub

        Public Property OrderId As Integer
            Get
                Return _OrderId
            End Get
            Set(Value As Integer)
                _OrderId = Value
            End Set
        End Property

        Public Property Amount As Decimal
            Get
                Return _Amount
            End Get
            Set(Value As Decimal)
                _Amount = Value
            End Set
        End Property

        Public Property AuthorizationCode As String
            Get
                Return _AuthorizationCode
            End Get
            Set(Value As String)
                _AuthorizationCode = Value
            End Set
        End Property

        Public Property TransactionId As String
            Get
                Return _TransactionId
            End Get
            Set(Value As String)
                _TransactionId = Value
            End Set
        End Property

        Public Property TransactionDate As String
            Get
                Return _TransactionDate
            End Get
            Set(Value As String)
                _TransactionDate = Value
            End Set
        End Property

    End Class

    Public Structure ClientName
        Public First As String
        Public Middle As String
        Public Last As String
    End Structure

End Namespace