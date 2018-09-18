Imports CloudCar.CCFramework.Model
Imports System.Data.Linq
Imports CloudCar.CCFramework.Core

Namespace CCAdmin.Commerce.Reports

    Partial Public Class MonthlySales
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadMonthlyReport()
            End If
        End Sub

        Private Sub LoadMonthlyReport()
            MonthlySalesRepeater.DataSource = MonthlyReportData.GetMonthlyReportData()
            MonthlySalesRepeater.DataBind()
        End Sub

        Public Sub DownloadCsvButtonClick(Sender As Object, Args As EventArgs) Handles DownloadCsvButton.Click
            Dim CsvContent As String = MonthlyReportData.GetMonthlyReportCSV()

            Session.Add("d", CsvContent)
            Response.Redirect("/Report/SalesReport.csv")
        End Sub

    End Class

    Public Class MonthlyReportData
        Private _Year As Integer
        Private _Month As Integer
        Private _Orders As Integer
        Private _Items As Integer
        Private _Sales As Decimal
        Private _CostOfGoodsSold As Decimal
        Private _Profit As Decimal
        Private _ProfitMargin As Decimal
        Private _Shipping As Decimal
        Private _Taxes As Decimal
        Private _Discount As Decimal

        Public Property Year As Integer
            Get
                Return _Year
            End Get
            Set(Value As Integer)
                _Year = Value
            End Set
        End Property

        Public Property Month As Integer
            Get
                Return _Month
            End Get
            Set(Value As Integer)
                _Month = Value
            End Set
        End Property

        Public Property Orders As Integer
            Get
                Return _Orders
            End Get
            Set(Value As Integer)
                _Orders = Value
            End Set
        End Property

        Public Property Items As Integer
            Get
                Return _Items
            End Get
            Set(Value As Integer)
                _Items = Value
            End Set
        End Property

        Public Property Sales As Decimal
            Get
                Return _Sales
            End Get
            Set(Value As Decimal)
                _Sales = Value
            End Set
        End Property

        Public Property CostOfGoodsSold As Decimal
            Get
                Return _CostOfGoodsSold
            End Get
            Set(Value As Decimal)
                _CostOfGoodsSold = Value
            End Set
        End Property

        Public Property Profit As Decimal
            Get
                Return _Profit
            End Get
            Set(Value As Decimal)
                _Profit = Value
            End Set
        End Property

        Public Property ProfitMargin As Decimal
            Get
                Return _ProfitMargin
            End Get
            Set(Value As Decimal)
                _ProfitMargin = Value
            End Set
        End Property

        Public Property Shipping As Decimal
            Get
                Return _Shipping
            End Get
            Set(Value As Decimal)
                _Shipping = Value
            End Set
        End Property

        Public ReadOnly Property ShippingCost As Decimal
            Get
                Return Shipping / Convert.ToDecimal(CCFramework.Core.Settings.ShippingMargin)
            End Get
        End Property

        Public Property Taxes As Decimal
            Get
                Return _Taxes
            End Get
            Set(Value As Decimal)
                _Taxes = Value
            End Set
        End Property

        Public Property Discount As Decimal
            Get
                Return _Discount
            End Get
            Set(Value As Decimal)
                _Discount = Value
            End Set
        End Property

        Public Shared Function GetMonthlyReportCSV() As String
            Dim CSVReport As New CSVGenerator()

            CSVReport.AddColumn("Year")
            CSVReport.AddColumn("Month")
            CSVReport.AddColumn("Orders")
            CSVReport.AddColumn("Items")
            CSVReport.AddColumn("Sales")
            CSVReport.AddColumn("COGS")
            CSVReport.AddColumn("Shipping")
            CSVReport.AddColumn("Shipping Cost")
            CSVReport.AddColumn("Discount")
            CSVReport.AddColumn("Taxes")
            CSVReport.AddColumn("Gross Profit")
            CSVReport.AddColumn("Margin")

            Dim CurrentDataContext As New CommerceDataContext

            For Each CurrentData As MonthlyReportData In GetMonthlyReportDataFunc(CurrentDataContext).ToList
                Dim CsvMonthRow As DataRow = CSVReport.CSVData.NewRow

                CsvMonthRow.Item(0) = CurrentData.Year
                CsvMonthRow.Item(1) = CurrentData.Month
                CsvMonthRow.Item(2) = CurrentData.Orders
                CsvMonthRow.Item(3) = CurrentData.Items
                CsvMonthRow.Item(4) = CurrentData.Sales
                CsvMonthRow.Item(5) = CurrentData.CostOfGoodsSold
                CsvMonthRow.Item(6) = CurrentData.Shipping
                CsvMonthRow.Item(7) = Math.Round(CurrentData.ShippingCost, 2)
                CsvMonthRow.Item(8) = CurrentData.Discount
                CsvMonthRow.Item(9) = Math.Round(CurrentData.Taxes, 2)
                CsvMonthRow.Item(10) = CurrentData.Profit
                CsvMonthRow.Item(11) = Math.Round(CurrentData.ProfitMargin, 2)

                CSVReport.AddRow(CsvMonthRow)
            Next

            GetMonthlyReportCSV = CSVReport.GenerateCSVString

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function GetMonthlyReportData() As List(Of MonthlyReportData)
            Dim CurrentDataContext As New CommerceDataContext

            GetMonthlyReportData = GetMonthlyReportDataFunc(CurrentDataContext).ToList

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetMonthlyReportDataFunc As Func(Of CommerceDataContext, IQueryable(Of MonthlyReportData)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                        From o In CurrentDataContext.Orders _
                        Join oi In CurrentDataContext.OrderItems On oi.OrderID Equals o.ID _
                        Join p In CurrentDataContext.Products On p.ID Equals oi.ProductID _
                        Where o.ApprovalState = 1 _
                        Let Quantity = oi.Quantity _
                        Let Price = oi.Price _
                        Let Cost = p.Cost _
                        Group oi, p By Year = o.OrderDate.Year, Month = o.OrderDate.Month, Product = oi.ProductID Into Group _
                        Select Year, Month, Product, _
                        Quantity = Group.Sum(Function(f) f.oi.Quantity), _
                        Cost = Group.Max(Function(f) f.p.Cost), _
                        Price = Group.Max(Function(f) f.oi.Price), _
                        COGS = Group.Max(Function(f) f.p.Cost) * Group.Sum(Function(f) f.oi.Quantity), _
                        Sales = Group.Max(Function(f) f.oi.Price) * Group.Sum(Function(f) f.oi.Quantity), _
                        Profit = (Group.Max(Function(f) f.oi.Price) * Group.Sum(Function(f) f.oi.Quantity)) - (Group.Max(Function(f) f.p.Cost) * Group.Sum(Function(f) f.oi.Quantity)), _
                        Margin = (((Group.Max(Function(f) f.oi.Price) * Group.Sum(Function(f) f.oi.Quantity)) - (Group.Max(Function(f) f.p.Cost) * Group.Sum(Function(f) f.oi.Quantity))) _
                        / (Group.Max(Function(f) f.oi.Price) * Group.Sum(Function(f) f.oi.Quantity))) * 100, _
                        Orders = (From o In CurrentDataContext.Orders _
                                  Where o.OrderDate.Month = Month And o.OrderDate.Year = Year And o.ApprovalState = 1 _
                                  Select o Distinct).Count _
                        Group By Year, Month Into Group _
                        Select New MonthlyReportData With {
                            .Year = Year,
                            .Month = Month, _
                            .Orders = Group.First.Orders, _
                            .Items = Group.Sum(Function(f) f.Quantity), _
                            .Sales = Group.Sum(Function(f) f.Sales), _
                            .CostOfGoodsSold = Group.Sum(Function(f) f.COGS), _
                            .Profit = Group.Sum(Function(f) f.Profit), _
                            .ProfitMargin = (Group.Sum(Function(f) f.Profit) / Group.Sum(Function(f) f.Sales)) * 100, _
                            .Shipping = (From o In CurrentDataContext.Orders _
                                        Where o.OrderDate.Month = Month _
                                        And o.OrderDate.Year = Year _
                                        And o.ApprovalState = 1 _
                                        Select o Distinct).Sum(Function(f) f.ShipCharge), _
                            .Taxes = CDec((From o In CurrentDataContext.Orders _
                                        Join oi In CurrentDataContext.OrderItems _
                                        On oi.OrderID Equals o.ID _
                                        Where o.OrderDate.Month = Month _
                                        And o.OrderDate.Year = Year And o.ApprovalState = 1 _
                                        Select Taxes = o.TaxPercent * (oi.Price * oi.Quantity)).Sum()),
                            .Discount = (From o In CurrentDataContext.Orders _
                                        Where o.OrderDate.Month = Month _
                                        And o.OrderDate.Year = Year _
                                        And o.ApprovalState = 1 _
                                        Select o Distinct).Sum(Function(f) f.Discount)})

    End Class

End Namespace