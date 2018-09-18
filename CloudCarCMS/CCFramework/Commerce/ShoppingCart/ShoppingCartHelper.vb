Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Commerce.ShoppingCart

    Public Class ShoppingCartHelper

        Public Shared GetShoppingCartSummaryItemsByUserId As Func(Of CommerceDataContext, Integer, IQueryable(Of ShoppingCartSummaryItem)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, UserId As Integer) From sc In db.ShoppingCarts _
                        Join p In db.Products On p.ID Equals sc.ProductID Join c In db.Colors On c.ID Equals sc.ColorID _
                        Join s In db.Sizes On s.ID Equals sc.SizeID Where sc.UserID = UserId _
                        Select New ShoppingCartSummaryItem(sc.ID, sc.Quantity, p.Name, sc.ColorID, c.Name, sc.SizeID, s.Name, p.Price, p.DefaultImageID, p.Permalink, p.Category.Permalink, p.PricingUnit))

        Public Shared GetShoppingCartSummaryItemsBySessionId As Func(Of CommerceDataContext, String, IQueryable(Of ShoppingCartSummaryItem)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, SessionId As String) From sc In db.ShoppingCarts _
                        Join p In db.Products On p.ID Equals sc.ProductID Join c In db.Colors On c.ID Equals sc.ColorID _
                        Join s In db.Sizes On s.ID Equals sc.SizeID Where sc.SessionID = SessionId _
                        Select New ShoppingCartSummaryItem(sc.ID, sc.Quantity, p.Name, sc.ColorID, c.Name, sc.SizeID, s.Name, p.Price, p.DefaultImageID, p.Permalink, p.Category.Permalink, p.PricingUnit))

        Public Sub New(ByVal SessionId As String)

        End Sub

        Private Function GetSubTotal() As Decimal
            'Dim total As Decimal = 0

            'If Not Membership.GetUser Is Nothing Then
            '    Dim username As String = Membership.GetUser().UserName
            '    Dim priceLevel As SMCommerce.EPriceLevel = CurrentUser.PriceLevel

            '    total = SMCommerce.ShoppingCartController.GetSubTotal(CurrentUser.UserID, priceLevel)
            'Else
            '    Dim SessionID As String = Session("SessionId").ToString

            '    total = SMCommerce.ShoppingCartController.GetSubTotal(SessionID, EPriceLevel.Regular)
            'End If

            'Return total
            Return 0
        End Function

        Private Sub CalculateTotal()
            'Dim total As Decimal = GetSubTotal()

            'hfTotal.Value = total.ToString

            'If rblSOptions.Items.Count > 0 Then
            '    Dim shipCharge As Decimal

            '    If Not Session("ShippingRates") Is Nothing Then
            '        Dim RateCollection As Shipping.ShippingRateCollection = CType(Session("ShippingRates"), Shipping.ShippingRateCollection)

            '        If Not rblSOptions.SelectedValue = String.Empty Then
            '            shipCharge = Math.Round(RateCollection.GetRateById(rblSOptions.SelectedValue).Rate, 2)
            '        Else
            '            shipCharge = 0
            '        End If
            '    Else
            '        shipCharge = 0
            '    End If

            '    hfSelectedRate.Value = shipCharge.ToString("n2")

            '    litShippingCharge.Text = shipCharge.ToString("n2")
            '    total += shipCharge
            'End If


            'litTotal.Text = total.ToString("C")
        End Sub

    End Class

    Public Class ShoppingCartSummaryItem
        Private mShoppingCartId As Integer
        Private mQuantity As Integer
        Private mName As String
        Private mColor As String
        Private mSize As String
        Private mColorId As Integer
        Private mSizeId As Integer
        Private mPrice As Decimal
        Private mDefaultImageId As Integer
        Private mPermalink As String
        Private mCategoryPermalink As String
        Private mPricingUnit As String

        Public Sub New(ByVal ShoppingCartId As Integer, ByVal Quantity As Integer, ByVal Name As String, ByVal ColorId As Integer, ByVal Color As String, ByVal SizeId As Integer, ByVal Size As String, ByVal Price As Decimal, ByVal DefaultImageId As Integer, ByVal Permalink As String, ByVal CategoryPermalink As String, PricingUnit As String)
            mShoppingCartId = ShoppingCartId
            mQuantity = Quantity
            mName = Name
            mColorId = ColorId
            mColor = Color
            mSizeId = SizeId
            mSize = Size
            mPrice = Price
            mDefaultImageId = DefaultImageId
            mPermalink = Permalink
            mCategoryPermalink = CategoryPermalink
            mPricingUnit = PricingUnit
        End Sub

        Public ReadOnly Property ShoppingCartId() As Integer
            Get
                Return mShoppingCartId
            End Get
        End Property

        Public Property Quantity() As Integer
            Get
                Return mQuantity
            End Get
            Set(ByVal value As Integer)
                mQuantity = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return mName
            End Get
            Set(ByVal value As String)
                mName = value
            End Set
        End Property

        Public Property ColorId() As Integer
            Get
                Return mColorId
            End Get
            Set(ByVal value As Integer)
                mColorId = value
            End Set
        End Property

        Public Property Color() As String
            Get
                Return mColor
            End Get
            Set(ByVal value As String)
                mColor = value
            End Set
        End Property

        Public Property SizeId() As Integer
            Get
                Return mSizeId
            End Get
            Set(ByVal value As Integer)
                mSizeId = value
            End Set
        End Property

        Public Property Size() As String
            Get
                Return mSize
            End Get
            Set(ByVal value As String)
                mSize = value
            End Set
        End Property

        Public Property Price() As Decimal
            Get
                Return mPrice
            End Get
            Set(ByVal value As Decimal)
                mPrice = value
            End Set
        End Property

        Public ReadOnly Property Total() As Decimal
            Get
                Return mPrice * mQuantity
            End Get
        End Property

        Public Property DefaultImageId() As Integer
            Get
                Return mDefaultImageId
            End Get
            Set(ByVal value As Integer)
                mDefaultImageId = value
            End Set
        End Property

        Public Property Permalink() As String
            Get
                Return mPermalink
            End Get
            Set(ByVal value As String)
                mPermalink = value
            End Set
        End Property

        Public Property CategoryPermalink() As String
            Get
                Return mCategoryPermalink
            End Get
            Set(ByVal value As String)
                mCategoryPermalink = value
            End Set
        End Property

        Public Property PricingUnit As String
            Get
                Return mPricingUnit
            End Get
            Set(value As String)
                mPricingUnit = value
            End Set
        End Property

    End Class

End Namespace