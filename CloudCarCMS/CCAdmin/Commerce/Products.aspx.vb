Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce

    Partial Public Class Products
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
                SearchEnabled = False
            End If

            lblMessage.Visible = False
        End Sub

        Protected Property SearchEnabled() As Boolean
            Get
                If ViewState("SearchEnabled") Is Nothing Then
                    ViewState.Add("SearchEnabled", False)
                End If

                Return CBool(ViewState("SearchEnabled"))
            End Get
            Set(ByVal value As Boolean)
                If ViewState("SearchEnabled") Is Nothing Then
                    ViewState.Add("SearchEnabled", value)
                Else
                    ViewState("SearchEnabled") = value
                End If
            End Set
        End Property

        Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            SearchProducts()
        End Sub

        Protected Sub btnMakeActive_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim ProductID As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim product As New ProductController()
            product.MakeActive(ProductID, True)
        End Sub

        Protected Sub btnMakeInactive_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim ProductID As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim product As New ProductController()
            product.MakeActive(ProductID, False)
        End Sub

        Private Sub SearchProducts()
            Dim pList As List(Of Product) = New ProductController().GetElements()

            Dim searchProductID As Integer
            If Integer.TryParse(txtProductNumber.Text, searchProductID) Then pList = pList.Where(Function(p) p.ID = searchProductID).ToList

            If Not txtProductName.Text = String.Empty Then pList = pList.Where(Function(p) p.Name.ToLower.Contains(txtProductName.Text.ToLower)).ToList

            Dim isProductActive As Boolean
            If Boolean.TryParse(ddlActiveFilter.SelectedValue, isProductActive) Then pList = pList.Where(Function(p) p.Active = isProductActive).ToList

            Dim productBrandID As Integer
            If Integer.TryParse(ddlBrands.SelectedValue, productBrandID) Then pList = pList.Where(Function(p) p.BrandID = productBrandID).ToList

            Dim productCategoryID As Integer
            If Integer.TryParse(ddlCategories.SelectedValue, productCategoryID) Then pList = pList.Where(Function(p) p.CategoryID = productCategoryID).ToList

            lvProducts.DataSource = pList
            lvProducts.DataBind()

            SearchEnabled = True
        End Sub

        Private Sub dpPagerBottom_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles dpPagerBottom.PreRender
            If Not SearchEnabled Then
                RefreshDataSources()
            Else
                SearchProducts()
            End If
        End Sub

        Private Sub RefreshDataSources()
            lvProducts.DataSource = New ProductController().GetElements().ToList
            lvProducts.DataBind()

            ddlBrands.Items.Clear()
            ddlBrands.Items.Add(New ListItem("Brands", ""))
            ddlBrands.AppendDataBoundItems = True
            ddlBrands.DataSource = New BrandController().GetElements()
            ddlBrands.DataBind()

            ddlCategories.Items.Clear()
            ddlCategories.Items.Add(New ListItem("Category", ""))
            ddlCategories.AppendDataBoundItems = True
            ddlCategories.DataSource = New CategoryController().GetElements()
            ddlCategories.DataBind()
        End Sub

        Protected Function GetDescription(ByVal description As String) As String
            Return CCFramework.Core.ApplicationFunctions.StripShortString(description, 160)
        End Function

    End Class
End Namespace