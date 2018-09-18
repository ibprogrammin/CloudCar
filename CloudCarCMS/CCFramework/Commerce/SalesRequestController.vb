Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.ShoppingCart

Namespace CCFramework.Commerce

    Public Class SalesRequestController
        Inherits DataControllerClass

        'Public Shared Function GetSalesRequestDistributor(ByVal RequestId As Integer)
        '    Dim db As New CommerceDataContext

        '    Dim CurrentSalesRequest = From sr In db.SalesRequests _
        '                              Join p In db.Products On sr.ProductId Equals p.ID _
        '                              Join a In db.Addresses On sr.CustomerAddressId Equals a.ID _
        '                              Where sr.Id = RequestId _
        '                              Select New With {.Id = sr.Id, .SalesRepId = sr.SalesRepId, .SalesRepName = sr.SalesRepName, _
        '                                     .Product = p.Name, .RegistrationNumber = sr.RegistrationNumber, .CustomerName = sr.CustomerName, _
        '                                     .Address = a.Address & ", " & a.City & ", " & a.Province.Name & ", " & a.Province.Country.Name & ", " & a.PCZIP}


        'End Function

        Public Shared GetSalesRequestsFunc As Func(Of CommerceDataContext, IQueryable(Of SalesRequest)) = _
            CompiledQuery.Compile(Function(StoreDataContext As CommerceDataContext) _
                                      From CurrentSalesRequest In StoreDataContext.SalesRequests Select CurrentSalesRequest)

        Public Shared GetSalesRequestByIdFunc As Func(Of CommerceDataContext, Integer, SalesRequest) = _
            CompiledQuery.Compile(Function(StoreDataContext As CommerceDataContext, Id As Integer) _
                                      (From CurrentSalesRequest In StoreDataContext.SalesRequests Where CurrentSalesRequest.Id = Id Select CurrentSalesRequest).FirstOrDefault)

        Public Shared GetSalesRequestByRequestCodeFunc As Func(Of CommerceDataContext, String, SalesRequest) = _
            CompiledQuery.Compile(Function(StoreDataContext As CommerceDataContext, Code As String) _
                                      (From CurrentSalesRequest In StoreDataContext.SalesRequests Where CurrentSalesRequest.RequestKey.ToString = Code Select CurrentSalesRequest).FirstOrDefault)

        Public Shared GetSalesRequestByShoppingCartItemIdFunc As Func(Of CommerceDataContext, Integer, SalesRequest) = _
            CompiledQuery.Compile(Function(StoreDataContext As CommerceDataContext, ShoppingCartItemId As Integer) _
                                      (From CurrentSalesRequest In StoreDataContext.SalesRequests Where CurrentSalesRequest.ShoppingCartId = ShoppingCartItemId Select CurrentSalesRequest).FirstOrDefault)

        Public Overloads Function Create(ByVal SalesRepName As String, ByVal SalesRepEmail As String, ByVal CustomerName As String, ByVal CustomerEmail As String, ByVal ProductId As Integer, ByVal Quantity As Integer) As Integer
            Dim NewSalesRequest As New SalesRequest
            Dim RequestId As Integer

            NewSalesRequest.SalesRepName = SalesRepName
            NewSalesRequest.SalesRepEmail = SalesRepEmail
            NewSalesRequest.CustomerName = CustomerName
            NewSalesRequest.CustomerEmail = CustomerEmail
            NewSalesRequest.ProductId = ProductId
            NewSalesRequest.Quantity = Quantity
            NewSalesRequest.Timestamp = DateTime.Now
            NewSalesRequest.RequestKey = Guid.NewGuid()

            db.SalesRequests.InsertOnSubmit(NewSalesRequest)
            db.SubmitChanges()

            RequestId = NewSalesRequest.Id

            NewSalesRequest = Nothing

            Return RequestId
        End Function

        Public Overloads Function Update(ByVal Id As Integer, ByVal SalesRepName As String, ByVal SalesRepEmail As String, ByVal CustomerName As String, ByVal CustomerEmail As String, ByVal ProductId As Integer, ByVal Quantity As Integer) As Boolean
            Dim CurrentSalesRequest As SalesRequest = GetSalesRequestByIdFunc(db, Id)

            If CurrentSalesRequest Is Nothing Then
                Throw New Exception("Sales Request with ID: " & Id.ToString & " does not exist.")
            Else
                CurrentSalesRequest.SalesRepName = SalesRepName
                CurrentSalesRequest.SalesRepEmail = SalesRepEmail
                CurrentSalesRequest.CustomerName = CustomerName
                CurrentSalesRequest.CustomerEmail = CustomerEmail
                CurrentSalesRequest.ProductId = ProductId
                CurrentSalesRequest.Quantity = Quantity
                CurrentSalesRequest.Timestamp = DateTime.Now
                CurrentSalesRequest.RequestKey = Guid.NewGuid()

                db.SubmitChanges()

                Return True
            End If
        End Function

        Public Overloads Function Delete(ByVal Id As Integer) As Boolean
            Try
                Dim CurrentSalesRequest As SalesRequest = GetSalesRequestByIdFunc(db, Id)

                db.SalesRequests.DeleteOnSubmit(CurrentSalesRequest)
                db.SubmitChanges()

                Return True
            Catch Ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function GetElement(ByVal Id As Integer) As SalesRequest
            Dim CurrentSalesRequest As SalesRequest = GetSalesRequestByIdFunc(db, Id)

            If CurrentSalesRequest Is Nothing Then
                Throw New Exception("Sales Request with ID: " & Id.ToString & " does not exist.")
            Else
                Return CurrentSalesRequest
            End If
        End Function

        Public Overloads Function GetElements() As List(Of SalesRequest)
            Dim SalesRequestList As List(Of SalesRequest) = GetSalesRequestsFunc(db).ToList

            If SalesRequestList Is Nothing And SalesRequestList.Count > 0 Then
                Throw New Exception("There are no Sales Requests")
            Else
                Return SalesRequestList
            End If
        End Function

        Public Shared Function GetRedemptionRequestByCode(ByVal Code As String) As SalesRequest
            Dim StoreDataContext As New CommerceDataContext

            Dim CurrentSalesRequest As SalesRequest = GetSalesRequestByRequestCodeFunc(StoreDataContext, Code)

            StoreDataContext.Dispose()

            Return CurrentSalesRequest
        End Function

        Public Shared Function GetRedemptionCodeByShoppingCartId(ByVal ShoppingCartId As Integer) As Guid
            Dim StoreDataContext As New CommerceDataContext

            Dim CurrentRedemptionCode As Guid = GetSalesRequestByShoppingCartItemIdFunc(StoreDataContext, ShoppingCartId).RequestKey

            StoreDataContext.Dispose()

            Return CurrentRedemptionCode
        End Function

        Public Shared Function SetRedemptionRequestShoppingCart(ByVal Code As String, ByVal ShoppingCartId As Integer) As Boolean
            Dim StoreDataContext As New CommerceDataContext

            Dim CurrentSalesRequest As SalesRequest = GetSalesRequestByRequestCodeFunc(StoreDataContext, Code)

            If Not CurrentSalesRequest Is Nothing Then
                Dim ShoppingCartItemDeleted As Boolean

                If CurrentSalesRequest.ShoppingCartId.HasValue And CurrentSalesRequest.ShoppingCartId > 0 Then
                    ShoppingCartItemDeleted = New ShoppingCartController().Delete(CurrentSalesRequest.ShoppingCartId)
                End If

                CurrentSalesRequest.ShoppingCartId = ShoppingCartId

                StoreDataContext.SubmitChanges()

                SetRedemptionRequestShoppingCart = True
            Else
                SetRedemptionRequestShoppingCart = False
            End If

            StoreDataContext.Dispose()
        End Function

        Public Shared Function SetRedemptionRequestRedeemed(ByVal Code As String, ByVal OrderId As Integer) As Boolean
            Dim StoreDataContext As New CommerceDataContext

            Dim CurrentSalesRequest As SalesRequest = GetSalesRequestByRequestCodeFunc(StoreDataContext, Code)

            If Not CurrentSalesRequest Is Nothing Then
                CurrentSalesRequest.OrderId = OrderId
                CurrentSalesRequest.Redeemed = True

                StoreDataContext.SubmitChanges()

                SetRedemptionRequestRedeemed = True
            Else
                SetRedemptionRequestRedeemed = False
            End If

            StoreDataContext.Dispose()
        End Function

        Public Shared Function CheckCodeRedeemed(ByVal Code As String) As Boolean
            Dim StoreDataContext As New CommerceDataContext

            Dim CurrentSalesRequest As SalesRequest = GetSalesRequestByRequestCodeFunc(StoreDataContext, Code)

            If Not CurrentSalesRequest Is Nothing Then
                If CurrentSalesRequest.Redeemed Or CurrentSalesRequest.OrderId.HasValue Then
                    CheckCodeRedeemed = True
                Else
                    CheckCodeRedeemed = False
                End If
            Else
                CheckCodeRedeemed = True
            End If

            StoreDataContext.Dispose()
        End Function

        Public Shared Function CheckCodeInUse(ByVal Code As String) As Boolean
            Dim StoreDataContext As New CommerceDataContext

            Dim CurrentSalesRequest As SalesRequest = GetSalesRequestByRequestCodeFunc(StoreDataContext, Code)

            If Not CurrentSalesRequest Is Nothing Then
                If CurrentSalesRequest.ShoppingCartId.HasValue Then
                    CheckCodeInUse = True
                Else
                    CheckCodeInUse = False
                End If
            Else
                CheckCodeInUse = True
            End If

            StoreDataContext.Dispose()
        End Function

        Public Shared Function CheckShoppingCartItemIsSalesRequest(ByVal ShoppingCartItemId As Integer) As Boolean
            Dim StoreDataContext As New CommerceDataContext

            Dim CurrentSalesRequest As SalesRequest = GetSalesRequestByShoppingCartItemIdFunc(StoreDataContext, ShoppingCartItemId)

            If Not CurrentSalesRequest Is Nothing Then
                CheckShoppingCartItemIsSalesRequest = True
            Else
                CheckShoppingCartItemIsSalesRequest = False
            End If

            StoreDataContext.Dispose()
        End Function

        Public Shared Sub SendCustomerRequestEmail(ByVal CustomerEmailAddress As String, ByVal CustomerName As String, ByVal RequestKey As Guid)
            Dim EmailSubject As String = "Here is Your Verification Code for Your Free Product from " & Core.Settings.HostName
            Dim EmailBody As New StringBuilder

            EmailBody.Append("<h1>Here is your free voucher</h1><br /><br />" & vbNewLine)
            EmailBody.Append("<p>Greetings " & CustomerName & ",</p><br /><br />" & vbNewLine)
            EmailBody.Append("<p>Click the following link to add the free product to your shopping cart:</p><br /><br />" & vbNewLine)
            EmailBody.Append("<a href=""http://" & Core.Settings.HostName & "/Store/Special/RedeemForm.aspx?RC=" & RequestKey.ToString & """>" & vbNewLine)
            EmailBody.Append("http://" & Core.Settings.HostName & "/Store/Special/RedeemForm.aspx?RC=" & RequestKey.ToString & "</a><br /><br />" & vbNewLine)
            EmailBody.Append("<p>If you already have an account with us, make sure you login first before submiting your request.</p>" & vbNewLine)
            EmailBody.Append("" & vbNewLine)

            SendEmails.Send(New Net.Mail.MailAddress(CustomerEmailAddress, CustomerName), EmailSubject, EmailBody.ToString)
        End Sub

        Public Shared Sub SendSalesRepThankYouEmail(ByVal SalesRepEmailAddress As String, ByVal SalesRepName As String)
            Dim EmailSubject As String = "Thank You from " & Core.Settings.HostName
            Dim EmailBody As New StringBuilder

            EmailBody.Append("<h1>Thank You for Submitting Your Request</h1><br /><br />" & vbNewLine)
            EmailBody.Append("<p>The customer will now recieve a notification in their email with instructions on how to redeem their free product.</p><br /><br />" & vbNewLine)
            EmailBody.Append("" & vbNewLine)

            SendEmails.Send(New Net.Mail.MailAddress(SalesRepEmailAddress, SalesRepName), EmailSubject, EmailBody.ToString)
        End Sub

    End Class

End Namespace
