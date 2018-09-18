Imports CloudCar.CCFramework.ContentManagement.CareerModule

Namespace CCControls.ContentManagement.CareerModule
    Public Class CareerListControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadCareers()
            End If
        End Sub

        Private Sub LoadCareers()
            CareersRepeater.DataSource = CareerController.GetElements()
            CareersRepeater.DataBind()
        End Sub

    End Class
End Namespace