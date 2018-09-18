Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates

Imports DotNetOpenAuth.OAuth2

Imports Google.Apis.Util
Imports Google.Apis.Services
Imports Google.Apis.Analytics.v3
Imports Google.Apis.Analytics.v3.Data
Imports Google.Apis.Authentication.OAuth2
Imports Google.Apis.Authentication.OAuth2.DotNetOpenAuth


Namespace CCFramework.Core

    Public Class AnalyticsAccessor
        Private _ProfileId As String
        Private _Service As AnalyticsService
        Private _RequestData As GaData

        Private Const _ScopeUrl As String = "https://www.googleapis.com/auth/analytics.readonly"

        Public Sub New(ProfileId As String, ServiceAccountId As String, PrivateKeyPath As String, CertificatePassword As String)
            Dim CurrentCertificate As X509Certificate2 = New X509Certificate2(PrivateKeyPath, CertificatePassword, X509KeyStorageFlags.Exportable Or X509KeyStorageFlags.MachineKeySet)

            'Dim CurrentClient As New AssertionFlowClient(GoogleAuthenticationServer.Description, CurrentCertificate) With {.Scope = _ScopeUrl, .ServiceAccountId = ServiceAccountId}

            'Dim CurrentAuthenticator As OAuth2Authenticator(Of AssertionFlowClient) = New OAuth2Authenticator(Of AssertionFlowClient)(CurrentClient, AddressOf AssertionFlowClient.GetState)

            '_Service = New AnalyticsService(CurrentAuthenticator)
            _ProfileId = ProfileId
        End Sub

        Public Function GetData(StartDate As String, EndDate As String, Metrics As String, Dimensions As String, Optional Filters As String = Nothing) As GAData
            Dim CurrentRequest As DataResource.GaResource.GetRequest = _Service.Data.Ga.Get(_ProfileId, StartDate, EndDate, Metrics)

            CurrentRequest.Dimensions = Dimensions

            If filters IsNot Nothing Then
                CurrentRequest.Filters = Filters
            End If

            _RequestData = CurrentRequest.Execute()

            Return _RequestData
        End Function

        Public Function GetJsChartData() As String
            If Not _RequestData Is Nothing Then
                Dim CurrentData As String = "[["

                '_RequestData.ColumnHeaders("Date").SetOrdinal(0)

                For CurrentColumnIndex As Integer = 0 To _RequestData.ColumnHeaders.Count - 1
                    If CurrentColumnIndex = _RequestData.ColumnHeaders.Count - 1 Then
                        CurrentData &= String.Format("'{0}'],", _RequestData.ColumnHeaders(CurrentColumnIndex).Name)
                    Else
                        CurrentData &= String.Format("'{0}',", _RequestData.ColumnHeaders(CurrentColumnIndex).Name)
                    End If
                Next

                For CurrentRowIndex As Integer = 0 To _RequestData.Rows.Count - 1
                    CurrentData &= "["

                    For CurrentColumnIndex As Integer = 0 To _RequestData.ColumnHeaders.Count - 1
                        If _RequestData.ColumnHeaders(CurrentColumnIndex).Name = "Date" Then
                            CurrentData &= String.Format("'{0:MMM dd yyyy}'", ParseAnalyticsDate(_RequestData.Rows(CurrentRowIndex)(CurrentColumnIndex).ToString()))
                        Else
                            CurrentData &= String.Format("{0}", _RequestData.Rows(CurrentRowIndex)(CurrentColumnIndex).ToString())
                        End If

                        If CurrentColumnIndex = _RequestData.ColumnHeaders.Count - 1 Then
                            CurrentData &= "]"
                        Else
                            CurrentData &= ","
                        End If
                    Next

                    If CurrentRowIndex = _RequestData.Rows.Count - 1 Then
                        CurrentData &= "]"
                    Else
                        CurrentData &= ","
                    End If
                Next

                Return CurrentData
            Else
                Return "[]"
            End If
        End Function

        Public Shared Function ParseAnalyticsDate(ParseDate As String) As DateTime
            Dim CurrentDate As DateTime

            CurrentDate = New DateTime(Integer.Parse(ParseDate.Substring(0, 4)), Integer.Parse(ParseDate.Substring(4, 2)), Integer.Parse(ParseDate.Substring(6, 2)))

            Return CurrentDate
        End Function

    End Class

End Namespace