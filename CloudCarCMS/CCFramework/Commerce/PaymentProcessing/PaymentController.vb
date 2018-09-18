Imports System.Data.Linq
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCFramework.Commerce.PaymentProcessing

    Public Class PaymentController
        Inherits DataControllerClass

        Public Shared GetPaymentFunc As Func(Of CommerceDataContext, IQueryable(Of BeanStreamPayment)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                              From b In CurrentDataContext.BeanStreamPayments Select b)

        Public Shared GetPaymentByIdFunc As Func(Of CommerceDataContext, Integer, BeanStreamPayment) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, Id As Integer) _
                                              (From b In CurrentDataContext.BeanStreamPayments _
                                               Where b.ID = Id Select b).FirstOrDefault)

        Public Shared GetPaymentByOrderIdFunc As Func(Of CommerceDataContext, Integer, BeanStreamPayment) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, OrderId As Integer) _
                                              (From b In CurrentDataContext.BeanStreamPayments _
                                               Where b.OrderID = OrderId Select b).FirstOrDefault)

        Public Overloads Function Create(ByVal OrderId As Integer, ByVal CustomerName As String, ByVal ExpMonth As String, ByVal ExpYear As String, ByVal CCNumber As String, ByVal CCEncryptionKey As String, ByVal CCValidationNum As String, ByVal ChargeAmount As Decimal, ByVal TransactionState As PaymentStatus, ByVal MessageID As Integer, ByVal MessageText As String) As Integer
            Dim item As New BeanStreamPayment
            Dim itemId As Integer

            If Settings.StoreCreditCards Then
                item.CCNumber = CCNumber
            Else
                Dim ec As New EasyCryptography
                Dim message As String = ec.Encrypt("***NOT STORED***", CCEncryptionKey)

                item.CCNumber = message
            End If


            If ExpMonth.Length > 2 Then
                item.CCExpMonth = ExpMonth.Substring(0, 2)
            Else
                item.CCExpMonth = ExpMonth
            End If

            If ExpYear.Length > 2 Then
                item.CCExpYear = ExpYear.Substring(0, 2)
            Else
                item.CCExpYear = ExpYear
            End If

            If CCValidationNum.Length > 4 Then
                item.CCValidationNum = CCValidationNum.Substring(0, 4)
            Else
                item.CCValidationNum = CCValidationNum
            End If

            If CustomerName.Length > 64 Then
                item.CCOwner = CustomerName.Substring(0, 64)
            Else
                item.CCOwner = CustomerName
            End If

            item.OrderID = OrderId
            item.Charge = ChargeAmount
            item.TransactionState = TransactionState
            item.CCEncryptionKey = CCEncryptionKey
            item.MessageID = MessageID
            item.MessageText = MessageText

            db.BeanStreamPayments.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = GetPaymentByIdFunc(db, ID)

                db.BeanStreamPayments.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal OrderID As Integer, ByVal CustomerName As String, ByVal ExpMonth As String, ByVal ExpYear As String, ByVal CCNumber As String, ByVal CCEncryptionKey As String, ByVal CCValidationNum As String, ByVal ChargeAmount As Decimal, ByVal TransactionState As PaymentStatus, ByVal MessageID As Integer, ByVal MessageText As String) As Boolean
            Dim item As BeanStreamPayment = GetPaymentByIdFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("Payment with ID: " & ID.ToString & " does not exist.")
            Else

                If Core.Settings.StoreCreditCards Then
                    item.CCNumber = CCNumber
                Else
                    Dim ec As New EasyCryptography
                    Dim message As String = ec.Encrypt("***NOT STORED***", CCEncryptionKey)

                    item.CCNumber = message
                End If

                If ExpMonth.Length > 2 Then
                    item.CCExpMonth = ExpMonth.Substring(0, 2)
                Else
                    item.CCExpMonth = ExpMonth
                End If

                If ExpYear.Length > 2 Then
                    item.CCExpYear = ExpYear.Substring(0, 2)
                Else
                    item.CCExpYear = ExpYear
                End If

                If CCValidationNum.Length > 4 Then
                    item.CCValidationNum = CCValidationNum.Substring(0, 4)
                Else
                    item.CCValidationNum = CCValidationNum
                End If

                If CustomerName.Length > 64 Then
                    item.CCOwner = CustomerName.Substring(0, 64)
                Else
                    item.CCOwner = CustomerName
                End If

                item.OrderID = OrderID
                item.Charge = ChargeAmount
                item.TransactionState = TransactionState
                item.CCEncryptionKey = CCEncryptionKey
                item.MessageID = MessageID
                item.MessageText = MessageText

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As BeanStreamPayment
            Dim item As BeanStreamPayment = GetPaymentByIdFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("BeanStream Payment " & ID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElements() As Collections.Generic.List(Of BeanStreamPayment)
            Dim items = GetPaymentFunc(db)

            If items Is Nothing Then
                Throw New Exception("There are no BeanStream Payments")
            Else
                Return items.ToList
            End If
        End Function

        Public Function SetPaymentAuthorized(ByVal PaymentId As Integer, ByVal AuthorizationCode As String, ByVal ProcessDate As String, ByVal TransactionId As Integer, ByVal TransactionState As PaymentStatus, ByVal ChargeAmount As Decimal) As Boolean
            Dim item As BeanStreamPayment = GetPaymentByIdFunc(db, PaymentId)

            If item Is Nothing Then
                Throw New Exception("BeanStream Payment " & PaymentId.ToString & " does not exist.")
            Else
                item.AuthCode = AuthorizationCode
                item.ProcessDate = Date.Now 'Date.Parse(ProcessDate)
                item.TransactionID = TransactionId
                item.TransactionState = TransactionState


                If Not item.Charge = ChargeAmount Then
                    item.Charge = ChargeAmount
                End If

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Function GetOrderPayment(ByVal OrderID As Integer) As BeanStreamPayment
            Dim item As BeanStreamPayment = GetPaymentByOrderIdFunc(db, OrderID)

            If item Is Nothing Then
                Throw New Exception("BeanStream payment for Order " & OrderID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Shared Function AuthorizeVBV(ByVal AuthCode As String, ByVal OrderID As Integer, ByVal TransactionID As Integer, ByVal TransactionState As Integer) As Boolean
            Try
                Dim db As New CommerceDataContext

                Dim payment As BeanStreamPayment = GetPaymentByOrderIdFunc(db, OrderID) ' (From bsp In db.BeanStreamPayments Where bsp.OrderID = OrderID).First

                payment.AuthCode = AuthCode
                payment.ProcessDate = Date.Now 'Date.Parse(ProcessDate)
                payment.TransactionID = TransactionID
                payment.TransactionState = TransactionState

                db.SubmitChanges()

                payment = Nothing
                db.Dispose()
            Catch e As Exception
                SendEmails.Send(New System.Net.Mail.MailAddress(""), "Authorize Error", e.ToString)
            End Try

            Return True
        End Function

    End Class

End Namespace