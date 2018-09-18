Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce

    Partial Public Class ProductReviews
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadReviews()
            End If
        End Sub

        Private Sub LoadReviews()
            gvProductReviews.DataSource = New ProductReviewController().GetElements().ToList
            gvProductReviews.DataBind()
        End Sub

        Private Sub gvProductReviews_PageIndexChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles gvProductReviews.PageIndexChanged
            gvProductReviews.CurrentPageIndex = e.NewPageIndex

            LoadReviews()
        End Sub

        Protected Sub btnSelect_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim review As ProductReview = New ProductReviewController().GetElement(id)

            litName.Text = review.name
            litEmail.Text = review.email
            litAvatarURL.Text = review.avatarurl
            litComment.Text = review.comment
            litProduct.Text = ProductController.GetProductName(review.productId)
            litDate.Text = review.timestamp.ToString("dddd MMM d, yyyy")

            phDetails.Visible = True
        End Sub

        Protected Sub btnDelete_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            If New ProductReviewController().Delete(id) Then
                LoadReviews()
            End If
        End Sub

    End Class
End Namespace