Imports CloudCar.CCFramework.Model

Namespace CCAdmin.ContentManagement

    Partial Public Class MenuItems
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub btnAdd_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAdd.Command
            Dim Title As String = txtTitle.Text
            Dim URL As String = txtURL.Text
            Dim Details As String = txtDetails.Text
            Dim Order As Integer = Integer.Parse(txtOrder.Text)
            Dim ParentPage As Integer = Integer.Parse(ddlParentPage.SelectedValue)
            Dim CssClass As String = txtCssClass.Text
            Dim Menu As String = txtMenu.Text
            Dim IconImageURL As String = txtIconImageURL.Text

            If Not hfID.Value = "" Then
                Dim ID As Integer = Integer.Parse(hfID.Value)

                CCFramework.ContentManagement.MenuItemController.Update(ID, Title, URL, Details, ParentPage, Order, CssClass, Menu, IconImageURL)

                lblStatus.Text = "You have successfully updated the Menu Item"
                lblStatus.Visible = True
            Else
                CCFramework.ContentManagement.MenuItemController.Create(Title, URL, Details, ParentPage, Order, CssClass, Menu, IconImageURL)

                lblStatus.Text = "The Menu Item has been successfully saved"
                lblStatus.Visible = True
            End If

            ClearControls()
            RefreshDataSources()
        End Sub

        Private Sub btnCancel_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnCancel.Command
            ClearControls()
        End Sub

        Private Sub RefreshDataSources()
            ddlParentPage.Items.Add(New ListItem("None", "0"))
            ddlParentPage.AppendDataBoundItems = True
            ddlParentPage.DataSource = CCFramework.ContentManagement.MenuItemController.GetElements()
            ddlParentPage.DataBind()

            gvMenuItems.DataSource = CCFramework.ContentManagement.MenuItemController.GetElements()
            gvMenuItems.DataBind()
        End Sub

        Private Sub ClearControls()
            hfID.Value = Nothing

            txtCssClass.Text = ""
            txtDetails.Text = ""
            txtOrder.Text = ""
            txtTitle.Text = ""
            txtURL.Text = ""
            txtMenu.Text = ""
            txtIconImageURL.Text = ""

            ddlParentPage.SelectedValue = Nothing
        End Sub

        Private Sub gvMenuItems_PageIndexChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles gvMenuItems.PageIndexChanged
            gvMenuItems.CurrentPageIndex = e.NewPageIndex

            RefreshDataSources()
        End Sub

        Protected Sub btnSelect_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim MenuItem As MenuItem = CCFramework.ContentManagement.MenuItemController.GetElement(id)

            hfID.Value = MenuItem.ID.ToString

            txtTitle.Text = MenuItem.Title
            txtDetails.Text = MenuItem.Details
            txtURL.Text = MenuItem.URL
            txtOrder.Text = MenuItem.Order.ToString
            txtCssClass.Text = MenuItem.CssClass
            txtMenu.Text = MenuItem.Menu
            txtIconImageURL.Text = MenuItem.IconImageUrl

            ddlParentPage.SelectedValue = MenuItem.ParentID.ToString
        End Sub

        Protected Sub btnDelete_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            If CCFramework.ContentManagement.MenuItemController.Delete(id) Then
                lblStatus.Text = "The Menu Item has been successfully deleted from the database."
                lblStatus.Visible = True

                RefreshDataSources()
            End If
        End Sub

    End Class
End Namespace