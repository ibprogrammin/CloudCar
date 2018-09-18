Imports CloudCar.CCFramework.Core

Namespace CCFramework.Commerce.Shipping.Fixed

    Public Class FixedShippingProxy
        Inherits ShippingServiceProxy

        Private ReadOnly _TotalWeight As Decimal

        Public Sub New(ByVal TotalWeight As Decimal)
            MyBase.New("", "")

            _TotalWeight = TotalWeight
        End Sub

        Protected Overrides Sub BuildRequestXml()

        End Sub

        Protected Overrides Sub SubmitRequest()
        End Sub

        Protected Overrides Sub ReadResponseToDataSet()
        End Sub

        Protected Overrides Sub LoadShippingRates()
            If _ShipToAddress.CountryCode.ToUpper = "CA" OrElse _ShipToAddress.CountryCode.ToUpper = "US" Then
                Dim Prefix As String

                If _ShipToAddress.CountryCode.ToUpper = "CA" Then
                    Prefix = _ShipToAddress.PostalCode.Substring(0, 3)
                Else
                    Prefix = _ShipToAddress.PostalCode
                End If

                Dim Zone As Integer

                Try
                    Zone = FixedShippingZoneController.GetShippingZone(Prefix)
                Catch Exception As FixedShippingZoneController.InvalidShippingZoneException
                    Zone = Settings.DefaultShippingZone
                End Try

                Dim MaxWeight As Decimal = FixedShippingRateController.GetMaxShippingWeightKg()
                Dim Rate As Decimal = 0

                If _TotalWeight > MaxWeight Then
                    Dim Multiplier As Integer = CInt(Math.Ceiling(_TotalWeight / MaxWeight))
                    Dim Remainder As Decimal = _TotalWeight - (MaxWeight * Multiplier)

                    Rate = (FixedShippingRateController.GetRateFromZoneKg(MaxWeight, Zone).Cost * Multiplier) + FixedShippingRateController.GetRateFromZoneKg(Remainder, Zone).Cost
                Else
                    Rate = FixedShippingRateController.GetRateFromZoneKg(_TotalWeight, Zone).Cost
                End If

                Rate = CInt(Rate * Settings.ShippingMargin)

                If Settings.UseMaxShippingCharge Then
                    If Rate > Settings.MaxShippingCharge Then
                        Rate = Settings.MaxShippingCharge
                    End If
                End If

                Dim CurrentRate As New ShippingRate(1, "Ground Shipping", Settings.CompanyName, "FixedShipping", Rate, Date.Now.AddDays(_DaysToShip).ToString, Date.Now.AddDays(_DaysToShip + 4).ToString)

                AddShippingRate(CurrentRate)

                _StatusMessage = "SUCCESS"
            End If
        End Sub

        Protected Overrides Function CheckIfOkToProcess() As Boolean
            If Not _ShipToAddress Is Nothing AndAlso Not _TotalWeight = Nothing Then
                _StatusMessage = "SUCCESS"

                Return True
            Else
                _StatusMessage = "ERROR PROCCESSING"

                Return False
            End If
        End Function

    End Class

End Namespace