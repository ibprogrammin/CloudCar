Imports CloudCar.CCFramework.ContentManagement.CallToActionModule
Imports CloudCar.CCFramework

Namespace CCAdmin.ContentManagement.CallToActionModule

    Public Class CallToAction
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load

        End Sub

        Private Sub NewButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles NewButton.Click
            DetailsFormPlaceHolder.Visible = True
            CallToActionRepeater.Visible = False
            NewButton.Visible = False
        End Sub

        Private Sub SaveButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles SaveButton.Click
            Dim CurrentCallToActionId As Integer

            If Integer.TryParse(CallToActionIdHiddenField.Value, CurrentCallToActionId) Then
                CallToActionController.Update(CurrentCallToActionId, HeadingTextBox.Text, DetailsTextArea.InnerText, ButtonTextTextBox.Text, ImageUrlTextBox.Text, LinkUrlTextBox.Text)
            Else
                CurrentCallToActionId = CallToActionController.Create(HeadingTextBox.Text, DetailsTextArea.InnerText, ButtonTextTextBox.Text, ImageUrlTextBox.Text, LinkUrlTextBox.Text)
            End If

            If Not CurrentCallToActionId = Nothing Then
                StatusMessageLabel.Text = "Call To Action item was saved successfully"
                StatusMessageLabel.Visible = True

                LoadItem(CurrentCallToActionId)
            Else
                StatusMessageLabel.Text = "There was an issue trying to create the Call To Action item"
                StatusMessageLabel.Visible = True
            End If

            CallToActionRepeater.DataBind()

            DetailsFormPlaceHolder.Visible = False
            CallToActionRepeater.Visible = True
            NewButton.Visible = True
        End Sub

        Private Sub CancelButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles CancelButton.Click
            DetailsFormPlaceHolder.Visible = False
            CallToActionRepeater.Visible = True
            NewButton.Visible = True

            ClearForm()
        End Sub

        Protected Sub SelectItemButtonClick(ByVal Sender As Object, ByVal Args As EventArgs)
            Dim CurrentItemId As Integer = Integer.Parse(CType(Sender, LinkButton).Attributes("ItemId"))

            If CurrentItemId = Nothing Then
                StatusMessageLabel.Text = "There was an error loading the item"
                StatusMessageLabel.Visible = True
            Else
                DetailsFormPlaceHolder.Visible = True
                LoadItem(CurrentItemId)
            End If

        End Sub

        Protected Sub DeleteItemButtonClick(ByVal Sender As Object, ByVal Args As EventArgs)
            Dim CurrentItemId As Integer = Integer.Parse(CType(Sender, LinkButton).Attributes("ItemId"))

            If Not CurrentItemId = Nothing Then
                If CallToActionController.Delete(CurrentItemId) Then
                    StatusMessageLabel.Text = "The item was successfully deleted"
                    StatusMessageLabel.Visible = True

                    CallToActionRepeater.DataBind()
                End If
            Else
                StatusMessageLabel.Text = "There was an issue trying to delete the current item"
                StatusMessageLabel.Visible = True
            End If
        End Sub

        Private Sub ClearForm()
            CallToActionIdHiddenField.Value = Nothing
            HeadingTextBox.Text = ""
            DetailsTextArea.InnerText = ""
            ButtonTextTextBox.Text = ""
            ImageUrlTextBox.Text = ""
        End Sub

        Private Sub LoadItem(ItemId As Integer)
            ClearForm()

            Dim CurrentCallToAction As Model.CallToAction = CallToActionController.GetItem(ItemId)

            CallToActionIdHiddenField.Value = CurrentCallToAction.Id.ToString

            HeadingTextBox.Text = CurrentCallToAction.Heading
            DetailsTextArea.InnerText = CurrentCallToAction.Details
            ButtonTextTextBox.Text = CurrentCallToAction.ButtonText
            ImageUrlTextBox.Text = CurrentCallToAction.ImageUrl
            LinkUrlTextBox.Text = CurrentCallToAction.LinkUrl
        End Sub

        Private Sub CallToActionRepeaterLoad(Sender As Object, Args As EventArgs) Handles CallToActionRepeater.Load
            CallToActionRepeater.DataSource = CallToActionController.GetItems
            CallToActionRepeater.DataBind()
        End Sub

    End Class

End Namespace