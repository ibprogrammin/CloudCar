Imports CloudCar.CCFramework.ContentManagement.NewsModule

Namespace CCControls.ContentManagement.NewsModule

    Partial Public Class NewsControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                BindNewsRepeater()
            End If
        End Sub

        Protected Sub BindNewsRepeater()
            Try
                NewsRepeater.DataSource = NewsController.GetItems()
                NewsRepeater.DataBind()
            Catch Ex As Exception

            End Try
        End Sub

    End Class

End Namespace