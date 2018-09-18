Imports CloudCar.CCFramework.Commerce.Shipping.PurolatorService
Imports System.Net
Imports CloudCar.CCFramework.Core

Namespace CCFramework.Commerce.Shipping.Purolator

    Public Class PurolatorServiceProxy
        Inherits ShippingServiceProxy

        Private ReadOnly _Service As EstimatingService
        Private ReadOnly _Request As GetFullEstimateRequestContainer
        Private _Response As GetFullEstimateResponseContainer
        Private ReadOnly _Shipment As Shipment
        Private ReadOnly _Pieces As List(Of Piece)
        Private ReadOnly _ServiceType As PurolatorServiceType
        Private ReadOnly _PickupType As PurolatorService.PickupType

        Private ReadOnly _WeightUnit As PurolatorService.WeightUnit
        Private ReadOnly _DimensionUnit As PurolatorService.DimensionUnit

        Private ReadOnly _ConfirmationEmail As String
        Private ReadOnly _Description As String

        Public Sub New(ByVal ConfirmationEmail As String, ByVal Description As String, ByVal WeightUnit As PurolatorService.WeightUnit, ByVal DimensionUnit As PurolatorService.DimensionUnit, ByVal ServiceType As PurolatorServiceType, ByVal PickupType As PickupType)
            MyBase.New("", "")

            _Service = New EstimatingService()
            _Request = New GetFullEstimateRequestContainer()
            _Response = New GetFullEstimateResponseContainer()
            _Shipment = New Shipment()
            _Pieces = New List(Of Piece)

            _ConfirmationEmail = ConfirmationEmail
            _Description = Description

            _WeightUnit = WeightUnit
            _DimensionUnit = DimensionUnit

            _ServiceType = ServiceType
            _PickupType = PickupType
        End Sub

        Protected Overrides Sub BuildRequestXml()

        End Sub

        Protected Overrides Sub SubmitRequest()
            SetAccessInformation()
            SetSenderInformation()
            SetRecieverInformation()
            ProcessShippingItems()

            If _Shipment.PackageInformation Is Nothing Then
                SetPackageInformation()
                SetShippmentInformation()
            End If

            _Service.RequestContextValue = New RequestContext
            _Service.RequestContextValue.Version = "1.3"
            _Service.RequestContextValue.Language = PurolatorService.Language.en
            _Service.RequestContextValue.GroupID = ""
            _Service.RequestContextValue.RequestReference = "RequestReference"

            _Request.Shipment = _Shipment
        End Sub

        Protected Overrides Sub ReadResponseToDataSet()

        End Sub

        Protected Overrides Sub LoadShippingRates()
            _Response = _Service.GetFullEstimate(_Request)

            'Try
            Dim CurrentSequence As Integer = 1

            If Not _Response.ShipmentEstimates Is Nothing Then
                For Each CurrentItem As ShipmentEstimate In _Response.ShipmentEstimates
                    Dim CurrentRate As ShippingRate

                    CurrentRate = New ShippingRate(CurrentSequence, GetPurolatorServiceTypeFormated(_ServiceType), "Purolator", If(CurrentItem.EstimatedTransitDaysSpecified, String.Format("Estimate transit days: {0}", CurrentItem.EstimatedTransitDays), ""), CDbl(CurrentItem.BasePrice), CurrentItem.ShipmentDate, CurrentItem.ExpectedDeliveryDate)

                    AddShippingRate(CurrentRate)

                    CurrentSequence += 1
                Next
            End If

            If Not _Response.ReturnShipmentEstimates Is Nothing Then
                For Each CurrentItem As ShipmentEstimate In _Response.ReturnShipmentEstimates
                    Dim CurrentRate As ShippingRate

                    CurrentRate = New ShippingRate(CurrentSequence, GetPurolatorServiceTypeFormated(_ServiceType), "Purolator", If(CurrentItem.EstimatedTransitDaysSpecified, String.Format("Estimate transit days: {0}", CurrentItem.EstimatedTransitDays), ""), CDbl(CurrentItem.BasePrice) * Settings.ShippingMargin, CurrentItem.ShipmentDate, CurrentItem.ExpectedDeliveryDate)

                    AddShippingRate(CurrentRate)

                    CurrentSequence += 1
                Next
            End If

            If Not _Response.ResponseInformation.InformationalMessages Is Nothing Then
                For Each item As InformationalMessage In _Response.ResponseInformation.InformationalMessages
                    _StatusMessage &= item.Code & " " & item.Message
                Next
            End If
            
            If Not _Response.ResponseInformation.Errors Is Nothing Then
                For Each item As PurolatorService.Error In _Response.ResponseInformation.Errors
                    _StatusMessage &= item.Code & " " & item.Description & " " & item.AdditionalInformation
                Next
            End If
            
            '_StatusMessage = "SUCCESS"
            'Catch Exception As Exception
            '_StatusMessage = "There was an error processing the response from purolator"
            'End Try
        End Sub

        Private Sub SetAccessInformation()
            _Service.Credentials = New NetworkCredential(_ShippingAuthentication.AuthenticationKey, _ShippingAuthentication.PassWord)
        End Sub

        Private Sub SetSenderInformation()
            Dim CurrentAddress As New Address

            Dim CurrentStreetAddress As StreetAddress = GetStreetAddress(_ShipFromAddress.FirstAddressLine)

            CurrentAddress.Name = _ShippingAuthentication.AttentionName
            CurrentAddress.Company = _ShippingAuthentication.CompanyName
            CurrentAddress.Department = ""
            CurrentAddress.StreetNumber = CurrentStreetAddress.StreetNumber
            CurrentAddress.StreetSuffix = ""
            CurrentAddress.StreetName = CurrentStreetAddress.StreetName
            CurrentAddress.StreetType = CurrentStreetAddress.StreetType
            CurrentAddress.StreetDirection = ""
            CurrentAddress.Suite = ""
            CurrentAddress.Floor = ""
            CurrentAddress.StreetAddress2 = ""
            CurrentAddress.StreetAddress3 = ""
            CurrentAddress.City = _ShipFromAddress.City
            CurrentAddress.Province = _ShipFromAddress.ProvinceOrStateCode
            CurrentAddress.Country = _ShipFromAddress.CountryCode
            CurrentAddress.PostalCode = _ShipFromAddress.PostalCode
            CurrentAddress.PhoneNumber = GetPhoneNumber(_ShippingAuthentication.PhoneNumber)
            CurrentAddress.FaxNumber = GetPhoneNumber(_ShippingAuthentication.FaxNumber)

            _Shipment.SenderInformation = New SenderInformation()
            _Shipment.SenderInformation.Address = CurrentAddress
            _Shipment.SenderInformation.TaxNumber = "123456"
        End Sub

        Private Sub SetRecieverInformation()
            Dim CurrentAddress As New Address

            Dim CurrentStreetAddress As StreetAddress = GetStreetAddress(_ShipToAddress.FirstAddressLine)

            CurrentAddress.Name = "Default Name"
            CurrentAddress.Company = "Default Company"
            CurrentAddress.Department = ""
            CurrentAddress.StreetNumber = CurrentStreetAddress.StreetNumber
            CurrentAddress.StreetSuffix = ""
            CurrentAddress.StreetName = CurrentStreetAddress.StreetName
            CurrentAddress.StreetType = CurrentStreetAddress.StreetType
            CurrentAddress.StreetDirection = ""
            CurrentAddress.Suite = ""
            CurrentAddress.Floor = ""
            CurrentAddress.StreetAddress2 = ""
            CurrentAddress.StreetAddress3 = ""
            CurrentAddress.City = _ShipToAddress.City
            CurrentAddress.Province = _ShipToAddress.ProvinceOrStateCode
            CurrentAddress.Country = _ShipToAddress.CountryCode
            CurrentAddress.PostalCode = _ShipToAddress.PostalCode
            CurrentAddress.PhoneNumber = GetPhoneNumber("15555555555")
            CurrentAddress.FaxNumber = GetPhoneNumber("15555555555")

            _Shipment.ReceiverInformation = New ReceiverInformation()
            _Shipment.ReceiverInformation.Address = CurrentAddress
            _Shipment.ReceiverInformation.TaxNumber = "123456"
        End Sub

        Public Sub ProcessShippingItems()
            For Each CurrentShippingItem As ShippingItem In _ShippingItems
                For CurrentIndex As Integer = 1 To CurrentShippingItem.Quantity
                    Dim CurrentPiece As New Piece()

                    CurrentPiece.Weight = New Weight()

                    Select Case _WeightUnit
                        Case PurolatorService.WeightUnit.kg
                            If CurrentShippingItem.Weight < 0.4 Then
                                CurrentPiece.Weight.Value = 0.4D
                            Else
                                CurrentPiece.Weight.Value = CDec(CurrentShippingItem.Weight)
                            End If
                        Case PurolatorService.WeightUnit.lb
                            If CurrentShippingItem.Weight < 1 Then
                                CurrentPiece.Weight.Value = 1
                            Else
                                CurrentPiece.Weight.Value = CDec(CurrentShippingItem.Weight)
                            End If
                    End Select

                    CurrentPiece.Weight.WeightUnit = _WeightUnit

                    CurrentPiece.Length = SetPackageDimension(CInt(CurrentShippingItem.Length), _DimensionUnit)
                    CurrentPiece.Width = SetPackageDimension(CInt(CurrentShippingItem.Width), _DimensionUnit)
                    CurrentPiece.Height = SetPackageDimension(CInt(CurrentShippingItem.Height), _DimensionUnit)

                    _Pieces.Add(CurrentPiece)
                Next CurrentIndex
            Next

            '_pieces(_packages.IndexOf(package)) = piece
            '_shipment.PackageInformation.PiecesInformation(_packages.IndexOf(package)) = piece
        End Sub

        Private Function SetPackageDimension(ByVal Value As Integer, ByVal Unit As PurolatorService.DimensionUnit) As Dimension
            Dim CurrentDimension As New Dimension

            CurrentDimension.Value = Value
            CurrentDimension.DimensionUnit = Unit

            Return CurrentDimension
        End Function

        Private Sub SetShippmentInformation()
            _Shipment.PaymentInformation = New PaymentInformation()
            _Shipment.PaymentInformation.PaymentType = PaymentType.Sender
            _Shipment.PaymentInformation.RegisteredAccountNumber = _ShippingAuthentication.UserName
            _Shipment.PaymentInformation.BillingAccountNumber = _ShippingAuthentication.UserName

            _Shipment.PickupInformation = New PickupInformation()
            _Shipment.PickupInformation.PickupType = _PickupType

            _Shipment.NotificationInformation = New NotificationInformation()
            _Shipment.NotificationInformation.ConfirmationEmailAddress = _ConfirmationEmail

            _Shipment.TrackingReferenceInformation = New TrackingReferenceInformation()
            'Optional
            _Shipment.TrackingReferenceInformation.Reference1 = "Reference1"
            _Shipment.TrackingReferenceInformation.Reference2 = "Reference2"
            _Shipment.TrackingReferenceInformation.Reference3 = "Reference3"
            _Shipment.TrackingReferenceInformation.Reference4 = "Reference4"
        End Sub

        Private Sub SetPackageInformation()
            _Shipment.PackageInformation = New PackageInformation
            _Shipment.PackageInformation.ServiceID = GetPurolatorServiceTypeString(_ServiceType)
            _Shipment.PackageInformation.Description = _Description

            _Shipment.PackageInformation.PiecesInformation = _Pieces.ToArray

            _Shipment.PackageInformation.TotalPieces = _Pieces.Count
            _Shipment.PackageInformation.TotalWeight = New TotalWeight
            _Shipment.PackageInformation.TotalWeight.Value = GetTotalWeight()
            _Shipment.PackageInformation.TotalWeight.WeightUnit = _WeightUnit

            _Shipment.PackageInformation.DangerousGoodsDeclarationDocumentIndicator = False

            '_shipment.PackageInformation.OptionsInformation = GetSignatureInformation("ResidentialSignatureDomestic")
            _Shipment.PackageInformation.OptionsInformation = GetSignatureInformation("OriginSignatureNotRequired")
        End Sub

        Public Function GetTotalWeight() As Integer
            Dim TotalWeight As Decimal = 0

            For Each Item As Piece In _Pieces
                TotalWeight += CDec(Item.Weight.Value)
            Next

            Return CInt(Math.Ceiling(TotalWeight))
        End Function

        Public Function GetSignatureInformation(ByVal Id As String) As OptionsInformation
            Dim CurrentOptionsInformation As New OptionsInformation()
            Dim OptionIdValuePair As New OptionIDValuePair()

            OptionIdValuePair.ID = Id
            OptionIdValuePair.Value = "true"

            CurrentOptionsInformation.Options = New OptionIDValuePair() {OptionIdValuePair}

            Return CurrentOptionsInformation
        End Function

        Public Shared Function GetStreetAddress(ByVal Address As String) As StreetAddress
            Dim CurrentStreetAddress As New StreetAddress

            CurrentStreetAddress.StreetNumber = Address.Substring(0, Address.IndexOf(" ")).Trim()
            CurrentStreetAddress.StreetName = Address.Substring(Address.IndexOf(" ") + 1, Address.LastIndexOf(" ") - Address.IndexOf(" ")).Trim()
            CurrentStreetAddress.StreetType = Address.Substring(Address.LastIndexOf(" "), Address.Length - Address.LastIndexOf(" ")).Trim()

            Return CurrentStreetAddress
        End Function

        Public Shared Function GetPhoneNumber(ByVal InputPhoneNumber As String) As PhoneNumber
            InputPhoneNumber = String.Join(Nothing, Regex.Split(InputPhoneNumber, "[^\d]"))

            Dim CurrentPhoneNumber As New PhoneNumber

            Select Case InputPhoneNumber.Length
                Case 7
                    CurrentPhoneNumber.CountryCode = ""
                    CurrentPhoneNumber.AreaCode = ""
                    CurrentPhoneNumber.Phone = InputPhoneNumber.Substring(4, 7)
                    CurrentPhoneNumber.Extension = ""
                Case 10
                    CurrentPhoneNumber.CountryCode = ""
                    CurrentPhoneNumber.AreaCode = InputPhoneNumber.Substring(1, 3)
                    CurrentPhoneNumber.Phone = InputPhoneNumber.Substring(4, 7)
                    CurrentPhoneNumber.Extension = ""
                Case 11
                    CurrentPhoneNumber.CountryCode = InputPhoneNumber.Substring(0, 1)
                    CurrentPhoneNumber.AreaCode = InputPhoneNumber.Substring(1, 3)
                    CurrentPhoneNumber.Phone = InputPhoneNumber.Substring(4, 7)
                Case Is > 11
                    CurrentPhoneNumber.CountryCode = InputPhoneNumber.Substring(0, 1)
                    CurrentPhoneNumber.AreaCode = InputPhoneNumber.Substring(1, 3)
                    CurrentPhoneNumber.Phone = InputPhoneNumber.Substring(4, 7)
                    CurrentPhoneNumber.Extension = InputPhoneNumber.Substring(11, InputPhoneNumber.Length - 11)
                Case Else
                    CurrentPhoneNumber.CountryCode = ""
                    CurrentPhoneNumber.AreaCode = ""
                    CurrentPhoneNumber.Phone = InputPhoneNumber
                    CurrentPhoneNumber.Extension = ""
            End Select

            Return CurrentPhoneNumber
        End Function

        Public Shared Function GetPhoneNumber(ByVal CountryCode As String, ByVal AreaCode As String, ByVal PhoneNumber As String, ByVal Extension As String) As PhoneNumber
            Dim CurrentPhoneNumber As New PhoneNumber

            CurrentPhoneNumber.CountryCode = CountryCode
            CurrentPhoneNumber.AreaCode = AreaCode
            CurrentPhoneNumber.Phone = PhoneNumber
            CurrentPhoneNumber.Extension = Extension

            Return CurrentPhoneNumber
        End Function

        Public Shared Function GetPurolatorServiceTypeFormated(ByVal CurrentService As PurolatorServiceType) As String
            Select Case CurrentService
                Case PurolatorServiceType.PurolatorExpress
                    Return "Express"
                Case PurolatorServiceType.PurolatorExpress1030AM
                    Return "Express 10:30 AM"
                Case PurolatorServiceType.PurolatorExpress9AM
                    Return "Express 9 AM"
                Case PurolatorServiceType.PurolatorExpressBox
                    Return "Express Box"
                Case PurolatorServiceType.PurolatorExpressBox1030AM
                    Return "Express Box 10:30 AM"
                Case PurolatorServiceType.PurolatorExpressBox9AM
                    Return "Express Box 9 AM"
                Case PurolatorServiceType.PurolatorExpressBoxEvening
                    Return "Express Box Evening"
                Case PurolatorServiceType.PurolatorExpressBoxInternational
                    Return "Express Box International"
                Case PurolatorServiceType.PurolatorExpressBoxUS
                    Return "Express Box U.S."
                Case PurolatorServiceType.PurolatorExpressEnvelope
                    Return "Express Envelope"
                Case PurolatorServiceType.PurolatorExpressEnvelope1030AM
                    Return "Express Envelope 10:30 AM"
                Case PurolatorServiceType.PurolatorExpressEnvelope9AM
                    Return "Express Envelope 9 AM"
                Case PurolatorServiceType.PurolatorExpressEnvelopeEvening
                    Return "Express Evening"
                Case PurolatorServiceType.PurolatorExpressEnvelopeInternational
                    Return "Express International"
                Case PurolatorServiceType.PurolatorExpressEnvelopeUS
                    Return "Express Envelope U.S."
                Case PurolatorServiceType.PurolatorExpressEvening
                    Return "Express Evening"
                Case PurolatorServiceType.PurolatorExpressInternational
                    Return "Express International"
                Case PurolatorServiceType.PurolatorExpressInternational1030AM
                    Return "Express International 10:30 AM"
                Case PurolatorServiceType.PurolatorExpressInternational12
                    Return "Express International 12:00"
                Case PurolatorServiceType.PurolatorExpressInternational9AM
                    Return "Express International 9 AM"
                Case PurolatorServiceType.PurolatorExpressInternationalBox1030AM
                    Return "Express International Box 10:30 AM"
                Case PurolatorServiceType.PurolatorExpressInternationalBox12
                    Return "Express International Box 12:00"
                Case PurolatorServiceType.PurolatorExpressInternationalBox9AM
                    Return "Express International Box 9 AM"
                Case PurolatorServiceType.PurolatorExpressInternationalEnvelope1030AM
                    Return "Express International Envelope 10:30 AM"
                Case PurolatorServiceType.PurolatorExpressInternationalEnvelope12
                    Return "Express International Envelope 12:00"
                Case PurolatorServiceType.PurolatorExpressInternationalEnvelope9AM
                    Return "Express International Envelope 9 AM"
                Case PurolatorServiceType.PurolatorExpressInternationalPack1030AM
                    Return "Express International Pack 10:30 AM"
                Case PurolatorServiceType.PurolatorExpressInternationalPack12
                    Return "Express International Pack 12:00"
                Case PurolatorServiceType.PurolatorExpressInternationalPack9AM
                    Return "Express International Pack 9 AM"
                Case PurolatorServiceType.PurolatorExpressPack
                    Return "Express Pack"
                Case PurolatorServiceType.PurolatorExpressPack1030AM
                    Return "Express Pack 10:30 AM"
                Case PurolatorServiceType.PurolatorExpressPack9AM
                    Return "Express Pack 9 AM"
                Case PurolatorServiceType.PurolatorExpressPackEvening
                    Return "Express Pack Evening"
                Case PurolatorServiceType.PurolatorExpressPackInternational
                    Return "Express Pack International"
                Case PurolatorServiceType.PurolatorExpressPackUS
                    Return "Express Pack U.S."
                Case PurolatorServiceType.PurolatorExpressUS
                    Return "Express U.S."
                Case PurolatorServiceType.PurolatorExpressUS1030AM
                    Return "Express U.S. 10:30 AM"
                Case PurolatorServiceType.PurolatorExpressUS12
                    Return "Express U.S. 12:00"
                Case PurolatorServiceType.PurolatorExpressUS9AM
                    Return "Express U.S. 9 AM"
                Case PurolatorServiceType.PurolatorExpressUSBox1030AM
                    Return "Express U.S. Box 10:30 AM"
                Case PurolatorServiceType.PurolatorExpressUSBox12
                    Return "Express U.S. Box 12:00"
                Case PurolatorServiceType.PurolatorExpressUSBox9AM
                    Return "Express U.S. Box 9 AM"
                Case PurolatorServiceType.PurolatorExpressUSEnvelope1030AM
                    Return "Express U.S. Envelope 10:30 AM"
                Case PurolatorServiceType.PurolatorExpressUSEnvelope12
                    Return "Express U.S. Envelope 12:00"
                Case PurolatorServiceType.PurolatorExpressUSEnvelope9AM
                    Return "Express U.S. Envelope 9 AM"
                Case PurolatorServiceType.PurolatorExpressUSPack1030AM
                    Return "Express U.S. Pack 10:30 AM"
                Case PurolatorServiceType.PurolatorExpressUSPack12
                    Return "Express U.S. Pack 12:00"
                Case PurolatorServiceType.PurolatorExpressUSPack9AM
                    Return "Express U.S. Pack 9 AM"
                Case PurolatorServiceType.PurolatorGround
                    Return "Ground"
                Case PurolatorServiceType.PurolatorGround1030AM
                    Return "Ground 10:3 AM"
                Case PurolatorServiceType.PurolatorGround9AM
                    Return "Ground 9 AM"
                Case PurolatorServiceType.PurolatorGroundDistribution
                    Return "Ground Distribution"
                Case PurolatorServiceType.PurolatorGroundEvening
                    Return "Ground Evening"
                Case PurolatorServiceType.PurolatorGroundUS
                    Return "Ground U.S."
                Case Else
                    Return ""
            End Select
        End Function

        Public Shared Function GetPurolatorServiceTypeString(ByVal CurrentService As PurolatorServiceType) As String
            Select Case CurrentService
                Case PurolatorServiceType.PurolatorExpress
                    Return "PurolatorExpress"
                Case PurolatorServiceType.PurolatorExpress1030AM
                    Return "PurolatorExpress10:30AM"
                Case PurolatorServiceType.PurolatorExpress9AM
                    Return "PurolatorExpress9AM"
                Case PurolatorServiceType.PurolatorExpressBox
                    Return "PurolatorExpressBox"
                Case PurolatorServiceType.PurolatorExpressBox1030AM
                    Return "PurolatorExpressBox10:30AM"
                Case PurolatorServiceType.PurolatorExpressBox9AM
                    Return "PurolatorExpressBox9AM"
                Case PurolatorServiceType.PurolatorExpressBoxEvening
                    Return "PurolatorExpressBoxEvening"
                Case PurolatorServiceType.PurolatorExpressBoxInternational
                    Return "PurolatorExpressBoxInternational"
                Case PurolatorServiceType.PurolatorExpressBoxUS
                    Return "PurolatorExpressBoxU.S."
                Case PurolatorServiceType.PurolatorExpressEnvelope
                    Return "PurolatorExpressEnvelope"
                Case PurolatorServiceType.PurolatorExpressEnvelope1030AM
                    Return "PurolatorExpressEnvelope10:30AM"
                Case PurolatorServiceType.PurolatorExpressEnvelope9AM
                    Return "PurolatorExpressEnvelope9AM"
                Case PurolatorServiceType.PurolatorExpressEnvelopeEvening
                    Return "PurolatorExpressEvening"
                Case PurolatorServiceType.PurolatorExpressEnvelopeInternational
                    Return "PurolatorExpressInternational"
                Case PurolatorServiceType.PurolatorExpressEnvelopeUS
                    Return "PurolatorExpressEnvelopeU.S."
                Case PurolatorServiceType.PurolatorExpressEvening
                    Return "PurolatorExpressEvening"
                Case PurolatorServiceType.PurolatorExpressInternational
                    Return "PurolatorExpressInternational"
                Case PurolatorServiceType.PurolatorExpressInternational1030AM
                    Return "PurolatorExpressInternational10:30AM"
                Case PurolatorServiceType.PurolatorExpressInternational12
                    Return "PurolatorExpressInternational12:00"
                Case PurolatorServiceType.PurolatorExpressInternational9AM
                    Return "PurolatorExpressInternational9AM"
                Case PurolatorServiceType.PurolatorExpressInternationalBox1030AM
                    Return "PurolatorExpressInternationalBox10:30AM"
                Case PurolatorServiceType.PurolatorExpressInternationalBox12
                    Return "PurolatorExpressInternationalBox12:00"
                Case PurolatorServiceType.PurolatorExpressInternationalBox9AM
                    Return "PurolatorExpressInternationalBox9AM"
                Case PurolatorServiceType.PurolatorExpressInternationalEnvelope1030AM
                    Return "PurolatorExpressInternationalEnvelope10:30AM"
                Case PurolatorServiceType.PurolatorExpressInternationalEnvelope12
                    Return "PurolatorExpressInternationalEnvelope12:00"
                Case PurolatorServiceType.PurolatorExpressInternationalEnvelope9AM
                    Return "PurolatorExpressInternationalEnvelope9AM"
                Case PurolatorServiceType.PurolatorExpressInternationalPack1030AM
                    Return "PurolatorExpressInternationalPack10:30AM"
                Case PurolatorServiceType.PurolatorExpressInternationalPack12
                    Return "PurolatorExpressInternationalPack12:00"
                Case PurolatorServiceType.PurolatorExpressInternationalPack9AM
                    Return "PurolatorExpressInternationalPack9AM"
                Case PurolatorServiceType.PurolatorExpressPack
                    Return "PurolatorExpressPack"
                Case PurolatorServiceType.PurolatorExpressPack1030AM
                    Return "PurolatorExpressPack10:30AM"
                Case PurolatorServiceType.PurolatorExpressPack9AM
                    Return "PurolatorExpressPack9AM"
                Case PurolatorServiceType.PurolatorExpressPackEvening
                    Return "PurolatorExpressPackEvening"
                Case PurolatorServiceType.PurolatorExpressPackInternational
                    Return "PurolatorExpressPackInternational"
                Case PurolatorServiceType.PurolatorExpressPackUS
                    Return "PurolatorExpressPackU.S."
                Case PurolatorServiceType.PurolatorExpressUS
                    Return "PurolatorExpressU.S."
                Case PurolatorServiceType.PurolatorExpressUS1030AM
                    Return "PurolatorExpressU.S.10:30AM"
                Case PurolatorServiceType.PurolatorExpressUS12
                    Return "PurolatorExpressU.S.12:00"
                Case PurolatorServiceType.PurolatorExpressUS9AM
                    Return "PurolatorExpressU.S.9AM"
                Case PurolatorServiceType.PurolatorExpressUSBox1030AM
                    Return "PurolatorExpressU.S.Box10:30AM"
                Case PurolatorServiceType.PurolatorExpressUSBox12
                    Return "PurolatorExpressU.S.Box12:00"
                Case PurolatorServiceType.PurolatorExpressUSBox9AM
                    Return "PurolatorExpressU.S.Box9AM"
                Case PurolatorServiceType.PurolatorExpressUSEnvelope1030AM
                    Return "PurolatorExpressU.S.Envelope10:30AM"
                Case PurolatorServiceType.PurolatorExpressUSEnvelope12
                    Return "PurolatorExpressU.S.Envelope12:00"
                Case PurolatorServiceType.PurolatorExpressUSEnvelope9AM
                    Return "PurolatorExpressU.S.Envelope9AM"
                Case PurolatorServiceType.PurolatorExpressUSPack1030AM
                    Return "PurolatorExpressU.S.Pack10:30AM"
                Case PurolatorServiceType.PurolatorExpressUSPack12
                    Return "PurolatorExpressU.S.Pack12:00"
                Case PurolatorServiceType.PurolatorExpressUSPack9AM
                    Return "PurolatorExpressU.S.Pack9AM"
                Case PurolatorServiceType.PurolatorGround
                    Return "PurolatorGround"
                Case PurolatorServiceType.PurolatorGround1030AM
                    Return "PurolatorGround10:30AM"
                Case PurolatorServiceType.PurolatorGround9AM
                    Return "PurolatorGround9AM"
                Case PurolatorServiceType.PurolatorGroundDistribution
                    Return "PurolatorGroundDistribution"
                Case PurolatorServiceType.PurolatorGroundEvening
                    Return "PurolatorGroundEvening"
                Case PurolatorServiceType.PurolatorGroundUS
                    Return "PurolatorGroundU.S."
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Public Structure StreetAddress
        Public StreetNumber As String
        Public StreetName As String
        Public StreetType As String
    End Structure

End Namespace