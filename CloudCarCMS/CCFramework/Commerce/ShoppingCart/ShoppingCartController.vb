Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Commerce.ShoppingCart

    Public Class ShoppingCartController
        Inherits DataControllerClass

        Public Shared GetShoppingCartByIdFunc As Func(Of CommerceDataContext, Integer, Model.ShoppingCart) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, Id As Integer) _
                                      (From sc In db.ShoppingCarts Where sc.ID = Id Select sc).FirstOrDefault)

        Public Shared GetShoppingCartsFunc As Func(Of CommerceDataContext, IQueryable(Of Model.ShoppingCart)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      From sc In db.ShoppingCarts Select sc)

        Public Shared GetShoppingCartsBySessionIdFunc As Func(Of CommerceDataContext, String, IQueryable(Of Model.ShoppingCart)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, SessionId As String) _
                                      From sc In db.ShoppingCarts Where sc.SessionID = SessionId Select sc)

        Public Shared GetShoppingCartsByUserIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Model.ShoppingCart)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, UserId As Integer) _
                                      From sc In db.ShoppingCarts Where sc.UserID = UserId Select sc)

        Public Shared GetShoppingCartWithSameOptionsFunc As Func(Of CommerceDataContext, String, Integer, Integer, Integer, Integer, Model.ShoppingCart) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, _
                                           SessionId As String,
                                           UserId As Integer,
                                           ProductId As Integer,
                                           SizeId As Integer,
                                           ColorId As Integer) _
                                      (From sc In db.ShoppingCarts _
                                      Where If(UserId > 0, sc.UserID = UserId, sc.SessionID = SessionId) _
                                      AndAlso sc.ProductID = ProductId _
                                      AndAlso sc.SizeID = SizeId _
                                      AndAlso sc.ColorID = ColorId _
                                      Select sc).FirstOrDefault)

        Public Overloads Function Create(ByVal SessionId As String, ByVal UserId As Integer, ByVal ProductId As Integer, ByVal ColorId As Integer, ByVal SizeId As Integer, ByVal Quantity As Integer) As Integer
            Dim CurrentShoppingCartMatch As Model.ShoppingCart = GetShoppingCartWithSameOptionsFunc(db, SessionId, UserId, ProductId, SizeId, ColorId)

            If CurrentShoppingCartMatch Is Nothing Then
                Dim CurrentShoppingCart As New Model.ShoppingCart
                Dim CurrentShoppingCartId As Integer

                CurrentShoppingCart.SessionID = SessionId
                CurrentShoppingCart.UserID = UserId
                CurrentShoppingCart.ProductID = ProductId
                CurrentShoppingCart.ColorID = ColorId
                CurrentShoppingCart.SizeID = SizeId
                CurrentShoppingCart.Quantity = Quantity
                CurrentShoppingCart.LastModified = DateTime.Now

                db.ShoppingCarts.InsertOnSubmit(CurrentShoppingCart)
                db.SubmitChanges()

                CurrentShoppingCartId = CurrentShoppingCart.ID

                Return CurrentShoppingCartId
            Else
                UpdateQuantity(CurrentShoppingCartMatch.ID, CurrentShoppingCartMatch.Quantity + Quantity)

                Return CurrentShoppingCartMatch.ID
            End If
        End Function

        Public Overloads Function Delete(ByVal ShoppingCartId As Integer) As Boolean
            Dim CurrentShoppingCart As Model.ShoppingCart = GetShoppingCartByIdFunc(db, ShoppingCartId)

            If CurrentShoppingCart Is Nothing Then
                Return False
            Else
                db.ShoppingCarts.DeleteOnSubmit(CurrentShoppingCart)
                db.SubmitChanges()

                Return True
            End If
        End Function

        Public Overloads Function Update(ByVal ShoppingCartId As Integer, ByVal SessionId As String, ByVal UserId As Integer, ByVal ProductId As Integer, ByVal ColorId As Integer, ByVal SizeId As Integer, ByVal Quantity As Integer) As Boolean
            Dim CurrentShoppingCart As Model.ShoppingCart

            CurrentShoppingCart = GetShoppingCartByIdFunc(db, ShoppingCartId)

            If CurrentShoppingCart Is Nothing Then
                Throw New Exception(String.Format("Shopping Cart with ID: {0} does not exist.", ShoppingCartId))
            Else
                CurrentShoppingCart.SessionID = SessionID
                CurrentShoppingCart.UserID = UserID
                CurrentShoppingCart.ProductID = ProductID
                CurrentShoppingCart.ColorID = ColorID
                CurrentShoppingCart.SizeID = SizeID
                CurrentShoppingCart.Quantity = Quantity
                CurrentShoppingCart.LastModified = DateTime.Now

                db.SubmitChanges()
            End If

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ShoppingCartId As Integer) As Model.ShoppingCart
            Dim CurrentShoppingCart As Model.ShoppingCart

            CurrentShoppingCart = GetShoppingCartByIdFunc(db, ShoppingCartId)

            If CurrentShoppingCart Is Nothing Then
                Throw New Exception(String.Format("Shopping Cart with ID: {0} does not exist.", ShoppingCartId))
            Else
                Return CurrentShoppingCart
            End If
        End Function

        Public Overloads Function GetElements() As List(Of Model.ShoppingCart)
            Dim CurrentShoppingCarts As List(Of Model.ShoppingCart) = GetShoppingCartsFunc(db).ToList

            If CurrentShoppingCarts Is Nothing OrElse CurrentShoppingCarts.Count < 1 Then
                Throw New Exception("There are no Shopping Carts")
            Else
                Return CurrentShoppingCarts
            End If
        End Function

        Public Function MoveShoppingCartItems(SessionId As String, UserId As Integer) As Boolean
            Dim CurrentCartItems As List(Of Model.ShoppingCart)

            CurrentCartItems = GetShoppingCartsBySessionIdFunc(db, SessionId).ToList

            If CurrentCartItems Is Nothing OrElse CurrentCartItems.Count < 1 Then
                Return False
            Else
                For Each CurrentItem In CurrentCartItems
                    CurrentItem.UserID = UserId
                Next

                db.SubmitChanges()

                Return True
            End If
        End Function
        
        Public Shared GetShoppingCartSubTotalBySessionIdFunc As Func(Of CommerceDataContext, String, EPriceLevel, IQueryable(Of Decimal)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, SessionId As String, PriceLevel As EPriceLevel) _
                                         From sc In db.ShoppingCarts Join p In db.Products On p.ID Equals sc.ProductID Where sc.SessionID = SessionId _
                                         Let Total = If(PriceLevel = EPriceLevel.Sales, p.PriceC, If(PriceLevel = EPriceLevel.Warehouse, p.PriceB, If(PriceLevel = EPriceLevel.Regular, p.Price, p.Price))) * sc.Quantity _
                                         Select Total)

        Public Function GetShoppingCartItems(ByVal SessionId As String) As List(Of Model.ShoppingCart)
            Dim CurrentCartItems As List(Of Model.ShoppingCart)

            CurrentCartItems = GetShoppingCartsBySessionIDFunc(db, SessionId).ToList

            If CurrentCartItems Is Nothing OrElse CurrentCartItems.Count < 1 Then
                Return Nothing
            Else
                Return CurrentCartItems
            End If
        End Function

        Public Shared Function GetItemCount(ByVal SessionId As String) As Integer
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentShoppingCartItems = GetShoppingCartsBySessionIDFunc(CurrentDataContext, SessionId).Select(Function(sc) New With {.Quantity = sc.Quantity}).ToList()

            If CurrentShoppingCartItems.Count > 0 Then
                Dim CurrentQuantity As Integer

                For Each CurrentItem In CurrentShoppingCartItems
                    CurrentQuantity += CurrentItem.Quantity
                Next

                GetItemCount = CurrentQuantity
            Else
                GetItemCount = 0
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetSubTotal(ByVal SessionId As String, ByVal PriceLevel As EPriceLevel) As Decimal
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentCartItems = GetShoppingCartSubTotalBySessionIdFunc(CurrentDataContext, SessionId, PriceLevel).ToList

            Dim CurrentTotal As Decimal = 0

            For Each Item In CurrentCartItems
                CurrentTotal += Item
            Next

            GetSubTotal = CurrentTotal

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetShoppingCartSubTotalByUserIdFunc As Func(Of CommerceDataContext, Integer, EPriceLevel, IQueryable(Of Decimal)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, UserId As Integer, PriceLevel As EPriceLevel) _
                                         From sc In db.ShoppingCarts Join p In db.Products On p.ID Equals sc.ProductID Where sc.UserID = UserId _
                                         Let Total = If(PriceLevel = EPriceLevel.Sales, p.PriceC, If(PriceLevel = EPriceLevel.Warehouse, p.PriceB, If(PriceLevel = EPriceLevel.Regular, p.Price, p.Price))) * sc.Quantity _
                                         Select Total)

        Public Function GetShoppingCartItems(ByVal UserId As Integer) As List(Of Model.ShoppingCart)
            Dim CurrentCartItems As List(Of Model.ShoppingCart)

            CurrentCartItems = GetShoppingCartsByUserIDFunc(db, UserId).ToList

            If CurrentCartItems Is Nothing OrElse CurrentCartItems.Count < 1 Then
                Return Nothing
            Else
                Return CurrentCartItems
            End If
        End Function

        Public Shared Function GetItemCount(ByVal UserId As Integer) As Integer
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentCartItems = GetShoppingCartsByUserIDFunc(CurrentDataContext, UserID).Select(Function(sc) New With {.Quantity = sc.Quantity})

            Dim CurrentQuantity As Integer = 0

            For Each Item In CurrentCartItems
                CurrentQuantity += Item.Quantity
            Next

            GetItemCount = CurrentQuantity

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetSubTotal(ByVal UserId As Integer, ByVal PriceLevel As EPriceLevel) As Decimal
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentCartItems = GetShoppingCartSubTotalByUserIdFunc(CurrentDataContext, UserId, PriceLevel).ToList

            Dim CurrentTotal As Decimal = 0

            For Each Item In CurrentCartItems
                CurrentTotal += Item
            Next

            GetSubTotal = CurrentTotal

            CurrentDataContext.Dispose()
        End Function

        Public Enum ShoppingCartUpdateState
            QuantityNotUpdated = 0  '"The quantity of this item cannot be updated."
            DoesNotExits = 1        '"The Shopping Cart with ID: " & ShoppingCartId.ToString & " does not exist."
            NotEnoughStock = 2      '"Sorry! We currently only have ( " & CurrentItemInventoryQuantity & " ) items in stock."
            SoldOut = 3             '"We are currently sold out of that item."
            Updated = 4             '"Thank you! Your cart has been updated!"
        End Enum

        Public Function UpdateQuantity(ByVal ShoppingCartId As Integer, ByVal Quantity As Integer) As String
            Dim CurrentMessage As String

            If SalesRequestController.CheckShoppingCartItemIsSalesRequest(ShoppingCartId) Then
                CurrentMessage = "You have an item in your cart which cannot be updated"
            Else
                Dim CurrentShoppingCart As Model.ShoppingCart

                CurrentShoppingCart = GetShoppingCartByIDFunc(db, ShoppingCartId)

                If CurrentShoppingCart Is Nothing Then
                    Throw New Exception("The Shopping Cart with ID: " & ShoppingCartId.ToString & " does not exist")
                Else
                    Dim CurrentProductController As New ProductController
                    Dim CurrentProduct As Model.Product = CurrentProductController.GetElement(CurrentShoppingCart.ProductID)
                    CurrentProductController.Dispose()

                    'So users can't use negative quantities and force a return
                    Dim AllowedQuantity As Integer
                    If Quantity < 0 Then
                        AllowedQuantity = 0
                    Else
                        AllowedQuantity = Quantity
                    End If

                    If CurrentProduct.TrackInventory = True Then
                        Dim CurrentInventoryController As New InventoryController
                        Dim CurrentItemInventoryQuantity As Integer = CurrentInventoryController.GetInventoryQuantity(CurrentShoppingCart.ProductID, CurrentShoppingCart.ColorID, CurrentShoppingCart.SizeID)
                        CurrentInventoryController.Dispose()

                        If CurrentItemInventoryQuantity >= AllowedQuantity Then
                            CurrentShoppingCart.Quantity = AllowedQuantity
                            CurrentMessage = CurrentProduct.Name & " has been updated!"
                        ElseIf CurrentItemInventoryQuantity > 0 Then
                            CurrentMessage = "Sorry! We currently only have <b>" & CurrentItemInventoryQuantity & "</b> " & CurrentProduct.Name & " in stock"
                        Else
                            'TODO if the item size and color combination is sold out, create a log message that will be displayed on the dashboard, indicating that there is a demand.
                            CurrentMessage = "Sorry! We are currently sold out of " & CurrentProduct.Name & ""
                        End If
                    Else
                        CurrentShoppingCart.Quantity = AllowedQuantity
                        CurrentShoppingCart.LastModified = DateTime.Now
                        CurrentMessage = CurrentProduct.Name & " has been updated!"
                    End If

                    db.SubmitChanges()
                End If
            End If

            Return CurrentMessage
        End Function

        Public Shared GetShoppingCartMembershipCountBySessionIdFunc As Func(Of CommerceDataContext, String, Integer) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, SessionId As String) _
                                         (From s In CurrentDataContext.ShoppingCarts _
                                          Where s.SessionID = SessionId And s.IsMembership _
                                          Select s).Count)

        Public Shared Function CartHasMembership(ByVal SessionId As String) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentItemCount As Integer = GetShoppingCartMembershipCountBySessionIdFunc(CurrentDataContext, SessionId)

            If CurrentItemCount > 0 Then
                CartHasMembership = True
            Else
                CartHasMembership = False
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetShoppingCartMembershipCountByUserIdFunc As Func(Of CommerceDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, UserId As Integer) _
                                         (From s In CurrentDataContext.ShoppingCarts _
                                          Where s.UserID = UserId And s.IsMembership _
                                          Select s).Count)

        Public Shared Function CartHasMembership(ByVal UserId As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentItemCount As Integer = GetShoppingCartMembershipCountByUserIdFunc(CurrentDataContext, UserId)

            If CurrentItemCount > 0 Then
                CartHasMembership = True
            Else
                CartHasMembership = False
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function ClearShoppingCart(ByVal SessionId As String) As Boolean
            Dim CurrentShoppingCartController As New ShoppingCartController

            Dim CurrentShoppingCartItems As List(Of Model.ShoppingCart) = CurrentShoppingCartController.GetShoppingCartItems(SessionId)

            ClearShoppingCart = Nothing

            For Each Item In CurrentShoppingCartItems
                ClearShoppingCart = CurrentShoppingCartController.Delete(Item.ID)
            Next

            CurrentShoppingCartController.Dispose()
        End Function

        Public Shared Function ClearShoppingCart(ByVal UserId As Integer) As Boolean
            Dim CurrentShoppingCartController As New ShoppingCartController

            Dim CurrentShoppingCartItems As List(Of Model.ShoppingCart) = CurrentShoppingCartController.GetShoppingCartItems(UserId)

            ClearShoppingCart = Nothing

            For Each Item In CurrentShoppingCartItems
                ClearShoppingCart = CurrentShoppingCartController.Delete(Item.ID)
            Next

            CurrentShoppingCartController.Dispose()
        End Function

        Public Shared Function ClearShoppingCartUserCheck(ByVal SessionId As String) As Boolean
            Dim CurrentShoppingCartController As New ShoppingCartController

            Dim CurrentShoppingCartItems As List(Of Model.ShoppingCart) = CurrentShoppingCartController.GetShoppingCartItems(SessionId)

            ClearShoppingCartUserCheck = Nothing

            For Each item In CurrentShoppingCartItems
                If item.UserID = -1 Then
                    ClearShoppingCartUserCheck = CurrentShoppingCartController.Delete(item.ID)
                End If
            Next

            CurrentShoppingCartController.Dispose()
        End Function

        Public Shared GetShoppingCartHasProductBySessionIdFunc As Func(Of CommerceDataContext, String, Integer, IQueryable(Of Model.ShoppingCart)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, SessionId As String, ProductId As Integer) _
                                      From s In db.ShoppingCarts Where s.SessionID = SessionId And s.ProductID = ProductId Select s)

        Public Shared GetShoppingCartHasProductByUserIdFunc As Func(Of CommerceDataContext, Integer, Integer, IQueryable(Of Model.ShoppingCart)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, UserId As Integer, ProductId As Integer) _
                                      From s In db.ShoppingCarts Where s.UserID = UserId And s.ProductID = ProductId Select s)

        Public Shared Function CartHasProduct(ByVal SessionId As String, ByVal ProductId As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentShoppingCartItems As List(Of Model.ShoppingCart) = GetShoppingCartHasProductBySessionIdFunc(CurrentDataContext, SessionId, ProductId).ToList

            If CurrentShoppingCartItems.Count > 0 Then
                CurrentDataContext.Dispose()

                Return True
            Else
                CurrentDataContext.Dispose()

                Return False
            End If
        End Function

        Public Shared Function CartHasProduct(ByVal UserId As Integer, ByVal ProductId As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentShoppingCartItems As List(Of Model.ShoppingCart) = GetShoppingCartHasProductByUserIdFunc(CurrentDataContext, UserId, ProductId).ToList

            If CurrentShoppingCartItems.Count > 0 Then
                CurrentDataContext.Dispose()

                Return True
            Else
                CurrentDataContext.Dispose()

                Return False
            End If
        End Function

        Public Shared GetUniqueShoppingCartCountFunc As Func(Of CommerceDataContext, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      (From s In db.ShoppingCarts Group By s.SessionID Into Group Select New With {.SessionId = SessionID}).Count)

        Public Shared GetUniqueUnregisteredShoppingCartCountFunc As Func(Of CommerceDataContext, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      (From s In db.ShoppingCarts Where s.UserID = -1 Group By s.SessionID Into Group Select New With {.SessionId = SessionID}).Count)

        Public Shared GetUniqueRegisteredShoppingCartCountFunc As Func(Of CommerceDataContext, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      (From s In db.ShoppingCarts Where s.UserID > 0 Group By s.SessionID Into Group Select New With {.SessionId = SessionID}).Count)

        Public Shared GetUnregisteredShoppingCartFunc As Func(Of CommerceDataContext, IQueryable(Of Model.ShoppingCart)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      From s In db.ShoppingCarts Where s.UserID = -1 Select s)

        Public Shared Function GetUniqueShoppingCartCount() As Integer
            Dim CurrentDataContext As New CommerceDataContext

            Dim Count As Integer = GetUniqueShoppingCartCountFunc(CurrentDataContext)

            CurrentDataContext.Dispose()

            Return Count
        End Function

        Public Shared Function GetUniqueUnregisteredShoppingCartCount() As Integer
            Dim CurrentDataContext As New CommerceDataContext

            Dim Count As Integer = GetUniqueUnregisteredShoppingCartCountFunc(CurrentDataContext)

            CurrentDataContext.Dispose()

            Return Count
        End Function

        Public Shared Function GetUniqueRegisteredShoppingCartCount() As Integer
            Dim CurrentDataContext As New CommerceDataContext

            Dim Count As Integer = GetUniqueRegisteredShoppingCartCountFunc(CurrentDataContext)

            CurrentDataContext.Dispose()

            Return Count
        End Function

        Public Shared Function DeleteUnregisteredShoppingCarts() As Boolean
            'Try
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentShoppingCartItems As IQueryable(Of Model.ShoppingCart) = GetUnregisteredShoppingCartFunc(CurrentDataContext)

            CurrentDataContext.ShoppingCarts.DeleteAllOnSubmit(CurrentShoppingCartItems)
            CurrentDataContext.SubmitChanges()

            CurrentDataContext.Dispose()

            Return True
            'Catch CurrentException As Exception
            'Return False
            'End Try
        End Function

    End Class

End Namespace