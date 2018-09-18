Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Commerce

    Public Class AddressController
        Inherits DataControllerClass

        Public Shared GetAddressesFunc As Func(Of CommerceDataContext, IQueryable(Of Address)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext) From a In db.Addresses Select a)

        Public Shared GetAddressByIdFunc As Func(Of CommerceDataContext, Integer, Address) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, AddressId As Integer) _
                                              (From a In db.Addresses Where a.ID = AddressId Select a).FirstOrDefault)

        Public Overloads Function Create(ByVal Address As String, ByVal City As String, ByVal PCZIP As String, ByVal ProvStateId As Integer, ByVal CountryId As Integer) As Integer
            Dim CurrentAddress As New Address
            Dim CurrentAddressId As Integer

            CurrentAddress.Address = Address
            CurrentAddress.City = City
            CurrentAddress.PCZIP = PCZIP
            CurrentAddress.ProvStateID = ProvStateID

            db.Addresses.InsertOnSubmit(CurrentAddress)
            db.SubmitChanges()

            CurrentAddressId = CurrentAddress.ID

            Return CurrentAddressId
        End Function

        Public Overloads Function Create(ByVal Address As String, ByVal City As String, ByVal PCZIP As String, ByVal ProvStateID As Integer) As Integer
            Dim CurrentAddress As New Address
            Dim CurrentAddressId As Integer

            CurrentAddress.Address = Address
            CurrentAddress.City = City
            CurrentAddress.PCZIP = PCZIP
            CurrentAddress.ProvStateID = ProvStateID

            db.Addresses.InsertOnSubmit(CurrentAddress)
            db.SubmitChanges()

            CurrentAddressId = CurrentAddress.ID

            Return CurrentAddressId
        End Function

        Public Overloads Function Delete(ByVal AddressId As Integer) As Boolean
            Try
                Dim CurrentAddress As Address = GetAddressByIdFunc(db, AddressId)

                db.Addresses.DeleteOnSubmit(CurrentAddress)
                db.SubmitChanges()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal AddressId As Integer, ByVal Address As String, ByVal City As String, ByVal PCZIP As String, ByVal ProvStateID As Integer, ByVal CountryID As Integer) As Boolean
            Dim CurrentAddress As Address = GetAddressByIdFunc(db, AddressId)

            If CurrentAddress Is Nothing Then
                Throw New Exception(String.Format("Address {0} does not exist.", AddressId))
            Else
                CurrentAddress.Address = Address
                CurrentAddress.City = City
                CurrentAddress.PCZIP = PCZIP
                CurrentAddress.ProvStateID = ProvStateID

                db.SubmitChanges()
            End If

            Return True
        End Function

        Public Overloads Function Update(ByVal AddressId As Integer, ByVal Address As String, ByVal City As String, ByVal PCZIP As String, ByVal ProvStateID As Integer) As Boolean
            Dim CurrentAddress As Address = GetAddressByIdFunc(db, AddressId)

            If CurrentAddress Is Nothing Then
                Throw New Exception(String.Format("Address {0} does not exist.", AddressId))
            Else
                CurrentAddress.Address = Address
                CurrentAddress.City = City
                CurrentAddress.PCZIP = PCZIP
                CurrentAddress.ProvStateID = ProvStateID

                db.SubmitChanges()
            End If

            Return True
        End Function

        Public Overloads Function GetElement(ByVal AddressId As Integer) As Address
            Dim CurrentAddress As Address = GetAddressByIdFunc(db, AddressId)

            If CurrentAddress Is Nothing Then
                Throw New Exception(String.Format("Address with ID: {0} does not exist.", AddressId))
            Else
                Return CurrentAddress
            End If
        End Function

        Public Overloads Function GetElements() As List(Of Address)
            Dim CurrentAddresses As List(Of Address) = GetAddressesFunc(db).ToList

            If CurrentAddresses Is Nothing OrElse CurrentAddresses.Count < 1 Then
                Throw New Exception("There are no Addresses")
            Else
                Return CurrentAddresses
            End If
        End Function

        Public Shared GetAddressIdMatchCountFunc As Func(Of CommerceDataContext, Integer, Integer) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, AddressId As Integer) _
                                              (From o In db.Orders _
                                               Where o.SAddressID = AddressId _
                                               Or o.BAddressID = AddressId _
                                               Select o.ID).Count)

        Public Shared GetAddressIdUserMatchCountFunc As Func(Of CommerceDataContext, Integer, Integer) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, AddressId As Integer) _
                                              (From u In db.RegisteredUsers _
                                               Where u.AddressID = AddressId _
                                               Select u.ID).Count)

        Public Shared Function IsAddressSafeToUpdate(ByVal AddressId As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim OrdersCount As Integer = GetAddressIdMatchCountFunc(CurrentDataContext, AddressId)

            Dim UsersCount As Integer = GetAddressIdUserMatchCountFunc(CurrentDataContext, AddressId)

            If OrdersCount > 0 OrElse UsersCount > 1 Then
                IsAddressSafeToUpdate = False
            Else
                IsAddressSafeToUpdate = True
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetSafeToDeleteAddressesFunc As Func(Of CommerceDataContext, IQueryable(Of Address)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                              From a In db.Addresses Where _
                                              (From o In db.Orders Where o.SAddressID = a.ID Or o.BAddressID = a.ID).Count = 0 _
                                              And (From ru In db.RegisteredUsers Where ru.AddressID = a.ID).Count = 0 Select a)

        Public Shared Function GetUnusedAddresses() As List(Of Address)
            Dim CurrentDataContext As New CommerceDataContext

            GetUnusedAddresses = GetSafeToDeleteAddressesFunc(CurrentDataContext).ToList()

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function DeleteUnusedAddresses() As Integer
            Try
                Dim CurrentDataContext As New CommerceDataContext

                Dim CurrentAddresses As List(Of Address) = GetSafeToDeleteAddressesFunc(CurrentDataContext).ToList
                Dim CurrentAddressesCount As Integer = CurrentAddresses.Count

                CurrentDataContext.Addresses.DeleteAllOnSubmit(CurrentAddresses)
                CurrentDataContext.SubmitChanges()

                DeleteUnusedAddresses = CurrentAddressesCount

                CurrentDataContext.Dispose()
            Catch CurrentException As Exception
                DeleteUnusedAddresses = 0
            End Try
        End Function

    End Class

    Public Class ProvinceController
        Inherits DataControllerClass
        
        Public Shared GetProvincesFunc As Func(Of CommerceDataContext, IQueryable(Of Province)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext) From p In db.Provinces Select p)

        Public Shared GetProvinceByIdFunc As Func(Of CommerceDataContext, Integer, Province) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, ProvinceId As Integer) _
                                              (From p In db.Provinces Where p.ID = ProvinceId Select p).FirstOrDefault)

        Public Shared GetProvinceNameByIdFunc As Func(Of CommerceDataContext, Integer, String) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, ProvinceId As Integer) _
                                              (From p In db.Provinces Where p.ID = ProvinceId Select p).FirstOrDefault.Name)

        Public Shared GetProvincesByCountryIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Province)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, CountryId As Integer) From p In db.Provinces Where p.CountryID = CountryId Select p)


        Public Overloads Function Create(ByVal Name As String, ByVal Tax As Decimal, ByVal CountryID As Integer, ByVal Code As String) As Integer
            Dim item As New Province
            Dim itemId As Integer

            item.Name = Name
            item.Tax = Tax
            item.CountryID = CountryID
            item.Code = Code

            db.Provinces.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = GetProvinceByIDFunc(db, ID)

                db.Provinces.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal Name As String, ByVal Tax As Decimal, ByVal CountryID As Integer, ByVal Code As String) As Boolean
            Dim item As Province

            item = GetProvinceByIdFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("Province " & ID.ToString & " does not exist.")
            Else
                item.Name = Name
                item.Tax = Tax
                item.CountryID = CountryID
                item.Code = Code

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As Province
            Dim item As Province

            item = GetProvinceByIDFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("Province " & ID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of Province)
            Dim CurrentProvinces As List(Of Province) = GetProvincesFunc(db).ToList

            If CurrentProvinces Is Nothing OrElse CurrentProvinces.Count < 1 Then
                Throw New Exception("There are no Provinces")
            Else
                Return CurrentProvinces
            End If
        End Function

        Public Overloads Function GetCountryProvince(ByVal CountryId As Integer) As List(Of Province)
            Dim CurrentProvinces As List(Of Province) = GetProvincesByCountryIdFunc(db, CountryId).ToList

            If CurrentProvinces Is Nothing OrElse CurrentProvinces.Count < 1 Then
                Throw New Exception("There are no Provinces")
            Else
                Return CurrentProvinces
            End If
        End Function

        Public Function GetProvWithCountry()
            'Dim PWC As New List(Of ProvinceWithCountry)

            Dim joinedProvinceList = (From p In db.Provinces Join c In db.Countries On p.CountryID Equals c.ID _
                                     Select New With {p.ID, p.Name, p.Code, p.Tax, p.CountryID, .CountryName = c.Name})

            'For Each item In joinedProvinceList
            '    Dim prov As New ProvinceWithCountry

            '    prov.ID = item.ID
            '    prov.Name = item.Name
            '    prov.Code = item.Code
            '    prov.Tax = item.Tax
            '    prov.CountryID = item.CountryID
            '    prov.CountryName = item.CountryName

            '    PWC.Add(prov)
            'Next

            Return joinedProvinceList ' PWC
        End Function

        Public Class ProvinceWithCountry
            Public ID As Integer
            Public Name As String
            Public Code As String
            Public Tax As Decimal
            Public CountryId As Integer
            Public CountryName As String
        End Class

        Public Shared Function GetProvinceName(ByVal ProvinceId As Integer) As String
            Dim CurrentDataContext As New CommerceDataContext

            GetProvinceName = GetProvinceNameByIdFunc(CurrentDataContext, ProvinceId)

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetProvinceCountryNameByIdFunc As Func(Of CommerceDataContext, Integer, String) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, ProvinceId As Integer) _
                                              (From p In db.Provinces Where p.ID = ProvinceId Select p.Country.Name).FirstOrDefault)

        Public Shared Function GetProvinceCountryName(ProvinceId As Integer) As String
            Dim CurrentDataContext As New CommerceDataContext

            GetProvinceCountryName = GetProvinceCountryNameByIdFunc(CurrentDataContext, ProvinceId)

            CurrentDataContext.Dispose()
        End Function

    End Class

    Public Class CountryController
        Inherits DataControllerClass

        Public Shared GetCountryFunc As Func(Of CommerceDataContext, IQueryable(Of Country)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext) From c In db.Countries Select c)

        Public Shared GetCountryByIdFunc As Func(Of CommerceDataContext, Integer, Country) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, CountryID As Integer) _
                                              (From c In db.Countries Where c.ID = CountryID Select c).FirstOrDefault)

        Public Shared GetCountryByProvinceFunc As Func(Of CommerceDataContext, Integer, Country) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, ProvinceID As Integer) _
                                              (From p In db.Provinces Join c In db.Countries On c.ID Equals p.CountryID Where p.ID = ProvinceID Select c).FirstOrDefault)

        Public Overloads Function Create(ByVal Name As String, ByVal Tax As Decimal, ByVal Code As String) As Integer
            Dim item As New Country
            Dim itemId As Integer

            item.Name = Name
            item.Tax = Tax
            item.Code = Code

            db.Countries.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = GetCountryByIDFunc(db, ID)

                db.Countries.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal Name As String, ByVal Tax As Decimal, ByVal Code As String) As Boolean
            Dim item As Country = GetCountryByIdFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("Country " & ID.ToString & " does not exist.")
            Else
                item.Name = Name
                item.Tax = Tax
                item.Code = Code

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal CountryId As Integer) As Country
            Dim CurrentCountry As Country = GetCountryByIDFunc(db, CountryId)

            If CurrentCountry Is Nothing Then
                Throw New Exception(String.Format("Country {0} does not exist.", CountryId))
            Else
                Return CurrentCountry
            End If
        End Function

        Public Overloads Function GetElements() As List(Of Country)
            Dim CurrentCountries As List(Of Country) = GetCountryFunc(db).ToList

            If CurrentCountries Is Nothing OrElse CurrentCountries.Count < 1 Then
                Throw New Exception("There are no Countries")
            Else
                Return CurrentCountries
            End If
        End Function

        Public Overloads Function GetCountryByProvince(ByVal ProvinceId As Integer) As Country
            Dim CurrentCountry As Country = GetCountryByProvinceFunc(db, ProvinceId)

            If CurrentCountry Is Nothing Then
                Throw New Exception("The country you are looking for does not exist")
            Else
                Return CurrentCountry
            End If
        End Function

        Public Shared GetCountryIdByProvinceIdFunc As Func(Of CommerceDataContext, Integer, Integer) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProvinceId As Integer) _
                                              (From p In CurrentDataContext.Provinces _
                                               Join c In CurrentDataContext.Countries _
                                               On c.ID Equals p.CountryID _
                                               Where p.ID = ProvinceId _
                                               Select c.ID).FirstOrDefault)

        Public Shared Function GetCountryIdByProvinceId(ByVal ProvinceId As Integer) As Integer
            Dim CurrentDataContext As New CommerceDataContext

            GetCountryIdByProvinceId = GetCountryIdByProvinceIdFunc(CurrentDataContext, ProvinceId)

            CurrentDataContext.Dispose()
        End Function

    End Class

End Namespace