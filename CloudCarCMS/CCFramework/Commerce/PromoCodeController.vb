Imports System.Data.Linq
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Commerce

    Public Class PromoCodeController
        Inherits DataControllerClass

        Public Shared GetPromoCodeFunc As Func(Of CommerceDataContext, IQueryable(Of PromoCode)) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext) From pc In db.PromoCodes Select pc)

        Public Shared GetPromoCodeByIDFunc As Func(Of CommerceDataContext, Integer, PromoCode) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, PromoCodeID As Integer) _
                                              (From pc In db.PromoCodes Where pc.id = PromoCodeID Select pc).FirstOrDefault)

        Public Shared GetPromoCodeByCodeFunc As Func(Of CommerceDataContext, String, PromoCode) = _
                    CompiledQuery.Compile(Function(db As CommerceDataContext, Code As String) _
                                              (From pc In db.PromoCodes Where pc.Code.ToLower = Code.ToLower Select pc).FirstOrDefault)

        Public Shared GetPromoCodeAvailableFunc As Func(Of CommerceDataContext, String, Boolean) = _
            CompiledQuery.Compile(Function(db As CommerceDataContext, Code As String) _
                                      If(Not (From pc In db.PromoCodes Where pc.Code.ToLower = Code.ToLower Select pc).FirstOrDefault Is Nothing, True, False))

        Public Overloads Function Create(ByVal Code As String, ByVal Discount As Decimal, ByVal FixedAmount As Boolean, ByVal SalesRep As String) As Integer
            Dim item As New PromoCode
            Dim itemId As Integer

            item.Code = Code
            item.Discount = Discount
            item.FixedAmount = FixedAmount
            item.SalesRep = SalesRep

            db.PromoCodes.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing

            Return itemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = GetPromoCodeByIDFunc(db, ID)

                db.PromoCodes.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal ID As Integer, ByVal Code As String, ByVal Discount As Decimal, ByVal FixedAmount As Boolean, ByVal SalesRep As String) As Boolean
            Dim item As PromoCode = GetPromoCodeByIDFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("Promo Code " & ID.ToString & " does not exist.")
            Else
                item.Code = Code
                item.Discount = Discount
                item.FixedAmount = FixedAmount
                item.SalesRep = SalesRep

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As PromoCode
            Dim item As PromoCode = GetPromoCodeByIDFunc(db, ID)

            If item Is Nothing Then
                Throw New Exception("Promo Code with ID: " & ID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Overloads Function GetElements() As Collections.Generic.List(Of PromoCode)
            Dim items = GetPromoCodeFunc(db)

            If items Is Nothing Then
                Throw New Exception("There are no Promo Codes")
            Else
                Return items.ToList
            End If
        End Function

        Public Shared Function GetPromoCodeDiscount(ByVal Code As String, ByVal Subtotal As Decimal) As Decimal
            Dim db As New CommerceDataContext

            Dim pc As PromoCode = PromoCodeController.GetPromoCodeByCodeFunc(db, Code)

            Dim discount As Decimal = 0

            If Not pc Is Nothing Then
                If pc.FixedAmount Then
                    discount = pc.Discount
                Else
                    discount = CDec(Math.Round((Subtotal * (pc.Discount * 0.01)), 2))
                End If
            End If

            Return discount
        End Function

    End Class

End Namespace