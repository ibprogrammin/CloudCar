Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Commerce

    Public Class CategoryController
        Inherits DataControllerClass

        Public Shared GetCategoryByIdFunc As Func(Of CommerceDataContext, Integer, Category) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, CategoryId As Integer) _
                                      (From c In db.Categories Where c.ID = CategoryId Select c).FirstOrDefault)

        Public Shared GetCategoryFunc As Func(Of CommerceDataContext, IQueryable(Of Category)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      From c In db.Categories Select c)

        Public Shared GetCategoryByPermalinkFunc As Func(Of CommerceDataContext, String, Category) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, permalink As String) _
                                      (From c In db.Categories Where c.Permalink Like permalink Select c).FirstOrDefault)

        Public Overloads Function Create(ByVal Name As String, ByVal Details As String, ByVal BrowserTitle As String, ByVal Description As String, ByVal Keywords As String, ByVal Permalink As String) As Integer
            Dim item As New Category
            Dim itemId As Integer

            item.Name = Name
            item.Description = Description
            item.Keywords = Keywords
            item.Permalink = Permalink
            item.Details = Details
            item.BrowserTitle = BrowserTitle

            db.Categories.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = GetCategoryByIDFunc(db, ID) ' (From i In db.Categories Where i.ID = ID Select i).First

                db.Categories.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal Name As String, ByVal Details As String, ByVal BrowserTitle As String, ByVal Description As String, ByVal Keywords As String, ByVal Permalink As String) As Boolean
            Dim item As Category

            item = GetCategoryByIdFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("Category with ID: " & ID.ToString & " does not exist.")
            Else
                item.Name = Name
                item.Description = Description
                item.Keywords = Keywords
                item.Permalink = Permalink
                item.Details = Details
                item.BrowserTitle = BrowserTitle

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As Category
            Dim item As Category

            item = GetCategoryByIDFunc(db, ID) ' (From i In db.Categories Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("Category with ID: " & ID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElement(ByVal Permalink As String) As Category
            Dim item As Category

            item = GetCategoryByPermalinkFunc(db, Permalink) ' (From i In db.Categories Where i.Permalink Like Permalink).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("The category you were looking for does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of Category)
            Dim CategoryList As New List(Of Category)

            CategoryList = GetCategoryFunc(db).ToList
            'From c In db.Categories Select c Where c.ID <> 2 'Remove where when membership is up

            If CategoryList Is Nothing And CategoryList.Count > 0 Then
                Throw New Exception("There are no Catergories")
            Else
                Return CategoryList
            End If
        End Function

        Public Shared GetCategoriesWithProductsFunc As Func(Of CommerceDataContext, IQueryable(Of Category)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) From c In db.Categories Join p In db.Products On p.CategoryID Equals c.ID Select c Distinct)

        Public Overloads Function GetCategoriesWithProducts() As List(Of Category)
            Dim CurrentCategories As List(Of Category) = GetCategoriesWithProductsFunc(db).ToList

            If CurrentCategories Is Nothing Then
                Throw New Exception("There are no Catergories")
            Else
                Return CurrentCategories
            End If
        End Function

        Public Shared Function GetRandomCategoryImage(ByVal CategoryId As Integer) As Integer
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentCount As Integer = ProductController.GetActiveProductsWithImagesFromCategoryIDCountFunc(CurrentDataContext, CategoryId)

            If CurrentCount > 0 Then
                Dim CurrentIndex As Integer = New Random().Next(CurrentCount)
                Dim CurrentProduct As Model.Product = ProductController.GetActiveProductsWithImagesFromCategoryIdFunc(CurrentDataContext, CategoryId).Skip(CurrentIndex).FirstOrDefault

                Return CurrentProduct.DefaultImageID
            Else
                Return 0
            End If
        End Function

        Public Shared GetCategoryNameByIdFunc As Func(Of CommerceDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, CategoryId As Integer) _
                                      (From c In CurrentDataContext.Categories _
                                       Where c.ID = CategoryId _
                                       Select c.Name).FirstOrDefault)

        Public Shared Function GetBreadCrumb(ByVal CategoryId As Integer) As String
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentCategory As String = GetCategoryNameByIdFunc(CurrentDataContext, CategoryId)

            Dim BreadCrumbStringBuilder As New StringBuilder
            BreadCrumbStringBuilder.AppendFormat("<a href=""/"">Home</a> {0} ", Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("<a href=""/Shop/Index.html"">Shop</a> {0} ", Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("{0}", CurrentCategory)

            GetBreadCrumb = BreadCrumbStringBuilder.ToString

            CurrentDataContext.Dispose()
        End Function

    End Class

End Namespace