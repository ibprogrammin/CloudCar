Imports CloudCar.CCFramework
Imports CloudCar.CCFramework.ContentManagement.EventsModule

Namespace CCControls.ContentManagement.EventsModule

    Partial Public Class EventsControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                BindEventsRepeater()
            End If
        End Sub

        Protected Sub BindEventsRepeater()
            EventsRepeater.DataSource = EventsController.GetItems()
            EventsRepeater.DataBind()
        End Sub

    End Class

End Namespace