Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCAdmin.Commerce.Reports

    Partial Public Class ProductSalesReport
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                FillDefaultDates()
            End If
        End Sub

        Protected Sub btnGo_Click(ByVal Sender As Object, ByVal Args As EventArgs) Handles btnGo.Click
            If Not ddlYearSelection.SelectedValue Is Nothing And Not ddlYearSelection.SelectedValue = String.Empty Then
                LoadReport()
            End If
        End Sub

        Private Sub btnDownloadCSV_Click(ByVal Sender As Object, ByVal Args As EventArgs) Handles btnDownloadCsv.Click
            DownloadCsvReport()
        End Sub

        Protected Sub lvBranchMonths_ItemDataBound(ByVal Sender As Object, ByVal Args As ListViewItemEventArgs) Handles lvBranchMonths.ItemDataBound
            If Args.Item.ItemType = ListViewItemType.DataItem Then
                Dim SelectedYear As Integer = Integer.Parse(ddlYearSelection.SelectedValue)
                Dim SelectedMonth As Integer = Integer.Parse(CType(Args.Item.FindControl("hfMonth"), HiddenField).Value)

                If Not SelectedYear = Nothing And Not SelectedMonth = Nothing Then
                    Dim SalesBreakdownDataGrid As DataGrid = CType(Args.Item.FindControl("dgSalesBreakdown"), DataGrid)

                    Dim CurrentReportData = From rd In GetReportData() Where rd.Year = SelectedYear And rd.Month = SelectedMonth Select rd

                    SalesBreakdownDataGrid.DataSource = CurrentReportData
                    SalesBreakdownDataGrid.DataBind()

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

            lvBranchMonths.Visible = True

            CurrentStoreDataContext.Dispose()
        End Sub

        Private Sub DownloadCsvReport()
            Dim CurrentStoreDataContext As New CommerceDataContext

            Dim CSVReport As New CSVGenerator()

            CSVReport.AddColumn("Year")
            CSVReport.AddColumn("Month")
            CSVReport.AddColumn("Product")
            CSVReport.AddColumn("Quantity")
            CSVReport.AddColumn("Cost")
            CSVReport.AddColumn("Price")
            CSVReport.AddColumn("Total")

            Dim SelectedYear As Integer = Integer.Parse(ddlYearSelection.SelectedValue)

            Dim SalesTable = From o In CurrentStoreDataContext.Orders Where o.OrderDate.Year = SelectedYear _
                    Where o.ApprovalState = 1 And (From oi In CurrentStoreDataContext.OrderItems Where oi.OrderID = o.ID Select oi).Count > 0 _
                    Group By Year = o.OrderDate.Year, Month = o.OrderDate.Month Into Group _
                    Order By Year, Month Ascending _
                    Select New With {Year, Month} Distinct

            For Each item In SalesTable
                Dim SelectedMonth As Integer = item.Month

                If Not SelectedYear = Nothing And Not SelectedMonth = Nothing Then
                    Dim CurrentReportData = From rd In GetReportData() _
                                            Where rd.Year = SelectedYear _
                                            And rd.Month = SelectedMonth _
                                            Select rd

                    For Each SalesItem In CurrentReportData
                        Dim CSVRow As DataRow = CSVReport.CSVData.NewRow

                        CSVRow.Item(0) = SalesItem.Year
                        CSVRow.Item(1) = MonthName(SalesItem.Month)
                        CSVRow.Item(2) = SalesItem.Product
                        CSVRow.Item(3) = SalesItem.Quantity
                        CSVRow.Item(4) = SalesItem.Cost
                        CSVRow.Item(5) = SalesItem.Price
                        CSVRow.Item(6) = SalesItem.Total

                        CSVReport.AddRow(CSVRow)
                    Next

                    Dim CSVMonthlyTotalRow As DataRow = CSVReport.CSVData.NewRow

                    CSVReport.AddRow(CSVMonthlyTotalRow)
                End If
            Next

            CurrentStoreDataContext.Dispose()

            Dim CsvContent As String = CSVReport.GenerateCSVString

            Session.Add("d", CsvContent)
            Response.Redirect("/Report/SalesReport.csv")
        End Sub

        Private Function GetReportData() As IEnumerable(Of ProductReportData)
            Dim CurrentStoreDataContext As New CommerceDataContext

            Dim CurrentReportData = From oi In CurrentStoreDataContext.OrderItems _
                    Join o In CurrentStoreDataContext.Orders On oi.OrderID Equals o.ID _
                    Join p In CurrentStoreDataContext.Products On oi.ProductID Equals p.ID _
                    Let Year = o.OrderDate.Year _
                    Let Month = o.OrderDate.Month _
                    Let Product = p.Name _
                    Let Quantity = oi.Quantity _
                    Let Cost = p.Cost _
                    Let Price = oi.Price _
                    Group By Year, Month, Product, Price, Cost Into Group _
                    Order By Year, Month, Product, Cost, Price _
                    Select New ProductReportData With { _
                        .Year = Year, _
                        .Month = Month, _
                        .Product = Product, _
                        .Quantity = Group.Sum(Function(f) f.Quantity), _
                        .Cost = Cost, _
                        .Price = Price}

            GetReportData = CurrentReportData.AsEnumerable
        End Function

        Protected Sub FillDefaultDates()
            Dim EarliestYear As Integer = OrderController.GetEarliestOrderYear
            Dim LatestYear As Integer = OrderController.GetLatestOrderYear

            ddlYearSelection.Items.Add(New ListItem("Year", ""))

            For i As Integer = EarliestYear To LatestYear
                ddlYearSelection.Items.Add(New ListItem(i.ToString, i.ToString))
            Next
        End Sub

        Public Class ProductReportData
            Private _Year As Integer
            Private _Month As Integer
            Private _Product As String
            Private _Quantity As Integer
            Private _Cost As Decimal
            Private _Price As Decimal

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

            Public Property Product() As String
                Get
                    Return _Product
                End Get
                Set(ByVal value As String)
                    _Product = value
                End Set
            End Property

            Public Property Quantity() As Integer
                Get
                    Return _Quantity
                End Get
                Set(ByVal value As Integer)
                    _Quantity = value
                End Set
            End Property

            Public Property Cost As Decimal
                Get
                    Return _Cost
                End Get
                Set(value As Decimal)
                    _Cost = value
                End Set
            End Property

            Public Property Price As Decimal
                Get
                    Return _Price
                End Get
                Set(value As Decimal)
                    _Price = value
                End Set
            End Property

            Public ReadOnly Property Total As Decimal
                Get
                    Return _Price * _Quantity
                End Get
            End Property

        End Class

    End Class

End Namespace