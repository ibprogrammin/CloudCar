Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.ShoppingCart

Namespace CCCommerce.Mobile

    Partial Public Class MobileShoppingCart
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not CCFramework.Core.Settings.StoreEnabled Then
                Response.Redirect(CCFramework.Core.Settings.StoreDisabledPage)
            End If

            If Not Page.IsPostBack Then
                If GetPostData() Then
                    RefreshDataSources()
                    LoadCart()
                End If
            End If
        End Sub

        Public Structure ShoppingItem
            Dim ProductID As Integer
            Dim ColourID As Integer
            Dim SizeID As Integer
            Dim Quantity As Integer
        End Structure

        Private Function GetPostData() As Boolean
            If Not Request.Form Is Nothing Then
                Dim Products As New List(Of ShoppingItem)

                For i As Integer = 0 To Request.Form.Count - 1
                    Dim key As String = Request.Form.GetKey(i)

                    If key.StartsWith("Product") Then
                        Dim values As String() = Request.Form(i).Split(","c)

                        Dim si As New ShoppingItem

                        If Not Integer.TryParse(values(0), si.ProductID) Then
                            si.ProductID = -1
                        Else
                            If Not Integer.TryParse(values(1), si.ColourID) Then
                                si.ColourID = -1
                            End If
                            If Not Integer.TryParse(values(2), si.SizeID) Then
                                si.SizeID = -1
                            End If
                            If Not Integer.TryParse(values(3), si.Quantity) Then
                                si.Quantity = 0
                            End If
                        End If

                        If Not si.Quantity = 0 Then
                            Products.Add(si)
                        End If
                    End If
                Next

                Dim SessionId As String = CType(Session("SessionId"), Guid).ToString

                ShoppingCartController.ClearShoppingCart(sessionID)

                For Each item As ShoppingItem In Products
                    Dim scid As Integer = New ShoppingCartController().Create(SessionId, -1, item.ProductID, item.ColourID, item.SizeID, item.Quantity)
                Next

                Return True
            Else
                lblStatus.Text &= "You did not submit any products. <br />"

                Return False
            End If

        End Function

        Private Sub RefreshDataSources()
            ddlCountry.Items.Clear()
            ddlCountry.Items.Add(New ListItem("Country", ""))
            ddlCountry.AppendDataBoundItems = True
            ddlCountry.DataSource = New CountryController().GetElements
            ddlCountry.DataBind()

            ddlProvince.Items.Clear()
            ddlProvince.Items.Add(New ListItem("Province/State", ""))
            ddlProvince.AppendDataBoundItems = True
            ddlProvince.DataSource = New ProvinceController().GetElements
            ddlProvince.DataBind()
        End Sub

        Private Sub LoadCart()
            Dim db As New CommerceDataContext

            Dim cartItems = From sc In db.ShoppingCarts _
                    Join p In db.Products On p.ID Equals sc.ProductID _
                    Join c In db.Colors On c.ID Equals sc.ColorID _
                    Join s In db.Sizes On s.ID Equals sc.SizeID _
                    Where sc.SessionID = Session("SessionId").ToString _
                    Select New With {sc.ID, sc.Quantity, .Name = p.Name, .Color = c.Name, .Size = s.Name, .ColorID = sc.ColorID, _
                    .SizeID = sc.SizeID, .Price = p.Price, .Total = (p.Price * sc.Quantity), _
                    .DefaultImageID = p.DefaultImageID, .Permalink = p.Permalink, .Category = p.Category.Permalink}

            If cartItems.Count > 0 Then
                rptSCItems.DataSource = cartItems
                rptSCItems.DataBind()

                CalculateTotal()
            Else
                phSC.Visible = False

                lblStatus.Visible = True
                lblStatus.Text = "You have no items in your shopping cart."

                btnCheckout.Visible = False
            End If

            db.Dispose()
        End Sub

        Private Sub CalculateTotal()
            Dim db As New CommerceDataContext

            Dim total As Decimal

            If Not System.Web.Security.Membership.GetUser Is Nothing Then
                Dim userID As Integer = New CCFramework.Core.RegisteredUserController().GetByUserName(System.Web.Security.Membership.GetUser.UserName).UserID

                Dim username As String = System.Web.Security.Membership.GetUser().UserName
                Dim priceLevel As EPriceLevel = CCFramework.Core.RegisteredUserController.GetUserPriceLevel(username)

                Dim cartItems = From sc In db.ShoppingCarts _
                        Join p In db.Products On p.ID Equals sc.ProductID _
                        Where sc.UserID = userID _
                        Select New With {.Total = ProductController.GetPrice(priceLevel, p) * sc.Quantity}

                For Each item In cartItems
                    total += item.Total
                Next
            Else
                Dim cartItems = From sc In db.ShoppingCarts _
                        Join p In db.Products On p.ID Equals sc.ProductID _
                        Where sc.SessionID = Session("SessionId").ToString _
                        Select New With {.Total = (p.Price * sc.Quantity)}

                For Each item In cartItems
                    total += item.Total
                Next
            End If

            litTotal.Text = total.ToString("C")

            db.Dispose()
        End Sub

        Private Sub btnCheckout_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnCheckout.Command
            Page.Validate("Checkout")

            If Page.IsValid Then
                Dim addressId As Integer

                If Not Session("AddressID") Is Nothing Then
                    addressId = CType(Session("AddressID"), Integer)

                    Dim updateAddress As Boolean = New AddressController().Update(addressId, txtAddress.Text, txtCity.Text, txtPostalCode.Text, Integer.Parse(ddlProvince.SelectedValue), Integer.Parse(ddlCountry.SelectedValue))
                Else
                    addressId = New AddressController().Create(txtAddress.Text, txtCity.Text, txtPostalCode.Text, Integer.Parse(ddlProvince.SelectedValue), Integer.Parse(ddlCountry.SelectedValue))
                    Session.Add("AddressID", addressId)
                End If

                If Not txtPromoCode.Text Is Nothing And Not txtPromoCode.Text = String.Empty Then
                    Dim PromoCode As String = txtPromoCode.Text

                    If Not Session("PromoCode") Is Nothing Then
                        Session("PromoCode") = PromoCode
                    Else
                        Session.Add("PromoCode", PromoCode)
                    End If
                End If


                Response.Redirect("~/CCCommerce/Mobile/MobileCheckout.aspx")
            End If
        End Sub

        Private Function ReadyForRates() As Boolean
            Dim sc As New ShoppingCartController
            If Not System.Web.Security.Membership.GetUser Is Nothing Then
                If sc.GetShoppingCartItems(New CCFramework.Core.RegisteredUserController().GetByUserName(System.Web.Security.Membership.GetUser.UserName).UserID).Count > 0 And Not txtAddress.Text = "" And Not txtCity.Text = "" And Not txtPostalCode.Text = "" And Not ddlCountry.SelectedValue = "" And Not ddlProvince.SelectedValue = "" Then
                    Return True
                Else
                    Return False
                End If
            Else
                If sc.GetShoppingCartItems(Session("SessionId").ToString).Count > 0 And Not txtAddress.Text = "" And Not txtCity.Text = "" And Not txtPostalCode.Text = "" And Not ddlCountry.SelectedValue = "" And Not ddlProvince.SelectedValue = "" Then
                    Return True
                Else
                    Return False
                End If
            End If

        End Function

        Private Function CartHasMembership(ByVal username As String) As Boolean
            Dim db As New CommerceDataContext

            Dim userID As Integer = New CCFramework.Core.RegisteredUserController().GetByUserName(System.Web.Security.Membership.GetUser.UserName).UserID

            Dim cartItems = From sc In db.ShoppingCarts _
                    Join p In db.Products On p.ID Equals sc.ProductID _
                    Join c In db.Colors On c.ID Equals sc.ColorID _
                    Join s In db.Sizes On s.ID Equals sc.SizeID _
                    Where sc.UserID = userID _
                    Select New With {sc.ID, sc.Quantity, .Name = p.Name, .Color = c.Name, .Size = s.Name, .ColorID = sc.ColorID, _
                    .SizeID = sc.SizeID, .Price = p.Price, .Total = (p.Price * sc.Quantity), .DefaultImageID = p.DefaultImageID, .Permalink = p.Permalink, .Category = p.Category.Permalink}

            If cartItems.Count = 1 Then
                rptSCItems.DataSource = cartItems
                rptSCItems.DataBind()
                'dgSCItems.DataSource = cartItems
                'dgSCItems.DataBind()

                CalculateTotal()

                Return True
            Else
                Return False
            End If

        End Function

    End Class
End NameSpace