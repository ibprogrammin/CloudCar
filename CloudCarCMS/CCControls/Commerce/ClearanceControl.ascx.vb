Imports CloudCar.CCFramework.Commerce

Namespace CCControls.Commerce

    Partial Public Class ClearanceControl
        Inherits ProductListControl

        Protected Overrides Sub LoadProducts()
            ListControlProducts = ProductController.GetActiveClearanceItems(ProductListDataContext).Take(ProductListControlProperties.Count).ToList
        End Sub

    End Class

End Namespace