Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCContentManagement

    Partial Public Class CMSProductsPage
        Inherits RoutablePage

        Private _categoryID As Integer
        Private _brandID As Integer

        Private DisplayBrand As Boolean = False
        Private DisplayCategory As Boolean = False

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not CCFramework.Core.Settings.StoreEnabled Then
                Response.Redirect(CCFramework.Core.Settings.StoreDisabledPage)
            End If

            If Not Page.IsPostBack Then
                Dim permalink As String = (From v In RequestContext.RouteData.Values Where v.Key = "category" Select New With {.id = v.Value}).SingleOrDefault.id.ToString

                If Not permalink Is Nothing Then
                    LoadCategory(permalink)
                End If

                'If Integer.TryParse(Request.QueryString("Category"), _categoryID) Then
                'LoadCategory()
                'End If
                'If Integer.TryParse(Request.QueryString("Brand"), _brandID) Then
                'LoadBrand()
                'End If
            End If

            lblStatus.Visible = False
        End Sub

        Private Sub LoadCategory(ByVal permalink As String)
            Dim category As Category = New CategoryController().GetElement(permalink)

            _categoryID = category.ID
            Me.Title = category.Name & CCFramework.Core.Settings.SiteTitle

            Dim meta As New StringBuilder()

            meta.Append(<meta name="Keywords" content=<%= category.Keywords %>/>.ToString)
            meta.Append(<meta name="Description" content=<%= category.Description %>/>.ToString)

            CType(MyBase.Master.FindControl("litKeywords"), Literal).Text = meta.ToString

            litCategoryTitle.Text = category.Name
            'litPageContent.Text = category.PageContent
            'litSideContent.Text = category.SideContent
            'litBottomContent.Text = category.BottomContent
            litBreadCrumb.Text = category.Name

            'If Not category.IconURL = String.Empty Then
            'imgIcon.Visible = True
            'imgIcon.Src = category.IconURL
            'imgIcon.Alt = category.Name
            'End If

            Dim db As New CommerceDataContext

            rptProducts.DataSource = ProductController.GetActiveProductsFromCategoryIDFunc(db, _categoryID).ToList
            rptProducts.DataBind()
        End Sub

        Private Sub LoadCategory()
            Dim category As Category = New CategoryController().GetElement(_categoryID)

            Me.Title = category.Name & CCFramework.Core.Settings.SiteTitle

            Dim meta As New StringBuilder()

            meta.Append(<meta name="Keywords" content=<%= category.Keywords %>/>.ToString)
            meta.Append(<meta name="Description" content=<%= category.Description %>/>.ToString)

            CType(MyBase.Master.FindControl("litKeywords"), Literal).Text = meta.ToString

            litCategoryTitle.Text = category.Name
            'litPageContent.Text = category.PageContent
            'litSideContent.Text = category.SideContent
            'litBottomContent.Text = category.BottomContent

            Dim db As New CommerceDataContext

            rptProducts.DataSource = ProductController.GetActiveProductsFromCategoryIDFunc(db, _categoryID).ToList ' From p In db.Products Where p.CategoryID = _categoryID And p.Active = True Select p
            rptProducts.DataBind()
        End Sub

        Private Sub LoadBrand()
            Dim brand As Brand = New BrandController().GetElement(_brandID)

            Me.Title = brand.Name & CCFramework.Core.Settings.SiteTitle

            Dim meta As New StringBuilder()

            meta.Append(<meta name="Keywords" content=<%= brand.Keywords %>/>.ToString)
            meta.Append(<meta name="Description" content=<%= brand.Description %>/>.ToString)

            CType(MyBase.Master.FindControl("litKeywords"), Literal).Text = meta.ToString

            'PageHeadingLiteral.Text = brand.Name
            'litPageDescription.text = brand.Description

            Dim db As New CommerceDataContext

            rptProducts.DataSource = From p In db.Products Where p.BrandID = _brandID And p.Active = True Select p
            rptProducts.DataBind()
        End Sub

    End Class
End NameSpace