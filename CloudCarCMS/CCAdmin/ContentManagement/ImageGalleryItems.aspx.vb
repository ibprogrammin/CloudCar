Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement

Namespace CCAdmin.ContentManagement

    Partial Public Class ImageGalleryItems
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub LoadGalleryItem(ByVal ItemId As Integer)
            Try
                Dim Item As ImageGalleryItem = ImageGalleryItemController.GetElement(ItemId)

                hfID.Value = Item.ID.ToString

                txtTitle.Text = Item.Title
                txtDescription.Text = Item.Description
                txtOrder.Text = Item.Order.ToString

                ddlGallery.SelectedValue = Item.GalleryID.ToString

                hfImageID.Value = Item.ImageID.ToString

                If Not Item.ImageID = 0 Then
                    Try
                        lblImageLocation.Text = "/images/db/" & Item.ImageID & "/full/" & CCFramework.Core.PictureController.GetPicture(Item.ImageID).PictureFileName
                        lblImageLocation.Visible = True

                        imgImage.Src = "/images/db/" & Item.ImageID & "/220/" & CCFramework.Core.PictureController.GetPicture(Item.ImageID).PictureFileName
                        imgImage.Visible = True
                    Catch ex As Exception

                    End Try
                End If
            Catch ex As Exception
                lblStatus.Text = ex.ToString
                lblStatus.Visible = True
            End Try
        End Sub

        Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Dim CurrentGalleryItemId As Integer

            If Page.IsValid Then
                Dim Title As String = txtTitle.Text
                Dim Description As String = txtDescription.Text
                Dim Order As Integer = Integer.Parse(txtOrder.Text)
                Dim GalleryID As Integer = Integer.Parse(ddlGallery.SelectedValue)

                Dim ImageID As Integer
                If fuImage.HasFile Then
                    ImageID = CCFramework.Core.ImageFunctions.UploadImage(fuImage)
                Else
                    ImageID = 0
                End If

                If Integer.TryParse(hfID.Value, CurrentGalleryItemId) Then
                    ImageGalleryItemController.Update(CurrentGalleryItemId, Title, Description, GalleryID, ImageID, Order)

                    lblStatus.Text = "Your image gallery item has been saved!"
                    lblStatus.Visible = True
                Else
                    CurrentGalleryItemId = ImageGalleryItemController.Create(Title, Description, GalleryID, ImageID, Order)

                    lblStatus.Text = "Your image gallery item has been created!"
                    lblStatus.Visible = True
                End If

                LoadGalleryItem(CurrentGalleryItemId)
                RefreshDataSources()
            End If
        End Sub

        Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
            ClearControls()
        End Sub

        Private Sub RefreshDataSources()
            ddlGallery.DataSource = ImageGalleryController.GetElements().ToList
            ddlGallery.DataBind()

            gvItems.DataSource = ImageGalleryItemController.GetElements().ToList
            gvItems.DataBind()
        End Sub

        Private Sub ClearControls()
            hfID.Value = Nothing
            hfImageID.Value = Nothing

            ddlGallery.SelectedValue = Nothing

            txtTitle.Text = ""
            txtDescription.Text = ""
            txtOrder.Text = ""

            imgImage.Src = ""
            imgImage.Alt = ""
            imgImage.Visible = False

            lblImageLocation.Text = ""
            lblImageLocation.Visible = False
        End Sub

        Private Sub gvMenuItems_PageIndexChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles gvItems.PageIndexChanged
            gvItems.CurrentPageIndex = e.NewPageIndex

            RefreshDataSources()
        End Sub

        Protected Sub btnDelete_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
            If Not hfID.Value = Nothing Then
                Dim ItemID As Integer = Integer.Parse(hfID.Value)

                If ImageGalleryItemController.Delete(ItemID) = True Then
                    lblStatus.Text = "The selected image gallery item has been deleted permanently!"
                    lblStatus.Visible = True

                    RefreshDataSources()

                    ClearControls()
                Else
                    lblStatus.Text = "An error occured while trying to delete the current image gallery item."
                    lblStatus.Visible = True
                End If
            Else
                lblStatus.Text = "Sorry but you do not have an image gallery item selected."
                lblStatus.Visible = True
            End If
        End Sub

        Protected Sub btnSelect_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            LoadGalleryItem(id)
        End Sub

    End Class
End Namespace