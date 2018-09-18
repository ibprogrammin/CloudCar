Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce.Reports

    Partial Public Class MonthlyBranchReport
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                FillDefaultDates()
            End If
        End Sub

        Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGo.Click
            If Not ddlYearSelection.SelectedValue Is Nothing And Not ddlYearSelection.SelectedValue = String.Empty Then
                LoadReport()
            End If
        End Sub

        Private Sub btnDownloadCSV_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownloadCsv.Click
            DownloadCsvReport()
        End Sub

        Protected Sub lvBranchMonths_ItemDataBound(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles lvBranchMonths.ItemDataBound
            If e.Item.ItemType = ListViewItemType.DataItem Then
                Dim SelectedYear As Integer = Integer.Parse(ddlYearSelection.SelectedValue)
                Dim SelectedMonth As Integer = Integer.Parse(CType(e.Item.FindControl("hfMonth"), HiddenField).Value)

                If Not SelectedYear = Nothing And Not SelectedMonth = Nothing Then
                    Dim SalesBreakdownDataGrid As DataGrid = CType(e.Item.FindControl("dgSalesBreakdown"), DataGrid)

                    Dim CurrentReportData = From rd In GetReportData() Where rd.Year = SelectedYear And rd.Month = SelectedMonth Select rd

                    SalesBreakdownDataGrid.DataSource = CurrentReportData
                    SalesBreakdownDataGrid.DataBind()

                    Dim MonthlyData = GetMonthlyData(SelectedYear, SelectedMonth)

                    Dim litCost As Literal = CType(e.Item.FindControl("litCost"), Literal)
                    Dim litSales As Literal = CType(e.Item.FindControl("litSales"), Literal)
                    Dim litDiscounts As Literal = CType(e.Item.FindControl("litDiscounts"), Literal)
                    Dim litShipping As Literal = CType(e.Item.FindControl("litShipping"), Literal)
                    Dim litProfit As Literal = CType(e.Item.FindControl("litProfit"), Literal)

                    litCost.Text = String.Format("{0:C}", MonthlyData.Cost)
                    litSales.Text = String.Format("{0:C}", MonthlyData.Price)
                    litDiscounts.Text = String.Format("{0:C}", MonthlyData.Discount)
                    litShipping.Text = String.Format("{0:C}", MonthlyData.Shipping)
                    litProfit.Text = String.Format("{0:C}", MonthlyData.Profit)
                End If
            End If
        End Sub

        Private Sub LoadReport()
            Dim CurrentStoreDataContext As New CommerceDataContext

            Dim SelectedYear As Integer = Integer.Parse(ddlYearSelection.SelectedValue)

            litYear.Text = SelectedYear.ToString

            Dim SalesTable = From o In CurrentStoreDataContext.Orders Where o.OrderDate.Year = SelectedYear _
                    Where o.ApprovalState = 1 And (From oi In CurrentStoreDataContext.OrderItems Where oi.OrderID = o.ID Select oi).Count > 0 _
                    Group By Year = o.OrderDate.Year, Month = o.OrderDate.Month Into Group _
                    Order By Year, Month Ascending _
                    Select New With {Year, Month} Distinct

            lvBranchMonths.DataSource = SalesTable
            lvBranchMonths.DataBind()

            Dim YearlyData = GetYearlyData(SelectedYear)

            litYearlyCost.Text = String.Format("{0:C}", YearlyData.Cost)
            litYearlySales.Text = String.Format("{0:C}", YearlyData.Price)
            litYearlyDiscounts.Text = String.Format("{0:C}", YearlyData.Discount)
            litYearlyShipping.Text = String.Format("{0:C}", YearlyData.Shipping)
            litYearlyProfit.Text = String.Format("{0:C}", YearlyData.Profit)

            CurrentStoreDataContext.Dispose()

            lvBranchMonths.Visible = True
        End Sub

        Private Sub DownloadCsvReport()
            Dim CurrentStoreDataContext As New CommerceDataContext

            Dim CSVReport As New CCFramework.Core.CSVGenerator()

            CSVReport.AddColumn("Branch")
            CSVReport.AddColumn("Rep")
            CSVReport.AddColumn("Order #")
            CSVReport.AddColumn("Year")
            CSVReport.AddColumn("Month")
            CSVReport.AddColumn("Cost")
            CSVReport.AddColumn("Sales")
            CSVReport.AddColumn("Discounts")
            CSVReport.AddColumn("Shipping")
            CSVReport.AddColumn("Profit")

            Dim SelectedYear As Integer = Integer.Parse(ddlYearSelection.SelectedValue)

            Dim SalesTable = From o In CurrentStoreDataContext.Orders Where o.OrderDate.Year = SelectedYear _
                    Where o.ApprovalState = 1 And (From oi In CurrentStoreDataContext.OrderItems Where oi.OrderID = o.ID Select oi).Count > 0 _
                    Group By Year = o.OrderDate.Year, Month = o.OrderDate.Month Into Group _
                    Order By Year, Month Ascending _
                    Select New With {Year, Month} Distinct

            For Each item In SalesTable
                Dim SelectedMonth As Integer = item.Month

                If Not SelectedYear = Nothing And Not SelectedMonth = Nothing Then
                    Dim CurrentReportData = From rd In GetReportData() Where rd.Year = SelectedYear And rd.Month = SelectedMonth Select rd

                    For Each SalesItem In CurrentReportData
                        Dim CSVRow As DataRow = CSVReport.CSVData.NewRow

                        CSVRow.Item(0) = SalesItem.Branch
                        CSVRow.Item(1) = SalesItem.SalesRep
                        CSVRow.Item(2) = SalesItem.OrderId
                        CSVRow.Item(3) = SalesItem.Year
                        CSVRow.Item(4) = MonthName(SalesItem.Month)
                        CSVRow.Item(5) = SalesItem.Cost
                        CSVRow.Item(6) = SalesItem.Price
                        CSVRow.Item(7) = SalesItem.Discount
                        CSVRow.Item(8) = SalesItem.Shipping
                        CSVRow.Item(9) = SalesItem.Profit

                        CSVReport.AddRow(CSVRow)
                    Next

                    Dim MonthlyData = GetMonthlyData(SelectedYear, SelectedMonth)

                    Dim CSVMonthlyTotalRow As DataRow = CSVReport.CSVData.NewRow

                    CSVMonthlyTotalRow.Item(0) = MonthName(SelectedMonth) & " Totals"
                    CSVMonthlyTotalRow.Item(1) = ""
                    CSVMonthlyTotalRow.Item(2) = ""
                    CSVMonthlyTotalRow.Item(3) = MonthlyData.Year
                    CSVMonthlyTotalRow.Item(4) = MonthName(SelectedMonth)
                    CSVMonthlyTotalRow.Item(5) = MonthlyData.Cost
                    CSVMonthlyTotalRow.Item(6) = MonthlyData.Price
                    CSVMonthlyTotalRow.Item(7) = MonthlyData.Discount
                    CSVMonthlyTotalRow.Item(8) = MonthlyData.Shipping
                    CSVMonthlyTotalRow.Item(9) = MonthlyData.Profit

                    CSVReport.AddRow(CSVMonthlyTotalRow)

                    CSVMonthlyTotalRow = CSVReport.CSVData.NewRow

                    CSVReport.AddRow(CSVMonthlyTotalRow)
                End If
            Next

            Dim YearlyData = GetYearlyData(SelectedYear)

            Dim CSVYearlyTotalRow As DataRow = CSVReport.CSVData.NewRow

            CSVYearlyTotalRow.Item(0) = SelectedYear & " Totals"
            CSVYearlyTotalRow.Item(1) = "Total"
            CSVYearlyTotalRow.Item(2) = ""
            CSVYearlyTotalRow.Item(3) = SelectedYear
            CSVYearlyTotalRow.Item(4) = ""
            CSVYearlyTotalRow.Item(5) = YearlyData.Cost
            CSVYearlyTotalRow.Item(6) = YearlyData.Price
            CSVYearlyTotalRow.Item(7) = YearlyData.Discount
            CSVYearlyTotalRow.Item(8) = YearlyData.Shipping
            CSVYearlyTotalRow.Item(9) = YearlyData.Profit

            CSVReport.AddRow(CSVYearlyTotalRow)

            Dim CsvContent As String = CSVReport.GenerateCSVString

            'Request.Form.Set("d", CsvContent)
            'hfReportData.Value = CsvContent
            Session.Add("d", CsvContent)
            Response.Redirect("/Report/SalesReport.csv")
            'Page.Server.Transfer("/Download/Report/SalesReport.csv", True)
        End Sub

        Private Function GetReportData() As ienumerable(Of ReportData)
            Dim CurrentStoreDataContext As New CommerceDataContext

            Dim CurrentReportData = From o In CurrentStoreDataContext.Orders _
                                    Join a In CurrentStoreDataContext.Addresses On o.SAddressID Equals a.ID _
                    Where o.ApprovalState = 1 And (From oi In CurrentStoreDataContext.OrderItems Where oi.OrderID = o.ID Select oi).Count > 0 _
                    Let Year = o.OrderDate.Year _
                    Let Month = o.OrderDate.Month _
                    Let Branch = If(o.DistributorId.HasValue, CType(o.DistributorId, String), a.PCZIP) _
                    Let SalesRep = If(o.PromoCode Is Nothing Or o.PromoCode = String.Empty, "No Rep", _
                                      (From pc In CurrentStoreDataContext.PromoCodes Where String.Equals(pc.Code, o.PromoCode) Select pc.SalesRep).FirstOrDefault) _
                    Let OrderId = o.ID _
                    Let Cost = (From oi In CurrentStoreDataContext.OrderItems _
                    Join p In CurrentStoreDataContext.Products On p.ID Equals oi.ProductID _
                    Where oi.OrderID = o.ID _
                    Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Cost = p.Cost}).Sum(Function(f) CType(f.Cost * f.Quantity, Decimal?)) _
                    Let Price = (From oi In CurrentStoreDataContext.OrderItems _
                    Where oi.OrderID = o.ID _
                    Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Price = oi.Price}).Sum(Function(f) CType(f.Price * f.Quantity, Decimal?)) _
                    Let Savings = o.Discount _
                    Let Shipping = o.ShipCharge _
                    Group By Year, Month, Branch, SalesRep, OrderId Into Group _
                    Select New With {Year, Month, Branch, SalesRep, OrderId, _
                    .Discount = Group.Sum(Function(f) f.Savings), _
                    .Cost = Group.Sum(Function(f) f.Cost), _
                    .Price = Group.Sum(Function(f) f.Price), _
                    .Shipping = Group.Sum(Function(f) f.Shipping), _
                    .Profit = Group.Sum(Function(f) f.Price) - Group.Sum(Function(f) f.Cost) - Group.Sum(Function(f) f.Savings)}

            Dim AdjustedReportData = From rd In CurrentReportData.AsEnumerable _
                    Let Year = rd.Year _
                    Let Month = rd.Month _
                    Let Branch = FixedShippingZoneController.GetBranchCity(rd.Branch) _
                    Let SalesRep = rd.SalesRep _
                    Let OrderId = rd.OrderId _
                    Let Discount = rd.Discount _
                    Let Cost = rd.Cost _
                    Let Price = rd.Price _
                    Let Shipping = rd.Shipping _
                    Let Profit = rd.Profit _
                    Group By Year, Month, Branch, SalesRep, OrderId Into Group _
                    Select New ReportData With {.Year = Year, .Month = Month, .Branch = Branch, .SalesRep = SalesRep, .OrderId = OrderId, _
                    .Discount = Group.Sum(Function(f) f.Discount), _
                    .Cost = Group.Sum(Function(f) f.Cost).Value, _
                    .Price = Group.Sum(Function(f) f.Price).Value, _
                    .Shipping = Group.Sum(Function(f) f.Shipping), _
                    .Profit = Group.Sum(Function(f) f.Profit).Value}

            Return AdjustedReportData.AsEnumerable

            'CurrentStoreDataContext.Dispose()
        End Function

        Private Function GetMonthlyData(ByVal CurrentYear As Integer, ByVal CurrentMonth As Integer)
            GetMonthlyData = (From st In GetReportData.AsEnumerable _
                Where st.Year = CurrentYear And st.Month = CurrentMonth _
                Let Year = st.Year _
                Let Month = st.Month _
                Let Discount = st.Discount _
                Let Cost = st.Cost _
                Let Price = st.Price _
                Let Shipping = st.Shipping _
                Let Profit = st.Profit _
                Group By Year, Month Into Group _
                Select New With {Year, Month, _
                    .Discount = Group.Sum(Function(f) f.Discount), _
                    .Cost = Group.Sum(Function(f) f.Cost), _
                    .Price = Group.Sum(Function(f) f.Price), _
                    .Shipping = Group.Sum(Function(f) f.Shipping), _
                    .Profit = Group.Sum(Function(f) f.Profit) _
                    }).FirstOrDefault
        End Function

        Private Function GetYearlyData(ByVal CurrentYear As Integer)
            GetYearlyData = (From st In GetReportData.AsEnumerable _
                Where st.Year = CurrentYear _
                Let Year = st.Year _
                Let Discount = st.Discount _
                Let Cost = st.Cost _
                Let Price = st.Price _
                Let Shipping = st.Shipping _
                Let Profit = st.Profit _
                Group By Year Into Group _
                Select New With {Year, _
                    .Discount = Group.Sum(Function(f) f.Discount), _
                    .Cost = Group.Sum(Function(f) f.Cost), _
                    .Price = Group.Sum(Function(f) f.Price), _
                    .Shipping = Group.Sum(Function(f) f.Shipping), _
                    .Profit = Group.Sum(Function(f) f.Profit) _
                    }).FirstOrDefault
        End Function

        Protected Sub FillDefaultDates()
            Dim EarliestYear As Integer = OrderController.GetEarliestOrderYear
            Dim LatestYear As Integer = OrderController.GetLatestOrderYear

            ddlYearSelection.Items.Add(New ListItem("Year", ""))

            For i As Integer = EarliestYear To LatestYear
                ddlYearSelection.Items.Add(New ListItem(i.ToString, i.ToString))
            Next
        End Sub

        Public Class ReportData
            Private _Year As Integer
            Private _Month As Integer
            Private _Branch As String
            Private _SalesRep As String
            Private _OrderId As Integer
            Private _Discount As Decimal
            Private _Cost As Decimal
            Private _Price As Decimal
            Private _Shipping As Decimal
            Private _Profit As Decimal

            Public Property Year() As Integer
                Get
                    Return _Year
                End Get
                Set(ByVal value As Integer)
                    _Year = value
                End Set
            End Property

            Public Property Month() As Integer
                Get
                    Return _Month
                End Get
                Set(ByVal value As Integer)
                    _Month = value
                End Set
            End Property

            Public Property Branch() As String
                Get
                    Return _Branch
                End Get
                Set(ByVal value As String)
                    _Branch = value
                End Set
            End Property

            Public Property SalesRep() As String
                Get
                    Return _SalesRep
                End Get
                Set(ByVal value As String)
                    _SalesRep = value
                End Set
            End Property

            Public Property OrderId() As Integer
                Get
                    Return _OrderId
                End Get
                Set(ByVal value As Integer)
                    _OrderId = value
                End Set
            End Property

            Public Property Discount() As Decimal
                Get
                    Return _Discount
                End Get
                Set(ByVal value As Decimal)
                    _Discount = value
                End Set
            End Property

            Public Property Cost() As Decimal
                Get
                    Return _Cost
                End Get
                Set(ByVal value As Decimal)
                    _Cost = value
                End Set
            End Property

            Public Property Price() As Decimal
                Get
                    Return _Price
                End Get
                Set(ByVal value As Decimal)
                    _Price = value
                End Set
            End Property

            Public Property Shipping() As Decimal
                Get
                    Return _Shipping
                End Get
                Set(ByVal value As Decimal)
                    _Shipping = value
                End Set
            End Property

            Public Property Profit() As Decimal
                Get
                    Return _Profit
                End Get
                Set(ByVal value As Decimal)
                    _Profit = value
                End Set
            End Property

        End Class

    End Class
End Namespace