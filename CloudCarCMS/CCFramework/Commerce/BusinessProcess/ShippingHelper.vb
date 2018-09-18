Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.ShoppingCart

Namespace CCFramework.Core.Shipping

    Public Class ShippingHelper

        Public Shared Function CleanPhoneNumberForShipping(ByVal strPhone As String) As Long
            strPhone = strPhone.Replace("(", vbNullString)
            strPhone = strPhone.Replace(")", vbNullString)
            strPhone = strPhone.Replace("-", vbNullString)
            strPhone = strPhone.Replace(".", vbNullString)
            strPhone = strPhone.Replace(" ", vbNullString)
            Return CLng(strPhone)
        End Function

        Public Shared Function GetFixedShippingOptions(ByVal UserId As Integer, ByVal AddressId As Integer, ByRef HttpContext As HttpContext) As List(Of ListItem)
            Dim OptionList As New List(Of ListItem)

            Dim Weight As Decimal = GetCartWeight(UserID)

            Dim CurrentAddressController As New AddressController()
            Dim CurrentAddress As Address = CurrentAddressController.GetElement(AddressId)
            CurrentAddressController.Dispose()

            If CurrentAddress.Province.Country.Code = "CA" Or CurrentAddress.Province.Country.Code = "US" Then
                Dim Prefix As String

                If CurrentAddress.Province.Country.Code = "CA" Then
                    Prefix = CurrentAddress.PCZIP.Substring(0, 3)
                Else
                    Prefix = CurrentAddress.PCZIP
                End If

                Dim Zone As Integer

                Try
                    Zone = FixedShippingZoneController.GetShippingZone(Prefix)
                Catch CurrentException As FixedShippingZoneController.InvalidShippingZoneException
                    Zone = Settings.DefaultShippingZone
                End Try

                Dim MaxWeight As Decimal = FixedShippingRateController.GetMaxShippingWeightKg()
                Dim Rate As Decimal = 0

                If Weight > MaxWeight Then
                    Dim multiplier As Integer = CInt(Math.Floor(Weight / MaxWeight))
                    Dim remainder As Decimal = Weight - (MaxWeight * multiplier)

                    Rate = (FixedShippingRateController.GetRateFromZoneKg(MaxWeight, Zone).Cost * multiplier) + FixedShippingRateController.GetRateFromZoneKg(remainder, Zone).Cost

                Else
                    Rate = FixedShippingRateController.GetRateFromZoneKg(Weight, Zone).Cost
                End If

                Rate = CInt(Rate * Settings.ShippingMargin)

                If Settings.UseMaxShippingCharge Then
                    If Rate > Settings.MaxShippingCharge Then
                        Rate = Settings.MaxShippingCharge
                    End If
                End If

                CType(HttpContext.Session("ShippingOptions"), Hashtable).Add("Fixed Shipping", New ShippingOption(Rate, "Fixed Shipping", "Fixed Shipping"))
                OptionList.Add(New ListItem("($" & FormatNumber(Rate, 2) & ")", "Fixed Shipping"))
            End If

            Return OptionList
        End Function

        Public Shared Function GetFixedShippingOptions(ByVal SessionId As String, ByVal AddressId As Integer, ByRef HttpContext As HttpContext) As List(Of ListItem)
            Dim OptionList As New List(Of ListItem)

            Dim Weight As Decimal = GetCartWeight(SessionID)

            Dim CurrentAddressController As New AddressController()
            Dim CurrentAddress As Address = CurrentAddressController.GetElement(AddressId)
            CurrentAddressController.Dispose()

            If CurrentAddress.Province.Country.Code = "CA" Or CurrentAddress.Province.Country.Code = "US" Then
                Dim Prefix As String

                If CurrentAddress.Province.Country.Code = "CA" Then
                    Prefix = CurrentAddress.PCZIP.Substring(0, 3)
                Else
                    Prefix = CurrentAddress.PCZIP
                End If

                Dim Zone As Integer

                Try
                    Zone = FixedShippingZoneController.GetShippingZone(Prefix)
                Catch CurrentException As FixedShippingZoneController.InvalidShippingZoneException
                    Zone = Settings.DefaultShippingZone
                End Try

                Dim MaxWeight As Decimal = FixedShippingRateController.GetMaxShippingWeightKg()
                Dim Rate As Decimal = 0

                If Weight > MaxWeight Then
                    Dim multiplier As Integer = CInt(Math.Ceiling(Weight / MaxWeight))
                    Dim remainder As Decimal = Weight - (MaxWeight * multiplier)

                    Rate = (FixedShippingRateController.GetRateFromZoneKg(MaxWeight, Zone).Cost * multiplier) + FixedShippingRateController.GetRateFromZoneKg(remainder, Zone).Cost
                Else
                    Rate = FixedShippingRateController.GetRateFromZoneKg(Weight, Zone).Cost
                End If

                Rate = CInt(Rate * Settings.ShippingMargin)

                If Settings.UseMaxShippingCharge Then
                    If Rate > Settings.MaxShippingCharge Then
                        Rate = Settings.MaxShippingCharge
                    End If
                End If

                CType(HttpContext.Session("ShippingOptions"), Hashtable).Add("Fixed Shipping", New ShippingOption(Rate, "Fixed Shipping", "Fixed Shipping"))
                OptionList.Add(New ListItem("($" & FormatNumber(Rate, 2) & ")", "Fixed Shipping"))
            End If

            Return OptionList
        End Function

        Public Shared Function GetUPSOptions(ByVal UserID As Integer, ByVal AddressID As Integer, ByRef HttpContext As HttpContext) As List(Of ListItem)
            Dim OptionList As New List(Of ListItem)

            Dim UPS As New Services.Shipping.UPSServices.UPSService
            Dim cookies As New System.Net.CookieContainer

            UPS.CookieContainer = cookies

            UPS.SetCustomerClassification(Services.Shipping.UPSServices.CClassification.Retail)
            UPS.RequestService("-1")

            UPS.SetAccessInfo(Settings.UPSLicenseNumber, Settings.UPSLicenseUser, Settings.UPSLicensePassword)
            UPS = GetToUPSAddress(UPS, AddressID)
            UPS.SetFromAddress(Settings.ShipFromCity, Settings.ShipFromProvince, Settings.ShipFromPC, Settings.ShipFromCountry, Settings.ShipFromAddress, "", "", CLng(Settings.ShipFromPN), Settings.ShipFromCompany, Settings.ShipFromAttention)

            Dim sc As New ShoppingCartController

            Dim Weight As Decimal = 0
            Dim Quantity As Integer = 0

            For Each item As ShoppingCart In sc.GetShoppingCartItems(UserID)
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

                AddRatesToSession(UPS.GetShippingRates, OptionList, HttpContext)

                'For Each Rate As Services.Shipping.UPSServices.UPSRate In UPS.GetShippingRates

                '    Rate.TotalCost = Rate.TotalCost * ShippingMargin

                '    If UseMaxShippingCharge Then
                '        If Rate.TotalCost > MaxShippingCharge Then
                '            Rate.TotalCost = MaxShippingCharge
                '        End If
                '    End If

                '    CType(HttpContext.Session("ShippingOptions"), Hashtable).Add(Rate.Service, New ShippingOption(Rate.TotalCost, "UPS", Rate.Service))

                '    OptionList.Add(New ListItem(Rate.Service & " ($" & FormatNumber(Rate.TotalCost, 2) & ")", Rate.Service))
                'Next

            End If

            Return OptionList
        End Function

        Public Shared Function GetUPSOptions(ByVal SessionID As String, ByVal AddressID As Integer, ByRef HttpContext As HttpContext) As List(Of ListItem)
            Dim OptionList As New List(Of ListItem)

            Dim UPS As New Services.Shipping.UPSServices.UPSService
            Dim cookies As New System.Net.CookieContainer

            UPS.CookieContainer = cookies

            UPS.SetCustomerClassification(Services.Shipping.UPSServices.CClassification.Retail)
            UPS.RequestService("-1")

            UPS.SetAccessInfo(Settings.UPSLicenseNumber, Settings.UPSLicenseUser, Settings.UPSLicensePassword)
            UPS = GetToUPSAddress(UPS, AddressID)
            UPS.SetFromAddress(Settings.ShipFromCity, Settings.ShipFromProvince, Settings.ShipFromPC, Settings.ShipFromCountry, Settings.ShipFromAddress, "", "", CLng(Settings.ShipFromPN), Settings.ShipFromCompany, Settings.ShipFromAttention)

            Dim sc As New ShoppingCartController

            Dim Weight As Decimal = 0
            Dim Quantity As Integer = 0

            For Each item As ShoppingCart In sc.GetShoppingCartItems(SessionID)
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

                AddRatesToSession(UPS.GetShippingRates, OptionList, HttpContext)

                'For Each Rate As Services.Shipping.UPSServices.UPSRate In UPS.GetShippingRates
                '    CType(HttpContext.Session("ShippingOptions"), Hashtable).Add(Rate.Service, New ShippingOption(Rate.TotalCost * ShippingMargin, "UPS", Rate.Service))

                '    OptionList.Add(New ListItem(Rate.Service & " ($" & FormatNumber(Rate.TotalCost * ShippingMargin, 2) & ")", Rate.Service))
                'Next

            End If

            Return OptionList
        End Function

        Public Shared Function GetToUPSAddress(ByRef UPS As Services.Shipping.UPSServices.UPSService, ByVal AddressID As Integer) As Services.Shipping.UPSServices.UPSService
            Dim address As Address = New AddressController().GetElement(AddressID)
            Dim province As Province = New ProvinceController().GetElement(address.ProvStateID)
            Dim country As Country = New CountryController().GetElement(province.CountryID)

            UPS.SetToAddress(address.City, province.Code, address.PCZIP, country.Code & vbNullString, address.Address, "", "", 0, "", "Attention")

            Return UPS

        End Function

        Public Shared Function GetCanadaPostOptions(ByVal UserID As Integer, ByVal AddressID As Integer, ByRef HttpContext As HttpContext) As List(Of ListItem)
            Dim OptionList As New List(Of ListItem)

            Dim CP As New Services.Shipping.CPServices.CanadaPost
            Dim cookies As New System.Net.CookieContainer

            CP.CookieContainer = cookies

            CP.SetMerchantID(Global.CloudCar.CCFramework.Core.Settings.CPMerchantID)
            CP.SetLanguage(Services.Shipping.CPServices.Language.English)
            CP = GetToCPAddress(CP, AddressID)
            CP.SetFromAddress(Global.CloudCar.CCFramework.Core.Settings.ShipFromCity, Global.CloudCar.CCFramework.Core.Settings.ShipFromProvince, Global.CloudCar.CCFramework.Core.Settings.ShipFromPC, Global.CloudCar.CCFramework.Core.Settings.ShipFromCountry)

            Dim sc As New ShoppingCartController

            For Each item As ShoppingCart In sc.GetShoppingCartItems(UserID)
                Dim product As Product = New ProductController().GetElement(item.ProductID)

                CP.AddShippingItem(item.Quantity, product.Weight, product.Length, product.Width, product.Height, product.Name, False)
            Next

            Dim strResult As String = CP.SubmitRequest

            If strResult = "SUCCESS" Then

                AddRatesToSession(CP.GetShippingRates, OptionList, HttpContext)

                'For Each Rate As Services.Shipping.CPServices.ShippingRate In CP.GetShippingRates ' Services.Shipping.CPService.ShippingRate
                '    If Not Rate.Name.Contains("Regular") Then
                '        CType(HttpContext.Session("ShippingOptions"), Hashtable).Add(Rate.Name, New ShippingOption(Rate.Rate * ShippingMargin, "Canada Post", Rate.Name))

                '        OptionList.Add(New ListItem("Canada Post " & Rate.Name & " ($" & FormatNumber(Rate.Rate * ShippingMargin, 2) & ")", Rate.Name))
                '    End If
                'Next
            End If

            Return OptionList
        End Function

        Public Shared Function GetCanadaPostOptions(ByVal SessionID As String, ByVal AddressID As Integer, ByRef HttpContext As HttpContext) As List(Of ListItem)
            Dim OptionList As New List(Of ListItem)

            Dim CP As New Services.Shipping.CPServices.CanadaPost
            Dim cookies As New System.Net.CookieContainer

            CP.CookieContainer = cookies

            CP.SetMerchantID(Global.CloudCar.CCFramework.Core.Settings.CPMerchantID)
            CP.SetLanguage(Services.Shipping.CPServices.Language.English)
            CP = GetToCPAddress(CP, AddressID)
            CP.SetFromAddress(Global.CloudCar.CCFramework.Core.Settings.ShipFromCity, Global.CloudCar.CCFramework.Core.Settings.ShipFromProvince, Global.CloudCar.CCFramework.Core.Settings.ShipFromPC, Global.CloudCar.CCFramework.Core.Settings.ShipFromCountry)

            Dim sc As New ShoppingCartController

            For Each item As ShoppingCart In sc.GetShoppingCartItems(SessionID)
                Dim product As Product = New ProductController().GetElement(item.ProductID)

                CP.AddShippingItem(item.Quantity, product.Weight, product.Length, product.Width, product.Height, product.Name, False)
            Next

            Dim strResult As String = CP.SubmitRequest

            If strResult = "SUCCESS" Then

                AddRatesToSession(CP.GetShippingRates, OptionList, HttpContext)

                'For Each Rate As Services.Shipping.CPServices.ShippingRate In CP.GetShippingRates
                '    If Not Rate.Name.Contains("Regular") Then
                '        CType(HttpContext.Session("ShippingOptions"), Hashtable).Add(Rate.Name, New ShippingOption(Rate.Rate * ShippingMargin, "Canada Post", Rate.Name))

                '        OptionList.Add(New ListItem("Canada Post " & Rate.Name & " ($" & FormatNumber(Rate.Rate * ShippingMargin, 2) & ")", Rate.Name))
                '    End If
                'Next
            End If

            Return OptionList
        End Function


        Public Shared Function GetToCPAddress(ByRef CP As Services.Shipping.CPServices.CanadaPost, ByVal AddressID As Integer) As Services.Shipping.CPServices.CanadaPost
            Dim address As Address = New AddressController().GetElement(AddressID)
            Dim province As Province = New ProvinceController().GetElement(address.ProvStateID)
            Dim country As Country = New CountryController().GetElement(province.CountryID)

            CP.SetToAddress(address.City, province.Code & vbNullString, address.PCZIP, country.Code)

            Return CP
        End Function

        Public Shared Function GetProvinceCode(ByVal ProvinceID As Integer) As String
            Dim province As Province = New ProvinceController().GetElement(ProvinceID)

            Return province.Code
        End Function


        Public Function GetCountryCode(ByVal CountryID As Integer) As String
            Dim country As Country = New CountryController().GetElement(CountryID)

            Return country.Code
        End Function

        Public Shared Function GetCartWeight(ByVal UserId As Integer) As Decimal
            Dim CurrentShoppingCartController As New ShoppingCartController
            Dim CurrentProductController As New ProductController

            Dim CurrentWeight As Decimal = 0

            For Each CurrentItem As ShoppingCart In CurrentShoppingCartController.GetShoppingCartItems(UserId)
                CurrentWeight += CurrentProductController.GetElement(CurrentItem.ProductID).Weight * CurrentItem.Quantity
            Next

            CurrentShoppingCartController.Dispose()
            CurrentProductController.Dispose()

            Return CurrentWeight
        End Function

        Public Shared Function GetCartWeight(ByVal SessionId As String) As Decimal
            Dim CurrentShoppingCartController As New ShoppingCartController
            Dim CurrentProductController As New ProductController

            Dim CurrentWeight As Decimal = 0

            For Each CurrentItem As ShoppingCart In CurrentShoppingCartController.GetShoppingCartItems(SessionId)
                CurrentWeight += CurrentProductController.GetElement(CurrentItem.ProductID).Weight * CurrentItem.Quantity
            Next

            CurrentShoppingCartController.Dispose()
            CurrentProductController.Dispose()

            Return CurrentWeight
        End Function


        Public Shared Function AddRatesToSession(ByVal Rates As Services.Shipping.UPSServices.UPSRate(), ByRef OptionList As List(Of ListItem), ByVal Context As HttpContext) As Boolean
            For Each Rate As Services.Shipping.UPSServices.UPSRate In Rates

                Rate.TotalCost = Rate.TotalCost * Settings.ShippingMargin

                If Settings.UseMaxShippingCharge Then
                    If Rate.TotalCost > Settings.MaxShippingCharge Then
                        Rate.TotalCost = Settings.MaxShippingCharge
                    End If
                End If

                CType(Context.Session("ShippingOptions"), Hashtable).Add(Rate.Service, New ShippingOption(CDec(Rate.TotalCost), "UPS", Rate.Service))

                OptionList.Add(New ListItem(Rate.Service & " ($" & FormatNumber(Rate.TotalCost, 2) & ")", Rate.Service))
            Next

            Return True
        End Function

        Public Shared Function AddRatesToSession(ByVal Rates As Services.Shipping.CPServices.ShippingRate(), ByRef OptionList As List(Of ListItem), ByVal Context As HttpContext) As Boolean
            For Each Rate As Services.Shipping.CPServices.ShippingRate In Rates
                If Not Rate.Name.Contains("Regular") Then
                    Rate.Rate = Rate.Rate * Global.CloudCar.CCFramework.Core.Settings.ShippingMargin

                    If Global.CloudCar.CCFramework.Core.Settings.UseMaxShippingCharge Then
                        If Rate.Rate > Global.CloudCar.CCFramework.Core.Settings.MaxShippingCharge Then
                            Rate.Rate = Global.CloudCar.CCFramework.Core.Settings.MaxShippingCharge
                        End If
                    End If

                    CType(Context.Session("ShippingOptions"), Hashtable).Add(Rate.Name, New ShippingOption(CDec(Rate.Rate), "Canada Post", Rate.Name))

                    OptionList.Add(New ListItem("Canada Post " & Rate.Name & " ($" & FormatNumber(Rate.Rate, 2) & ")", Rate.Name))
                End If
            Next

            Return True
        End Function

    End Class

    Public Structure ShippingOption
        Public Total As Decimal
        Public Company As String
        Public Service As String

        Public Sub New(ByVal _Total As Decimal, ByVal _Company As String, ByVal _Service As String)
            Total = _Total
            Company = _Company
            Service = _Service
        End Sub

    End Structure

End Namespace