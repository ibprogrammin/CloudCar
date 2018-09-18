Imports AjaxControlToolkit
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCContentManagement.ClientForms

    Partial Public Class Menu1
        Inherits RoutablePage

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadPage()
            End If
        End Sub

        Private Sub LoadPage()
            Dim contentpage As ContentPage = CCFramework.ContentManagement.ContentPageController.GetPageFromLink(CCFramework.Core.Settings.MenuPage)

            litBreadCrumb.Text = CCFramework.ContentManagement.ContentPageController.GetBreadCrumb(contentpage.id, "Home")

            litHeading.Text = contentpage.pageTitle
            litContent.Text = contentpage.pageContent
            litScripts.Text = contentpage.script

            If contentpage.headerImageID.HasValue And Not contentpage.headerImageID.Value = 0 Then
                imgHeadingImage.Alt = contentpage.contentTitle
                imgHeadingImage.Src = "/images/db/" & contentpage.headerImageID & "/920/" & contentpage.permalink & ".jpg"
                imgHeadingImage.Visible = True
            End If

            PageKeywordsMeta.Attributes("content") = contentpage.keywords
            PageDescriptionMeta.Attributes("content") = contentpage.description
            
            MyBase.Title = contentpage.pageTitle & CCFramework.Core.Settings.SiteTitle


            Dim db As New CommerceDataContext
            Dim menuCategories = From mc In db.MenuCategories Select mc

            For Each item As MenuCategory In menuCategories
                Dim tp As New TabPanel()

                Dim id As Integer = item.id

                tp.ID = "tp" & item.category
                tp.HeaderText = <h3><%= item.category %></h3>.ToString

                Dim categoryItems = From mci In db.MenuCategoryItems Where mci.menuCategoryID = id Select mci

                For Each catItem As MenuCategoryItem In categoryItems
                    Dim lit As New Literal

                    lit.Text = <h2 style="float: right; color: #666;"><%= catItem.Product.Price.ToString("C") %></h2>.ToString
                    lit.Text &= <h2><b><a href=<%= "/product/" & catItem.Product.Category.Permalink & "/" & catItem.Product.Permalink & ".html" %> title=<%= catItem.Product.Name %>><%= catItem.Product.Name %></a></b></h2>.ToString
                    lit.Text &= <p style="width: 400px; float: left; margin-bottom: 20px;"><%= catItem.Product.Description %></p>.ToString
                    lit.Text &= <br/>.ToString

                    tp.Controls.Add(lit)

                    Dim currentRating As Integer = ProductReviewController.GetProductRating(catItem.productID)
                    If currentRating = 0 Then
                        Dim litNoRating As New Literal
                        litNoRating.Text = <p style="float: right;">Sorry! This item is not rated yet!</p>.ToString
                        litNoRating.Text &= <br style="clear: both;"/>.ToString

                        tp.Controls.Add(litNoRating)
                    Else
                        Dim rating As New Rating

                        'Mary 20/06/2010: ID is a required property
                        rating.ID = "rating-" + catItem.id.ToString()

                        rating.RatingDirection = RatingDirection.LeftToRightTopToBottom
                        rating.MaxRating = 5
                        rating.CurrentRating = currentRating
                        rating.Style.Add("float", "right")
                        rating.EmptyStarCssClass = "StarEmpty"
                        rating.FilledStarCssClass = "StarFilled"
                        rating.WaitingStarCssClass = "StarWaiting"
                        rating.StarCssClass = "RatingItem"
                        rating.CssClass = "RatingStar"

                        Dim break As New HtmlGenericControl
                        break.InnerHtml = "<br style='clear: both;' />"

                        tp.Controls.Add(rating)
                        tp.Controls.Add(break)
                    End If

                Next

                tcMenu.Controls.Add(tp)
            Next

            tcMenu.ActiveTabIndex = 0
        End Sub

    End Class
End NameSpace