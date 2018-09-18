Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Core

Namespace CCAdmin

    Partial Public Class AdminDefault
        Inherits Page

        Private GraphItemCount As Integer = 7

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                If CCFramework.Core.Settings.StoreAdminEnabled Then
                    GenerateMonthlySalesChartFlot()
                    GenerateDailySalesChartFlot()
                    'GenerateMonthlySalesChart()
                    'GenerateDailySalesChart()

                    Try
                        rptTopSellers.DataSource = New ProductController().GetTopSellers(GraphItemCount)
                        rptTopSellers.DataBind()

                        rptInventory.DataSource = New ProductController().GetLowInventory(GraphItemCount)
                        rptInventory.DataBind()
                    Catch ex As Exception
                        'phGraphs.Visible = False
                    End Try
                Else
                    phSales.Visible = False
                    phStoreControls.Visible = False
                End If

                CurrentStatus()
            End If
        End Sub

        Public MonthlyPlots As String
        Public DailyPlots As String

        Private Sub GenerateMonthlySalesChartFlot()
            Dim sb As New StringBuilder

            sb.Append("[")

            Dim monthlysales As List(Of MonthlySalesSummary) = New OrderController().GetMonthlySales(GraphItemCount)

            For Each item As MonthlySalesSummary In monthlysales
                Dim period As New Date(item.Year, item.Month, 1)

                'MonthlySalesChart.Series("Series1").Points.AddXY(period, item.Sales)

                If ReferenceEquals(item, monthlysales.Last) Then
                    sb.Append("[" & GetJavascriptTimestamp(period) & "," & item.Sales & "]")
                    'sb.Append("[" & (period.ToUniversalTime.Ticks / 10000) & "," & item.Sales & "]")
                Else
                    sb.Append("[" & GetJavascriptTimestamp(period) & "," & item.Sales & "],")
                    'sb.Append("[" & (period.ToUniversalTime.Ticks / 10000) & "," & item.Sales & "],")
                End If
            Next

            sb.Append("]")

            MonthlyPlots = sb.ToString
        End Sub

        Private Sub GenerateDailySalesChartFlot()
            Dim sb As New StringBuilder

            sb.Append("[")

            Dim dailysales As List(Of SalesSummary) = New OrderController().GetDailySales(GraphItemCount)

            For Each item As SalesSummary In dailysales
                Dim period As New Date(item.Year, item.Month, item.Day)

                If ReferenceEquals(item, dailysales.Last) Then
                    sb.Append("[" & GetJavascriptTimestamp(period) & "," & item.Sales & "]")
                Else
                    sb.Append("[" & GetJavascriptTimestamp(period) & "," & item.Sales & "],")
                End If
            Next

            sb.Append("]")

            DailyPlots = sb.ToString
        End Sub


        Private Sub GenerateMonthlySalesChart()
            Dim sb As New StringBuilder

            sb.Append("<script type='text/javascript'>")
            sb.Append("$.jqplot('monthlysales', [[")

            Dim monthlysales As List(Of MonthlySalesSummary) = New OrderController().GetMonthlySales(GraphItemCount)
            Dim topMonth As Decimal = 0
            Dim bottomMonth As Decimal = 0

            For Each item As MonthlySalesSummary In monthlysales
                Dim period As New Date(item.Year, item.Month, 1)

                'MonthlySalesChart.Series("Series1").Points.AddXY(period, item.Sales)

                If ReferenceEquals(item, monthlysales.Last) Then
                    sb.Append("['" & period.ToString("yyyy-MM-dd") & "'," & item.Sales & "]")
                Else
                    sb.Append("['" & period.ToString("yyyy-MM-dd") & "'," & item.Sales & "],")
                End If

                If item.Sales > topMonth Then
                    topMonth = item.Sales
                End If

                If item.Sales < bottomMonth Then
                    bottomMonth = item.Sales
                End If
            Next

            If monthlysales.Count > 0 Then
                Dim firstMonth As New Date(monthlysales.First.Year, monthlysales.First.Month, 1)

                If firstMonth.Month = 1 Then
                    firstMonth.AddMonths(11)
                    firstMonth.AddYears(-1)
                Else
                    firstMonth.AddMonths(-1)
                End If

                topMonth += 20
                bottomMonth -= 20

                sb.Append("]], {")
                sb.Append("axes:{")
                sb.Append("yaxis:{min:" & bottomMonth & ", max:" & topMonth & ", tickOptions:{formatString:'$%.2f', fontSize:'9px', fontFamily:'Arial'}}, ")
                sb.Append("xaxis:{renderer:$.jqplot.DateAxisRenderer, rendererOptions:{tickRenderer:$.jqplot.CanvasAxisTickRenderer}, tickOptions:{formatString:'%b %Y', fontSize:'9px', fontFamily:'Arial'}, min:'" & firstMonth.ToString("MMM dd, yyyy") & "', tickInterval:'1 month'}")
                sb.Append("}, ")
                sb.Append("series: [{color:'#5FAB78', markerOptions:{style:'circle'}}], ")
                sb.Append("legend:{show:false}, ")
                sb.Append("highlighter:{sizeAdjust:1, fadeTooltip:true}, ")
                sb.Append("cursor:{show:false}, ")
                sb.Append("grid:{drawGridLines:true, gridLineColor:'#cccccc', background:'#fffdf6', borderColor:'#999999', borderWidth:1.0, shadow:false, renderer:$.jqplot.CanvasGridRenderer, rendererOptions:{} }")
                sb.Append("});")

                sb.Append("</script>")

                litWeeklySalesScript.Text = sb.ToString

                'phGraphs.Visible = True
            End If
        End Sub

        Private Sub GenerateDailySalesChart()
            Dim sb As New StringBuilder

            sb.Append("<script type='text/javascript'>")
            sb.Append("$.jqplot('dailysales', [[")

            Dim dailysales As List(Of SalesSummary) = New OrderController().GetDailySales(GraphItemCount)
            Dim topDay As Decimal = 0
            Dim bottomDay As Decimal = 0

            For Each item As SalesSummary In dailysales
                Dim period As New Date(item.Year, item.Month, item.Day)

                If ReferenceEquals(item, dailysales.Last) Then
                    sb.Append("['" & period.ToString("yyyy-MM-dd") & "'," & item.Sales & "]")
                Else
                    sb.Append("['" & period.ToString("yyyy-MM-dd") & "'," & item.Sales & "],")
                End If

                If item.Sales > topDay Then
                    topDay = item.Sales
                End If

                If item.Sales < bottomDay Then
                    bottomDay = item.Sales
                End If
            Next

            If dailysales.Count > 0 Then
                Dim firstDay As New Date(dailysales.First.Year, dailysales.First.Month, dailysales.First.Day)
                firstDay.AddDays(-1)

                topDay += 20
                bottomDay -= 20

                sb.Append("]], {")
                sb.Append("axes:{")
                sb.Append("yaxis:{min:" & bottomDay & ", max:" & topDay & ", tickOptions:{formatString:'$%.2f', fontSize:'9px', fontFamily:'Arial'}}, ")
                sb.Append("xaxis:{renderer:$.jqplot.DateAxisRenderer, rendererOptions:{tickRenderer:$.jqplot.CanvasAxisTickRenderer}, tickOptions:{formatString:'%b %e, %Y', fontSize:'9px', fontFamily:'Arial'}, min:'" & firstDay.ToString("MMM dd, yyyy") & "'}")
                sb.Append("}, ")
                sb.Append("series: [{color:'#5FAB78', markerOptions:{style:'circle'}}], ")
                sb.Append("legend:{show:false}, ")
                sb.Append("highlighter:{sizeAdjust:1, fadeTooltip:true}, ")
                sb.Append("cursor:{show:false}, ")
                sb.Append("grid:{drawGridLines:true, gridLineColor:'#cccccc', background:'#fffdf6', borderColor:'#999999', borderWidth:1.0, shadow:false, renderer:$.jqplot.CanvasGridRenderer, rendererOptions:{} }")
                sb.Append("});")

                sb.Append("</script>")

                litDailySalesScript.Text = sb.ToString
            End If
        End Sub

        Private Sub CurrentStatus()
            Dim UsersOnline As Integer = Integer.Parse(Application("userCount"))
            Dim TotalUsers As Integer = Membership.GetAllUsers.Count
            Dim LoggedInUsers As Integer = Membership.GetNumberOfUsersOnline

            litLoggedInUsers.Text = LoggedInUsers
            litTotalUsers.Text = UsersOnline
            litRegisteredUsers.Text = TotalUsers

            Dim UnshippedOrders As Integer = OrderController.GetUnshippedOrderCount()
            litUnshippedOrders.Text = UnshippedOrders

            Dim NewLeads As Integer = CCFramework.ContentManagement.SalesInquiryController.GetNewInquiriesCount()
            litNewLeads.Text = NewLeads
        End Sub


        Private Const TicksPerSecond As Long = 1000

        Public Shared Function GetJavascriptTimestamp(ByVal Input As DateTime) As Long

            Dim StartSpanDate As TimeSpan = New TimeSpan(DateTime.Parse("1/1/1970").Ticks)
            Dim TotalSpanTime As DateTime = Input.Subtract(StartSpanDate)

            Return (TotalSpanTime.Ticks / 10000) + (TicksPerSecond * 60 * 60 * 24)

        End Function

        Private Sub btnRestartApplication_Command(ByVal Sender As Object, ByVal E As CommandEventArgs) Handles btnRestartApplication.Command
            HttpRuntime.UnloadAppDomain()
        End Sub

    End Class
End Namespace