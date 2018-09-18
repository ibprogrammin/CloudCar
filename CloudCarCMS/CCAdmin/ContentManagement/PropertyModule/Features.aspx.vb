Imports CloudCar.CCFramework.Model

Namespace CCAdmin.ContentManagement.PropertyModule

    Partial Public Class Features
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub LoadFeature(ByVal FeatureId As Integer)
            Try
                Dim SelectedFeature As Feature = CCFramework.ContentManagement.FeatureController.GetElement(FeatureId)

                hfFeatureId.Value = SelectedFeature.Id.ToString

                txtName.Text = SelectedFeature.Name
                DetailsTextArea.InnerText = SelectedFeature.Details
            Catch Ex As Exception
                lblStatus.Text = Ex.ToString
                lblStatus.Visible = True
            End Try
        End Sub

        Private Sub SaveButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnSave.Click
            Dim FeatureId As Integer

            If Page.IsValid Then
                Dim Name As String = txtName.Text
                Dim Details As String = DetailsTextArea.InnerText

                If Integer.TryParse(hfFeatureId.Value, FeatureId) Then
                    CCFramework.ContentManagement.FeatureController.Update(FeatureId, Name, Details)

                    lblStatus.Text = "Your feature has been saved!"
                    lblStatus.Visible = True
                Else
                    CCFramework.ContentManagement.FeatureController.Create(Name, Details)

                    lblStatus.Text = "Your feature has been created!"
                    lblStatus.Visible = True

                    RefreshDataSources()
                End If
            End If
        End Sub

        Private Sub NewButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnNew.Click
            ClearControls()
        End Sub

        Private Sub RefreshDataSources()
            lbFeatures.Items.Clear()
            lbFeatures.Items.Add(New ListItem("Please select a feature...", ""))
            lbFeatures.AppendDataBoundItems = True
            lbFeatures.DataSource = CCFramework.ContentManagement.FeatureController.GetElements
            lbFeatures.DataBind()
        End Sub

        Private Sub ClearControls()
            hfFeatureId.Value = Nothing

            txtName.Text = ""
            DetailsTextArea.InnerText = ""
        End Sub

        Private Sub FeaturesListBoxSelectedIndexChanged(ByVal Sender As Object, ByVal E As EventArgs) Handles lbFeatures.SelectedIndexChanged
            If Not lbFeatures.SelectedValue = Nothing Then
                Dim FeatureId As Integer = Integer.Parse(lbFeatures.SelectedValue)

                LoadFeature(FeatureId)
            End If
        End Sub

        Private Sub btnDelete_Command(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles btnDelete.Command
            If Not hfFeatureId.Value = Nothing Then
                Dim FeatureId As Integer = Integer.Parse(hfFeatureId.Value)

                If CCFramework.ContentManagement.FeatureController.Delete(FeatureId) = True Then
                    lblStatus.Text = "The selected feature has been deleted permanently!"
                    lblStatus.Visible = True

                    RefreshDataSources()

                    ClearControls()
                Else
                    lblStatus.Text = "An error occured while trying to delete the current feature."
                    lblStatus.Visible = True
                End If
            Else
                lblStatus.Text = "Sorry but you do not have a feature selected."
                lblStatus.Visible = True
            End If
        End Sub

    End Class
End Namespace