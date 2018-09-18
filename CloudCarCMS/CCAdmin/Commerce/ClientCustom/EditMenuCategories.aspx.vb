Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce.ClientCustom

    Partial Public Class EditMenuCategories
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub RefreshDataSources()
            Dim db As New CommerceDataContext

            gvMenuCategories.DataSource = From mc In db.MenuCategories Select mc
            gvMenuCategories.DataBind()

            gvMenuCategoryItems.DataSource = From mci In db.MenuCategoryItems _
                Join p In db.Products On p.ID Equals mci.productID _
                Join mc In db.MenuCategories On mc.id Equals mci.menuCategoryID _
                Select New With {.id = mci.id, .product = p.Name, .category = mc.category}
            gvMenuCategoryItems.DataBind()

            ddlMenuCategory.Items.Clear()
            ddlMenuCategory.Items.Add(New ListItem("Select", ""))
            ddlMenuCategory.Items.Add(New ListItem("----------", ""))
            ddlMenuCategory.AppendDataBoundItems = True
            ddlMenuCategory.DataSource = From mc In db.MenuCategories Select mc
            ddlMenuCategory.DataBind()

            ddlProduct.Items.Clear()
            ddlProduct.Items.Add(New ListItem("Select", ""))
            ddlProduct.Items.Add(New ListItem("----------", ""))
            ddlProduct.AppendDataBoundItems = True
            ddlProduct.DataSource = From p In db.Products Select New With {.id = p.ID, .name = p.Name}
            ddlProduct.DataBind()
        End Sub

        Private Sub btnAddMenuCategory_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddMenuCategory.Command
            phAddMenuCategory.Visible = True
            btnAddMenuCategory.Visible = False
        End Sub

        Private Sub btnAddMC_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddMC.Command

            If Not hfMCID.Value = "" Then
                Dim ID As Integer = Integer.Parse(hfMCID.Value)

                Dim db As New CommerceDataContext

                Dim category = (From mc In db.MenuCategories Where mc.id = ID Select mc).FirstOrDefault

                category.category = txtCategory.Text

                db.SubmitChanges()

                category = Nothing
                db = Nothing

                lblStatus.Text = "Menu Category has been successfully updated."
                lblStatus.Visible = True
            Else
                Dim db As New CommerceDataContext

                Dim category As New MenuCategory

                category.category = txtCategory.Text

                db.MenuCategories.InsertOnSubmit(category)
                db.SubmitChanges()

                category = Nothing
                db = Nothing

                lblStatus.Text = "Menu Category has been successfully created."
                lblStatus.Visible = True
            End If

            RefreshDataSources()

            phAddMenuCategory.Visible = False
            btnAddMenuCategory.Visible = True

            hfMCID.Value = ""
        End Sub

        Private Sub btnCancelMC_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnCancelMC.Command
            phAddMenuCategory.Visible = False
            btnAddMenuCategory.Visible = True

            hfMCID.Value = ""
            txtCategory.Text = ""
        End Sub

        Protected Sub gvMenuCategories_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMenuCategories.RowCommand
            If e.CommandName = "DeleteItem" Then
                Dim Id As Integer = Integer.Parse(e.CommandArgument.ToString)

                Dim db As New CommerceDataContext

                Dim category = (From mc In db.MenuCategories Where mc.id = Id Select mc).FirstOrDefault

                db.MenuCategories.DeleteOnSubmit(category)
                db.SubmitChanges()

                lblStatus.Text = "Menu Category has been successfully deleted."
                lblStatus.Visible = True

                RefreshDataSources()
            ElseIf e.CommandName = "Select" Then
                Dim ID As Integer = Integer.Parse(e.CommandArgument.ToString)

                Dim db As New CommerceDataContext

                Dim selectedCategory As MenuCategory = (From mc In db.MenuCategories Where mc.id = ID Select mc).FirstOrDefault

                With selectedCategory
                    hfMCID.Value = ID.ToString
                    txtCategory.Text = selectedCategory.category
                End With

                phAddMenuCategory.Visible = True
                btnAddMenuCategory.Visible = False
            End If
        End Sub

        Private Sub btnAddMenuCategoryItem_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddMenuCategoryItem.Command
            phAddMenuCategoryItem.Visible = True
            btnAddMenuCategoryItem.Visible = False
        End Sub

        Private Sub btnAddMCI_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddMCI.Command

            If Not hfMCIID.Value = "" Then
                Dim ID As Integer = Integer.Parse(hfMCIID.Value)

                Dim db As New CommerceDataContext

                Dim categoryItem = (From mci In db.MenuCategoryItems Where mci.id = ID Select mci).FirstOrDefault

                categoryItem.menuCategoryID = Integer.Parse(ddlMenuCategory.SelectedValue)
                categoryItem.productID = Integer.Parse(ddlProduct.SelectedValue)

                db.SubmitChanges()

                categoryItem = Nothing
                db = Nothing

                lblStatus.Text = "Menu Category Item has been successfully updated."
                lblStatus.Visible = True
            Else
                Dim db As New CommerceDataContext

                Dim categoryItem As New MenuCategoryItem

                categoryItem.menuCategoryID = Integer.Parse(ddlMenuCategory.SelectedValue)
                categoryItem.productID = Integer.Parse(ddlProduct.SelectedValue)

                db.MenuCategoryItems.InsertOnSubmit(categoryItem)
                db.SubmitChanges()

                categoryItem = Nothing
                db = Nothing

                lblStatus.Text = "Menu Category Item has been successfully created."
                lblStatus.Visible = True
            End If

            RefreshDataSources()

            phAddMenuCategoryItem.Visible = False
            btnAddMenuCategoryItem.Visible = True

            hfMCIID.Value = ""
        End Sub

        Private Sub btnCancelMCI_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnCancelMCI.Command
            phAddMenuCategoryItem.Visible = False
            btnAddMenuCategoryItem.Visible = True

            hfMCIID.Value = ""
            ddlMenuCategory.SelectedIndex = 0
            ddlProduct.SelectedIndex = 0
        End Sub

        Protected Sub gvMenuCategoryItems_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMenuCategoryItems.RowCommand
            If e.CommandName = "DeleteItem" Then
                Dim Id As Integer = Integer.Parse(e.CommandArgument.ToString)

                Dim db As New CommerceDataContext

                Dim categoryItem = (From mci In db.MenuCategoryItems Where mci.id = Id Select mci).FirstOrDefault

                db.MenuCategoryItems.DeleteOnSubmit(categoryItem)
                db.SubmitChanges()

                lblStatus.Text = "Menu Category Item has been successfully deleted."
                lblStatus.Visible = True

                RefreshDataSources()
            ElseIf e.CommandName = "Select" Then
                Dim ID As Integer = Integer.Parse(e.CommandArgument.ToString)

                Dim db As New CommerceDataContext

                Dim selectedCategoryItem As MenuCategoryItem = (From mci In db.MenuCategoryItems Where mci.id = ID Select mci).FirstOrDefault

                With selectedCategoryItem
                    hfMCID.Value = ID.ToString
                    ddlMenuCategory.SelectedValue = .menuCategoryID.ToString
                    ddlProduct.SelectedValue = .productID.ToString
                End With

                phAddMenuCategoryItem.Visible = True
                btnAddMenuCategoryItem.Visible = False
            End If
        End Sub

    End Class
End Namespace