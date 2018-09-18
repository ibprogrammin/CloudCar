Namespace CCAdmin.ContentManagement.CalendarModule
    Partial Public Class Instructors
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadInstructors()
            End If
        End Sub

        Private Sub LoadInstructors()
            Dim CurrentInstructorController As New CCFramework.ContentManagement.CalendarModule.InstructorController()

            gvInstructors.DataSource = CurrentInstructorController.GetElements()
            gvInstructors.DataBind()
        End Sub

        Private Sub InstructorsGridViewPageIndexChanged(ByVal Sender As Object, ByVal E As DataGridPageChangedEventArgs) Handles gvInstructors.PageIndexChanged
            gvInstructors.CurrentPageIndex = E.NewPageIndex

            LoadInstructors()
        End Sub

    End Class
End Namespace