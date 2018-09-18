Imports CloudCar.CCFramework.ContentManagement.EventsModule

Namespace CCControls.ContentManagement.EventsModule

    Partial Public Class UpcomingEventsControl
        Inherits UserControl

        Protected Overrides Sub OnLoad(ByVal E As EventArgs)
            If Not Page.IsPostBack Then
                UpcomingEventsRepeater.DataSource = EventsController.GetUpcomingEvents(Count)
                UpcomingEventsRepeater.DataBind()
            End If
        End Sub

        Public Property Count() As Integer
            Get
                If Not ViewState("Count") Is Nothing Then
                    Return CType(ViewState("Count"), Integer)
                Else
                    Return 2
                End If
            End Get
            Set(ByVal Value As Integer)
                If Not ViewState("Count") Is Nothing Then
                    ViewState("Count") = Value
                Else
                    ViewState.Add("Count", Value)
                End If
            End Set
        End Property

    End Class

End Namespace