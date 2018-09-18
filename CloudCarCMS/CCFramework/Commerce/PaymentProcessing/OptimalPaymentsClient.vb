Imports CloudCar.OptimalPayments.NetBanx
Imports CloudCar.CCFramework.Core

Namespace CCFramework.Commerce.PaymentProcessing

    Public Class OptimalPaymentsClient
        Inherits PaymentProcessClient

        Private Const _MerchantAccountName As String = "cloudcartest"

        Private _ProcessedResponse As CreditCardResponse

        Private ReadOnly _ReferenceNumber As String
        Private ReadOnly _CardType As CardTypeV1
        Private ReadOnly _PostalCode As String

        Public Sub New(ReferenceNumber As String, CardType As CardTypeV1, PostalCode As String)
            _postalCode = PostalCode
            _cardType = CardType
            _referenceNumber = ReferenceNumber
        End Sub

        Public Overrides Function ProcessPayment() As Boolean
            Dim CurrentCreditCardAuthorizationRequest As New CCAuthRequestV1()
            Dim CurrentMerchantAccount As New MerchantAccountV1()

            If Settings.TestMode Then
                'TODO Set up a legitimate test account
                CurrentMerchantAccount.accountNum = "1000023167"
                CurrentMerchantAccount.storeID = "test"
                CurrentMerchantAccount.storePwd = "test"
            Else
                CurrentMerchantAccount.accountNum = Settings.OptimalAccountNumber
                CurrentMerchantAccount.storeID = Settings.OptimalStoreId
                CurrentMerchantAccount.storePwd = Settings.OptimalStorePassword
            End If

            CurrentCreditCardAuthorizationRequest.merchantAccount = CurrentMerchantAccount
            CurrentCreditCardAuthorizationRequest.merchantRefNum = _ReferenceNumber
            CurrentCreditCardAuthorizationRequest.amount = FormatNumber(ClientAmount.ToString, 2)

            Dim CurrentCard As New CardV1
            CurrentCard.cardNum = ClientCardDetails.CreditCardNumber

            Dim CurrentCardExpiry As New CardExpiryV1

            CurrentCardExpiry.month = ClientCardDetails.ExpiryDateMonth
            CurrentCardExpiry.year = ClientCardDetails.ExpiryDateYear

            CurrentCard.cardExpiry = CurrentCardExpiry
            CurrentCard.cardType = _CardType
            CurrentCard.cardTypeSpecified = True

            If CInt(ClientCardDetails.Cvd) <> 0 Then
                CurrentCard.cvdIndicator = 1
                CurrentCard.cvdIndicatorSpecified = True
                CurrentCard.cvd = CStr(ClientCardDetails.Cvd)
            Else
                CurrentCard.cvdIndicatorSpecified = False
            End If

            CurrentCreditCardAuthorizationRequest.card = CurrentCard

            Dim CurrentBillingDetails As New BillingDetailsV1

            CurrentBillingDetails.cardPayMethod = CardPayMethodV1.WEB
            CurrentBillingDetails.cardPayMethodSpecified = True
            CurrentBillingDetails.firstName = ClientName.First
            CurrentBillingDetails.lastName = ClientName.Last
            CurrentBillingDetails.zip = _PostalCode

            CurrentCreditCardAuthorizationRequest.billingDetails = CurrentBillingDetails

            CurrentCreditCardAuthorizationRequest.previousCustomer = False
            CurrentCreditCardAuthorizationRequest.previousCustomerSpecified = True
            CurrentCreditCardAuthorizationRequest.customerIP = HttpContext.Current.Request.UserHostAddress
            CurrentCreditCardAuthorizationRequest.dupeCheck = False
            CurrentCreditCardAuthorizationRequest.dupeCheckSpecified = True

            Dim CurrentCreditCardService As New CreditCardServiceV1()
            Dim CurrentCreditCardTransactionResponse As CCTxnResponseV1 = CurrentCreditCardService.ccPurchase(CurrentCreditCardAuthorizationRequest)

            _ProcessedResponse.ErrorCode = CurrentCreditCardTransactionResponse.code
            _ProcessedResponse.ResponseText = CurrentCreditCardTransactionResponse.code & " - " & CurrentCreditCardTransactionResponse.decision & " - " & CurrentCreditCardTransactionResponse.description
            _ProcessedResponse.ResponseText &= "Details: " + Environment.NewLine

            If Not CurrentCreditCardTransactionResponse.detail Is Nothing Then
                For Each CurrentDetail As DetailV1 In CurrentCreditCardTransactionResponse.detail
                    _ProcessedResponse.ResponseText &= " - " & CurrentDetail.value & Environment.NewLine
                Next
            End If

            _ProcessedResponse.ResponseText = _ProcessedResponse.ResponseText.Replace("\n", Environment.NewLine)

            If DecisionV1.ACCEPTED.Equals(CurrentCreditCardTransactionResponse.decision) Then
                _ProcessedResponse.DecisionText = "Transaction Successful"
            Else
                _ProcessedResponse.DecisionText = "Transaction Failed with decision: " & CurrentCreditCardTransactionResponse.decision
            End If

            _ProcessedResponse.AuthCode = CurrentCreditCardTransactionResponse.authCode

            If _ProcessedResponse.DecisionText = "Transaction Successful" Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function ProcessReturn() As Boolean
            Return False
        End Function

    End Class

    Public Structure CreditCardResponse
        Dim ResponseText As String
        Dim DecisionText As String
        Dim AuthCode As String
        Dim ErrorCode As Integer
    End Structure

End Namespace