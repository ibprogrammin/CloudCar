Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement.CalendarModule

    Public Class UserMembershipController
        Inherits DataControllerClass

        Public Shared GetUserMembershipsFunc As Func(Of CommerceDataContext, IQueryable(Of UserMembership)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                             From um In CurrentDataContext.UserMemberships Select um)

        Public Shared GetUserMembershipByIdFunc As Func(Of CommerceDataContext, Integer, UserMembership) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, UserMembershipId As Integer) _
                                              (From um In CurrentDataContext.UserMemberships Where um.Id = UserMembershipId Select um).FirstOrDefault)

        Public Overloads Function Create(ByVal UserId As Integer, ByVal StartDate As DateTime, ByVal ExpiryDate As DateTime, ByVal ProductId As Integer) As Integer
            Dim Item As New UserMembership
            Dim ItemId As Integer

            Item.UserId = UserId
            Item.StartDate = StartDate
            Item.ExpiryDate = ExpiryDate
            Item.ProductId = ProductId

            db.UserMemberships.InsertOnSubmit(Item)
            db.SubmitChanges()

            ItemId = Item.Id

            Item = Nothing

            Return ItemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim Item As UserMembership = GetUserMembershipByIdFunc(db, ID)

                db.UserMemberships.DeleteOnSubmit(Item)
                db.SubmitChanges()

                Return True
            Catch Ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal Id As Integer, ByVal UserId As Integer, ByVal StartDate As DateTime, ByVal ExpiryDate As DateTime, ByVal ProductId As Integer) As Boolean
            Dim Item As UserMembership = GetUserMembershipByIdFunc(db, Id)

            If Item Is Nothing Then
                Throw New Exception("User membership " & Id.ToString & " does not exist.")
            Else
                Item.UserId = UserId
                Item.StartDate = StartDate
                Item.ExpiryDate = ExpiryDate
                Item.ProductId = ProductId

                db.SubmitChanges()
            End If

            Item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal Id As Integer) As UserMembership
            Dim Item As UserMembership = GetUserMembershipByIdFunc(db, Id)

            If Item Is Nothing Then
                Throw New Exception("User membership with ID: " & Id.ToString & " does not exist.")
            Else
                Return Item
            End If

            Item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of UserMembership)
            Dim CurrentUserMemberships As List(Of UserMembership) = GetUserMembershipsFunc(db).ToList

            If CurrentUserMemberships Is Nothing Then
                Throw New Exception("There are no user membership")
            Else
                Return CurrentUserMemberships
            End If
        End Function

    End Class

End Namespace