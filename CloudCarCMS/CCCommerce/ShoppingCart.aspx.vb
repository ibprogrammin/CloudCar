Imports CloudCar.CCControls.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Core.Shipping
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Commerce.Shipping
Imports CloudCar.CCFramework.Commerce.ShoppingCart

Namespace CCCommerce

    Partial Public Class ShoppingCartPage
        Inherits Page

        Protected Overrides Sub OnLoad(ByVal E As EventArgs)
            If Not Settings.StoreEnabled Then
                Response.Redirect(Settings.StoreDisabledPage)
            End If

            If Not Page.IsPostBack Then
                RefreshDataSources()

                InitUserStatus()
                If ShoppingCartUserLoggedIn Then
                    LoadUserDetails()
                End If

                LoadCart()
            End If
        End Sub

        Private Sub LoadUserDetails()
            Dim CurrentAddressController As New AddressController()
            Dim CurrentAddress As Address = CurrentAddressController.GetElement(ShoppingCartRegisteredUser.AddressID)
            CurrentAddressController.Dispose()

            If Not CurrentAddress Is Nothing Then
                Dim CurrentProvinceController As New ProvinceController()

                Dim CurrentCountryId As Integer = CurrentProvinceController.GetElement(CurrentAddress.ProvStateID).CountryID
                CountryDropDown.SelectedValue = CurrentCountryId.ToString

                ProvinceDropDown.DataSource = CurrentProvinceController.GetCountryProvince(CurrentCountryId)
                ProvinceDropDown.DataBind()
                ProvinceDropDown.SelectedValue = CurrentAddress.ProvStateID.ToString

                CurrentProvinceController.Dispose()

                AddressTextBox.Text = CurrentAddress.Address
                CityTextBox.Text = CurrentAddress.City
                PostalCodeTextBox.Text = CurrentAddress.PCZIP
                
                AddressTextBox.Enabled = False
                CityTextBox.Enabled = False
                PostalCodeTextBox.Enabled = False
                CountryDropDown.Enabled = False
                ProvinceDropDown.Enabled = False

                ShoppingCartAddressId = CurrentAddress.ID

                LoadShippingRates()

                CheckStepOneImage.Visible = True
            End If
        End Sub

        Private Sub RefreshDataSources()
            Dim CurrentProvinceController As New ProvinceController
            Dim CurrentCountryController As New CountryController

            ProvinceDropDown.Items.Clear()
            ProvinceDropDown.Items.Add(New ListItem("Province/State", ""))
            ProvinceDropDown.AppendDataBoundItems = True
            ProvinceDropDown.DataSource = CurrentProvinceController.GetElements()
            ProvinceDropDown.DataBind()

            CountryDropDown.Items.Clear()
            CountryDropDown.Items.Add(New ListItem("Country", ""))
            CountryDropDown.AppendDataBoundItems = True
            CountryDropDown.DataSource = CurrentCountryController.GetElements()
            CountryDropDown.DataBind()

            CurrentProvinceController.Dispose()
            CurrentCountryController.Dispose()
        End Sub

        Private Sub LoadCart()
            Dim ShoppingCartSummaryItems As List(Of ShoppingCartSummaryItem)

            Dim CurrentDataContext As New CommerceDataContext

            If ShoppingCartUserLoggedIn Then
                'If CartHasMembership(Membership.GetUser.UserName) Then
                'TODO: if there is a membership in the cart then do not allow any other items in the cart.. give a status message stating that 
                'only one item may be purchased at a time durring a recuring billing transaction.
                'lblStatus.Text = "Please note that you may only purchase one membership at a time per user account. If you are buying for a family member, please register them seperatly."
                'End If
                ShoppingCartSummaryItems = ShoppingCartHelper.GetShoppingCartSummaryItemsByUserId(CurrentDataContext, ShoppingCartRegisteredUser.UserID).ToList
            Else
                ShoppingCartSummaryItems = ShoppingCartHelper.GetShoppingCartSummaryItemsBySessionId(CurrentDataContext, ShoppingCartSessionId).ToList
            End If

            CurrentDataContext.Dispose()

            If Not ShoppingCartSummaryItems Is Nothing AndAlso ShoppingCartSummaryItems.Count > 0 Then
                ShoppingCartItemsRepeater.DataSource = ShoppingCartSummaryItems
                ShoppingCartItemsRepeater.DataBind()

                If Settings.ShowCartWeight Then

                    Dim Weight As Decimal

                    If Web.Security.Membership.GetUser Is Nothing Then
                        Weight = ShippingHelper.GetCartWeight(ShoppingCartSessionId)
                    Else
                        Weight = ShippingHelper.GetCartWeight(ShoppingCartRegisteredUser.UserID)
                    End If

                    CartWeightLiteral.Text = String.Format("({0} Kg)", Weight.ToString)
                    CartWeightLiteral.Visible = True
                End If

                CalculateTotal()
            Else
                ShoppingCartPlaceHolder.Visible = False

                StatusMessageLabel.Attributes("class") = "warning"
                StatusMessageLabel.Visible = True
                StatusMessageLabel.InnerText = String.Format("You have no items in your shopping cart.")

                CheckoutButton.Visible = False
            End If
        End Sub

        Private Sub LoadShippingRates()
            If ReadyForRates() Then
                ShoppingCartFreeShipping = Nothing
                
                ShippingOptionsRadioList.Items.Clear()

                ShippingRateLabel.Visible = False

                If Settings.UseFreeShipping AndAlso GetSubTotal() > Settings.FreeShippingAfterAmount Then
                    ShippingRateLabel.Text = String.Format("Free over ${0}", Settings.FreeShippingAfterAmount.ToString("n2"))
                    ShippingRateLabel.Visible = True
                    CheckStepTwoImage.Visible = True
                    ShoppingCartFreeShipping = True

                    ShippingOptionsRadioList.Items.Add(New ListItem("Free Shipping ($0.00)", "Free Shipping"))
                    ShippingOptionsRadioList.SelectedIndex() = 0
                    ShippingOptionsRadioList.Visible = False
                Else
                    If Not ShoppingCartUserLoggedIn Then
                        Dim CurrentAddressController As New AddressController
                        ShoppingCartAddressId = CurrentAddressController.Create(AddressTextBox.Text, CityTextBox.Text, PostalCodeTextBox.Text, Integer.Parse(ProvinceDropDown.SelectedValue), Integer.Parse(CountryDropDown.SelectedValue))
                        CurrentAddressController.Dispose()

                        ShoppingCartShippingRates = New ShippingRatesController(ShoppingCartSessionId, ShoppingCartAddressId)
                    Else
                        ShoppingCartAddressId = ShoppingCartRegisteredUser.AddressID

                        ShoppingCartShippingRates = New ShippingRatesController(ShoppingCartRegisteredUser.UserID, ShoppingCartAddressId)
                    End If

                    ShoppingCartShippingRates.RequestAvailableShippingRates()

                    For Each CurrentRateItem As ListItem In ShoppingCartShippingRates.GetRatesAsListItems()
                        ShippingOptionsRadioList.Items.Add(CurrentRateItem)
                    Next

                    ShippingOptionsRadioList.Visible = True
                End If

                CalculateTotal()
            End If
        End Sub

        Private Function GetSubTotal() As Decimal
            Dim CurrentTotal As Decimal = 0D

            If ShoppingCartUserLoggedIn Then
                Dim CurrentPriceLevel As EPriceLevel = CType(ShoppingCartRegisteredUser.PriceLevel, EPriceLevel)

                CurrentTotal = ShoppingCartController.GetSubTotal(ShoppingCartRegisteredUser.UserID, CurrentPriceLevel)
            Else
                CurrentTotal = ShoppingCartController.GetSubTotal(ShoppingCartSessionId, EPriceLevel.Regular)
            End If

            Return CurrentTotal
        End Function

        Private Sub CalculateTotal()
            Dim CurrentTotal As Decimal = GetSubTotal()

            TotalHiddenField.Value = CurrentTotal.ToString

            If ShippingOptionsRadioList.Items.Count > 0 Then
                Dim CurrentShippingCharge As Decimal

                If Not ShoppingCartShippingRates Is Nothing Then
                    If Not ShippingOptionsRadioList.SelectedValue = String.Empty Then
                        CurrentShippingCharge = CDec(Math.Round(GetSelectedShippingRate.Rate, 2))
                    Else
                        CurrentShippingCharge = 0
                    End If
                Else
                    CurrentShippingCharge = 0
                End If

                SelectedRateHiddenField.Value = CurrentShippingCharge.ToString("n2")

                ShippingChargesLiteral.Text = CurrentShippingCharge.ToString("n2")
                CurrentTotal += CurrentShippingCharge
            End If

            TotalLiteral.Text = CurrentTotal.ToString("C")
        End Sub

        Private Sub CountryDropDownSelectedIndexChanged(ByVal Sender As Object, ByVal Args As EventArgs) Handles CountryDropDown.SelectedIndexChanged
            If Not CountryDropDown.SelectedValue = "" Then
                Dim CurrentCountryId As Integer = Integer.Parse(CountryDropDown.SelectedValue)

                Dim CurrentProvinceController As New ProvinceController

                ProvinceDropDown.DataSource = CurrentProvinceController.GetCountryProvince(CurrentCountryId)
                ProvinceDropDown.DataBind()

                CurrentProvinceController.Dispose()

                ProvinceDropDown.Focus()
            End If
        End Sub

        Private Sub ProvinceDropDownSelectedIndexChanged(ByVal Sender As Object, ByVal Args As EventArgs) Handles ProvinceDropDown.SelectedIndexChanged
            If Not ProvinceDropDown.SelectedValue = "" Then
                Dim CurrentProvinceId As Integer = Integer.Parse(ProvinceDropDown.SelectedValue)

                Dim CurrentCountryController As New CountryController
                CountryDropDown.SelectedValue = CurrentCountryController.GetCountryByProvince(CurrentProvinceId).ID.ToString
                CurrentCountryController.Dispose()

                PostalCodeTextBox.Focus()
            End If
        End Sub

        Private Sub ShippingOptionsButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles ShippingOptionsButton.Command
            LoadShippingRates()

            CheckStepOneImage.Visible = True
        End Sub

        Protected Sub EmptyCartButtonClick(ByVal Sender As Object, ByVal Args As EventArgs)
            If ShoppingCartUserLoggedIn Then
                ShoppingCartController.ClearShoppingCart(ShoppingCartRegisteredUser.UserID)
            Else
                ShoppingCartController.ClearShoppingCart(ShoppingCartSessionId)
            End If

            LoadCart()

            'Dim CurrentShoppingDetailsControl As ShoppingDetailsControl = CType(Me.Master.FindControl("MasterShoppingDetailsControl"), ShoppingDetailsControl)

            'CurrentShoppingDetailsControl.LoadDetails()
        End Sub

        Private Sub ShippingOptionsRadioListSelectedIndexChanged(ByVal Sender As Object, ByVal Args As EventArgs) Handles ShippingOptionsRadioList.SelectedIndexChanged
            CalculateTotal()

            CheckStepTwoImage.Visible = True
        End Sub

        Protected Sub DeleteItemButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs)
            Dim CurrentItemId As Integer
            Dim CurrentItemDeleted As Boolean = False

            If Integer.TryParse(Args.CommandArgument.ToString, CurrentItemId) Then
                Dim CurrentShoppingCartController As New ShoppingCartController
                CurrentItemDeleted = CurrentShoppingCartController.Delete(CurrentItemId)
                CurrentShoppingCartController.Dispose()
            End If

            If CurrentItemDeleted Then
                LoadCart()
                LoadShippingRates()
            End If

            'Dim CurrentShoppingDetailsControl As ShoppingDetailsControl = CType(Me.Master.FindControl("MasterShoppingDetailsControl"), ShoppingDetailsControl)
            'CurrentShoppingDetailsControl.LoadDetails()
        End Sub

        Private Sub UpdateCartButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles UpdateCartButton.Click
            Dim CurrentShoppingCartController As New ShoppingCartController()

            Dim UpdateMessage As New StringBuilder
            'UpdateMessage.Append("<ul>")

            For Each CurrentRepeaterItem As RepeaterItem In ShoppingCartItemsRepeater.Items
                Dim ShoppingCartId As Integer = Integer.Parse(CType(CurrentRepeaterItem.FindControl("ShoppingCartIdHiddenField"), HiddenField).Value)

                Dim Quantity As Integer

                If Not Integer.TryParse(CType(CurrentRepeaterItem.FindControl("QuantityTextBox"), TextBox).Text, Quantity) Then
                    Quantity = 0
                End If

                'UpdateMessage.Append(String.Format("<li>{0}</li>", CurrentShoppingCartController.UpdateQuantity(ShoppingCartId, Quantity)))
                UpdateMessage.Append(String.Format("{0}" & vbNewLine, CurrentShoppingCartController.UpdateQuantity(ShoppingCartId, Quantity)))
            Next

            CurrentShoppingCartController.Dispose()

            'UpdateMessage.Append("</ul>")

            StatusMessageLabel.Attributes("class") = "success"
            StatusMessageLabel.InnerText = UpdateMessage.ToString
            StatusMessageLabel.Visible = True

            LoadCart()
            LoadShippingRates()

            Dim CurrentShoppingDetailsControl As ShoppingDetailsControl = CType(Me.Master.FindControl("MasterShoppingDetailsControl"), ShoppingDetailsControl)
            CurrentShoppingDetailsControl.LoadDetails()
        End Sub

        Private Sub CheckoutButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles CheckoutButton.Command
            Page.Validate("SearchAddress")
            Page.Validate("CheckoutGroup")

            If Page.IsValid Then
                Dim CurrentRate As CCFramework.Commerce.Shipping.ShippingRate

                If ShoppingCartFreeShipping Then
                    ShoppingCartSelectedRate = New ShippingOption(0D, Settings.CompanyName, "Free Shipping")
                Else
                    CurrentRate = GetSelectedShippingRate()
                    ShoppingCartSelectedRate = New ShippingOption(CDec(CurrentRate.Rate), CurrentRate.Company, CurrentRate.Name)
                End If

                If Not PromoCodeTextBox.Text Is Nothing AndAlso Not PromoCodeTextBox.Text = String.Empty Then
                    ShoppingCartPromoCode = PromoCodeTextBox.Text
                End If

                Response.RedirectToRoute("RouteCheckout")
            End If
        End Sub

        Private Function GetSelectedShippingRate() As CCFramework.Commerce.Shipping.ShippingRate
            Return ShoppingCartShippingRates.GetRateById(ShippingOptionsRadioList.SelectedValue)
        End Function

        Private Function ReadyForRates() As Boolean
            Dim CurrentShoppingCartController As New ShoppingCartController
            Dim CurrentShoppingCartItems As List(Of ShoppingCart)

            If ShoppingCartUserLoggedIn Then
                CurrentShoppingCartItems = CurrentShoppingCartController.GetShoppingCartItems(ShoppingCartRegisteredUser.UserID)

                If Not CurrentShoppingCartItems Is Nothing AndAlso CurrentShoppingCartItems.Count > 0 AndAlso Not AddressTextBox.Text = "" AndAlso Not CityTextBox.Text = "" AndAlso Not PostalCodeTextBox.Text = "" AndAlso Not CountryDropDown.SelectedValue = "" AndAlso Not ProvinceDropDown.SelectedValue = "" Then
                    CurrentShoppingCartController.Dispose()
                    Return True
                Else
                    CurrentShoppingCartController.Dispose()
                    Return False
                End If
            Else
                CurrentShoppingCartItems = CurrentShoppingCartController.GetShoppingCartItems(ShoppingCartSessionId)

                If Not CurrentShoppingCartItems Is Nothing AndAlso CurrentShoppingCartItems.Count > 0 AndAlso Not AddressTextBox.Text = "" AndAlso Not CityTextBox.Text = "" AndAlso Not PostalCodeTextBox.Text = "" AndAlso Not CountryDropDown.SelectedValue = "" AndAlso Not ProvinceDropDown.SelectedValue = "" Then
                    CurrentShoppingCartController.Dispose()
                    Return True
                Else
                    CurrentShoppingCartController.Dispose()
                    Return False
                End If
            End If

        End Function

        Private Function CartHasMembership() As Boolean
            CartHasMembership = Nothing

            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentUserId As Integer = ShoppingCartRegisteredUser.UserID

            Dim cartItems = (From sc In CurrentDataContext.ShoppingCarts _
                    Join p In CurrentDataContext.Products On p.ID Equals sc.ProductID _
                    Join c In CurrentDataContext.Colors On c.ID Equals sc.ColorID _
                    Join s In CurrentDataContext.Sizes On s.ID Equals sc.SizeID _
                    Where sc.UserID = CurrentUserId _
                    Select New With {sc.ID, sc.Quantity, .Name = p.Name, .Color = c.Name, .Size = s.Name, .ColorID = sc.ColorID, _
                    .SizeID = sc.SizeID, .Price = p.Price, .Total = (p.Price * sc.Quantity), .DefaultImageID = p.DefaultImageID, .Permalink = p.Permalink, .Category = p.Category.Permalink}).ToList()

            If cartItems.Count = 1 Then
                ShoppingCartItemsRepeater.DataSource = cartItems
                ShoppingCartItemsRepeater.DataBind()

                CalculateTotal()
            Else
                CartHasMembership = False
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Property ShoppingCartAddressId As Integer
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

        Public ReadOnly Property ShoppingCartSessionId As String
            Get
                Return Session("SessionId").ToString
            End Get
        End Property

        'Public Property ShoppingCartShippingOptions As Hashtable
        '    Get
        '        Return CType(Session("ShippingOptions"), Hashtable)
        '    End Get
        '    Set(Value As Hashtable)
        '        If Session("ShippingOptions") Is Nothing Then
        '            Session.Add("ShippingOptions", Value)
        '        Else
        '            Session.Remove("ShippingOptions")
        '            Session("ShippingOptions") = Value
        '        End If
        '    End Set
        'End Property

        Public Property ShoppingCartFreeShipping As Boolean
            Get
                If Session("FreeShipping") Is Nothing Then
                    Session.Add("FreeShipping", False)
                End If

                Return CBool(Session("FreeShipping"))
            End Get
            Set(Value As Boolean)
                If Session("FreeShipping") Is Nothing Then
                    Session.Add("FreeShipping", Value)
                Else
                    Session.Remove("FreeShipping")
                    Session("FreeShipping") = Value
                End If
            End Set
        End Property

        Public Property ShoppingCartPromoCode As String
            Get
                Return CStr(Session("PromoCode"))
            End Get
            Set(Value As String)
                If Session("PromoCode") Is Nothing Then
                    Session.Add("PromoCode", Value)
                Else
                    Session.Remove("PromoCode")
                    Session("PromoCode") = Value
                End If
            End Set
        End Property

        Public Property ShoppingCartShippingRates As ShippingRatesController
            Get
                Return CType(Session("ShippingRates"), ShippingRatesController)
            End Get
            Set(Value As ShippingRatesController)
                If Session("ShippingRates") Is Nothing Then
                    Session.Add("ShippingRates", Value)
                Else
                    Session.Remove("ShippingRates")
                    Session("ShippingRates") = Value
                End If
            End Set
        End Property

        Public Property ShoppingCartSelectedRate As ShippingOption
            Get
                Return CType(Session("SelectedRate"), ShippingOption)
            End Get
            Set(Value As ShippingOption)
                If Session("SelectedRate") Is Nothing Then
                    Session.Add("SelectedRate", Value)
                Else
                    Session.Remove("SelectedRate")
                    Session("SelectedRate") = Value
                End If
            End Set
        End Property

        Public ReadOnly Property ShoppingCartUserLoggedIn As Boolean
            Get
                Return CBool(Session("UserLoggedIn"))
            End Get
        End Property

        Public ReadOnly Property ShoppingCartRegisteredUser As RegisteredUser
            Get
                Return CType(Session("RegisteredUser"), RegisteredUser)
            End Get
        End Property

        Public ReadOnly Property ShoppingCartUserName As String
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

                Dim CurrentRegisteredUserController As New RegisteredUserController
                If Session("RegisteredUser") Is Nothing Then
                    Session.Add("RegisteredUser", CurrentRegisteredUserController.GetByUserName(ShoppingCartUserName))
                Else
                    Session("RegisteredUser") = CurrentRegisteredUserController.GetByUserName(ShoppingCartUserName)
                End If
                CurrentRegisteredUserController.Dispose()
            Else
                If Session("UserLoggedIn") Is Nothing Then
                    Session.Add("UserLoggedIn", False)
                Else
                    Session("UserLoggedIn") = False
                End If
            End If
        End Sub

    End Class

End Namespace