Imports CloudCar.CCFramework.Commerce

Namespace CCControls.Commerce

    Public Class ColorControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadColors()
            End If
        End Sub

        Private Sub LoadColors()
            ColorRepeater.DataSource = ColourController.GetColorsWithProducts()
            ColorRepeater.DataBind()
        End Sub

    End Class
End Namespace