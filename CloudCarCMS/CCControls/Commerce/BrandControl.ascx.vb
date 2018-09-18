Imports CloudCar.CCFramework.Commerce

Namespace CCControls.Commerce

    Public Class BrandControl
        Inherits UserControl

        Protected Overrides Sub OnLoad(ByVal E As EventArgs)
            If Not Page.IsPostBack Then
                LoadBrands()
            End If
        End Sub

        Private Sub LoadBrands()
            If Visible Then
                Dim CurrentBrandController As New BrandController

                BrandRepeater.DataSource = CurrentBrandController.GetBrandsWithProducts
                BrandRepeater.DataBind()

                CurrentBrandController.Dispose()
            End If
        End Sub

    End Class
End Namespace