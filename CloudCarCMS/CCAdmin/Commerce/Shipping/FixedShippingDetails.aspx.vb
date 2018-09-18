Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce.Shipping

    Partial Public Class FixedShippingDetails
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub RefreshDataSources()
            Dim product As New ProductController()

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

            dgZones.DataSource = CCFramework.Commerce.FixedShippingZoneController.GetShippingZones().OrderBy(Function(z) z.PrefixLow).ToList
            dgZones.DataBind()

            dgRates.DataSource = CCFramework.Commerce.FixedShippingRateController.GetShippingRates().ToList
            dgRates.DataBind()
        End Sub


        Private Sub btnAddZone_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddZone.Command
            If Page.IsValid Then
                Dim ID As Integer

                If Not Integer.TryParse(hfZoneID.Value, ID) Then
                    CCFramework.Commerce.FixedShippingZoneController.Create(txtPrefixLow.Text, txtPrefixHigh.Text, Integer.Parse(txtZoneSZ.Text), Integer.Parse(ddlProvine.SelectedValue), Integer.Parse(ddlDistributor.SelectedValue))

                    lblStatus.Text = "You have sucessfully created the new zone"
                    lblStatus.Visible = True
                Else
                    CCFramework.Commerce.FixedShippingZoneController.Update(Integer.Parse(hfZoneID.Value), txtPrefixLow.Text, txtPrefixHigh.Text, Integer.Parse(txtZoneSZ.Text), Integer.Parse(ddlProvine.SelectedValue), Integer.Parse(ddlDistributor.SelectedValue))

                    lblStatus.Text = "You have sucessfully updated the current zone"
                    lblStatus.Visible = True
                End If

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


        Private Sub btnAddRate_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddRate.Command
            If Page.IsValid Then
                Dim ID As Integer

                If Not Integer.TryParse(hfRateID.Value, ID) Then
                    CCFramework.Commerce.FixedShippingRateController.Create(Decimal.Parse(txtWeightLBS.Text), Decimal.Parse(txtWeightKG.Text), Integer.Parse(txtZoneSR.Text), Decimal.Parse(txtCost.Text))

                    lblStatus.Text = "You have sucessfully created a new rate"
                    lblStatus.Visible = True
                Else
                    CCFramework.Commerce.FixedShippingRateController.Update(Integer.Parse(hfRateID.Value), Decimal.Parse(txtWeightLBS.Text), Decimal.Parse(txtWeightKG.Text), Integer.Parse(txtZoneSR.Text), Decimal.Parse(txtCost.Text))

                    lblStatus.Text = "You have sucessfully updated the current rate"
                    lblStatus.Visible = True
                End If

                RefreshDataSources()
            End If
        End Sub

        Private Sub btnClearRate_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnClearRate.Command
            hfRateID.Value = Nothing

            txtWeightLBS.Text = ""
            txtWeightKG.Text = ""
            txtCost.Text = ""
            txtZoneSR.Text = ""
        End Sub


        Private Sub dgZones_PageIndexChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgZones.PageIndexChanged
            dgZones.CurrentPageIndex = e.NewPageIndex
            RefreshDataSources()
        End Sub

        Protected Sub btnSelectZone_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim zone As FixedShippingZone = CCFramework.Commerce.FixedShippingZoneController.GetShippingZone(id)

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
                CCFramework.Commerce.FixedShippingZoneController.Delete(ID)

                lblStatus.Text = "You have successfully deleted the Zone (ID: " & ID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to delete the Zone (ID: " & ID.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub


        Private Sub dgRates_PageIndexChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgRates.PageIndexChanged
            dgRates.CurrentPageIndex = e.NewPageIndex
            RefreshDataSources()
        End Sub

        Protected Sub btnSelectRate_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim rate As FixedShippingRate = CCFramework.Commerce.FixedShippingRateController.GetShippingRate(id)

            hfRateID.Value = rate.ID.ToString

            txtWeightLBS.Text = rate.WeightLBS.ToString
            txtWeightKG.Text = rate.WeightKGS.ToString
            txtZoneSR.Text = rate.Zone.ToString
            txtCost.Text = rate.Cost.ToString
        End Sub

        Protected Sub btnDeleteRate_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim ID As Integer = Integer.Parse(e.CommandArgument.ToString)

            Try
                CCFramework.Commerce.FixedShippingRateController.Delete(ID)

                lblStatus.Text = "You have successfully deleted the Rate (ID: " & ID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to delete the Rate (ID: " & ID.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub

    End Class
End Namespace