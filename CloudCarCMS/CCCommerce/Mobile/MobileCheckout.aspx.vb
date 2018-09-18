Imports CloudCar.CCFramework.Commerce.BusinessProcess
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Core.Shipping
Imports CloudCar.CCFramework.Commerce.ShoppingCart
Imports CloudCar.CCFramework.Commerce.PaymentProcessing

Namespace CCCommerce.Mobile

    Partial Public Class MobileCheckout
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Settings.StoreEnabled Then
                Response.Redirect(Settings.StoreDisabledPage)
            End If

            If Not Page.IsPostBack Then
                'Reset the Purchase Session if this is the first time loading the page.
                Session("Purchase") = Nothing

                RefreshDataSources()

                Dim paymentType As EPaymentType = CType(Settings.DefaultPaymentType, EPaymentType)
                If paymentType = EPaymentType.CreditCard Then
                    ddlCardType.Visible = True
                End If

                'GetPostData()

                If Not Session("AddressID") Is Nothing And Not Session("SessionId") Is Nothing Then
                    GetShippingRate()
                    LoadDetails()
                    LoadCredentials()
                Else
                    lblStatus.Text = "The Address or Shopping Cart ID is missing, please try again."
                    lblStatus.Visible = True
                End If
            End If
        End Sub

        Private Sub GetPostData()
            Dim ShippingAddressID As Integer

            If Not Request("said") Is Nothing Then
                ShippingAddressID = Integer.Parse(Request("said"))

                Session.Add("AddressID", ShippingAddressID)
            Else
                lblStatus.Text &= "You did not submit a shipping address. <br />"
            End If

            Dim ShoppingCartSessionID As Guid

            If Not Request("scsid") Is Nothing Then
                ShoppingCartSessionID = New Guid(Request("scsid"))

                If Not Session("NewSessionID") Is Nothing Then
                    Session("NewSessionID") = ShoppingCartSessionID
                Else
                    Session.Add("NewSessionID", ShoppingCartSessionID)
                End If
            Else
                lblStatus.Text &= "You did not submit a valid shopping cart id. <br />"
            End If

            Dim PromoCode As String

            If Not Request("pc") Is Nothing Then
                PromoCode = Request("pc")

                If Not Session("PromoCode") Is Nothing Then
                    Session("PromoCode") = PromoCode
                Else
                    Session.Add("PromoCode", PromoCode)
                End If
            End If

            'TODO Create proper session variables
            'SessionID
            'AddressID
            'SelectedRate
            'Discount

            'TODO in order for the app to work with our solution the following criteria must be met:
            '   -Products must be retrieved from the database
            '   -A Simple User must be created
            '   -A valid shopping cart must be created
            '   -A shipping address must be created

            'TODO The following data will need to be submitted to this Mobile Checkout form:
            '   -The ID for the shipping address
            '   -The Session ID from the shopping cart that will be created by the mobile app.
            '
            'Optional:
            '   -The Promo Code which will issue a discount. The neccessary calcultations will need to be made dependant on the items in the order and the promo code.

        End Sub

        Private Sub LoadDetails()
            Dim address As Address = New AddressController().GetElement(CInt(Session("AddressID")))
            Dim province As Province = New ProvinceController().GetElement(address.ProvStateID)
            Dim country As Country = New CountryController().GetElement(province.CountryID)

            Dim sessionID As String = Session("SessionId").ToString

            Dim rate As CCFramework.Core.Shipping.ShippingOption = CType(Session("SelectedRate"), CCFramework.Core.Shipping.ShippingOption)
            Dim discount As Decimal = 0
            Dim oTotal As OrderTotals

            Dim items As Integer = ShoppingCartController.GetItemCount(sessionID)
            Dim subtotal As Decimal = ShoppingCartController.GetSubTotal(sessionID, EPriceLevel.Regular)
            Dim taxrate As Decimal = province.Tax + country.Tax

            discount = GetDiscount(subtotal)

            oTotal = New OrderTotals(items, ShoppingCartController.GetSubTotal(sessionID, EPriceLevel.Regular), rate.Total, taxrate, discount)


            litOrderItems.Text = oTotal.Items.ToString
            litSubTotal.Text = oTotal.SubTotal.ToString("C")
            litShipCharge.Text = oTotal.ShippingCharge.ToString("C")
            litDiscount.Text = oTotal.Discount.ToString("C")
            litTaxRate.Text = oTotal.TaxRate.ToString("P")
            litTaxAmount.Text = oTotal.Tax.ToString("C")
            litTotal.Text = oTotal.Total.ToString("C")

            If Session("Discount") Is Nothing Then
                Session.Add("Discount", discount)
            Else
                Session("Discount") = discount
            End If
        End Sub

        Private Sub LoadCredentials()
            If Not Session("AddressID") Is Nothing Then
                Dim address As Address = New AddressController().GetElement(CInt(Session("AddressID")))
                Dim province As Province = New ProvinceController().GetElement(address.ProvStateID)
                Dim country As Country = New CountryController().GetElement(province.CountryID)

                Dim strAddress As String = address.Address & "<br /> " & address.City & ", " & province.Name & ", " & country.Name & "<br />" & address.PCZIP

                litSAddress.Text = strAddress

                If Not System.Web.Security.Membership.GetUser Is Nothing Then
                    Dim user As RegisteredUser = New CCFramework.Core.RegisteredUserController().GetByUserName(System.Web.Security.Membership.GetUser.UserName)
                    Dim simpleUser As SimpleUser = New CCFramework.Core.SimpleUserController().GetElement(user.UserID)
                    Dim name As New BillingName(simpleUser.FirstName, simpleUser.MiddleName, simpleUser.LastName)

                    txtEmail.Text = simpleUser.Email
                    txtPN.Text = simpleUser.PhoneNumber
                    txtCCName.Text = name.Full

                    txtEmail.Enabled = False
                    txtPN.Enabled = False
                End If
            End If
        End Sub

        Private Sub RefreshDataSources()
            ddlBCountry.Items.Clear()
            ddlBCountry.Items.Add(New ListItem("Country", ""))
            ddlBCountry.AppendDataBoundItems = True
            ddlBCountry.DataSource = New CountryController().GetElements
            ddlBCountry.DataBind()

            ddlCardType.Items.Clear()
            ddlCardType.Items.Add(New ListItem("Card Type", ""))
            ddlCardType.AppendDataBoundItems = True
            ddlCardType.DataSource = New CreditCardTypeController().GetElements
            ddlCardType.DataBind()

            ddlExpYear.Items.Clear()
            ddlExpYear.Items.Add(New ListItem("Year", ""))
            ddlExpYear.AppendDataBoundItems = True
            ddlExpYear.DataSource = CCFramework.Core.CCFunctions.GetExpirationYears()
            ddlExpYear.DataBind()

            ddlExpMonth.Items.Clear()
            ddlExpMonth.Items.Add(New ListItem("Month", ""))
            ddlExpMonth.AppendDataBoundItems = True
            ddlExpMonth.Items.AddRange(CCFramework.Core.CCFunctions.GetExpirationMonths.ToArray)

            ddlBProvince.Items.Clear()
            ddlBProvince.Items.Add(New ListItem("Province/State", ""))
            ddlBProvince.AppendDataBoundItems = True
            ddlBProvince.DataSource = New ProvinceController().GetElements
            ddlBProvince.DataBind()
        End Sub

        Private Sub ckbSameAsShipping_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ckbSameAsShipping.CheckedChanged
            If ckbSameAsShipping.Checked = True Then
                tblBillingAddress.Visible = False
            Else
                tblBillingAddress.Visible = True
            End If
        End Sub

        Private pPurchase As Purchase

        Private Sub btnPurchase_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnPurchase.Command
            If CheckFormCleared() Then
                Dim paymentType As EPaymentType = CType(Settings.DefaultPaymentType, EPaymentType)
                Dim sessionID As String = Session("NewSessionID").ToString

                Dim sAddressID As Integer
                Dim bAddressID As Integer

                If ckbSameAsShipping.Checked Then
                    sAddressID = CInt(Session("AddressID"))
                    bAddressID = CInt(Session("AddressID"))
                Else
                    sAddressID = CInt(Session("AddressID"))
                    bAddressID = New AddressController().Create(txtBAddress.Text, txtBCity.Text, txtBPC.Text, Integer.Parse(ddlBProvince.SelectedValue), Integer.Parse(ddlBCountry.SelectedValue))
                End If

                Dim selectedRate As CCFramework.Core.Shipping.ShippingOption = CType(Session("SelectedRate"), CCFramework.Core.Shipping.ShippingOption)

                ' New Purchase Implementation
                Dim signedIn As Boolean
                If System.Web.Security.Membership.GetUser Is Nothing Then
                    signedIn = False
                Else
                    signedIn = True
                End If

                Dim discount As Decimal = CType(Session("Discount"), Decimal)

                'Dim username As String = Membership.GetUser.UserName
                Dim ccName As String = txtCCName.Text
                Dim email As String = txtEmail.Text
                Dim phone As String = txtPN.Text

                pPurchase = New Purchase(paymentType, sessionID, signedIn, selectedRate, discount, bAddressID, sAddressID, ccName, email, phone)

                pPurchase.SetCreditCardDetails(txtCCNumber.Text, Integer.Parse(ddlExpYear.SelectedValue), Integer.Parse(ddlExpMonth.SelectedValue), Integer.Parse(txtCCCVD.Text))

                Dim message As PaymentStatusMessage = pPurchase.ProcessPayment()

                If message.Approved = True And message.IsVBV = False Then
                    Dim IsMembership As Boolean = False 'Check if this is a membership purchase

                    If IsMembership Then
                        'Send an Email with the safety waiver
                    Else
                        CCFramework.Core.SendEmails.SendEmailInvoice(pPurchase.OrderID, txtEmail.Text, True, Context)
                    End If

                    Response.Redirect("~/CCCommerce/Mobile/MobileThankYou.aspx")
                ElseIf message.Approved = False And message.IsVBV = True Then
                    'TODO Move to an iFrame within the page so it looks like the user is still on the current web site.
                    Page.Controls.Clear()
                    Response.Write(message.VBVResponse)
                Else
                    'Declined so retry payment. Get new details
                    If Session("RetryOrder") Is Nothing Then
                        Session.Add("RetryOrder", True)
                    Else
                        Session("RetryOrder") = True
                    End If

                    If Session("Purchase") Is Nothing Then
                        Session.Add("Purchase", pPurchase)
                    Else
                        Session("Purchase") = pPurchase
                    End If

                    lblStatus.Text = "We're sorry! Your order has been declined. error code: " & message.Message
                    lblStatus.Visible = True
                End If
            Else
                Response.Redirect("~/CCCommerce/Mobile/Mobile/ShoppingCart.aspx")
            End If

        End Sub

        Private Sub RetryPayment()

            pPurchase = CType(Session("Purchase"), Purchase)

            Dim CCName As String = txtCCName.Text
            Dim Email As String = txtEmail.Text
            Dim PhoneNumber As String = txtPN.Text

            pPurchase.UpdateUser(CCName, Email, PhoneNumber)

            If Not ckbSameAsShipping.Checked Then
                Dim BillingAddress As String = txtBAddress.Text
                Dim BillingCity As String = txtBCity.Text
                Dim BillingPostalCode As String = txtBPC.Text
                Dim BillingProvince As Integer = Integer.Parse(ddlBProvince.SelectedValue)
                Dim BillingCountry As Integer = Integer.Parse(ddlBCountry.SelectedValue)

                pPurchase.UpdateBillingAddress(BillingAddress, BillingCity, BillingPostalCode, BillingProvince, BillingCountry)
            End If

            Dim CCNumber As String = txtCCNumber.Text
            Dim ExpiryYear As Integer = Integer.Parse(ddlExpYear.SelectedValue)
            Dim ExpiryMonth As Integer = Integer.Parse(ddlExpMonth.SelectedValue)
            Dim CCCVD As Integer = Integer.Parse(txtCCCVD.Text)

            pPurchase.SetCreditCardDetails(CCNumber, ExpiryYear, ExpiryMonth, CCCVD)

            'pPurchase = New Commerce.Purchase(Commerce.ePaymentType.BeanStream, sessionID, signedIn, Membership.GetUser.UserName, _
            '                                     selectedRate, bAddressID, sAddressID, txtCCName.Text, txtEmail.Text, txtPN.Text)

            Dim message As PaymentStatusMessage = pPurchase.RetryPayment

            If message.Approved = True And message.IsVBV = False Then
                Dim IsMembership As Boolean = False 'Check if this is a membership purchase

                If IsMembership Then
                    'Send an Email with the safety waiver
                Else
                    CCFramework.Core.SendEmails.SendEmailInvoice(pPurchase.OrderID, Email, True, Context)
                End If

                Response.Redirect("~/CCCommerce/ThankYou.aspx")
            ElseIf message.Approved = False And message.IsVBV = True Then
                'TODO Move to an iFrame within the page so it looks like the user is still on the current web site.
                Page.Controls.Clear()
                Response.Write(message.VBVResponse)
            Else
                'Declined so retry payment. Get new details
                If Session("RetryOrder") Is Nothing Then
                    Session.Add("RetryOrder", True)
                Else
                    Session("RetryOrder") = True
                End If

                If Session("Purchase") Is Nothing Then
                    Session.Add("Purchase", pPurchase)
                Else
                    Session("Purchase") = pPurchase
                End If

                lblStatus.Text = "We're sorry! Your order has been declined. error code: " & message.Message
                lblStatus.Visible = True
            End If

        End Sub

        'Private Sub RetryPayment()
        '    Dim paymentID As Integer = Session("NewPaymentID")
        '    Dim orderID As Integer = Session("NewOrderID")
        '    Dim userID As Integer = Session("UserID")

        '    Dim names As BillingName = New BillingName(txtCCName.Text)

        '    If Membership.GetUser Is Nothing Then
        '        Dim updateUser As Boolean = New Commerce.cSimpleUser().Update(userID, names.First, names.Middle, names.Last, txtEmail.Text, txtPN.Text)
        '    End If

        '    Dim order As Order = New Commerce.cOrder().GetElement(orderID)

        '    If Not order.BAddressID = order.SAddressID Then
        '        Dim updateAdress As Boolean = New Commerce.cAddress().Update(order.BAddressID, txtBAddress.Text, txtBCity.Text, txtBPC.Text, Integer.Parse(ddlBProvince.SelectedValue), Integer.Parse(ddlBCountry.SelectedValue))
        '    End If

        '    Dim bAddress As Address = New Commerce.cAddress().GetElement(order.BAddressID)
        '    Dim bProvince As Province = New Commerce.cProvince().GetElement(bAddress.ProvStateID)
        '    Dim bCountry As Country = New Commerce.cCountry().GetElement(bProvince.CountryID)

        '    Dim oTotal As Commerce.OrderTotals = New Commerce.OrderTotals(Commerce.cOrder.GetItemCount(orderID), Commerce.cOrder.GetSubTotal(orderID), order.ShipCharge, order.TaxPercent)

        '    Dim expDate As Date = New Date(Integer.Parse(ddlExpYear.SelectedValue), Integer.Parse(ddlExpMonth.SelectedValue), 1)
        '    Dim expYear As Integer = Integer.Parse(expDate.Year.ToString.Substring(2))

        '    Dim ec As New Commerce.EasyCryptography
        '    Dim key As String = ec.GenerateEncryptionKey()
        '    Dim CCNumber As String = ec.Encrypt(txtCCNumber.Text, key)

        '    Dim Details As Commerce.BeanStreamBilling.BeanStreamTransaction
        '    Details = New Commerce.BeanStreamBilling.BeanStreamTransaction(oTotal.Total, names.Full, txtPN.Text, bAddress.Address, "", bAddress.City, bProvince.Code, _
        '                                                                   bAddress.PCZIP, bCountry.Code, txtEmail.Text, orderID.ToString, txtCCNumber.Text, names.Full, _
        '                                                                   expDate.Month, expYear, Integer.Parse(txtCCCVD.Text))

        '    Dim bsPayment As New Commerce.BeanStreamFunctions(Details)
        '    Dim returnMessage As Commerce.BeanStreamBilling.BeanStreamReturnMessage

        '    returnMessage = bsPayment.ProcessRequest()

        '    Select Case returnMessage.Status
        '        Case Commerce.BeanStreamBilling.PaymentStatus.Approved
        '            Session("RetryOrder") = False

        '            Dim updatePayment As Boolean = New Commerce.cBeanStreamPayment().Update(paymentID, orderID, names.Full, expDate.Month.ToString, expDate.Year.ToString.Substring(2), CCNumber, key, txtCCCVD.Text, oTotal.Total, Commerce.BeanStreamBilling.PaymentStatus.Approved, returnMessage.MessageID, returnMessage.MessageText)
        '            Dim paymentAuthorized As Boolean = New Commerce.cBeanStreamPayment().SetPaymentAuthorized(paymentID, returnMessage.AuthorizationCode, returnMessage.ProcessDate, returnMessage.TransactionID, Commerce.BeanStreamBilling.PaymentStatus.Approved, returnMessage.ChargeAmount)
        '            Dim orderAuth As Boolean = Commerce.cOrder.SetOrderAuthorized(orderID)

        '            Commerce.SendEmails.SendEmailInvoice(orderID, txtEmail.Text, Context)
        '        Case Else
        '            'Handle Verified by Visa
        '            If returnMessage.ResponseType = "R" Then
        '                Dim updatePayment As Boolean = New Commerce.cBeanStreamPayment().Update(paymentID, orderID, names.Full, expDate.Month.ToString, expDate.Year.ToString.Substring(2), CCNumber, key, txtCCCVD.Text, oTotal.Total, returnMessage.Status, returnMessage.MessageID, returnMessage.MessageText)

        '                Page.Controls.Clear()
        '                Response.Write(returnMessage.XMLResponse.InnerText)
        '            Else
        '                Dim updatePayment As Boolean = New Commerce.cBeanStreamPayment().Update(paymentID, orderID, names.Full, expDate.Month.ToString, expDate.Year.ToString.Substring(2), CCNumber, key, txtCCCVD.Text, oTotal.Total, returnMessage.Status, returnMessage.MessageID, returnMessage.MessageText)

        '                lblStatus.Text = returnMessage.Message
        '                lblStatus.Visible = True
        '            End If

        '    End Select

        '    If Not Session("NewPaymentID") Is Nothing Then
        '        Session("NewPaymentID") = paymentID
        '    Else
        '        Session.Add("NewPaymentID", paymentID)
        '    End If

        '    If returnMessage.Status = Commerce.BeanStreamBilling.PaymentStatus.Approved Then
        '        Response.Redirect("~/CCCommerce/ThankYou.aspx")
        '    End If
        'End Sub

        Private Function CheckFormCleared() As Boolean
            'TODO implement this to validate all the information e.g. Proper data types, valid Credit card, valid email, all data filled out.
            'Name on card must be less than 64 chars
            'Expiry Date must be greater then todays date
            'CC less then 20 chars
            If Not Session("RetryOrder") Is Nothing Then
                If CBool(Session("RetryOrder")) = True Then
                    RetryPayment()

                    Return False
                Else
                    Return True
                End If
            Else
                If Not Session("NewSessionID") Is Nothing And Not Session("SelectedRate") Is Nothing And Not Session("Discount") Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            End If

        End Function

        Private Sub ddlBCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBCountry.SelectedIndexChanged
            Dim countryID As Integer = Integer.Parse(ddlBCountry.SelectedValue)

            ddlBProvince.DataSource = New ProvinceController().GetCountryProvince(countryID)
            ddlBProvince.DataBind()
        End Sub

        Private Function GetDiscount(ByVal subtotal As Decimal) As Decimal
            Dim discount As Decimal = 0
            If Not Session("PromoCode") Is Nothing Then
                Dim promoCode As String = CType(Session("PromoCode"), String)

                If Not promoCode = String.Empty Then
                    discount = PromoCodeController.GetPromoCodeDiscount(promoCode, subtotal)
                End If
            End If

            Return discount
        End Function

        Private Function GetShippingRate() As Boolean
            Dim fixedOptions As ListItem

            Dim addressId As Integer

            If Not Session("AddressID") Is Nothing Then
                addressId = CType(Session("AddressID"), Integer)

                Session.Remove("ShippingOptions")
                Session.Add("ShippingOptions", New Hashtable)

                fixedOptions = ShippingHelper.GetFixedShippingOptions(Session("SessionId").ToString, addressId, MyBase.Context).FirstOrDefault

                Dim selectedRate As ShippingOption = CType(CType(Session("ShippingOptions"), Hashtable).Item(fixedOptions.Value), CCFramework.Core.Shipping.ShippingOption)

                If Not Session("SelectedRate") Is Nothing Then
                    Session("SelectedRate") = selectedRate ' shipCharge
                Else
                    Session.Add("SelectedRate", selectedRate) ' shipCharge)
                End If
            Else
                'There is no address set
                lblStatus.Text = "Unable to load the shipping rate."
                lblStatus.Visible = True

                btnPurchase.Enabled = False
            End If

            Return True
        End Function

    End Class
End Namespace