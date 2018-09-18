Namespace CCControls

    Partial Public Class SubscribeControl
        Inherits UserControl

        Private Sub SubscribeButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles SubscribeButton.Click
            Dim CurrentMailAddress As Net.Mail.MailAddress

            Try
                CurrentMailAddress = New Net.Mail.MailAddress(SubscriberEmailTextBox.Text)

                Try
                    CCFramework.ContentManagement.SubscriptionController.Add(CurrentMailAddress)

                Catch Ex As Exception
                    MessagePopUp.Message = "This address is already subscribed"
                    MessagePopUp.Show()
                End Try

            Catch Ex As Exception
                MessagePopUp.Message = "This is not a valid email address"
                MessagePopUp.Show()
            Finally
                CurrentMailAddress = Nothing
            End Try

        End Sub
    End Class

End Namespace