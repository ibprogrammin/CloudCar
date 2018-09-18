Namespace CCFramework.Commerce.PaymentProcessing

    Public Class DwollaClient

    End Class

    Public Class DwollaUrls
        Public Shared BaseUrl As String = "https://www.dwolla.com/oauth/rest"

        Public Shared Balances As String = BaseUrl + "/balance"
        Public Shared Contacts As String = BaseUrl + "/contacts"
        Public Shared FundingSources As String = BaseUrl + "/fundingsources"
        Public Shared Register As String = BaseUrl + "/register"
        Public Shared Requests As String = BaseUrl + "/requests"
        Public Shared Transactions As String = BaseUrl + "/transactions"
        Public Shared Users As String = BaseUrl + "/users"
    End Class

End Namespace