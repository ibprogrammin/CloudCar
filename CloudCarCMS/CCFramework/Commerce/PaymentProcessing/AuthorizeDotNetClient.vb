Imports System.IO
Imports System.Net

Namespace CCFramework.Commerce.PaymentProcessing

    Public Class AuthorizeDotNetClient

        'Private Const _TestMode As Boolean = True
        Private Const _TestUrl As String = "https://test.authorize.net/gateway/transact.dll"
        Private Const _ReleaseUrl As String = "https://secure.authorize.net/gateway/transact.dll"

        'TODO Login Id and transaction key should be settings
        Private _LoginId As String '"API_LOGIN_ID"
        Private _TransactionKey As String '"TRANSACTION_KEY"

        Private _PostUrl As String

        Private _CreditCardNumber As String
        Private _ExpiryDate As String

        Private _Amount As Decimal
        Private _Description As String

        Private _FirstName As String
        Private _LastName As String
        Private _Address As String
        Private _RegionCode As String
        Private _ZipPostal As String

        Public Sub New(LoginId As String, TransactionKey As String, Optional TestMode As Boolean = True)
            _LoginId = LoginId
            _TransactionKey = TransactionKey

            If TestMode Then
                _PostUrl = _TestUrl
            Else
                _PostUrl = _ReleaseUrl
            End If
        End Sub

        Public Sub SetCreditCardDetails(CreditCardNumber As String, ExpiryDate As String)
            _CreditCardNumber = CreditCardNumber
            _ExpiryDate = ExpiryDate
        End Sub

        Public Sub SetOrderDetails(Amount As Decimal, Description As String)
            _Amount = Amount
            _Description = Description
        End Sub

        Public Sub SetCustomerDetails(FirstName As String, LastName As String, Address As String, RegionCode As String, ZipPostal As String)
            _FirstName = FirstName
            _LastName = LastName
            _Address = Address
            _RegionCode = RegionCode
            _ZipPostal = ZipPostal
        End Sub

        Private Function ValidateDetails() As Boolean
            Return True
        End Function

        Public Function GetPostValues() As Dictionary(Of String, String)
            Dim CurrentPostValues As New Dictionary(Of String, String)

            CurrentPostValues.Add("x_login", _LoginId)
            CurrentPostValues.Add("x_tran_key", _TransactionKey)

            CurrentPostValues.Add("x_delim_data", "TRUE")
            CurrentPostValues.Add("x_delim_char", "|")
            CurrentPostValues.Add("x_relay_response", "FALSE")

            CurrentPostValues.Add("x_type", "AUTH_CAPTURE")
            CurrentPostValues.Add("x_method", "CC")
            CurrentPostValues.Add("x_card_num", _CreditCardNumber)
            CurrentPostValues.Add("x_exp_date", _ExpiryDate)

            CurrentPostValues.Add("x_amount", _Amount.ToString)
            CurrentPostValues.Add("x_description", _Description)

            CurrentPostValues.Add("x_first_name", _FirstName)
            CurrentPostValues.Add("x_last_name", _LastName)
            CurrentPostValues.Add("x_address", _Address)
            CurrentPostValues.Add("x_state", _RegionCode)
            CurrentPostValues.Add("x_zip", _ZipPostal)

            Return CurrentPostValues
        End Function

        Public Function ProcessRequest() As AuthorizeDotNetResponseMessage
            If ValidateDetails() Then
                Dim CurrentPostString As String = ""
                For Each CurrentField As KeyValuePair(Of String, String) In GetPostValues()
                    CurrentPostString &= String.Format("{0}={1}&", CurrentField.Key, HttpUtility.UrlEncode(CurrentField.Value))
                Next
                CurrentPostString = Left(CurrentPostString, Len(CurrentPostString) - 1)

                Dim CurrentRequest As HttpWebRequest = CType(WebRequest.Create(_PostUrl), HttpWebRequest)
                CurrentRequest.Method = "POST"
                CurrentRequest.ContentLength = CurrentPostString.Length
                CurrentRequest.ContentType = "application/x-www-form-urlencoded"

                Dim CurrentStreamWritter As StreamWriter = Nothing
                CurrentStreamWritter = New StreamWriter(CurrentRequest.GetRequestStream())
                CurrentStreamWritter.Write(CurrentPostString)
                CurrentStreamWritter.Close()

                Dim CurrentResponse As HttpWebResponse = CType(CurrentRequest.GetResponse(), HttpWebResponse)
                Dim CurrentStreamReader As New StreamReader(CurrentResponse.GetResponseStream())
                Dim CurrentPostResponse As String = CurrentStreamReader.ReadToEnd()
                CurrentStreamReader.Close()

                Dim CurrentResponseArray As Array = Split(CurrentPostResponse, GetPostValues("x_delim_char"), -1)

                Dim CurrentAuthorizeDotNetResponseMessage As New AuthorizeDotNetResponseMessage
                CurrentAuthorizeDotNetResponseMessage.ResponseCode = CInt(CurrentResponseArray(0))
                CurrentAuthorizeDotNetResponseMessage.ResultDescription = CStr(CurrentResponseArray(3))
                CurrentAuthorizeDotNetResponseMessage.AuthorizationCode = CStr(CurrentResponseArray(4))
                CurrentAuthorizeDotNetResponseMessage.AvsResponse = CStr(CurrentResponseArray(5))
                CurrentAuthorizeDotNetResponseMessage.TransactionId = CStr(CurrentResponseArray(6))
                CurrentAuthorizeDotNetResponseMessage.Amount = CDec(CurrentResponseArray(9))
                CurrentAuthorizeDotNetResponseMessage.Method = CStr(CurrentResponseArray(10))

                Dim CurrentResponseMessage As String
                CurrentResponseMessage = "" & vbCrLf
                For Each Value In CurrentResponseArray
                    CurrentResponseMessage &= String.Format("{0}&nbsp;" & vbCrLf, Value.ToString)
                Next
                CurrentResponseMessage &= "" & vbCrLf

                Return CurrentAuthorizeDotNetResponseMessage
            Else
                Return Nothing ' "Error: The data you are trying to send is invalid"
            End If
        End Function

        Public Shared Function CheckResponseSuccess(ResponseMessage As AuthorizeDotNetResponseMessage) As Boolean
            If ResponseMessage.ResponseCode = 1 Then
                Return True
            Else
                Return False
            End If
        End Function

    End Class

    Public Class AuthorizeDotNetResponseMessage
        Public ResponseCode As Integer
        Public ResultDescription As String
        Public AuthorizationCode As String
        Public AvsResponse As String
        Public TransactionId As String
        Public Amount As Decimal
        Public Method As String
    End Class

End Namespace