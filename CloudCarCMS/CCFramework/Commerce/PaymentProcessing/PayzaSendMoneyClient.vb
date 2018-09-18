Imports System.IO
Imports System.Net

Namespace CCFramework.Commerce.PaymentProcessing

    Public Class PayzaSendMoneyClient

        Private _Response As String
        Public Property Response As String
            Get
                Return _Response
            End Get
            Set(Value As String)
                _Response = Value
            End Set
        End Property

        Private _Server As String
        Public Property Server As String
            Get
                Return _Server
            End Get
            Set(Value As String)
                _Server = Value
            End Set
        End Property

        Private _Url As String
        Public Property Url As String
            Get
                Return _Url
            End Get
            Set(Value As String)
                _Url = Value
            End Set
        End Property

        Private _UserName As String
        Public Property UserName As String
            Get
                Return _UserName
            End Get
            Set(Value As String)
                _UserName = Value
            End Set
        End Property

        Private _Password As String
        Public Property Password As String
            Get
                Return _Password
            End Get
            Set(Value As String)
                _Password = Value
            End Set
        End Property

        Private _Data As String
        Public Property Data As String
            Get
                Return _Data
            End Get
            Set(Value As String)
                _Data = Value
            End Set
        End Property

        Public Sub New(Username As String, Password As String)
            _UserName = Username
            _Password = Password
            _Data = String.Empty
        End Sub

        Public Function BuildPostVariables(AmountPaid As String, Currency As String, ReceiverEmail As String, SenderEmail As String, PurchaseType As String, Note As String, TestMode As String) As String
            Dim DataStringBuilder As New StringBuilder

            DataStringBuilder.AppendFormat("USER={0}&PASSWORD={1}&AMOUNT={2}&CURRENCY={3}&RECEIVEREMAIL={4}&SENDEREMAIL={5}&PURCHASETYPE={6}&NOTE={7}&TESTMODE={8}", _
                                           HttpUtility.UrlEncode(_UserName), HttpUtility.UrlEncode(_Password), HttpUtility.UrlEncode(AmountPaid), _
                                           HttpUtility.UrlEncode(Currency), HttpUtility.UrlEncode(ReceiverEmail), HttpUtility.UrlEncode(SenderEmail), _
                                           HttpUtility.UrlEncode(PurchaseType), HttpUtility.UrlEncode(Note), HttpUtility.UrlEncode(TestMode))

            Return DataStringBuilder.ToString
        End Function

        Public Function ProcessRequest() As String
            Dim CurrentResponseString As String = String.Empty

            Dim CurrentWriter As StreamWriter = Nothing
            Dim CurrentRequest As HttpWebRequest

            Try
                CurrentRequest = CType(WebRequest.Create(_Server + _Url), HttpWebRequest)
                CurrentRequest.Method = "POST"
                CurrentRequest.ContentLength = _Data.Length
                CurrentRequest.ContentType = "application/x-www-form-urlencoded"

                CurrentWriter = New StreamWriter(CurrentRequest.GetRequestStream())
                CurrentWriter.Write(_Data)
                CurrentWriter.Close()

                Dim CurrentResponse = CurrentRequest.GetResponse()
                Dim CurrentStreamReader As New StreamReader(CurrentResponse.GetResponseStream())
                Dim CurrentResult As String = String.Empty
                Dim TempString As String = String.Empty
                Dim Flag As Boolean = True

                While Flag
                    TempString = CurrentStreamReader.ReadLine

                    If Not TempString Is Nothing Then
                        CurrentResult &= TempString
                    Else
                        Flag = False
                    End If
                End While

                CurrentResponseString = HttpUtility.UrlDecode(CurrentResult)
                CurrentStreamReader.Close()
            Catch Ex As Exception

            Finally
                If Not CurrentWriter Is Nothing Then
                    CurrentWriter.Close()
                    CurrentWriter.Dispose()
                End If
            End Try

            Return CurrentResponseString
        End Function

    End Class

End Namespace