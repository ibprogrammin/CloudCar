Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement.CalendarModule

    Public Class BookingController
        Inherits CCFramework.Core.DataControllerClass

        Public Shared GetBookingsFunc As Func(Of CommerceDataContext, IQueryable(Of Booking)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                             From b In CurrentDataContext.Bookings Select b)

        Public Shared GetBookingByIdFunc As Func(Of CommerceDataContext, Integer, Integer, Booking) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, UserId As Integer, ScheduleId As Integer) _
                                              (From b In CurrentDataContext.Bookings Where b.UserMembershipId = UserId And b.ScheduleId = ScheduleId Select b).FirstOrDefault)

        Public Shared GetBookingByScheduleIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Booking)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ScheduleId As Integer) _
                                      From b In CurrentDataContext.Bookings Where b.ScheduleId = ScheduleId Select b)

        Public Shared GetBookingCountByUserIdAndDayFunc As Func(Of CommerceDataContext, Integer, Date, Integer) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, UserId As Integer, SelectedDate As Date) _
                                              (From b In CurrentDataContext.Bookings _
                                                Join s In CurrentDataContext.Schedules On b.ScheduleId Equals s.Id _
                                                Where b.UserMembershipId = UserId _
                                                And s.BookingDate.Year = SelectedDate.Year _
                                                And s.BookingDate.Month = SelectedDate.Month _
                                                And s.BookingDate.Day = SelectedDate.Day _
                                                Select s).Count)

        Public Overloads Function Create(ByVal UserId As Integer, ByVal ScheduleId As Integer, ByVal Approved As Boolean) As Integer
            Dim Item As New Booking
            Dim ItemId As Integer

            Item.UserMembershipId = UserId
            Item.ScheduleId = ScheduleId
            Item.Approved = Approved

            db.Bookings.InsertOnSubmit(Item)
            db.SubmitChanges()

            ItemId = Integer.Parse(Item.UserMembershipId.ToString() & Item.ScheduleId.ToString)

            Item = Nothing

            Return ItemId
        End Function

        Public Overloads Function Delete(ByVal UserId As Integer, ByVal ScheduleId As Integer) As Boolean
            Try
                Dim Item As Booking = GetBookingByIdFunc(db, UserId, ScheduleId)

                db.Bookings.DeleteOnSubmit(Item)
                db.SubmitChanges()

                Return True
            Catch Ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal UserId As Integer, ByVal ScheduleId As Integer, ByVal Approved As Boolean) As Boolean
            Dim Item As Booking = GetBookingByIdFunc(db, UserId, ScheduleId)

            If Item Is Nothing Then
                Throw New Exception("Booking " & UserId.ToString & ScheduleId.ToString & " does not exist.")
            Else
                Item.UserMembershipId = UserId
                Item.ScheduleId = ScheduleId
                Item.Approved = Approved

                db.SubmitChanges()
            End If

            Item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal UserId As Integer, ByVal ScheduleId As Integer) As Booking
            Dim Item As Booking = GetBookingByIdFunc(db, UserId, ScheduleId)

            If Item Is Nothing Then
                Throw New Exception("Booking with ID: " & UserId.ToString & ScheduleId.ToString & " does not exist.")
            Else
                Return Item
            End If

            Item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of Booking)
            Dim CurrentBookings As List(Of Booking) = GetBookingsFunc(db).ToList

            If CurrentBookings Is Nothing Then
                Throw New Exception("There are no bookings")
            Else
                Return CurrentBookings
            End If
        End Function

        Public Shared Function CheckIfUserIsBooked(ByVal ScheduleId As Integer, ByVal UserMembershipId As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentBooking As Booking = GetBookingByIdFunc(CurrentDataContext, UserMembershipId, ScheduleId)

            If Not CurrentBooking Is Nothing Then
                CheckIfUserIsBooked = True
            Else
                CheckIfUserIsBooked = False
            End If

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetBookingsBySchedule(ByVal ScheduleId As Integer) As List(Of Booking)
            Dim CurrentDataContext As New CommerceDataContext

            GetBookingsBySchedule = GetBookingByScheduleIdFunc(CurrentDataContext, ScheduleId).ToList

            'CurrentDataContext.Dispose()
        End Function

        Private Const BookingsPerDay As Integer = 1

        Public Shared Function ExceededMaximumBookingsPerDay(ByVal UserId As Integer, ByVal ScheduleId As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentScheduleController As New ScheduleController()
            Dim CurrentSchedule As Schedule = CurrentScheduleController.GetElement(ScheduleId)

            If GetBookingCountByUserIdAndDayFunc(CurrentDataContext, UserId, CurrentSchedule.BookingDate) >= BookingsPerDay Then
                ExceededMaximumBookingsPerDay = True
            Else
                ExceededMaximumBookingsPerDay = False
            End If

            CurrentDataContext.Dispose()
        End Function

    End Class

End Namespace