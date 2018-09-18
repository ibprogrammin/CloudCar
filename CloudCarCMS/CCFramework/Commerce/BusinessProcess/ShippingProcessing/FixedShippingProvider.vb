Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Core.Shipping

    Public Class FixedShippingProvider
        Inherits ShippingProviderClass
        Implements IShippingProvider

        Public Sub RequestRateOptions(ByVal UserId As Integer, ByVal AddressId As Integer) Implements IShippingProvider.RequestRateOptions
            Dim Weight As Decimal = Shipping.ShippingHelper.GetCartWeight(UserId)

            Dim address As Address = New AddressController().GetElement(AddressId)

            If address.Province.Country.Code = "CA" Or address.Province.Country.Code = "US" Then
                Dim Prefix As String

                If address.Province.Country.Code = "CA" Then
                    Prefix = address.PCZIP.Substring(0, 3)
                Else
                    Prefix = address.PCZIP
                End If

                Dim Zone As Integer

                Try
                    Zone = FixedShippingZoneController.GetShippingZone(Prefix)
                Catch ex As FixedShippingZoneController.InvalidShippingZoneException
                    Zone = Global.CloudCar.CCFramework.Core.Settings.DefaultShippingZone
                End Try

                Dim MaxWeight As Decimal = FixedShippingRateController.GetMaxShippingWeightKG()
                Dim Rate As Decimal = 0

                If Weight > MaxWeight Then
                    Dim multiplier As Integer = CInt(Math.Floor(Weight / MaxWeight))
                    Dim remainder As Decimal = CDec(Weight - (MaxWeight * multiplier))

                    Rate = (FixedShippingRateController.GetRateFromZoneKG(MaxWeight, Zone).Cost * multiplier) + FixedShippingRateController.GetRateFromZoneKG(remainder, Zone).Cost

                Else
                    Rate = FixedShippingRateController.GetRateFromZoneKG(Weight, Zone).Cost
                End If

                Rate = CDec(Rate * Global.CloudCar.CCFramework.Core.Settings.ShippingMargin)

                If Global.CloudCar.CCFramework.Core.Settings.UseMaxShippingCharge Then
                    If Rate > Global.CloudCar.CCFramework.Core.Settings.MaxShippingCharge Then
                        Rate = Global.CloudCar.CCFramework.Core.Settings.MaxShippingCharge
                    End If
                End If

                Dim ProviderRates As New List(Of Shipping.ShippingRate)
                ProviderRates.Add(New Shipping.ShippingRate("Ground Shipping", "Fixed Shipping", "FixedShipping", "Fixed Shipping", Rate))

                AddRates(ProviderRates)
            End If
        End Sub

        Public Sub RequestRateOptions(ByVal SessionId As String, ByVal AddressId As Integer) Implements IShippingProvider.RequestRateOptions
            Dim Weight As Decimal = ShippingHelper.GetCartWeight(SessionId)

            Dim address As Address = New AddressController().GetElement(AddressId)

            If address.Province.Country.Code = "CA" Or address.Province.Country.Code = "US" Then
                Dim Prefix As String

                If address.Province.Country.Code = "CA" Then
                    Prefix = address.PCZIP.Substring(0, 3)
                Else
                    Prefix = address.PCZIP
                End If

                Dim Zone As Integer

                Try
                    Zone = FixedShippingZoneController.GetShippingZone(Prefix)
                Catch ex As FixedShippingZoneController.InvalidShippingZoneException
                    Zone = Settings.DefaultShippingZone
                End Try

                Dim MaxWeight As Decimal = FixedShippingRateController.GetMaxShippingWeightKG()
                Dim Rate As Decimal = 0

                If Weight > MaxWeight Then
                    Dim multiplier As Integer = CInt(Math.Ceiling(Weight / MaxWeight))
                    Dim remainder As Decimal = Weight - (MaxWeight * multiplier)

                    Rate = (FixedShippingRateController.GetRateFromZoneKG(MaxWeight, Zone).Cost * multiplier) + FixedShippingRateController.GetRateFromZoneKG(remainder, Zone).Cost
                Else
                    Rate = FixedShippingRateController.GetRateFromZoneKG(Weight, Zone).Cost
                End If

                Rate = CDec(Rate * Settings.ShippingMargin)

                If Settings.UseMaxShippingCharge Then
                    If Rate > Settings.MaxShippingCharge Then
                        Rate = Settings.MaxShippingCharge
                    End If
                End If

                Dim ProviderRates As New List(Of ShippingRate)
                ProviderRates.Add(New ShippingRate("Ground Shipping", "Fixed Shipping", "FixedShipping", "Fixed Shipping", Rate))

                AddRates(ProviderRates)
            End If
        End Sub

        Private Sub AddRates(ByVal ProviderRates As Object) Implements IShippingProvider.AddRates
            For Each Rate As ShippingRate In CType(ProviderRates, List(Of ShippingRate))

                Rate.Rate = CDec(Rate.Rate * Settings.ShippingMargin)

                If Settings.UseMaxShippingCharge Then
                    If Rate.Rate > Settings.MaxShippingCharge Then
                        Rate.Rate = Settings.MaxShippingCharge
                    End If
                End If

                mRates.Add(New ShippingRate(Rate.Label, Rate.Company, Rate.Service, Rate.Decsription, Rate.Rate))
            Next
        End Sub

    End Class

End Namespace