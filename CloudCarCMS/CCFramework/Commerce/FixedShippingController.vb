Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Commerce

    Public Class FixedShippingZoneController

        Public Shared GetShippingZoneByIdFunc As Func(Of CommerceDataContext, Integer, FixedShippingZone) = _
                CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) (From z In db.FixedShippingZones Where z.ID = id Select z).FirstOrDefault)

        Public Shared Function Create(ByVal PrefixLow As String, ByVal PrefixHigh As String, ByVal Zone As Integer, ByVal ProvinceId As Integer, ByVal DistributorUserID As Integer) As Integer
            Dim db As New CommerceDataContext

            Dim shippingZone As New FixedShippingZone

            shippingZone.PrefixLow = PrefixLow
            shippingZone.PrefixHigh = PrefixHigh
            shippingZone.Zone = Zone
            shippingZone.ProvinceID = ProvinceId
            shippingZone.DistributorUserID = DistributorUserID

            db.FixedShippingZones.InsertOnSubmit(shippingZone)
            db.SubmitChanges()

            Create = shippingZone.ID
            db.Dispose()
        End Function

        Public Shared Sub Update(ByVal ID As Integer, ByVal PrefixLow As String, ByVal PrefixHigh As String, ByVal Zone As Integer, ByVal ProvinceID As Integer, ByVal DistributorUserID As Integer)
            Dim db As New CommerceDataContext
            Dim shippingZone As FixedShippingZone

            shippingZone = GetShippingZoneByIdFunc(db, ID)

            If shippingZone Is Nothing Then
                Throw New InvalidShippingZoneException()
            Else
                shippingZone.PrefixLow = PrefixLow
                shippingZone.PrefixHigh = PrefixHigh
                shippingZone.Zone = Zone
                shippingZone.ProvinceID = ProvinceID
                shippingZone.DistributorUserID = DistributorUserID

                db.SubmitChanges()
            End If

            shippingZone = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim db As New CommerceDataContext

                Dim zone = GetShippingZoneByIdFunc(db, ID)

                db.FixedShippingZones.DeleteOnSubmit(zone)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared GetAllZoneDistributorsFunc As Func(Of CommerceDataContext, IQueryable(Of RegisteredUser)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) From z In db.FixedShippingZones Join ru In db.RegisteredUsers On ru.ID Equals z.DistributorUserID Select ru Distinct)

        Public Shared GetDistributorShippingZonesFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of FixedShippingZone)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, UserId As Integer) From z In db.FixedShippingZones Where z.DistributorUserID = UserId Select z)

        Public Shared GetAllShippingZonesFunc As Func(Of CommerceDataContext, IQueryable(Of FixedShippingZone)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) From z In db.FixedShippingZones Select z)

        Public Shared Function GetAllShippingZoneDistributors() As System.Linq.IQueryable(Of RegisteredUser)
            Dim db As New CommerceDataContext

            Dim distributors = GetAllZoneDistributorsFunc(db)

            If distributors Is Nothing Then
                Throw New InvalidShippingZoneException()
            Else
                Return distributors
            End If

            db = Nothing
        End Function

        Public Shared Function GetShippingZones() As System.Linq.IQueryable(Of FixedShippingZone)
            Dim db As New CommerceDataContext

            Dim zones = GetAllShippingZonesFunc(db)

            If zones Is Nothing Then
                Throw New InvalidShippingZoneException()
            Else
                GetShippingZones = zones
            End If

            db = Nothing
        End Function

        Public Shared Function GetShippingZone(ByVal ID As Integer) As FixedShippingZone
            Dim db As New CommerceDataContext
            Dim zone As FixedShippingZone

            zone = GetShippingZoneByIdFunc(db, ID)

            If zone Is Nothing Then
                Throw New InvalidShippingZoneException()
            Else
                GetShippingZone = zone
            End If

            db = Nothing
        End Function

        Public Shared Function GetDistributorShippingZones(ByVal UserId As Integer) As System.Linq.IQueryable(Of FixedShippingZone)
            Dim db As New CommerceDataContext

            Dim zones = GetDistributorShippingZonesFunc(db, UserId)

            If zones Is Nothing Then
                Throw New InvalidShippingZoneException()
            Else
                Return zones
            End If

            db = Nothing
        End Function

        Public Shared GetShippingZoneByPrefixFunc As Func(Of CommerceDataContext, String, FixedShippingZone) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, Prefix As String) _
                                      (From z In db.FixedShippingZones Where CheckPrefix(Prefix, z.PrefixLow, z.PrefixHigh) Select z).FirstOrDefault)

        'Public Shared Function GetShippingZone(ByVal Prefix As String) As Integer
        '    Dim db As New CommerceDataContext
        '    Dim zone As FixedShippingZone

        '    zone = GetShippingZoneByPrefixFunc(db, Prefix)

        '    If zone Is Nothing Then
        '        Throw New InvalidShippingZoneException()
        '    Else
        '        GetShippingZone = zone.Zone
        '    End If

        '    db = Nothing
        'End Function

        Public Shared Function GetShippingZone(ByVal Prefix As String) As Integer
            Dim db As New CommerceDataContext
            Dim zones As List(Of FixedShippingZone)

            zones = GetAllShippingZonesFunc(db).ToList

            Dim zone As Integer

            For Each item As FixedShippingZone In zones
                If CheckPrefix(Prefix, item.PrefixLow, item.PrefixHigh) Then
                    zone = item.Zone
                    Exit For
                End If
            Next

            If zone = Nothing Then
                Throw New InvalidShippingZoneException()
            Else
                GetShippingZone = zone
            End If

            db.Dispose()
        End Function

        Public Shared Function GetShippingZoneDistributor(ByVal Prefix As String) As Integer
            Dim db As New CommerceDataContext
            Dim zones As List(Of FixedShippingZone)

            zones = GetAllShippingZonesFunc(db).ToList

            Dim zone As FixedShippingZone = Nothing

            For Each item As FixedShippingZone In zones
                If CheckPrefix(Prefix, item.PrefixLow, item.PrefixHigh) Then
                    zone = item
                    Exit For
                End If
            Next

            If zone Is Nothing Then
                'Throw New InvalidShippingZoneException()
                GetShippingZoneDistributor = Nothing
            Else
                GetShippingZoneDistributor = zone.DistributorUserID
            End If

            db.Dispose()
        End Function

        Public Shared Function GetUnselectedZones() As List(Of String)
            Dim StartPrefixChar As Char = "A"c
            Dim EndPrefixChar As Char = "Z"c

            Dim StartPrefixInt As Integer = Convert.ToInt16(StartPrefixChar)
            Dim EndPrefixInt As Integer = Convert.ToInt16(EndPrefixChar)

            Dim ListOfPrefixes As New List(Of String)

            For i As Integer = StartPrefixInt To EndPrefixInt
                For j As Integer = 0 To 9
                    For k As Integer = StartPrefixInt To EndPrefixInt
                        Dim Prefix As String = Convert.ToChar(i).ToString & j.ToString & Convert.ToChar(k)

                        If FixedShippingZoneController.GetShippingZoneDistributor(Prefix) = Nothing Then
                            ListOfPrefixes.Add(Prefix)
                        End If
                    Next
                Next
            Next

            Return ListOfPrefixes
        End Function

        Public Class InvalidShippingZoneException
            Inherits Exception

            Public Sub New()
                MyBase.New("The shipping zone you are looking for does not exist")
            End Sub

        End Class

        'Moved to a CLR Function within the SQL Server DB
        'Public Shared Function CheckPrefix(ByVal Prefix As String, ByVal PrefixLow As SqlTypes.SqlString, ByVal PrefixHigh As SqlTypes.SqlString) As SqlTypes.SqlBoolean ' Boolean
        '    Dim p As String = Prefix.ToLower
        '    Dim pl As String = PrefixLow.ToString.ToLower
        '    Dim ph As String = PrefixHigh.ToString.ToLower

        '    Dim match As New Regex("^[" & pl.Substring(0, 1) & "-" & ph.Substring(0, 1) & _
        '                            "][" & pl.Substring(1, 1) & "-" & ph.Substring(1, 1) & _
        '                            "][" & pl.Substring(2, 1) & "-" & ph.Substring(2, 1) & "]$")

        '    If match.IsMatch(p) Then
        '        Return New SqlTypes.SqlBoolean(True)
        '    Else
        '        Return New SqlTypes.SqlBoolean(False)
        '    End If
        'End Function

        Public Shared Function ClearZipCode(ByVal ZipCode As String, ByRef NewZipCode As Integer) As Boolean
            If Integer.TryParse(ZipCode.Substring(0, ZipCode.Length - 1).TrimStart("0"c) & ZipCode.Substring(ZipCode.Length - 1, 1), NewZipCode) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function CheckPrefix(ByVal Prefix As String, ByVal PrefixLow As String, ByVal PrefixHigh As String) As Boolean
            Dim ZipCode As Integer

            If ClearZipCode(Prefix, ZipCode) Then
                Dim ZipCodeLow As Integer
                Dim ZipCodeHigh As Integer

                If ClearZipCode(PrefixLow, ZipCodeLow) And ClearZipCode(PrefixHigh, ZipCodeHigh) Then
                    If ZipCode >= ZipCodeLow And ZipCode <= ZipCodeHigh Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return CheckCodeInRange(Prefix.Substring(0, 3), PrefixLow.Substring(0, 3), PrefixHigh.Substring(0, 3))
            End If
        End Function

        Public Shared Function CheckCodeInRange(ByVal Code As String, ByVal CodeLow As String, ByVal CodeHigh As String) As Boolean
            Dim s As String = Code.ToLower
            Dim a As String = CodeLow.ToLower
            Dim b As String = CodeHigh.ToLower

            Dim s1 As Integer = Convert.ToInt16(s.Substring(0, 1).ToCharArray.First)
            Dim s2 As Integer = Convert.ToInt16(s.Substring(1, 1).ToCharArray.First)

            Dim a1 As Integer = Convert.ToInt16(a.Substring(0, 1).ToCharArray.First)
            Dim a2 As Integer = Convert.ToInt16(a.Substring(1, 1).ToCharArray.First)

            Dim b1 As Integer = Convert.ToInt16(b.Substring(0, 1).ToCharArray.First)
            Dim b2 As Integer = Convert.ToInt16(b.Substring(1, 1).ToCharArray.First)


            If s1 >= a1 And s1 <= b1 Then
                If s1 = a1 And s1 = b1 Then
                    If s2 >= a2 And s2 <= b2 Then
                        If Code.Length > 2 Then
                            Return CheckCodeInRange(Code.Remove(0, 1), CodeLow.Remove(0, 1), CodeHigh.Remove(0, 1))
                        Else
                            Return True
                        End If
                    Else
                        Return False
                    End If
                ElseIf s1 = a1 Then
                    If s2 >= a2 Then
                        If Code.Length > 2 Then
                            Return CheckCodeInRange(Code.Remove(0, 1), CodeLow.Remove(0, 1), CodeHigh.Remove(0, 1))
                        Else
                            Return True
                        End If
                    Else
                        Return False
                    End If
                ElseIf s1 = b1 Then
                    If s2 <= b2 Then
                        If Code.Length > 2 Then
                            Return CheckCodeInRange(Code.Remove(0, 1), CodeLow.Remove(0, 1), CodeHigh.Remove(0, 1))
                        Else
                            Return True
                        End If
                    Else
                        Return False
                    End If
                Else
                    If Code.Length > 2 Then
                        Return CheckCodeInRange(Code.Remove(0, 1), CodeLow.Remove(0, 1), CodeHigh.Remove(0, 1))
                    Else
                        Return True
                    End If
                End If
            Else
                Return False
            End If

        End Function

        Public Shared Function GetBranch(ByVal Code As String) As String
            Dim CurrentStoreDataContext As New CommerceDataContext

            Dim DistributorId As Integer

            If Not Integer.TryParse(Code, DistributorId) Then
                DistributorId = GetShippingZoneDistributor(Code)
            End If

            Dim CurrentRegisteredUser As RegisteredUser = RegisteredUserController.GetRegisteredUserByUserIDFunc(CurrentStoreDataContext, DistributorId)

            If CurrentRegisteredUser Is Nothing Then
                GetBranch = "None"
            Else
                GetBranch = CurrentRegisteredUser.UserName
            End If

            CurrentStoreDataContext.Dispose()
        End Function

        Public Shared Function GetBranchCity(ByVal Code As String) As String
            Dim CurrentStoreDataContext As New CommerceDataContext

            Dim DistributorId As Integer

            If Not Integer.TryParse(Code, DistributorId) Then
                DistributorId = GetShippingZoneDistributor(Code)
            End If

            Dim CurrentRegisteredUser As RegisteredUser = RegisteredUserController.GetRegisteredUserByUserIDFunc(CurrentStoreDataContext, DistributorId)

            If CurrentRegisteredUser Is Nothing Then
                GetBranchCity = "None"
            Else
                GetBranchCity = CurrentRegisteredUser.Address.City
            End If

            CurrentStoreDataContext.Dispose()
        End Function

    End Class

    Public Class FixedShippingRateController

        Public Shared GetShippingRateByIdFunc As Func(Of CommerceDataContext, Integer, FixedShippingRate) = _
                CompiledQuery.Compile(Function(db As CommerceDataContext, id As Integer) (From r In db.FixedShippingRates Where r.ID = id Select r).FirstOrDefault)

        Public Shared Function Create(ByVal WeightLBS As Decimal, ByVal WeightKGS As Decimal, ByVal Zone As Integer, ByVal Cost As Decimal) As Integer
            Dim db As New CommerceDataContext

            Dim shippingRate As New FixedShippingRate

            shippingRate.WeightLBS = WeightLBS
            shippingRate.WeightKGS = WeightKGS
            shippingRate.Zone = Zone
            shippingRate.Cost = Cost

            db.FixedShippingRates.InsertOnSubmit(shippingRate)
            db.SubmitChanges()

            Create = shippingRate.ID

            db.Dispose()
        End Function

        Public Shared Sub Update(ByVal ID As Integer, ByVal WeightLBS As Decimal, ByVal WeightKGS As Decimal, ByVal Zone As Integer, ByVal Cost As Decimal)
            Dim db As New CommerceDataContext
            Dim shippingRate As FixedShippingRate

            shippingRate = GetShippingRateByIdFunc(db, ID)

            If shippingRate Is Nothing Then
                Throw New InvalidShippingRateException()
            Else
                shippingRate.WeightLBS = WeightLBS
                shippingRate.WeightKGS = WeightKGS
                shippingRate.Zone = Zone
                shippingRate.Cost = Cost

                db.SubmitChanges()
            End If

            db.Dispose()
        End Sub

        Public Shared Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim db As New CommerceDataContext

                Dim rate = GetShippingRateByIdFunc(db, ID)

                db.FixedShippingRates.DeleteOnSubmit(rate)
                db.SubmitChanges()

                Delete = True

                db.Dispose()
            Catch ex As Exception
                Delete = False
            End Try
        End Function

        Public Shared GetAllShippingRatesFunc As Func(Of CommerceDataContext, IQueryable(Of FixedShippingRate)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      From r In db.FixedShippingRates Select r)

        Public Shared Function GetShippingRates() As List(Of FixedShippingRate)
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentRates As List(Of FixedShippingRate)
            CurrentRates = GetAllShippingRatesFunc(CurrentDataContext).ToList

            If CurrentRates Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidShippingRateException()
            Else
                GetShippingRates = CurrentRates

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetShippingRate(ByVal ID As Integer) As FixedShippingRate
            Dim db As New CommerceDataContext
            Dim rate As FixedShippingRate

            rate = GetShippingRateByIdFunc(db, ID)

            If rate Is Nothing Then
                Throw New InvalidShippingRateException()
            Else
                GetShippingRate = rate
            End If

            db.Dispose()
        End Function

        Public Class InvalidShippingRateException
            Inherits Exception

            Public Sub New()
                MyBase.New("The shipping rate you are looking for does not exist")
            End Sub

        End Class

        Public Shared GetShippingRateByZoneKgFunc As Func(Of CommerceDataContext, Decimal, Integer, FixedShippingRate) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, WeightKG As Decimal, Zone As Integer) _
                                      (From r In db.FixedShippingRates Where r.WeightKGS >= WeightKG And r.Zone = Zone Select r Order By r.WeightKGS).FirstOrDefault)

        Public Shared GetShippingRateByZoneLbsFunc As Func(Of CommerceDataContext, Decimal, Integer, FixedShippingRate) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, WeightLBS As Decimal, Zone As Integer) _
                                      (From r In db.FixedShippingRates Where r.WeightLBS >= WeightLBS And r.Zone = Zone Select r Order By r.WeightLBS).FirstOrDefault)

        Public Shared Function GetRateFromZoneKg(ByVal Weight As Decimal, ByVal Zone As Integer) As FixedShippingRate
            Dim db As New CommerceDataContext

            Dim rate As FixedShippingRate = GetShippingRateByZoneKgFunc(db, Weight, Zone)

            If Not rate Is Nothing Then
                GetRateFromZoneKg = rate
            Else
                Throw New InvalidShippingRateException()
            End If

            db.Dispose()
        End Function

        Public Shared Function GetRateFromZoneLbs(ByVal Weight As Decimal, ByVal Zone As Integer) As FixedShippingRate
            Dim db As New CommerceDataContext

            Dim rate As FixedShippingRate = GetShippingRateByZoneLbsFunc(db, Weight, Zone)

            If Not rate Is Nothing Then
                GetRateFromZoneLbs = rate
            Else
                Throw New InvalidShippingRateException()
            End If

            db.Dispose()
        End Function

        Public Shared GetMaxShippingWeightKgFunc As Func(Of CommerceDataContext, Decimal) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      (From r In db.FixedShippingRates Select r.WeightKGS).Max)

        Public Shared GetMaxShippingWeightLbsFunc As Func(Of CommerceDataContext, Decimal) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      (From r In db.FixedShippingRates Select r.WeightLBS).Max)

        Public Shared Function GetMaxShippingWeightKg() As Decimal
            Dim db As New CommerceDataContext

            Dim Weight As Decimal = 0

            Weight = GetMaxShippingWeightKgFunc(db)

            GetMaxShippingWeightKg = Weight

            db.Dispose()
        End Function

        Public Shared Function GetMaxShippingWeightLbs() As Decimal
            Dim db As New CommerceDataContext

            Dim Weight As Decimal = 0

            Weight = GetMaxShippingWeightLbsFunc(db)

            GetMaxShippingWeightLbs = Weight

            db.Dispose()
        End Function

    End Class

End Namespace