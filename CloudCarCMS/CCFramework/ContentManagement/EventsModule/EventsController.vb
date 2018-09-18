Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports System.Data.Linq

Namespace CCFramework.ContentManagement.EventsModule

    Public Class EventsController

        Public Shared GetEventsFunc As Func(Of ContentDataContext, IQueryable(Of Model.Event)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      From e In CurrentDataContext.Events Select e)

        Public Shared GetApprovedEventsFunc As Func(Of ContentDataContext, IQueryable(Of Model.Event)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      From e In CurrentDataContext.Events _
                                      Where e.Approved = True Select e _
                                      Order By e.EventDate Ascending)

        Public Shared GetUpcomingEventsFunc As Func(Of ContentDataContext, Integer, IQueryable(Of Model.Event)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Count As Integer) _
                                      From e In CurrentDataContext.Events _
                                      Where e.Approved = True _
                                      And e.EventDate > DateTime.Now _
                                      Select e _
                                      Order By e.EventDate Ascending _
                                      Take Count)

        Public Shared GetPastEventsFunc As Func(Of ContentDataContext, IQueryable(Of Model.Event)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      From e In CurrentDataContext.Events _
                                      Where e.EventDate < DateTime.Now _
                                      Select e Order By e.EventDate Ascending)

        Public Shared GetEventByIdFunc As Func(Of ContentDataContext, Integer, Model.Event) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) _
                                      (From e In CurrentDataContext.Events Where e.Id = Id Select e).FirstOrDefault)

        Public Shared GetEventByPermalinkFunc As Func(Of ContentDataContext, String, Model.Event) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Permalink As String) _
                                      (From e In CurrentDataContext.Events Where e.Permalink Like Permalink Select e).FirstOrDefault)

        Public Shared GetEventIdByPermalinkFunc As Func(Of ContentDataContext, String, Integer) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Permalink As String) _
                                      (From e In CurrentDataContext.Events Where e.Permalink Like Permalink Select e.Id).FirstOrDefault)

        Public Shared GetNextEventFunc As Func(Of ContentDataContext, Model.Event) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      (From e In CurrentDataContext.Events _
                                       Where e.EventDate > DateTime.Now _
                                       And e.Approved = True _
                                       Select e Order By e.EventDate Ascending).FirstOrDefault)

        Public Shared GetEventsByDateFunc As Func(Of ContentDataContext, Date, IQueryable(Of Model.Event)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, SelectedDate As Date) _
                                              From e In CurrentDataContext.Events _
                                              Where e.EventDate.Value.Day = SelectedDate.Day _
                                              And e.EventDate.Value.Month = SelectedDate.Month _
                                              And e.EventDate.Value.Year = SelectedDate.Year _
                                              Order By e.EventDate _
                                              Select e)

        ''' <summary>
        ''' Creates the Event with an image
        ''' </summary>
        ''' <param name="Title">The title.</param>
        ''' <param name="Permalink">The permalink for the event.</param>
        ''' <param name="Time">The time information.</param>
        ''' <param name="Location">The location information.</param>
        ''' <param name="Details">Details about the event.</param>
        ''' <param name="ImageId">The picture ID.</param>
        ''' <param name="EventDate">The event date.</param>
        ''' <param name="Approved">if set to <c>true</c> [approved].</param>
        ''' <returns>An integer value representing the newly created events Id</returns>
        Public Shared Function Create(ByVal Title As String, ByVal Permalink As String, ByVal Time As String, ByVal Location As String, ByVal Details As String, ByVal ImageId As Integer, ByVal EventDate As DateTime, ByVal Approved As Boolean) As Integer
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentEvent As New Model.Event
            Dim CurrentEventId As Integer

            CurrentEvent.Title = Title
            CurrentEvent.Time = Time
            CurrentEvent.Location = Location
            CurrentEvent.Details = Details
            CurrentEvent.ImageId = ImageId
            CurrentEvent.DateAdded = Date.Now
            CurrentEvent.EventDate = EventDate
            CurrentEvent.Approved = Approved
            CurrentEvent.Permalink = Permalink

            CurrentDataContext.Events.InsertOnSubmit(CurrentEvent)
            CurrentDataContext.SubmitChanges()

            CurrentEventId = CurrentEvent.Id

            CurrentDataContext.Dispose()

            Return CurrentEventId
        End Function

        ''' <summary>
        ''' Creates the Event without a picture
        ''' </summary>
        ''' <param name="Title">The title.</param>
        ''' <param name="Permalink">The permalink for the event.</param>
        ''' <param name="Time">The time information.</param>
        ''' <param name="Location">The location information.</param>
        ''' <param name="Details">Details about the event.</param>
        ''' <param name="EventDate">The event date.</param>
        ''' <param name="Approved">if set to <c>true</c> [approved].</param>
        ''' <returns>An integer value representing the newly created events Id</returns>
        Public Shared Function Create(ByVal Title As String, ByVal Permalink As String, ByVal Time As String, ByVal Location As String, ByVal Details As String, ByVal EventDate As DateTime, ByVal Approved As Boolean) As Integer
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentEvent As New Model.Event
            Dim CurrentEventId As Integer

            CurrentEvent.Title = Title
            CurrentEvent.Time = Time
            CurrentEvent.Location = Location
            CurrentEvent.Details = Details
            CurrentEvent.DateAdded = Date.Now
            CurrentEvent.EventDate = EventDate
            CurrentEvent.Approved = Approved
            CurrentEvent.Permalink = Permalink

            CurrentDataContext.Events.InsertOnSubmit(CurrentEvent)
            CurrentDataContext.SubmitChanges()

            CurrentEventId = CurrentEvent.Id

            CurrentDataContext.Dispose()

            Return CurrentEventId
        End Function

        ''' <summary>
        ''' Updates the Events with an image
        ''' </summary>
        ''' <param name="Id">The Event Id.</param>
        ''' <param name="Title">The title.</param>
        ''' <param name="Permalink">The permalink for the event.</param>
        ''' <param name="Time">The time information.</param>
        ''' <param name="Location">The location information.</param>
        ''' <param name="Details">Details about the event.</param>
        ''' <param name="ImageId">The picture ID.</param>
        ''' <param name="EventDate">The event date.</param>
        Public Shared Sub Update(ByVal Id As Integer, ByVal Title As String, ByVal Permalink As String, ByVal Time As String, ByVal Location As String, ByVal Details As String, ByVal ImageId As Integer, ByVal EventDate As DateTime)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentEvent As Model.Event

            CurrentEvent = GetEventByIdFunc(CurrentDataContext, Id)

            If CurrentEvent Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("Event does not exist")
            Else
                CurrentEvent.Title = Title
                CurrentEvent.Time = Time
                CurrentEvent.Location = Location
                CurrentEvent.Details = Details
                CurrentEvent.ImageId = ImageId
                CurrentEvent.DateAdded = Date.Now
                CurrentEvent.EventDate = EventDate
                CurrentEvent.Permalink = Permalink

                CurrentDataContext.SubmitChanges()

                CurrentDataContext.Dispose()
            End If
        End Sub

        ''' <summary>
        ''' Updates the Event without an image
        ''' </summary>
        ''' <param name="Id">The Event Id.</param>
        ''' <param name="Title">The title.</param>
        ''' <param name="Permalink">The permalink for the event.</param>
        ''' <param name="Time">The time information.</param>
        ''' <param name="Location">The location information.</param>
        ''' <param name="Details">Details about the event.</param>
        ''' <param name="EventDate">The event date.</param>
        Public Shared Sub Update(ByVal Id As Integer, ByVal Title As String, ByVal Permalink As String, ByVal Time As String, ByVal Location As String, ByVal Details As String, ByVal EventDate As DateTime)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentEvent As Model.Event

            CurrentEvent = GetEventByIdFunc(CurrentDataContext, Id)

            If CurrentEvent Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("Event does not exist")
            Else
                CurrentEvent.Title = Title
                CurrentEvent.Time = Time
                CurrentEvent.Location = Location
                CurrentEvent.Details = Details
                CurrentEvent.DateAdded = Date.Now
                CurrentEvent.EventDate = EventDate
                CurrentEvent.Permalink = Permalink

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If
        End Sub

        Public Shared Sub RemovePicture(ByVal Id As Integer)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentEvent As Model.Event

            CurrentEvent = GetEventByIdFunc(CurrentDataContext, Id)

            If CurrentEvent Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("Event does not exist")
            Else
                CurrentEvent.ImageId = 0

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If
        End Sub

        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Try
                Dim CurrentDataContext As New ContentDataContext
                Dim CurrentEvent As Model.Event

                CurrentEvent = GetEventByIdFunc(CurrentDataContext, Id)

                CurrentDataContext.Events.DeleteOnSubmit(CurrentEvent)
                CurrentDataContext.SubmitChanges()

                CurrentDataContext.Dispose()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetItem(ByVal Id As Integer) As Model.Event
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentEvent As Model.Event

            CurrentEvent = GetEventByIdFunc(CurrentDataContext, Id)

            If CurrentEvent Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("Event does not exist")
            Else
                GetItem = CurrentEvent

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetItem(ByVal Permalink As String) As Model.Event
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentEvent As Model.Event

            CurrentEvent = GetEventByPermalinkFunc(CurrentDataContext, Permalink)

            If CurrentEvent Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("Event does not exist")
            Else
                GetItem = CurrentEvent

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetEventIdByPermalink(ByVal Permalink As String) As Integer
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentEventId As Integer

            CurrentEventId = GetEventIdByPermalinkFunc(CurrentDataContext, Permalink)

            If CurrentEventId = Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("Event does not exist")
            Else
                GetEventIdByPermalink = CurrentEventId

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetNextEvent() As Model.Event
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentEvent As Model.Event

            CurrentEvent = GetNextEventFunc(CurrentDataContext)

            If CurrentEvent Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("Event does not exist")
            Else
                GetNextEvent = CurrentEvent

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetItems() As List(Of Model.Event)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentEventsList As List(Of Model.Event)

            CurrentEventsList = GetEventsFunc(CurrentDataContext).ToList

            If CurrentEventsList Is Nothing And CurrentEventsList.Count > 0 Then
                CurrentDataContext.Dispose()

                Throw New Exception("There are currently no events")
            Else
                GetItems = CurrentEventsList

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetEventsByDate(ByVal SelectedDate As Date) As List(Of Model.Event)
            Dim CurrentDataContext As New ContentDataContext

            Return GetEventsByDateFunc(CurrentDataContext, SelectedDate).ToList()
        End Function

        Public Shared Function GetApprovedEvents() As List(Of Model.Event)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentEventsList As List(Of Model.Event)

            CurrentEventsList = GetApprovedEventsFunc(CurrentDataContext).ToList

            If CurrentEventsList Is Nothing And CurrentEventsList.Count > 0 Then
                CurrentDataContext.Dispose()

                Throw New Exception("There are currently no events")
            Else
                GetApprovedEvents = CurrentEventsList

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetUpcomingEvents(ByVal Count As Integer) As List(Of Model.Event)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentEventsList As List(Of Model.Event)

            CurrentEventsList = GetUpcomingEventsFunc(CurrentDataContext, Count).ToList

            If CurrentEventsList Is Nothing And CurrentEventsList.Count > 0 Then
                GetUpcomingEvents = Nothing
            Else
                GetUpcomingEvents = CurrentEventsList
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Sub SubmitEventApprovalRequest(ByVal Id As Integer)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentEvent As Model.Event

            Dim Domain As String

            If Settings.EnableSSL And Settings.FullSSL Then
                Domain = String.Format("https://{0}", Settings.HostName)
            Else
                Domain = String.Format("http://{0}", Settings.HostName)
            End If

            CurrentEvent = GetEventByIdFunc(CurrentDataContext, Id)

            Dim CurrentNetworkCredentials As New Net.NetworkCredential

            CurrentNetworkCredentials.UserName = Settings.SMTPUser
            CurrentNetworkCredentials.Password = Settings.SMTPPass

            Dim CurrentSmtpClient As New Net.Mail.SmtpClient

            CurrentSmtpClient.Host = Settings.SMTPHost
            CurrentSmtpClient.Port = Settings.SMTPPort
            CurrentSmtpClient.Credentials = CurrentNetworkCredentials

            Dim CurrentMailMessage As New Net.Mail.MailMessage

            CurrentMailMessage.To.Add(Settings.AdminEmail)
            CurrentMailMessage.From = New Net.Mail.MailAddress(Settings.MailFromAddress)
            CurrentMailMessage.Subject = "New Event for Approval"
            CurrentMailMessage.IsBodyHtml = True
            CurrentMailMessage.Body = <html>
                                          <body>
                                              <table>
                                                  <tr>
                                                      <td>Title</td><td><%= CurrentEvent.Title %></td>
                                                  </tr>
                                                  <tr>
                                                      <td>Date</td><td><%= FormatDateTime(CurrentEvent.EventDate.Value, DateFormat.LongDate) %></td>
                                                  </tr>
                                                  <tr>
                                                      <td>Time Information</td><td><%= CurrentEvent.Time %></td>
                                                  </tr>
                                                  <tr>
                                                      <td>Location</td><td><%= CurrentEvent.Location %></td>
                                                  </tr>
                                                  <tr>
                                                      <td>Picture</td><td><img src=<%= String.Format("{0}/images/db/{1}/full/{2}.jpg", Domain, CurrentEvent.ImageId, CurrentEvent.Title.Replace(" ", "")) %>/></td>
                                                  </tr>
                                                  <tr>
                                                      <td><a href=<%= String.Format("{0}/CCAdmin/ContentManagement/EventsNews.aspx", Domain) %>>Approve</a></td>
                                                  </tr>
                                              </table>
                                          </body>
                                      </html>.ToString


            CurrentSmtpClient.EnableSsl = True
            CurrentSmtpClient.Send(CurrentMailMessage)

            CurrentSmtpClient.Dispose()
            CurrentMailMessage.Dispose()
            CurrentDataContext.Dispose()
        End Sub

        Public Shared Sub ApproveEvent(ByVal Id As Integer)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentEvent As Model.Event = GetEventByIdFunc(CurrentDataContext, Id)

            CurrentEvent.Approved = True
            CurrentDataContext.SubmitChanges()

            CurrentDataContext.Dispose()
        End Sub

        Public Shared Sub DeleteOldEvents()
            Dim CurrentDataContext As New ContentDataContext

            For Each Item In GetPastEventsFunc(CurrentDataContext)
                CurrentDataContext.Events.DeleteOnSubmit(Item)
            Next

            CurrentDataContext.SubmitChanges()

            CurrentDataContext.Dispose()
        End Sub

        Shared Sub Update(ByVal CurrentEventId As Integer, ByVal CurrentEventTitle As String, ByVal CurrentEventTime As String, ByVal CurrentEventLocation As String, ByVal CurrentEventDetails As String, ByVal CurrentEventDate As Date)
            Throw New NotImplementedException
        End Sub

        Public Shared GetEventTitleByIdFunc As Func(Of ContentDataContext, Integer, String) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) _
                                      (From e In CurrentDataContext.Events Where e.Id = Id Select e.Title).FirstOrDefault)

        Public Shared Function GetBreadCrumb(ByVal EventId As Integer) As String
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentEventTitle As String = GetEventTitleByIdFunc(CurrentDataContext, EventId)

            Dim BreadCrumbStringBuilder As New StringBuilder
            BreadCrumbStringBuilder.AppendFormat("<a href=""/"">Home</a> {0} ", Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("<a href=""/Events/Index.html"">Events</a> {0} ", Settings.BreadCrumbDelimiter)
            BreadCrumbStringBuilder.AppendFormat("{0}", CurrentEventTitle)

            Return BreadCrumbStringBuilder.ToString
        End Function

    End Class

End Namespace