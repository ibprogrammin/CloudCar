Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement.CalendarModule

    Public Class TempMembershipController
        Inherits DataControllerClass

        Public Shared GetTempMembershipsFunc As Func(Of CommerceDataContext, IQueryable(Of TempMembership)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                             From tm In CurrentDataContext.TempMemberships Select tm)

        Public Shared GetTempMembershipByIdFunc As Func(Of CommerceDataContext, Guid, TempMembership) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, TempMembershipId As Guid) _
                                              (From tm In CurrentDataContext.TempMemberships Where tm.Id = TempMembershipId Select tm).FirstOrDefault)

        Public Overloads Function Create(ByVal DurationInMonths As Integer, ByVal Redeemed As Boolean) As Guid
            Dim Item As New TempMembership
            Dim ItemId As Guid

            Item.DurationInMonths = DurationInMonths
            Item.Redeemed = Redeemed

            db.TempMemberships.InsertOnSubmit(Item)
            db.SubmitChanges()

            ItemId = Item.Id

            Item = Nothing

            Return ItemId
        End Function

        Public Overloads Function Delete(ByVal Id As Guid) As Boolean
            Try
                Dim Item As TempMembership = GetTempMembershipByIdFunc(db, Id)

                db.TempMemberships.DeleteOnSubmit(Item)
                db.SubmitChanges()

                Return True
            Catch Ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal Id As Guid, ByVal DurationInMonths As Integer, ByVal Redeemed As Boolean) As Boolean
            Dim Item As TempMembership = GetTempMembershipByIdFunc(db, Id)

            If Item Is Nothing Then
                Throw New Exception("Temp Membership " & Id.ToString & " does not exist.")
            Else
                Item.DurationInMonths = DurationInMonths
                Item.Redeemed = Redeemed

                db.SubmitChanges()
            End If

            Item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal Id As Guid) As TempMembership
            Dim Item As TempMembership = GetTempMembershipByIdFunc(db, Id)

            If Item Is Nothing Then
                Throw New Exception("Temp membership with ID: " & Id.ToString & " does not exist.")
            Else
                Return Item
            End If

            Item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of TempMembership)
            Dim CurrentTempMemberships As List(Of TempMembership) = GetTempMembershipsFunc(db).ToList

            If CurrentTempMemberships Is Nothing Then
                Throw New Exception("There are no temp membership")
            Else
                Return CurrentTempMemberships
            End If
        End Function

    End Class

End Namespace