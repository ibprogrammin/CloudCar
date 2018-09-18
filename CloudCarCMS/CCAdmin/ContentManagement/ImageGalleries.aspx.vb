Imports CloudCar.CCFramework.Model

Namespace CCAdmin.ContentManagement

    Partial Public Class ImageGalleries
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub LoadGallery(ByVal GalleryID As Integer)
            Try
                Dim Gallery As ImageGallery = CCFramework.ContentManagement.ImageGalleryController.GetElement(GalleryID)

                hfID.Value = Gallery.ID.ToString

                txtTitle.Text = Gallery.Title
                txtDescription.Text = Gallery.Description
            Catch ex As Exception
                lblStatus.Text = ex.ToString
                lblStatus.Visible = True
            End Try
        End Sub

        Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Dim GalleryID As Integer

            If Page.IsValid Then
                If Integer.TryParse(hfID.Value, GalleryID) Then
                    CCFramework.ContentManagement.ImageGalleryController.Update(GalleryID, txtTitle.Text, txtDescription.Text)

                    lblStatus.Text = "Your image gallery has been saved!"
                    lblStatus.Visible = True
                Else
                    CCFramework.ContentManagement.ImageGalleryController.Create(txtTitle.Text, txtDescription.Text)

                    lblStatus.Text = "Your image gallery has been created!"
                    lblStatus.Visible = True
                End If

                RefreshDataSources()
            End If
        End Sub

        Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
            ClearControls()
        End Sub

        Private Sub RefreshDataSources()
            lbImageGalleries.Items.Clear()
            lbImageGalleries.Items.Add(New ListItem("To edit select an image gallery from this list", ""))
            lbImageGalleries.AppendDataBoundItems = True
            lbImageGalleries.DataSource = CCFramework.ContentManagement.ImageGalleryController.GetElements
            lbImageGalleries.DataBind()
        End Sub

        Private Sub ClearControls()
            hfID.Value = Nothing

            txtTitle.Text = ""
            txtDescription.Text = ""
        End Sub

        Private Sub lbImageGalleries_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbImageGalleries.SelectedIndexChanged
            If Not lbImageGalleries.SelectedValue = Nothing Then
                Dim GalleryID As Integer = Integer.Parse(lbImageGalleries.SelectedValue)

                LoadGallery(GalleryID)
            End If
        End Sub

        Private Sub btnDelete_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnDelete.Command
            If Not hfID.Value = Nothing Then
                Dim GalleryID As Integer = Integer.Parse(hfID.Value)

                If CCFramework.ContentManagement.ImageGalleryController.Delete(GalleryID) = True Then
                    lblStatus.Text = "The selected image gallery has been deleted permanently!"
                    lblStatus.Visible = True

                    RefreshDataSources()

                    ClearControls()
                Else
                    lblStatus.Text = "An error occured while trying to delete the current image gallery."
                    lblStatus.Visible = True
                End If
            Else
                lblStatus.Text = "Sorry but you do not have an image gallery selected."
                lblStatus.Visible = True
            End If
        End Sub

    End Class
End Namespace