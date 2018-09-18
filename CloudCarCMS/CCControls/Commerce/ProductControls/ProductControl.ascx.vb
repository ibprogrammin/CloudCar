Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports System.Web.Security
Imports CloudCar.CCFramework.Commerce.ShoppingCart
Imports System.ComponentModel

Namespace CCControls.Commerce.ProductControls

    Partial Public Class ProductControl
        Inherits UserControl

        Private Const _ShowIcon As Boolean = True
        Private Const _ProductImageDisplaySize As Integer = 218
        Private Const _ProductNameDisplayLength As Integer = 32
        Private Const _ProductDescriptionDisplayLength As Integer = 120

        Public ProductImageClass As String = ""

        Private _CurrentProductShoppingCartMediator As ProductShoppingCartMediator

        <Bindable(True)> _
        <Category("Appearance")> _
        <DefaultValue("")> _
        <Localizable(True)> _
        Protected Property CurrentProductShoppingCartMediator As ProductShoppingCartMediator
            Get
                Return _CurrentProductShoppingCartMediator
            End Get
            Set(Value As ProductShoppingCartMediator)
                _CurrentProductShoppingCartMediator = Value
            End Set
        End Property

        Public Property ProductId() As Integer
            Get
                If Not ViewState("ProductID") Is Nothing Then
                    Return CInt(ViewState("ProductID"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal Value As Integer)
                If ViewState("ProductID") Is Nothing Then
                    ViewState.Add("ProductID", Value)
                Else
                    ViewState("ProductID") = Value
                End If

                LoadProduct()
            End Set
        End Property

        Private Sub LoadProduct()
            _CurrentProductShoppingCartMediator = New ProductShoppingCartMediator(Session("SessionId").ToString)

            Dim CurrentProductController As New ProductController

            Dim CurrentProduct As Product = CurrentProductController.GetElement(ProductId)
            Dim CurrentProductColors As List(Of Color) = CurrentProductController.GetColors(CurrentProduct.ID)

            If Not CurrentProductColors Is Nothing AndAlso CurrentProductColors.Count > 0 Then
                ColorOptionsRepeater.DataSource = CurrentProductColors.OrderBy(Function(f) f.RGBColourCode).ToList
                ColorOptionsRepeater.DataBind()
            End If

            'ColorRadioButton.DataSource = New ProductController().GetColors(CurrentProduct.ID)
            'ColorRadioButton.DataBind()

            'If ColorRadioButton.Items.Count < 1 Then
            'ColorRadioButton.Visible = False
            'End If

            SizeDropDown.DataSource = CurrentProductController.GetSizes(CurrentProduct.ID)
            SizeDropDown.DataBind()

            If SizeDropDown.Items.Count <= 1 Then
                SizeDropDown.Visible = False
            End If

            _CurrentProductShoppingCartMediator.SelectedProductId = CurrentProduct.ID
            _CurrentProductShoppingCartMediator.ItemIsMembership = CurrentProduct.Membership

            NameHyperLink.Text = ApplicationFunctions.StripShortString(CurrentProduct.Name, _ProductNameDisplayLength)
            NameHyperLink.ToolTip = CurrentProduct.Name
            NameHyperLink.NavigateUrl = String.Format("/Shop/{0}/{1}.html", CurrentProduct.Category.Permalink, CurrentProduct.Permalink)
            MoreImagesHyperLink.NavigateUrl = String.Format("~/CCCommerce/ProductImages.aspx?Product={0}", CurrentProduct.ID)

            If Not CurrentProduct.DefaultImageID = Nothing Then
                If CurrentProduct.TopSeller AndAlso _ShowIcon Then
                    ProductImage.Src = String.Format("/images/db/{0}/{1}/{2}.jpg?overlay=ts", CurrentProduct.DefaultImageID, _ProductImageDisplaySize, CurrentProduct.Permalink)
                ElseIf CurrentProduct.Clearance AndAlso _ShowIcon Then
                    ProductImage.Src = String.Format("/images/db/{0}/{1}/{2}.jpg?overlay=sale", CurrentProduct.DefaultImageID, _ProductImageDisplaySize, CurrentProduct.Permalink)
                Else
                    ProductImage.Src = String.Format("/images/db/{0}/{1}/{2}.jpg", CurrentProduct.DefaultImageID, _ProductImageDisplaySize, CurrentProduct.Permalink)
                End If

                'ProductImage.Src = String.Format("/images/db/{0}/{1}/{2}.jpg", CurrentProduct.DefaultImageID, _ProductImageDisplaySize, CurrentProduct.Permalink)
                ProductImage.Attributes("rel") = String.Format("/images/db/{0}/full/{1}.jpg", CurrentProduct.DefaultImageID, CurrentProduct.Permalink)
                ProductImage.Alt = CurrentProduct.ImageAlt
                ProductImage.Attributes("class") = String.Format("image-zoom-{0}", CurrentProduct.Permalink)
            Else
                ProductImage.Src = Settings.NoImageUrl
            End If

            DescriptionLiteral.Text = ApplicationFunctions.StripShortString(CurrentProduct.Description, _ProductDescriptionDisplayLength)
            FullTitleLiteral.Text = CurrentProduct.Name
            FullDescriptionLiteral.Text = CurrentProduct.Description

            Dim CurrentPrice As Decimal
            Dim CurrentListPrice As Decimal = CurrentProduct.ListPrice

            If Not Membership.GetUser Is Nothing Then
                Dim CurrentUserName As String = Membership.GetUser().UserName

                CurrentPrice = ProductController.GetPrice(RegisteredUserController.GetUserPriceLevel(CurrentUserName), CurrentProduct)
            Else
                CurrentPrice = CurrentProduct.Price
            End If

            If Not CurrentListPrice = 0 AndAlso CurrentListPrice > CurrentPrice Then
                Dim CurrentSavings As Decimal = CurrentListPrice - CurrentPrice
                Dim CurrentDiscount As Integer = CInt(Math.Round(((CurrentListPrice - CurrentPrice) * 100) / CurrentListPrice))

                ListPriceLiteral.Text = String.Format("{0:C}<br />", CurrentListPrice)
                SavingsLiteral.Text = String.Format("Save {0:C}", CurrentSavings)

                DiscountPanel.Controls.Add(New Literal() With {.Text = String.Format("Save {0}%", CurrentDiscount)})
                DiscountPanel.Visible = True
            End If

            PriceLiteral.Text = String.Format("{0:C} {1}<br />", CurrentPrice, CurrentProduct.PricingUnit)

            Dim NewUniqueId As Guid = Guid.NewGuid

            AddToCartButton.ValidationGroup = "vgProduct" & NewUniqueId.ToString
            rfvQuantity.ValidationGroup = "vgProduct" & NewUniqueId.ToString

            CurrentProductController.Dispose()
        End Sub

        Private Sub AddToCartButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles AddToCartButton.Command
            If ColorOptionsRepeater.Items.Count > 0 AndAlso _CurrentProductShoppingCartMediator.SelectedColor = Settings.NoColorID Then
                StatusMessageLabel.Text = String.Format("<h4>You must select a color!</h4>")
                StatusMessageLabel.Visible = True
            Else
                'If ColorRadioButton.Items.Count < 1 Then
                '    CurrentProductDisplayHelper.SelectedColor = Settings.NoColorID
                'Else
                '    CurrentProductDisplayHelper.SelectedColor = Integer.Parse(ColorRadioButton.SelectedValue)
                'End If

                If SizeDropDown.Items.Count <= 1 Then
                    _CurrentProductShoppingCartMediator.SelectedSize = Settings.NoSizeID
                Else
                    _CurrentProductShoppingCartMediator.SelectedSize = Integer.Parse(SizeDropDown.SelectedValue)
                End If

                If Not Integer.TryParse(QuantityTextBox.Text, _CurrentProductShoppingCartMediator.SelectedQuantity) Then
                    _CurrentProductShoppingCartMediator.SelectedQuantity = 1
                End If

                _CurrentProductShoppingCartMediator.ProductAddingToCart(Sender, New EventArgs())

                Select Case _CurrentProductShoppingCartMediator.CurrentAddShoppingCartItemState
                    Case AddShoppingCartItemState.AddItem
                        Response.RedirectToRoute("RouteShoppingCartA")
                    Case AddShoppingCartItemState.AddMembership
                        Response.Redirect(String.Format("~/CCCommerce/Membership/MemberApp.aspx?ProductID={0}", _CurrentProductShoppingCartMediator.SelectedProductId))
                    Case Else
                        StatusMessageLabel.Text = String.Format("<h4>{0}</h4>", _CurrentProductShoppingCartMediator.StatusMessage)
                        StatusMessageLabel.Visible = True
                End Select
            End If
        End Sub

        Protected Sub ColorSelectionClicked(ByVal Sender As Object, ByVal Args As CommandEventArgs)
            'TODO Change the product image when the color is changed, if there is one available
            Dim ColourId As Integer = Integer.Parse(Args.CommandArgument.ToString)
            _CurrentProductShoppingCartMediator.SelectedColor = ColourId

            ColorOptionsRepeater.DataSource = New ProductController().GetColors(ProductId).OrderBy(Function(f) f.RGBColourCode).ToList
            ColorOptionsRepeater.DataBind()
        End Sub

        Protected Overrides Sub OnInit(Args As EventArgs)
            MyBase.OnInit(Args)
            Page.RegisterRequiresControlState(Me)
        End Sub

        Protected Overrides Function SaveControlState() As Object
            Dim BaseState As Object = MyBase.SaveControlState()
            Return New Pair(BaseState, _CurrentProductShoppingCartMediator)
        End Function

        Protected Overrides Sub LoadControlState(SavedState As Object)
            Dim Value As Pair = CType(SavedState, Pair)
            _CurrentProductShoppingCartMediator = CType(Value.Second, ProductShoppingCartMediator)
        End Sub

    End Class

    Public Class ProductControlEventArgs
        Inherits EventArgs

        Public Sub New(ByVal _IsRegisteredUser As Boolean, ByVal _HasMembership As Boolean, ByVal _ProductID As Integer, ByVal _SizeID As Integer, ByVal _ColorID As Integer, ByVal _Quantity As Integer, ByVal _Inventory As Integer)
            IsRegisteredUser = _IsRegisteredUser
            HasMembership = _HasMembership
            ProductID = _ProductID
            SizeID = _SizeID
            ColorID = _ColorID
            Quantity = _Quantity
            Inventory = _Inventory
        End Sub

        Private pIsRegisteredUser As Boolean
        Public Property IsRegisteredUser() As Boolean
            Get
                Return pIsRegisteredUser
            End Get
            Set(ByVal value As Boolean)
                pIsRegisteredUser = value
            End Set
        End Property

        Private pHasMembership As Boolean
        Public Property HasMembership() As Boolean
            Get
                Return pHasMembership
            End Get
            Set(ByVal value As Boolean)
                pHasMembership = value
            End Set
        End Property

        Private pProductID As Integer
        Public Property ProductID() As Integer
            Get
                Return pProductID
            End Get
            Set(ByVal value As Integer)
                pProductID = value
            End Set
        End Property

        Private cColorID As Integer
        Public Property ColorID() As Integer
            Get
                Return cColorID
            End Get
            Set(ByVal value As Integer)
                cColorID = value
            End Set
        End Property

        Private cSizeID As Integer
        Public Property SizeID() As Integer
            Get
                Return cSizeID
            End Get
            Set(ByVal value As Integer)
                cSizeID = value
            End Set
        End Property

        Private pQuantity As Integer
        Public Property Quantity() As Integer
            Get
                Return pQuantity
            End Get
            Set(ByVal value As Integer)
                pQuantity = value
            End Set
        End Property

        Private pInventory As Integer
        Public Property Inventory() As Integer
            Get
                Return pInventory
            End Get
            Set(ByVal value As Integer)
                pInventory = value
            End Set
        End Property

    End Class

End Namespace