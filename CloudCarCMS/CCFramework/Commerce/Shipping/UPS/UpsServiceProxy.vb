Imports System.Xml.XPath
Imports System.Xml
Imports CloudCar.CCFramework.Core

Namespace CCFramework.Commerce.Shipping.UPS

    Public Class UpsServiceProxy
        Inherits ShippingServiceProxy

        Private _AccessRequestXml As String
        Private _ShipRequestXml As String

        Private ReadOnly _CustomerClassification As CustomerClassification
        Private ReadOnly _ServiceType As ServiceType
        Private ReadOnly _PickupType As PickupType
        Private ReadOnly _PackagingMethod As PackagingMethod
        Private ReadOnly _DimensionUnit As DimensionUnit
        Private ReadOnly _WeightUnit As WeightUnit

        Private Const _ResidentialAddressIndicatior As Boolean = False

        Public Sub New(ByVal CustomerClassification As CustomerClassification, ByVal ServiceType As ServiceType, Optional ByVal DimensionUnit As DimensionUnit = DimensionUnit.Centimeters, Optional ByVal WeightUnit As WeightUnit = WeightUnit.Kilograms, Optional ByVal PackagingMethod As PackagingMethod = PackagingMethod.Unknown, Optional ByVal PickupType As PickupType = PickupType.DailyPickup)
            MyBase.New("https://onlinetools.ups.com/ups.app/xml/Rate", "http://localhost:10981/Schema/RatingServiceSelectionResponse")

            _CustomerClassification = CustomerClassification
            _ServiceType = ServiceType
            _PickupType = PickupType
            _PackagingMethod = PackagingMethod
            _DimensionUnit = DimensionUnit
            _WeightUnit = WeightUnit
        End Sub

        Private Sub BuildAccessRequestXml()
            _AccessRequestXml = ""
            _AccessRequestXml &= "<?xml version=""1.0"" ?>" & vbNewLine
            _AccessRequestXml &= "<AccessRequest xml:lang='en-US'>" & vbNewLine
            _AccessRequestXml &= "  <AccessLicenseNumber>" & _ShippingAuthentication.AuthenticationKey & "</AccessLicenseNumber>" & vbNewLine
            _AccessRequestXml &= "  <UserId>" & _ShippingAuthentication.UserName & "</UserId>" & vbNewLine
            _AccessRequestXml &= "  <Password>" & _ShippingAuthentication.PassWord & "</Password>" & vbNewLine
            _AccessRequestXml &= "</AccessRequest>" & vbNewLine
        End Sub

        Private Sub BuildShipRequestXml()
            _ShipRequestXml = ""
            _ShipRequestXml &= "<?xml version=""1.0"" ?>"
            _ShipRequestXml &= "<RatingServiceSelectionRequest>"
            _ShipRequestXml &= "    <Request>"
            _ShipRequestXml &= "        <TransactionReference>"
            _ShipRequestXml &= "            <CustomerContext>Rating and Service</CustomerContext>"
            _ShipRequestXml &= "            <XpciVersion>1.0</XpciVersion>"
            _ShipRequestXml &= "        </TransactionReference>"
            _ShipRequestXml &= "        <RequestAction>Rate</RequestAction>"

            'Rate/Shop : Change Based on user preference. make sure the neccessary variables are set
            If Not _ServiceType = ServiceType.All Then
                _ShipRequestXml &= "        <RequestOption>Rate</RequestOption>"
            Else
                _ShipRequestXml &= "        <RequestOption>Shop</RequestOption>"
            End If

            _ShipRequestXml &= "    </Request>"
            _ShipRequestXml &= "    <PickupType>"
            _ShipRequestXml &= "        <Code>" & GetPickupTypeCode(_PickupType) & "</Code>"
            _ShipRequestXml &= "        <Description>" & GetPickupTypeName(_PickupType) & "</Description>"
            _ShipRequestXml &= "    </PickupType>"
            _ShipRequestXml &= "    <CustomerClassification>"
            _ShipRequestXml &= "        <Code>" & GetCustomerClasificationCode(_CustomerClassification) & "</Code>"
            _ShipRequestXml &= "    </CustomerClassification>"
            _ShipRequestXml &= "    <Shipment>"
            _ShipRequestXml &= "        <Description>Rate Shopping - Domestic</Description>"
            _ShipRequestXml &= "        <Shipper>"
            _ShipRequestXml &= "            <Address>"
            _ShipRequestXml &= "                <AddressLine1>" & _ShipFromAddress.FirstAddressLine & "</AddressLine1>"

            If Not _ShipFromAddress.SecondAddressLine = String.Empty Then
                _ShipRequestXml &= "                <AddressLine2>" & _ShipFromAddress.SecondAddressLine & "</AddressLine2>"
            Else
                _ShipRequestXml &= "                <AddressLine2 />"
            End If

            _ShipRequestXml &= "                <City>" & _ShipFromAddress.City & "</City>"
            _ShipRequestXml &= "                <StateProvinceCode>" & _ShipFromAddress.ProvinceOrStateCode & "</StateProvinceCode>"
            _ShipRequestXml &= "                <PostalCode>" & _ShipFromAddress.PostalCode & "</PostalCode>"
            _ShipRequestXml &= "                <CountryCode>" & _ShipFromAddress.CountryCode & "</CountryCode>"
            _ShipRequestXml &= "            </Address>"
            _ShipRequestXml &= "        </Shipper>"
            _ShipRequestXml &= "        <ShipTo>"
            _ShipRequestXml &= "            <CompanyName>" & _ShippingAuthentication.CompanyName & "</CompanyName>"
            _ShipRequestXml &= "            <AttentionName>" & _ShippingAuthentication.AttentionName & "</AttentionName>"
            _ShipRequestXml &= "            <PhoneNumber>" & _ShippingAuthentication.PhoneNumber & "</PhoneNumber>"
            _ShipRequestXml &= "            <Address>"
            _ShipRequestXml &= "                <AddressLine1>" & _ShipToAddress.FirstAddressLine & "</AddressLine1>"

            If Not _ShipToAddress.SecondAddressLine = String.Empty Then
                _ShipRequestXml &= "                <AddressLine2>" & _ShipToAddress.SecondAddressLine & "</AddressLine2>"
            Else
                _ShipRequestXml &= "                <AddressLine2 />"
            End If

            _ShipRequestXml &= "                <City>" & _ShipToAddress.City & "</City>"
            _ShipRequestXml &= "                <StateProvinceCode>" & _ShipToAddress.ProvinceOrStateCode & "</StateProvinceCode>"
            _ShipRequestXml &= "                <PostalCode>" & _ShipToAddress.PostalCode & "</PostalCode>"
            _ShipRequestXml &= "                <CountryCode>" & _ShipToAddress.CountryCode & "</CountryCode>"

            If _ResidentialAddressIndicatior Then
                _ShipRequestXml &= "                <ResidentialAddressIndicator />"
            End If

            _ShipRequestXml &= "            </Address>"
            _ShipRequestXml &= "        </ShipTo>"
            _ShipRequestXml &= "        <ShipFrom>"
            _ShipRequestXml &= "            <CompanyName>" & _ShippingAuthentication.CompanyName & "</CompanyName>"
            _ShipRequestXml &= "            <AttentionName>" & _ShippingAuthentication.AttentionName & "</AttentionName>"
            _ShipRequestXml &= "            <PhoneNumber>" & _ShippingAuthentication.PhoneNumber.ToString & "</PhoneNumber>"
            _ShipRequestXml &= "            <FaxNumber>" & _ShippingAuthentication.FaxNumber & "</FaxNumber>"
            _ShipRequestXml &= "            <Address>"
            _ShipRequestXml &= "                <AddressLine1>" & _ShipFromAddress.FirstAddressLine & "</AddressLine1>"
            _ShipRequestXml &= "                <AddressLine2 />"
            _ShipRequestXml &= "                <AddressLine3 />"
            _ShipRequestXml &= "                <City>" & _ShipFromAddress.City & "</City>"
            _ShipRequestXml &= "                <StateProvinceCode>" & _ShipFromAddress.ProvinceOrStateCode & "</StateProvinceCode>"
            _ShipRequestXml &= "                <PostalCode>" & _ShipFromAddress.PostalCode & "</PostalCode>"
            _ShipRequestXml &= "                <CountryCode>" & _ShipFromAddress.CountryCode & "</CountryCode>"

            If _ResidentialAddressIndicatior Then
                _ShipRequestXml &= "                <ResidentialAddressIndicator />"
            End If

            _ShipRequestXml &= "            </Address>"
            _ShipRequestXml &= "        </ShipFrom>"

            If Not _ServiceType = ServiceType.All Then
                _ShipRequestXml &= "        <Service><Code>" & GetServiceCode(_ServiceType) & "</Code></Service>"
            End If

            For Each CurrentShippingItem As ShippingItem In _ShippingItems
                For CurrentItemIndex As Integer = 1 To CurrentShippingItem.Quantity
                    _ShipRequestXml &= "        <Package>"
                    _ShipRequestXml &= "            <PackagingType>"
                    _ShipRequestXml &= "                <Code>" & GetPackagingCode(_PackagingMethod) & "</Code>"
                    _ShipRequestXml &= "                <Description>" & CurrentShippingItem.Description & "</Description>"
                    _ShipRequestXml &= "            </PackagingType>"
                    _ShipRequestXml &= "            <Description>Rate</Description>"
                    _ShipRequestXml &= "            <Dimensions>"
                    _ShipRequestXml &= "                <UnitOfMeasurement>"
                    _ShipRequestXml &= "                    <Code>" & GetDimensionUnitString(_DimensionUnit) & "</Code>"
                    _ShipRequestXml &= "                </UnitOfMeasurement>"
                    _ShipRequestXml &= "                <Length>" & CurrentShippingItem.Length.ToString & "</Length>"
                    _ShipRequestXml &= "                <Width>" & CurrentShippingItem.Width.ToString & "</Width>"
                    _ShipRequestXml &= "                <Height>" & CurrentShippingItem.Height.ToString & "</Height>"
                    _ShipRequestXml &= "            </Dimensions>"
                    _ShipRequestXml &= "            <PackageWeight>"
                    _ShipRequestXml &= "                <UnitOfMeasurement>"
                    _ShipRequestXml &= "                    <Code>" & GetWeightUnitString(_WeightUnit) & "</Code>"
                    _ShipRequestXml &= "                </UnitOfMeasurement>"
                    _ShipRequestXml &= "                <Weight>" & CurrentShippingItem.Weight.ToString & "</Weight>"
                    _ShipRequestXml &= "            </PackageWeight>"

                    If IsLargePackage(CurrentShippingItem.Length, CurrentShippingItem.Width, CurrentShippingItem.Height) Then
                        _ShipRequestXml &= "            <LargePackageIndicator />"
                    End If

                    _ShipRequestXml &= "        </Package>"
                Next
            Next

            _ShipRequestXml &= "        <ShipmentServiceOptions />"
            _ShipRequestXml &= "    </Shipment>"
            _ShipRequestXml &= "</RatingServiceSelectionRequest>"
        End Sub

        Protected Overrides Sub BuildRequestXml()
            BuildAccessRequestXml()
            BuildShipRequestXml()

            _RequestXmlMessage = String.Format("{0} {1} {2}", _AccessRequestXml, vbNewLine, _ShipRequestXml)
        End Sub
        
        Protected Overrides Sub LoadShippingRates()
            If Not _RatesDataSet Is Nothing And _RatesDataSet.Tables("Response").Rows(0).Item("ResponseStatusDescription").ToString = "Success" Then
                Dim CurrentXmlDocument As XmlDocument = New XmlDocument()
                Dim CurrentDataTable As DataTable = New DataTable

                CurrentDataTable.Columns.Add("Service Code")
                CurrentDataTable.Columns.Add("Service Type")
                CurrentDataTable.Columns.Add("Shipment Warning")
                CurrentDataTable.Columns.Add("Billing Weight")
                CurrentDataTable.Columns.Add("Billing Weight Unit")
                CurrentDataTable.Columns.Add("Transportation Currency")
                CurrentDataTable.Columns.Add("Transportation Cost")
                CurrentDataTable.Columns.Add("Service Options Currency")
                CurrentDataTable.Columns.Add("Service Options Cost")
                CurrentDataTable.Columns.Add("Total Currency")
                CurrentDataTable.Columns.Add("Total Cost")
                CurrentDataTable.Columns.Add("Gauranteed Days to Delivery")
                CurrentDataTable.Columns.Add("Scheduled Delivery Time")

                CurrentXmlDocument.LoadXml(_RatesDataSet.GetXml())

                Dim ratingResponse As XPathNavigator = CurrentXmlDocument.CreateNavigator.SelectSingleNode("/RatingServiceSelectionResponse")

                For Each ratedShipment As XPathNavigator In ratingResponse.Select("RatedShipment")
                    Dim CurrentDataRow As DataRow = CurrentDataTable.NewRow

                    For Each serviceCode As XPathNavigator In ratedShipment.Select("Service/Code")
                        CurrentDataRow(0) = serviceCode.Value()
                        CurrentDataRow(1) = GetServiceName(serviceCode.Value)
                    Next
                    For Each shipmentWarning As XPathNavigator In ratedShipment.Select("RatedShipmentWarning")
                        CurrentDataRow(2) = shipmentWarning.Value()
                    Next
                    For Each BillingWeight As XPathNavigator In ratedShipment.Select("BillingWeight/Weight")
                        CurrentDataRow(3) = BillingWeight.Value()
                    Next
                    For Each BillingWeightUnit As XPathNavigator In ratedShipment.Select("BillingWeight/UnitOfMeasurement/Code")
                        CurrentDataRow(4) = BillingWeightUnit.Value()
                    Next
                    For Each transportationCurrency As XPathNavigator In ratedShipment.Select("TransportationCharges/CurrencyCode")
                        CurrentDataRow(5) = transportationCurrency.Value()
                    Next
                    For Each transportationCost As XPathNavigator In ratedShipment.Select("TransportationCharges/MonetaryValue")
                        CurrentDataRow(6) = transportationCost.Value()
                    Next
                    For Each serviceOptionsCurrency As XPathNavigator In ratedShipment.Select("ServiceOptionsCharges/CurrencyCode")
                        CurrentDataRow(7) = serviceOptionsCurrency.Value()
                    Next
                    For Each serviceOptionsCost As XPathNavigator In ratedShipment.Select("ServiceOptionsCharges/MonetaryValue")
                        CurrentDataRow(8) = serviceOptionsCost.Value()
                    Next
                    For Each TotalCurrency As XPathNavigator In ratedShipment.Select("TotalCharges/CurrencyCode")
                        CurrentDataRow(9) = TotalCurrency.Value()
                    Next
                    For Each TotalCost As XPathNavigator In ratedShipment.Select("TotalCharges/MonetaryValue")
                        CurrentDataRow(10) = TotalCost.Value()
                    Next
                    For Each gdtd As XPathNavigator In ratedShipment.Select("GuaranteedDaysToDelivery")
                        CurrentDataRow(11) = gdtd.Value()
                    Next
                    For Each sdt As XPathNavigator In ratedShipment.Select("ScheduledDeliveryTime")
                        CurrentDataRow(12) = sdt.Value()
                    Next

                    CurrentDataTable.Rows.Add(CurrentDataRow)
                Next

                For Each CurrentDataRow As DataRow In CurrentDataTable.Rows
                    Dim NewShippingRate As ShippingRate

                    Dim EstimatedShippingDate As String = ""
                    Dim EstimatedDeliveryDate As String = ""
                    Dim GuaranteedDaysToDelivery As Integer

                    EstimatedShippingDate = Date.Now.AddDays(_DaysToShip).ToString

                    If Not Integer.TryParse(CStr(CurrentDataRow(11)), GuaranteedDaysToDelivery) Then
                        EstimatedDeliveryDate = Date.Now.AddDays(_DaysToShip + GuaranteedDaysToDelivery).ToString
                    End If

                    NewShippingRate = New ShippingRate(CInt(CurrentDataRow(0)), CStr(CurrentDataRow(1)), "UPS", "", CDbl(CurrentDataRow(10)) * Settings.ShippingMargin, EstimatedShippingDate, EstimatedDeliveryDate)

                    AddShippingRate(NewShippingRate)
                Next

                _StatusMessage = "SUCCESS"
            Else
                Dim CurrentErrorCode As String = _RatesDataSet.Tables("Error").Rows(0).Item("ErrorCode").ToString
                Dim CurrentErrorDescription As String = _RatesDataSet.Tables("Error").Rows(0).Item("ErrorDescription").ToString

                _StatusMessage = String.Format("There was an error (Code {0}): {1}<br /><br />", CurrentErrorCode, CurrentErrorDescription)
            End If
        End Sub

        Public Shared Function GetPickupTypeCode(ByVal PickupType As PickupType) As String
            Select Case PickupType
                Case PickupType.DailyPickup
                    Return "01"
                Case PickupType.CustomerCounter
                    Return "03"
                Case PickupType.OneTimePickup
                    Return "06"
                Case PickupType.OnCallAir
                    Return "07"
                Case PickupType.SuggestedRetailRates
                    Return "11"
                Case PickupType.LetterCounter
                    Return "19"
                Case PickupType.AirServiceCenter
                    Return "20"
                Case Else
                    Return "-1"
            End Select
        End Function

        Public Shared Function GetPickupTypeName(ByVal PickupType As PickupType) As String
            Select Case PickupType
                Case PickupType.DailyPickup
                    Return "Daily Pickup"
                Case PickupType.CustomerCounter
                    Return "Customer Counter"
                Case PickupType.OneTimePickup
                    Return "One Time Pickup"
                Case PickupType.OnCallAir
                    Return "On Call Air"
                Case PickupType.SuggestedRetailRates
                    Return "Suggested Retail Rates"
                Case PickupType.LetterCounter
                    Return "Letter Counter"
                Case PickupType.AirServiceCenter
                    Return "Air Service Center"
                Case Else
                    Return "Invalid Pickup Type"
            End Select
        End Function

        Public Shared Function GetCustomerClasificationCode(ByVal CustomerClassification As CustomerClassification) As String
            Select Case CustomerClassification
                Case CustomerClassification.Occasional
                    Return "03"
                Case CustomerClassification.Retail
                    Return "04"
                Case CustomerClassification.Wholesale
                    Return "01"
                Case Else
                    Return "03"
            End Select
        End Function

        Public Shared Function GetWeightUnitString(ByVal CurrentWeightUnit As WeightUnit) As String
            Select Case CurrentWeightUnit
                Case WeightUnit.Kilograms
                    Return "KGS"
                Case WeightUnit.Pounds
                    Return "LBS"
                Case Else
                    Return "KGS"
            End Select
        End Function

        Public Shared Function GetWeightUnit(ByVal CurrentWeightUnit As String) As WeightUnit
            Select Case CurrentWeightUnit.ToUpper
                Case "KGS"
                    Return WeightUnit.Kilograms
                Case "LBS"
                    Return WeightUnit.Pounds
                Case Else
                    Return WeightUnit.Kilograms
            End Select
        End Function

        Public Shared Function GetDimensionUnitString(ByVal CurrentDimensionUnit As DimensionUnit) As String
            Select Case CurrentDimensionUnit
                Case DimensionUnit.Centimeters
                    Return "CM"
                Case DimensionUnit.Inches
                    Return "IN"
                Case Else
                    Return "IN"
            End Select
        End Function

        Public Shared Function GetDimensionUnit(ByVal CurrentDimensionUnit As String) As DimensionUnit
            Select Case CurrentDimensionUnit.ToUpper
                Case "IN"
                    Return DimensionUnit.Inches
                Case "CM"
                    Return DimensionUnit.Centimeters
                Case Else
                    Return DimensionUnit.Inches
            End Select
        End Function

        Public Shared Function GetPackagingCode(ByVal CurrentPackagingMethod As PackagingMethod) As String
            Select Case CurrentPackagingMethod
                Case PackagingMethod.Unknown
                    Return "00"
                Case PackagingMethod.UPSLetter
                    Return "01"
                Case PackagingMethod.CustomerSuppliedPackage
                    Return "02"
                Case PackagingMethod.Tube
                    Return "03"
                Case PackagingMethod.PAK
                    Return "04"
                Case PackagingMethod.UPSExpressBox
                    Return "21"
                Case PackagingMethod.UPSSmallExpressBox
                    Return "2a"
                Case PackagingMethod.UPSMediumExpressBox
                    Return "2b"
                Case PackagingMethod.UPSLargeExpressBox
                    Return "2c"
                Case PackagingMethod.UPS25KGBox
                    Return "24"
                Case PackagingMethod.UPS10KGBox
                    Return "25"
                Case PackagingMethod.Pallet
                    Return "30"
                Case Else
                    Return "00"
            End Select
        End Function

        Private Function IsLargePackage(ByVal Length As Double, ByVal Width As Double, ByVal Height As Double) As Boolean
            Dim CurrentVolume As Double = Length + ((2 * Width) + (2 * Height))

            Select Case _DimensionUnit
                Case DimensionUnit.Inches
                    If CurrentVolume > 130.0 Then
                        Return True
                    Else
                        Return False
                    End If
                Case DimensionUnit.Centimeters
                    If (CurrentVolume * 0.39) > 130.0 Then
                        Return True
                    Else
                        Return False
                    End If
                Case Else
                    Return False
            End Select
        End Function

        Public Function GetServiceName(ByVal CurrentServiceCode As String) As String
            Select Case CurrentServiceCode
                Case "-1"
                    Return "No Service Type"
                Case "01"
                    Return "Next Day Air"
                Case "02"
                    Return "Second Day Air"
                Case "03"
                    Return "Ground"
                Case "07"
                    Return "Worldwide Express"
                Case "08"
                    Return "Worldwide Expedited"
                Case "11"
                    Return "Standard"
                Case "12"
                    Return "Three Day Select"
                Case "13"
                    Return "Next Day Air Saver"
                Case "14"
                    Return "Next Day Air Early AM"
                Case "54"
                    Return "Worldwide Express Plus"
                Case "59"
                    Return "Second Day Air AM"
                Case "65"
                    Return "Saver"
                Case "82"
                    Return "Today Standard"
                Case "83"
                    Return "Today Dedicated Courrier"
                Case "84"
                    Return "Today Intercity"
                Case "85"
                    Return "Today Express"
                Case "86"
                    Return "Today Express Saver"
                Case "TDCB"
                    Return "Trade Direct Cross Border"
                Case "TDA"
                    Return "Trade Direct Air"
                Case "TDO"
                    Return "Trade Direct Ocean"
                Case "308"
                    Return "Freight LTL"
                Case "309"
                    Return "Freight LTL Guaranteed"
                Case "310"
                    Return "Freight LTL Urgent"
                Case Else
                    Return "No Service Type"
            End Select
        End Function

        Public Shared Function GetServiceType(ByVal CurrentServiceCode As String) As ServiceType
            Select Case CurrentServiceCode
                Case "-1"
                    Return ServiceType.All
                Case "01"
                    Return ServiceType.UPSNextDayAir
                Case "02"
                    Return ServiceType.UPSSecondDayAir
                Case "03"
                    Return ServiceType.UPSGround
                Case "07"
                    Return ServiceType.UPSWorldwideExpress
                Case "08"
                    Return ServiceType.UPSWorldwideExpedited
                Case "11"
                    Return ServiceType.UPSStandard
                Case "12"
                    Return ServiceType.UPSThreeDaySelect
                Case "13"
                    Return ServiceType.UPSNextDayAirSaver
                Case "14"
                    Return ServiceType.UPSNextDayAirEarlyAM
                Case "54"
                    Return ServiceType.UPSWorldwideExpressPlus
                Case "59"
                    Return ServiceType.UPSSecondDayAirAM
                Case "65"
                    Return ServiceType.UPSSaver
                Case "82"
                    Return ServiceType.UPSTodayStandard
                Case "83"
                    Return ServiceType.UPSTodayDedicatedCourrier
                Case "84"
                    Return ServiceType.UPSTodayIntercity
                Case "85"
                    Return ServiceType.UPSTodayExpress
                Case "86"
                    Return ServiceType.UPSTodayExpressSaver
                Case "TDCB"
                    Return ServiceType.TradeDirectCrossBorder
                Case "TDA"
                    Return ServiceType.TradeDirectAir
                Case "TDO"
                    Return ServiceType.TradeDirectOcean
                Case "308"
                    Return ServiceType.UPSFreightLTL
                Case "309"
                    Return ServiceType.UPSFreightLTLGuaranteed
                Case "310"
                    Return ServiceType.UPSFreightLTLUrgent
                Case Else
                    Return ServiceType.All
            End Select
        End Function

        Public Shared Function GetAllServiceTypes() As DataTable
            Dim CurrentDataTable As DataTable = New DataTable("ServiceTypesTable")

            CurrentDataTable.Columns.Add("Code")
            CurrentDataTable.Columns.Add("Description")

            CurrentDataTable.Rows.Add("-1", "All Available Services")
            CurrentDataTable.Rows.Add("01", "UPS Next Day Air")
            CurrentDataTable.Rows.Add("02", "UPS Second Day Air")
            CurrentDataTable.Rows.Add("03", "UPS Ground")
            CurrentDataTable.Rows.Add("07", "UPS Worldwide Express")
            CurrentDataTable.Rows.Add("08", "UPS Worldwide Expedited")
            CurrentDataTable.Rows.Add("11", "UPS Standard")
            CurrentDataTable.Rows.Add("12", "UPS Three Day Select")
            CurrentDataTable.Rows.Add("13", "UPS Next Day Air Saver")
            CurrentDataTable.Rows.Add("14", "UPS Next Day Air Early AM")
            CurrentDataTable.Rows.Add("54", "UPS Worldwide Express Plus")
            CurrentDataTable.Rows.Add("59", "UPS Second Day Air AM")
            CurrentDataTable.Rows.Add("65", "UPS Saver")
            CurrentDataTable.Rows.Add("82", "UPS Today Standard")
            CurrentDataTable.Rows.Add("83", "UPS Today Dedicated Courrier")
            CurrentDataTable.Rows.Add("84", "UPS Today Intercity")
            CurrentDataTable.Rows.Add("85", "UPS Today Express")
            CurrentDataTable.Rows.Add("86", "UPS Today Express Saver")
            CurrentDataTable.Rows.Add("TDCB", "Trade Direct Cross Border")
            CurrentDataTable.Rows.Add("TDA", "Trade Direct Air")
            CurrentDataTable.Rows.Add("TDO", "Trade Direct Ocean")
            CurrentDataTable.Rows.Add("308", "UPS Freight LTL")
            CurrentDataTable.Rows.Add("309", "UPS Freight LTL Guaranteed")
            CurrentDataTable.Rows.Add("310", "UPS Freight LTL Urgent")

            Return CurrentDataTable
        End Function

        Public Shared Function GetServiceCode(ByVal Type As ServiceType) As String
            Select Case Type
                Case ServiceType.All
                    Return "-1"
                Case ServiceType.UPSNextDayAir
                    Return "01"
                Case ServiceType.UPSSecondDayAir
                    Return "02"
                Case ServiceType.UPSGround
                    Return "03"
                Case ServiceType.UPSWorldwideExpress
                    Return "07"
                Case ServiceType.UPSWorldwideExpedited
                    Return "08"
                Case ServiceType.UPSStandard
                    Return "11"
                Case ServiceType.UPSThreeDaySelect
                    Return "12"
                Case ServiceType.UPSNextDayAirSaver
                    Return "13"
                Case ServiceType.UPSNextDayAirEarlyAM
                    Return "14"
                Case ServiceType.UPSWorldwideExpressPlus
                    Return "54"
                Case ServiceType.UPSSecondDayAirAM
                    Return "59"
                Case ServiceType.UPSSaver
                    Return "65"
                Case ServiceType.UPSTodayStandard
                    Return "82"
                Case ServiceType.UPSTodayDedicatedCourrier
                    Return "83"
                Case ServiceType.UPSTodayIntercity
                    Return "84"
                Case ServiceType.UPSTodayExpress
                    Return "85"
                Case ServiceType.UPSTodayExpressSaver
                    Return "86"
                Case ServiceType.TradeDirectCrossBorder
                    Return "TDCB"
                Case ServiceType.TradeDirectAir
                    Return "TDA"
                Case ServiceType.TradeDirectOcean
                    Return "TDO"
                Case ServiceType.UPSFreightLTL
                    Return "308"
                Case ServiceType.UPSFreightLTLGuaranteed
                    Return "309"
                Case ServiceType.UPSFreightLTLUrgent
                    Return "310"
                Case Else
                    Return "-1"
            End Select
        End Function

    End Class

End Namespace