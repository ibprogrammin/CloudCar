Imports CloudCar.CCFramework.ContentManagement.EventsModule
Imports CloudCar.CCFramework

Namespace CCControls.ContentManagement.EventsModule

    Public Class EventsDetailsControl
        Inherits UserControl

        Public Property EventId() As Integer
            Get
                Return CInt(ViewState("EventId"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("EventId") = Value
                LoadEvent()
            End Set
        End Property

        Private Sub LoadEvent()
            If Not EventId = Nothing Then
                Dim CurrentEvent As Model.Event = EventsController.GetItem(EventId)

                TitleLiteral.Text = CurrentEvent.Title
                ContentLiteral.Text = CurrentEvent.Details
                StartDateLiteral.Text = CType(CurrentEvent.EventDate, DateTime).ToLongDateString
                TimeLiteral.Text = String.Format(" - {0}", CurrentEvent.Time)
                LocationLiteral.Text = CurrentEvent.Location

                If CurrentEvent.ImageId.HasValue Then
                    ThumbnailImage.ImageUrl = String.Format("/images/db/{0}/200/{1}.jpg", CurrentEvent.ImageId, CurrentEvent.Title.Replace(" ", "").Replace("'", ""))
                    ThumbnailImage.Visible = True
                End If
            Else
                Throw New Exception("The Event Id has not been set")
            End If
        End Sub

    End Class

End Namespace