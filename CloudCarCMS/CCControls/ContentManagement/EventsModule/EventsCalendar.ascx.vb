Imports CloudCar.CCFramework
Imports CloudCar.CCFramework.ContentManagement.EventsModule

Namespace CCControls.ContentManagement.EventsModule

    Public Class EventsCalendar
        Inherits UserControl

        Private Const _ShowName As Boolean = True
        Private Const _ShowTime As Boolean = True

        Protected Sub EventsCalendarDayRender(ByVal Sender As Object, ByVal Args As DayRenderEventArgs) Handles EventsCalendar.DayRender
            Dim CurrentEvents As List(Of Model.Event) = EventsController.GetEventsByDate(Args.Day.Date)

            For Each CurrentEvent As Model.Event In CurrentEvents
                Dim CurrentControl As New HyperLink()

                CurrentControl.ID = String.Format("EventsButton{0}", CurrentEvent.Id)
                CurrentControl.Text = ""

                If _ShowName Then
                    CurrentControl.Text &= String.Format("{0}", CurrentEvent.Title)
                End If

                If _ShowTime Then
                    CurrentControl.Text &= String.Format(" <br /> {0:h:mm tt}", CurrentEvent.Time)
                End If

                CurrentControl.NavigateUrl = String.Format("/Events/{0}.html", CurrentEvent.Permalink)

                Args.Cell.Controls.Add(CurrentControl)
            Next

        End Sub

    End Class

End Namespace