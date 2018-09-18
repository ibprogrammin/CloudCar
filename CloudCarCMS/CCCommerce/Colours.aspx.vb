Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.Resources

Namespace CCCommerce

    Partial Public Class Colours
        Inherits CloudCarProductListingPage

        Protected Overrides Sub LoadDetails(ByVal Permalink As String)
            Dim CurrentColor As Color = New ColourController().GetElement(Permalink)

            Me.Title = CurrentColor.Name & Settings.SiteTitle

            PageKeywordsMeta.Attributes("content") = CurrentColor.Name
            PageDescriptionMeta.Attributes("content") = CurrentColor.Name

            If Settings.EnableSSL = True And Settings.FullSSL Then
                PageCanonicalMeta.Attributes("href") = String.Format("https://{0}/Colour/{1}.html", Settings.HostName, Permalink)
            Else
                PageCanonicalMeta.Attributes("href") = String.Format("http://{0}/Colour/{1}.html", Settings.HostName, Permalink)
            End If

            PageHeadingLiteral.Text = CurrentColor.Name
            PageContentLiteral.Text = ""

            PageBreadCrumbControl.ColorId = CurrentColor.ID

            ProductRepeater.DataSource = FilterProducts(ProductController.GetActiveProductsByColor(CurrentColor.ID))
            ProductRepeater.DataBind()
        End Sub

        Protected Overrides Function FilterProducts(Products As List(Of Product)) As List(Of Product)
            Try
                FilterByPrice(Products, PriceRangeDropDown.SelectedValue)
                FilterByBrand(Products, BrandDropDown.SelectedValue)

                ProductCountLiteral.Text = String.Format("Displaying {0} products", Products.Count)

                FilterProducts = Products
            Catch Ex As Exception

                StatusMessageLabel.Text = CommerceResources.CategoryNoProductsMessage
                StatusMessageLabel.Visible = True

                FilterProducts = Nothing
            End Try

        End Function

        Protected Overrides Sub RefreshDataSources()
            BrandDropDown.Items.Clear()
            BrandDropDown.Items.Add(New ListItem("All", "0"))
            BrandDropDown.AppendDataBoundItems = True
            BrandDropDown.DataSource = New BrandController().GetElements
            BrandDropDown.DataBind()
        End Sub

    End Class

End NameSpace