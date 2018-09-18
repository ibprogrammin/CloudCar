Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Core.Shipping
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Commerce.ShoppingCart
Imports CloudCar.CCFramework.Commerce.BusinessProcess
Imports CloudCar.CCFramework.Commerce.PaymentProcessing

Namespace CCCommerce

    Partial Public Class Checkout
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Settings.StoreEnabled Then
                Response.Redirect(Settings.StoreDisabledPage)
            End If

            If Not Page.IsPostBack Then
                If Not CheckOutAddressId = Nothing AndAlso Not CheckOutSelectedRate.Total = Nothing Then
                    'Reset the Purchase Session if this is the first time loading the page.
                    CheckOutPurchase = Nothing
                    CheckOutRetryOrder = False

                    Dim CurrentPaymentType As EPaymentType = CType(Settings.DefaultPaymentType, EPaymentType)
                    If CurrentPaymentType = EPaymentType.CreditCard Then
                        BillingType.Visible = True
                    End If

                    RefreshDataSources()

                    InitUserStatus()
                    LoadDetails()
                    LoadCredentials()
                Else
                    Response.RedirectToRoute("RouteShoppingCartA")
                End If
            End If
        End Sub

        Private Sub LoadDetails()
            'If Not Session("SessionId") Is Nothing And Not Session("AddressID") Is Nothing And Not Session("SelectedRate") Is Nothing Then
            Dim CurrentAddressController As New AddressController
            
            Dim CurrentAddress As Address = CurrentAddressController.GetElement(CheckOutAddressId)
            Dim CurrentProvince As Province = CurrentAddress.Province
            Dim CurrentCountry As Country = CurrentAddress.Province.Country

            CurrentAddressController.Dispose()

            Dim CurrentShippingOption As ShippingOption = CheckOutSelectedRate

            Dim CurrentOrderTotal As OrderTotals

            Dim CurrentItemCount As Integer
            Dim CurrentSubtotal As Decimal
            Dim CurrentTaxRate As Decimal

            If Not CheckOutUserLoggedIn Then
                CurrentItemCount = ShoppingCartController.GetItemCount(CheckOutSessionId)
                CurrentSubtotal = ShoppingCartController.GetSubTotal(CheckOutSessionId, EPriceLevel.Regular)
                CurrentTaxRate = CurrentProvince.Tax + CurrentCountry.Tax

                CheckOutDiscount = GetDiscount(CurrentSubtotal)

                CurrentOrderTotal = New OrderTotals(CurrentItemCount, CurrentSubtotal, CurrentShippingOption.Total, CurrentTaxRate, CheckOutDiscount)
            Else
                Dim CurrentRegisteredUserController As New RegisteredUserController
                Dim CurrentUser As RegisteredUser = CurrentRegisteredUserController.GetByUserName(CheckOutUserName)
                CurrentRegisteredUserController.Dispose()

                Dim CurrentPriceLevel As EPriceLevel = CType(CurrentUser.PriceLevel, EPriceLevel)

                CurrentItemCount = ShoppingCartController.GetItemCount(CurrentUser.UserID)
                CurrentSubtotal = ShoppingCartController.GetSubTotal(CurrentUser.UserID, CurrentPriceLevel)
                CurrentTaxRate = CurrentProvince.Tax + CurrentCountry.Tax

                CheckOutDiscount = GetDiscount(CurrentSubtotal)

                CurrentOrderTotal = New OrderTotals(CurrentItemCount, CurrentSubtotal, CurrentShippingOption.Total, CurrentTaxRate, CheckOutDiscount)
            End If

            OrderItemsLiteral.Text = CurrentOrderTotal.Items.ToString
            SubTotalLiteral.Text = CurrentOrderTotal.SubTotal.ToString("C")
            ShippingChargeLiteral.Text = CurrentOrderTotal.ShippingCharge.ToString("C")
            DiscountLiteral.Text = CurrentOrderTotal.Discount.ToString("C")
            TaxRateLiteral.Text = CurrentOrderTotal.TaxRate.ToString("P")
            TaxChargesLiteral.Text = CurrentOrderTotal.Tax.ToString("C")
            TotalLiteral.Text = CurrentOrderTotal.Total.ToString("C")

            ShippingDetailsLiteral.Text = String.Format("Shipping Service: {0} {1}", CheckOutSelectedRate.Company, CheckOutSelectedRate.Service)
            'End If
        End Sub

        Private Sub LoadCredentials()
            If Not CheckOutAddressId = Nothing Then
                Dim CurrentAddressController As New AddressController

                Dim CurrentAddress As Address = CurrentAddressController.GetElement(CheckOutAddressId)
                Dim CurrentProvince As Province = CurrentAddress.Province
                Dim CurrentCountry As Country = CurrentAddress.Province.Country

                CurrentAddressController.Dispose()

                Dim CurrentAddressString As String = String.Format("{0}<br />{1}, {2}, {3}<br />{4}", CurrentAddress.Address, CurrentAddress.City, CurrentProvince.Name, CurrentCountry.Name, CurrentAddress.PCZIP)

                ShippingAddressLiteral.Text = CurrentAddressString

                If CheckOutUserLoggedIn Then
                    Dim CurrentRegisteredUserController As New RegisteredUserController
                    Dim CurrentSimpleUserController As New SimpleUserController

                    Dim CurrentRegisteredUser As RegisteredUser = CurrentRegisteredUserController.GetByUserName(CheckOutUserName)
                    Dim CurrentSimpleUser As SimpleUser = CurrentSimpleUserController.GetElement(CurrentRegisteredUser.UserID)

                    CurrentRegisteredUserController.Dispose()
                    CurrentSimpleUserController.Dispose()

                    Dim CurrentBillingName As New BillingName(CurrentSimpleUser.FirstName, CurrentSimpleUser.MiddleName, CurrentSimpleUser.LastName)

                    EmailAddressTextBox.Text = CurrentSimpleUser.Email
                    PhoneNumberTextBox.Text = CurrentSimpleUser.PhoneNumber
                    CreditCardNameTextBox.Text = CurrentBillingName.Full

                    EmailAddressTextBox.Enabled = False
                    PhoneNumberTextBox.Enabled = False
                End If
            End If
        End Sub

        Private Sub RefreshDataSources()
            Dim CurrentCreditCardTypeController As New CreditCardTypeController

            BillingType.Items.Clear()
            BillingType.Items.Add(New ListItem("Card Type", ""))
            BillingType.AppendDataBoundItems = True
            BillingType.DataSource = CurrentCreditCardTypeController.GetElements
            BillingType.DataBind()

            CurrentCreditCardTypeController.Dispose()

            ExpiryYearDropDown.Items.Clear()
            ExpiryYearDropDown.Items.Add(New ListItem("Year", ""))
            ExpiryYearDropDown.AppendDataBoundItems = True
            ExpiryYearDropDown.DataSource = CCFunctions.GetExpirationYears()
            ExpiryYearDropDown.DataBind()

            ExpiryMonthDropDown.Items.Clear()
            ExpiryMonthDropDown.Items.Add(New ListItem("Month", ""))
            ExpiryMonthDropDown.AppendDataBoundItems = True
            ExpiryMonthDropDown.Items.AddRange(CCFunctions.GetExpirationMonths.ToArray)
        End Sub

        Private Sub PurchaseButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles PurchaseButton.Command
            If CheckFormCleared() Then
                Dim ShippingAddressId As Integer
                Dim BillingAddressId As Integer

                If BillingSameAsShippingCheckBox.Checked Then
                    ShippingAddressId = CheckOutAddressId
                    BillingAddressId = CheckOutAddressId
                Else
                    ShippingAddressId = CheckOutAddressId

                    Dim CurrentAddressController As New AddressController
                    BillingAddressId = CurrentAddressController.Create(BillingAddressTextBox.Text, BillingCityTextBox.Text, BillingPostalCodeTextBox.Text, Integer.Parse(BillingProvinceDropDown.SelectedValue), Integer.Parse(BillingCountryDropDown.SelectedValue))
                    CurrentAddressController.Dispose()
                End If

                Dim CurrentCreditCardName As String = CreditCardNameTextBox.Text
                Dim CurrentEmailAddress As String = EmailAddressTextBox.Text
                Dim CurrentPhoneNumber As String = PhoneNumberTextBox.Text

                CheckOutPurchase = New Purchase(CheckOutPaymentType, CheckOutSessionId, CheckOutUserLoggedIn, CheckOutSelectedRate, CheckOutDiscount, BillingAddressId, ShippingAddressId, CurrentCreditCardName, CurrentEmailAddress, CurrentPhoneNumber)

                CheckOutPurchase.SetCreditCardDetails(CreditCardNumberTextBox.Text, Integer.Parse(ExpiryYearDropDown.SelectedValue), Integer.Parse(ExpiryMonthDropDown.SelectedValue), Integer.Parse(CreditCardCvdTextBox.Text))

                Dim PaymentStatusMessage As PaymentStatusMessage = CheckOutPurchase.ProcessPayment()

                If PaymentStatusMessage.Approved = True AndAlso PaymentStatusMessage.IsVBV = False Then
                    FinalizeOrder(CheckOutPurchase.OrderId, EmailAddressTextBox.Text)
                ElseIf PaymentStatusMessage.Approved = False AndAlso PaymentStatusMessage.IsVBV = True Then
                    'TODO Move to an iFrame within the page so it looks like the user is still on the current web site.
                    Page.Controls.Clear()
                    Response.Write(PaymentStatusMessage.VBVResponse)
                Else
                    CheckOutRetryOrder = True

                    StatusMessageLabel.Text = String.Format("We're sorry! Your order has been declined. Error code: {0}", PaymentStatusMessage.Message)
                    StatusMessageLabel.Visible = True
                End If
            Else
                Response.RedirectToRoute("RouteShoppingCartA")
            End If
        End Sub

        Private Sub RetryPayment()
            If Not CheckOutPurchase Is Nothing Then
                Dim CurrentCreditCardName As String = CreditCardNameTextBox.Text
                Dim CurrentEmailAddress As String = EmailAddressTextBox.Text
                Dim CurrentPhoneNumber As String = PhoneNumberTextBox.Text

                CheckOutPurchase.UpdateUser(CurrentCreditCardName, CurrentEmailAddress, CurrentPhoneNumber)

                If Not BillingSameAsShippingCheckBox.Checked Then
                    Dim BillingAddress As String = BillingAddressTextBox.Text
                    Dim BillingCity As String = BillingCityTextBox.Text
                    Dim BillingPostalCode As String = BillingPostalCodeTextBox.Text
                    Dim BillingProvince As Integer = Integer.Parse(BillingProvinceDropDown.SelectedValue)
                    Dim BillingCountry As Integer = Integer.Parse(BillingCountryDropDown.SelectedValue)

                    CheckOutPurchase.UpdateBillingAddress(BillingAddress, BillingCity, BillingPostalCode, BillingProvince, BillingCountry)
                End If

                Dim CurrentCreditCardNumber As String = CreditCardNumberTextBox.Text
                Dim CurrentExpiryYear As Integer = Integer.Parse(ExpiryYearDropDown.SelectedValue)
                Dim CurrentExpiryMonth As Integer = Integer.Parse(ExpiryMonthDropDown.SelectedValue)
                Dim CurrentCreditCardCvd As Integer = Integer.Parse(CreditCardCvdTextBox.Text)

                CheckOutPurchase.SetCreditCardDetails(CurrentCreditCardNumber, CurrentExpiryYear, CurrentExpiryMonth, CurrentCreditCardCvd)

                Dim CurrentStatusMessage As PaymentStatusMessage = CheckOutPurchase.RetryPayment

                If CurrentStatusMessage.Approved = True AndAlso CurrentStatusMessage.IsVBV = False Then
                    FinalizeOrder(CheckOutPurchase.OrderId, EmailAddressTextBox.Text)
                ElseIf CurrentStatusMessage.Approved = False AndAlso CurrentStatusMessage.IsVBV = True Then
                    'TODO Move to an iFrame within the page so it looks like the user is still on the current web site.
                    Page.Controls.Clear()
                    Response.Write(CurrentStatusMessage.VBVResponse)
                Else
                    CheckOutRetryOrder = True

                    StatusMessageLabel.Text = String.Format("We're sorry! Your order has been declined. error code: {0}", CurrentStatusMessage.Message)
                    StatusMessageLabel.Visible = True
                End If
            Else
                Response.RedirectToRoute("RouteShoppingCartA")
            End If
        End Sub

        Private Sub FinalizeOrder(OrderId As Integer, EmailAddress As String)
            SendEmails.SendEmailInvoice(OrderId, EmailAddress, True, Context)

            Session.Remove("OrderId")
            Session.Remove("EmailAddress")
            Session.Add("OrderId", OrderId)
            Session.Add("EmailAddress", EmailAddress)

            Response.RedirectToRoute("RouteThankYouA")
        End Sub

        Private Function CheckFormCleared() As Boolean
            'TODO implement this to validate all the information e.g. Proper data types, valid Credit card, valid email, all data filled out.
            'Name on card must be less than 64 chars
            'Expiry Date must be greater then todays date
            'CC less then 20 chars
            If Not CheckOutRetryOrder = Nothing Then
                If CheckOutRetryOrder Then
                    RetryPayment()

                    Return False
                Else
                    Return True
                End If
            Else
                If Not CheckOutSessionId Is Nothing _
                        AndAlso Not CheckOutSelectedRate.Total = Nothing Then
                    Return True
                Else
                    Return False
                End If
            End If

        End Function

        Public Sub BillingSameAsShippingCheckBoxCheckedChanged(ByVal Sender As Object, ByVal Args As EventArgs) Handles BillingSameAsShippingCheckBox.CheckedChanged
            If BillingSameAsShippingCheckBox.Checked = True Then
                BillingAddressPlaceHolder.Visible = False
            Else
                BillingAddressPlaceHolder.Visible = True
            End If
        End Sub

        Public Sub BillingCountryDropDownLoad(Sender As Object, Args As EventArgs) Handles BillingCountryDropDown.Init
            Dim CurrentCountryController As New CountryController

            BillingCountryDropDown.Items.Clear()
            BillingCountryDropDown.Items.Add(New ListItem("Country", ""))
            BillingCountryDropDown.AppendDataBoundItems = True
            BillingCountryDropDown.DataSource = CurrentCountryController.GetElements
            BillingCountryDropDown.DataBind()

            CurrentCountryController.Dispose()
        End Sub

        Public Sub BillingProvinceDropDownLoad(Sender As Object, Args As EventArgs) Handles BillingProvinceDropDown.Init
            Dim CurrentProvinceController As New ProvinceController

            BillingProvinceDropDown.Items.Clear()
            BillingProvinceDropDown.Items.Add(New ListItem("Province/State", ""))
            BillingProvinceDropDown.AppendDataBoundItems = True
            BillingProvinceDropDown.DataSource = CurrentProvinceController.GetElements
            BillingProvinceDropDown.DataBind()

            CurrentProvinceController.Dispose()
        End Sub

        Public Sub BillingCountryDropDownSelectedIndexChanged(ByVal Sender As Object, ByVal Args As EventArgs) Handles BillingCountryDropDown.SelectedIndexChanged
            Dim CurrentCountryId As Integer = Integer.Parse(BillingCountryDropDown.SelectedValue)

            Dim CurrentProvinceController As New ProvinceController

            BillingProvinceDropDown.DataSource = CurrentProvinceController.GetCountryProvince(CurrentCountryId)
            BillingProvinceDropDown.DataBind()

            CurrentProvinceController.Dispose()

            BillingProvinceDropDown.Focus()
        End Sub

        Public Sub BillingProvinceDropDownSelectedIndexChanged(ByVal Sender As Object, ByVal Args As EventArgs) Handles BillingProvinceDropDown.SelectedIndexChanged
            Dim CurrentProvinceId As Integer = Integer.Parse(BillingProvinceDropDown.SelectedValue)

            BillingCountryDropDown.SelectedValue = CountryController.GetCountryIdByProvinceId(CurrentProvinceId).ToString
            BillingPostalCodeTextBox.Focus()
        End Sub

        Private Function GetDiscount(ByVal SubTotal As Decimal) As Decimal
            Dim CurrentDiscount As Decimal = 0

            If Not Session("PromoCode") Is Nothing Then
                Dim CurrentPromoCode As String = CType(Session("PromoCode"), String)

                If Not CurrentPromoCode = String.Empty Then
                    CurrentDiscount = PromoCodeController.GetPromoCodeDiscount(CurrentPromoCode, SubTotal)
                End If
            End If

            Return CurrentDiscount
        End Function

