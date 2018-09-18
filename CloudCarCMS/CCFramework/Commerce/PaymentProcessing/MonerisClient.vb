Imports CloudCar.CCFramework.Core
Imports Moneris

Namespace CCFramework.Commerce.PaymentProcessing

    Public Class MonerisClient
        Private Const _Host As String = "esqa.moneris.com"
        Private Const _Crypt As String = "7" 'SSL Enabled Merchant

        Private ReadOnly _StoreId As String
        Private ReadOnly _ApiToken As String

        Private _AvsCheckInformation As AvsInfo
        Private _CvdCheckInformation As CvdInfo

        Private _OrderId As String      'up to 50, only 3 to 10 characters will be displayed in Merchant Direct
        Private _Amount As String       'must be in the format 0.00, Penny amounts must be displayed

        Private _Pan As String        'Min 13 Digits Max 20, no spaces or special characters
        Private _ExpiryDate As String 'Format YYMM

        Private _Receipt As Receipt

        Public Function GetReceipt() As Receipt
            Return _Receipt
        End Function

        Public Sub New(StoreId As String, ApiToken As String)
            If Settings.TestMode = True Then
                _StoreId = "store5"
                _ApiToken = "yesguy"
            Else
                _StoreId = StoreId
                _ApiToken = ApiToken
            End If
        End Sub

        Public Sub SetCvdInformation(CvdNumber As String)
            _CvdCheckInformation = New CvdInfo
            _CvdCheckInformation.SetCvdIndicator("1")
            _CvdCheckInformation.SetCvdValue(CvdNumber)
        End Sub

        Public Sub SetAvsInformation(StreetNumber As String, StreetName As String, ZipPostalCode As String)
            _AvsCheckInformation = New AvsInfo
            _AvsCheckInformation.SetAvsStreetNumber(StreetNumber)  'Street number and street name cannot be more then 19 chars combined
            _AvsCheckInformation.SetAvsStreetName(StreetName)
            _AvsCheckInformation.SetAvsZipCode(ZipPostalCode)
        End Sub

        Public Sub SetOrderDetails(OrderId As String, Amount As String)
            _OrderId = OrderId
            _Amount = Amount
        End Sub

        Public Sub SetCreditCardDetails(CreditCardNumber As String, ExpiryDate As String)
            _Pan = CreditCardNumber
            _ExpiryDate = ExpiryDate
        End Sub

        Public Function ProcessRequest() As Boolean
            Dim CurrentPurchase As New Purchase(_OrderId, _Amount, _Pan, _ExpiryDate, _Crypt)

            If Not _AvsCheckInformation Is Nothing Then
                CurrentPurchase.SetAvsInfo(_AvsCheckInformation)
            End If

            If Not _CvdCheckInformation Is Nothing Then
                CurrentPurchase.SetCvdInfo(_CvdCheckInformation)
            End If

            'Appended to your business name on the cardholders account statement
            'purchase.SetDynamicDescriptor("2134565");

            Dim CurrentRequest As New HttpsPostRequest(_Host, _StoreId, _ApiToken, CurrentPurchase)
            
            Try
                _Receipt = CurrentRequest.GetReceipt()

                'Console.WriteLine("CardType = " + CurrentReceipt.GetCardType())
                'Console.WriteLine("TransAmount = " + CurrentReceipt.GetTransAmount())
                'Console.WriteLine("TxnNumber = " + CurrentReceipt.GetTxnNumber())
                'Console.WriteLine("ReceiptId = " + CurrentReceipt.GetReceiptId())
                'Console.WriteLine("TransType = " + CurrentReceipt.GetTransType())
                'Console.WriteLine("ReferenceNum = " + CurrentReceipt.GetReferenceNum())
                'Console.WriteLine("ResponseCode = " + CurrentReceipt.GetResponseCode())
                'Console.WriteLine("ISO = " + CurrentReceipt.GetISO())
                'Console.WriteLine("BankTotals = " + CurrentReceipt.GetBankTotals())
                'Console.WriteLine("Message = " + CurrentReceipt.GetMessage())
                'Console.WriteLine("AuthCode = " + CurrentReceipt.GetAuthCode())
                'Console.WriteLine("Complete = " + CurrentReceipt.GetComplete())
                'Console.WriteLine("TransDate = " + CurrentReceipt.GetTransDate())
                'Console.WriteLine("TransTime = " + CurrentReceipt.GetTransTime())
                'Console.WriteLine("Ticket = " + CurrentReceipt.GetTicket())
                'Console.WriteLine("TimedOut = " + CurrentReceipt.GetTimedOut())
                'Console.WriteLine("IsVisaDebit = " + CurrentReceipt.GetIsVisaDebit())
            Catch Ex As Exception
                'Console.WriteLine(Ex)

                Return False
            End Try

            Return TransactionApproved()
        End Function

        Private Function TransactionApproved() As Boolean
            If CInt(_Receipt.GetResponseCode) < 50 Then
                Return True
            Else
                Return False
            End If
        End Function

    End Class

End Namespace