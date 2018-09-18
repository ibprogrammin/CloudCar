Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCContentManagement

    Partial Public Class CMSDetails
        Inherits RoutablePage

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Dim permalink As String = (From v In RequestContext.RouteData.Values Where v.Key = "product" Select New With {.id = v.Value}).SingleOrDefault.id.ToString

                If Not permalink = Nothing Then
                    LoadProduct(permalink)
                End If
            End If
        End Sub

        Private Sub LoadProduct(ByVal permalink As String)
            Dim db As New CommerceDataContext

            Dim product As Product = New ProductController().GetElement(permalink)

            imgMainProduct.Src = "/images/db/" & product.HeaderImageID & "/920/" & product.Permalink & ".jpg"
            imgMainProduct.Alt = product.Name

            litProductTitle.Text = product.Name
            litProductDescription.Text = product.Description

            litBreadCrumb.Text = "<a href=""/Home/Products/" & product.Category.Permalink & ".html"" title=""" & product.Category.Name & """ class=""BreadCrumb"">" & product.Category.Name & "</a>" & _
                                 "&raquo; " & product.Name & "</a>"

            hlMoreImages.HRef = "~/CCCommerce/ProductImages.aspx?Product=" & product.ID
            hlMoreImages.Title = "Click here for more images of " & product.Name

            Me.Title = product.Name & " " & product.Category.Name & CCFramework.Core.Settings.SiteTitle

            Dim meta As New StringBuilder()

            meta.Append(<meta name="Keywords" content=<%= product.Keywords %>/>.ToString)
            meta.Append(<meta name="Description" content=<%= product.Description %>/>.ToString)

            CType(MyBase.Master.FindControl("litKeywords"), Literal).Text = meta.ToString

            prcReview.ProductID = product.ID

            rptReviews.DataSource = product.ProductReviews
            rptReviews.DataBind()
        End Sub

    End Class
End NameSpace