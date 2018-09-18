Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce.Shipping

    Partial Public Class ShippingZoneDetails
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub RefreshDataSources()
            ddlDistributor.Items.Clear()
            ddlDistributor.Items.Add(New ListItem("Select", ""))
            ddlDistributor.AppendDataBoundItems = True
            ddlDistributor.DataSource = New CCFramework.Core.RegisteredUserController().GetElements
            ddlDistributor.DataBind()

            ddlProvine.Items.Clear()
            ddlProvine.Items.Add(New ListItem("Select", ""))
            ddlProvine.AppendDataBoundItems = True
            ddlProvine.DataSource = New ProvinceController().GetElements()
            ddlProvine.DataBind()

            Dim UserId As Integer

            If Integer.TryParse(ddlSelectDistributor.SelectedValue, UserId) Then
                ZonesGridView.DataSource = FixedShippingZoneController.GetDistributorShippingZones(UserId).OrderBy(Function(z) z.PrefixLow).ToList
                ZonesGridView.DataBind()
            Else
                ZonesGridView.DataSource = FixedShippingZoneController.GetShippingZones().OrderBy(Function(z) z.PrefixLow).ToList
                ZonesGridView.DataBind()
            End If
        End Sub

        Private Sub btnAddZone_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddZone.Command
            If Page.IsValid Then
                Dim ID As Integer

                If Not Integer.TryParse(hfZoneID.Value, ID) Then
                    FixedShippingZoneController.Create(txtPrefixLow.Text, txtPrefixHigh.Text, Integer.Parse(txtZoneSZ.Text), Integer.Parse(ddlProvine.SelectedValue), Integer.Parse(ddlDistributor.SelectedValue))

                    lblStatus.Text = "You have sucessfully created the new zone"
                    lblStatus.Visible = True
                Else
                    FixedShippingZoneController.Update(Integer.Parse(hfZoneID.Value), txtPrefixLow.Text, txtPrefixHigh.Text, Integer.Parse(txtZoneSZ.Text), Integer.Parse(ddlProvine.SelectedValue), Integer.Parse(ddlDistributor.SelectedValue))

                    lblStatus.Text = "You have sucessfully updated the current zone"
                    lblStatus.Visible = True
                End If

                LoadSelectedDistributors()
                RefreshDataSources()
            End If
        End Sub

        Private Sub btnClearZone_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnClearZone.Command
            hfZoneID.Value = Nothing

            txtPrefixLow.Text = ""
            txtPrefixHigh.Text = ""
            txtZoneSZ.Text = ""

            ddlProvine.SelectedIndex = Nothing
            ddlDistributor.SelectedIndex = Nothing
        End Sub

        Private Sub ZonesGridViewPageIndexChanging(ByVal Sender As Object, ByVal Args As GridViewPageEventArgs) Handles ZonesGridView.PageIndexChanging
            ZonesGridView.PageIndex = Args.NewPageIndex
            RefreshDataSources()
        End Sub

        Protected Sub btnSelectZone_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim zone As FixedShippingZone = FixedShippingZoneController.GetShippingZone(id)

            hfZoneID.Value = zone.ID.ToString

            txtPrefixLow.Text = zone.PrefixLow
            txtPrefixHigh.Text = zone.PrefixHigh
            txtZoneSZ.Text = zone.Zone.ToString

            ddlProvine.SelectedValue = zone.ProvinceID.ToString
            ddlDistributor.SelectedValue = zone.DistributorUserID.ToString
        End Sub

        Protected Sub btnDeleteZone_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim ID As Integer = Integer.Parse(e.CommandArgument.ToString)

            Try
                FixedShippingZoneController.Delete(ID)

                lblStatus.Text = "You have successfully deleted the Zone (ID: " & ID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to delete the Zone (ID: " & ID.ToString & ")"
            End Try

            lblStatus.Visible = True

            LoadSelectedDistributors()
            RefreshDataSources()
        End Sub

        Private Sub btnTestPostalCode_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnTestPostalCode.Command
            Dim Prefix As String = txtTestPostalCode.Text

            Dim DistributorId As Integer = FixedShippingZoneController.GetShippingZoneDistributor(Prefix)
            Dim DistributorUser As String = CCFramework.Core.RegisteredUserController.GetUserNameByID(DistributorId)

            lblTestPostalCode.Text = "<b>Branch:</b> " & DistributorUser & " <b>Address:</b> " & OrderController.GetOrderPickupAddress(Prefix)
            lblTestPostalCode.Visible = True
        End Sub

        Private Sub ddlSelectDistributor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSelectDistributor.Load
            If Not Page.IsPostBack Then
                LoadSelectedDistributors()
            End If
        End Sub

        Private Sub LoadSelectedDistributors()
            ddlSelectDistributor.Items.Clear()
            ddlSelectDistributor.Items.Add(New ListItem("Select", ""))
            ddlSelectDistributor.AppendDataBoundItems = True
            ddlSelectDistributor.DataSource = FixedShippingZoneController.GetAllShippingZoneDistributors()
            ddlSelectDistributor.DataBind()
        End Sub

        Private Sub ddlSelectDistributor_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSelectDistributor.SelectedIndexChanged
            RefreshDataSources()
        End Sub

        Private Sub btnDisplayUnusedPostalCodes_Command(ByVal sender As Object, ByVal e As CommandEventArgs) Handles btnDisplayUnusedPostalCodes.Command
            Dim UnselectedZonesList As List(Of String) = FixedShippingZoneController.GetUnselectedZones()

            For Each item As String In UnselectedZonesList
                lblPrefixList.Text &= item
                If Not item Is UnselectedZonesList.Last Then
                    lblPrefixList.Text &= ", "
                End If
            Next

            lblPrefixList.Visible = True
        End Sub

    End Class
End Namespace