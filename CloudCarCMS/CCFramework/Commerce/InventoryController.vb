Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Commerce

    Public Class InventoryController
        Inherits DataControllerClass

        Public Shared GetInventoryByIDFunc As Func(Of CommerceDataContext, Integer, Inventory) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, ID As Integer) _
                                      (From i In db.Inventories Where i.ID = ID Select i).FirstOrDefault)

        Public Shared GetInventoryFunc As Func(Of CommerceDataContext, IQueryable(Of Inventory)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      From i In db.Inventories Select i)

        Public Shared GetInventoryByProductColorSizeIDFunc As Func(Of CommerceDataContext, Integer, Integer, Integer, IQueryable(Of Inventory)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, ProductID As Integer, ColorID As Integer, SizeID As Integer) _
                                      From i In db.Inventories Where i.ProductID = ProductID _
                                       And i.ColorID = ColorID And i.SizeID = SizeID Select i)

        Public Overloads Function Create(ByVal productId As Integer, ByVal colorID As Integer, ByVal sizeID As Integer, ByVal Quantity As Integer) As Integer
            Dim item As New Inventory
            Dim itemId As Integer

            item.ProductID = productId
            item.ColorID = colorID
            item.SizeID = sizeID
            item.Quantity = Quantity
            item.LastModified = DateTime.Now

            db.Inventories.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = GetInventoryByIDFunc(db, ID) ' (From i In db.Inventories Where i.ID = ID Select i).First

                db.Inventories.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal productID As Integer, ByVal colorID As Integer, ByVal sizeID As Integer, ByVal quantity As Integer) As Boolean
            Dim item As Inventory

            item = GetInventoryByIDFunc(db, ID) ' (From i In db.Inventories Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("Inventory with ID: " & ID.ToString & " does not exist.")
            Else
                item.ProductID = productID
                item.ColorID = colorID
                item.SizeID = sizeID
                item.Quantity = quantity
                item.LastModified = DateTime.Now

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function UpdateQuantity(ByVal ID As Integer, ByVal quantity As Integer) As Boolean
            Dim item As Inventory

            item = GetInventoryByIDFunc(db, ID) ' (From i In db.Inventories Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("Inventory with ID: " & ID.ToString & " does not exist.")
            Else
                item.Quantity = quantity
                item.LastModified = DateTime.Now

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function AddInventory(ByVal ID As Integer, ByVal Amount As Integer) As Boolean
            Dim item As Inventory

            item = GetInventoryByIDFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("Inventory with ID: " & ID.ToString & " does not exist.")
            Else
                item.Quantity = item.Quantity + Amount
                item.LastModified = DateTime.Now

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function SubtractInventory(ByVal ID As Integer, ByVal Amount As Integer) As Boolean
            Dim item As Inventory

            item = GetInventoryByIDFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("Inventory with ID: " & ID.ToString & " does not exist.")
            Else
                item.Quantity = item.Quantity - Amount
                item.LastModified = DateTime.Now

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function UpdateProductInventory(ByVal productID As Integer, ByVal colorID As Integer, ByVal sizeID As Integer, ByVal quantity As Integer) As Boolean
            Dim item As Inventory

            item = GetInventoryByProductColorSizeIDFunc(db, productID, colorID, sizeID).FirstOrDefault ' (From i In db.Inventories Where i.ProductID = productID And i.ColorID = colorID And i.SizeID = sizeID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("Inventory with product: " & productID.ToString & " does not exist.")
            Else
                item.ProductID = productID
                item.ColorID = colorID
                item.SizeID = sizeID
                item.Quantity = quantity
                item.LastModified = DateTime.Now

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As Inventory
            Dim item As Inventory

            item = GetInventoryByIDFunc(db, ID) ' (From i In db.Inventories Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("Inventory with ID: " & ID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of Inventory)
            Dim itemList As New List(Of Inventory)

            Dim items = GetInventoryFunc(db) ' From c In db.Inventories Select c

            If items Is Nothing And items.Count > 0 Then
                Throw New Exception("There are no Inventory items")
            Else
                For Each e As Inventory In items
                    itemList.Add(e)
                Next

                Return itemList
            End If
        End Function

        Public Overloads Function GetGridElements() As List(Of Object)
            Dim itemList As New List(Of Object)

            Dim items = From i In db.Inventories _
                        Join p In db.Products On i.ProductID Equals p.ID _
                        Join c In db.Colors On i.ColorID Equals c.ID _
                        Join s In db.Sizes On i.SizeID Equals s.ID _
                        Select New With {.id = i.ID, .product = p.Name, .color = c.Name, .size = s.Name, .quantity = i.Quantity}

            If items Is Nothing And items.Count > 0 Then
                Throw New Exception("There are no Inventory items")
            Else
                For Each e As Object In items
                    itemList.Add(e)
                Next

                Return itemList
            End If
        End Function

        Public Overloads Function GetProductInventory(ByVal ProductID As Integer) As IQueryable(Of Inventory)
            'Dim itemList As New Generic.List(Of Inventory)

            Dim items = GetProductInventoryFunc(db, ProductID) '(From i In db.Inventories Where i.ProductID = ProductID).SingleOrDefault

            If items Is Nothing Then
                Throw New Exception("Inventory for product: " & ProductID.ToString & " does not exist.")
            Else
                'Return item
                'For Each e As Inventory In itemList
                'itemList.Add(e)
                'Next

                Return items 'itemList
            End If

            items = Nothing
        End Function

        Public Overloads Sub RemoveProductInventory(ByVal ProductId As Integer, ByVal ColorId As Integer, ByVal SizeId As Integer, ByVal Quantity As Integer)
            Dim CurrentInventoryItems As List(Of Inventory) = GetInventoryByProductColorSizeIDFunc(db, ProductId, ColorId, SizeID).ToList

            Dim TempQuantity As Integer = Quantity

            If CurrentInventoryItems.First Is Nothing Then
                Throw New Exception(String.Format("Error updating Inventory for product: {0}.", ProductId))
            Else
                If Quantity <= CurrentInventoryItems.First.Quantity Then
                    CurrentInventoryItems.First.Quantity = CurrentInventoryItems.First.Quantity - Quantity
                Else
                    Dim Index As Integer = 0

                    While TempQuantity > 0
                        If CurrentInventoryItems(Index).Quantity > TempQuantity Then
                            CurrentInventoryItems(Index).Quantity -= TempQuantity
                            TempQuantity = 0
                        Else
                            TempQuantity -= CurrentInventoryItems(Index).Quantity
                            CurrentInventoryItems(Index).Quantity = 0
                        End If

                        Index += 1
                    End While
                End If

                db.SubmitChanges()
            End If
        End Sub

        Public Shared GetProductInventoryFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Inventory)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, pID As Integer) _
                                      From i In db.Inventories Where i.ProductID = pID Select i)

        Public Overloads Function ProductHasInventory(ByVal ProductID As Integer, ByVal ColorID As Integer, ByVal SizeID As Integer) As Boolean
            Dim itemList As New List(Of Inventory)

            Dim items = GetInventoryByProductColorSizeIDFunc(db, ProductID, ColorID, SizeID) ' From i In db.Inventories Where i.ProductID = ProductID And i.ColorID = ColorID And i.SizeID = SizeID

            If items.Count < 1 Then
                Return False
            Else
                Return True
            End If

            items = Nothing
        End Function

        Public Shared GetProductInventoryQuantity As Func(Of CommerceDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, pId As Integer) _
                        (From i In db.Inventories Where i.ProductID = pId _
                         Group By i.ProductID Into Quantity = Sum(i.Quantity) Select New With {.Quantity = Quantity}).FirstOrDefault.Quantity)

        Public Shared GetInventoryAllQuantityCompiled As Func(Of CommerceDataContext, Integer, Integer, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, pId As Integer, cID As Integer, sID As Integer) _
                        (From i In db.Inventories Where i.ProductID = pId And i.ColorID = cID And i.SizeID = sID _
                         Group By i.ProductID, i.ColorID, i.SizeID Into Quantity = Sum(i.Quantity) Select New With {.Quantity = Quantity}).FirstOrDefault.Quantity)

        Public Shared GetInventoryColorQuantityCompiled As Func(Of CommerceDataContext, Integer, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, pId As Integer, cID As Integer) _
                        (From i In db.Inventories Where i.ProductID = pId And i.ColorID = cID _
                         Group By i.ProductID, i.ColorID Into Quantity = Sum(i.Quantity) Select New With {.Quantity = Quantity}).FirstOrDefault.Quantity)

        Public Shared GetInventorySizeQuantityCompiled As Func(Of CommerceDataContext, Integer, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, pId As Integer, sID As Integer) _
                        (From i In db.Inventories Where i.ProductID = pId And i.SizeID = sID _
                         Group By i.ProductID, i.SizeID Into Quantity = Sum(i.Quantity) Select New With {.Quantity = Quantity}).FirstOrDefault.Quantity)

        Public Shared GetInventoryQuantityCompiled As Func(Of CommerceDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, pId As Integer) _
                        (From i In db.Inventories Where i.ProductID = pId _
                         Group By i.ProductID Into Quantity = Sum(i.Quantity) Select New With {.Quantity = Quantity}).FirstOrDefault.Quantity)

        Public Function GetInventoryQuantity(ByVal ProductId As Integer, ByVal ColorId As Integer, ByVal SizeId As Integer) As Integer
            Dim CurrentInventory As Integer = 0

            Try
                CurrentInventory = GetInventoryAllQuantityCompiled(db, ProductId, ColorId, SizeId)
            Catch Ex As Exception
                CurrentInventory = 0
            End Try

            Return CurrentInventory
        End Function

        Public Function GetInventoryQuantity(ByVal ProductId As Integer) As Integer
            Dim CurrentInventory As Integer = 0

            Try
                CurrentInventory = GetInventoryQuantityCompiled(db, ProductId)
            Catch Ex As Exception
                CurrentInventory = 0
            End Try

            Return CurrentInventory
        End Function

        Public Function ConsolidateInventory() As Boolean
            'TODO: Have this function cycle through the inventory table combining inventory on duplicate rows (where productId, sizeID and colorID are the same).
            Return False
        End Function

        Public Overloads Function GetLowInventorySummary() As List(Of Inventory)
            Dim itemList As New List(Of Inventory)

            Dim items = GetInventoryFunc(db).OrderBy(Function(i) i.Quantity).Take(5)

            If items Is Nothing And items.Count > 0 Then
                Throw New Exception("There are no Inventory items")
            Else
                For Each e As Inventory In items
                    itemList.Add(e)
                Next

                Return itemList
            End If
        End Function

    End Class

End Namespace