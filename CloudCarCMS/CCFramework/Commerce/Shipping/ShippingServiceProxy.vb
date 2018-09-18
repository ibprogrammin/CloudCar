Imports System.IO
Imports System.Net

Namespace CCFramework.Commerce.Shipping

    Public MustInherit Class ShippingServiceProxy

        'TODO Make this a setting
        Protected Const _DaysToShip As Integer = 2

        Protected _RequestAddress As String
        Protected _SchemaFile As String

        Protected _ShippingAuthentication As ShippingAuthentication
        Protected _ShipToAddress As ShippingAddress
        Protected _ShipFromAddress As ShippingAddress
        Protected _ShippingItems As List(Of ShippingItem)
        Protected _ShippingRates As List(Of ShippingRate)
        
        Protected _StatusMessage As String

        Protected _AppendToRequest As String = ""

        Protected _RequestXmlMessage As String
        Protected _ResponseXmlMessage As String

        Protected _RatesDataSet As DataSet
        Protected _DataSetReadMode As XmlReadMode = XmlReadMode.Auto

        Protected MustOverride Sub BuildRequestXml()
        Protected MustOverride Sub LoadShippingRates()

        Protected Sub New(ByVal RequestAddress As String, ByVal SchemaFile As String)
            _RequestAddress = RequestAddress
            _SchemaFile = SchemaFile

            _ShippingItems = New List(Of ShippingItem)
            _ShippingRates = New List(Of ShippingRate)
        End Sub

        Public Sub SetAuthentication(ByVal Authentication As ShippingAuthentication)
            _ShippingAuthentication = Authentication
        End Sub

        Public Sub AddShippingItem(ByVal NewShippingItem As ShippingItem)
            _ShippingItems.Add(NewShippingItem)
        End Sub

        Protected Sub AddShippingRate(ByVal NewShippingRate As ShippingRate)
            _ShippingRates.Add(NewShippingRate)
        End Sub

        Public Sub SetShipToAddress(ByVal Address As ShippingAddress)
            _ShipToAddress = Address
        End Sub

        Public Sub SetShipFromAddress(ByVal Address As ShippingAddress)
            _ShipFromAddress = Address
        End Sub

        Protected Overridable Sub SubmitRequest()
            Dim CurrentRequestString As String = String.Format("{0}{1}", _AppendToRequest, _RequestXmlMessage)
            'CurrentRequestString = HttpUtility.UrlEncode(CurrentRequestString)

            Dim CurrentRequestData As Byte() = ASCIIEncoding.ASCII.GetBytes(CurrentRequestString)
            Dim CurrentWebRequest As HttpWebRequest = CType(HttpWebRequest.Create(New Uri(_RequestAddress)), HttpWebRequest)
            'Dim CurrentWebRequest As HttpWebRequest = HttpWebRequest.Create(_RequestAddress)

            'CurrentWebRequest.Timeout = 8000
            CurrentWebRequest.Method = "POST"
            CurrentWebRequest.KeepAlive = True
            CurrentWebRequest.ContentType = "application/x-www-form-urlencoded"
            CurrentWebRequest.ContentLength = CurrentRequestData.Length
            'wr.AuthenticationLevel = Net.Security.AuthenticationLevel.MutualAuthRequired

            Try
                Dim SendStream As Stream = CurrentWebRequest.GetRequestStream()
                SendStream.Write(CurrentRequestData, 0, CurrentRequestData.Length)
                SendStream.Close()

                CurrentWebRequest.BeginGetResponse(AddressOf ResponseCallback, CurrentWebRequest)

                'Dim CurrentWebResponse As HttpWebResponse = CurrentWebRequest.GetResponse()

                'Using CurrentStreamReader As StreamReader = New StreamReader(CurrentWebResponse.GetResponseStream())
                '    _ResponseXmlMessage = CurrentStreamReader.ReadToEnd()
                '    CurrentStreamReader.Close()
                'End Using

                'CurrentWebResponse.Close()

            Catch CurrentException As Exception
                _ResponseXmlMessage = CurrentException.Message
            End Try

        End Sub

        Private Sub ResponseCallback(ByVal AsynchronousResult As IAsyncResult)
            Dim CurrentWebRequest As HttpWebRequest = CType(AsynchronousResult.AsyncState, HttpWebRequest)

            Dim CurrentWebResponse As HttpWebResponse = CType(CurrentWebRequest.EndGetResponse(AsynchronousResult), HttpWebResponse)

            Using CurrentStreamReader As StreamReader = New StreamReader(CurrentWebResponse.GetResponseStream())
                _ResponseXmlMessage = CurrentStreamReader.ReadToEnd()
                CurrentStreamReader.Close()
            End Using

            CurrentWebResponse.Close()
        End Sub

        Protected Overridable Sub ReadResponseToDataSet()
            Dim CurrentXmlStringReader As StringReader = New StringReader(_ResponseXmlMessage)

            _RatesDataSet = New DataSet
            _RatesDataSet.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema
            _RatesDataSet.Namespace = _SchemaFile
            _RatesDataSet.ReadXml(CurrentXmlStringReader, _DataSetReadMode)
        End Sub

        Protected Overridable Function CheckIfOkToProcess() As Boolean
            If Not _ShippingAuthentication Is Nothing And Not _ShipFromAddress Is Nothing And Not _ShipToAddress Is Nothing And _ShippingItems.Count > 0 Then
                _StatusMessage = "SUCCESS"

                Return True
            Else
                _StatusMessage = "ERROR PROCCESSING"

                Return False
            End If
        End Function

        Public Sub ProcessRateRequest()
            If CheckIfOkToProcess() Then
                BuildRequestXml()
                SubmitRequest()
                ReadResponseToDataSet()
                LoadShippingRates()
            End If
        End Sub

        Public ReadOnly Property CheckStatus As String
            Get
                Return _StatusMessage
            End Get
        End Property

        Public ReadOnly Property RateItems As List(Of ShippingRate)
            Get
                Return _ShippingRates
            End Get
        End Property

        Public ReadOnly Property ShippingItems As List(Of ShippingItem)
            Get
                Return _ShippingItems
            End Get
        End Property

        Public ReadOnly Property RequestMessage As String
            Get
                Return _RequestXmlMessage
            End Get
        End Property

        Public ReadOnly Property ResponseMessage As String
            Get
                Return _ResponseXmlMessage
            End Get
        End Property

    End Class

End Namespace