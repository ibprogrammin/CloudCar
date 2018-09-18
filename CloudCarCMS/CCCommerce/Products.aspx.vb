Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.Resources

Namespace CCCommerce

    Partial Public Class ProductsPage
        Inherits CloudCarProductListingPage

        Protected Overrides Sub LoadDetails(ByVal Permalink As String)
            Dim CurrentCategory As Category = New CategoryController().GetElement(Permalink)

            Me.Title = CurrentCategory.BrowserTitle & Settings.SiteTitle

            PageKeywordsMeta.Attributes("content") = CurrentCategory.Keywords
            PageDescriptionMeta.Attributes("content") = CurrentCategory.Description

            If Settings.EnableSSL = True And Settings.FullSSL Then
                PageCanonicalMeta.Attributes("href") = String.Format("https://{0}/Shop/{1}.html", Settings.HostName, Permalink)
            Else
                PageCanonicalMeta.Attributes("href") = String.Format("http://{0}/Shop/{1}.html", Settings.HostName, Permalink)
            End If

            PageHeadingLiteral.Text = CurrentCategory.Name
            PageContentLiteral.Text = CurrentCategory.Details

            PageBreadCrumbControl.CategoryId = CurrentCategory.ID
            
            ProductRepeater.DataSource = FilterProducts(ProductController.GetActiveProductsByCategory(CurrentCategory.ID))
            ProductRepeater.DataBind()
        End Sub

        Protected Overrides Function FilterProducts(Products As List(Of Product)) As List(Of Product)
            Try
                FilterByPrice(Products, PriceRangeDropDown.SelectedValue)
                FilterByColor(Products, ColorDropDown.SelectedValue)
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

            ColorDropDown.Items.Clear()
            ColorDropDown.Items.Add(New ListItem("All", "0"))
            ColorDropDown.AppendDataBoundItems = True
            ColorDropDown.DataSource = New ColourController().GetElements
            ColorDropDown.DataBind()
        End Sub

    End Class

End Namespace