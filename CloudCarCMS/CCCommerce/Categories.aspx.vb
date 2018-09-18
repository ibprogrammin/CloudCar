Imports CloudCar.CCContentManagement
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCControls.Commerce.ProductControls
Imports CloudCar.CCFramework.Core

Namespace CCCommerce

    Partial Public Class Categories
        Inherits CloudCarContentPage

        Public Sub New()
            MyBase.New()
            Permalink = Settings.ShopPage
        End Sub

        Protected Overrides Sub OnLoad(ByVal Args As EventArgs)
            If Not Settings.StoreEnabled Then
                Response.Redirect(Settings.StoreDisabledPage)
            End If

            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub RefreshDataSources()
            'Title = Settings.CategoryTitle & Settings.SiteTitle

            If Settings.EnableSSL = True And Settings.FullSSL Then
                PageCanonicalMeta.Attributes("href") = String.Format("https://{0}/Shop/Index.html", Settings.HostName)
            Else
                PageCanonicalMeta.Attributes("href") = String.Format("http://{0}/Shop/Index.html", Settings.HostName)
            End If
        End Sub

        Protected Function GetCategoryProducts(ByVal CategoryId As Integer) As String
            Dim CurrentDataContext As New CommerceDataContext

            GetCategoryProducts = BuildProductListString(ProductController.GetActiveProductsFromCategoryIdFunc(CurrentDataContext, CategoryId).ToList, ProductController.GetActiveProductsFromCategoryIdCountFunc(CurrentDataContext, CategoryId))

            CurrentDataContext.Dispose()
        End Function

        Protected Function GetBrandProducts(ByVal BrandId As Integer) As String
            Dim CurrentDataContext As New CommerceDataContext

            GetBrandProducts = BuildProductListString(ProductController.GetActiveProductsFromBrandIdFunc(CurrentDataContext, BrandId).ToList, ProductController.GetActiveProductsFromBrandIdCountFunc(CurrentDataContext, BrandId))

            CurrentDataContext.Dispose()
        End Function

        Private Function BuildProductListString(CurrentProducts As List(Of Product), ActiveProductCount As Integer) As String
            Dim CurrentRandomStartIndex As Integer = New Random().Next(ActiveProductCount)
            Dim CurrentDifference As Integer = ActiveProductCount - CurrentRandomStartIndex

            If ActiveProductCount > Settings.ProductNamesToDisplay And CurrentDifference < Settings.ProductNamesToDisplay Then
                CurrentRandomStartIndex = CurrentRandomStartIndex - (Settings.ProductNamesToDisplay - CurrentDifference)
            ElseIf ActiveProductCount <= Settings.ProductNamesToDisplay Then
                CurrentRandomStartIndex = 0
            End If

            Dim CurrentProductStringBuilder As New StringBuilder
            Dim CurrentIndex As Integer = 1

            CurrentProducts = CurrentProducts.Skip(CurrentRandomStartIndex).Take(Settings.ProductNamesToDisplay).ToList

            For Each p In CurrentProducts
                If Not CurrentIndex = 3 Then
                    If p.Name.Length > 20 Then
                        CurrentProductStringBuilder.Append(p.Name.Substring(0, 20) & ", ")
                    Else
                        CurrentProductStringBuilder.Append(p.Name & ", ")
                    End If
                Else
                    If p.Name.Length > 20 Then
                        CurrentProductStringBuilder.Append(p.Name.Substring(0, 20) & "...")
                    Else
                        CurrentProductStringBuilder.Append(p.Name & "...")
                    End If
                End If

                CurrentIndex += 1
            Next

            Return CurrentProductStringBuilder.ToString
        End Function

        Protected Function GetCategoryImage(ByVal CategoryId As Integer) As Integer
            Dim db As New CommerceDataContext

            Dim count As Integer = ProductController.GetActiveProductsWithImagesFromCategoryIdCountFunc(db, CategoryID)

            If count > 0 Then
                Dim index As Integer = New Random().Next(count)
                Dim product As Product = ProductController.GetActiveProductsWithImagesFromCategoryIdFunc(db, CategoryID).Skip(index).FirstOrDefault

                Return product.DefaultImageID
            Else
                Return 0
            End If

        End Function

        Protected Sub pcProduct_OutOfStock(ByVal Sender As Object, ByVal Args As ProductControlEventArgs)

            If Args.Inventory = 0 Then
                lblStatus.Text = String.Format("<p style=""color: red;"">Sorry! We currently have none of those items in stock.</p>")
            Else
                lblStatus.Text = String.Format("<p style=""color: red;"">Sorry! We currently only have ( {0} ) items in stock.</p>", Args.Inventory)
            End If

            lblStatus.Visible = True

        End Sub

        Protected Sub pcProduct_AddMembership(ByVal sender As Object, ByVal e As ProductControlEventArgs)
            'Dim sdc As ShoppingDetailsControl = CType(Me.Master.FindControl("MasterShoppingDetailsControl"), ShoppingDetailsControl)

            'sdc.LoadDetails()

            'lblStatus.Text = ""

            If e.IsRegisteredUser Then
                If Not e.HasMembership Then
                    Response.Redirect("~/CCCommerce/Membership/MemberApp.aspx?ProductID=" & e.ProductID)
                Else
                    'lblStatus.Text = "You already have a membership set up for this account or a membership in your shopping cart. If you would like to purchase a membership for another family member, you must sign out, or create a new account."
                End If
            Else
                If Not e.HasMembership Then
                    Response.Redirect("~/CCCommerce/Membership/MemberApp.aspx?ProductID=" & e.ProductID)
                Else
                    'lblStatus.Text = "You already have a membership set up for this account or a membership in your shopping cart. If you would like to purchase a membership for another family member, you must sign out, or create a new account."
                End If

            End If

        End Sub
        
    End Class

End NameSpace