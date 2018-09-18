Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCControls.Commerce.ProductControls

    Partial Public Class BrandCategoriesControl
        Inherits UserControl

        Private Sub LoadBrand()
            Dim Categories As List(Of Category) = BrandController.GetBrandCategories(BrandID)

            rptCategories.DataSource = Categories
            rptCategories.DataBind()
        End Sub

        Public Property BrandId() As Integer
            Get
                If Not ViewState("BrandID") Is Nothing Then
                    Return CInt(ViewState("BrandID"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal value As Integer)
                If ViewState("BrandID") Is Nothing Then
                    ViewState.Add("BrandID", value)
                Else
                    ViewState("BrandID") = value
                End If

                LoadBrand()
            End Set
        End Property

    End Class

End Namespace