Imports System.Data.Linq
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Core

    Public Class SimpleUserController
        Inherits DataControllerClass

        Public Shared GetSimpleUserFunc As Func(Of CommerceDataContext, IQueryable(Of SimpleUser)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext) From u In db.SimpleUsers Select u)

        Public Shared GetSimpleUserByIdFunc As Func(Of CommerceDataContext, Integer, SimpleUser) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, UserId As Integer) _
                                              (From u In db.SimpleUsers Where u.ID = UserId Select u).FirstOrDefault)

        Public Overloads Function Create(ByVal FirstName As String, ByVal MiddleName As String, ByVal LastName As String, ByVal Email As String, ByVal PhoneNumber As String) As Integer
            Dim item As New SimpleUser
            Dim itemId As Integer

            item.FirstName = FirstName
            item.MiddleName = MiddleName
            item.LastName = LastName
            item.Email = Email
            item.PhoneNumber = PhoneNumber

            db.SimpleUsers.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = GetSimpleUserByIdFunc(db, ID)

                db.SimpleUsers.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal FirstName As String, ByVal MiddleName As String, ByVal LastName As String, ByVal Email As String, ByVal PhoneNumber As String) As Boolean
            Dim item As SimpleUser = GetSimpleUserByIdFunc(db, ID)

            If item Is Nothing Then
                Throw New InvalidUserException("User " & ID.ToString & " does not exist.")
            Else
                item.FirstName = FirstName
                item.MiddleName = MiddleName
                item.LastName = LastName
                item.Email = Email
                item.PhoneNumber = PhoneNumber

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As SimpleUser
            Dim item As SimpleUser = GetSimpleUserByIdFunc(db, ID)

            If item Is Nothing Then
                Throw New InvalidUserException("User " & ID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of SimpleUser)
            Dim items = GetSimpleUserFunc(db)

            If items Is Nothing Then
                Throw New InvalidUserException("There are no Users")
            Else
                Return items.ToList
            End If
        End Function

        Public Overloads Function GetDistinctCustomers() As List(Of SimpleUser)
            Dim DistinctUserList As List(Of SimpleUser) = Nothing

            Dim CurrentUsers As List(Of SimpleUser) = GetSimpleUserFunc(db).OrderBy(Function(f) _
                                                            f.LastName & ", " & f.FirstName).ToList

            If Not CurrentUsers Is Nothing Then
                DistinctUserList = (From f In CurrentUsers Select New SimpleUser With { _
                .ID = 0, .FirstName = f.FirstName, .MiddleName = f.MiddleName, .LastName = f.LastName, _
                .Email = f.Email, .PhoneNumber = f.PhoneNumber} Distinct).Distinct().ToList()
            End If

            If DistinctUserList Is Nothing Then
                Throw New InvalidUserException("There are no Users")
            Else
                GetDistinctCustomers = DistinctUserList
            End If
        End Function

        Public Shared Function GetUserIdByUserName(ByVal UserName As String) As Integer
            Dim CurrentDataContext As New CommerceDataContext

            Dim item As Integer = RegisteredUserController.GetUserIdByUserNameFunc(CurrentDataContext, UserName)

            If item = Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidUserException("User " & UserName & " does not exist.")
            Else
                GetUserIdByUserName = item

                CurrentDataContext.Dispose()
            End If
        End Function

    End Class

    Public Class RegisteredUserController
        Inherits DataControllerClass

        Public Shared GetRegisteredUserFunc As Func(Of CommerceDataContext, IQueryable(Of RegisteredUser)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext) From u In db.RegisteredUsers Select u)

        Public Shared GetRegisteredUserByIdFunc As Func(Of CommerceDataContext, Integer, RegisteredUser) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, Id As Integer) _
                                              (From u In db.RegisteredUsers Where u.ID = Id Select u).FirstOrDefault)

        Public Shared GetRegisteredUserByUserIdFunc As Func(Of CommerceDataContext, Integer, RegisteredUser) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, UserId As Integer) _
                                              (From u In db.RegisteredUsers Where u.UserID = UserId Select u).FirstOrDefault)

        Public Shared GetRegisteredUserByUserNameFunc As Func(Of CommerceDataContext, String, RegisteredUser) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, UserName As String) _
                                              (From u In db.RegisteredUsers Where u.UserName Like UserName Select u).FirstOrDefault)

        Public Shared GetUserIdByUserNameFunc As Func(Of CommerceDataContext, String, Integer) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, UserName As String) _
                                              (From u In db.RegisteredUsers Where u.UserName Like UserName Select u).FirstOrDefault.UserID)

        Public Overloads Function Create(ByVal UserID As Integer, ByVal AddressID As Integer, ByVal BirthDate As DateTime, ByVal Notes As String, ByVal Allergies As String, ByVal UserName As String, ByVal PriceLevel As EPriceLevel) As Integer
            Dim item As New RegisteredUser
            Dim itemId As Integer

            item.UserID = UserID
            item.AddressID = AddressID
            item.UserName = UserName
            item.BirthDate = BirthDate
            item.Allergies = Allergies
            item.Notes = Notes
            item.PriceLevel = PriceLevel

            db.RegisteredUsers.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = GetRegisteredUserByIdFunc(db, ID)

                db.RegisteredUsers.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal UserID As Integer, ByVal AddressID As Integer, ByVal BirthDate As DateTime, ByVal Notes As String, ByVal Allergies As String, ByVal UserName As String, ByVal PriceLevel As EPriceLevel) As Boolean
            Dim item As RegisteredUser = GetRegisteredUserByIdFunc(db, ID)

            If item Is Nothing Then
                Throw New InvalidUserException("User with ID: " & ID.ToString & " does not exist.")
            Else
                item.UserID = UserID
                item.AddressID = AddressID
                item.UserName = UserName
                item.BirthDate = BirthDate
                item.Allergies = Allergies
                item.Notes = Notes
                item.PriceLevel = PriceLevel

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As RegisteredUser
            Dim item As RegisteredUser = GetRegisteredUserByIdFunc(db, ID)

            If item Is Nothing Then
                Throw New InvalidUserException("User " & ID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetByUserID(ByVal UserID As Integer) As RegisteredUser
            Dim item As RegisteredUser = GetRegisteredUserByUserIdFunc(db, UserID) ' (From i In db.RegisteredUsers Where i.UserID = UserID).SingleOrDefault

            If item Is Nothing Then
                Throw New InvalidUserException("User " & UserID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Shared Function GetUserNameById(ByVal UserID As Integer) As String
            Dim CurrentDataContext As New CommerceDataContext

            Dim item As RegisteredUser = GetRegisteredUserByUserIdFunc(CurrentDataContext, UserID)

            If item Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidUserException("User " & UserID.ToString & " does not exist.")
            Else
                GetUserNameById = item.UserName

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Overloads Function GetByUserName(ByVal UserName As String) As RegisteredUser
            Dim item As RegisteredUser = GetRegisteredUserByUserNameFunc(db, UserName) ' (From i In db.RegisteredUsers Where i.UserName = UserName).SingleOrDefault

            If item Is Nothing Then
                Throw New InvalidUserException("User " & UserName & " does not exist.")
            Else
                GetByUserName = item
            End If
        End Function

        'TODO Change Membership to Recurring Billing Item
        Public Shared Function UserHasMembership(ByVal UserName As String) As Boolean
            'Dim CurrentDataContext As New CommerceDataContext

            'Dim cm = (From m In CurrentDataContext.CurrentMemberships _
            '            Join r In CurrentDataContext.RegisteredUsers On r.UserID Equals m.UserID _
            '            Where r.UserName = UserName Select m).SingleOrDefault

            'If Not cm Is Nothing Then
            '    UserHasMembership = True
            'Else
            '    UserHasMembership = False
            'End If

            'CurrentDataContext.Dispose()

            UserHasMembership = False
        End Function

        Public Overloads Function GetElements() As Collections.Generic.List(Of RegisteredUser)
            Dim items = GetRegisteredUserFunc(db)

            If items Is Nothing Then
                Throw New InvalidUserException("User is not Registered")
            Else
                Return items.ToList
            End If
        End Function

        Public Shared Sub UpdateNotes(ByVal UserName As String, ByVal Notes As String)
            Dim CurrentDataContext As New CommerceDataContext

            Dim user As RegisteredUser = GetRegisteredUserByUserNameFunc(CurrentDataContext, UserName)

            If user Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidUserException("User does not exist")
            Else
                user.Notes = Notes

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If
        End Sub

        Public Shared Function GetNotes(ByVal username As String) As String
            Dim CurrentDataContext As New CommerceDataContext

            Dim user As RegisteredUser = GetRegisteredUserByUserNameFunc(CurrentDataContext, username)

            If user Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidUserException("User profile does not exist")
            Else
                GetNotes = user.Notes

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function ResetUserPassword(ByVal username As String, ByRef password As String) As Boolean
            Try
                Dim newpassword As String = Membership.GetUser(username).ResetPassword

                'TODO: Create an email template and make an automated message to the users email address.
                'Dim mailClient As New System.Net.Mail.SmtpClient
                'mailClient.Host = ECommerce.GlobalVariables.GetSMTPHost
                'mailClient.Credentials = New System.Net.NetworkCredential(ECommerce.GlobalVariables.GetSMTPUser, ECommerce.GlobalVariables.GetSMTPPass)
                'mailClient.Port = ECommerce.GlobalVariables.GetSMTPPort
                'mailClient.Send("passwordreset@" & ECommerce.GlobalVariables.DomainName, Membership.GetUser(e.CommandArgument.ToString).Email, Resources.Resource.YourPasswordHasBeenReset, Resources.Resource.YourPasswordHasBeenResetTo.Replace("[PASSWORD]", password))
                'mailClient = Nothing

                password = newpassword
                Return True
            Catch ex As Exception
                password = ex.Message
                Return False
            End Try

        End Function

        Public Shared Function ResetUserPassword(ByVal username As String, ByRef password As String, ByVal answer As String) As Boolean
            Try
                Dim newpassword As String = Membership.GetUser(username).ResetPassword(answer)

                'TODO: Create an email template and make an automated message to the users email address.
                'Dim mailClient As New System.Net.Mail.SmtpClient
                'mailClient.Host = ECommerce.GlobalVariables.GetSMTPHost
                'mailClient.Credentials = New System.Net.NetworkCredential(ECommerce.GlobalVariables.GetSMTPUser, ECommerce.GlobalVariables.GetSMTPPass)
                'mailClient.Port = ECommerce.GlobalVariables.GetSMTPPort
                'mailClient.Send("passwordreset@" & ECommerce.GlobalVariables.DomainName, Membership.GetUser(e.CommandArgument.ToString).Email, Resources.Resource.YourPasswordHasBeenReset, Resources.Resource.YourPasswordHasBeenResetTo.Replace("[PASSWORD]", password))
                'mailClient = Nothing

                password = newpassword
                Return True
            Catch ex As Exception
                password = ex.Message
                Return False
            End Try
        End Function

        Public Shared Function DeleteUserProfile(ByVal username As String, ByRef Message As String) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Try
                Dim user As RegisteredUser = GetRegisteredUserByUserNameFunc(CurrentDataContext, username)

                If Not user Is Nothing Then
                    Dim simpleUser = SimpleUserController.GetSimpleUserByIdFunc(CurrentDataContext, user.UserID)

                    If Not simpleUser Is Nothing Then
                        CurrentDataContext.RegisteredUsers.DeleteOnSubmit(user)
                        CurrentDataContext.SimpleUsers.DeleteOnSubmit(simpleUser)

                        CurrentDataContext.SubmitChanges()

                        Message = "User successfully deleted!"

                        DeleteUserProfile = Membership.DeleteUser(username)
                    Else
                        CurrentDataContext.RegisteredUsers.DeleteOnSubmit(user)

                        CurrentDataContext.SubmitChanges()

                        Message = "User <b>" & username & "</b> does not have a base user profile associated! The membership and registered profile have been deleted."

                        DeleteUserProfile = Membership.DeleteUser(username)
                    End If
                Else
                    Message = "User <b>" & username & "</b> does not have a registered user profile associated! The membership profile has been deleted."

                    DeleteUserProfile = Membership.DeleteUser(username)
                End If
            Catch CurrentException As Exception
                Message = CurrentException.Message

                DeleteUserProfile = False
            Finally
                CurrentDataContext.Dispose()
            End Try
        End Function

        Public Shared Function IsUserRegistered(ByVal UserId As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentRegisteredUser As RegisteredUser = GetRegisteredUserByUserIdFunc(CurrentDataContext, UserId)

            If Not CurrentRegisteredUser Is Nothing Then
                IsUserRegistered = True
            Else
                IsUserRegistered = False
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetUserPriceLevel(ByVal UserName As String) As EPriceLevel
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentRegisteredUser As RegisteredUser = GetRegisteredUserByUserNameFunc(CurrentDataContext, UserName)

            If CurrentRegisteredUser Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidUserException("User profile does not exist")
            Else
                GetUserPriceLevel = CType(CurrentRegisteredUser.PriceLevel, EPriceLevel)

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetUserPriceLevel(ByVal UserId As Integer) As EPriceLevel
            Dim CurrentDataContext As New CommerceDataContext

            Dim user As RegisteredUser = GetRegisteredUserByUserIdFunc(CurrentDataContext, UserId)

            If user Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidUserException("User profile does not exist")
            Else
                GetUserPriceLevel = CType(user.PriceLevel, EPriceLevel)

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function SetUserPriceLevel(ByVal username As String, ByVal level As EPriceLevel) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim item As RegisteredUser = GetRegisteredUserByUserNameFunc(CurrentDataContext, username)

            If item Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidUserException("User " & username & " does not exist.")
            Else
                item.PriceLevel = level

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()

                SetUserPriceLevel = True
            End If
        End Function

        Public Shared Sub SetUserMembershipStatus(ByVal UserName As String, ByVal MembershipEnabled As Boolean)
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentRegisteredUser As RegisteredUser = GetRegisteredUserByUserNameFunc(CurrentDataContext, UserName)

            If CurrentRegisteredUser Is Nothing Then
                CurrentDataContext.Dispose()

                'Throw New InvalidUserException("User " & UserName & " does not exist.")
            Else
                CurrentRegisteredUser.MembershipStatus = MembershipEnabled

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If
        End Sub

        Public Shared Function GetUserMembershipStatus(ByVal UserName As String) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentRegisteredUser As RegisteredUser = GetRegisteredUserByUserNameFunc(CurrentDataContext, UserName)

            If CurrentRegisteredUser Is Nothing Then
                CurrentDataContext.Dispose()

                GetUserMembershipStatus = Nothing
                'Throw New InvalidUserException("User profile does not exist")
            Else
                GetUserMembershipStatus = CurrentRegisteredUser.MembershipStatus

                CurrentDataContext.Dispose()
            End If
        End Function

    End Class

    Public Class InvalidUserException
        Inherits Exception

        Public Sub New(ByVal Message As String)
            MyBase.New(Message)
        End Sub

    End Class

End Namespace