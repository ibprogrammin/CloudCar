Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement.PropertyModule

    Public Class PropertyController

        Public Shared GetPropertyByIdFunc As Func(Of CommerceDataContext, Integer, [Property]) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, Id As Integer) (From p In db.Properties Where p.Id = Id Select p).FirstOrDefault)

        Public Shared GetAllPropertiesFunc As Func(Of CommerceDataContext, IQueryable(Of [Property])) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) From p In db.Properties Select p)

        Public Shared GetPropertyByPermalinkFunc As Func(Of CommerceDataContext, String, [Property]) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, Permalink As String) (From p In db.Properties Where p.Permalink Like Permalink And p.Active = True Select p).FirstOrDefault)

        Public Shared GetPermalinkExistsFunc As Func(Of CommerceDataContext, String, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, Permalink As String) (From p In db.Properties Where p.Permalink Like Permalink Select p).Count)

        Public Shared GetPermalinkExistsUpdateFunc As Func(Of CommerceDataContext, String, Integer, Integer) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, Permalink As String, PropertyId As Integer) (From p In db.Properties Where p.Permalink Like Permalink And Not p.Id = PropertyId Select p).Count)

        Public Shared GetHighestPropertyPriceFunc As Func(Of CommerceDataContext, Decimal) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      (From p In db.Properties Order By p.Price Descending Select p.Price).FirstOrDefault)

        Public Shared GetLowestPropertyPriceFunc As Func(Of CommerceDataContext, Decimal) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      (From p In db.Properties Order By p.Price Select p.Price).FirstOrDefault)

        Public Shared GetPropertyCitiesFunc As Func(Of CommerceDataContext, IQueryable(Of String)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      From p In db.Properties Join a In db.Addresses On p.AddressId Equals a.ID Order By a.City Select a.City Distinct)

        Public Shared SearchPropertiesFunc As Func(Of CommerceDataContext, PropertySearch, IOrderedQueryable(Of [Property])) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, Search As PropertySearch) _
                                      From p In db.Properties Join a In db.Addresses On p.AddressId Equals a.ID _
                                      Where p.Price >= Search.PriceLow And p.Price <= Search.PriceHigh _
                                      And If(Search.Bedrooms = 0, True, p.Bedrooms >= Search.Bedrooms - 1 And p.Bedrooms <= Search.Bedrooms + 1) _
                                      And If(Search.Bathrooms = 0, True, p.Bathrooms >= Search.Bathrooms - 1 And p.Bathrooms <= Search.Bathrooms + 1) _
                                      And If(Search.City = String.Empty, True, a.City.ToLower = Search.City.ToLower) _
                                      And p.Active = True And p.Vacancy = True _
                                      Select p Order By p.Price Ascending)

        Public Shared Function Create(ByVal Title As String, ByVal Details As String, ByVal ListingId As String, ByVal AddressId As Integer, ByVal ImageGalleryId As Integer, ByVal Testimonial As String, ByVal Price As Decimal, ByVal Bedrooms As Integer, ByVal Bathrooms As Integer, ByVal PageTitle As String, ByVal Permalink As String, ByVal Keywords As String, ByVal Description As String, ByVal ListingLink As String, ByVal Active As Boolean, ByVal Vacancy As Boolean) As Integer
            Dim db As New CommerceDataContext
            Dim item As New [Property]
            Dim itemId As Integer

            item.Title = Title
            item.Details = Details
            item.ListingId = ListingId
            item.AddressId = AddressId
            item.ImageGalleryId = ImageGalleryId
            item.Testimonial = Testimonial
            item.Price = Price
            item.Bedrooms = Bedrooms
            item.Bathrooms = Bathrooms
            item.PageTitle = PageTitle
            item.Permalink = Permalink
            item.Keywords = Keywords
            item.Description = Description
            item.ListingLink = ListingLink
            item.Active = Active
            item.Vacancy = Vacancy

            db.Properties.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.Id

            item = Nothing
            db = Nothing

            Return itemId
        End Function

        Public Shared Sub Update(ByVal Id As Integer, ByVal Title As String, ByVal Details As String, ByVal ListingId As String, ByVal AddressId As Integer, ByVal ImageGalleryId As Integer, ByVal Testimonial As String, ByVal Price As Decimal, ByVal Bedrooms As Integer, ByVal Bathrooms As Integer, ByVal PageTitle As String, ByVal Permalink As String, ByVal Keywords As String, ByVal Description As String, ByVal ListingLink As String, ByVal Active As Boolean, ByVal Vacancy As Boolean)
            Dim db As New CommerceDataContext

            Dim item As [Property] = GetPropertyByIDFunc(db, Id)

            If item Is Nothing Then
                Throw New InvalidPropertyException("Property does not exist")
            Else
                item.Title = Title
                item.Details = Details
                item.ListingId = ListingId
                item.AddressId = AddressId
                item.ImageGalleryId = ImageGalleryId
                item.Testimonial = Testimonial
                item.Price = Price
                item.Bedrooms = Bedrooms
                item.Bathrooms = Bathrooms
                item.PageTitle = PageTitle
                item.Permalink = Permalink
                item.Keywords = Keywords
                item.Description = Description
                item.ListingLink = ListingLink
                item.Active = Active
                item.Vacancy = Vacancy

                db.SubmitChanges()
            End If

            item = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Try
                Dim db As New CommerceDataContext

                Dim item As [Property] = GetPropertyByIDFunc(db, Id)

                db.Properties.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetElement(ByVal Id As Integer) As [Property]
            Dim db As New CommerceDataContext

            Dim item As [Property] = GetPropertyByIDFunc(db, Id)

            If item Is Nothing Then
                Throw New InvalidPropertyException("Property does not exist")
            Else
                Return item
            End If

            db = Nothing
        End Function

        Public Shared Function GetElements() As List(Of [Property])
            Dim db As New CommerceDataContext

            Dim items As List(Of [Property]) = GetAllPropertiesFunc(db).OrderBy(Function(f) f.ListingId).ToList

            If items Is Nothing Then
                Throw New InvalidPropertyException("There are no properties stored in the data table.")
            Else
                Return items
            End If

            db = Nothing
        End Function

        Public Shared Function GetPropertyFromLink(ByVal Permalink As String) As [Property]
            Dim db As New CommerceDataContext
            Dim SelectedProperty As [Property] = GetPropertyByPermalinkFunc(db, Permalink)

            If SelectedProperty Is Nothing Then
                Throw New InvalidPropertyException("The property you have requested does not exist.")
            Else
                Return SelectedProperty
            End If

            db = Nothing
        End Function

        Public Shared Function HasPermalink(ByVal Permalink As String) As Boolean
            Dim db As New CommerceDataContext

            Dim pageCount As Integer = GetPermalinkExistsFunc(db, Permalink)

            If pageCount = 0 Then
                Return False
            Else
                Return True
            End If

            db = Nothing
        End Function

        Public Shared Function HasPermalink(ByVal Permalink As String, ByVal PropertyId As Integer) As Boolean
            Dim db As New CommerceDataContext

            Dim pageCount As Integer
            pageCount = GetPermalinkExistsUpdateFunc(db, Permalink, PropertyId)

            If pageCount = 0 Then
                Return False
            Else
                Return True
            End If

            db = Nothing
        End Function

        Public Shared Function GetLowestPropertyPrice() As Decimal
            Dim db As New CommerceDataContext

            Dim Price As Decimal = GetLowestPropertyPriceFunc(db)

            If Not Price = Nothing Then
                Return Price
            Else
                Return 0
            End If
        End Function

        Public Shared Function GetHighestPropertyPrice() As Decimal
            Dim db As New CommerceDataContext

            Dim Price As Decimal = GetHighestPropertyPriceFunc(db)

            If Not Price = Nothing Then
                Return Price
            Else
                Return GetLowestPropertyPrice()
            End If
        End Function

        Public Shared Function GetPropertyCities() As List(Of String)
            Dim db As New CommerceDataContext

            Dim Cities As List(Of String) = GetPropertyCitiesFunc(db).ToList

            If Not Cities Is Nothing Then
                Return Cities
            Else
                Cities = New List(Of String)
                Cities.Add("None")

                Return Cities
            End If
        End Function

        Public Shared Function SearchProperties(ByVal City As String, ByVal PriceLow As Decimal, ByVal PriceHigh As Decimal, ByVal Bedrooms As Integer, ByVal Bathrooms As Integer) As List(Of [Property])
            Dim CurrentDataContext As New CommerceDataContext

            Dim SearchParamaters As New PropertySearch

            SearchParamaters.City = City
            SearchParamaters.PriceLow = PriceLow
            SearchParamaters.PriceHigh = PriceHigh
            SearchParamaters.Bedrooms = Bedrooms
            SearchParamaters.Bathrooms = Bathrooms

            Dim Items As List(Of [Property]) = SearchPropertiesFunc(CurrentDataContext, SearchParamaters).ToList

            If Items Is Nothing Then
                Throw New InvalidPropertyException("There were no resulting properties returned in your search query.")
            Else
                Return Items
            End If

            CurrentDataContext = Nothing
        End Function

        Public Shared Function ChangePropertyImageGallery(ByVal PropertyId As Integer, ByVal ImageGalleryId As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentProperty As [Property] = GetPropertyByIDFunc(CurrentDataContext, PropertyId)

            If CurrentProperty Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidPropertyException("Property does not exist")
            Else
                CurrentProperty.ImageGalleryId = ImageGalleryId

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()

                ChangePropertyImageGallery = True
            End If
        End Function

        Public Shared Sub SetPropertyActive(ByVal Id As Integer, ByVal Active As Boolean)
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentProperty As [Property] = GetPropertyByIDFunc(CurrentDataContext, Id)

            If CurrentProperty Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidPropertyException("Property does not exist")
            Else
                CurrentProperty.Active = Active

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If
        End Sub

        Public Shared Sub SetPropertyVacant(ByVal Id As Integer, ByVal Vacant As Boolean)
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentProperty As [Property] = GetPropertyByIDFunc(CurrentDataContext, Id)

            If CurrentProperty Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidPropertyException("Property does not exist")
            Else
                CurrentProperty.Vacancy = Vacant

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If
        End Sub

    End Class

    Public Class InvalidPropertyException
        Inherits Exception

        Public Sub New(ByVal Message As String)
            MyBase.New(Message)
        End Sub

    End Class

    Public Class PropertyFeatureController

        Public Shared GetPropertyFeatureByIdFunc As Func(Of CommerceDataContext, Integer, PropertyFeature) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, Id As Integer) (From p In db.PropertyFeatures Where p.Id = Id Select p).FirstOrDefault)

        Public Shared GetAllPropertyFeaturesFunc As Func(Of CommerceDataContext, IQueryable(Of PropertyFeature)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) From p In db.PropertyFeatures Select p)

        Public Shared GetPropertyFeaturesByPropertyIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of PropertyFeature)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, PropertyId As Integer) From p In db.PropertyFeatures Where p.PropertyId = PropertyId Select p)

        Public Shared GetPropertyFeaturesByFeatureIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of PropertyFeature)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, PropertyId As Integer) From p In db.PropertyFeatures Where p.FeatureId = PropertyId Select p)

        Public Shared Function Create(ByVal PropertyId As Integer, ByVal FeatureId As Integer) As Integer
            Dim db As New CommerceDataContext
            Dim item As New PropertyFeature
            Dim itemId As Integer

            item.PropertyId = PropertyId
            item.FeatureId = FeatureId

            db.PropertyFeatures.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.Id

            item = Nothing
            db = Nothing

            Return itemId
        End Function

        Public Shared Sub Update(ByVal Id As Integer, ByVal PropertyId As Integer, ByVal FeatureId As Integer)
            Dim db As New CommerceDataContext

            Dim item As PropertyFeature = GetPropertyFeatureByIdFunc(db, Id)

            If item Is Nothing Then
                Throw New InvalidPropertyFeatureException("Property feature does not exist")
            Else
                item.PropertyId = PropertyId
                item.FeatureId = FeatureId

                db.SubmitChanges()
            End If

            item = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Try
                Dim db As New CommerceDataContext

                Dim item As PropertyFeature = GetPropertyFeatureByIdFunc(db, Id)

                db.PropertyFeatures.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetElement(ByVal Id As Integer) As PropertyFeature
            Dim db As New CommerceDataContext

            Dim item As PropertyFeature = GetPropertyFeatureByIdFunc(db, Id)

            If item Is Nothing Then
                Throw New InvalidPropertyFeatureException("Property Feature does not exist")
            Else
                Return item
            End If

            db = Nothing
        End Function

        Public Shared Function GetElements() As List(Of PropertyFeature)
            Dim db As New CommerceDataContext

            Dim items As List(Of PropertyFeature) = GetAllPropertyFeaturesFunc(db).ToList

            If items Is Nothing Then
                Throw New InvalidPropertyFeatureException("There are no property features stored in the data table.")
            Else
                Return items
            End If

            db = Nothing
        End Function

        Public Shared Function GetPropertyFeaturesByProperty(ByVal PropertyId As Integer) As List(Of PropertyFeature)
            Dim db As New CommerceDataContext

            Dim items As List(Of PropertyFeature) = GetPropertyFeaturesByPropertyIdFunc(db, PropertyId).ToList

            If items Is Nothing Then
                Throw New InvalidPropertyFeatureException("There are no property features linked with this property.")
            Else
                Return items
            End If

            db = Nothing
        End Function

    End Class

    Public Class InvalidPropertyFeatureException
        Inherits Exception

        Public Sub New(ByVal Message As String)
            MyBase.New(Message)
        End Sub

    End Class

    Public Class PropertySearch
        Public City As String
        Public PriceLow As Decimal
        Public PriceHigh As Decimal
        Public Bedrooms As Integer
        Public Bathrooms As Integer
    End Class

End Namespace