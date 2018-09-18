Imports CloudCar.CCFramework.Commerce

Namespace CCControls.Commerce

    Public Class LatestControl
        Inherits ProductListControl

        Protected Overrides Sub LoadProducts()
            ListControlProducts = ProductController.GetLatestProductsFunc(ProductListDataContext, ProductListControlProperties.Count).ToList
        End Sub

    End Class

End Namespace