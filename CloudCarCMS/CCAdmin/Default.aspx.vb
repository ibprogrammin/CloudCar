Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.GAConnect
Imports CloudCar.CCFramework.Core
Imports Google.Apis.Analytics

Namespace CCAdmin
    Public Class Dashboard
        Inherits Page

        Private GraphItemCount As Integer = 4

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                If CCFramework.Core.Settings.StoreAdminEnabled Then
                    Try
                        TopSellersRepeater.DataSource = New ProductController().GetTopSellers(GraphItemCount)
                        TopSellersRepeater.DataBind()

                        InventoryRepeater.DataSource = New ProductController().GetLowInventory(GraphItemCount)
                        InventoryRepeater.DataBind()
                    Catch Ex As Exception

                    End Try
                Else
                    phSales.Visible = False
                    phStoreControls.Visible = False
                End If

                CurrentStatus()
            End If
        End Sub

        Private Sub CurrentStatus()
            Dim UsersOnline As Integer = CInt(Application("userCount"))
            Dim TotalUsers As Integer = Membership.GetAllUsers.Count
            Dim LoggedInUsers As Integer = Membership.GetNumberOfUsersOnline
            Dim Guests As Integer = UsersOnline - LoggedInUsers

            GuestUsersLiteral.Text = Guests.ToString
            RegisteredUsersLiteral.Text = LoggedInUsers.ToString
            TotalUsersLiteral.Text = UsersOnline.ToString
            'litRegisteredUsers.Text = TotalUsers

            Dim UnshippedOrders As Integer = OrderController.GetUnshippedOrderCount()
            litUnshippedOrders.Text = UnshippedOrders.ToString

            Dim NewLeads As Integer = CCFramework.ContentManagement.SalesInquiryController.GetNewInquiriesCount()
            litNewLeads.Text = NewLeads.ToString
        End Sub

        Private Sub RestartApplicationButtonClick(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles RestartApplicationButton.Command
            HttpRuntime.UnloadAppDomain()
        End Sub

        Public Function GetMonthlyVisitData() As String
            'Dim Analytics As New AnalyticsAccessor("ga:24382466", "48307761651.apps.googleusercontent.com", "C:\privatekey.p12", "P5gv51AQxT3lBRzj7j76DTWS")
            'Dim Data As v3.Data.GAData = Analytics.GetData(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"),
            '                                       DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"),
            '                                       "ga:NewVisitors",
            '                                       "ga:Date")

            Dim CurrentGaAccessor As New GAAccessor

            CurrentGaAccessor.AddDimension(GADimension.Date)
            CurrentGaAccessor.AddMetric(GAMetric.NewVisits)
            CurrentGaAccessor.AddMetric(GAMetric.Bounces)

            Try
                CurrentGaAccessor.GetAnalyticsInfo("24382466", DateTime.Now.AddMonths(-1), DateTime.Now, 30)
            Catch ex As Net.WebException
                '    Response.Write(GABaseData.AuthenticationKey)
                '    Response.Write(ex.Message)
                '    Response.Write(ex.StackTrace)
            End Try

            Return CurrentGaAccessor.GetJsChartData()
        End Function

    End Class

End Namespace