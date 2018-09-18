Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Commerce.PaymentProcessing

    Public Class CreditCardPaymentController
        Inherits DataControllerClass

        Public Shared GetCreditCardPaymentByIdFunc As Func(Of CommerceDataContext, Integer, CreditCardPayment) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, ID As Integer) _
                                      (From c In db.CreditCardPayments Where c.ID = ID Select c).FirstOrDefault)

        Public Shared GetCreditCardPaymentsFunc As Func(Of CommerceDataContext, IQueryable(Of CreditCardPayment)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      From c In db.CreditCardPayments Select c)

        Public Shared GetCreditCardPaymentByOrderIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of CreditCardPayment)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, OrderID As Integer) _
                                      From c In db.CreditCardPayments Where c.OrderID = OrderID Select c)

        Public Overloads Function Create(ByVal OrderID As Integer, ByVal CCNumber As String, ByVal CCTypeID As Integer, ByVal CCExpiration As Date, ByVal CCValidationNum As String, ByVal PCZIP As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal LastName As String, ByVal ChargeAmount As Decimal, ByVal EncryptionKey As String) As Integer
            Dim item As New CreditCardPayment
            Dim itemId As Integer

            item.CCNumber = CCNumber
            item.CCTypeID = CCTypeID
            item.CCExpiration = CCExpiration
            item.CCValidationNum = CCValidationNum
            item.CCPCZIP = PCZIP
            item.FirstName = FirstName
            item.LastName = LastName
            item.MiddleName = MiddleName
            item.OrderID = OrderID
            item.Charge = ChargeAmount
            item.CCEncryptionKey = EncryptionKey

            db.CreditCardPayments.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = GetCreditCardPaymentByIDFunc(db, ID) ' (From i In db.CreditCardPayments Where i.ID = ID Select i).First

                db.CreditCardPayments.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal OrderID As Integer, ByVal CCNumber As String, ByVal CCTypeID As Integer, ByVal CCExpiration As Date, ByVal CCValidationNum As String, ByVal PCZIP As String, ByVal FirstName As String, ByVal MiddleName As String, ByVal LastName As String, ByVal ChargeAmount As Decimal, ByVal EncryptionKey As String) As Boolean
            Dim item As CreditCardPayment

            item = GetCreditCardPaymentByIDFunc(db, ID) ' (From i In db.CreditCardPayments Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("CC Payment with ID: " & ID.ToString & " does not exist.")
            Else
                item.CCNumber = CCNumber
                item.CCTypeID = CCTypeID
                item.CCExpiration = CCExpiration
                item.CCValidationNum = CCValidationNum
                item.CCPCZIP = PCZIP
                item.FirstName = FirstName
                item.LastName = LastName
                item.MiddleName = MiddleName
                item.OrderID = OrderID
                item.Charge = ChargeAmount
                item.CCEncryptionKey = EncryptionKey

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As CreditCardPayment
            Dim item As CreditCardPayment

            item = GetCreditCardPaymentByIDFunc(db, ID) ' (From i In db.CreditCardPayments Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("CC Payment with ID: " & ID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of CreditCardPayment)
            Dim itemList As New List(Of CreditCardPayment)

            Dim items = GetCreditCardPaymentsFunc(db).ToList ' From c In db.CreditCardPayments Select c

            If items Is Nothing And items.Count > 0 Then
                Throw New Exception("There are no CC Payments")
            Else
                For Each e As CreditCardPayment In items
                    itemList.Add(e)
                Next

                Return itemList
            End If
        End Function

        Public Function SetPaymentAuthorized(ByVal CCPaymentID As Integer, ByVal AuthorizationCode As String, ByVal Response As String, ByVal ChargeAmount As Decimal) As Boolean
            Dim item As CreditCardPayment

            item = GetCreditCardPaymentByIDFunc(db, CCPaymentID) ' (From i In db.CreditCardPayments Where i.ID = CCPaymentID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("CC Payment with ID: " & CCPaymentID.ToString & " does not exist.")
            Else
                item.AuthCode = AuthorizationCode
                item.CCResponse = Response

                If Not item.Charge = ChargeAmount Then
                    item.Charge = ChargeAmount
                End If

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Function GetOrderPayment(ByVal OrderID As Integer) As CreditCardPayment
            Dim item As CreditCardPayment

            item = GetCreditCardPaymentByOrderIDFunc(db, OrderID).SingleOrDefault ' (From i In db.CreditCardPayments Where i.OrderID = OrderID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("CC Payment for Order: " & OrderID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

    End Class

    Public Class CreditCardTypeController
        Inherits DataControllerClass

        Public Shared GetCreditCardTypeByIDFunc As Func(Of CommerceDataContext, Integer, CreditCardType) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, ID As Integer) _
                                      (From c In db.CreditCardTypes Where c.ID = ID Select c).FirstOrDefault)

        Public Shared GetCreditCardTypesFunc As Func(Of CommerceDataContext, IQueryable(Of CreditCardType)) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext) _
                                      From c In db.CreditCardTypes Select c)

        Public Overloads Function Create(ByVal Type As String) As Integer
            Dim item As New CreditCardType
            Dim itemId As Integer

            item.Type = Type

            db.CreditCardTypes.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = GetCreditCardTypeByIDFunc(db, ID) ' (From i In db.CreditCardTypes Where i.ID = ID Select i).First

                db.CreditCardTypes.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal Type As String) As Boolean
            Dim item As CreditCardType

            item = GetCreditCardTypeByIDFunc(db, ID) ' (From i In db.CreditCardTypes Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("CC Type with ID: " & ID.ToString & " does not exist.")
            Else
                item.Type = Type

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As CreditCardType
            Dim item As CreditCardType

            item = GetCreditCardTypeByIDFunc(db, ID) ' (From i In db.CreditCardTypes Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("CC Type with ID: " & ID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of CreditCardType)
            Dim itemList As New List(Of CreditCardType)

            Dim items = GetCreditCardTypesFunc(db).ToList ' From c In db.CreditCardTypes Select c

            If items Is Nothing And items.Count > 0 Then
                Throw New Exception("There are no CC Types")
            Else
                For Each e As CreditCardType In items
                    itemList.Add(e)
                Next

                Return itemList
            End If
        End Function

    End Class

End Namespace
