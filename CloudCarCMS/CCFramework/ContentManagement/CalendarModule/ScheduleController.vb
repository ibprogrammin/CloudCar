Imports System.IO
Imports System.Data.Linq
'Imports Microsoft.Reporting.WebForms
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement.CalendarModule

    Public Class ScheduleController
        Inherits Core.DataControllerClass

        Public Shared GetSchedulesFunc As Func(Of CommerceDataContext, IOrderedQueryable(Of Schedule)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                             From s In CurrentDataContext.Schedules Select s Order By s.BookingDate)

        Public Shared GetScheduleByIdFunc As Func(Of CommerceDataContext, Integer, Schedule) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ScheduleId As Integer) _
                                              (From s In CurrentDataContext.Schedules Where s.Id = ScheduleId Select s).FirstOrDefault)

        Public Overloads Function Create(ByVal ProgramId As Integer, ByVal Capacity As Integer, ByVal Free As Boolean, ByVal BookingDate As DateTime, ByVal Duration As Integer) As Integer
            Dim Item As New Schedule
            Dim ItemId As Integer

            Item.ProgramId = ProgramId
            Item.Capacity = Capacity
            Item.Free = Free
            Item.BookingDate = BookingDate
            Item.Duration = Duration

            db.Schedules.InsertOnSubmit(Item)
            db.SubmitChanges()

            ItemId = Item.Id

            Item = Nothing

            Return ItemId
        End Function

        Public Overloads Function Delete(ByVal Id As Integer) As Boolean
            'Try
            Dim Item As Schedule = GetScheduleByIdFunc(db, Id)

            db.Schedules.DeleteOnSubmit(Item)
            db.SubmitChanges()

            Return True
            'Catch Ex As Exception
            'Return False
            'End Try
        End Function

        Public Overloads Function Update(ByVal Id As Integer, ByVal ProgramId As Integer, ByVal Capacity As Integer, ByVal Free As Boolean, ByVal BookingDate As DateTime, ByVal Duration As Integer) As Boolean
            Dim Item As Schedule = GetScheduleByIdFunc(db, Id)

            If Item Is Nothing Then
                Throw New Exception("Schedule " & Id.ToString & " does not exist.")
            Else
                Item.ProgramId = ProgramId
                Item.Capacity = Capacity
                Item.Free = Free
                Item.BookingDate = BookingDate
                Item.Duration = Duration

                db.SubmitChanges()
            End If

            Item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As Schedule
            Dim Item As Schedule = GetScheduleByIdFunc(db, ID)

            If Item Is Nothing Then
                Throw New Exception("Schedule with ID: " & ID.ToString & " does not exist.")
            Else
                Return Item
            End If

            Item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of Schedule)
            Dim CurrentSchedules As List(Of Schedule) = GetSchedulesFunc(db).ToList

            If CurrentSchedules Is Nothing Then
                Throw New Exception("There are no schedules")
            Else
                Return CurrentSchedules
            End If
        End Function


        Public Shared GetProgramSchedulesByProgramIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Schedule)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProgramId As Integer) _
                                              From s In CurrentDataContext.Schedules _
                                              Where s.ProgramId = ProgramId _
                                              Order By s.BookingDate _
                                              Select s)

        Public Shared Function GetProgramSchedules(ByVal ProgramId As Integer) As List(Of Schedule)
            Dim CurrentDataContext As New CommerceDataContext

            GetProgramSchedules = GetProgramSchedulesByProgramIdFunc(CurrentDataContext, ProgramId).ToList()

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetProgramSchedulesByDateFunc As Func(Of CommerceDataContext, Date, IQueryable(Of Schedule)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, SelectedDate As Date) _
                                              From s In CurrentDataContext.Schedules _
                                              Where s.BookingDate.Day = SelectedDate.Day _
                                              And s.BookingDate.Month = SelectedDate.Month _
                                              And s.BookingDate.Year = SelectedDate.Year _
                                              Order By s.BookingDate _
                                              Select s)

        Public Shared Function GetProgramSchedulesByDate(ByVal SelectedDate As Date) As List(Of Schedule)
            Dim CurrentDataContext As New CommerceDataContext

            'GetProgramSchedulesByDate = 
            Return GetProgramSchedulesByDateFunc(CurrentDataContext, SelectedDate).ToList()

            'CurrentDataContext.Dispose()
        End Function

        Public Shared GetPastProgramSchedulesByProgramIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Schedule)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProgramId As Integer) _
                                              From s In CurrentDataContext.Schedules _
                                              Where s.ProgramId = ProgramId _
                                              And s.BookingDate < Date.Now _
                                              Order By s.BookingDate _
                                              Select s)

        Public Shared Function GetPastProgramSchedules(ByVal ProgramId As Integer) As List(Of Schedule)
            Dim CurrentDataContext As New CommerceDataContext

            GetPastProgramSchedules = GetPastProgramSchedulesByProgramIdFunc(CurrentDataContext, ProgramId).ToList()

            CurrentDataContext.Dispose()
        End Function


        Public Shared GetFutureProgramSchedulesByProgramIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Schedule)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProgramId As Integer) _
                                              From s In CurrentDataContext.Schedules _
                                              Where s.ProgramId = ProgramId _
                                              And s.BookingDate >= Date.Now _
                                              Order By s.BookingDate _
                                              Select s)

        Public Shared Function GetFutureProgramSchedules(ByVal ProgramId As Integer) As List(Of Schedule)
            Dim CurrentDataContext As New CommerceDataContext

            GetFutureProgramSchedules = GetFutureProgramSchedulesByProgramIdFunc(CurrentDataContext, ProgramId).ToList()

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetFutureScheduleDaysFunc As Func(Of CommerceDataContext, IQueryable(Of Date)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                              (From s In CurrentDataContext.Schedules _
                                              Where s.BookingDate >= Date.Now _
                                              Order By s.BookingDate _
                                              Select s.BookingDate.Date Distinct))

        Public Shared Function GetFutureScheduleDays() As List(Of Date)
            Dim CurrentDataContext As New CommerceDataContext

            GetFutureScheduleDays = GetFutureScheduleDaysFunc(CurrentDataContext).ToList

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetFutureScheduleDaysByMonthYearFunc As Func(Of CommerceDataContext, Integer, Integer, IQueryable(Of Date)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, SelectedMonth As Integer, SelectedYear As Integer) _
                                              (From s In CurrentDataContext.Schedules _
                                              Where s.BookingDate >= Date.Now _
                                              And s.BookingDate.Month = SelectedMonth _
                                              And s.BookingDate.Year = SelectedYear _
                                              Order By s.BookingDate _
                                              Select s.BookingDate.Date Distinct))

        Public Shared Function GetFutureScheduleDaysByMonthYear(ByVal SelectedMonth As Integer, ByVal SelectedYear As Integer) As List(Of Date)
            Dim CurrentDataContext As New CommerceDataContext

            GetFutureScheduleDaysByMonthYear = GetFutureScheduleDaysByMonthYearFunc(CurrentDataContext, SelectedMonth, SelectedYear).ToList

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetSchedulesByMonthYearProgramFunc As Func(Of CommerceDataContext, Integer, Integer, Integer, IQueryable(Of Schedule)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, SelectedMonth As Integer, SelectedYear As Integer, SelectedProgramId As Integer) _
                                              (From s In CurrentDataContext.Schedules _
                                              Where s.ProgramId = SelectedProgramId _
                                              And s.BookingDate.Month = SelectedMonth _
                                              And s.BookingDate.Year = SelectedYear _
                                              Order By s.BookingDate _
                                              Select s))

        Public Shared Function GetSchedulesByMonthYearProgram(ByVal SelectedMonth As Integer, ByVal SelectedYear As Integer, ByVal SelectedProgramId As Integer) As List(Of Schedule)
            Dim CurrentDataContext As New CommerceDataContext

            GetSchedulesByMonthYearProgram = GetSchedulesByMonthYearProgramFunc(CurrentDataContext, SelectedMonth, SelectedYear, SelectedProgramId).ToList

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetScheduleCapacityFunc As Func(Of CommerceDataContext, Integer, Integer) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ScheduleId As Integer) _
                                              (From s In CurrentDataContext.Schedules _
                                              Where s.Id = ScheduleId Select s.Capacity).FirstOrDefault())

        Public Shared Function GetScheduleCapacity(ByVal ScheduleId As Integer) As Integer
            Dim CurrentDataContext As New CommerceDataContext

            GetScheduleCapacity = GetScheduleCapacityFunc(CurrentDataContext, ScheduleId)

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetScheduleBookingsCountFunc As Func(Of CommerceDataContext, Integer, Integer) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ScheduleId As Integer) _
                                              (From b In CurrentDataContext.Bookings _
                                              Where b.ScheduleId = ScheduleId Select b).Count())

        Public Shared Function GetScheduleBookingsCount(ByVal ScheduleId As Integer) As Integer
            Dim CurrentDataContext As New CommerceDataContext

            GetScheduleBookingsCount = GetScheduleBookingsCountFunc(CurrentDataContext, ScheduleId)

            CurrentDataContext.Dispose()
        End Function

        Private Const CancelHoursBeforeSchedule As Integer = 24

        Public Shared Function CanCancelSchedule(ByVal ScheduleId As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentSchedule As Schedule = GetScheduleByIdFunc(CurrentDataContext, ScheduleId)

            If (CurrentSchedule.BookingDate - Date.Now).Ticks > (CancelHoursBeforeSchedule * TimeSpan.TicksPerHour) Then
                CanCancelSchedule = True
            Else
                CanCancelSchedule = False
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetRemainingCapacity(ByVal ScheduleId As Integer) As Integer
            Dim Capacity As Integer = GetScheduleCapacity(ScheduleId)
            Dim Bookings As Integer = GetScheduleBookingsCount(ScheduleId)

            Return (Capacity - Bookings)
        End Function

        Public Shared GetSignedUpProgramSchedulesByUserIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Schedule)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, UserId As Integer) _
                                              From s In CurrentDataContext.Schedules _
                                              Join b In CurrentDataContext.Bookings _
                                              On b.ScheduleId Equals s.Id _
                                              Where b.UserMembershipId = UserId _
                                              Order By s.BookingDate _
                                              Select s)

        Public Shared Function GetSignedUpProgramSchedules(ByVal UserId As Integer) As List(Of Schedule)
            Dim CurrentDataContext As New CommerceDataContext

            'GetSignedUpProgramSchedules = 
            Return GetSignedUpProgramSchedulesByUserIdFunc(CurrentDataContext, UserId).ToList()

            'CurrentDataContext.Dispose()
        End Function

        Public Shared GetFutureSignedUpProgramSchedulesByUserIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Schedule)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, UserId As Integer) _
                                              From s In CurrentDataContext.Schedules _
                                              Join b In CurrentDataContext.Bookings _
                                              On b.ScheduleId Equals s.Id _
                                              Where b.UserMembershipId = UserId _
                                              And s.BookingDate >= Date.Now _
                                              Order By s.BookingDate _
                                              Select s)

        Public Shared Function GetFutureSignedUpProgramSchedules(ByVal UserId As Integer) As List(Of Schedule)
            Dim CurrentDataContext As New CommerceDataContext

            'GetSignedUpProgramSchedules = 
            Return GetFutureSignedUpProgramSchedulesByUserIdFunc(CurrentDataContext, UserId).ToList()

            'CurrentDataContext.Dispose()
        End Function

        Public Shared Function CancelClass(ByVal ScheduleId As Integer) As Boolean
            Dim CurrentBookingController As New BookingController
            Dim CurrentBookings As List(Of Booking) = BookingController.GetBookingsBySchedule(ScheduleId)

            If Not CurrentBookings Is Nothing Then
                If CurrentBookings.Count > 0 Then

                    Dim CurrentProgramName As String = CurrentBookings.First.Schedule.Program.Name
                    Dim CurrentProgramDate As Date = CurrentBookings.First.Schedule.BookingDate

                    Dim Subject As String = String.Format("Your {0} class has been canceled!", CurrentProgramName)
                    Dim Message As String = String.Format("The {0} class scheduled for {1:dddd MMM dd, yyyy h:mm tt} you signed up for has been canceled.", CurrentProgramName, CurrentProgramDate)

                    Dim CurrentSimpleUserController As New CCFramework.Core.SimpleUserController()

                    For Each Item As Booking In CurrentBookings
                        Dim CurrentUser As SimpleUser = CurrentSimpleUserController.GetElement(Item.UserMembershipId)

                        CurrentBookingController.Delete(Item.UserMembershipId, Item.ScheduleId)

                        CCFramework.Core.SendEmails.SendNoticeMessage(CurrentUser.Email, Subject, Message)
                    Next
                End If
            End If

            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentSchedule As Schedule = GetScheduleByIdFunc(CurrentDataContext, ScheduleId)

            CurrentDataContext.Schedules.DeleteOnSubmit(CurrentSchedule)
            CurrentDataContext.SubmitChanges()

            CancelClass = True

            'Dim CurrentScheduleController As New ScheduleController()

            'CancelClass = CurrentScheduleController.Delete(ScheduleId)
        End Function

        Public Shared GetSignedUpUsersByScheduleIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of SimpleUser)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ScheduleId As Integer) _
                                              From u In CurrentDataContext.SimpleUsers _
                                              Join b In CurrentDataContext.Bookings _
                                              On u.ID Equals b.UserMembershipId _
                                              Where b.ScheduleId = ScheduleId _
                                              Order By u.FirstName _
                                              Select u)

        Public Shared Function GetSignedUpUsers(ByVal ScheduleId As Integer) As List(Of SimpleUser)
            Dim CurrentDataContext As New CommerceDataContext

            GetSignedUpUsers = GetSignedUpUsersByScheduleIdFunc(CurrentDataContext, ScheduleId).ToList()

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetScheduleForReport() As IQueryable(Of ScheduleFull)
            Dim db As New ContentDataContext

            GetScheduleForReport = From sf In db.ScheduleFulls Select sf

            db = Nothing
        End Function

        Public Shared Sub GeneratePDF(ByVal ReportFile As String, ByVal ExportFile As String)
            'Dim rv As New ReportViewer
            'Dim bytes As Byte()
            'Dim streamids As String() = Nothing
            'Dim mimeType As String = Nothing
            'Dim encoding As String = Nothing
            'Dim extension As String = Nothing
            'Dim warnings As Warning() = Nothing
            'Dim fs As FileStream
            'Dim ds As ObjectDataSource
            'Dim rds As ReportDataSource

            'File.Delete(ExportFile)
            'rv.LocalReport.ReportPath = ReportFile
            'ds = New ObjectDataSource
            'ds.SelectMethod = "GetScheduleForReport"
            'ds.TypeName = "CloudCar.CCFramework.ContentManagement.CalendarModule.ScheduleController"

            'rds = New ReportDataSource

            'rds.Name = "ScheduleFull"
            'rds.Value = ds

            'rv.LocalReport.DataSources.Add(rds)
            'bytes = rv.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)

            'fs = New FileStream(ExportFile, FileMode.Create)
            'fs.Write(bytes, 0, bytes.Length)
            'fs.Close()

            'warnings = Nothing
            'streamids = Nothing
            'ds = Nothing
            'rds = Nothing
            'fs = Nothing
            'bytes = Nothing
            'rv = Nothing
        End Sub

    End Class

End Namespace