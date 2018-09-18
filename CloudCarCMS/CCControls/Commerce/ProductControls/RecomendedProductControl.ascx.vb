Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.Product

Namespace CCControls.Commerce.ProductControls
    Public Class RecomendedProductControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load

        End Sub

        Public Property ProductId() As Integer
            Get
                If Not ViewState("ProductID") Is Nothing Then
                    Return CInt(ViewState("ProductID"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal Value As Integer)
                If ViewState("ProductID") Is Nothing Then
                    ViewState.Add("ProductID", Value)
                Else
                    ViewState("ProductID") = Value
                End If

                LoadProducts()
            End Set
        End Property

        Private Sub LoadProducts()
            Dim CurrentRecomendedProducts As List(Of Product)
            CurrentRecomendedProducts = New ProductRecomendationController().GetElements(ProductId, 3)

            If Not CurrentRecomendedProducts Is Nothing AndAlso CurrentRecomendedProducts.Count > 0 Then
                RecomendedProductRepeater.DataSource = CurrentRecomendedProducts
                RecomendedProductRepeater.DataBind()
            End If
        End Sub

    End Class

End Namespace