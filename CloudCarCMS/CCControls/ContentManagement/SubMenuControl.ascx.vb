Imports CloudCar.CCFramework.Model

Namespace CCControls.ContentManagement

    Partial Public Class SubMenuControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadSubMenu()
            End If
        End Sub

        Private Sub LoadSubMenu()
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentLinksList As IQueryable(Of CCFramework.ContentManagement.ContentPageController.SubMenuItem) = CCFramework.ContentManagement.ContentPageController.GetSubMenuLinksFunc(CurrentDataContext)

            rptSubMenu.DataSource = CurrentLinksList
            rptSubMenu.DataBind()
        End Sub

    End Class

End Namespace