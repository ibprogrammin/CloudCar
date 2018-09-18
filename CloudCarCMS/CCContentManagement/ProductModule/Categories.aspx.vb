Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCControls.Commerce.ProductControls

Namespace CCContentManagement

    Partial Public Class CMSProductCategories
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not CCFramework.Core.Settings.StoreEnabled Then
                Response.Redirect(CCFramework.Core.Settings.StoreDisabledPage)
            End If

            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub RefreshDataSources()
            Me.Title = CCFramework.Core.Settings.CategoryTitle & CCFramework.Core.Settings.SiteTitle

            Dim db As New CommerceDataContext

            dlBrands.DataSource = New BrandController().GetBrandsWithProducts
            dlBrands.DataBind()

            rptCategories.DataSource = New CategoryController().GetCategoriesWithProducts()
            rptCategories.DataBind()

            'TODO Uncomment these to enable them
            'Dim topSellers As List(Of Product) = SMCommerce.ProductController.GetProductsAsTopSellersFunc(db).ToList ' = From p In db.Products Where p.TopSeller = True And p.Active = True Select p
            'Dim clearance As List(Of Product) = SMCommerce.ProductController.GetProductsAsClearanceFunc(db).ToList ' = From p In db.Products Where p.Clearance = True And p.Active = True Select p

            'If topSellers.Count > 0 Then
            '    dlTopSellers.DataSource = topSellers
            '    dlTopSellers.DataBind()
            '    dlTopSellers.Visible = True
            'End If

            'If clearance.Count > 0 Then
            '    dlClearance.DataSource = clearance
            '    dlClearance.DataBind()
            '    dlClearance.Visible = True
            'End If

            db.Dispose()
        End Sub

        Protected Function GetCategoryProducts(ByVal CategoryID As Integer) As String
            Dim db As New CommerceDataContext

            Dim take As Integer = CCFramework.Core.Settings.ProductNamesToDisplay
            Dim count As Integer = ProductController.GetActiveProductsFromCategoryIDCountFunc(db, CategoryID)
            Dim index As Integer = New Random().Next(count)
            Dim difference As Integer = count - index

            If count > take And difference < take Then
                index = index - (take - difference)
            ElseIf count <= take Then
                index = 0
            End If

            Dim products As List(Of Product) = ProductController.GetActiveProductsFromCategoryIDFunc(db, CategoryID).Skip(index).Take(take).ToList

            Dim productString As New StringBuilder
            Dim x As Integer = 1

            For Each p In products
                If Not x = 3 Then
                    If p.Name.Length > 20 Then
                        productString.Append(p.Name.Substring(0, 20) & ", ")
                    Else
                        productString.Append(p.Name & ", ")
                    End If
                Else
                    If p.Name.Length > 20 Then
                        productString.Append(p.Name.Substring(0, 20) & "...")
                    Else
                        productString.Append(p.Name & "...")
                    End If
                End If

                x += 1
            Next

            Return productString.ToString
        End Function

        Protected Function GetBrandProducts(ByVal BrandID As Integer) As String
            Dim db As New CommerceDataContext

            Dim take As Integer = CCFramework.Core.Settings.ProductNamesToDisplay
            Dim count As Integer = ProductController.GetActiveProductsFromBrandIDCountFunc(db, BrandID)
            Dim index As Integer = New Random().Next(count)
            Dim difference As Integer = count - index

            If count > take And difference < take Then
                index = index - (take - difference)
            ElseIf count <= take Then
                index = 0
            End If

            Dim products As List(Of Product) = ProductController.GetActiveProductsFromBrandIDFunc(db, BrandID).Skip(index).Take(3).ToList

            Dim productString As New StringBuilder
            Dim x As Integer = 1

            For Each p In products
                If Not x = 3 Then
                    If p.Name.Length > 20 Then
                        productString.Append(p.Name.Substring(0, 20) & ", ")
                    Else
                        productString.Append(p.Name & ", ")
                    End If
                Else
                    If p.Name.Length > 20 Then
                        productString.Append(p.Name.Substring(0, 20) & "...")
                    Else
                        productString.Append(p.Name & "...")
                    End If
                End If

                x += 1
            Next

            Return productString.ToString
        End Function

        Protected Function GetCategoryImage(ByVal CategoryID As Integer) As Integer
            Dim db As New CommerceDataContext

            Dim count As Integer = ProductController.GetActiveProductsWithImagesFromCategoryIDCountFunc(db, CategoryID)

            If count > 0 Then
                Dim index As Integer = New Random().Next(count)
                Dim product As Product = ProductController.GetActiveProductsWithImagesFromCategoryIDFunc(db, CategoryID).Skip(index).FirstOrDefault

                Return product.DefaultImageID
            Else
                Return 0
            End If

        End Function

        'Protected Sub pcProduct_CartItemAdded(ByVal sender As Object, ByVal e As EventArgs)
        '    Dim sdc As ShoppingDetailsControl = CType(Me.Master.FindControl("MasterShoppingDetailsControl"), ShoppingDetailsControl)

        '    sdc.LoadDetails()

        '    Response.Redirect("~/CCCommerce/ShoppingCart.aspx")
        'End Sub

        Protected Sub pcProduct_CartItemAdded(ByVal sender As Object, ByVal e As EventArgs)
            'Dim sdc As ShoppingDetailsControl = CType(Me.Master.FindControl("MasterShoppingDetailsControl"), ShoppingDetailsControl)

            'sdc.LoadDetails()

            'lblStatus.Text = ""

            Response.Redirect("~/CCCommerce/ShoppingCart.aspx")
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