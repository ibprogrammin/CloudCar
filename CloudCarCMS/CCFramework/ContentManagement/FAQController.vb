Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement

    Public Class FAQController

        Public Shared Function Create(ByVal Question As String, ByVal Answer As String, ByVal Order As Integer) As Integer
            Dim db As New ContentDataContext
            Dim faq As New Faq
            Dim faqId As Integer

            faq.Question = Question
            faq.Answer = Answer
            faq.OrderNumber = Order

            db.Faqs.InsertOnSubmit(faq)
            db.SubmitChanges()

            faqId = faq.FaqID

            faq = Nothing
            db = Nothing

            Return faqId
        End Function

        Public Shared Sub Update(ByVal FaqID As Integer, ByVal Question As String, ByVal Answer As String, ByVal Order As Integer)
            Dim db As New ContentDataContext
            Dim faq As Faq

            faq = (From f In db.Faqs Where f.FaqID = FaqID).SingleOrDefault

            If faq Is Nothing Then
                Throw New Exception("FAQ does not exist")
            Else
                faq.Question = Question
                faq.Answer = Answer
                faq.OrderNumber = Order

                db.SubmitChanges()
            End If

            faq = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal FaqId As Integer) As Boolean
            Try
                Dim db As New ContentDataContext

                Dim faq = (From f In db.Faqs Where f.FaqID = FaqId Select f).First

                db.Faqs.DeleteOnSubmit(faq)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetElement(ByVal Id As Integer) As Faq
            Dim db As New ContentDataContext
            Dim faq As Faq

            faq = (From f In db.Faqs Where f.FaqID = Id).SingleOrDefault

            If faq Is Nothing Then
                Throw New Exception("FAQ does not exist")
            Else
                GetElement = faq
            End If

            db = Nothing
        End Function

        Public Shared Function GetElements() As List(Of Faq)
            Dim db As New ContentDataContext
            Dim faqCol As New List(Of Faq)

            Dim faqs = From f In db.Faqs Select f Order By f.OrderNumber Descending

            If faqs Is Nothing And faqs.Count > 0 Then
                Throw New Exception("FAQ does not exist")
            Else
                For Each f As Faq In faqs
                    faqCol.Add(f)
                Next

                GetElements = faqCol
            End If

            db = Nothing
        End Function

    End Class

End Namespace