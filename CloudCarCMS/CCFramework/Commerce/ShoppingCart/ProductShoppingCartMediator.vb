Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model
Imports System.Web.Security

Namespace CCFramework.Commerce.ShoppingCart

    Public Interface IProductShoppingCartMediator
        Event AddingToCart(ByVal Sender As Object, ByVal Args As EventArgs)
        Event AddToCart(ByVal Sender As Object, ByVal Args As EventArgs)
        Event CartItemAdded(ByVal Sender As Object, ByVal Args As EventArgs)
        Event OutOfStock(ByVal Sender As Object, ByVal Args As EventArgs)
        Event AddMembership(ByVal Sender As Object, ByVal Args As EventArgs)

        Property ProductId As Integer
        Property SelectedColor As Integer
        Property SelectedSize As Integer
        Property SelectedQuantity As Integer
        Property IsMembership As Boolean

        Property CurrentAddShoppingCartItemState As AddShoppingCartItemState

        Property SessionId As String

        Property StatusMessage As String
    End Interface

    <Serializable()> _
    Public Class ProductShoppingCartMediator
        Implements IProductShoppingCartMediator

        Public Event AddingToCart(ByVal Sender As Object, ByVal Args As EventArgs) Implements IProductShoppingCartMediator.AddingToCart
        Public Event AddToCart(ByVal Sender As Object, ByVal Args As EventArgs) Implements IProductShoppingCartMediator.AddToCart
        Public Event CartItemAdded(ByVal Sender As Object, ByVal Args As EventArgs) Implements IProductShoppingCartMediator.CartItemAdded
        Public Event OutOfStock(ByVal Sender As Object, ByVal Args As EventArgs) Implements IProductShoppingCartMediator.OutOfStock
        Public Event AddMembership(ByVal Sender As Object, ByVal Args As EventArgs) Implements IProductShoppingCartMediator.AddMembership

        Public Sub New(SessionId As String)
            _SessionId = SessionId
            _CurrentAddShoppingCartItemState = AddShoppingCartItemState.None
            _StatusMessage = ""

            If Membership.GetUser Is Nothing Then
                _IsRegisteredUser = False
            Else
                _IsRegisteredUser = True
                _CurrentUserName = Membership.GetUser.UserName
            End If

            _SelectedColor = Settings.NoColorID
            _SelectedSize = Settings.NoSizeID
            _SelectedQuantity = 0
        End Sub

        Public Sub SetItemDetails(ProductId As Integer, ColorId As Integer, SizeId As Integer, Quantity As Integer, IsMembership As Boolean)
            _ProductId = ProductId
            _SelectedColor = ColorId
            _SelectedSize = SizeId
            _SelectedQuantity = Quantity
            _IsMembership = IsMembership
        End Sub

        Private ReadOnly _IsRegisteredUser As Boolean
        Private ReadOnly _CurrentUserName As String

        Private _TrackInventory As Boolean
        Private _CurrentInventory As Integer

        Private _HasMembership As Boolean

        Public Sub ProductAddingToCart(Sender As Object, Args As EventArgs) Handles Me.AddingToCart
            Dim CurrentProductController As New ProductController
            Dim CurrentProduct As Model.Product = CurrentProductController.GetElement(SelectedProductId)
            CurrentProductController.Dispose()

            _CurrentInventory = 0
            _TrackInventory = CurrentProduct.TrackInventory

            If _TrackInventory = True Then
                Dim CurrentInventoryController As New InventoryController
                _CurrentInventory = CurrentInventoryController.GetInventoryQuantity(SelectedProductId, SelectedColor, SelectedSize)
                CurrentInventoryController.Dispose()

                If _CurrentInventory >= SelectedQuantity Then
                    RaiseEvent AddToCart(Sender, New EventArgs())
                ElseIf _CurrentInventory > 0 Then
                    RaiseEvent OutOfStock(Sender, New EventArgs())
                Else
                    RaiseEvent OutOfStock(Sender, New EventArgs())
                End If
            Else
                RaiseEvent AddToCart(Sender, New EventArgs())
            End If

        End Sub

        Public Sub ProductAddToCart(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.AddToCart
            Dim CurrentShoppingCartController As New ShoppingCartController()
            Dim CurrentRegisteredUserController As New RegisteredUserController
            Dim CurrentRegisteredUser As RegisteredUser

            If ItemIsMembership Then
                If _IsRegisteredUser Then
                    CurrentRegisteredUser = CurrentRegisteredUserController.GetByUserName(_CurrentUserName)

                    If RegisteredUserController.UserHasMembership(CurrentRegisteredUser.UserName) OrElse ShoppingCartController.CartHasMembership(CurrentRegisteredUser.UserID) Then
                        _HasMembership = True
                        RaiseEvent AddMembership(Sender, New EventArgs())
                    Else
                        _HasMembership = False
                        RaiseEvent AddMembership(Sender, New EventArgs())
                    End If
                Else
                    If ShoppingCartController.CartHasMembership(SessionId) Then
                        _HasMembership = True
                        RaiseEvent AddMembership(Sender, New EventArgs())
                    Else
                        _HasMembership = False
                        RaiseEvent AddMembership(Sender, New EventArgs())
                    End If
                End If
            Else
                Dim CurrentCartHasMembership As Boolean

                If _IsRegisteredUser Then
                    CurrentRegisteredUser = CurrentRegisteredUserController.GetByUserName(_CurrentUserName)

                    CurrentCartHasMembership = ShoppingCartController.CartHasMembership(CurrentRegisteredUser.UserID)

                    If Not CurrentCartHasMembership Then
                        CurrentShoppingCartController.Create(SessionId, CurrentRegisteredUser.UserID, SelectedProductId, SelectedColor, SelectedSize, SelectedQuantity)
                    End If
                Else
                    CurrentCartHasMembership = ShoppingCartController.CartHasMembership(SessionId)

                    If Not CurrentCartHasMembership Then
                        CurrentShoppingCartController.Create(SessionId, -1, SelectedProductId, SelectedColor, SelectedSize, SelectedQuantity)
                    End If
                End If

                If Not CurrentCartHasMembership Then
                    RaiseEvent CartItemAdded(Sender, New EventArgs())
                End If
            End If

            CurrentRegisteredUserController.Dispose()
            CurrentShoppingCartController.Dispose()
        End Sub

        Protected Sub ProductCartItemAdded(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.CartItemAdded
            _CurrentAddShoppingCartItemState = AddShoppingCartItemState.AddItem
        End Sub

        Protected Sub ProductOutOfStock(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.OutOfStock
            If _CurrentInventory = 0 Then
                StatusMessage = String.Format("Sorry! This item is out of stock")
            Else
                StatusMessage = String.Format("Sorry! There is only {0} available.", _CurrentInventory)
            End If
        End Sub

        Protected Sub ProductAddMembership(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.AddMembership
            If _IsRegisteredUser Then
                If Not _HasMembership Then
                    CurrentAddShoppingCartItemState = AddShoppingCartItemState.AddMembership
                Else
                    StatusMessage = String.Format("You already have a membership set up for this account or a membership in your shopping cart. If you would like to purchase a membership for another family member, you must sign out, or create a new account.")
                End If
            Else
                If Not _HasMembership Then
                    CurrentAddShoppingCartItemState = AddShoppingCartItemState.AddMembership
                Else
                    StatusMessage = String.Format("You already have a membership set up for this account or a membership in your shopping cart. If you would like to purchase a membership for another family member, you must sign out, or create a new account.")
                End If
            End If
        End Sub

        Private _ProductId As Integer
        Private _SelectedColor As Integer
        Private _SelectedSize As Integer
        Private _SelectedQuantity As Integer
        Private _IsMembership As Boolean

        Private _CurrentAddShoppingCartItemState As AddShoppingCartItemState

        Private _StatusMessage As String
        Private _SessionId As String

        Public Property SelectedColor() As Integer Implements IProductShoppingCartMediator.SelectedColor
            Get
                Return _SelectedColor
            End Get
            Set(ByVal Value As Integer)
                _SelectedColor = Value
            End Set
        End Property

        Public Property SelectedSize() As Integer Implements IProductShoppingCartMediator.SelectedSize
            Get
                Return _SelectedSize
            End Get
            Set(ByVal Value As Integer)
                _SelectedSize = Value
            End Set
        End Property

        Public Property SelectedQuantity() As Integer Implements IProductShoppingCartMediator.SelectedQuantity
            Get
                Return _SelectedQuantity
            End Get
            Set(ByVal Value As Integer)
                _SelectedQuantity = Value
            End Set
        End Property

        Public Property SelectedProductId() As Integer Implements IProductShoppingCartMediator.ProductId
            Get
                Return _ProductId
            End Get
            Set(ByVal Value As Integer)
                _ProductId = Value
            End Set
        End Property

        Public Property CurrentAddShoppingCartItemState() As AddShoppingCartItemState Implements IProductShoppingCartMediator.CurrentAddShoppingCartItemState
            Get
                Return _CurrentAddShoppingCartItemState
            End Get
            Set(ByVal Value As AddShoppingCartItemState)
                _CurrentAddShoppingCartItemState = Value
            End Set
        End Property

        Public Property SessionId() As String Implements IProductShoppingCartMediator.SessionId
            Get
                Return _SessionId
            End Get
            Set(ByVal Value As String)
                _SessionId = Value
            End Set
        End Property

        Public Property StatusMessage() As String Implements IProductShoppingCartMediator.StatusMessage
            Get
                Return _StatusMessage
            End Get
            Set(ByVal Value As String)
                _StatusMessage = Value
            End Set
        End Property

        Public Property ItemIsMembership() As Boolean Implements IProductShoppingCartMediator.IsMembership
            Get
                Return _IsMembership
            End Get
            Set(ByVal Value As Boolean)
                _IsMembership = Value
            End Set
        End Property
        
    End Class

    Public Enum AddShoppingCartItemState
        None = 0
        AddItem = 1
        AddMembership = 2
        OutOfStock = 3
    End Enum

End Namespace