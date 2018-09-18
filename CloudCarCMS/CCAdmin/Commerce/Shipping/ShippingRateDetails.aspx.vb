Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce.Shipping

    Partial Public Class ShippingRateDetails
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub RefreshDataSources()
            RatesGridView.DataSource = FixedShippingRateController.GetShippingRates()
            RatesGridView.DataBind()
        End Sub

        Private Sub btnAddRate_Command(ByVal sender As Object, ByVal e As CommandEventArgs) Handles btnAddRate.Command
            If Page.IsValid Then
                Dim ID As Integer

                If Not Integer.TryParse(hfRateID.Value, ID) Then
                    FixedShippingRateController.Create(Decimal.Parse(txtWeightLBS.Text), Decimal.Parse(txtWeightKG.Text), Integer.Parse(txtZoneSR.Text), Decimal.Parse(txtCost.Text))

                    lblStatus.Text = "You have sucessfully created a new rate"
                    lblStatus.Visible = True
                Else
                    FixedShippingRateController.Update(Integer.Parse(hfRateID.Value), Decimal.Parse(txtWeightLBS.Text), Decimal.Parse(txtWeightKG.Text), Integer.Parse(txtZoneSR.Text), Decimal.Parse(txtCost.Text))

                    lblStatus.Text = "You have sucessfully updated the current rate"
                    lblStatus.Visible = True
                End If

                RefreshDataSources()
            End If
        End Sub

        Private Sub btnClearRate_Command(ByVal sender As Object, ByVal e As CommandEventArgs) Handles btnClearRate.Command
            hfRateID.Value = Nothing

            txtWeightLBS.Text = ""
            txtWeightKG.Text = ""
            txtCost.Text = ""
            txtZoneSR.Text = ""
        End Sub

        Private Sub RatesGridViewPageIndexChanging(ByVal Sender As Object, ByVal Args As GridViewPageEventArgs) Handles RatesGridView.PageIndexChanging
            RatesGridView.PageIndex = Args.NewPageIndex
            RefreshDataSources()
        End Sub

        Protected Sub btnSelectRate_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim rate As FixedShippingRate = FixedShippingRateController.GetShippingRate(id)

            hfRateID.Value = rate.ID.ToString

            txtWeightLBS.Text = rate.WeightLBS.ToString
            txtWeightKG.Text = rate.WeightKGS.ToString
            txtZoneSR.Text = rate.Zone.ToString
            txtCost.Text = rate.Cost.ToString
        End Sub

        Protected Sub btnDeleteRate_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim ID As Integer = Integer.Parse(e.CommandArgument.ToString)

            Try
                FixedShippingRateController.Delete(ID)

                lblStatus.Text = "You have successfully deleted the Rate (ID: " & ID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to delete the Rate (ID: " & ID.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub

    End Class

End Namespace