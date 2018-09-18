Imports CloudCar.CCFramework.Core

Namespace CCFramework.Commerce.Shipping.CanadaPost

    Public Class CanadaPostServiceProxy
        Inherits ShippingServiceProxy

        Private Const _ReadyToShip As Boolean = True
        Private Const _TurnAroundTime As Integer = 48

        Private ReadOnly _RequestLanguage As Language

        Public Sub New(ByVal RequestLanguage As Language)
            MyBase.New("http://sellonline.canadapost.ca:30000/", "http://sellonline.canadapost.ca/DevelopersResources/protocolV3/eParcel.dtd")

            _AppendToRequest = "XMLRequest="
            _RequestLanguage = RequestLanguage
            _DataSetReadMode = XmlReadMode.InferTypedSchema
        End Sub

        Protected Overrides Sub BuildRequestXml()
            Dim XmlHeader As New StringBuilder
            Dim XmlFooter As New StringBuilder
            Dim XmlContent As New StringBuilder

            XmlHeader.Append("<?xml version=""1.0"" ?>" & vbNewLine)
            XmlHeader.Append("<!DOCTYPE eparcel SYSTEM ""http://sellonline.canadapost.ca/DevelopersResources/protocolV3/eParcel.dtd"">")
            XmlHeader.Append("<eparcel>" & vbNewLine)
            XmlHeader.Append(String.Format("<language>{0}</language>" & vbNewLine, GetLanguage(_RequestLanguage)))
            XmlHeader.Append("<ratesAndServicesRequest>" & vbNewLine)

            XmlContent.Append(String.Format("<merchantCPCID>{0}</merchantCPCID>" & vbNewLine, _ShippingAuthentication.AuthenticationKey))

            If Not _ShipFromAddress.PostalCode = Nothing Then
                XmlContent.Append(String.Format("<fromPostalCode>{0}</fromPostalCode>" & vbNewLine, _ShipFromAddress.PostalCode))
            End If

            XmlContent.Append(String.Format("<turnAroundTime>{0}</turnAroundTime>" & vbNewLine, _TurnAroundTime.ToString))

            Dim CurrentPrice As Double = 0.0
            For CurrentIndex As Integer = 0 To _ShippingItems.Count - 1
                CurrentPrice += (_ShippingItems.Item(CurrentIndex).Price * _ShippingItems.Item(CurrentIndex).Quantity)
            Next
            XmlContent.Append(String.Format("<itemsPrice>{0:0.00}</itemsPrice>", CurrentPrice.ToString))

            XmlContent.Append("<lineItems>" & vbNewLine)

            For Each CurrentShippingItem As ShippingItem In _ShippingItems
                XmlContent.Append("<item>" & vbNewLine)

                XmlContent.Append(String.Format("<quantity>{0}</quantity>" & vbNewLine, CurrentShippingItem.Quantity.ToString))
                XmlContent.Append(String.Format("<weight>{0}</weight>" & vbNewLine, CurrentShippingItem.Weight.ToString))
                XmlContent.Append(String.Format("<length>{0}</length>" & vbNewLine, CurrentShippingItem.Length.ToString))
                XmlContent.Append(String.Format("<width>{0}</width>" & vbNewLine, CurrentShippingItem.Width.ToString))
                XmlContent.Append(String.Format("<height>{0}</height>" & vbNewLine, CurrentShippingItem.Height.ToString))

                If CurrentShippingItem.Description = String.Empty Then
                    XmlContent.Append("<description>None</description>" & vbNewLine)
                Else
                    XmlContent.Append(String.Format("<description>{0}</description>" & vbNewLine, CurrentShippingItem.Description.ToString))
                End If


                If _ReadyToShip Then
                    XmlContent.Append("<readyToShip/>" & vbNewLine)
                End If

                XmlContent.Append("</item>" & vbNewLine)
            Next

            XmlContent.Append("</lineItems>" & vbNewLine)

            If Not _ShipToAddress.City = Nothing Then
                XmlContent.Append(String.Format("<city>{0}</city>" & vbNewLine, _ShipToAddress.City))
            End If

            If Not _ShipToAddress.ProvinceOrState = Nothing Then
                XmlContent.Append(String.Format("<provOrState>{0}</provOrState>" & vbNewLine, _ShipToAddress.ProvinceOrState))
            End If

            If Not _ShipToAddress.Country = Nothing Then
                XmlContent.Append(String.Format("<country>{0}</country>" & vbNewLine, _ShipToAddress.Country))
            Else
                Throw New Exception("A fault has occurred in the Service. The Country has not been set for the recipient address.")
            End If

            If Not _ShipToAddress.PostalCode = Nothing Then
                XmlContent.Append(String.Format("<postalCode>{0}</postalCode>" & vbNewLine, _ShipToAddress.PostalCode))
            Else
                Throw New Exception("A fault has occurred in the Service. The Postal Code has not been set for the recipient address.")
            End If

            XmlFooter.Append("</ratesAndServicesRequest>" & vbNewLine)
            XmlFooter.Append("</eparcel>" & vbNewLine)

            _RequestXmlMessage = String.Format("{0}{1}{2}", XmlHeader.ToString, XmlContent.ToString, XmlFooter.ToString)
        End Sub

        Protected Overrides Sub LoadShippingRates()
            If Not _RatesDataSet.Tables("product") Is Nothing Then
                If _RatesDataSet.Tables("product").Rows.Count > 0 Then
                    For Each CurrentDataRow As DataRow In _RatesDataSet.Tables("product").Rows
                        Dim NewShippingRate As ShippingRate

                        Dim CurrentId As Integer = CType(CurrentDataRow("id"), Integer)
                        Dim CurrentName As String = CType(CurrentDataRow("name"), String)
                        Dim CurrentRate As Double = CDbl(CurrentDataRow("rate")) * Settings.ShippingMargin
                        Dim EstimatedShippingDate As String = CType(CurrentDataRow("shippingDate"), String)
                        Dim EstimatedDeliveryDate As String = CType(CurrentDataRow("deliveryDate"), String)

                        NewShippingRate = New ShippingRate(CurrentId, CurrentName, "Canada Post", "", CurrentRate, EstimatedShippingDate, EstimatedDeliveryDate)

                        AddShippingRate(NewShippingRate)
                    Next

                    _StatusMessage = "SUCCESS"
                Else
                    Dim CurrentErrorCode As String = _RatesDataSet.Tables("error").Rows(0).Item(0).ToString
                    Dim CurrentErrorDescription As String = _RatesDataSet.Tables("error").Rows(0).Item(1).ToString

                    _StatusMessage = String.Format("There was an error (Code {0}): {1}<br /><br />", CurrentErrorCode, CurrentErrorDescription)
                End If
            Else
                Dim CurrentErrorCode As String = "-1"
                Dim CurrentErrorDescription As String = "No result set was returned"

                _StatusMessage = String.Format("There was an error (Code {0}): {1}<br /><br />", CurrentErrorCode, CurrentErrorDescription)
            End If
        End Sub

        Public Function GetProductName(ByVal ProductCode As Integer) As String
            Select Case ProductCode
                Case 1010
                    Return "REGULAR"
                Case 1020
                    Return "EXPEDITED"
                Case 1030
                    Return "XPRESSPOST"
                Case 1040
                    Return "PRIORITY COURIER"
                Case 2005
                    Return "SMALL PACKETS SURFACE"
                Case 2015
                    Return "SMALL PACKETS AIR"
                Case 2020
                    Return "EXPEDITED US BUSINESS CONTRACT"
                Case 2025
                    Return "EXPEDITED US COMMERCIAL"
                Case 2030
                    Return "XPRESSPOST USA"
                Case 2040
                    Return "PUROLATOR INTERNATIONAL"
                Case 2050
                    Return "PUROPAK INTERNATIONAL"
                Case 3005
                    Return "SMALL PACKETS SURFACE"
                Case 3010
                    Return "SURFACE INTERNATIONAL"
                Case 3015
                    Return "SMALL PACKETS AIR"
                Case 3020
                    Return "AIR INTERNATIONAL"
                Case 3025
                    Return "XPRESSPOST INTERNATIONAL"
                Case 3040
                    Return "PUROLATOR INTERNATIONAL"
                Case 3050
                    Return "PUROPAK INTERNATIONAL"
                Case Else
                    Return "INVALID PRODUCT CODE"
            End Select
        End Function

        Public Shared Function GetDayOfWeek(ByVal CurrentDay As Integer) As String
            Select Case CurrentDay
                Case 1
                    Return "Sunday"
                Case 2
                    Return "Monday"
                Case 3
                    Return "Tuesday"
                Case 4
                    Return "Wednesday"
                Case 5
                    Return "Thursday"
                Case 6
                    Return "Friday"
                Case 7
                    Return "Saturday"
                Case Else
                    Return "Invalid Day of the week"
            End Select
        End Function

        Public Shared Function GetLanguage(ByVal CurrentLanguage As Language) As String
            Select Case CurrentLanguage
                Case Language.English
                    GetLanguage = "en"
                Case Language.French
                    GetLanguage = "fr"
                Case Else
                    GetLanguage = "en"
            End Select
        End Function

        Public Shared Function GetAllServiceTypes() As DataTable
            Dim CurrentDataTable As DataTable = New DataTable("ServiceTypesTable")

            CurrentDataTable.Columns.Add("Code")
            CurrentDataTable.Columns.Add("Product")

            CurrentDataTable.Rows.Add("-1", "All Available Services")
            CurrentDataTable.Rows.Add("1010", "Regular")
            CurrentDataTable.Rows.Add("1020", "Expedited")
            CurrentDataTable.Rows.Add("1030", "Xpresspost")
            CurrentDataTable.Rows.Add("1040", "Priority Courier")
            CurrentDataTable.Rows.Add("2005", "Small Packets Surface")
            CurrentDataTable.Rows.Add("2015", "Small Packets Air")
            CurrentDataTable.Rows.Add("2020", "Expedited US Business Contract")
            CurrentDataTable.Rows.Add("2025", "Expedited US Commercial")
            CurrentDataTable.Rows.Add("2030", "Xpresspost USA")
            CurrentDataTable.Rows.Add("2040", "Purolator International")
            CurrentDataTable.Rows.Add("2050", "Puropak International")
            CurrentDataTable.Rows.Add("3005", "Small Packets Surface")
            CurrentDataTable.Rows.Add("3010", "Small Packets International")
            CurrentDataTable.Rows.Add("3015", "Small Packets Air")
            CurrentDataTable.Rows.Add("3020", "Air International")
            CurrentDataTable.Rows.Add("3025", "Xpresspost International")
            CurrentDataTable.Rows.Add("3040", "Purolator International")
            CurrentDataTable.Rows.Add("3050", "Puropak International")

            Return CurrentDataTable

        End Function

    End Class

End Namespace