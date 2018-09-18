Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement

    Public Class TestimonialController
        Private db As New ContentDataContext

        Public Function Create(ByVal Quote As String, ByVal Author As String, ByVal Approved As Boolean, ByVal ApprovedBy As String, ByVal ImageID As Integer) As Integer
            Dim item As New Testimonial
            Dim itemId As Integer

            item.Quote = Quote
            item.Author = Author
            item.Approved = Approved
            item.ApprovedBy = ApprovedBy
            If Approved Then
                item.ApprovedOn = Date.Now
            End If
            item.ImageID = ImageID

            db.Testimonials.InsertOnSubmit(item)
            db.SubmitChanges()

            itemId = item.ID

            item = Nothing

            Return itemId
        End Function

        Public Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim item = (From i In db.Testimonials Where i.ID = ID Select i).First

                db.Testimonials.DeleteOnSubmit(item)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function Update(ByVal ID As Integer, ByVal Quote As String, ByVal Author As String, ByVal Approved As Boolean, ByVal ApprovedBy As String, ByVal ImageID As Integer) As Boolean
            Dim item As Testimonial

            item = (From i In db.Testimonials Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("Testimonial with ID: " & ID.ToString & " does not exist.")
            Else
                item.Quote = Quote
                item.Author = Author
                item.Approved = Approved
                item.ApprovedBy = ApprovedBy
                If Approved Then
                    item.ApprovedOn = Date.Now
                End If
                item.ImageID = ImageID

                db.SubmitChanges()
            End If

            item = Nothing

            Return True
        End Function

        Public Function GetElement(ByVal ID As Integer) As Testimonial
            Dim item As Testimonial

            item = (From i In db.Testimonials Where i.ID = ID).SingleOrDefault

            If item Is Nothing Then
                Throw New Exception("Testimonial with ID: " & ID.ToString & " does not exist.")
            Else
                Return item
            End If

            item = Nothing
        End Function

        Public Function GetElements() As List(Of Testimonial)
            Dim itemList As New List(Of Testimonial)

            Dim items = From t In db.Testimonials Select t

            If items Is Nothing And items.Count > 0 Then
                Throw New Exception("There are no Testimonials")
            Else
                For Each e As Testimonial In items
                    itemList.Add(e)
                Next

                Return itemList
            End If
        End Function

    End Class

End Namespace