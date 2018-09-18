Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce.Shipping.CanadaPost
Imports CloudCar.CCFramework.Commerce.Shipping.Purolator
Imports CloudCar.CCFramework.Commerce.Shipping.Fixed
Imports CloudCar.CCFramework.Commerce.Shipping.UPS
Imports System.Net
Imports CloudCar.CCFramework.Commerce.ShoppingCart

Namespace CCFramework.Commerce.Shipping

    Public Class ShippingRatesController

        Private Const _DefaultConnectionLimit As Integer = 20
        Private Const _ServicePointIdleTime As Integer = 8000

        Private ReadOnly _ShippingRates As List(Of ShippingRate)
        Private ReadOnly _ShoppingCartItems As List(Of Model.ShoppingCart)
        Private ReadOnly _ToAddress As Address
        Private ReadOnly _CartWeight As Decimal

        Public Sub New(ByVal SessionId As String, ByVal AddressId As Integer)
            Dim CurrentShoppingCartController As New ShoppingCartController
            Dim CurrentAddressController As New AddressController

            _ShoppingCartItems = CurrentShoppingCartController.GetShoppingCartItems(SessionId)

            _ToAddress = CurrentAddressController.GetElement(AddressId)
            _ToAddress.Province = CurrentAddressController.GetElement(AddressId).Province
            _ToAddress.Province.Country = CurrentAddressController.GetElement(AddressId).Province.Country

            _CartWeight = Core.Shipping.ShippingHelper.GetCartWeight(SessionId)

            _ShippingRates = New List(Of ShippingRate)

            CurrentShoppingCartController.Dispose()
            CurrentAddressController.Dispose()
        End Sub

        Public Sub New(ByVal UserId As Integer, ByVal AddressId As Integer)
            Dim CurrentShoppingCartController As New ShoppingCartController
            Dim CurrentAddressController As New AddressController

            _ShoppingCartItems = CurrentShoppingCartController.GetShoppingCartItems(UserId)

            _ToAddress = CurrentAddressController.GetElement(AddressId)
            _ToAddress.Province = CurrentAddressController.GetElement(AddressId).Province
            _ToAddress.Province.Country = CurrentAddressController.GetElement(AddressId).Province.Country

            _CartWeight = Core.Shipping.ShippingHelper.GetCartWeight(UserId)

            _ShippingRates = New List(Of ShippingRate)

            CurrentShoppingCartController.Dispose()
            CurrentAddressController.Dispose()
        End Sub

        Public Sub RequestAvailableShippingRates()
            Dim CurrentToShippingAddress As New ShippingAddress(_ToAddress.Address, "", _ToAddress.City, _ToAddress.Province.Name, _ToAddress.Province.Code, _ToAddress.PCZIP, _ToAddress.Province.Country.Name, _ToAddress.Province.Country.Code)
            Dim CurrentFromShippingAddress As New ShippingAddress(Settings.ShipFromAddress, "", Settings.ShipFromCity, Settings.ShipFromProvince, Settings.ShipFromProvinceCode, Settings.ShipFromPC, Settings.ShipFromCountry, Settings.ShipFromCountryCode)

            ServicePointManager.DefaultConnectionLimit = _DefaultConnectionLimit
            'ServicePointManager.MaxServicePointIdleTime = _ServicePointIdleTime

            If Settings.UseCPRates Then
                Dim CanadaPostProxy As New CanadaPostServiceProxy(Language.English)
                Dim CurrentAuthentication As New ShippingAuthentication("", "", Settings.CPMerchantID, Settings.CompanyName, Settings.ShipFromAttention, Settings.HomePage, "")

                CanadaPostProxy.SetAuthentication(CurrentAuthentication)
                CanadaPostProxy.SetShipToAddress(CurrentToShippingAddress)
                CanadaPostProxy.SetShipFromAddress(CurrentFromShippingAddress)

                For Each CurrentShoppingCartItem As Model.ShoppingCart In _ShoppingCartItems
                    Dim CurrentProduct As Model.Product = New ProductController().GetElement(CurrentShoppingCartItem.ProductID)

                    Dim CurrentShippingItem As New ShippingItem(CurrentShoppingCartItem.Quantity, CurrentProduct.Weight, CurrentProduct.Length, CurrentProduct.Width, CurrentProduct.Height, CurrentProduct.Name, CurrentProduct.Price)

                    CanadaPostProxy.AddShippingItem(CurrentShippingItem)
                Next

                CanadaPostProxy.ProcessRateRequest()

                AppendRates(CanadaPostProxy.RateItems)
            End If

            If Settings.UseUPSRates Then
                Dim UpsProxy As New UpsServiceProxy(CustomerClassification.Retail, ServiceType.All, DimensionUnit.Centimeters, WeightUnit.Kilograms, PackagingMethod.CustomerSuppliedPackage, PickupType.CustomerCounter)
                Dim CurrentAuthentication As New ShippingAuthentication(Settings.UPSLicenseUser, Settings.UPSLicensePassword, Settings.UPSLicenseNumber, Settings.CompanyName, Settings.ShipFromAttention, Settings.HomePage, "")

                UpsProxy.SetAuthentication(CurrentAuthentication)
                UpsProxy.SetShipToAddress(CurrentToShippingAddress)
                UpsProxy.SetShipFromAddress(CurrentFromShippingAddress)

                For Each CurrentShoppingCartItem As Model.ShoppingCart In _ShoppingCartItems
                    Dim CurrentProduct As Model.Product = New ProductController().GetElement(CurrentShoppingCartItem.ProductID)

                    Dim CurrentShippingItem As New ShippingItem(CurrentShoppingCartItem.Quantity, CurrentProduct.Weight, CurrentProduct.Length, CurrentProduct.Width, CurrentProduct.Height, CurrentProduct.Name, CurrentProduct.Price)

                    UpsProxy.AddShippingItem(CurrentShippingItem)
                Next

                UpsProxy.ProcessRateRequest()

                AppendRates(UpsProxy.RateItems)
            End If

            If Settings.UsePurolatorRates Then
                Dim PurolatorProxy As New PurolatorServiceProxy(Settings.AdminEmail, "lalala lalala", PurolatorService.WeightUnit.kg, PurolatorService.DimensionUnit.in, PurolatorServiceType.PurolatorExpress, PurolatorService.PickupType.DropOff)
                Dim CurrentAuthentication As New ShippingAuthentication(Settings.PurolatorLicenseUser, Settings.PurolatorLicensePassword, Settings.PurolatorLicenseKey, Settings.CompanyName, Settings.ShipFromAttention, Settings.HomePage, "")

                PurolatorProxy.SetAuthentication(CurrentAuthentication)
                PurolatorProxy.SetShipToAddress(CurrentToShippingAddress)
                PurolatorProxy.SetShipFromAddress(CurrentFromShippingAddress)

                For Each CurrentShoppingCartItem As Model.ShoppingCart In _ShoppingCartItems
                    Dim CurrentProduct As Model.Product = New ProductController().GetElement(CurrentShoppingCartItem.ProductID)

                    Dim CurrentShippingItem As New ShippingItem(CurrentShoppingCartItem.Quantity, CurrentProduct.Weight, CurrentProduct.Length, CurrentProduct.Width, CurrentProduct.Height, CurrentProduct.Name, CurrentProduct.Price)

                    PurolatorProxy.AddShippingItem(CurrentShippingItem)
                Next

                PurolatorProxy.ProcessRateRequest()

                AppendRates(PurolatorProxy.RateItems)
            End If

            If Settings.UseFixedRate Then
                Dim FixedShippingProxy As New FixedShippingProxy(_CartWeight)

                FixedShippingProxy.SetShipToAddress(CurrentToShippingAddress)

                For Each CurrentShoppingCartItem As Model.ShoppingCart In _ShoppingCartItems
                    Dim CurrentProduct As Model.Product = New ProductController().GetElement(CurrentShoppingCartItem.ProductID)

                    Dim CurrentShippingItem As New ShippingItem(CurrentShoppingCartItem.Quantity, CurrentProduct.Weight, CurrentProduct.Length, CurrentProduct.Width, CurrentProduct.Height, CurrentProduct.Name, CurrentProduct.Price)

                    FixedShippingProxy.AddShippingItem(CurrentShippingItem)
                Next

                FixedShippingProxy.ProcessRateRequest()

                AppendRates(FixedShippingProxy.RateItems)
            End If

            If Settings.UsePickUpRate Then
                _ShippingRates.Add(New ShippingRate(1, "Local Pick Up", Settings.CompanyName, "Local Pick Up", 0.0, Date.Now.AddDays(2).ToString, Date.Now.AddDays(6).ToString))
            End If

            SortRates()
        End Sub

        Private Sub AppendRates(ByVal Rates As List(Of ShippingRate))
            'If _ShippingRates.Count > 0 Then
            '_ShippingRates.Union(Rates)
            'Else
            _ShippingRates.AddRange(Rates)
            'End If
        End Sub

        Private Sub SortRates()
            '_ShippingRates = _ShippingRates.OrderBy(Function(Rate) Rate.Company And Rate.Rate).ToList
        End Sub

        Public Function GetRatesAsListItems() As List(Of ListItem)
            Return (From CurrentRate In _ShippingRates Select New ListItem(String.Format("{0} {1} {2:$0.00}", CurrentRate.Company, CurrentRate.Name, CurrentRate.Rate), CurrentRate.UniqueId.ToString)).ToList()
        End Function

        Public Function GetRateById(ByVal Id As String) As ShippingRate
            Return (From Rate In _ShippingRates Where Rate.UniqueId.ToString Like Id Select Rate).FirstOrDefault
        End Function

    End Class

End Namespace