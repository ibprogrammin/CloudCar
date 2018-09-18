Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce.ClientCustom

    Partial Public Class BoxCalculator
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub RefreshDataSources()
            Dim product As New ProductController()

            ddlProduct.Items.Clear()
            ddlProduct.Items.Add(New ListItem("Select", ""))
            ddlProduct.AppendDataBoundItems = True
            ddlProduct.DataSource = product.GetElements
            ddlProduct.DataBind()

            ddlRoom.Items.Clear()
            ddlRoom.Items.Add(New ListItem("Select", ""))
            ddlRoom.AppendDataBoundItems = True
            ddlRoom.DataSource = CCFramework.Core.ClientControlls.BoxCalculatorController.Room.GetRooms()
            ddlRoom.DataBind()

            dgRooms.DataSource = CCFramework.Core.ClientControlls.BoxCalculatorController.Room.GetRooms().ToList
            dgRooms.DataBind()

            dgRoomProducts.DataSource = CCFramework.Core.ClientControlls.BoxCalculatorController.RoomProduct.GetRoomProducts().ToList
            dgRoomProducts.DataBind()
        End Sub

        Private Sub btnAddRoom_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddRoom.Command
            If Page.IsValid Then
                Dim RoomID As Integer

                If Integer.TryParse(hfRoomID.Value, RoomID) Then
                    CCFramework.Core.ClientControlls.BoxCalculatorController.Room.Update(RoomID, txtName.Text)

                    lblStatus.Text = "You have sucessfully updated the current room"
                    lblStatus.Visible = True
                Else
                    CCFramework.Core.ClientControlls.BoxCalculatorController.Room.Create(txtName.Text)

                    lblStatus.Text = "You have sucessfully created the new room"
                    lblStatus.Visible = True
                End If

                ClearRoom()

                RefreshDataSources()
            End If
        End Sub

        Private Sub btnClearRoom_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnClearRoom.Command
            ClearRoom()
        End Sub

        Private Sub btnAddRoomProduct_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddRoomProduct.Command
            If Page.IsValid Then
                Dim RoomProductID As Integer

                If Integer.TryParse(hfRoomProductID.Value, RoomProductID) Then
                    CCFramework.Core.ClientControlls.BoxCalculatorController.RoomProduct.Update(RoomProductID, Integer.Parse(ddlRoom.SelectedValue), Integer.Parse(ddlProduct.SelectedValue), ckbReoccurs.Checked, Integer.Parse(txtQuantity.Text))

                    lblStatus.Text = "You have sucessfully updated the current product for this room"
                    lblStatus.Visible = True
                Else
                    CCFramework.Core.ClientControlls.BoxCalculatorController.RoomProduct.Create(Integer.Parse(ddlRoom.SelectedValue), Integer.Parse(ddlProduct.SelectedValue), ckbReoccurs.Checked, Integer.Parse(txtQuantity.Text))

                    lblStatus.Text = "You have sucessfully created a new product for this room"
                    lblStatus.Visible = True
                End If

                ClearRoomProduct()

                RefreshDataSources()
            End If
        End Sub

        Private Sub btnClearRoomProduct_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnClearRoomProduct.Command
            ClearRoomProduct()
        End Sub


        Private Sub dgRooms_PageIndexChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRooms.PageIndexChanged
            dgRooms.CurrentPageIndex = e.NewPageIndex

            RefreshDataSources()
        End Sub

        Protected Sub btnSelectRoom_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim room As BCRoom = CCFramework.Core.ClientControlls.BoxCalculatorController.Room.GetRoom(id)

            hfRoomID.Value = room.ID.ToString
            txtName.Text = room.RoomName
        End Sub

        Protected Sub btnDeleteRoom_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim ID As Integer = Integer.Parse(e.CommandArgument.ToString)

            Try
                CCFramework.Core.ClientControlls.BoxCalculatorController.Room.Delete(ID)

                lblStatus.Text = "You have successfully deleted the Room (ID: " & ID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to delete the Room (ID: " & ID.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub


        Private Sub dgRoomProducts_PageIndexChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRoomProducts.PageIndexChanged
            dgRoomProducts.CurrentPageIndex = e.NewPageIndex

            RefreshDataSources()
        End Sub

        Protected Sub btnSelectRoomProduct_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim product As BCRoomProduct = CCFramework.Core.ClientControlls.BoxCalculatorController.RoomProduct.GetRoomProduct(id)

            hfRoomProductID.Value = product.ID.ToString

            txtQuantity.Text = product.Quantity.ToString

            ddlProduct.SelectedValue = product.ProductID.ToString
            ddlRoom.SelectedValue = product.RoomID.ToString

            ckbReoccurs.Checked = product.Reoccurs
        End Sub

        Protected Sub btnDeleteRoomProduct_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim ID As Integer = Integer.Parse(e.CommandArgument.ToString)

            Try
                CCFramework.Core.ClientControlls.BoxCalculatorController.RoomProduct.Delete(ID)

                lblStatus.Text = "You have successfully deleted the Room Product (ID: " & ID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to delete the Room Product (ID: " & ID.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub

        Private Sub ClearRoom()
            hfRoomID.Value = Nothing
            txtName.Text = ""
        End Sub

        Private Sub ClearRoomProduct()
            hfRoomProductID.Value = Nothing
            txtQuantity.Text = ""
            ddlRoom.SelectedIndex = Nothing
            ddlProduct.SelectedIndex = Nothing
            ckbReoccurs.CausesValidation = False
        End Sub

        Public Function GetRoomName(ByVal RoomID As Integer) As String
            Return CCFramework.Core.ClientControlls.BoxCalculatorController.Room.GetRoom(RoomID).RoomName
        End Function

        Public Function GetProductName(ByVal ProductID As Integer) As String
            Return New ProductController().GetElement(ProductID).Name
        End Function

    End Class
End Namespace