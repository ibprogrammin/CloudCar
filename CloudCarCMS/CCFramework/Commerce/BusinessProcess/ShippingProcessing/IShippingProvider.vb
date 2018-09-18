Namespace CCFramework.Core.Shipping

    Public Interface IShippingProvider
        Sub RequestRateOptions(ByVal UserId As Integer, ByVal AddressId As Integer)
        Sub RequestRateOptions(ByVal SessionId As String, ByVal AddressId As Integer)
        Sub AddRates(ByVal ProviderRates As Object)
    End Interface

    Public MustInherit Class ShippingProviderClass
        Protected mRates As List(Of ShippingRate)

        Public Sub New()
            mRates = New List(Of ShippingRate)
        End Sub

        Public ReadOnly Property Rates() As List(Of ShippingRate)
            Get
                Return mRates
            End Get
        End Property

        Public Function GetRates() As List(Of ShippingRate)
            Return mRates
        End Function

    End Class

End Namespace