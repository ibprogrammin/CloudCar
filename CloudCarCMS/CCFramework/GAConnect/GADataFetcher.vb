Namespace CCFramework.GAConnect

    Public Class GADataFetcher
        Public Email As String
        Public Password As String
        Private _AuthenticationKey As String
        Private Const _AuthenticationUrl As String = "https://www.google.com/accounts/ClientLogin"
        Private Const _AuthenticationParam As String = "Auth="
        Private Const _CloudCarGoogleApiKey As String = "AIzaSyAWbwklKw1i8-4ybi_szde4IMGCENfagsw"

        Public Sub New(Email As String, Password As String)
            Me.Email = Email
            Me.Password = Password

            Try
                Authenticate()
                GABaseData.AuthenticationKey = _AuthenticationKey
            Catch Ex As Exception

            End Try
        End Sub

        Private Sub Authenticate()
            Dim CurrentAuthenticationPost As String = String.Format("accountType=HOSTED_OR_GOOGLE&Email={0}&Passwd={1}&service=analytics&source=xxx-xxx&key={2}", Email, Password, _CloudCarGoogleApiKey)
            _AuthenticationKey = Nothing

            Dim CurrentResult As String = GAHttpRequests.HttpPostRequest(_AuthenticationUrl, CurrentAuthenticationPost)
            'Dim CurrentTokens As String() = CurrentResult.Split(New String() {"\n"}, StringSplitOptions.RemoveEmptyEntries)
            'For Each CurrentToken In CurrentTokens
            'If CurrentToken.StartsWith(_AuthenticationParam) Then
            '_AuthenticationKey = CurrentToken.Remove(_AuthenticationParam) & 
            'End If
            'Next

            _AuthenticationKey = CurrentResult.Substring(CurrentResult.IndexOf(_AuthenticationParam), CurrentResult.Length - CurrentResult.IndexOf(_AuthenticationParam)).Replace(_AuthenticationParam, "")

            'If _AuthenticationKey Is Nothing Then
            '_AuthenticationKey = CurrentResult
            'End If
        End Sub

        Private Shared Function GetProfiles(Of T As {GAProfile, New})(XmlDocument As XDocument) As IEnumerable(Of GAProfile)
            Dim PrefixNamespace As XNamespace = XmlDocument.Root.GetNamespaceOfPrefix("dxp")
            Dim DefaultNamespace As XNamespace = XmlDocument.Root.GetDefaultNamespace()
            Dim CurrentProfiles As IEnumerable(Of GAProfile) = Nothing

            CurrentProfiles = XmlDocument.Root.Descendants(DefaultNamespace.ToString & "entry").Select(Function(f) New GAProfile() With { _
                 .AccountId = f.Elements(PrefixNamespace.ToString & "property").Where(Function(x) x.Attribute("name").Value = "ga:accountId").First.Attribute("value").Value, _
                 .AccountName = f.Elements(PrefixNamespace.ToString & "property").Where(Function(x) x.Attribute("name").Value = "ga:accountName").First.Attribute("value").Value, _
                 .ProfileId = f.Elements(PrefixNamespace.ToString & "property").Where(Function(x) x.Attribute("name").Value = "ga:profileId").First.Attribute("value").Value, _
                 .WebPropertyId = f.Elements(PrefixNamespace.ToString & "property").Where(Function(x) x.Attribute("name").Value = "ga:webPropertyId").First.Attribute("value").Value, _
                 .Currency = f.Elements(PrefixNamespace.ToString & "property").Where(Function(x) x.Attribute("name").Value = "ga:currency").First.Attribute("value").Value, _
                 .TimeZone = f.Elements(PrefixNamespace.ToString & "property").Where(Function(x) x.Attribute("name").Value = "ga:timezone").First.Attribute("value").Value, _
                 .TableId = f.Element(PrefixNamespace.ToString & "tableId").Value, _
                 .Updated = DateTime.Parse(f.Element(DefaultNamespace.ToString & "updated").Value), _
                 .Id = f.Element(DefaultNamespace.ToString & "id").Value, _
                 .Title = f.Element(DefaultNamespace.ToString & "title").Value _
            })
            Return CurrentProfiles
        End Function

        Public Function GetUserProfiles() As IEnumerable(Of GAProfile)
            Dim CurrentXmlData = GABaseData.GetProfilesData(Email, Password)
            Return GetProfiles(Of GAProfile)(CurrentXmlData)
        End Function

        Public Function GetAnalytics(TableId As String, FromDate As DateTime, ToDate As DateTime, MaxRecords As Integer, Dimensions As List(Of GADimension), Metrics As List(Of GAMetric), Sort As GAMetric, Order As GASortDirection) As IEnumerable(Of GAData)
            Dim CurrentData As IEnumerable(Of GABaseData) = GABaseData.GetBaseData(TableId, Dimensions, Metrics, FromDate, ToDate, Sort, Order, MaxRecords)

            Return CurrentData.Select(Function(d) New GAData() With { _
                             .Pageviews = Convert.ToInt32(d.Metrics.FirstOrDefault(Function(met) met.Key = GAMetric.PageViews).Value), _
                             .Bounces = Convert.ToInt32(d.Metrics.FirstOrDefault(Function(met) met.Key = GAMetric.Bounces).Value), _
                             .Entrances = Convert.ToInt32(d.Metrics.FirstOrDefault(Function(met) met.Key = GAMetric.Entrances).Value), _
                             .Exits = Convert.ToInt32(d.Metrics.FirstOrDefault(Function(met) met.Key = GAMetric.Exits).Value), _
                             .NewVisits = Convert.ToInt32(d.Metrics.FirstOrDefault(Function(met) met.Key = GAMetric.NewVisits).Value), _
                             .TimeOnPage = Convert.ToDouble(d.Metrics.FirstOrDefault(Function(met) met.Key = GAMetric.TimeOnPage).Value), _
                             .TimeOnSite = Convert.ToDouble(d.Metrics.FirstOrDefault(Function(met) met.Key = GAMetric.TimeOnSite).Value), _
                             .Visitors = Convert.ToInt32(d.Metrics.FirstOrDefault(Function(met) met.Key = GAMetric.Visits).Value), _
                             .Visits = Convert.ToInt32(d.Metrics.FirstOrDefault(Function(met) met.Key = GAMetric.PageViews).Value), _
                             .UniquePageviews = Convert.ToInt32(d.Metrics.FirstOrDefault(Function(met) met.Key = GAMetric.UniquePageviews).Value), _
                             .ExitPagePath = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.ExitPagePath).Value, _
                             .LandingPagePath = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.LandingPagePath).Value, _
                             .NextPagePath = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.NextPagePath).Value, _
                             .PagePath = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.PagePath).Value, _
                             .PageTitle = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.PageTitle).Value, _
                             .PreviousPagePath = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.PreviousPagePath).Value, _
                             .SecondPagePath = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.SecondPagePath).Value, _
                             .Browser = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.Browser).Value, _
                             .BrowserVersion = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.BrowserVersion).Value, _
                             .City = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.City).Value, _
                             .ConnectionSpeed = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.ConnectionSpeed).Value, _
                             .Country = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.Country).Value, _
                             .[Date] = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.[Date]).Value, _
                             .DaysSinceLastVisit = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.DaysSinceLastVisit).Value, _
                             .Day = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.Day).Value, _
                             .FlashVersion = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.FlashVersion).Value, _
                             .Hostname = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.Hostname).Value, _
                             .IsMobile = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.IsMobile).Value, _
                             .Hour = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.Hour).Value, _
                             .JavaEnabled = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.JavaEnabled).Value, _
                             .Language = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.Language).Value, _
                             .Latitude = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.Latitude).Value, _
                             .Longitude = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.Longitude).Value, _
                             .Month = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.Month).Value, _
                             .NetworkDomain = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.NetworkDomain).Value, _
                             .NetworkLocation = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.NetworkLocation).Value, _
                             .OperatingSystem = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.OperatingSystem).Value, _
                             .OperatingSystemVersion = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.OperatingSystemVersion).Value, _
                             .PageDepth = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.PageDepth).Value, _
                             .Region = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.Region).Value, _
                             .ScreenColors = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.ScreenColors).Value, _
                             .ScreenResolution = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.ScreenResolution).Value, _
                             .SubContinent = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.SubContinent).Value, _
                             .UserDefinedValue = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.UserDefinedValue).Value, _
                             .VisitCount = Convert.ToInt32(d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.VisitCount).Value), _
                             .VisitLength = Convert.ToInt32(d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.VisitLength).Value), _
                             .VisitorType = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.VisitorType).Value, _
                             .Week = Convert.ToInt32(d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.Week).Value), _
                             .Year = Convert.ToInt32(d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.Year).Value), _
                             .Source = d.Dimensions.FirstOrDefault(Function([dim]) [dim].Key = GADimension.Source).Value})
        End Function

    End Class

End Namespace