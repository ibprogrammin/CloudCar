Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.ShoppingCart

Namespace CCFramework.Core.Shipping

    Public Class UnitedPostalServiceProvider
        Inherits ShippingProviderClass
        Implements IShippingProvider

        Public Sub RequestRateOptions(ByVal UserId As Integer, ByVal AddressId As Integer) Implements IShippingProvider.RequestRateOptions
            Dim UPS As New Services.Shipping.UPSServices.UPSService
            Dim cookies As New System.Net.CookieContainer

            UPS.CookieContainer = cookies

            UPS.SetCustomerClassification(Services.Shipping.UPSServices.CClassification.Retail)
            UPS.RequestService("-1")

            UPS.SetAccessInfo(Settings.UPSLicenseNumber, Settings.UPSLicenseUser, Settings.UPSLicensePassword)
            UPS = GetToUPSAddress(UPS, AddressId)
            UPS.SetFromAddress(Settings.ShipFromCity, Settings.ShipFromProvince, Settings.ShipFromPC, Settings.ShipFromCountry, Settings.ShipFromAddress, "", "", CLng(Settings.ShipFromPN), Settings.ShipFromCompany, Settings.ShipFromAttention)

            Dim sc As New ShoppingCartController

            Dim Weight As Decimal = 0
            Dim Quantity As Integer = 0

            For Each item As ShoppingCart In sc.GetShoppingCartItems(UserId)
                Dim currentProduct As New ProductController

                Weight += currentProduct.GetElement(item.ProductID).Weight * item.Quantity
                Quantity += 1
            Next

            Dim _quantity As Integer = 0 'Math.Ceiling((Weight * Quantity) / 68)
            Dim _weight As Integer = 0 '(Weight * Quantity) / _quantity

            If Not Quantity = 0 And Not Weight = 0 Then
                _quantity = CInt(Math.Ceiling((Weight * Quantity) / 68))
                _weight = CInt(Math.Max((Weight * Quantity) / _quantity, 1))

                UPS.AddShippingItem(CType(_quantity, Integer), Services.Shipping.UPSServices.PackagingMethod.CustomerSuppliedPackage, Services.Shipping.UPSServices.WeightUnit.KGS, _weight, Services.Shipping.UPSServices.DimensionUnit.Centimeters, 0, 0, 0)
            End If

            Dim strResult As String = UPS.SubmitRequest

            If strResult = "SUCCESS" Then
                AddRates(UPS.GetShippingRates)
            End If
        End Sub

        Public Sub RequestRateOptions(ByVal SessionId As String, ByVal AddressId As Integer) Implements IShippingProvider.RequestRateOptions
            Dim UPS As New Services.Shipping.UPSServices.UPSService
            Dim cookies As New System.Net.CookieContainer

            UPS.CookieContainer = cookies

            UPS.SetCustomerClassification(Services.Shipping.UPSServices.CClassification.Retail)
            UPS.RequestService("-1")

            UPS.SetAccessInfo(Settings.UPSLicenseNumber, Settings.UPSLicenseUser, Settings.UPSLicensePassword)
            UPS = GetToUPSAddress(UPS, AddressId)
            UPS.SetFromAddress(Settings.ShipFromCity, Settings.ShipFromProvince, Settings.ShipFromPC, Settings.ShipFromCountry, Settings.ShipFromAddress, "", "", CLng(Settings.ShipFromPN), Settings.ShipFromCompany, Settings.ShipFromAttention)

            Dim sc As New ShoppingCartController

            Dim Weight As Decimal = 0
            Dim Quantity As Integer = 0

            For Each item As ShoppingCart In sc.GetShoppingCartItems(SessionId)
                Dim currentProduct As New ProductController

                Weight += currentProduct.GetElement(item.ProductID).Weight * item.Quantity
                Quantity += 1
            Next

            Dim _quantity As Integer = 0 'Math.Ceiling((Weight * Quantity) / 68)
            Dim _weight As Integer = 0 '(Weight * Quantity) / _quantity

            If Not Quantity = 0 And Not Weight = 0 Then
                _quantity = CInt(Math.Ceiling((Weight * Quantity) / 68))
                _weight = CInt(Math.Max((Weight * Quantity) / _quantity, 1))

                UPS.AddShippingItem(CType(_quantity, Integer), Services.Shipping.UPSServices.PackagingMethod.CustomerSuppliedPackage, Services.Shipping.UPSServices.WeightUnit.KGS, _weight, Services.Shipping.UPSServices.DimensionUnit.Centimeters, 0, 0, 0)
            End If

            Dim strResult As String = UPS.SubmitRequest

            If strResult = "SUCCESS" Then
                AddRates(UPS.GetShippingRates)
            End If
        End Sub

        Private Sub AddRates(ByVal ProviderRates As Object) Implements IShippingProvider.AddRates
            For Each Rate As Services.Shipping.UPSServices.UPSRate In CType(ProviderRates, Services.Shipping.UPSServices.UPSRate())

                Rate.TotalCost = Rate.TotalCost * Settings.ShippingMargin

                If Settings.UseMaxShippingCharge Then
                    If Rate.TotalCost > Settings.MaxShippingCharge Then
                        Rate.TotalCost = Settings.MaxShippingCharge
                    End If
                End If

                mRates.Add(New Shipping.ShippingRate("UPS " & Rate.Service, "United Postal Service", Rate.Service, Rate.GauranteedDaysToDelivery, CDec(Rate.TotalCost)))
            Next
        End Sub

        Public Shared Function GetToUPSAddress(ByRef UPS As Services.Shipping.UPSServices.UPSService, ByVal AddressID As Integer) As Services.Shipping.UPSServices.UPSService
            Dim address As Address = New AddressController().GetElement(AddressID)
            Dim province As Province = New ProvinceController().GetElement(address.ProvStateID)
            Dim country As Country = New CountryController().GetElement(province.CountryID)

            UPS.SetToAddress(address.City, province.Code, address.PCZIP, country.Code & vbNullString, address.Address, "", "", 0, "", "Attention")

            Return UPS
        End Function

    End Class

End Namespace