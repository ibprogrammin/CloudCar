Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.ShoppingCart

Namespace CCFramework.Core.Shipping

    Public Class CanadaPostProvider
        Inherits ShippingProviderClass
        Implements IShippingProvider

        Public Sub RequestRateOptions(ByVal UserID As Integer, ByVal AddressID As Integer) Implements IShippingProvider.RequestRateOptions
            Dim CP As New Services.Shipping.CPServices.CanadaPost
            Dim cookies As New Net.CookieContainer

            CP.CookieContainer = cookies

            CP.SetMerchantID(Settings.CPMerchantID)
            CP.SetLanguage(Services.Shipping.CPServices.Language.English)
            CP = GetToCPAddress(CP, AddressID)
            CP.SetFromAddress(Settings.ShipFromCity, Settings.ShipFromProvince, Settings.ShipFromPC, Settings.ShipFromCountry)

            Dim sc As New ShoppingCartController

            For Each item As ShoppingCart In sc.GetShoppingCartItems(UserID)
                Dim product As Product = New ProductController().GetElement(item.ProductID)

                CP.AddShippingItem(item.Quantity, product.Weight, product.Length, product.Width, product.Height, product.Name, False)
            Next

            Dim strResult As String = CP.SubmitRequest

            If strResult = "SUCCESS" Then
                AddRates(CP.GetShippingRates)
            End If
        End Sub

        Public Sub RequestRateOptions(ByVal SessionId As String, ByVal AddressId As Integer) Implements IShippingProvider.RequestRateOptions
            Dim CP As New Services.Shipping.CPServices.CanadaPost
            Dim cookies As New System.Net.CookieContainer

            CP.CookieContainer = cookies

            CP.SetMerchantID(Global.CloudCar.CCFramework.Core.Settings.CPMerchantID)
            CP.SetLanguage(Services.Shipping.CPServices.Language.English)
            CP = GetToCPAddress(CP, AddressId)
            CP.SetFromAddress(Global.CloudCar.CCFramework.Core.Settings.ShipFromCity, Global.CloudCar.CCFramework.Core.Settings.ShipFromProvince, Global.CloudCar.CCFramework.Core.Settings.ShipFromPC, Global.CloudCar.CCFramework.Core.Settings.ShipFromCountry)

            Dim sc As New ShoppingCartController

            For Each item As ShoppingCart In sc.GetShoppingCartItems(SessionId)
                Dim product As Product = New ProductController().GetElement(item.ProductID)

                CP.AddShippingItem(item.Quantity, product.Weight, product.Length, product.Width, product.Height, product.Name, False)
            Next

            Dim strResult As String = CP.SubmitRequest

            If strResult = "SUCCESS" Then
                AddRates(CP.GetShippingRates)
            End If
        End Sub

        Public Shared Function GetToCPAddress(ByRef CP As Services.Shipping.CPServices.CanadaPost, ByVal AddressID As Integer) As Services.Shipping.CPServices.CanadaPost
            Dim address As Address = New AddressController().GetElement(AddressID)
            Dim province As Province = New ProvinceController().GetElement(address.ProvStateID)
            Dim country As Country = New CountryController().GetElement(province.CountryID)

            CP.SetToAddress(address.City, province.Code & vbNullString, address.PCZIP, country.Code)

            Return CP
        End Function

        Private Sub AddRates(ByVal ProviderRates As Object) Implements IShippingProvider.AddRates
            For Each Rate As Services.Shipping.CPServices.ShippingRate In CType(ProviderRates, Services.Shipping.CPServices.ShippingRate())
                If Not Rate.Name.Contains("Regular") Then
                    Rate.Rate = Rate.Rate * Settings.ShippingMargin

                    If Settings.UseMaxShippingCharge Then
                        If Rate.Rate > Settings.MaxShippingCharge Then
                            Rate.Rate = Settings.MaxShippingCharge
                        End If
                    End If

                    mRates.Add(New ShippingRate("Canada Post " & Rate.Name, "Canada Post", Rate.Name, Rate.DeliveryDate, CDec(Rate.Rate)))
                End If
            Next
        End Sub

    End Class

End Namespace