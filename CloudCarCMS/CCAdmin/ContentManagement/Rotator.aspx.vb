Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement

Namespace CCAdmin.ContentManagement

    Partial Public Class Rotator
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub LoadItem(ByVal ItemId As Integer)
            Try
                Dim CurrentContentDataContext As New ContentDataContext
                Dim Item As ImageRotator = ImageRotatorController.GetElement(CurrentContentDataContext, ItemId)

                hfItemID.Value = Item.id.ToString

                txtTitle.Text = Item.title
                txtSubHeading.Text = Item.subheading
                DetailsTextArea.InnerText = Server.HtmlDecode(Item.details)
                txtLinkURL.Text = Item.linkurl
                txtOrder.Text = Item.order.ToString

                ddlPage.SelectedValue = Item.pageID.ToString

                hfImageID.Value = Item.imageID.ToString

                lblImageLocation.Text = "/images/db/" & Item.imageID & "/full/" & Item.Picture.PictureFileName
                imgRotatorImage.Src = "/images/db/" & Item.imageID & "/900/" & Item.Picture.PictureFileName

                lblImageLocation.Visible = True
                imgRotatorImage.Visible = True

                CurrentContentDataContext.Dispose()
            Catch Ex As Exception
                lblStatus.Text = Ex.ToString
                lblStatus.Visible = True
            End Try
        End Sub

        Private Sub SaveButtonClick(ByVal Sender As Object, ByVal E As System.EventArgs) Handles btnSave.Click
            Dim ItemId As Integer

            If Page.IsValid Then
                Dim ImageId As Integer = Nothing

                If fuImage.HasFile Then
                    ImageID = CCFramework.Core.ImageFunctions.UploadImage(fuImage)
                Else
                    ImageID = Integer.Parse(hfImageID.Value)
                End If

                If Integer.TryParse(hfItemID.Value, ItemId) Then
                    ImageRotatorController.Update(ItemId, txtTitle.Text, txtSubHeading.Text, Server.HtmlEncode(DetailsTextArea.InnerText), txtLinkURL.Text, CInt(txtOrder.Text), ImageId, CInt(ddlPage.SelectedValue))

                    LoadItem(ItemId)

                    lblStatus.Text = "Your item has been saved!"
                    lblStatus.Visible = True
                Else
                    ImageRotatorController.Create(txtTitle.Text, txtSubHeading.Text, Server.HtmlEncode(DetailsTextArea.InnerText), txtLinkURL.Text, CInt(txtOrder.Text), ImageId, CInt(ddlPage.SelectedValue))

                    lblStatus.Text = "Your item has been created!"
                    lblStatus.Visible = True

                    RefreshDataSources()
                End If
            End If
        End Sub

        Private Sub ClearButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnClear.Click
            ClearControls()
        End Sub

        Private Sub RefreshDataSources()
            ddlPage.Items.Clear()
            ddlPage.Items.Add(New ListItem("Select a Page", ""))
            ddlPage.AppendDataBoundItems = True
            ddlPage.DataSource = ContentPageController.GetElement()
            ddlPage.DataBind()

            Dim CurrentDataContext As New ContentDataContext

            lbRotator.Items.Clear()
            lbRotator.Items.Add(New ListItem("Select an Image Rotator Item", ""))
            lbRotator.AppendDataBoundItems = True
            lbRotator.DataSource = ImageRotatorController.GetElements(CurrentDataContext)
            lbRotator.DataBind()

            CurrentDataContext.Dispose()
        End Sub

        Private Sub ClearControls()
            hfItemID.Value = Nothing
            hfImageID.Value = Nothing

            txtTitle.Text = ""
            txtSubHeading.Text = ""
            txtLinkURL.Text = ""
            txtOrder.Text = ""
            DetailsTextArea.InnerText = ""

            imgRotatorImage.Src = ""
            imgRotatorImage.Visible = False

            lblImageLocation.Text = ""
            lblImageLocation.Visible = False

            ddlPage.SelectedIndex = 0
        End Sub

        Private Sub RotatorListBoxSelectedIndexChanged(ByVal Sender As Object, ByVal E As EventArgs) Handles lbRotator.SelectedIndexChanged
            If Not lbRotator.SelectedValue = Nothing Then
                Dim ItemId As Integer = Integer.Parse(lbRotator.SelectedValue)

                LoadItem(ItemId)
            End If
        End Sub

        Private Sub DeleteButtonClick(ByVal Sender As Object, ByVal E As CommandEventArgs) Handles btnDelete.Command
            If Not hfItemID.Value = Nothing Then
                Dim ItemId As Integer = Integer.Parse(hfItemID.Value)

                If ImageRotatorController.Delete(ItemId) = True Then
                    lblStatus.Text = "The selected item has been deleted permanently!"
                    lblStatus.Visible = True

                    RefreshDataSources()

                    ClearControls()
                Else
                    lblStatus.Text = "An error occured while trying to delete the current item."
                    lblStatus.Visible = True
                End If
            Else
                lblStatus.Text = "Sorry but you do not have an item selected."
                lblStatus.Visible = True
            End If
        End Sub

        Public Sub GetImageButtonClick(ByVal Sender As Object, ByVal E As CommandEventArgs)

        End Sub

    End Class

End Namespace