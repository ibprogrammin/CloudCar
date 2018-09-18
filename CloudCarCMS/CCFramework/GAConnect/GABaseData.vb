Namespace CCFramework.GAConnect

    Public Class GABaseData
        Public Dimensions As IEnumerable(Of KeyValuePair(Of GADimension, String))
        Public Metrics As IEnumerable(Of KeyValuePair(Of GAMetric, String))

        Private Const _FeedUrl As String = "https://www.google.com/analytics/feeds/accounts/default"
        Private Const _PageViewReportUrl As String = "https://www.google.com/analytics/feeds/data?ids={0}&dimensions={1}&metrics={2}&start-date={3}&end-date={4}&sort={5}&max-results={6}"
        Private Const _CloudCarGoogleApiKey As String = "AIzaSyAWbwklKw1i8-4ybi_szde4IMGCENfagsw"

        Private Shared _authenticationKey As String
        Friend Shared Property AuthenticationKey() As String
            Get
                Return _authenticationKey
            End Get
            Set(Value As String)
                _authenticationKey = Value
            End Set
        End Property

        Private Shared Function GetXmlData(TableId As String, _
                                           Dimensions As IEnumerable(Of GADimension), _
                                           Metrics As IEnumerable(Of GAMetric), _
                                           FromDate As DateTime, _
                                           ToDate As DateTime, _
                                           Sort As GAMetric, _
                                           Direction As GASortDirection, _
                                           MaxRecords As Integer) As XDocument
            Dim CurrentDocument As XDocument = Nothing

            If AuthenticationKey.Length > 0 Then
                Dim DimensionStringBuilder As New StringBuilder()

                For i As Integer = 0 To Dimensions.Count() - 1
                    DimensionStringBuilder.AppendFormat("ga:{0}", Dimensions.ElementAt(i))
                    If i < Dimensions.Count() - 1 Then
                        DimensionStringBuilder.Append(",")
                    End If
                Next

                Dim MetricStringBuilder As New StringBuilder()
                For i As Integer = 0 To Metrics.Count() - 1
                    MetricStringBuilder.AppendFormat("ga:{0}", Metrics.ElementAt(i))
                    If i < Metrics.Count() - 1 Then
                        MetricStringBuilder.Append(",")
                    End If
                Next

                Dim CurrentSorter As String = "ga:" & Convert.ToString(Sort)
                If Direction = SortDirection.Descending Then
                    CurrentSorter = "-" & CurrentSorter
                End If

                'https://www.google.com/analytics/feeds/data?ids=ga:24382466&dimensions=ga:date&metrics=ga:visits&start-date=2013-08-25&end-date=2013-09-25&sort=desc&max-results=30&key=AIzaSyAWbwklKw1i8-4ybi_szde4IMGCENfagsw

                Dim CurrentHeader As String() = New String() {"Authorization: GoogleLogin auth=" & _authenticationKey}

                Dim CurrentUrl = String.Format(_PageViewReportUrl, "ga:" & TableId, DimensionStringBuilder.ToString, MetricStringBuilder.ToString, FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd"), CurrentSorter, MaxRecords)

                CurrentDocument = XDocument.Parse(GAHttpRequests.HttpGetRequest(CurrentUrl, CurrentHeader))
            End If

            Return CurrentDocument
        End Function

        Public Shared Function GetBaseData(TableId As String, _
                                           Dimensions As IEnumerable(Of GADimension), _
                                           Metrics As IEnumerable(Of GAMetric), _
                                           FromDate As DateTime, _
                                           ToDate As DateTime, _
                                           Sort As GAMetric, _
                                           Direction As GASortDirection, _
                                           MaxRecords As Integer) As IEnumerable(Of GABaseData)
            Dim CurrentData As IEnumerable(Of GABaseData) = Nothing
            Dim CurrentXml As XDocument = GetXmlData(TableId, Dimensions, Metrics, FromDate, ToDate, Sort, Direction, MaxRecords)

            'TODO If any errors occur during implementation of GAConnect they will probably originate here
            If Not CurrentXml Is Nothing Then
                Dim PrefixNamespace As XNamespace = CurrentXml.Root.GetNamespaceOfPrefix("dxp")
                Dim DefaultNamespace As XNamespace = CurrentXml.Root.GetDefaultNamespace()

                CurrentData = CurrentXml.Root.Descendants(DefaultNamespace.ToString & "entry").Select(Function(CurrentElement) New GABaseData() With { _
                        .Dimensions = New List(Of KeyValuePair(Of GADimension, String))(CurrentElement.Elements(PrefixNamespace.ToString & "dimension").Select(Function(DimensionElement) New KeyValuePair(Of GADimension, String)(DimensionElement.Attribute("name").Value.Replace("ga:", "").ParseEnum(Of GADimension)(), DimensionElement.Attribute("value").Value))),
                        .Metrics = New List(Of KeyValuePair(Of GAMetric, String))(From MetricElement In CurrentElement.Elements(PrefixNamespace.ToString & "metric") Select New KeyValuePair(Of GAMetric, String)(MetricElement.Attribute("name").Value.Replace("ga:", "").ParseEnum(Of GAMetric)(), MetricElement.Attribute("value").Value)) _
                    })
            End If

            Return CurrentData
        End Function

        Public Shared Function GetProfilesData(Email As String, Password As String) As XDocument
            Dim CurrentDocumentAnalyticsData As XDocument = Nothing

            If _authenticationKey.Length > 0 Then
                Dim CurrentHeader As String() = New String() {"Authorization: GoogleLogin " & _authenticationKey}
                CurrentDocumentAnalyticsData = XDocument.Parse(GAHttpRequests.HttpGetRequest(_FeedUrl, CurrentHeader))
            End If

            Return CurrentDocumentAnalyticsData
        End Function

    End Class

End Namespace






'{"web":{"auth_uri":"https://accounts.google.com/o/oauth2/auth","client_secret":"P5gv51AQxT3lBRzj7j76DTWS","token_uri":"https://accounts.google.com/o/oauth2/token","client_email":"48307761651@developer.gserviceaccount.com","redirect_uris":["https://localhost/oauth2callback"],"client_x509_cert_url":"https://www.googleapis.com/robot/v1/metadata/x509/48307761651@developer.gserviceaccount.com","client_id":"48307761651.apps.googleusercontent.com","auth_provider_x509_cert_url":"https://www.googleapis.com/oauth2/v1/certs","javascript_origins":["https://localhost"]}}