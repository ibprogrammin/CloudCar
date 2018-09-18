Imports System.Xml
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce.PaymentProcessing

Namespace CCFramework.Commerce.BusinessProcess.PaymentProcessing

    Public Class BeanStreamTransaction
        Public TransactionAmount As Decimal
        Public Name As String
        Public PhoneNumber As String
        Public Address1 As String
        Public Address2 As String
        Public City As String
        Public ProvinceCode As String
        Public PostalCode As String
        Public CountryCode As String
        Public Email As String
        Public OrderNumber As String
        Public CardNumber As String
        Public CardOwner As String
        Public ExpMonth As Integer
        Public ExpYear As Integer
        Public CVD As Integer

        Public Sub New(ByVal _amount As Decimal, ByVal _name As String, ByVal _phoneNumber As String, ByVal _address1 As String, ByVal _address2 As String, ByVal _city As String, ByVal _provinceCode As String, ByVal _postalCode As String, _
                       ByVal _countryCode As String, ByVal _email As String, ByVal _orderNumber As String, ByVal _cardNumber As String, ByVal _cardOwner As String, ByVal _expMonth As Integer, ByVal _expYear As Integer, ByVal _cvd As Integer)
            TransactionAmount = _amount
            Name = _name
            PhoneNumber = _phoneNumber
            Address1 = _address1
            Address2 = _address2
            City = _city
            ProvinceCode = _provinceCode
            PostalCode = _postalCode
            CountryCode = _countryCode
            Email = _email
            OrderNumber = _orderNumber
            CardNumber = _cardNumber
            CardOwner = _cardOwner
            ExpMonth = _expMonth
            ExpYear = _expYear
            CVD = _cvd
        End Sub

        Public Recuring As Boolean = False 'Pass as a 1 or 0 through the request
        Public BillingPeriod As Char 'Set to D/W/M/Y - Days/Weeks/Months/Years
        Public BillingIncrement As Integer
        Public FirstBilling As String 'Set to MMDDYYYY
        Public SecondBilling As String 'Set to MMDDYYYY
        Public Charge As Boolean
        Public Expiry As String 'Set to MMDDYYYY
        Public EndMonth As Boolean
        Public ApplyTax1 As Boolean
        Public ApplyTax2 As Boolean

        Public Sub New(ByVal _amount As Decimal, ByVal _name As String, ByVal _phoneNumber As String, ByVal _address1 As String, ByVal _address2 As String, ByVal _city As String, ByVal _provinceCode As String, ByVal _postalCode As String, _
                       ByVal _countryCode As String, ByVal _email As String, ByVal _orderNumber As String, ByVal _cardNumber As String, ByVal _cardOwner As String, ByVal _expMonth As Integer, ByVal _expYear As Integer, ByVal _cvd As Integer, _
                       ByVal _recurring As Boolean, ByVal _billingPeriod As Char, ByVal _billingIncrement As Integer, ByVal _firstBilling As String, ByVal _SecondBilling As String, ByVal _charge As Boolean, ByVal _expiry As String, ByVal _endMonth As Boolean, ByVal _applyTax1 As Boolean, ByVal _applyTax2 As Boolean)

            TransactionAmount = _amount
            Name = _name
            PhoneNumber = _phoneNumber
            Address1 = _address1
            Address2 = _address2
            City = _city
            ProvinceCode = _provinceCode
            PostalCode = _postalCode
            CountryCode = _countryCode
            Email = _email
            OrderNumber = _orderNumber
            CardNumber = _cardNumber
            CardOwner = _cardOwner
            ExpMonth = _expMonth
            ExpYear = _expYear
            CVD = _cvd

            Recuring = _recurring
            BillingPeriod = _billingPeriod
            BillingIncrement = _billingIncrement
            FirstBilling = _firstBilling
            SecondBilling = _SecondBilling
            Charge = _charge
            Expiry = _expiry
            EndMonth = _endMonth
            ApplyTax1 = _applyTax1
            ApplyTax2 = _applyTax2
        End Sub

        Public Sub SetRecurring(ByVal _recurring As Boolean, ByVal _billingPeriod As Char, ByVal _billingIncrement As Integer, ByVal _firstBilling As String, ByVal _SecondBilling As String, ByVal _charge As Boolean, ByVal _expiry As String, ByVal _endMonth As Boolean, ByVal _applyTax1 As Boolean, ByVal _applyTax2 As Boolean)
            Recuring = _recurring
            BillingPeriod = _billingPeriod
            BillingIncrement = _billingIncrement
            FirstBilling = _firstBilling
            SecondBilling = _SecondBilling
            Charge = _charge
            Expiry = _expiry
            EndMonth = _endMonth
            ApplyTax1 = _applyTax1
            ApplyTax2 = _applyTax2
        End Sub

    End Class

    Public Class BeanStreamReturnMessage
        Public Status As PaymentStatus
        Public Message As String
        Public XMLResponse As XmlDocument
        Private _chargeAmount As Decimal

        Public Sub New(ByVal _Status As PaymentStatus, ByVal _Message As String, ByVal _XMLResponse As XmlDocument, ByVal _transactionAmount As Decimal)
            Status = _Status
            Message = _Message
            XMLResponse = _XMLResponse
            _chargeAmount = _transactionAmount
        End Sub

        Public ReadOnly Property AuthorizationCode() As String
            Get
                Return BeanStreamClient.GetXmlElement(XMLResponse, "trnAuthCode")
            End Get
        End Property

        Public ReadOnly Property ResponseType() As String
            Get
                Return BeanStreamClient.GetXmlElement(XMLResponse, "responseType")
            End Get
        End Property

        Public ReadOnly Property PageContents() As String
            Get
                Return BeanStreamClient.GetXmlElement(XMLResponse, "pageContents")
            End Get
        End Property

        Public ReadOnly Property MessageID() As Integer
            Get
                Dim _messageID As Integer

                If Integer.TryParse(BeanStreamClient.GetXmlElement(XMLResponse, "messageId"), _messageID) Then
                    Return _messageID
                Else
                    Return -1
                End If
            End Get
        End Property

        Public ReadOnly Property MessageText() As String
            Get
                Return BeanStreamClient.GetXmlElement(XMLResponse, "messageText")
            End Get
        End Property

        Public ReadOnly Property ProcessDate() As String
            Get
                Return BeanStreamClient.GetXmlElement(XMLResponse, "trnDate")
            End Get
        End Property

        Public ReadOnly Property TransactionID() As String
            Get
                Return BeanStreamClient.GetXmlElement(XMLResponse, "trnId")
            End Get
        End Property

        Public ReadOnly Property ChargeAmount() As Decimal
            Get
                Return _chargeAmount
            End Get
        End Property

    End Class

    Public Class BeanStreamClient

        Private Details As BeanStreamTransaction

        Public Sub New(ByVal TransactionDetails As BeanStreamTransaction)
            Details = TransactionDetails
        End Sub

        Public Function ProcessRequest() As BeanStreamReturnMessage
            Dim objTrans As Beanstream.ProcessTransaction
            Dim xmlDocRsp As XmlDocument
            Dim trnRequest As String
            Dim trnResponse As String
            Dim errorFields() As String

            Try
                'Build the xml formatted transaction request message
                'trnRequest = Request("trnAmount")
                trnRequest = BuildXmlTransactionRequest(Details)

                'Create an instance of the Beanstream SOAP transaction web service object
                objTrans = New Beanstream.ProcessTransaction()

                'Submit the transaction request to Beanstream SOAP transaction web service
                trnResponse = objTrans.TransactionProcess(trnRequest)

                'DEBUG: Uncomment the following two lines to display the transaction request and response strings to the browser.
                'lblRequest.Text = "<b>Request:</b><br>" & Server.HtmlEncode(trnRequest)
                'lblResponse.Text = "<b>Response:</b><br>" & Server.HtmlEncode(trnResponse)

                'Load the xml formated transaction response in the xml document response object
                xmlDocRsp = New XmlDocument()
                xmlDocRsp.LoadXml(trnResponse)

                Dim returnMessage As BeanStreamReturnMessage

                Select Case GetXmlElement(xmlDocRsp, "errorType")
                    Case "N"
                        'No errors returned in the response - Transaction has been processed successfully
                        If GetXmlElement(xmlDocRsp, "trnApproved") = "1" Then
                            'Transaction has been approved, redirect to the approval page passing all response parameters
                            'Response.Redirect("PaymentApproved.aspx?trnId=" & GetXmlElement(xmlDocRsp, "trnId") & "&trnOrderNumber=" & Server.UrlEncode(Request("trnOrderNumber")) & "&authCode=" & GetXmlElement(xmlDocRsp, "trnAuthCode"))
                            returnMessage = New BeanStreamReturnMessage(PaymentStatus.Approved, "The transaction has been approved", xmlDocRsp, Details.TransactionAmount)
                        Else
                            'Transaction has been declined, redirect to the decline page passing all response parameters
                            'Response.Redirect("PaymentDeclined.aspx?messageText=" & Server.UrlEncode(GetXmlElement(xmlDocRsp, "messageText")))
                            returnMessage = New BeanStreamReturnMessage(PaymentStatus.Declined, "The transaction has been declined", xmlDocRsp, Details.TransactionAmount)
                        End If
                    Case "U"
                        'User generated error detected due to data validation checks. User must correct the indicated problem and resubmit their request.
                        errorFields = Split(GetXmlElement(xmlDocRsp, "messageText"), ",")

                        Dim errorMessage As String

                        errorMessage = "<b>Please correct the following fields:</b><ul>"

                        For i As Integer = 0 To UBound(errorFields)
                            errorMessage &= "<li>" & errorFields(i) & "</li>"
                        Next

                        errorMessage &= "<ul>"

                        returnMessage = New BeanStreamReturnMessage(PaymentStatus.UserError, errorMessage, xmlDocRsp, Details.TransactionAmount)
                    Case "S"
                        'System generated error detected due to merchant integration. Merchant must investigate the problem and correct their integration.
                        Dim errorMessage As String
                        errorMessage = "<b>A System Error Occured:</b> " & GetXmlElement(xmlDocRsp, "messageText") & " Please contact <a href='mailto:" & Settings.SupportEmail & "'>" & Settings.SupportEmail & "</a>"

                        returnMessage = New BeanStreamReturnMessage(PaymentStatus.SystemError, errorMessage, xmlDocRsp, Details.TransactionAmount)
                    Case Else
                        'This condition may occur in cases where a response could not be received from the payment gateway.
                        Dim errorMessage As String
                        errorMessage = "<b>Unexpected Response:</b> An unexpected response has been received processing your request.  Please contact <a href='mailto:" & Settings.SupportEmail & "'>" & Settings.SupportEmail & "</a>"

                        returnMessage = New BeanStreamReturnMessage(PaymentStatus.Invalid, errorMessage, xmlDocRsp, Details.TransactionAmount)
                End Select

                'Destroy the Beanstream SOAP transaction web service object
                objTrans = Nothing

                Return returnMessage
            Catch ex As Exception
                'Failed to submit the transaction request to the Beanstream
                'lblErrorMessage.Text = "<b>Unable to process payment:</b> " & ex.Message & "<br><br>"
                Throw New Exception("Unable to process Bean Stream Transaction Exception")
            End Try
        End Function

        Private Function BuildXmlTransactionRequest(ByVal Details As BeanStreamTransaction) As String

            Dim xmlDoc As New XmlDocument()
            Dim xmlRoot As XmlNode

            xmlDoc.LoadXml("<transaction></transaction>")
            xmlRoot = xmlDoc.DocumentElement

            Call AddXmlElement(xmlDoc, xmlRoot, "serviceVersion", Settings.BSServiceVersion)
            Call AddXmlElement(xmlDoc, xmlRoot, "merchant_id", Settings.BSMerchantID)
            Call AddXmlElement(xmlDoc, xmlRoot, "trnAmount", Details.TransactionAmount.ToString)
            Call AddXmlElement(xmlDoc, xmlRoot, "ordName", Details.Name)
            Call AddXmlElement(xmlDoc, xmlRoot, "ordPhoneNumber", Details.PhoneNumber)
            Call AddXmlElement(xmlDoc, xmlRoot, "ordAddress1", Details.Address1)
            Call AddXmlElement(xmlDoc, xmlRoot, "ordAddress2", Details.Address2)
            Call AddXmlElement(xmlDoc, xmlRoot, "ordCity", Details.City)
            Call AddXmlElement(xmlDoc, xmlRoot, "ordProvince", Details.ProvinceCode)
            Call AddXmlElement(xmlDoc, xmlRoot, "ordPostalCode", Details.PostalCode)
            Call AddXmlElement(xmlDoc, xmlRoot, "ordCountry", Details.CountryCode)
            Call AddXmlElement(xmlDoc, xmlRoot, "ordEmailAddress", Details.Email)
            Call AddXmlElement(xmlDoc, xmlRoot, "trnOrderNumber", Details.OrderNumber)
            Call AddXmlElement(xmlDoc, xmlRoot, "trnAmount", Details.TransactionAmount.ToString)
            Call AddXmlElement(xmlDoc, xmlRoot, "trnCardOwner", Details.Name)
            Call AddXmlElement(xmlDoc, xmlRoot, "trnCardNumber", Details.CardNumber)
            Call AddXmlElement(xmlDoc, xmlRoot, "trnExpMonth", Details.ExpMonth.ToString.PadLeft(2, "0"c))
            Call AddXmlElement(xmlDoc, xmlRoot, "trnExpYear", Details.ExpYear.ToString.PadLeft(2, "0"c))
            Call AddXmlElement(xmlDoc, xmlRoot, "trnCardCvd", Details.CVD.ToString)

            'The return address for verified by visa.
            If Settings.EnableSSL Then
                Call AddXmlElement(xmlDoc, xmlRoot, "termUrl", "https://" & Settings.HostName & Settings.BSTermURL)
            Else
                Call AddXmlElement(xmlDoc, xmlRoot, "termUrl", "http://" & Settings.HostName & Settings.BSTermURL)
            End If

            'Recurring Billing Variables
            If Details.Recuring Then
                Call AddXmlElement(xmlDoc, xmlRoot, "trnRecurring", "1")
                Call AddXmlElement(xmlDoc, xmlRoot, "rbBillingPeriod", Details.BillingPeriod)
                Call AddXmlElement(xmlDoc, xmlRoot, "rbBillingIncrement", Nothing)
                Call AddXmlElement(xmlDoc, xmlRoot, "rbFirstBilling", Nothing)
                Call AddXmlElement(xmlDoc, xmlRoot, "rbSecondBilling", Nothing)
                Call AddXmlElement(xmlDoc, xmlRoot, "rbCharge", Nothing)
                Call AddXmlElement(xmlDoc, xmlRoot, "rbExpiry", Nothing)
                Call AddXmlElement(xmlDoc, xmlRoot, "rbEndMonth", Nothing)
                Call AddXmlElement(xmlDoc, xmlRoot, "rbApplyTax1", Nothing)
                Call AddXmlElement(xmlDoc, xmlRoot, "rbApplyTax2", Nothing)
            End If

            BuildXmlTransactionRequest = xmlDoc.InnerXml

            xmlDoc = Nothing

        End Function

        Private Sub AddXmlElement(ByRef xmlDoc As XmlDocument, ByRef xmlRoot As XmlNode, ByVal eleName As String, ByVal eleValue As String)
            Dim xmlElem As XmlElement

            xmlElem = xmlDoc.CreateElement(eleName)
            xmlElem.InnerText = eleValue
            xmlRoot.AppendChild(xmlElem)
        End Sub

        Public Shared Function GetXmlElement(ByRef objXml As XmlDocument, ByVal eleName As String) As String

            Dim objLst As XmlNodeList

            objLst = objXml.GetElementsByTagName(eleName)
            If objLst.Count > 0 Then
                GetXmlElement = objLst.Item(0).InnerText
            Else
                Return "Invalid"
            End If

        End Function

    End Class

End Namespace