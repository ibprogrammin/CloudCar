Imports Braintree
Imports CloudCar.CCFramework.Core

Namespace CCFramework.Commerce.PaymentProcessing

    Public Class BraintreeClient
        Private _Gateway As BraintreeGateway
        Private _Request As TransactionRequest
        Private _Result As Result(Of Transaction)

        Public Sub SetGateway(MerchantId As String, PublicKey As String, PrivateKey As String)
            Dim CurrentEnvironment As Environment
            If Settings.TestMode Then
                CurrentEnvironment = Environment.SANDBOX
            Else
                CurrentEnvironment = Environment.PRODUCTION
            End If

            _Gateway = New BraintreeGateway With {
                .Environment = CurrentEnvironment,
                .MerchantId = MerchantId,
                .PublicKey = PublicKey,
                .PrivateKey = PrivateKey}
        End Sub

        Public Sub SetRequest(Amount As Decimal, CreditCardNumber As String, ExpiryMonth As String, ExpiryYear As String)
            _Request = New TransactionRequest With {
                .Amount = Amount,
                .CreditCard = New TransactionCreditCardRequest With {
                    .Number = CreditCardNumber,
                    .ExpirationMonth = ExpiryMonth,
                    .ExpirationYear = ExpiryYear
                    }
                }
        End Sub

        Public Function ProcessRequest() As String
            Dim CurrentRequestMessage As String = String.Empty
            Dim CurrentTransaction As Transaction

            _Result = _Gateway.Transaction.Sale(_Request)

            If _Result.IsSuccess() Then
                CurrentTransaction = _Result.Target
                CurrentRequestMessage = String.Format("Success!: {0}", CurrentTransaction.Id)
            ElseIf Not _Result.Transaction Is Nothing Then
                CurrentRequestMessage &= String.Format("Message: {0}", _Result.Message)

                CurrentTransaction = _Result.Transaction

                CurrentRequestMessage &= "Error processing transaction:"
                CurrentRequestMessage &= String.Format("  Status: {0}", CurrentTransaction.Status)
                CurrentRequestMessage &= String.Format("  Code: {0}", CurrentTransaction.ProcessorResponseCode)
                CurrentRequestMessage &= String.Format("  Text: {0}", CurrentTransaction.ProcessorResponseText)
            Else
                CurrentRequestMessage &= String.Format("Message: {0}", _Result.Message)

                For Each CurrentError As ValidationError In _Result.Errors.DeepAll()
                    CurrentRequestMessage &= String.Format("  Attribute: {0}", CurrentError.Attribute)
                    CurrentRequestMessage &= String.Format("  Code: {0}", CurrentError.Code)
                    CurrentRequestMessage &= String.Format("  Message: {0}", CurrentError.Message)
                Next
            End If

            Return CurrentRequestMessage
        End Function

    End Class

End Namespace