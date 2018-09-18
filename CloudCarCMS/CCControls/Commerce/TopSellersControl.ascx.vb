Imports CloudCar.CCFramework.Commerce

Namespace CCControls.Commerce

    Public Class TopSellersControl
        Inherits ProductListControl

        Protected Overrides Sub LoadProducts()
            ListControlProducts = ProductController.GetProductsAsTopSellersFunc(ProductListDataContext).Take(ProductListControlProperties.Count).ToList
        End Sub

    End Class

End Namespace