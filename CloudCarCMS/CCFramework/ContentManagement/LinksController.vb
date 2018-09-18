Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement

    Public Class LinksController

        Public Shared Function CreateLink(ByVal Title As String, ByVal URL As String, ByVal Notes As String, ByVal PictureID As Integer) As Integer
            Dim db As New ContentDataContext
            Dim link As New Link
            Dim linkId As Integer

            link.LinksTitle = Title
            link.LinkURL = URL
            link.LinkNotes = Notes
            link.PictureID = PictureID

            db.Links.InsertOnSubmit(link)
            db.SubmitChanges()

            linkId = link.LinksID

            link = Nothing
            db = Nothing

            Return linkId
        End Function

        Public Shared Function CreateLink(ByVal Title As String, ByVal URL As String, ByVal Notes As String) As Integer
            Dim db As New ContentDataContext
            Dim link As New Link
            Dim linkId As Integer

            link.LinksTitle = Title
            link.LinkURL = URL
            link.LinkNotes = Notes

            db.Links.InsertOnSubmit(link)
            db.SubmitChanges()

            linkId = link.LinksID

            link = Nothing
            db = Nothing

            Return linkId
        End Function

        Public Shared Sub UpdateLink(ByVal LinkID As Integer, ByVal Title As String, ByVal URL As String, ByVal Notes As String, ByVal PictureID As Integer)
            Dim db As New ContentDataContext
            Dim link As Link

            link = (From l In db.Links Where l.LinksID = LinkID).SingleOrDefault

            If link Is Nothing Then
                Throw New Exception("Link does not exist")
            Else
                link.LinksTitle = Title
                link.LinkURL = URL
                link.LinkNotes = Notes
                link.PictureID = PictureID

                db.SubmitChanges()
            End If

            link = Nothing
            db = Nothing
        End Sub

        Public Shared Sub UpdateLink(ByVal LinkID As Integer, ByVal Title As String, ByVal URL As String, ByVal Notes As String)
            Dim db As New ContentDataContext
            Dim link As Link

            link = (From l In db.Links Where l.LinksID = LinkID).SingleOrDefault

            If link Is Nothing Then
                Throw New Exception("Link does not exist")
            Else
                link.LinksTitle = Title
                link.LinkURL = URL
                link.LinkNotes = Notes

                db.SubmitChanges()
            End If

            link = Nothing
            db = Nothing
        End Sub

        Public Shared Function DeleteLink(ByVal LinkId As Integer) As Boolean
            Try
                Dim db As New ContentDataContext

                Dim link = (From l In db.Links Where l.LinksID = LinkId Select l).First

                db.Links.DeleteOnSubmit(link)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetLink(ByVal LinkID As Integer) As Link
            Dim db As New ContentDataContext
            Dim link As Link

            link = (From l In db.Links Where l.LinksID = LinkID).SingleOrDefault

            If link Is Nothing Then
                Throw New Exception("Link does not exist")
            Else
                GetLink = link
            End If

            db = Nothing
        End Function

        Public Shared Function GetLinks() As List(Of Link)
            Dim db As New ContentDataContext
            Dim linkCol As New List(Of Link)

            Dim links = From l In db.Links Select l

            If links Is Nothing And links.Count > 0 Then
                Throw New Exception("Link does not exist")
            Else
                For Each l As Link In links
                    linkCol.Add(l)
                Next

                GetLinks = linkCol
            End If

            db = Nothing
        End Function

    End Class

End Namespace