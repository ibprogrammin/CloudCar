Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement

    Public Class SalesInquiryController

        Public Shared GetAllSalesInquiryFunc As Func(Of ContentDataContext, IQueryable(Of SalesInquiry)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) _
                                      From s In db.SalesInquiries Select s)

        Public Shared GetSalesInquiryFunc As Func(Of ContentDataContext, Integer, SalesInquiry) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, id As Integer) _
                                      (From s In db.SalesInquiries Where s.id = id Select s).SingleOrDefault())

        Public Shared GetSalesInquiryEmailFunc As Func(Of ContentDataContext, String, SalesInquiry) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, email As String) _
                                      (From s In db.SalesInquiries Where s.email Like email Select s).SingleOrDefault())

        Public Shared Function Create(ByVal Name As String, ByVal Email As String, ByVal Inquiry As String, ByVal DateSent As DateTime, ByVal Checked As Boolean) As Integer
            Dim db As New ContentDataContext

            Dim item As New SalesInquiry

            item.name = Name
            item.email = Email
            item.inquiry = inquiry
            item.checked = Checked
            item.datesent = DateSent

            db.SalesInquiries.InsertOnSubmit(item)
            db.SubmitChanges()

            Dim id As Integer = item.id

            item = Nothing
            db = Nothing

            Return id
        End Function

        Public Shared Function Create(ByVal si As SalesInquiry) As Integer
            Dim db As New ContentDataContext

            db.SalesInquiries.InsertOnSubmit(si)
            db.SubmitChanges()

            db = Nothing

            Return si.id
        End Function

        Public Shared Sub Update(ByVal ID As Integer, ByVal Name As String, ByVal Email As String, ByVal Inquiry As String, ByVal DateSent As DateTime, ByVal Checked As Boolean)
            Dim db As New ContentDataContext

            Dim item As SalesInquiry = GetSalesInquiryFunc(db, ID)

            If item Is Nothing Then
                Throw New InvalidSalesInquiryException()
            Else
                item.name = Name
                item.email = Email
                item.inquiry = Inquiry
                item.checked = Checked
                item.datesent = DateSent

                db.SubmitChanges()
            End If

            item = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim db As New ContentDataContext

                Dim item As SalesInquiry = GetSalesInquiryFunc(db, ID)

                db.SalesInquiries.DeleteOnSubmit(item)
                db.SubmitChanges()

                item = Nothing
                db = Nothing

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetElement() As System.Linq.IQueryable(Of SalesInquiry)
            Dim db As New ContentDataContext

            Dim items = GetAllSalesInquiryFunc(db)

            If items Is Nothing Then
                Throw New InvalidSalesInquiryException()
            Else
                GetElement = items
            End If

            items = Nothing
            db = Nothing
        End Function

        Public Shared Function GetElement(ByVal ID As Integer) As SalesInquiry
            Dim db As New ContentDataContext

            Dim item As SalesInquiry = GetSalesInquiryFunc(db, ID)

            If item Is Nothing Then
                Throw New InvalidSalesInquiryException()
            Else
                GetElement = item
            End If

            item = Nothing
            db = Nothing
        End Function

        Public Shared Function GetElement(ByVal Email As String) As SalesInquiry
            Dim db As New ContentDataContext

            Dim item As SalesInquiry = GetSalesInquiryEmailFunc(db, Email)

            If item Is Nothing Then
                Throw New InvalidSalesInquiryException()
            Else
                GetElement = item
            End If

            item = Nothing
            db = Nothing
        End Function

        Public Shared Sub SetChecked(ByVal ID As Integer)
            Dim db As New ContentDataContext

            Dim item As SalesInquiry = GetSalesInquiryFunc(db, ID)

            If item Is Nothing Then
                Throw New InvalidSalesInquiryException()
            Else
                item.checked = True

                db.SubmitChanges()
            End If

            item = Nothing
            db = Nothing
        End Sub

        Public Shared GetNewInquiriesFunc As Func(Of ContentDataContext, Integer) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) _
                                    (From si In db.SalesInquiries Where si.checked = False Select si).Count)

        Public Shared Function GetNewInquiriesCount() As Integer
            Dim db As New ContentDataContext

            Return GetNewInquiriesFunc(db)
        End Function

        Public Class InvalidSalesInquiryException
            Inherits Exception

            Public Sub New()
                MyBase.New("The sales inquiry you are looking for does not exist")
            End Sub

        End Class

    End Class

End Namespace