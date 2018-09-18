Imports CloudCar.CCFramework.ContentManagement.NewsModule
Imports CloudCar.CCFramework.ContentManagement.EventsModule

Namespace CCControls.ContentManagement

    Partial Public Class RecentEventAndNewsControl
        Inherits UserControl

        Protected Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                rptEntries.DataSource = EventsController.GetApprovedEvents().OrderByDescending(Function(t) t.EventDate).Take(1)
                rptEntries.DataBind()
            End If
        End Sub

    End Class

End Namespace