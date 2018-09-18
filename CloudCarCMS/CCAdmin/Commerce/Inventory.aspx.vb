Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce

    Partial Public Class Inventory
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()

                ViewState.Add("sortAscending", True)
                ViewState.Add("lastSort", "")

                If Not Request.QueryString("Product") Is Nothing Then
                    Dim ProductID As Integer = Integer.Parse(Request.QueryString("Product"))

                    hlBackToProduct.NavigateUrl = "~/CCAdmin/Commerce/ProductDetails.aspx?Product=" & ProductID
                    hlBackToProduct.Visible = True
                End If
            End If

            lblStatus.Visible = False
        End Sub

        Private Sub LoadInventory(ByVal InventoryID As Integer)
            Dim item As CCFramework.Model.Inventory = New InventoryController().GetElement(InventoryID)

            hfInventoryID.Value = InventoryID.ToString
            ddlProduct.SelectedValue = item.ProductID.ToString
            ddlColor.SelectedValue = item.ColorID.ToString
            ddlSize.SelectedValue = item.SizeID.ToString
            txtQuantity.Text = item.Quantity.ToString
        End Sub

        Protected Sub btnEdit_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim InventoryID As Integer = Integer.Parse(e.CommandArgument.ToString)

            LoadInventory(InventoryID)
        End Sub

        Protected Sub btnDelete_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim InventoryID As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim item As New InventoryController

            Try
                item.Delete(InventoryID)

                lblStatus.Text = "You have successfully deleted the inventory (ID: " & InventoryID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to delete the inventory (ID: " & InventoryID.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub

        Protected Sub btnUpdate_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim InventoryID As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim item As New InventoryController

            Try
                'TODO Update inventory
                'item.UpdateQuantity(InventoryID, quantity)

                lblStatus.Text = "You have successfully updated this inventory (ID: " & InventoryID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to update this inventory (ID: " & InventoryID.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub

        Protected Sub btnAdd_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAdd.Command
            Dim item As New InventoryController

            Dim InventoryID As Integer
            Dim productID As Integer = Integer.Parse(ddlProduct.SelectedValue)
            Dim colorID As Integer = Integer.Parse(ddlColor.SelectedValue)
            Dim sizeID As Integer = Integer.Parse(ddlSize.SelectedValue)
            Dim quantity As Integer

            If Integer.TryParse(txtQuantity.Text, quantity) Then
                If Integer.TryParse(hfInventoryID.Value, InventoryID) Then
                    item.Update(InventoryID, productID, colorID, sizeID, Integer.Parse(txtQuantity.Text))

                    lblStatus.Text = "You have successfully updated the inventory (ID: " & InventoryID.ToString & ")"
                Else
                    If item.ProductHasInventory(productID, colorID, sizeID) Then
                        'TODO update the inventory table to add to the already existing inventory
                        lblStatus.Text = "Please modify the already existing inventory levels for this product!" '"You have successfully added the inventory (ID: " & InventoryID.ToString & ")"
                    Else
                        InventoryID = item.Create(productID, Integer.Parse(ddlColor.SelectedValue), Integer.Parse(ddlSize.SelectedValue), Integer.Parse(txtQuantity.Text))

                        lblStatus.Text = "You have successfully added the inventory (ID: " & InventoryID.ToString & ")"
                    End If
                End If
            Else
                lblStatus.Text = "The quantity of inventory must be a positive or negative whole number."
            End If

            lblStatus.Visible = True

            ClearControls()
            RefreshDataSources()
        End Sub

        Private Sub btnClear_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnClear.Command
            ClearControls()
        End Sub

        Public Sub btnAddInventory_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim btnAddInventory As LinkButton = CType(sender, LinkButton)
            Dim InventoryID As Integer = Integer.Parse(btnAddInventory.Attributes("InventoryID"))
            Dim Quantity As Integer = Integer.Parse(CType(btnAddInventory.Parent.FindControl("txtAdjustInventory"), TextBox).Text)

            Dim add As Boolean = New InventoryController().AddInventory(InventoryID, Quantity)

            RefreshDataSources()
        End Sub

        Public Sub btnSubtractInventory_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim btnSubtractInventory As LinkButton = CType(sender, LinkButton)
            Dim InventoryID As Integer = Integer.Parse(btnSubtractInventory.Attributes("InventoryID"))
            Dim Quantity As Integer = Integer.Parse(CType(btnSubtractInventory.Parent.FindControl("txtAdjustInventory"), TextBox).Text)

            Dim add As Boolean = New InventoryController().SubtractInventory(InventoryID, Quantity)

            RefreshDataSources()
        End Sub

        Private Sub ClearControls()
            hfInventoryID.Value = Nothing

            txtQuantity.Text = ""

            ddlProduct.SelectedValue = Nothing
            ddlSize.SelectedValue = Nothing
            ddlColor.SelectedValue = Nothing
        End Sub

        Private Sub RefreshDataSources()
            ddlProduct.Items.Clear()
            ddlProduct.Items.Add(New ListItem("Product", ""))
            ddlProduct.AppendDataBoundItems = True
            ddlProduct.DataSource = New ProductController().GetElements
            ddlProduct.DataBind()

            ddlSize.Items.Clear()
            ddlSize.Items.Add(New ListItem("Size", ""))
            ddlSize.AppendDataBoundItems = True
            ddlSize.DataSource = New ProductSizeController().GetElements
            ddlSize.DataBind()

            ddlColor.Items.Clear()
            ddlColor.Items.Add(New ListItem("Colour", ""))
            ddlColor.AppendDataBoundItems = True
            ddlColor.DataSource = New ProductColourController().GetElements
            ddlColor.DataBind()

            dgInventory.DataSource = New InventoryController().GetGridElements()
            dgInventory.DataBind()
        End Sub


        Private Property sortAscending() As Boolean
            Get
                Return CBool(ViewState("sortAscending"))
            End Get
            Set(ByVal value As Boolean)
                ViewState("sortAscending") = value
            End Set
        End Property

        Private Property lastSort() As String
            Get
                Return CStr(ViewState("lastSort"))
            End Get
            Set(ByVal value As String)
                If Not String.IsNullOrEmpty(value) Then
                    ViewState("lastSort") = value
                End If
            End Set
        End Property

        Private Sub dgInventory_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgInventory.SortCommand
            If Not e.SortExpression = lastSort Then
                sortAscending = True
            End If

            Select Case e.SortExpression
                Case "Color"
                    If sortAscending Then
                        dgInventory.DataSource = New InventoryController().GetGridElements.OrderBy(Function(p) p.Color)
                        sortAscending = False
                    Else
                        dgInventory.DataSource = New InventoryController().GetGridElements.OrderByDescending(Function(p) p.Color)
                        sortAscending = True
                    End If
                Case "Size"
                    If sortAscending Then
                        dgInventory.DataSource = New InventoryController().GetGridElements.OrderBy(Function(p) p.size)
                        sortAscending = False
                    Else
                        dgInventory.DataSource = New InventoryController().GetGridElements.OrderByDescending(Function(p) p.Size)
                        sortAscending = True
                    End If
                Case "Name"
                    If sortAscending Then
                        dgInventory.DataSource = New InventoryController().GetGridElements.OrderBy(Function(p) p.Product)
                        sortAscending = False
                    Else
                        dgInventory.DataSource = New InventoryController().GetGridElements.OrderByDescending(Function(p) p.Product)
                        sortAscending = True
                    End If
                Case "ID"
                    If sortAscending Then
                        dgInventory.DataSource = New InventoryController().GetGridElements.OrderBy(Function(p) p.ID)
                        sortAscending = False
                    Else
                        dgInventory.DataSource = New InventoryController().GetGridElements.OrderByDescending(Function(p) p.ID)
                        sortAscending = True
                    End If
            End Select

            lastSort = e.SortExpression

            dgInventory.DataBind()
        End Sub

    End Class
End Namespace