Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.Product

Namespace CCFramework.Commerce

    Public Class ProductController
        Inherits DataControllerClass

        Public Shared GetAllProductsFunc As Func(Of CommerceDataContext, IQueryable(Of Model.Product)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) From p In db.Products Select p)

        Public Shared GetProductsAsTopSellersFunc As Func(Of CommerceDataContext, IQueryable(Of Model.Product)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) From p In db.Products Where p.TopSeller = True And p.Active = True Select p)

        Public Shared GetProductsAsClearanceFunc As Func(Of CommerceDataContext, IQueryable(Of Model.Product)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) From p In db.Products Where p.Clearance = True And p.Active = True Select p)

        Public Shared GetProductFromPermalinkFunc As Func(Of CommerceDataContext, String, Model.Product) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, permalink As String) (From p In db.Products Where p.Permalink Like permalink Select p).SingleOrDefault)

        Public Shared GetProductFromIdFunc As Func(Of CommerceDataContext, Integer, Model.Product) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) (From p In db.Products Where p.ID = id Select p).SingleOrDefault)

        Public Shared GetLatestProductsFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Model.Product)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, Count As Integer) _
                                      From p In CurrentDataContext.Products _
                                      Where p.Active = True _
                                      Select p _
                                      Order By p.ID Descending _
                                      Take Count)

        Public Shared GetActiveProductsFromCategoryIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Model.Product)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, CategoryId As Integer) _
                                      From p In CurrentDataContext.Products _
                                      Where p.CategoryID = CategoryId _
                                      And p.Active = True _
                                      Select p)

        Public Shared GetActiveProductsFromColorIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Model.Product)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ColorId As Integer) _
                                      (From p In CurrentDataContext.Products _
                                      Join c In CurrentDataContext.ProductColors _
                                      On c.ProductID Equals p.ID _
                                      Where c.ColorID = ColorId _
                                      And p.Active = True _
                                      Select p).Distinct)

        Public Shared GetActiveProductsFromCategoryIdCountFunc As Func(Of CommerceDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, CategoryId As Integer) _
                                      (From p In CurrentDataContext.Products _
                                       Where p.CategoryID = CategoryId _
                                       And p.Active = True _
                                       Select p).Count)

        Public Shared GetActiveProductsWithImagesFromCategoryIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Model.Product)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, CategoryId As Integer) _
                                      From p In db.Products Where p.CategoryID = CategoryId And p.Active = True And Not p.DefaultImageID = 0 Select p)

        Public Shared GetActiveProductsWithImagesFromCategoryIdCountFunc As Func(Of CommerceDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, CategoryId As Integer) _
                                      (From p In db.Products Where p.CategoryID = CategoryId And p.Active = True And Not p.DefaultImageID = 0 Select p).Count)

        Public Shared GetActiveProductsFromBrandIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Model.Product)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, BrandId As Integer) _
                                      From p In CurrentDataContext.Products _
                                      Where p.BrandID = BrandId _
                                      And p.Active = True _
                                      Select p)

        Public Shared GetActiveProductsFromBrandIdCountFunc As Func(Of CommerceDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, BrandID As Integer) (From p In db.Products Where p.BrandID = BrandID And p.Active = True Select p).Count)

        Public Shared GetProductColorFromIdFunc As Func(Of CommerceDataContext, Integer, ProductColor) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) (From p In db.ProductColors Where p.ID = id Select p).FirstOrDefault)

        Public Shared GetProductColorFromProductIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of ProductColor)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) From p In db.ProductColors Where p.ProductID = id Select p)

        Public Shared GetProductColorFromColorAndProductIdFunc As Func(Of CommerceDataContext, Integer, Integer, ProductColor) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, productId As Integer, colorId As Integer) _
                                      (From p In db.ProductColors Where p.ProductID = productId And p.ColorID = colorId Select p).FirstOrDefault)

        Public Shared GetProductSizesFromIdFunc As Func(Of CommerceDataContext, Integer, ProductSize) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) (From p In db.ProductSizes Where p.ID = id Select p).FirstOrDefault)

        Public Shared GetProductSizesFromProductIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of ProductSize)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) From p In db.ProductSizes Where p.ProductID = id Select p)

        Public Shared GetProductSizesFromSizeAndProductIdFunc As Func(Of CommerceDataContext, Integer, Integer, ProductSize) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, productId As Integer, sizeId As Integer) _
                                      (From p In db.ProductSizes Where p.ProductID = productId And p.SizeID = sizeId Select p).FirstOrDefault)

        Public Shared GetImageFromIdFunc As Func(Of CommerceDataContext, Integer, ProductImage) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) (From i In db.ProductImages Where i.ID = id Select i).FirstOrDefault)

        Public Shared GetProductImageFromProductIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of ProductImage)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) From p In db.ProductImages Where p.ProductID = id Select p)

        Public Shared GetImageFromProductAndImageIdFunc As Func(Of CommerceDataContext, Integer, Integer, ProductImage) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, productId As Integer, imageId As Integer) _
                                      (From i In db.ProductImages Where i.ProductID = productId And i.ImageID = imageId Select i).FirstOrDefault)

        Public Shared GetProductReviewsFromProductIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of ProductReview)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) From p In db.ProductReviews Where p.productId = id Select p)

        Public Shared GetShoppingCartFromProductIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Model.ShoppingCart)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) From p In db.ShoppingCarts Where p.ProductID = id Select p)

        Public Shared GetMenuCategoryItemsFromProductIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of MenuCategoryItem)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) From p In db.MenuCategoryItems Where p.productID = id Select p)

        Public Shared GetInventoyFromProductIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Inventory)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) From p In db.Inventories Where p.ProductID = id Select p)

        Public Shared GetBcRoomProductFromProductIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of BCRoomProduct)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) From p In db.BCRoomProducts Where p.ProductID = id Select p)

        Public Shared GetProductHasOrderItemsFunc As Func(Of CommerceDataContext, Integer, Boolean) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, ProductID As Integer) _
                                      If((From oi In db.OrderItems Where oi.ProductID = ProductID Select oi).Count > 0, True, False))


        Public Overloads Function Create(ByVal Sku As String, ByVal Name As String, ByVal Details As String, ByVal Description As String, ByVal Keywords As String, ByVal BrowserTitle As String, ByVal AlternateTag As String, PricingUnit As String, ByVal Cost As Decimal, ByVal ListPrice As Decimal, ByVal Price As Decimal, ByVal PriceB As Decimal, ByVal PriceC As Decimal, ByVal Discount As Decimal, ByVal Weight As Decimal, ByVal Length As Decimal, ByVal Width As Decimal, ByVal Height As Decimal, ByVal Active As Boolean, ByVal BrandID As Integer, ByVal CategoryID As Integer, ByVal DefaultImageID As Integer, ByVal HeaderImageID As Integer, ByVal TopSeller As Boolean, ByVal Clearance As Boolean, ByVal IsMembership As Boolean, ByVal Permalink As String, ByVal TrackInventory As Boolean, ByVal IsDigitalMedia As Boolean, ByVal Filename As String, ByVal TestimonialId As Integer) As Integer
            Dim CurrentProduct As New Model.Product
            Dim CurrentProductId As Integer

            CurrentProduct.SKU = Sku
            CurrentProduct.Name = Name
            CurrentProduct.Details = Details
            CurrentProduct.Description = Description
            CurrentProduct.Keywords = Keywords
            CurrentProduct.PricingUnit = PricingUnit
            CurrentProduct.Cost = Cost
            CurrentProduct.ListPrice = ListPrice
            CurrentProduct.Price = Price
            CurrentProduct.PriceB = PriceB
            CurrentProduct.PriceC = PriceC
            CurrentProduct.Discount = Discount
            CurrentProduct.Weight = Weight
            CurrentProduct.Length = Length
            CurrentProduct.Width = Width
            CurrentProduct.Height = Height
            CurrentProduct.Active = Active
            CurrentProduct.BrandID = BrandID
            CurrentProduct.CategoryID = CategoryID
            CurrentProduct.DefaultImageID = DefaultImageID
            CurrentProduct.HeaderImageID = HeaderImageID
            CurrentProduct.TopSeller = TopSeller
            CurrentProduct.Clearance = Clearance
            CurrentProduct.Membership = IsMembership
            CurrentProduct.Permalink = Permalink
            CurrentProduct.TrackInventory = TrackInventory
            CurrentProduct.IsDigitalMedia = IsDigitalMedia
            CurrentProduct.Filename = Filename
            CurrentProduct.BrowserTitle = BrowserTitle
            CurrentProduct.ImageAlt = AlternateTag
            CurrentProduct.TestimonialId = TestimonialId

            db.Products.InsertOnSubmit(CurrentProduct)
            db.SubmitChanges()

            CurrentProductId = CurrentProduct.ID

            Return CurrentProductId
        End Function

        Public Overloads Function Delete(ByVal ProductId As Integer) As Boolean
            Try
                If Not GetProductHasOrderItemsFunc(db, ProductId) Then
                    Dim CurrentProduct As Model.Product = GetProductFromIdFunc(db, ProductId)

                    DeleteShoppingCartByProduct(ProductId)
                    DeleteInventoryByProduct(ProductId)
                    DeleteReviewsByProduct(ProductId)
                    DeleteColorByProduct(ProductId)
                    DeleteSizesByProduct(ProductId)
                    DeleteImagesByProduct(ProductId)
                    DeleteRecomendationsByProduct(ProductId)

                    DeleteBCRoomProductByProduct(ProductId)
                    DeleteMenuCategoryItemsByProduct(ProductId)

                    db.Products.DeleteOnSubmit(CurrentProduct)
                    db.SubmitChanges()

                    Return True
                Else
                    Return False
                End If
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ProductId As Integer, ByVal Sku As String, ByVal Name As String, ByVal Details As String, ByVal Description As String, ByVal Keywords As String, ByVal BrowserTitle As String, ByVal AlternateTag As String, PricingUnit As String, ByVal Cost As Decimal, ByVal ListPrice As Decimal, ByVal Price As Decimal, ByVal PriceB As Decimal, ByVal PriceC As Decimal, ByVal Discount As Decimal, ByVal Weight As Decimal, ByVal Length As Decimal, ByVal Width As Decimal, ByVal Height As Decimal, ByVal Active As Boolean, ByVal BrandId As Integer, ByVal CategoryId As Integer, ByVal DefaultImageId As Integer, ByVal HeaderImageId As Integer, ByVal TopSeller As Boolean, ByVal Clearance As Boolean, ByVal IsMembership As Boolean, ByVal Permalink As String, ByVal TrackInventory As Boolean, ByVal IsDigitalMedia As Boolean, ByVal Filename As String, ByVal TestimonialId As Integer) As Boolean

            Dim CurrentProduct As Model.Product

            CurrentProduct = GetProductFromIdFunc(db, ProductId)

            If CurrentProduct Is Nothing Then
                Throw New Exception(String.Format("Product with ID: {0} does not exist.", ProductId))
            Else
                CurrentProduct.SKU = Sku
                CurrentProduct.Name = Name
                CurrentProduct.Details = Details
                CurrentProduct.Description = Description
                CurrentProduct.Keywords = Keywords
                CurrentProduct.PricingUnit = PricingUnit
                CurrentProduct.Cost = Cost
                CurrentProduct.ListPrice = ListPrice
                CurrentProduct.Price = Price
                CurrentProduct.PriceB = PriceB
                CurrentProduct.PriceC = PriceC
                CurrentProduct.Discount = Discount
                CurrentProduct.Weight = Weight
                CurrentProduct.Length = Length
                CurrentProduct.Width = Width
                CurrentProduct.Height = Height
                CurrentProduct.Active = Active
                CurrentProduct.BrandID = BrandId
                CurrentProduct.CategoryID = CategoryId
                CurrentProduct.TopSeller = TopSeller
                CurrentProduct.Clearance = Clearance
                CurrentProduct.Membership = IsMembership
                CurrentProduct.Permalink = Permalink
                CurrentProduct.TrackInventory = TrackInventory
                CurrentProduct.IsDigitalMedia = IsDigitalMedia
                CurrentProduct.Filename = Filename
                CurrentProduct.BrowserTitle = BrowserTitle
                CurrentProduct.ImageAlt = AlternateTag
                CurrentProduct.TestimonialId = TestimonialId

                If DefaultImageId = 0 And CurrentProduct.DefaultImageID > 0 Then

                ElseIf Not DefaultImageId = 0 Then
                    CurrentProduct.DefaultImageID = DefaultImageId
                Else
                    CurrentProduct.DefaultImageID = DefaultImageId
                End If

                If HeaderImageId = 0 And CurrentProduct.HeaderImageID > 0 Then

                ElseIf Not HeaderImageId = 0 Then
                    CurrentProduct.HeaderImageID = HeaderImageId
                Else
                    CurrentProduct.HeaderImageID = HeaderImageId
                End If

                db.SubmitChanges()

            End If

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ProductId As Integer) As Model.Product
            Dim CurrentProduct As Model.Product

            CurrentProduct = GetProductFromIdFunc(db, ProductId)

            If CurrentProduct Is Nothing Then
                Throw New Exception(String.Format("Product with ID: {0} does not exist.", ProductId))
            Else
                Return CurrentProduct
            End If
        End Function

        Public Overloads Function GetElement(ByVal Permalink As String) As Model.Product
            Dim CurrentProduct As Model.Product

            CurrentProduct = GetProductFromPermalinkFunc(db, Permalink)

            If CurrentProduct Is Nothing Then
                Throw New Exception(String.Format("Product with Permalink: {0} does not exist.", Permalink))
            Else
                Return CurrentProduct
            End If
        End Function

        Public Overloads Function GetElements() As List(Of Model.Product)
            Dim CurrentProducts As List(Of Model.Product) = GetAllProductsFunc(db).ToList

            If CurrentProducts Is Nothing Then
                Throw New Exception("There are no Products")
            Else
                Return CurrentProducts
            End If
        End Function

        Public Shared Function ProductHasOrderItems(ByVal ProductId As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            ProductHasOrderItems = GetProductHasOrderItemsFunc(CurrentDataContext, ProductId)

            CurrentDataContext.Dispose()
        End Function

        Public Sub AddColor(ByVal ProductId As Integer, ByVal ColorId As Integer, ByVal ImageId As Integer)
            Dim CurrentColor As New ProductColor

            CurrentColor.ColorID = ColorId
            CurrentColor.ProductID = ProductId
            CurrentColor.ImageID = ImageId

            db.ProductColors.InsertOnSubmit(CurrentColor)
            db.SubmitChanges()
        End Sub

        Public Function RemoveColor(ByVal ProductColorId As Integer) As Boolean
            Try
                Dim CurrentColor As ProductColor = GetProductColorFromIdFunc(db, ProductColorId)

                db.ProductColors.DeleteOnSubmit(CurrentColor)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Function RemoveColor(ByVal ProductId As Integer, ByVal ColorId As Integer) As Boolean
            Try
                Dim CurrentColor As ProductColor = GetProductColorFromColorAndProductIdFunc(db, ProductId, ColorId)

                db.ProductColors.DeleteOnSubmit(CurrentColor)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Sub AddSize(ByVal ProductId As Integer, ByVal SizeId As Integer)
            Dim CurrentSize As New ProductSize

            CurrentSize.SizeID = SizeId
            CurrentSize.ProductID = ProductId
            
            db.ProductSizes.InsertOnSubmit(CurrentSize)
            db.SubmitChanges()
        End Sub

        Public Function RemoveSize(ByVal ProductSizeId As Integer) As Boolean
            Try
                Dim CurrentSize As ProductSize = GetProductSizesFromIdFunc(db, ProductSizeId)

                db.ProductSizes.DeleteOnSubmit(CurrentSize)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Function RemoveSize(ByVal ProductId As Integer, ByVal SizeId As Integer) As Boolean
            Try
                Dim CurrentSize As ProductSize = GetProductSizesFromSizeAndProductIdFunc(db, ProductId, SizeId)

                db.ProductSizes.DeleteOnSubmit(CurrentSize)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Shared GetSizesFromProductIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Size)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, ProductId As Integer) _
                                      From ps In db.ProductSizes _
                                      Join s In db.Sizes _
                                      On s.ID Equals ps.SizeID _
                                      Where ps.ProductID = ProductId _
                                      Select s)

        Public Function GetSizes(ByVal ProductId As Integer) As List(Of Size)
            Dim CurrentSizes As List(Of Size) = GetSizesFromProductIdFunc(db, ProductId).ToList

            If CurrentSizes Is Nothing OrElse CurrentSizes.Count < 1 Then
                'Throw New Exception("There are no Product Sizes")
                Return Nothing
            Else
                Return CurrentSizes
            End If
        End Function

        Public Sub AddImage(ByVal ProductId As Integer, ByVal ImageId As Integer)
            Dim CurrentImage As New ProductImage

            CurrentImage.ImageID = ImageId
            CurrentImage.ProductID = ProductId

            db.ProductImages.InsertOnSubmit(CurrentImage)
            db.SubmitChanges()
        End Sub

        Public Function RemoveImage(ByVal ProductImageId As Integer) As Boolean
            Try
                Dim CurrentImage As ProductImage = GetImageFromIdFunc(db, ProductImageId)

                db.ProductImages.DeleteOnSubmit(CurrentImage)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Function RemoveImage(ByVal ProductId As Integer, ByVal ImageId As Integer) As Boolean
            Try
                Dim CurrentImage As ProductImage = GetImageFromProductAndImageIdFunc(db, ProductId, ImageId)

                db.ProductImages.DeleteOnSubmit(CurrentImage)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function


        Public Function DeleteShoppingCartByProduct(ByVal ProductId As Integer) As Boolean
            Try
                Dim CurrentShoppingCartItems As List(Of Model.ShoppingCart) = GetShoppingCartFromProductIdFunc(db, ProductId).ToList

                db.ShoppingCarts.DeleteAllOnSubmit(CurrentShoppingCartItems)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Function DeleteInventoryByProduct(ByVal ProductId As Integer) As Boolean
            Try
                Dim CurrentInventoryItems As List(Of Inventory) = GetInventoyFromProductIdFunc(db, ProductId).ToList

                db.Inventories.DeleteAllOnSubmit(CurrentInventoryItems)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Function DeleteColorByProduct(ByVal ProductId As Integer) As Boolean
            Try
                Dim CurrentProductColors As List(Of ProductColor) = GetProductColorFromProductIdFunc(db, ProductId).ToList

                db.ProductColors.DeleteAllOnSubmit(CurrentProductColors)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Function DeleteSizesByProduct(ByVal ProductId As Integer) As Boolean
            Try
                Dim CurrentProductSizes As List(Of ProductSize) = GetProductSizesFromProductIdFunc(db, ProductId).ToList

                db.ProductSizes.DeleteAllOnSubmit(CurrentProductSizes)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Function DeleteImagesByProduct(ByVal ProductId As Integer) As Boolean
            Try
                Dim CurrentProductImages As List(Of ProductImage) = GetProductImageFromProductIdFunc(db, ProductId).ToList

                db.ProductImages.DeleteAllOnSubmit(CurrentProductImages)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Function DeleteReviewsByProduct(ByVal ProductId As Integer) As Boolean
            Try
                Dim CurrentProductReviews As List(Of ProductReview) = GetProductReviewsFromProductIdFunc(db, ProductId).ToList

                db.ProductReviews.DeleteAllOnSubmit(CurrentProductReviews)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Function DeleteMenuCategoryItemsByProduct(ByVal ProductId As Integer) As Boolean
            Try
                Dim CurrentMenuCategoryItems As List(Of MenuCategoryItem) = GetMenuCategoryItemsFromProductIdFunc(db, ProductId).ToList

                db.MenuCategoryItems.DeleteAllOnSubmit(CurrentMenuCategoryItems)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Function DeleteRecomendationsByProduct(ByVal ProductId As Integer) As Boolean
            Try
                Dim CurrentProductRecomendations As List(Of ProductRecomendation) = ProductRecomendationController.GetAllProductsRecomendationsFunc(db, ProductId).ToList

                db.ProductRecomendations.DeleteAllOnSubmit(CurrentProductRecomendations)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Function DeleteBcRoomProductByProduct(ByVal ProductId As Integer) As Boolean
            Try
                Dim CurrentBcRoomProducts As List(Of BCRoomProduct) = GetBcRoomProductFromProductIdFunc(db, ProductId).ToList

                db.BCRoomProducts.DeleteAllOnSubmit(CurrentBcRoomProducts)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function


        Public Shared GetColorsFromProductIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Color)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, productId As Integer) _
                                      From pc In db.ProductColors Join c In db.Colors On c.ID Equals pc.ColorID Where pc.ProductID = productId Select c)

        Public Function GetColors(ByVal ProductId As Integer) As List(Of Color)
            Dim CurrentColors As List(Of Color) = GetColorsFromProductIdFunc(db, ProductId).ToList

            If CurrentColors Is Nothing OrElse CurrentColors.Count < 1 Then
                'Throw New Exception("There are no Colours")
                Return Nothing
            Else
                Return CurrentColors
            End If
        End Function

        Public Function GetProductColorImage(ByVal ProductId As Integer, ByVal ColorId As Integer) As Integer
            Dim CurrentProductColor As ProductColor = GetProductColorFromColorAndProductIdFunc(db, ProductId, ColorId)

            If CurrentProductColor Is Nothing Then
                Throw New Exception("There is no matching product color.")
            Else
                If CurrentProductColor.ImageID.HasValue Then
                    Return CurrentProductColor.ImageID.Value
                Else
                    Return 0
                End If
            End If
        End Function

        Public Sub MakeActive(ByVal ProductId As Integer, ByVal Active As Boolean)
            Dim CurrentProduct As Model.Product

            CurrentProduct = GetProductFromIdFunc(db, ProductId)

            If CurrentProduct Is Nothing Then
                Throw New Exception(String.Format("Product with ID: {0} does not exist.", ProductId))
            Else
                CurrentProduct.Active = Active

                db.SubmitChanges()
            End If
        End Sub


        Public Shared Function GetLatestProducts(ByVal Count As Integer) As List(Of Model.Product)
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentProducts As List(Of Model.Product)
            CurrentProducts = GetLatestProductsFunc(CurrentDataContext, Count).ToList

            If CurrentProducts Is Nothing OrElse CurrentProducts.Count < 1 Then
                CurrentDataContext.Dispose()

                Throw New Exception("There are no products in this category")
            Else
                GetLatestProducts = CurrentProducts

                'CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetActiveProductsByCategory(ByVal CategoryId As Integer) As List(Of Model.Product)
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentProducts As List(Of Model.Product)
            CurrentProducts = GetActiveProductsFromCategoryIdFunc(CurrentDataContext, CategoryId).ToList

            If CurrentProducts Is Nothing OrElse CurrentProducts.Count < 1 Then
                CurrentDataContext.Dispose()

                Throw New Exception("There are no products in this category")
            Else
                GetActiveProductsByCategory = CurrentProducts

                'CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetActiveProductsByBrand(ByVal BrandId As Integer) As List(Of Model.Product)
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentProducts As List(Of Model.Product)
            CurrentProducts = GetActiveProductsFromBrandIdFunc(CurrentDataContext, BrandId).ToList

            If CurrentProducts Is Nothing OrElse CurrentProducts.Count < 1 Then
                CurrentDataContext.Dispose()

                Throw New Exception("There are no products in this category")
            Else
                GetActiveProductsByBrand = CurrentProducts

                'CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetActiveProductsByColor(ByVal ColorId As Integer) As List(Of Model.Product)
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentProducts As List(Of Model.Product)
            CurrentProducts = GetActiveProductsFromColorIdFunc(CurrentDataContext, ColorId).ToList

            If CurrentProducts Is Nothing OrElse CurrentProducts.Count < 1 Then
                CurrentDataContext.Dispose()

                Throw New Exception("There are no products in with this color")
            Else
                GetActiveProductsByColor = CurrentProducts

                'CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared GetActiveClearanceItems As Func(Of CommerceDataContext, IQueryable(Of Model.Product)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                From p In CurrentDataContext.Products _
                                Where p.Active And p.Clearance Select p)

        Public Shared GetAllActiveTopSellerItems As Func(Of CommerceDataContext, IQueryable(Of Model.Product)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                (From p In CurrentDataContext.Products _
                                 Join oi In CurrentDataContext.OrderItems _
                                 On oi.ProductID Equals p.ID _
                                 Where p.Active = True _
                                 Group By oi.Product _
                                 Into Quantity = Sum(oi.Quantity) _
                                 Order By Quantity Descending _
                                 Select Product))

        Public Shared GetActiveTopSellerItems As Func(Of CommerceDataContext, Integer, IQueryable(Of Model.Product)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, Count As Integer) _
                                (From p In CurrentDataContext.Products _
                                 Join oi In CurrentDataContext.OrderItems _
                                 On oi.ProductID Equals p.ID _
                                 Where p.Active = True _
                                 Group By oi.Product _
                                 Into Quantity = Sum(oi.Quantity) _
                                 Order By Quantity Descending _
                                 Select Product _
                                 Take Count))

        Public Shared GetTopSellerItems As Func(Of CommerceDataContext, Integer, IQueryable(Of InventorySummary)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, Count As Integer) _
                                (From oi In db.OrderItems _
                                 Where oi.Order.ApprovalState = 1 _
                                Join p In db.Products On oi.ProductID Equals p.ID _
                                Join c In db.Categories On p.CategoryID Equals c.ID _
                                Group By oi.ProductID, p.Name, p.Permalink, CategoryPermalink = c.Permalink, _
                                Inventory = If(p.TrackInventory, _
                                               (From i In db.Inventories _
                                                Where i.ProductID = p.ID _
                                                Group By pID = i.ProductID Into Quantity = Sum(i.Quantity) _
                                Select New With {.Quantity = CType(Quantity, Nullable(Of Integer))}).First.Quantity, 100) _
                                Into Quantity = Sum(oi.Quantity) _
                                Order By Quantity Descending _
                                Select New InventorySummary(ProductID, Name, Permalink, CategoryPermalink, Quantity, Inventory) _
                                Take Count))

        Public Function GetTopSellers(ByVal Count As Integer) As List(Of InventorySummary)
            Dim CurrentInventorySummaryItems As List(Of InventorySummary) = GetTopSellerItems(db, Count).ToList

            If CurrentInventorySummaryItems Is Nothing OrElse CurrentInventorySummaryItems.Count < 1 Then
                Throw New Exception("There are no Top Selling Products")
            Else
                Return CurrentInventorySummaryItems
            End If
        End Function

        Public Shared GetLowInventoryItems As Func(Of CommerceDataContext, Integer, IQueryable(Of InventorySummary)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, Count As Integer) _
                                (From p In db.Products _
                                Join c In db.Categories On p.CategoryID Equals c.ID _
                                Where p.TrackInventory = True _
                                Group By ProductId = p.ID, Name = p.Name, Permalink = p.Permalink, CategoryPermalink = c.Permalink, _
                                Inventory = (From i In db.Inventories Where i.ProductID = p.ID Group By pID = i.ProductID Into Inventory = Sum(i.Quantity) _
                                Select New With {.Inventory = CType(Inventory, Nullable(Of Integer))}).First.Inventory, _
                                Quantity = (From oi In db.OrderItems Where oi.ProductID = p.ID And oi.Order.ApprovalState = 1 Group By pID = oi.ProductID Into Quantity = Sum(oi.Quantity) _
                                Select New With {.Quantity = CType(Quantity, Nullable(Of Integer))}).First.Quantity _
                                Into Group _
                                Order By Inventory _
                                Select New InventorySummary(ProductId, Name, Permalink, CategoryPermalink, Quantity, Inventory) _
                                Take Count))

        Public Function GetLowInventory(ByVal Count As Integer) As List(Of InventorySummary)
            Dim CurrentInventorySummaryItems As List(Of InventorySummary) = GetLowInventoryItems(db, Count).ToList

            If CurrentInventorySummaryItems Is Nothing OrElse CurrentInventorySummaryItems.Count = 0 Then
                Throw New Exception("There are no low inventory items")
            Else
                Return CurrentInventorySummaryItems
            End If
        End Function

        Public Shared GetProductInventoryQuantity As Func(Of CommerceDataContext, Integer, Integer) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, SelectedProductId As Integer) _
                                        (From i In CurrentDataContext.Inventories _
                                        Where i.ProductID = SelectedProductId _
                                        Group By i.ProductID _
                                            Into Quantity = Sum(i.Quantity) _
                                            Select New With _
                                                {.ID = ProductID, .Quantity = Quantity} _
                                                ).FirstOrDefault.Quantity)

        Public Function GetProductInventory(ByVal SelectedProductId As Integer) As Integer
            GetProductInventory = GetProductInventoryQuantity(db, SelectedProductId)
        End Function

        Public Function Price(ByVal ProductId As Integer) As Decimal
            Dim CurrentTotal As Decimal = 0
            Dim CurrentProduct = GetElement(ProductId)

            If Not CurrentProduct Is Nothing Then
                If Roles.GetRolesForUser().Contains("Sales") Then
                    CurrentTotal = CurrentProduct.PriceB
                ElseIf Roles.GetRolesForUser().Contains("Warehouse") Then
                    CurrentTotal = CurrentProduct.PriceC
                Else
                    CurrentTotal = CurrentProduct.Price
                End If
            End If

            Return CurrentTotal
        End Function

        Public Function Price(ByVal SelectedProduct As Model.Product) As Decimal
            Dim CurrentPrice As Decimal = 0
            Dim CurrentUserName As String = Membership.GetUser().UserName
            Dim CurrentPriceLevel As EPriceLevel = RegisteredUserController.GetUserPriceLevel(CurrentUserName)

            If Not SelectedProduct Is Nothing Then
                If CurrentPriceLevel = EPriceLevel.Sales Then
                    CurrentPrice = SelectedProduct.PriceB
                ElseIf CurrentPriceLevel = EPriceLevel.Warehouse Then
                    CurrentPrice = SelectedProduct.PriceC
                Else
                    CurrentPrice = SelectedProduct.Price
                End If
            End If

            Return CurrentPrice
        End Function

        Public Shared Function GetPrice(ByVal PriceLevel As EPriceLevel, ByVal SelectedProduct As Model.Product) As Decimal
            Dim CurrentPrice As Decimal

            Select Case PriceLevel
                Case EPriceLevel.Regular
                    CurrentPrice = SelectedProduct.Price
                Case EPriceLevel.Warehouse
                    CurrentPrice = SelectedProduct.PriceB
                Case EPriceLevel.Sales
                    CurrentPrice = SelectedProduct.PriceC
                Case Else
                    CurrentPrice = SelectedProduct.Price
            End Select

            Return CurrentPrice
        End Function

        Public Shared Function GetPrice(ByVal PriceLevel As EPriceLevel, ByVal ProductId As Integer) As Decimal
            Dim CurrentPrice As Decimal

            Dim CurrentProductController As New ProductController
            Dim CurrentProduct As Model.Product = CurrentProductController.GetElement(ProductId)
            CurrentProductController.Dispose()

            Select Case PriceLevel
                Case EPriceLevel.Regular
                    CurrentPrice = CurrentProduct.Price
                Case EPriceLevel.Warehouse
                    CurrentPrice = CurrentProduct.PriceB
                Case EPriceLevel.Sales
                    CurrentPrice = CurrentProduct.PriceC
                Case Else
                    CurrentPrice = CurrentProduct.Price
            End Select

            Return CurrentPrice
        End Function

        Public Shared Function GetProductName(ByVal ProductId As Integer) As String
            Dim CurrentDataContext As New CommerceDataContext
            Dim CurrentProductName As String = GetProductFromIdFunc(CurrentDataContext, ProductId).Name
            CurrentDataContext.Dispose()

            Return CurrentProductName
        End Function

        Public Shared GetProductNameByProductIdFunc As Func(Of CommerceDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProductId As Integer) _
                                      (From p In CurrentDataContext.Products _
                                       Where p.ID = ProductId Select p.Name).FirstOrDefault)

        Public Shared Function GetBreadCrumb(ByVal ProductId As Integer) As String
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentProduct As Model.Product = GetProductFromIdFunc(CurrentDataContext, ProductId)

            Dim BreadCrumbStringBuilder As New StringBuilder
            BreadCrumbStringBuilder.Append("You are in: ") '<a href=""/"">Home</a> &raquo; 
            BreadCrumbStringBuilder.AppendFormat("<a href=""/Shop/Index.html"">Shop Online</a> {0} ", Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("<a href=""/Shop/{0}.html"">{1}</a> {2} ", CurrentProduct.Category.Permalink, CurrentProduct.Category.Name, Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("{0}", CurrentProduct.Name)

            GetBreadCrumb = BreadCrumbStringBuilder.ToString

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetClearanceBreadCrumb(ByVal Label As String) As String
            Dim BreadCrumbStringBuilder As New StringBuilder
            BreadCrumbStringBuilder.AppendFormat("<a href=""/"">Home</a> {0} ", Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("<a href=""/Shop/Index.html"">Shop</a> {0} ", Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("{0}", Label)

            GetClearanceBreadCrumb = BreadCrumbStringBuilder.ToString
        End Function

        Public Shared Function GetTopSellerBreadCrumb(ByVal Label As String) As String
            Dim BreadCrumbStringBuilder As New StringBuilder
            BreadCrumbStringBuilder.AppendFormat("<a href=""/"">Home</a> {0} ", Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("<a href=""/Shop/Index.html"">Shop</a> {0} ", Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("{0}", Label)

            GetTopSellerBreadCrumb = BreadCrumbStringBuilder.ToString
        End Function

        Public Shared GetProductTracksInventoryFromIdFunc As Func(Of CommerceDataContext, Integer, Boolean) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProductId As Integer) _
                                      (From p In CurrentDataContext.Products _
                                       Where p.ID = ProductId _
                                       Select p.TrackInventory).FirstOrDefault)

        Public Shared Function DoesProductTrackInventory(ByVal ProductId As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            DoesProductTrackInventory = GetProductTracksInventoryFromIdFunc(CurrentDataContext, ProductId)

            CurrentDataContext.Dispose()
        End Function

    End Class


    'TODO compile queries for controller classes at bottom

    Public Class ProductColourController
        Inherits DataControllerClass

        Public Overloads Function Create(ByVal Name As String) As Integer
            Dim item As New Color
            Dim itemId As Integer

            item.Name = Name

            db.Colors.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = (From i In db.Colors Where i.ID = ID Select i).First

                db.Colors.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal Name As String) As Boolean
            Dim item As Color

            item = (From i In db.Colors Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("Color with ID: " & ID.ToString & " does not exist.")
            Else
                item.Name = Name

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As Color
            Dim item As Color

            item = (From i In db.Colors Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("Color with ID: " & ID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of Color)
            Dim itemList As New List(Of Color)

            Dim items = From c In db.Colors Select c

            If items Is Nothing And items.Count > 0 Then
                Throw New Exception("There are no Colors")
            Else
                For Each e As Color In items
                    itemList.Add(e)
                Next

                Return itemList
            End If
        End Function

        Public Shared GetProductColorsFromProductIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of ProductColor)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, ProductId As Integer) _
                                      From pc In db.ProductColors Where pc.ProductID = ProductId Select pc)

        Public Function GetColors(ByVal ProductID As Integer) As List(Of ProductColor)
            'Dim itemList As New Generic.List(Of Color)

            Dim items = GetProductColorsFromProductIDFunc(db, ProductID)
            'From pc In db.ProductColors Join c In db.Colors On c.ID Equals pc.ColorID Where pc.ProductID = ProductID Select c()()

            If items Is Nothing Then ' And items.Count > 0 Then
                Throw New Exception("There are no Product Colours")
            Else
                'For Each e As Color In items
                '    Dim color As Color = e

                '    itemList.Add(color)
                'Next

                Return items.ToList ' itemList
            End If
        End Function

        Public Sub AddColor(ByVal ProductID As Integer, ByVal ColorID As Integer, ByVal ImageID As Integer)
            Dim color As New ProductColor

            color.ColorID = ColorID
            color.ProductID = ProductID
            color.ImageID = ImageID

            db.ProductColors.InsertOnSubmit(color)
            db.SubmitChanges()

            color = Nothing
        End Sub

    End Class

    Public Class ProductSizeController
        Inherits DataControllerClass

        Public Overloads Function Create(ByVal Name As String) As Integer
            Dim item As New Size
            Dim itemId As Integer

            item.Name = Name

            db.Sizes.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = (From i In db.Sizes Where i.ID = ID Select i).First

                db.Sizes.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal Name As String) As Boolean
            Dim item As Size

            item = (From i In db.Sizes Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("Size with ID: " & ID.ToString & " does not exist.")
            Else
                item.Name = Name

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As Size
            Dim item As Size

            item = (From i In db.Sizes Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("Size with ID: " & ID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of Size)
            Dim itemList As New List(Of Size)

            Dim items = From c In db.Sizes Select c

            If items Is Nothing And items.Count > 0 Then
                Throw New Exception("There are no Sizes")
            Else
                For Each e As Size In items
                    itemList.Add(e)
                Next

                Return itemList
            End If
        End Function

    End Class

    Public Class ProductReviewController
        Inherits DataControllerClass

        Public Overloads Function Create(ByVal Name As String, ByVal Email As String, ByVal AvatarUrl As String, ByVal Comment As String, ByVal Rating As Integer, ByVal ProductID As Integer) As Integer
            Dim item As New ProductReview
            Dim itemId As Integer

            item.name = Name
            item.email = Email
            item.avatarurl = AvatarUrl
            item.comment = Comment
            item.rating = Rating
            item.productId = ProductID
            item.timestamp = Date.Now

            db.ProductReviews.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.id

            item = Nothing

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = (From i In db.ProductReviews Where i.id = ID Select i).First

                db.ProductReviews.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal Name As String, ByVal Email As String, ByVal AvatarUrl As String, ByVal Comment As String, ByVal Rating As Integer, ByVal ProductID As Integer) As Boolean
            Dim item As ProductReview

            item = (From i In db.ProductReviews Where i.id = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("Product Review with ID: " & ID.ToString & " does not exist.")
            Else
                item.name = Name
                item.email = Email
                item.avatarurl = AvatarUrl
                item.comment = Comment
                item.rating = Rating
                item.productId = ProductID
                item.timestamp = Date.Now

                db.SubmitChanges()

            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As ProductReview
            Dim item As ProductReview

            item = (From i In db.ProductReviews Where i.id = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("Product Review with ID: " & ID & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of ProductReview)
            Dim itemList As New List(Of ProductReview)

            Dim items = From pr In db.ProductReviews Select pr

            If items Is Nothing And items.Count > 0 Then
                Throw New Exception("There are no Product Reviews")
            Else
                For Each e As ProductReview In items
                    itemList.Add(e)
                Next

                Return itemList
            End If
        End Function

        Public Overloads Function GetElements(ByVal ProductID As Integer) As List(Of ProductReview)
            Dim itemList As New List(Of ProductReview)

            Dim items = From pr In db.ProductReviews Where pr.productId = ProductID Select pr

            If items Is Nothing And items.Count > 0 Then
                Throw New Exception("There are no Product Reviews for this Product")
            Else
                For Each e As ProductReview In items
                    itemList.Add(e)
                Next

                Return itemList
            End If
        End Function

        Public Shared Function GetProductRating(ByVal ProductId As Integer) As Integer
            Dim db As New CommerceDataContext

            Dim ratings = From r In db.ProductReviews Where r.productId = ProductID Select r

            Dim sumRatings As Integer = 0
            For Each item In ratings
                sumRatings += item.rating
            Next

            If Not ratings.Count = 0 Then
                Dim averageRating As Integer = CInt(Math.Ceiling(sumRatings / ratings.Count))

                Return averageRating
            Else
                Return 0
            End If

        End Function

    End Class

    Public Class InventorySummary
        Private _productID As Integer
        Private _name As String
        Private _permalink As String
        Private _categoryPermalink As String
        Private _quantity As Integer = 0
        Private _inventory As Integer = 0

        Public Sub New(ByVal productID As Integer, ByVal name As String, ByVal permalink As String, ByVal categoryPermalink As String, ByVal quantity As Nullable(Of Integer), ByVal inventory As Nullable(Of Integer))
            _productID = productID
            _name = name
            _permalink = permalink
            _categoryPermalink = categoryPermalink

            'If quantity = Nothing Then
            '_quantity = 0
            'Else
            _quantity = quantity.GetValueOrDefault
            'End If

            'If inventory = Nothing Then
            '_inventory = 0
            'Else
            _inventory = inventory.GetValueOrDefault
            'End If
        End Sub

        Public Property ProductID() As Integer
            Get
                Return _productID
            End Get
            Set(ByVal value As Integer)
                _productID = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Permalink() As String
            Get
                Return _permalink
            End Get
            Set(ByVal value As String)
                _permalink = value
            End Set
        End Property

        Public Property CategoryPermalink() As String
            Get
                Return _categoryPermalink
            End Get
            Set(ByVal value As String)
                _categoryPermalink = value
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

        Public Property Inventory() As Integer
            Get
                Return _inventory
            End Get
            Set(ByVal Value As Integer)
                _inventory = Value
            End Set
        End Property

        Public ReadOnly Property InventoryLevelDescription() As String
            Get
                Select Case Inventory
                    Case Is < 1
                        Return "Sold Out"
                    Case 1, 2
                        Return "Very Low"
                    Case 3, 4
                        Return "Low"
                    Case Else
                        Return "Full"
                End Select
            End Get
        End Property

        Public ReadOnly Property InventoryLevelColor() As String
            Get
                Select Case Inventory
                    Case Is < 1
                        Return "#FF0000"
                    Case 1, 2
                        Return "#ff3300"
                    Case 3, 4
                        Return "#ffdd22"
                    Case Else
                        Return "#00cc00"
                End Select
            End Get
        End Property

    End Class

End Namespace