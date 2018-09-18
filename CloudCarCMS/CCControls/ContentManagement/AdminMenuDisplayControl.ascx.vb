Imports CloudCar.CCFramework.Model

Namespace CCControls.ContentManagement

    Partial Public Class AdminMenuDisplayControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadMenu()
            End If
        End Sub

        Private Sub LoadMenu()
            Dim MenuItems As List(Of MenuItem) = CCFramework.ContentManagement.MenuItemController.GetOrderedElements().Where(Function(f) f.Menu = "Admin").ToList

            rptMenuItems.DataSource = MenuItems
            rptMenuItems.DataBind()
        End Sub

    End Class

End Namespace