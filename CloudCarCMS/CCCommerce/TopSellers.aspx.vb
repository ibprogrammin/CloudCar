Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.ContentManagement
Imports CloudCar.Resources

Namespace CCCommerce

    Partial Public Class TopSellers
        Inherits CloudCarProductListingPage

        Public Sub New()
            RouteData.Values.Add("permalink", Settings.TopSellersPage)
        End Sub

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

        Protected Overrides Sub LoadDetails(ByVal Permalink As String)
            Dim CurrentPage As ContentPage = ContentPageController.GetPageFromLink(Permalink)

            Title = String.Format("{0}{1}", CurrentPage.pageTitle, Settings.SiteTitle)

            PageKeywordsMeta.Attributes("content") = CurrentPage.keywords
            PageDescriptionMeta.Attributes("content") = CurrentPage.description

            If Settings.EnableSSL = True And Settings.FullSSL Then
                PageCanonicalMeta.Attributes("href") = String.Format("https://{0}/Shop/{1}.html", Settings.HostName, Permalink)
            Else
                PageCanonicalMeta.Attributes("href") = String.Format("http://{0}/Shop/{1}.html", Settings.HostName, Permalink)
            End If

            PageHeadingLiteral.Text = CurrentPage.contentTitle
            PageContentLiteral.Text = CurrentPage.pageContent

            PageBreadCrumbControl.PageId = CurrentPage.id

            Dim CurrentDataContext As New CommerceDataContext

            ProductRepeater.DataSource = FilterProducts(ProductController.GetAllActiveTopSellerItems(CurrentDataContext).ToList)
            ProductRepeater.DataBind()

            CurrentDataContext.Dispose()
        End Sub

        Protected Overrides Function FilterProducts(ByVal Products As List(Of Product)) As List(Of Product)
            Try
                FilterByColor(Products, ColorDropDown.SelectedValue)
                FilterByBrand(Products, BrandDropDown.SelectedValue)
                FilterByPrice(Products, PriceRangeDropDown.SelectedValue)

                ProductCountLiteral.Text = String.Format("Displaying {0} products", Products.Count)

                FilterProducts = Products
            Catch Ex As Exception

                StatusMessageLabel.Text = CommerceResources.CategoryNoProductsMessage
                StatusMessageLabel.Visible = True

                FilterProducts = Nothing
            End Try
        End Function

    End Class

End NameSpace