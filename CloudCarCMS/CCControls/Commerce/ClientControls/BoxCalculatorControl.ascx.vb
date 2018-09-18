Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.ShoppingCart

Namespace CCControls.Commerce.ClientControls

    Partial Public Class BoxCalculatorControl
        Inherits UserControl

        Public Event CartItemAdded(ByVal Sender As Object, ByVal E As EventArgs)

        Private Property ProductList() As List(Of TempRoomProducts)
            Get
                If Not Session("ProductList") Is Nothing Then
                    Return CType(Session("ProductList"), List(Of TempRoomProducts))
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As List(Of TempRoomProducts))
                If Session("ProductList") Is Nothing Then
                    Session.Add("ProductList", value)
                Else
                    Session("ProductList") = value
                End If
            End Set
        End Property

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            'btnAddToCart.OnClientClick = String.Format("fnClickAddToCart('{0}','{1}')", btnAddToCart.UniqueID, "")

            If Not Page.IsPostBack Then
                LoadBoxCalculator()
            End If
        End Sub

        Private Sub LoadBoxCalculator()
            rptBoxCalc.DataSource = CCFramework.Core.ClientControlls.BoxCalculatorController.Room.GetRooms()
            rptBoxCalc.DataBind()
        End Sub

        Private Function AddProductToList(ByVal ProductId As Integer, ByVal Quantity As Integer) As Boolean
            Dim ProductAdded As Boolean = False

            If Not ProductList Is Nothing Then
                For Each Item As TempRoomProducts In ProductList
                    If Item.ProductID = ProductId Then
                        Item.Quantity = Item.Quantity + Quantity
                        ProductAdded = True
                    End If
                Next
            Else
                ProductList = New List(Of TempRoomProducts)
            End If

            If Not ProductAdded Then
                Dim trp As New TempRoomProducts(ProductId, Quantity)

                ProductList.Add(trp)
            End If

            Return ProductAdded
        End Function

        Protected Sub btnBCSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            ProductList = Nothing

            For Each item As RepeaterItem In rptBoxCalc.Items
                Dim tb As TextBox = CType(item.FindControl("txtRoomCount"), TextBox)

                If Not tb.Text Is Nothing And Not tb.Text = String.Empty Then
                    Dim Quantity As Integer

                    If Integer.TryParse(tb.Text, Quantity) Then
                        Dim RoomID As Integer = Integer.Parse(tb.Attributes("RoomID"))
                        Dim products As IQueryable(Of BCRoomProduct)

                        products = CCFramework.Core.ClientControlls.BoxCalculatorController.RoomProduct.GetRoomProductsByRoom(RoomID)

                        For Each product As BCRoomProduct In products
                            If product.Reoccurs Then
                                AddProductToList(product.ProductID, product.Quantity)
                            Else
                                AddProductToList(product.ProductID, (product.Quantity * Quantity))
                            End If
                        Next
                    End If
                End If
            Next

            rptProducts.DataSource = ProductList
            rptProducts.DataBind()

            lblDisplaySelectedProducts.Visible = True

            mpeConfirm.Show()
        End Sub

        Protected Sub btnAddToCart_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            For Each item As TempRoomProducts In ProductList
                Dim shoppingCart As New ShoppingCartController()

                'Dim product As Product = New SMCommerce.ProductController().GetElement(item.ProductID)

                If Not Membership.GetUser Is Nothing Then
                    Dim user As RegisteredUser = New CCFramework.Core.RegisteredUserController().GetByUserName(Membership.GetUser.UserName)

                    shoppingCart.Create(Session("SessionId").ToString, user.UserID, item.ProductID, CCFramework.Core.Settings.NoColorID, CCFramework.Core.Settings.NoSizeID, item.Quantity)
                Else
                    shoppingCart.Create(Session("SessionId").ToString, -1, item.ProductID, CCFramework.Core.Settings.NoColorID, CCFramework.Core.Settings.NoSizeID, item.Quantity)
                End If
            Next

            RaiseEvent CartItemAdded(sender, New EventArgs())
        End Sub

        'Protected Sub btnContinueShopping_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '    mpeConfirm.Hide()
        'End Sub

        Private Class TempRoomProducts
            Public _productID As Integer
            Public _quantity As Integer

            Private product As Product

            Public Sub New(ByVal pID As Integer, ByVal qty As Integer)
                ProductID = pID
                Quantity = qty
            End Sub

            Public Property ProductID() As Integer
                Get
                    Return _productID
                End Get
                Set(ByVal value As Integer)
                    _productID = value

                    product = New ProductController().GetElement(ProductID)
                End Set
            End Property

            Public Property Quantity() As Integer
                Get
                    Return _quantity
                End Get
                Set(ByVal value As Integer)
                    _quantity = value
                End Set
            End Property

            Public ReadOnly Property Name() As String
                Get
                    Return product.Name
                End Get
            End Property

            Public ReadOnly Property ImageID() As Integer
                Get
                    Return product.DefaultImageID
                End Get
            End Property

            Public ReadOnly Property Permalink() As String
                Get
                    Return product.Permalink
                End Get
            End Property

            Public ReadOnly Property Category() As String
                Get
                    Return product.Category.Permalink
                End Get
            End Property

        End Class

    End Class

End Namespace