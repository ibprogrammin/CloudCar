Imports CloudCar.CCFramework.ContentManagement
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.ContentManagement
    Partial Public Class Subscribers
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load

        End Sub

        Protected Sub SubscribersGridViewLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles SubscribersGridView.Load
            SubscribersGridView.DataSource = SubscriptionController.GetSubscribers
            SubscribersGridView.DataBind()
        End Sub

        Private Sub DownloadCsvButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles DownloadCsvButton.Click
            Dim CsvContent As String = SubscriptionController.GetSubscriberCsv()

            Session.Add("d", CsvContent)

            Response.Redirect("/Report/SalesReport.csv")

        End Sub

    End Class
End Namespace