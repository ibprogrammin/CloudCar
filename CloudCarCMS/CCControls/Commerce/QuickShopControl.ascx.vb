Imports CloudCar.CCFramework.Commerce

Namespace CCControls.Commerce

    Partial Public Class QuickShopControl
        Inherits UserControl

        Protected Overrides Sub OnLoad(ByVal E As EventArgs)
            If Not Page.IsPostBack Then
                LoadBrands()
            End If
        End Sub

        Private Sub LoadBrands()
            Dim CurrentBrandController = New BrandController()

            BrandsRepeater.DataSource = CurrentBrandController.GetElements()
            BrandsRepeater.DataBind()

            CurrentBrandController.Dispose()
        End Sub

    End Class

End Namespace