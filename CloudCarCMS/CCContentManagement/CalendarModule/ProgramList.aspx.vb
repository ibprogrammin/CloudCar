Namespace CCContentManagement.CalendarModule

    Partial Public Class ProgramList
        Inherits ContentPageTemplate

        Protected Overrides Sub OnInit(ByVal E As EventArgs)
            Permalink = CCFramework.Core.Settings.ProgramsPage

            MyBase.OnInit(E)
        End Sub

        Protected Overrides Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            MyBase.PageLoad(Sender, E)

            If Not Page.IsPostBack Then
                LoadPrograms()
            End If
        End Sub

        Private Sub LoadPrograms()
            Dim CurrentProgramController As New CCFramework.ContentManagement.CalendarModule.ProgramController

            ProgramsRepeater.DataSource = CurrentProgramController.GetElements()
            ProgramsRepeater.DataBind()
        End Sub

    End Class

End Namespace