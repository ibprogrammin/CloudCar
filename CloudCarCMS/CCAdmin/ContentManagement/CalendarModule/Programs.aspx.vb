Imports CloudCar.CCFramework.ContentManagement.CalendarModule

Namespace CCAdmin.ContentManagement.CalendarModule
    Partial Public Class Programs
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadPrograms()
            End If
        End Sub

        Private Sub LoadPrograms()
            Dim CurrentProgramController As New ProgramController()

            gvPrograms.DataSource = CurrentProgramController.GetElements()
            gvPrograms.DataBind()
        End Sub

        Private Sub ProgramsGridViewPageIndexChanged(ByVal Sender As Object, ByVal E As DataGridPageChangedEventArgs) Handles gvPrograms.PageIndexChanged
            gvPrograms.CurrentPageIndex = E.NewPageIndex

            LoadPrograms()
        End Sub

    End Class
End Namespace