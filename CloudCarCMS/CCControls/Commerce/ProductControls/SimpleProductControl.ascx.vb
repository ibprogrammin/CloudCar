Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCControls.Commerce.ProductControls

    Partial Public Class SimpleProductControl
        Inherits UserControl

        Private Const _ShowIcon As Boolean = True

        Private Sub LoadProduct()
            Dim CurrentProduct As Product = New ProductController().GetElement(ProductId)

            NameHyperLink.Text = CurrentProduct.Name
            NameHyperLink.NavigateUrl = String.Format("/Shop/{0}/{1}.html", CurrentProduct.Category.Permalink, CurrentProduct.Permalink)

            If CurrentProduct.TopSeller AndAlso _ShowIcon Then
                ProductImage.Src = String.Format("/images/db/{0}/255/{1}.jpg?overlay=ts", CurrentProduct.DefaultImageID, CurrentProduct.Permalink)
            ElseIf CurrentProduct.Clearance AndAlso _ShowIcon Then
                ProductImage.Src = String.Format("/images/db/{0}/255/{1}.jpg?overlay=sale", CurrentProduct.DefaultImageID, CurrentProduct.Permalink)
            Else
                ProductImage.Src = String.Format("/images/db/{0}/255/{1}.jpg", CurrentProduct.DefaultImageID, CurrentProduct.Permalink)
            End If

            ProductImage.Alt = CurrentProduct.ImageAlt

            ShortDescriptionLiteral.Text = ApplicationFunctions.StripShortString(CurrentProduct.Description, 120)

            PriceLiteral.Text = CurrentProduct.Price.ToString("C")
        End Sub

        Public Property ProductId() As Integer
            Get
                If Not ViewState("ProductId") Is Nothing Then
                    Return CInt(ViewState("ProductId"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal Value As Integer)
                If ViewState("ProductId") Is Nothing Then
                    ViewState.Add("ProductId", Value)
                Else
                    ViewState("ProductId") = Value
                End If

                LoadProduct()
            End Set
        End Property

    End Class

End Namespace