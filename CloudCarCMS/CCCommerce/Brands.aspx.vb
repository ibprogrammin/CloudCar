Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.Resources

Namespace CCCommerce

    Partial Public Class Brands
        Inherits CloudCarProductListingPage

        Protected Overrides Sub LoadDetails(ByVal Permalink As String)
            Dim CurrentBrandController As New BrandController
            Dim CurrentBrand As Brand = CurrentBrandController.GetElement(Permalink)
            CurrentBrandController.Dispose()

            Me.Title = String.Format("{0}{1}", CurrentBrand.Name, Settings.SiteTitle)

            PageKeywordsMeta.Attributes("content") = CurrentBrand.Keywords
            PageDescriptionMeta.Attributes("content") = CurrentBrand.Description

            If Settings.EnableSSL = True And Settings.FullSSL Then
                PageCanonicalMeta.Attributes("href") = String.Format("https://{0}/Shop/Brand/{1}.html", Settings.HostName, CurrentBrand.Permalink)
            Else
                PageCanonicalMeta.Attributes("href") = String.Format("http://{0}/Shop/Brand/{1}.html", Settings.HostName, CurrentBrand.Permalink)
            End If

            PageHeadingLiteral.Text = CurrentBrand.Name
            PageContentLiteral.Text = CurrentBrand.Description

            Dim BreadCrumbStringBuilder As New StringBuilder

            BreadCrumbStringBuilder.Append(String.Format("<a href=""/"">Home</a> &raquo; "))
            BreadCrumbStringBuilder.Append(String.Format("<a href=""/Shop/Index.html"">Shop</a> &raquo; "))
            BreadCrumbStringBuilder.Append(String.Format("{0}", CurrentBrand.Name))

            BreadCrumbLiteral.Text = BreadCrumbStringBuilder.ToString()

            ProductRepeater.DataSource = FilterProducts(ProductController.GetActiveProductsByBrand(CurrentBrand.ID))
            ProductRepeater.DataBind()
        End Sub

        Protected Overrides Function FilterProducts(Products As List(Of Product)) As List(Of Product)
            Try
                FilterByPrice(Products, PriceRangeDropDown.SelectedValue)
                FilterByColor(Products, ColorDropDown.SelectedValue)

                ProductCountLiteral.Text = String.Format("Displaying {0} products", Products.Count)

                FilterProducts = Products
            Catch Ex As Exception

                ProductCountLiteral.Text = String.Format("Displaying {0} products", 0)
                StatusMessageLabel.Text = CommerceResources.CategoryNoProductsMessage
                StatusMessageLabel.Visible = True

                FilterProducts = Nothing
            End Try

        End Function

        Protected Overrides Sub RefreshDataSources()
            ColorDropDown.Items.Clear()
            ColorDropDown.Items.Add(New ListItem("All", "0"))
            ColorDropDown.AppendDataBoundItems = True
            ColorDropDown.DataSource = New ColourController().GetElements
            ColorDropDown.DataBind()
        End Sub

    End Class

End Namespace