#Region "Check Out Properties"

        Public ReadOnly Property CheckOutUserLoggedIn As Boolean
            Get
                Return CBool(Session("UserLoggedIn"))
            End Get
        End Property

        Public ReadOnly Property CheckOutUserName As String
            Get
                Return CStr(Session("UserName"))
            End Get
        End Property

        Private Sub InitUserStatus()
            If Not Web.Security.Membership.GetUser Is Nothing Then
                If Session("UserLoggedIn") Is Nothing Then
                    Session.Add("UserLoggedIn", True)
                Else
                    Session("UserLoggedIn") = True
                End If

                If Session("UserName") Is Nothing Then
                    Session.Add("UserName", Web.Security.Membership.GetUser.UserName)
                Else
                    Session("UserName") = Web.Security.Membership.GetUser.UserName
                End If
            Else
                If Session("UserLoggedIn") Is Nothing Then
                    Session.Add("UserLoggedIn", False)
                Else
                    Session("UserLoggedIn") = False
                End If
            End If
        End Sub

        Public Property CheckOutPurchase As Purchase
            Get
                Return CType(Session("Purchase"), Purchase)
            End Get
            Set(Value As Purchase)
                If Session("Purchase") Is Nothing Then
                    Session.Add("Purchase", Value)
                Else
                    Session("Purchase") = Value
                End If
            End Set
        End Property

        Public Property CheckOutSessionId As String
            Get
                Return Session("SessionId").ToString
            End Get
            Set(Value As String)
                If Session("SessionId") Is Nothing Then
                    Session.Add("SessionId", New Guid(Value))
                Else
                    Session("SessionId") = New Guid(Value)
                End If
            End Set
        End Property

        Public Property CheckOutAddressId As Integer
            Get
                Return CInt(Session("AddressID"))
            End Get
            Set(Value As Integer)
                If Session("AddressID") Is Nothing Then
                    Session.Add("AddressID", Value)
                Else
                    Session("AddressID") = Value
                End If
            End Set
        End Property

        Public Property CheckOutSelectedRate As ShippingOption
            Get
                Return CType(Session("SelectedRate"), ShippingOption)
            End Get
            Set(Value As ShippingOption)
                If Session("SelectedRate") Is Nothing Then
                    Session.Add("SelectedRate", Value)
                Else
                    Session("SelectedRate") = Value
                End If
            End Set
        End Property

        Public Property CheckOutDiscount As Decimal
            Get
                Return CDec(Session("Discount"))
            End Get
            Set(Value As Decimal)
                If Session("Discount") Is Nothing Then
                    Session.Add("Discount", Value)
                Else
                    Session("Discount") = Value
                End If
            End Set
        End Property

        Public ReadOnly Property CheckOutPaymentType As EPaymentType
            Get
                Return CType(Settings.DefaultPaymentType, EPaymentType)
            End Get
        End Property

        Public Property CheckOutRetryOrder As Boolean
            Get
                Return CBool(Session("RetryOrder"))
            End Get
            Set(Value As Boolean)
                If Session("RetryOrder") Is Nothing Then
                    Session.Add("RetryOrder", Value)
                Else
                    Session("RetryOrder") = Value
                End If
            End Set
        End Property

#End Region

    End Class

    Public Class BillingName
        Public First As String
        Public Last As String
        Public Middle As String
        Public Full As String

        Public Sub New(ByVal _name As String)
            Full = _name

            Dim names As String() = _name.Split(" "c)

            Dim firstName As String = names(0)
            Dim lastName As String = names(names.Count - 1)
            Dim middleName As String = ""

            If names.Count > 2 Then
                For Each item In names
                    If Not item = names(0) AndAlso Not item = names(names.Count - 1) Then
                        middleName &= item & " "
                    End If
                Next
            End If

            First = firstName
            Middle = middleName
            Last = lastName
        End Sub

        Public Sub New(ByVal _first As String, ByVal _middle As String, ByVal _last As String)
            First = _first
            Middle = _middle
            Last = _last
            If Not Middle = "" Then
                Full = _first & " " & _middle & " " & _last
            Else
                Full = _first & " " & _last
            End If

        End Sub

    End Class

End Namespace