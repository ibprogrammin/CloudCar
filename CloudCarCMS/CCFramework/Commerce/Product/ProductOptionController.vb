Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Commerce.Product

    Public Class ProductOptionController
        Inherits DataControllerClass

        Public Shared GetAllProductOptionsFunc As Func(Of CommerceDataContext, IQueryable(Of ProductOption)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                      From p In CurrentDataContext.ProductOptions Select p)

        Public Shared GetProductsOptionsFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of ProductOption)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProductId As Integer) _
                                      From p In CurrentDataContext.ProductOptions Where p.ProductId = ProductId Select p)

        Public Shared GetProductOptionFunc As Func(Of CommerceDataContext, Integer, ProductOption) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, Id As Integer) _
                                      (From p In CurrentDataContext.ProductOptions Where p.Id = Id Select p).FirstOrDefault)

        Public Overloads Function Create(ByVal Id As Integer, ProductId As Integer, ImageId As Integer, Title As String, Description As String, PriceA As Decimal, PriceB As Decimal, PriceC As Decimal) As Integer
            Dim CurrentProductOption As New ProductOption
            
            CurrentProductOption.ProductId = ProductId
            CurrentProductOption.Title = Title
            CurrentProductOption.Description = Description
            CurrentProductOption.PriceA = PriceA
            CurrentProductOption.PriceB = PriceB
            CurrentProductOption.PriceC = PriceC

            db.ProductOptions.InsertOnSubmit(CurrentProductOption)
            db.SubmitChanges()

            Return CurrentProductOption.Id
        End Function

        Public Overloads Function Delete(ByVal Id As Integer) As Boolean
            Try
                Dim CurrentProductOption As ProductOption = GetProductOptionFunc(db, Id)

                db.ProductOptions.DeleteOnSubmit(CurrentProductOption)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal Id As Integer, ProductId As Integer, ImageId As Integer, Title As String, Description As String, PriceA As Decimal, PriceB As Decimal, PriceC As Decimal) As Boolean
            Dim CurrentProductOption As ProductOption

            CurrentProductOption = GetProductOptionFunc(db, Id)

            If CurrentProductOption Is Nothing Then
                Throw New Exception(String.Format("Product option with ID: {0} does not exist.", Id))
            Else
                CurrentProductOption.ProductId = ProductId
                CurrentProductOption.Title = Title
                CurrentProductOption.Description = Description
                CurrentProductOption.PriceA = PriceA
                CurrentProductOption.PriceB = PriceB
                CurrentProductOption.PriceC = PriceC

                If ImageId = 0 And CurrentProductOption.ImageId > 0 Then

                ElseIf Not ImageId = 0 Then
                    CurrentProductOption.ImageId = ImageId
                Else
                    CurrentProductOption.ImageId = ImageId
                End If

                db.SubmitChanges()

            End If

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ProductOptionId As Integer) As ProductOption
            Dim CurrentProductOption As ProductOption

            CurrentProductOption = GetProductOptionFunc(db, ProductOptionId)

            If CurrentProductOption Is Nothing Then
                Throw New Exception(String.Format("Product Option with ID: {0} does not exist.", ProductOptionId))
            Else
                Return CurrentProductOption
            End If
        End Function

        Public Overloads Function GetElements() As List(Of ProductOption)
            Dim CurrentProductOptions As List(Of ProductOption) = GetAllProductOptionsFunc(db).ToList

            If CurrentProductOptions Is Nothing Then
                Throw New Exception("There are no Product Options")
            Else
                Return CurrentProductOptions
            End If
        End Function

        Public Overloads Function GetElements(ByVal ProductId As Integer) As List(Of ProductOption)
            Dim CurrentProductOptions As List(Of ProductOption)

            CurrentProductOptions = GetProductsOptionsFunc(db, ProductId).ToList

            If CurrentProductOptions Is Nothing Then
                Throw New Exception(String.Format("Product with ID: {0} has no options.", ProductId))
            Else
                Return CurrentProductOptions
            End If
        End Function

    End Class

End Namespace