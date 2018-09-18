Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement

    Public Class VideoController
        Public Shared GetVideoByIDFunc As Func(Of ContentDataContext, Integer, Video) = _
                CompiledQuery.Compile(Function(db As ContentDataContext, id As Integer) (From v In db.Videos Where v.ID = id Select v).FirstOrDefault)

        Public Shared GetAllVideosFunc As Func(Of ContentDataContext, IQueryable(Of Video)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) From v In db.Videos Select v)

        Public Shared Function Create(ByVal Title As String, ByVal VideoID As String, ByVal Player As SMVideoType, ByVal Details As String, ByVal Description As String, ByVal Keywords As String) As Integer
            Dim db As New ContentDataContext

            Dim vid As New Video

            vid.Title = Title
            vid.VideoID = VideoID
            vid.Player = Player
            vid.Details = Details
            vid.Description = Description
            vid.Keywords = Keywords

            db.Videos.InsertOnSubmit(vid)
            db.SubmitChanges()

            Dim CurrentVideoId As Integer = vid.ID

            vid = Nothing
            db = Nothing

            Return CurrentVideoId
        End Function

        Public Shared Sub Update(ByVal ID As Integer, ByVal Title As String, ByVal VideoId As String, ByVal Player As SMVideoType, ByVal Details As String, ByVal Description As String, ByVal Keywords As String)
            Dim db As New ContentDataContext

            Dim vid As Video = GetVideoByIDFunc(db, ID)

            If vid Is Nothing Then
                Throw New InvalidVideoException()
            Else
                vid.Title = Title
                vid.VideoID = VideoId
                vid.Player = Player
                vid.Details = Details
                vid.Description = Description
                vid.Keywords = Keywords

                db.SubmitChanges()
            End If

            vid = Nothing
            db = Nothing
        End Sub

        Public Shared Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim db As New ContentDataContext

                Dim vid As Video = GetVideoByIDFunc(db, ID)

                db.Videos.DeleteOnSubmit(vid)
                db.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetElements() As System.Linq.IQueryable(Of Video)
            Dim db As New ContentDataContext

            Dim videos As IQueryable(Of Video) = GetAllVideosFunc(db)

            If videos Is Nothing Then
                Throw New InvalidVideoException()
            Else
                Return videos
            End If

            db = Nothing
        End Function

        Public Shared Function GetElement(ByVal VideoId As Integer) As Video
            Dim db As New ContentDataContext

            Dim vid As Video = GetVideoByIDFunc(db, VideoId)

            If vid Is Nothing Then
                Throw New InvalidVideoException()
            Else
                Return vid
            End If

            db = Nothing
        End Function

        Public Class InvalidVideoException
            Inherits Exception

            Public Sub New()
                MyBase.New("The video you are looking for does not exist")
            End Sub

        End Class

        Public Shared Function GetPlayerHTML(ByVal Player As SMVideoType, ByVal VideoId As String, ByVal Width As Integer, ByVal Height As Integer) As String
            Select Case Player
                Case SMVideoType.None
                    Return ""
                Case SMVideoType.YouTube
                    Return <iframe src=<%= String.Format("http://www.youtube.com/embed/{0}?rel=0&wmode=transparent", VideoId) %> width=<%= Width.ToString %> height=<%= Height.ToString %> frameborder="0" allowfullscreen="true"></iframe>.ToString
                Case SMVideoType.Vimeo
                    Return <iframe src=<%= String.Format("http://player.vimeo.com/video/{0}?byline=0&amp;portrait=0&amp;color=ff0000", VideoId) %> width=<%= Width.ToString %> height=<%= Height.ToString %> frameborder="0"></iframe>.ToString
                Case Else
                    Return ""
            End Select
        End Function

    End Class

    Public Enum SMVideoType
        None = 0
        YouTube = 1
        Vimeo = 2
    End Enum

End Namespace