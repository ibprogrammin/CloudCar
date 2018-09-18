Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce.Shipping
Imports CloudCar.CCFramework.Commerce.Shipping.CanadaPost
Imports CloudCar.CCFramework.Commerce.Shipping.UPS
Imports CloudCar.CCFramework.Commerce.Shipping.Purolator
'Imports CloudCar.CCFramework.Core.Shipping
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Core.Shipping
Imports CloudCar.CCFramework.Commerce.Shipping.Fixed
Imports System.Net
Imports CloudCar.CCFramework.Commerce.ShoppingCart

'Imports CloudCar.CCFramework.Core.Shipping

Namespace CCControls.Commerce.ShippingControls

    Public Class ShippingRatesControl
        Inherits UserControl

        Private Const _DefaultConnectionLimit As Integer = 12

        Private _ShippingRates As List(Of CCFramework.Commerce.Shipping.ShippingRate)

        Public Sub RequestAvailableShippingRates(ByVal SessionId As String, ByVal AddressId As Integer)
            Dim CurrentAddressController As New AddressController()
            Dim CurrentFromAddress As Address = CurrentAddressController.GetElement(AddressId)
            CurrentAddressController.Dispose()
            
            Dim CurrentToShippingAddress As New ShippingAddress(CurrentFromAddress.Address, "", CurrentFromAddress.City, CurrentFromAddress.Province.Name, CurrentFromAddress.Province.Code, CurrentFromAddress.PCZIP, CurrentFromAddress.Province.Country.Name, CurrentFromAddress.Province.Country.Code)
            Dim CurrentFromShippingAddress As New ShippingAddress("2168 Mountainside drive", "", "Burlington", "Ontario", "ON", "L7P 1B4", "Canada", "CA")
            'Dim CurrentFromShippingAddress As New ShippingAddress("2168 Mountainside drive", "", Settings.ShipFromCity, Settings.ShipFromProvince, "ON", Settings.ShipFromPC, Settings.ShipFromCountry, "CA")

            ServicePointManager.DefaultConnectionLimit = _DefaultConnectionLimit

            If Settings.UseCPRates Then
                Dim CanadaPostProxy As New CanadaPostServiceProxy(Language.English)
                Dim CurrentAuthentication As New ShippingAuthentication("", "", Settings.CPMerchantID, Settings.CompanyName, Settings.ShipFromAttention, Settings.HomePage, "")

                CanadaPostProxy.SetAuthentication(CurrentAuthentication)
                CanadaPostProxy.SetShipToAddress(CurrentToShippingAddress)
                CanadaPostProxy.SetShipFromAddress(CurrentFromShippingAddress)

                Dim CurrentShoppingCartController As New ShoppingCartController

                For Each CurrentShoppingCartItem As ShoppingCart In CurrentShoppingCartController.GetShoppingCartItems(SessionId)
                    Dim CurrentProduct As Product = New ProductController().GetElement(CurrentShoppingCartItem.ProductID)

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

                Dim CurrentShoppingCartController As New ShoppingCartController

                For Each CurrentShoppingCartItem As ShoppingCart In CurrentShoppingCartController.GetShoppingCartItems(SessionId)
                    Dim CurrentProduct As Product = New ProductController().GetElement(CurrentShoppingCartItem.ProductID)

                    Dim CurrentShippingItem As New ShippingItem(CurrentShoppingCartItem.Quantity, CurrentProduct.Weight, CurrentProduct.Length, CurrentProduct.Width, CurrentProduct.Height, CurrentProduct.Name, CurrentProduct.Price)

                    UpsProxy.AddShippingItem(CurrentShippingItem)
                Next

                UpsProxy.ProcessRateRequest()

                AppendRates(UpsProxy.RateItems)
            End If

            'If Settings.UsePurolatorRates Then
            If True Then
                Dim PurolatorProxy As New PurolatorServiceProxy(Settings.AdminEmail, "lalala lalala", PurolatorService.WeightUnit.kg, PurolatorService.DimensionUnit.in, PurolatorServiceType.PurolatorExpress, PurolatorService.PickupType.DropOff)
                Dim CurrentAuthentication As New ShippingAuthentication("", "", Settings.CPMerchantID, Settings.CompanyName, Settings.ShipFromAttention, Settings.HomePage, "")

                PurolatorProxy.SetAuthentication(CurrentAuthentication)
                PurolatorProxy.SetShipToAddress(CurrentToShippingAddress)
                PurolatorProxy.SetShipFromAddress(CurrentFromShippingAddress)

                Dim CurrentShoppingCartController As New ShoppingCartController

                For Each CurrentShoppingCartItem As ShoppingCart In CurrentShoppingCartController.GetShoppingCartItems(SessionId)
                    Dim CurrentProduct As Product = New ProductController().GetElement(CurrentShoppingCartItem.ProductID)

                    Dim CurrentShippingItem As New ShippingItem(CurrentShoppingCartItem.Quantity, CurrentProduct.Weight, CurrentProduct.Length, CurrentProduct.Width, CurrentProduct.Height, CurrentProduct.Name, CurrentProduct.Price)

                    PurolatorProxy.AddShippingItem(CurrentShippingItem)
                Next

                PurolatorProxy.ProcessRateRequest()

                AppendRates(PurolatorProxy.RateItems)
            End If

            If Settings.UseFixedRate Then
                Dim CurrentCartWeight As Decimal = ShippingHelper.GetCartWeight(SessionId)

                Dim FixedShippingProxy As New FixedShippingProxy(CurrentCartWeight)
                'Dim CurrentAuthentication As New ShippingAuthentication("", "", Settings.CPMerchantID, Settings.CompanyName, Settings.ShipFromAttention, Settings.HomePage, "")

                'PurolatorProxy.SetAuthentication(CurrentAuthentication)
                FixedShippingProxy.SetShipToAddress(CurrentToShippingAddress)
                'FixedShippingProxy.SetShipFromAddress(CurrentFromShippingAddress)

                Dim CurrentShoppingCartController As New ShoppingCartController

                For Each CurrentShoppingCartItem As ShoppingCart In CurrentShoppingCartController.GetShoppingCartItems(SessionId)
                    Dim CurrentProduct As Product = New ProductController().GetElement(CurrentShoppingCartItem.ProductID)

                    Dim CurrentShippingItem As New ShippingItem(CurrentShoppingCartItem.Quantity, CurrentProduct.Weight, CurrentProduct.Length, CurrentProduct.Width, CurrentProduct.Height, CurrentProduct.Name, CurrentProduct.Price)

                    FixedShippingProxy.AddShippingItem(CurrentShippingItem)
                Next

                FixedShippingProxy.ProcessRateRequest()

                AppendRates(FixedShippingProxy.RateItems)
            End If

            If Settings.UsePickUpRate Then
                _ShippingRates.Add(New CCFramework.Commerce.Shipping.ShippingRate(1, "Local Pick Up", Settings.CompanyName, "Local Pick Up", 0.0, Date.Now.AddDays(2).ToString, Date.Now.AddDays(6).ToString))
            End If

            SortRates()
        End Sub

        Public Sub RequestAvailableShippingRates(ByVal UserId As Integer, ByVal AddressId As Integer)

        End Sub

        Private Sub AppendRates(ByVal Rates As List(Of CCFramework.Commerce.Shipping.ShippingRate))
            _ShippingRates.Union(Rates)
        End Sub

        Private Sub SortRates()
            _ShippingRates = _ShippingRates.OrderBy(Function(Rate) Rate.Company).ThenBy(Function(Rate) Rate.Rate).ToList
        End Sub
        
    End Class

End Namespace