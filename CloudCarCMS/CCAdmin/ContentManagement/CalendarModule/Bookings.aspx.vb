Namespace CCAdmin.ContentManagement.CalendarModule
    Partial Public Class Bookings
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadBookings()
            End If
        End Sub

        Private Sub LoadBookings()
            Dim CurrentBookingController As New CCFramework.ContentManagement.CalendarModule.BookingController()

            gvBookings.DataSource = CurrentBookingController.GetElements()
            gvBookings.DataBind()
        End Sub

        Private Sub BookingsGridViewPageIndexChanged(ByVal Sender As Object, ByVal E As DataGridPageChangedEventArgs) Handles gvBookings.PageIndexChanged
            gvBookings.CurrentPageIndex = E.NewPageIndex

            LoadBookings()
        End Sub

    End Class
End Namespace