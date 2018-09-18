Namespace CCFramework.Core.Shipping

    Public Class ShippingRate
        Private mRateId As Guid
        Private mLabel As String
        Private mCompany As String
        Private mService As String
        Private mDescription As String
        Private mRate As Decimal

        Public Sub New(ByVal Label As String, ByVal Company As String, ByVal Service As String, ByVal Description As String, ByVal Rate As Decimal)
            mRateId = Guid.NewGuid
            mLabel = Label
            mCompany = Company
            mService = Service
            mDescription = Description
            mRate = Rate
        End Sub

        Public ReadOnly Property RateId() As Guid
            Get
                Return mRateId
            End Get
        End Property

        Public Property Label() As String
            Get
                Return mLabel
            End Get
            Set(ByVal value As String)
                mLabel = value
            End Set
        End Property

        Public Property Company() As String
            Get
                Return mCompany
            End Get
            Set(ByVal value As String)
                mCompany = value
            End Set
        End Property

        Public Property Service() As String
            Get
                Return mService
            End Get
            Set(ByVal value As String)
                mService = value
            End Set
        End Property

        Public Property Decsription() As String
            Get
                Return mDescription
            End Get
            Set(ByVal value As String)
                mDescription = value
            End Set
        End Property

        Public Property Rate() As Decimal
            Get
                Return mRate
            End Get
            Set(ByVal value As Decimal)
                mRate = value
            End Set
        End Property

    End Class

    Public Class ShippingRateCollection
        Private mRates As List(Of ShippingRate)

        Public Sub New()
            mRates = New List(Of ShippingRate)
        End Sub

        Public Sub RequestAvailableShippingRates(ByVal SessionId As String, ByVal AddressId As Integer)
            If Settings.UseCPRates Then
                Dim CanadaPostProvider As New CanadaPostProvider()
                CanadaPostProvider.RequestRateOptions(SessionId, AddressId)
                AppendRates(CanadaPostProvider)
            End If

            If Settings.UseUPSRates Then
                Dim UnitedPostalServiceProvider As New UnitedPostalServiceProvider()
                UnitedPostalServiceProvider.RequestRateOptions(SessionId, AddressId)
                AppendRates(UnitedPostalServiceProvider)
            End If

            If Settings.UsePickUpRate Then
                mRates.Add(New ShippingRate("Local Pick Up", Settings.CompanyName, "Local Pick Up", "", 0))
            End If

            If Settings.UseFixedRate Then
                Dim FixedShippingProvider As New FixedShippingProvider
                FixedShippingProvider.RequestRateOptions(SessionId, AddressId)
                AppendRates(FixedShippingProvider)
            End If

            SortRatesByPrice()
        End Sub

        Public Sub RequestAvailableShippingRates(ByVal UserId As Integer, ByVal AddressId As Integer)
            If Settings.UseCPRates Then
                Dim CanadaPostProvider As New CanadaPostProvider()
                CanadaPostProvider.RequestRateOptions(UserId, AddressId)
                AppendRates(CanadaPostProvider)
            End If

            If Settings.UseUPSRates Then
                Dim UnitedPostalServiceProvider As New UnitedPostalServiceProvider()
                UnitedPostalServiceProvider.RequestRateOptions(UserId, AddressId)
                AppendRates(UnitedPostalServiceProvider)
            End If

            If Settings.UsePickUpRate Then
                mRates.Add(New ShippingRate("Local Pick Up", Settings.CompanyName, "Local Pick Up", "", 0))
            End If

            If Settings.UseFixedRate Then
                Dim FixedShippingProvider As New FixedShippingProvider
                FixedShippingProvider.RequestRateOptions(UserId, AddressId)
                AppendRates(FixedShippingProvider)
            End If

            SortRatesByPrice()
        End Sub

        Private Sub AppendRates(ByVal Provider As Shipping.ShippingProviderClass)
            For Each item As ShippingRate In Provider.Rates
                mRates.Add(item)
            Next
            'mRates.Union(Provider.GetRates)
        End Sub

        Public ReadOnly Property Rates() As List(Of Shipping.ShippingRate)
            Get
                Return mRates
            End Get
        End Property

        Public Function GetRatesAsListItems() As List(Of ListItem)
            Dim RatesList As New List(Of ListItem)

            For Each CurrentRate As ShippingRate In mRates
                RatesList.Add(New ListItem(CurrentRate.Label, CurrentRate.RateId.ToString))
            Next

            Return RatesList
        End Function

        Private Sub SortRatesByPrice(Optional ByVal Descending As Boolean = False)
            If Descending Then
                mRates.OrderByDescending(Function(f) f.Rate)
            Else
                mRates.OrderBy(Function(f) f.Rate)
            End If
        End Sub

        Public Function GetRateById(ByVal Guid As String) As Shipping.ShippingRate
            Dim SelectedRate As Shipping.ShippingRate

            SelectedRate = (From r In mRates Where r.RateId.ToString Like Guid Select r).FirstOrDefault

            Return SelectedRate
        End Function

    End Class

End Namespace