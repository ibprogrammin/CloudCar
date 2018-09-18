Imports System.Globalization
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce.Reports

    Partial Public Class DailyDiscountReport
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                FillDefaultDates()
            End If
        End Sub

        Protected Sub lvSalesDays_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs)
            If e.Item.ItemType = ListViewItemType.DataItem Then
                Dim db As New CommerceDataContext

                Dim CurrentDate As Date = Date.Parse(CType(e.Item.FindControl("hDate"), HiddenField).Value)
                'Dim LitDailyTotal As Literal = CType(e.Item.FindControl("litDailyTotal"), Literal)
                'LitDailyTotal.Text = SMCommerce.OrderController.GetDailySalesTotalFunc(db, CurrentDate).ToString("C")

                If Not CurrentDate = Nothing Then
                    Dim dgSalesBreakdown As DataGrid = CType(e.Item.FindControl("dgSalesBreakdown"), DataGrid)

                    Dim SalesTable = From pc In db.PromoCodes Join o In db.Orders On o.PromoCode.ToLower Equals pc.Code.ToLower _
                                      Where o.ApprovalState = 1 And o.OrderDate.Date = CurrentDate _
                                      Let Cost = (From oi In db.OrderItems Join p In db.Products On p.ID Equals oi.ProductID Where oi.OrderID = o.ID _
                                      Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Cost = p.Cost}).Sum(Function(f) CType(f.Cost * f.Quantity, Nullable(Of Decimal))) _
                                      Let Price = (From oi In db.OrderItems Where oi.OrderID = o.ID _
                                      Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Price = oi.Price}).Sum(Function(f) CType(f.Price * f.Quantity, Nullable(Of Decimal))) _
                                      Let Savings = o.Discount _
                                      Group By pc.SalesRep Into Group _
                                      Select New With {SalesRep, _
                                                       .Discount = Group.Sum(Function(f) f.Savings), _
                                                       .Cost = Group.Sum(Function(f) f.Cost), _
                                                       .Price = Group.Sum(Function(f) f.Price), _
                                                       .Profit = Group.Sum(Function(f) f.Price) - Group.Sum(Function(f) f.Cost) - Group.Sum(Function(f) f.Savings)}

                    dgSalesBreakdown.DataSource = SalesTable
                    dgSalesBreakdown.DataBind()

                    Dim litCost As Literal = CType(e.Item.FindControl("litCost"), Literal)
                    Dim litSales As Literal = CType(e.Item.FindControl("litSales"), Literal)
                    Dim litDiscounts As Literal = CType(e.Item.FindControl("litDiscounts"), Literal)
                    Dim litProfit As Literal = CType(e.Item.FindControl("litProfit"), Literal)

                    litCost.Text = String.Format("{0:C}", SalesTable.Sum(Function(s) s.Cost))
                    litSales.Text = String.Format("{0:C}", SalesTable.Sum(Function(s) s.Price))
                    litDiscounts.Text = String.Format("{0:C}", SalesTable.Sum(Function(s) CType(s.Discount, Nullable(Of Decimal))))
                    litProfit.Text = String.Format("{0:C}", SalesTable.Sum(Function(s) CType(s.Profit, Nullable(Of Decimal))))
                End If
            End If
        End Sub

        Protected Sub lvBranchDays_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs)
            If e.Item.ItemType = ListViewItemType.DataItem Then
                Dim db As New CommerceDataContext

                Dim CurrentDate As Date = Date.Parse(CType(e.Item.FindControl("hDate"), HiddenField).Value)
                'Dim LitDailyTotal As Literal = CType(e.Item.FindControl("litDailyTotal"), Literal)
                'LitDailyTotal.Text = SMCommerce.OrderController.GetDailySalesTotalFunc(db, CurrentDate).ToString("C")

                If Not CurrentDate = Nothing Then
                    Dim dgSalesBreakdown As DataGrid = CType(e.Item.FindControl("dgSalesBreakdown"), DataGrid)

                    Dim SalesTable = From o In db.Orders Join a In db.Addresses On o.SAddressID Equals a.ID _
                                      Where o.ApprovalState = 1 And o.OrderDate.Date = CurrentDate _
                                      Let PostalCode = a.PCZIP _
                                      Let Cost = (From oi In db.OrderItems Join p In db.Products On p.ID Equals oi.ProductID Where oi.OrderID = o.ID _
                                      Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Cost = p.Cost}).Sum(Function(f) CType(f.Cost * f.Quantity, Nullable(Of Decimal))) _
                                      Let Price = (From oi In db.OrderItems Where oi.OrderID = o.ID _
                                      Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Price = oi.Price}).Sum(Function(f) CType(f.Price * f.Quantity, Nullable(Of Decimal))) _
                                      Let Savings = o.Discount _
                                      Group By PostalCode Into Group _
                                      Select New With {PostalCode, _
                                                       .Discount = Group.Sum(Function(f) f.Savings), _
                                                       .Cost = Group.Sum(Function(f) f.Cost), _
                                                       .Price = Group.Sum(Function(f) f.Price), _
                                                       .Profit = Group.Sum(Function(f) f.Price) - Group.Sum(Function(f) f.Cost) - Group.Sum(Function(f) f.Savings)}

                    Dim NewSalesTable = From st In SalesTable.AsEnumerable _
                                        Let Branch = GetBranch(st.PostalCode) _
                                        Let Discount = st.Discount _
                                        Let Cost = st.Cost _
                                        Let Price = st.Price _
                                        Let Profit = st.Profit _
                                        Group By Branch Into Group _
                                        Select New With {Branch, _
                                                       .Discount = Group.Sum(Function(f) f.Discount), _
                                                       .Cost = Group.Sum(Function(f) f.Cost), _
                                                       .Price = Group.Sum(Function(f) f.Price), _
                                                       .Profit = Group.Sum(Function(f) f.Profit)}

                    dgSalesBreakdown.DataSource = NewSalesTable
                    dgSalesBreakdown.DataBind()

                    Dim litCost As Literal = CType(e.Item.FindControl("litCost"), Literal)
                    Dim litSales As Literal = CType(e.Item.FindControl("litSales"), Literal)
                    Dim litDiscounts As Literal = CType(e.Item.FindControl("litDiscounts"), Literal)
                    Dim litProfit As Literal = CType(e.Item.FindControl("litProfit"), Literal)

                    litCost.Text = String.Format("{0:C}", NewSalesTable.Sum(Function(s) s.Cost))
                    litSales.Text = String.Format("{0:C}", NewSalesTable.Sum(Function(s) s.Price))
                    litDiscounts.Text = String.Format("{0:C}", NewSalesTable.Sum(Function(s) CType(s.Discount, Nullable(Of Decimal))))
                    litProfit.Text = String.Format("{0:C}", NewSalesTable.Sum(Function(s) CType(s.Profit, Nullable(Of Decimal))))
                End If
            End If
        End Sub

        Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGo.Click
            If Not txbInitial.Text Is Nothing And Not txbInitial.Text = String.Empty And Not txbFinal.Text Is Nothing And Not txbFinal.Text = String.Empty Then
                Select Case ddlReportType.SelectedValue
                    Case "Branch"
                        ShowBranchReport()
                    Case "Sales Rep"
                        ShowSalesRepReport()
                    Case Else
                End Select
            End If
        End Sub

        Private Sub ShowSalesRepReport()
            Dim db As New CommerceDataContext

            Dim culture As New CultureInfo("en-US", True)

            Dim InitialDate As String = DateTime.Parse(txbInitial.Text, culture, DateTimeStyles.NoCurrentDateDefault).ToShortDateString()
            Dim FinalDate As String = DateTime.Parse(txbFinal.Text, culture, DateTimeStyles.NoCurrentDateDefault).ToShortDateString()

            Dim SalesTable = From o In db.Orders Where Not o.PromoCode Is Nothing _
                             Group By Key = o.OrderDate Into Group _
                             Where Key.Date >= InitialDate And Key.Date <= FinalDate _
                             Order By Key Ascending _
                             Select New With {.OrderDate = Key.Date} Distinct

            lvSalesDays.DataSource = SalesTable
            lvSalesDays.DataBind()

            Dim TotalsTable = From pc In db.PromoCodes Join o In db.Orders On o.PromoCode.ToLower Equals pc.Code.ToLower _
                              Where o.ApprovalState = 1 And o.OrderDate.Date >= InitialDate And o.OrderDate.Date <= FinalDate _
                              Let Cost = (From oi In db.OrderItems Join p In db.Products On p.ID Equals oi.ProductID Where oi.OrderID = o.ID _
                              Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Cost = p.Cost}).Sum(Function(f) CType(f.Cost * f.Quantity, Nullable(Of Decimal))) _
                              Let Price = (From oi In db.OrderItems Where oi.OrderID = o.ID _
                              Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Price = oi.Price}).Sum(Function(f) CType(f.Price * f.Quantity, Nullable(Of Decimal))) _
                              Let Savings = o.Discount _
                              Group By pc.SalesRep Into Group _
                              Select New With {SalesRep, _
                                               .Discount = Group.Sum(Function(f) f.Savings), _
                                               .Cost = Group.Sum(Function(f) f.Cost), _
                                               .Price = Group.Sum(Function(f) f.Price), _
                                               .Profit = Group.Sum(Function(f) f.Price) - Group.Sum(Function(f) f.Cost) - Group.Sum(Function(f) f.Savings)}


            litYearlyCost.Text = String.Format("{0:C}", TotalsTable.Sum(Function(s) s.Cost))
            litYearlySales.Text = String.Format("{0:C}", TotalsTable.Sum(Function(s) s.Price))
            litYearlyDiscounts.Text = String.Format("{0:C}", TotalsTable.Sum(Function(s) CType(s.Discount, Nullable(Of Decimal))))
            litYearlyProfit.Text = String.Format("{0:C}", TotalsTable.Sum(Function(s) CType(s.Profit, Nullable(Of Decimal))))

            lvSalesDays.Visible = True
            lvBranchDays.Visible = False
        End Sub

        Private Sub ShowBranchReport()
            Dim db As New CommerceDataContext

            Dim culture As New CultureInfo("en-US", True)

            Dim initialDate As String = DateTime.Parse(txbInitial.Text, culture, DateTimeStyles.NoCurrentDateDefault).ToShortDateString()
            Dim finalDate As String = DateTime.Parse(txbFinal.Text, culture, DateTimeStyles.NoCurrentDateDefault).ToShortDateString()

            Dim SalesTable = From o In db.Orders _
                             Group By Key = o.OrderDate Into Group _
                             Order By Key Ascending _
                             Where Key.Date >= initialDate And Key.Date <= finalDate _
                             Select New With {.OrderDate = Key.Date} Distinct

            lvBranchDays.DataSource = SalesTable
            lvBranchDays.DataBind()

            Dim TotalsTable = From o In db.Orders Join a In db.Addresses On o.SAddressID Equals a.ID _
                              Where o.ApprovalState = 1 And o.OrderDate.Date >= initialDate And o.OrderDate.Date <= finalDate _
                              Let PostalCode = a.PCZIP _
                              Let Cost = (From oi In db.OrderItems Join p In db.Products On p.ID Equals oi.ProductID Where oi.OrderID = o.ID _
                              Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Cost = p.Cost}).Sum(Function(f) CType(f.Cost * f.Quantity, Nullable(Of Decimal))) _
                              Let Price = (From oi In db.OrderItems Where oi.OrderID = o.ID _
                              Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Price = oi.Price}).Sum(Function(f) CType(f.Price * f.Quantity, Nullable(Of Decimal))) _
                              Let Savings = o.Discount _
                              Group By PostalCode Into Group _
                              Select New With {PostalCode, _
                                               .Discount = Group.Sum(Function(f) f.Savings), _
                                               .Cost = Group.Sum(Function(f) f.Cost), _
                                               .Price = Group.Sum(Function(f) f.Price), _
                                               .Profit = Group.Sum(Function(f) f.Price) - Group.Sum(Function(f) f.Cost) - Group.Sum(Function(f) f.Savings)}

            Dim NewTotalsTable = From st In TotalsTable.AsEnumerable _
                                Let Branch = GetBranch(st.PostalCode) _
                                Let Discount = st.Discount _
                                Let Cost = st.Cost _
                                Let Price = st.Price _
                                Let Profit = st.Profit _
                                Group By Branch Into Group _
                                Select New With {Branch, _
                                               .Discount = Group.Sum(Function(f) f.Discount), _
                                               .Cost = Group.Sum(Function(f) f.Cost), _
                                               .Price = Group.Sum(Function(f) f.Price), _
                                               .Profit = Group.Sum(Function(f) f.Profit)}

            litYearlyCost.Text = String.Format("{0:C}", NewTotalsTable.Sum(Function(s) s.Cost))
            litYearlySales.Text = String.Format("{0:C}", NewTotalsTable.Sum(Function(s) s.Price))
            litYearlyDiscounts.Text = String.Format("{0:C}", NewTotalsTable.Sum(Function(s) CType(s.Discount, Nullable(Of Decimal))))
            litYearlyProfit.Text = String.Format("{0:C}", NewTotalsTable.Sum(Function(s) CType(s.Profit, Nullable(Of Decimal))))

            lvBranchDays.Visible = True
            lvSalesDays.Visible = False
        End Sub

        Protected Sub FillDefaultDates()
            Dim initialDate As Date = DateTime.Now.AddDays(-10)
            Dim finalDate As Date = DateTime.Now

            txbInitial.Text = initialDate.ToString("M/d/yyyy")
            txbFinal.Text = finalDate.ToString("M/d/yyyy")

            ddlReportType.Items.Add(New ListItem("Report Type", ""))
            ddlReportType.Items.Add(New ListItem("Branch", "Branch"))
            ddlReportType.Items.Add(New ListItem("Sales Rep", "Sales Rep"))
        End Sub

        Public Shared Function GetBranch(ByVal Code As String) As String
            Dim db As New CommerceDataContext

            Dim distributorId As Integer = CCFramework.Commerce.FixedShippingZoneController.GetShippingZoneDistributor(Code)
            'Dim user As SimpleUser = SimpleUserController.GetSimpleUserByIDFunc(db, distributorId)
            Dim ru As RegisteredUser = CCFramework.Core.RegisteredUserController.GetRegisteredUserByUserIdFunc(db, distributorId)

            If ru Is Nothing Then
                Return "None"
            Else
                Return ru.UserName
            End If
        End Function

        Private Sub btnDownloadCSV_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownloadCsv.Click
            Select Case ddlReportType.SelectedValue
                Case "Branch"
                    GetBranchCsvFile()
                Case "Sales Rep"
                    GetSalesRepCsvFile()
                Case Else
            End Select
        End Sub

        Private Function GetSalesRepCsvFile() As String
            Dim db As New CommerceDataContext

            Dim CSVReport As New CCFramework.Core.CSVGenerator()

            CSVReport.AddColumn("Sales Rep")
            CSVReport.AddColumn("Year")
            CSVReport.AddColumn("Month")
            CSVReport.AddColumn("Day")
            CSVReport.AddColumn("Cost")
            CSVReport.AddColumn("Sales")
            CSVReport.AddColumn("Discounts")
            CSVReport.AddColumn("Profit")

            Dim culture As New CultureInfo("en-US", True)

            Dim InitialDate As String = DateTime.Parse(txbInitial.Text, culture, DateTimeStyles.NoCurrentDateDefault).ToShortDateString()
            Dim FinalDate As String = DateTime.Parse(txbFinal.Text, culture, DateTimeStyles.NoCurrentDateDefault).ToShortDateString()

            Dim DatesTable = From o In db.Orders Where Not o.PromoCode Is Nothing _
                             Group By Key = o.OrderDate Into Group _
                             Where Key.Date >= InitialDate And Key.Date <= FinalDate _
                             Order By Key Ascending _
                             Select New With {.OrderDate = Key.Date} Distinct

            For Each item In DatesTable
                Dim CurrentDate As Date = item.OrderDate

                Dim SalesTable = From pc In db.PromoCodes Join o In db.Orders On o.PromoCode.ToLower Equals pc.Code.ToLower _
                                  Where o.ApprovalState = 1 And o.OrderDate.Date = CurrentDate _
                                  Let Cost = (From oi In db.OrderItems Join p In db.Products On p.ID Equals oi.ProductID Where oi.OrderID = o.ID _
                                  Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Cost = p.Cost}).Sum(Function(f) CType(f.Cost * f.Quantity, Nullable(Of Decimal))) _
                                  Let Price = (From oi In db.OrderItems Where oi.OrderID = o.ID _
                                  Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Price = oi.Price}).Sum(Function(f) CType(f.Price * f.Quantity, Nullable(Of Decimal))) _
                                  Let Savings = o.Discount _
                                  Group By pc.SalesRep Into Group _
                                  Select New With {SalesRep, _
                                                   .Discount = Group.Sum(Function(f) f.Savings), _
                                                   .Cost = Group.Sum(Function(f) f.Cost), _
                                                   .Price = Group.Sum(Function(f) f.Price), _
                                                   .Profit = Group.Sum(Function(f) f.Price) - Group.Sum(Function(f) f.Cost) - Group.Sum(Function(f) f.Savings)}

                For Each SalesItem In SalesTable
                    Dim CSVRow As DataRow = CSVReport.CSVData.NewRow

                    CSVRow.Item(0) = SalesItem.SalesRep
                    CSVRow.Item(1) = item.OrderDate.Year
                    CSVRow.Item(2) = MonthName(item.OrderDate.Month)
                    CSVRow.Item(3) = item.OrderDate.Day
                    CSVRow.Item(4) = SalesItem.Cost
                    CSVRow.Item(5) = SalesItem.Price
                    CSVRow.Item(6) = SalesItem.Discount
                    CSVRow.Item(7) = SalesItem.Profit

                    CSVReport.AddRow(CSVRow)
                Next

                Dim MonthlyCost As Decimal = SalesTable.Sum(Function(s) s.Cost).Value
                Dim MonthlySales As Decimal = SalesTable.Sum(Function(s) s.Price).Value
                Dim MonthlyDiscounts As Decimal = SalesTable.Sum(Function(s) CType(s.Discount, Nullable(Of Decimal))).Value
                Dim MonthlyProfit As Decimal = SalesTable.Sum(Function(s) CType(s.Profit, Nullable(Of Decimal))).Value

                Dim CSVMonthlyTotalRow As DataRow = CSVReport.CSVData.NewRow

                CSVMonthlyTotalRow.Item(0) = "Totals"
                CSVMonthlyTotalRow.Item(1) = item.OrderDate.Year
                CSVMonthlyTotalRow.Item(2) = MonthName(item.OrderDate.Month)
                CSVMonthlyTotalRow.Item(3) = item.OrderDate.Day
                CSVMonthlyTotalRow.Item(4) = MonthlyCost
                CSVMonthlyTotalRow.Item(5) = MonthlySales
                CSVMonthlyTotalRow.Item(6) = MonthlyDiscounts
                CSVMonthlyTotalRow.Item(7) = MonthlyProfit

                CSVReport.AddRow(CSVMonthlyTotalRow)
            Next

            Dim TotalsTable = From pc In db.PromoCodes Join o In db.Orders On o.PromoCode.ToLower Equals pc.Code.ToLower _
                              Where o.ApprovalState = 1 And o.OrderDate.Date >= InitialDate And o.OrderDate.Date <= FinalDate _
                              Let Cost = (From oi In db.OrderItems Join p In db.Products On p.ID Equals oi.ProductID Where oi.OrderID = o.ID _
                              Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Cost = p.Cost}).Sum(Function(f) CType(f.Cost * f.Quantity, Nullable(Of Decimal))) _
                              Let Price = (From oi In db.OrderItems Where oi.OrderID = o.ID _
                              Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Price = oi.Price}).Sum(Function(f) CType(f.Price * f.Quantity, Nullable(Of Decimal))) _
                              Let Savings = o.Discount _
                              Group By pc.SalesRep Into Group _
                              Select New With {SalesRep, _
                                               .Discount = Group.Sum(Function(f) f.Savings), _
                                               .Cost = Group.Sum(Function(f) f.Cost), _
                                               .Price = Group.Sum(Function(f) f.Price), _
                                               .Profit = Group.Sum(Function(f) f.Price) - Group.Sum(Function(f) f.Cost) - Group.Sum(Function(f) f.Savings)}

            Dim YearlyCost As Decimal = 0
            Dim YearlySales As Decimal = 0
            Dim YearlyDiscounts As Decimal = 0
            Dim YearlyProfit As Decimal = 0

            If Not TotalsTable Is Nothing Then
                YearlyCost = TotalsTable.Sum(Function(s) s.Cost).Value
                YearlySales = TotalsTable.Sum(Function(s) s.Price).Value
                YearlyDiscounts = TotalsTable.Sum(Function(s) CType(s.Discount, Nullable(Of Decimal))).Value
                YearlyProfit = TotalsTable.Sum(Function(s) CType(s.Profit, Nullable(Of Decimal))).Value
            End If

            Dim CSVYearlyTotalRow As DataRow = CSVReport.CSVData.NewRow

            CSVYearlyTotalRow.Item(0) = "Totals"
            CSVYearlyTotalRow.Item(1) = ""
            CSVYearlyTotalRow.Item(2) = ""
            CSVYearlyTotalRow.Item(3) = ""
            CSVYearlyTotalRow.Item(4) = YearlyCost
            CSVYearlyTotalRow.Item(5) = YearlySales
            CSVYearlyTotalRow.Item(6) = YearlyDiscounts
            CSVYearlyTotalRow.Item(7) = YearlyProfit

            CSVReport.AddRow(CSVYearlyTotalRow)

            Dim CsvContent As String = CSVReport.GenerateCSVString

            Session.Add("d", CsvContent)
            Response.Redirect("/Report/SalesReport.csv")

            Return CsvContent
        End Function

        Private Function GetBranchCsvFile() As String
            Dim db As New CommerceDataContext

            Dim CSVReport As New CCFramework.Core.CSVGenerator()

            CSVReport.AddColumn("Branch")
            CSVReport.AddColumn("Year")
            CSVReport.AddColumn("Month")
            CSVReport.AddColumn("Day")
            CSVReport.AddColumn("Cost")
            CSVReport.AddColumn("Sales")
            CSVReport.AddColumn("Discounts")
            CSVReport.AddColumn("Profit")

            Dim culture As New CultureInfo("en-US", True)

            Dim initialDate As String = DateTime.Parse(txbInitial.Text, culture, DateTimeStyles.NoCurrentDateDefault).ToShortDateString()
            Dim finalDate As String = DateTime.Parse(txbFinal.Text, culture, DateTimeStyles.NoCurrentDateDefault).ToShortDateString()

            Dim DatesTable = From o In db.Orders _
                             Group By Key = o.OrderDate Into Group _
                             Order By Key Ascending _
                             Where Key.Date >= initialDate And Key.Date <= finalDate _
                             Select New With {.OrderDate = Key.Date} Distinct

            For Each item In DatesTable
                Dim CurrentDate As Date = item.OrderDate

                Dim SalesTable = From o In db.Orders Join a In db.Addresses On o.SAddressID Equals a.ID _
                                                  Where o.ApprovalState = 1 And o.OrderDate.Date = CurrentDate _
                                                  Let PostalCode = a.PCZIP _
                                                  Let Cost = (From oi In db.OrderItems Join p In db.Products On p.ID Equals oi.ProductID Where oi.OrderID = o.ID _
                                                  Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Cost = p.Cost}).Sum(Function(f) CType(f.Cost * f.Quantity, Nullable(Of Decimal))) _
                                                  Let Price = (From oi In db.OrderItems Where oi.OrderID = o.ID _
                                                  Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Price = oi.Price}).Sum(Function(f) CType(f.Price * f.Quantity, Nullable(Of Decimal))) _
                                                  Let Savings = o.Discount _
                                                  Group By PostalCode Into Group _
                                                  Select New With {PostalCode, _
                                                                   .Discount = Group.Sum(Function(f) f.Savings), _
                                                                   .Cost = Group.Sum(Function(f) f.Cost), _
                                                                   .Price = Group.Sum(Function(f) f.Price), _
                                                                   .Profit = Group.Sum(Function(f) f.Price) - Group.Sum(Function(f) f.Cost) - Group.Sum(Function(f) f.Savings)}

                Dim NewSalesTable = From st In SalesTable.AsEnumerable _
                                    Let Branch = GetBranch(st.PostalCode) _
                                    Let Discount = st.Discount _
                                    Let Cost = st.Cost _
                                    Let Price = st.Price _
                                    Let Profit = st.Profit _
                                    Group By Branch Into Group _
                                    Select New With {Branch, _
                                                   .Discount = Group.Sum(Function(f) f.Discount), _
                                                   .Cost = Group.Sum(Function(f) f.Cost), _
                                                   .Price = Group.Sum(Function(f) f.Price), _
                                                   .Profit = Group.Sum(Function(f) f.Profit)}

                For Each SalesItem In NewSalesTable
                    Dim CSVRow As DataRow = CSVReport.CSVData.NewRow

                    CSVRow.Item(0) = "Totals"
                    CSVRow.Item(1) = item.OrderDate.Year
                    CSVRow.Item(2) = MonthName(item.OrderDate.Month)
                    CSVRow.Item(3) = item.OrderDate.Day
                    CSVRow.Item(4) = SalesItem.Cost
                    CSVRow.Item(5) = SalesItem.Price
                    CSVRow.Item(6) = SalesItem.Discount
                    CSVRow.Item(7) = SalesItem.Profit

                    CSVReport.AddRow(CSVRow)
                Next

                Dim MonthlyCost As Decimal = NewSalesTable.Sum(Function(s) s.Cost).Value
                Dim MonthlySales As Decimal = NewSalesTable.Sum(Function(s) s.Price).Value
                Dim MonthlyDiscounts As Decimal = NewSalesTable.Sum(Function(s) CType(s.Discount, Nullable(Of Decimal))).Value
                Dim MonthlyProfit As Decimal = NewSalesTable.Sum(Function(s) CType(s.Profit, Nullable(Of Decimal))).Value

                Dim CSVMonthlyTotalRow As DataRow = CSVReport.CSVData.NewRow

                CSVMonthlyTotalRow.Item(0) = "Totals"
                CSVMonthlyTotalRow.Item(1) = item.OrderDate.Year
                CSVMonthlyTotalRow.Item(2) = MonthName(item.OrderDate.Month)
                CSVMonthlyTotalRow.Item(3) = item.OrderDate.Day
                CSVMonthlyTotalRow.Item(4) = MonthlyCost
                CSVMonthlyTotalRow.Item(5) = MonthlySales
                CSVMonthlyTotalRow.Item(6) = MonthlyDiscounts
                CSVMonthlyTotalRow.Item(7) = MonthlyProfit

                CSVReport.AddRow(CSVMonthlyTotalRow)
            Next

            Dim TotalsTable = From o In db.Orders Join a In db.Addresses On o.SAddressID Equals a.ID _
                              Where o.ApprovalState = 1 And o.OrderDate.Date >= initialDate And o.OrderDate.Date <= finalDate _
                              Let PostalCode = a.PCZIP _
                              Let Cost = (From oi In db.OrderItems Join p In db.Products On p.ID Equals oi.ProductID Where oi.OrderID = o.ID _
                              Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Cost = p.Cost}).Sum(Function(f) CType(f.Cost * f.Quantity, Nullable(Of Decimal))) _
                              Let Price = (From oi In db.OrderItems Where oi.OrderID = o.ID _
                              Select New With {.Quantity = Convert.ToDecimal(oi.Quantity), .Price = oi.Price}).Sum(Function(f) CType(f.Price * f.Quantity, Nullable(Of Decimal))) _
                              Let Savings = o.Discount _
                              Group By PostalCode Into Group _
                              Select New With {PostalCode, _
                                               .Discount = Group.Sum(Function(f) f.Savings), _
                                               .Cost = Group.Sum(Function(f) f.Cost), _
                                               .Price = Group.Sum(Function(f) f.Price), _
                                               .Profit = Group.Sum(Function(f) f.Price) - Group.Sum(Function(f) f.Cost) - Group.Sum(Function(f) f.Savings)}

            Dim NewTotalsTable = From st In TotalsTable.AsEnumerable _
                                Let Branch = GetBranch(st.PostalCode) _
                                Let Discount = st.Discount _
                                Let Cost = st.Cost _
                                Let Price = st.Price _
                                Let Profit = st.Profit _
                                Group By Branch Into Group _
                                Select New With {Branch, _
                                               .Discount = Group.Sum(Function(f) f.Discount), _
                                               .Cost = Group.Sum(Function(f) f.Cost), _
                                               .Price = Group.Sum(Function(f) f.Price), _
                                               .Profit = Group.Sum(Function(f) f.Profit)}

            Dim YearlyCost As Decimal = 0
            Dim YearlySales As Decimal = 0
            Dim YearlyDiscounts As Decimal = 0
            Dim YearlyProfit As Decimal = 0

            If Not NewTotalsTable Is Nothing Then
                YearlyCost = NewTotalsTable.Sum(Function(s) s.Cost).Value
                YearlySales = NewTotalsTable.Sum(Function(s) s.Price).Value
                YearlyDiscounts = NewTotalsTable.Sum(Function(s) CType(s.Discount, Nullable(Of Decimal))).Value
                YearlyProfit = NewTotalsTable.Sum(Function(s) CType(s.Profit, Nullable(Of Decimal))).Value
            End If

            Dim CSVYearlyTotalRow As DataRow = CSVReport.CSVData.NewRow

            CSVYearlyTotalRow.Item(0) = "Totals"
            CSVYearlyTotalRow.Item(1) = ""
            CSVYearlyTotalRow.Item(2) = ""
            CSVYearlyTotalRow.Item(3) = ""
            CSVYearlyTotalRow.Item(4) = YearlyCost
            CSVYearlyTotalRow.Item(5) = YearlySales
            CSVYearlyTotalRow.Item(6) = YearlyDiscounts
            CSVYearlyTotalRow.Item(7) = YearlyProfit

            CSVReport.AddRow(CSVYearlyTotalRow)

            Dim CsvContent As String = CSVReport.GenerateCSVString

            Session.Add("d", CsvContent)
            Response.Redirect("/Report/SalesReport.csv")

            Return CsvContent
        End Function

    End Class

End Namespace