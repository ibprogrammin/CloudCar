Imports System.Net.Mail
Imports CloudCar.CCFramework.ContentManagement

Namespace CCControls.Blogging

    Partial Public Class MailingListControl
        Inherits UserControl

        Private Sub JoinMailingListButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles JoinMailingListButton.Command
            If Not EmailAddressTextBox.Text = Nothing Then
                Dim CurrentEmailAddress As New MailAddress(EmailAddressTextBox.Text)

                SubscriptionController.Add(CurrentEmailAddress)

                JoinMailingListPlaceHolder.Visible = False
                ThankYouPlaceHolder.Visible = True
            End If
        End Sub

    End Class

End Namespace