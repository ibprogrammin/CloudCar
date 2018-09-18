Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Commerce

    Public Class BrandController
        Inherits DataControllerClass

        Public Shared GetBrandByIdFunc As Func(Of CommerceDataContext, Integer, Brand) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, BrandId As Integer) _
                                      (From i In CurrentDataContext.Brands _
                                       Where i.ID = BrandId _
                                       Select i).FirstOrDefault)

        Public Shared GetBrandsFunc As Func(Of CommerceDataContext, IQueryable(Of Brand)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                      From b In CurrentDataContext.Brands Select b)

        Public Overloads Function Create(ByVal Name As String, ByVal Description As String, ByVal Keywords As String, ByVal Url As String, ByVal LogoImageId As Integer, ByVal Permalink As String) As Integer
            Dim CurrentBrand As New Brand
            Dim CurrentBrandId As Integer

            CurrentBrand.Name = Name
            CurrentBrand.Description = Description
            CurrentBrand.Keywords = Keywords
            CurrentBrand.URL = URL
            CurrentBrand.LogoImageID = LogoImageID
            CurrentBrand.Permalink = Permalink

            db.Brands.InsertOnSubmit(CurrentBrand)
            db.SubmitChanges()

            CurrentBrandId = CurrentBrand.ID

            Return CurrentBrandId
        End Function

        Public Overloads Function Delete(ByVal BrandId As Integer) As Boolean
            Try
                Dim CurrentBrand As Brand = GetBrandByIdFunc(db, BrandId)

                db.Brands.DeleteOnSubmit(CurrentBrand)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal BrandId As Integer, ByVal Name As String, ByVal Description As String, ByVal Keywords As String, ByVal Url As String, ByVal LogoImageId As Integer, ByVal Permalink As String) As Boolean
            Dim CurrentBrand As Brand

            CurrentBrand = GetBrandByIdFunc(db, BrandId)

            If CurrentBrand Is Nothing Then
                Throw New Exception(String.Format("Brand with ID: {0} does not exist.", BrandId))

                Return False
            Else
                CurrentBrand.Name = Name
                CurrentBrand.Description = Description
                CurrentBrand.Keywords = Keywords
                CurrentBrand.URL = Url
                CurrentBrand.LogoImageID = LogoImageId
                CurrentBrand.Permalink = Permalink

                db.SubmitChanges()

                Return True
            End If
        End Function

        Public Overloads Function GetElement(ByVal BrandId As Integer) As Brand
            Dim CurrentBrand As Brand

            CurrentBrand = GetBrandByIdFunc(db, BrandId)

            If CurrentBrand Is Nothing Then
                Throw New Exception(String.Format("Brand with ID: {0} does not exist.", BrandId))
            Else
                Return CurrentBrand
            End If
        End Function

        Public Overloads Function GetElements() As List(Of Brand)
            Dim CurrentBrands As List(Of Brand)

            CurrentBrands = GetBrandsFunc(db).ToList

            If CurrentBrands Is Nothing OrElse CurrentBrands.Count < 1 Then
                Throw New Exception("There are no Brands")
            Else
                Return CurrentBrands
            End If
        End Function

        Public Shared GetBrandByPermalinkFunc As Func(Of CommerceDataContext, String, Brand) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, Permalink As String) _
                        (From b In CurrentDataContext.Brands _
                         Where b.Permalink.ToLower Like Permalink _
                         Select b).FirstOrDefault)

        Public Shared GetBrandCategoriesFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Category)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, BrandId As Integer) _
                                      From b In CurrentDataContext.Brands _
                                      Join p In CurrentDataContext.Products _
                                      On p.BrandID Equals b.ID _
                                      Join c In CurrentDataContext.Categories _
                                      On p.CategoryID Equals c.ID _
                                      Where b.ID = BrandId Select c Distinct)

        Public Overloads Function GetElement(ByVal Permalink As String) As Brand
            Dim CurrentBrand As Brand = GetBrandByPermalinkFunc(db, Permalink.ToLower)

            If CurrentBrand Is Nothing Then
                Throw New Exception("The brand you were looking for does not exist.")
            Else
                GetElement = CurrentBrand
            End If
        End Function

        Public Shared GetBrandsWithProductsFunc As Func(Of CommerceDataContext, IQueryable(Of Brand)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                    From b In CurrentDataContext.Brands _
                                    Join p In CurrentDataContext.Products _
                                    On p.BrandID Equals b.ID _
                                    Select b Distinct)

        Public Overloads Function GetBrandsWithProducts() As List(Of Brand)
            Dim CurrentBrands As List(Of Brand)

            CurrentBrands = GetBrandsWithProductsFunc(db).ToList

            If CurrentBrands Is Nothing And CurrentBrands.Count < 1 Then
                Throw New Exception("There are no Brands")
            Else
                Return CurrentBrands
            End If
        End Function

        Public Shared Function GetBrandCategories(ByVal BrandId As Integer) As List(Of Category)
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentCategories As List(Of Category)
            CurrentCategories = GetBrandCategoriesFunc(CurrentDataContext, BrandId).ToList

            If CurrentCategories Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception(String.Format("ID: {0} does not contain any elements.", BrandId))
            Else
                GetBrandCategories = CurrentCategories

                CurrentDataContext.Dispose()
            End If
        End Function

    End Class

End Namespace