Namespace CCFramework.Model

    Partial Public Class CommerceDataContext

        Private Sub OnCreated()
            CommandTimeout = 4000
        End Sub

    End Class

    Partial Public Class SimpleUser
        Implements IEquatable(Of SimpleUser)

        Public Overloads Function Equals(Other As SimpleUser) As Boolean Implements IEquatable(Of SimpleUser).Equals
            If Me Is Nothing AndAlso Other Is Nothing Then
                Return True
            ElseIf Me Is Nothing OrElse Other Is Nothing Then
                Return False
            ElseIf ID = Other.ID AndAlso FirstName = Other.FirstName AndAlso MiddleName = Other.MiddleName AndAlso LastName = Other.LastName AndAlso Email = Other.Email AndAlso PhoneNumber = Other.PhoneNumber Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overloads Shared Function Equals(SimpleUserA As SimpleUser, SimpleUserB As SimpleUser) As Boolean
            If SimpleUserA.ID = SimpleUserB.ID AndAlso SimpleUserA.FirstName = SimpleUserB.FirstName _
                AndAlso SimpleUserA.MiddleName = SimpleUserB.MiddleName _
                AndAlso SimpleUserA.LastName = SimpleUserB.LastName AndAlso SimpleUserA.Email = SimpleUserB.Email _
                AndAlso SimpleUserA.PhoneNumber = SimpleUserB.PhoneNumber Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function Equals(Other As Object) As Boolean
            If Other Is Nothing Then Return False

            Dim CurrentSimpleUserObject As SimpleUser = TryCast(Other, SimpleUser)
            If CurrentSimpleUserObject Is Nothing Then
                Return False
            Else
                Return Equals(CurrentSimpleUserObject)
            End If
        End Function

        Public Overloads Shared Function GetHashCode(CurrentSimpleUser As SimpleUser) As Integer
            Return (CurrentSimpleUser.ID & CurrentSimpleUser.FirstName & CurrentSimpleUser.MiddleName & CurrentSimpleUser.LastName & CurrentSimpleUser.Email & CurrentSimpleUser.PhoneNumber).GetHashCode
        End Function

        Public Overloads Function GetHashCode() As Integer
            Return (ID & FirstName & MiddleName & LastName & Email & PhoneNumber).GetHashCode
        End Function

        Public Shared Operator =(SimpleUserA As SimpleUser, SimpleUserB As SimpleUser) As Boolean
            If SimpleUserA.ID = SimpleUserB.ID AndAlso SimpleUserA.FirstName = SimpleUserB.FirstName _
                AndAlso SimpleUserA.MiddleName = SimpleUserB.MiddleName _
                AndAlso SimpleUserA.LastName = SimpleUserB.LastName _
                AndAlso SimpleUserA.Email = SimpleUserB.Email _
                AndAlso SimpleUserA.PhoneNumber = SimpleUserB.PhoneNumber Then
                Return True
            Else
                Return False
            End If
        End Operator

        Public Shared Operator <>(SimpleUserA As SimpleUser, SimpleUserB As SimpleUser) As Boolean
            If Not SimpleUserA.ID = SimpleUserB.ID _
                OrElse Not SimpleUserA.FirstName = SimpleUserB.FirstName _
                OrElse Not SimpleUserA.MiddleName = SimpleUserB.MiddleName _
                OrElse Not SimpleUserA.LastName = SimpleUserB.LastName _
                OrElse Not SimpleUserA.Email = SimpleUserB.Email _
                OrElse Not SimpleUserA.PhoneNumber = SimpleUserB.PhoneNumber Then
                Return True
            Else
                Return False
            End If
        End Operator

    End Class

End Namespace