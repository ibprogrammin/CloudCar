Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCCommerce

    Partial Public Class ProductImages
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Settings.StoreEnabled Then
                Response.Redirect(CCFramework.Core.Settings.StoreDisabledPage)
            End If

            If Not Page.IsPostBack Then
                Dim productID As Integer

                If Integer.TryParse(Request.QueryString("Product"), productID) Then
                    RefreshDataSources(productID)
                End If
            End If
        End Sub

        Private Sub RefreshDataSources(ByVal productID As Integer)
            Dim imageTable As New DataTable("ProductImages")

            imageTable.Columns.Add("ImageID")
            imageTable.Columns.Add("Description")

            Dim db As New CommerceDataContext

            Dim product As Product = New ProductController().GetElement(productID)
            Dim colorImages As List(Of ProductColor) = New ProductColourController().GetColors(productID).ToList
            Dim additionalImages As List(Of ProductImage) = (From pi In db.ProductImages Where pi.ProductID = productID Select pi).ToList

            Dim productName As String = ""

            If Not product Is Nothing Then
                Dim dr1 As DataRow = imageTable.NewRow

                dr1.Item("ImageID") = product.DefaultImageID
                dr1.Item("Description") = product.Name

                imageTable.Rows.Add(dr1)

                Dim dr2 As DataRow = imageTable.NewRow

                dr2.Item("ImageID") = product.HeaderImageID
                dr2.Item("Description") = product.Name

                imageTable.Rows.Add(dr2)

                imgMain.ImageUrl = "/images/db/" & product.HeaderImageID & "/640/image" & product.HeaderImageID & ".jpg"
                imgMain.AlternateText = product.ImageAlt
                lblDescription.Text = product.Name

                litMetaKeywords.Text = <meta name="Keywords" content=<%= product.Keywords %>/>.ToString
                litMetaDescription.Text = <meta name="Description" content=<%= product.Description %>/>.ToString

                MyBase.Title = product.BrowserTitle & " Image Gallery" & CCFramework.Core.Settings.SiteTitle

                litTitle.Text = product.Name
                productName = product.Name
            End If

            If colorImages.Count > 0 Then
                For Each item In colorImages
                    Dim dr As DataRow = imageTable.NewRow

                    dr.Item("ImageID") = item.ImageID
                    dr.Item("Description") = item.Color.Name & " " & productName

                    imageTable.Rows.Add(dr)
                Next
            End If

            If additionalImages.Count > 0 Then
                For Each item In additionalImages
                    Dim dr As DataRow = imageTable.NewRow

                    dr.Item("ImageID") = item.ImageID
                    dr.Item("Description") = item.Description

                    imageTable.Rows.Add(dr)
                Next
            End If

            rptImages.DataSource = imageTable
            rptImages.DataBind()
        End Sub

        Protected Sub btnImage_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim senderImage As ImageButton = CType(sender, ImageButton)

            imgMain.ImageUrl = String.Format("/images/db/{0}/640/image.jpg", e.CommandArgument.ToString)
            imgMain.AlternateText = senderImage.AlternateText
            lblDescription.Text = senderImage.AlternateText
        End Sub

    End Class
End NameSpace