Imports System.Data.Linq
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCFramework.Commerce.Product

    Public Class ProductRecomendationController
        Inherits DataControllerClass

        Public Shared GetAllProductRecomendationsFunc As Func(Of CommerceDataContext, IQueryable(Of ProductRecomendation)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                      From p In CurrentDataContext.ProductRecomendations Select p)

        Public Shared GetAllProductsRecomendationsFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of ProductRecomendation)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProductId As Integer) _
                                      From p In CurrentDataContext.ProductRecomendations _
                                      Where p.ProductId = ProductId Select p)

        Public Shared GetAllProductsRecomendationsByCountFunc As Func(Of CommerceDataContext, Integer, Integer, IQueryable(Of Model.Product)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProductId As Integer, Count As Integer) _
                                      From pr In CurrentDataContext.ProductRecomendations _
                                      Join p In CurrentDataContext.Products _
                                      On p.ID Equals pr.RecomendedProductId _
                                      Where pr.ProductId = ProductId _
                                      AndAlso Not pr.RecomendedProductId = ProductId _
                                      Select p Distinct Take Count)

        Public Shared GetProductRecomendationFunc As Func(Of CommerceDataContext, Integer, Integer, ProductRecomendation) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProductId As Integer, RecomendedProductId As Integer) _
                                      (From p In CurrentDataContext.ProductRecomendations _
                                       Where p.ProductId = ProductId And p.RecomendedProductId = RecomendedProductId _
                                       Select p).FirstOrDefault)

        Public Overloads Function Create(ProductId As Integer, RecomendedProductId As Integer) As Integer
            Dim CurrentProductRecomendation As New ProductRecomendation

            CurrentProductRecomendation.ProductId = ProductId
            CurrentProductRecomendation.RecomendedProductId = RecomendedProductId

            db.ProductRecomendations.InsertOnSubmit(CurrentProductRecomendation)
            db.SubmitChanges()

            Return CurrentProductRecomendation.ProductId + CurrentProductRecomendation.RecomendedProductId
        End Function

        Public Overloads Function Delete(ByVal ProductId As Integer, RecomendedProductId As Integer) As Boolean
            Try
                Dim CurrentProductRecomendation As ProductRecomendation = GetProductRecomendationFunc(db, ProductId, RecomendedProductId)

                db.ProductRecomendations.DeleteOnSubmit(CurrentProductRecomendation)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ProductId As Integer, RecomendedProductId As Integer, NewProductRecomendationId As Integer) As Boolean
            Dim CurrentProductRecomendation As ProductRecomendation

            CurrentProductRecomendation = GetProductRecomendationFunc(db, ProductId, RecomendedProductId)

            If CurrentProductRecomendation Is Nothing Then
                Throw New Exception(String.Format("Product recommendation with ID: {0} does not exist.", ProductId + RecomendedProductId))
            Else
                CurrentProductRecomendation.ProductId = ProductId
                CurrentProductRecomendation.RecomendedProductId = NewProductRecomendationId

                db.SubmitChanges()
            End If

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ProductId As Integer, RecomendedProductId As Integer) As ProductRecomendation
            Dim CurrentProductRecomendation As ProductRecomendation

            CurrentProductRecomendation = GetProductRecomendationFunc(db, ProductId, RecomendedProductId)

            If CurrentProductRecomendation Is Nothing Then
                Throw New Exception(String.Format("Product recommendation with ID: {0} does not exist.", ProductId + RecomendedProductId))
            Else
                Return CurrentProductRecomendation
            End If
        End Function

        Public Overloads Function GetElements() As List(Of ProductRecomendation)
            Dim CurrentProductRecomendations As List(Of ProductRecomendation) = GetAllProductRecomendationsFunc(db).ToList

            If CurrentProductRecomendations Is Nothing Then
                Throw New Exception("There are no Product Recomendations")
            Else
                Return CurrentProductRecomendations
            End If
        End Function

        Public Overloads Function GetElements(ByVal ProductId As Integer) As List(Of ProductRecomendation)
            Dim CurrentProductRecomendations As List(Of ProductRecomendation)

            CurrentProductRecomendations = GetAllProductsRecomendationsFunc(db, ProductId).ToList

            If CurrentProductRecomendations Is Nothing Then
                Throw New Exception(String.Format("Product with ID: {0} has no recommendations.", ProductId))
            Else
                Return CurrentProductRecomendations
            End If
        End Function

        Public Overloads Function GetElements(ByVal ProductId As Integer, Count As Integer) As List(Of Model.Product)
            Dim CurrentProductRecomendations As List(Of Model.Product)

            CurrentProductRecomendations = GetAllProductsRecomendationsByCountFunc(db, ProductId, Count).ToList

            If CurrentProductRecomendations Is Nothing Then
                Throw New Exception(String.Format("Product with ID: {0} has no recommendations.", ProductId))
            Else
                Return CurrentProductRecomendations
            End If
        End Function

    End Class

End Namespace