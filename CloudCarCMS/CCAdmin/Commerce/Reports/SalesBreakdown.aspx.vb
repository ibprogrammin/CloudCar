Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce.Reports

    Partial Public Class SalesBreakdown
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                FillDefaultDates()
            End If
        End Sub

        Protected Sub lvSalesMonths_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs)
            If e.Item.ItemType = ListViewItemType.DataItem Then
                Dim db As New CommerceDataContext

                Dim SelectedYear As Integer = Integer.Parse(ddlYearSelection.SelectedValue)
                Dim SelectedMonth As Integer = Integer.Parse(CType(e.Item.FindControl("hfMonth"), HiddenField).Value)

                If Not SelectedYear = Nothing And Not SelectedMonth = Nothing Then
                    Dim dgSalesBreakdown As DataGrid = CType(e.Item.FindControl("dgSalesBreakdown"), DataGrid)

                    Dim SalesTable = From pc In db.PromoCodes Join o In db.Orders On o.PromoCode.ToLower Equals pc.Code.ToLower _
                                      Where o.ApprovalState = 1 And o.OrderDate.Year = SelectedYear And o.OrderDate.Month = SelectedMonth _
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



                    '        From o In db.Orders Join a In db.Addresses On a.ID Equals o.SAddressID _
                    '        Join oi In db.OrderItems On oi.OrderID Equals o.ID _
                    '        Let Price = oi.Price * oi.Quantity _
                    '        Let Cost = (From p In db.Products Where p.ID = oi.ProductID Select p.Cost).First * oi.Quantity _
                    '        Let SalesRep = (From pc In db.PromoCodes Where String.Equals(pc.Code, o.PromoCode) Select pc.SalesRep).First _
                    '        Let Savings = o.Discount _
                    '        Let PCZIPCode = a.PCZIP _
                    '        Group By Year = o.OrderDate.Year, Month = o.OrderDate.Month, PCZIPCode, SalesRep Into Group _
                    '        Where Year = SelectedYear And Month = SelectedMonth _
                    '        Order By Year, Month, SalesRep Ascending _
                    '        Select New With {Year, Month, .Code = PCZIPCode, .Rep = If(SalesRep = Nothing, "None", SalesRep), _
                    '                         .Cost = Group.Sum(Function(p) p.Cost), .Sales = Group.Sum(Function(p) p.Price), _
                    '                         .Discounts = Group.Sum(Function(p) p.Savings), _
                    '                         .Profit = Group.Sum(Function(p) p.Price) - Group.Sum(Function(p) p.Cost) - Group.Sum(Function(p) p.Savings)}()()()()()

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

        Protected Sub lvBranchMonths_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs)
            If e.Item.ItemType = ListViewItemType.DataItem Then
                Dim db As New CommerceDataContext

                Dim SelectedYear As Integer = Integer.Parse(ddlYearSelection.SelectedValue)
                Dim SelectedMonth As Integer = Integer.Parse(CType(e.Item.FindControl("hfMonth"), HiddenField).Value)

                If Not SelectedYear = Nothing And Not SelectedMonth = Nothing Then
                    Dim dgSalesBreakdown As DataGrid = CType(e.Item.FindControl("dgSalesBreakdown"), DataGrid)

                    Dim SalesTable = From o In db.Orders Join a In db.Addresses On o.SAddressID Equals a.ID _
                                      Where o.ApprovalState = 1 And o.OrderDate.Year = SelectedYear And o.OrderDate.Month = SelectedMonth _
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
                                        Let Branch = GetBranchCity(st.PostalCode) _
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
            If Not ddlYearSelection.SelectedValue Is Nothing And Not ddlYearSelection.SelectedValue = String.Empty Then
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

            Dim SelectedYear As Integer = Integer.Parse(ddlYearSelection.SelectedValue)

            Dim SalesTable = From o In db.Orders _
                             Let SalesRep = (From pc In db.PromoCodes Where String.Equals(pc.Code, o.PromoCode) Select pc.SalesRep).FirstOrDefault _
                             Group By Year = o.OrderDate.Year, Month = o.OrderDate.Month, SalesRep Into Group _
                             Order By Year, Month Ascending _
                             Where Not SalesRep Is Nothing _
                             Select New With {Year, Month} Distinct

            lvSalesMonths.DataSource = SalesTable
            lvSalesMonths.DataBind()

            Dim TotalsTable = From pc In db.PromoCodes Join o In db.Orders On o.PromoCode.ToLower Equals pc.Code.ToLower _
                              Where o.ApprovalState = 1 And o.OrderDate.Year = SelectedYear _
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

            lvSalesMonths.Visible = True
            lvBranchMonths.Visible = False
        End Sub

        Private Sub ShowBranchReport()
            Dim db As New CommerceDataContext

            Dim SelectedYear As Integer = Integer.Parse(ddlYearSelection.SelectedValue)

            Dim SalesTable = From o In db.Orders _
                             Group By Year = o.OrderDate.Year, Month = o.OrderDate.Month Into Group _
                             Order By Year, Month Ascending _
                             Select New With {Year, Month} Distinct

            lvBranchMonths.DataSource = SalesTable
            lvBranchMonths.DataBind()

            Dim TotalsTable = From o In db.Orders Join a In db.Addresses On o.SAddressID Equals a.ID _
                              Where o.ApprovalState = 1 And o.OrderDate.Year = SelectedYear _
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
                                Let Branch = GetBranchCity(st.PostalCode) _
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

            lvBranchMonths.Visible = True
            lvSalesMonths.Visible = False
        End Sub

        Protected Sub FillDefaultDates()
            Dim earliest As Integer = OrderController.GetEarliestOrderYear
            Dim latest As Integer = OrderController.GetLatestOrderYear

            ddlYearSelection.Items.Add(New ListItem("Year", ""))

            For i As Integer = earliest To latest
                ddlYearSelection.Items.Add(New ListItem(i.ToString, i.ToString))
            Next

            ddlReportType.Items.Add(New ListItem("Report Type", ""))
            ddlReportType.Items.Add(New ListItem("Branch", "Branch"))
            ddlReportType.Items.Add(New ListItem("Sales Rep", "Sales Rep"))

            'For m As Integer = 1 To 12
            '    ddlMonthSelection.Items.Add(New ListItem(MonthName(m), m.ToString))
            'Next
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

        Public Shared Function GetBranchCity(ByVal Code As String) As String
            Dim db As New CommerceDataContext

            Dim distributorId As Integer = CCFramework.Commerce.FixedShippingZoneController.GetShippingZoneDistributor(Code)
            'Dim user As SimpleUser = SimpleUserController.GetSimpleUserByIDFunc(db, distributorId)
            Dim ru As RegisteredUser = CCFramework.Core.RegisteredUserController.GetRegisteredUserByUserIdFunc(db, distributorId)

            If ru Is Nothing Then
                Return "None"
            Else
                Return ru.Address.City
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
            CSVReport.AddColumn("Cost")
            CSVReport.AddColumn("Sales")
            CSVReport.AddColumn("Discounts")
            CSVReport.AddColumn("Profit")

            Dim SelectedYear As Integer = Integer.Parse(ddlYearSelection.SelectedValue)

            Dim SalesTable = From o In db.Orders _
                             Let SalesRep = (From pc In db.PromoCodes Where String.Equals(pc.Code, o.PromoCode) Select pc.SalesRep).FirstOrDefault _
                             Group By Year = o.OrderDate.Year, Month = o.OrderDate.Month, SalesRep Into Group _
                             Order By Year, Month Ascending _
                             Where Not SalesRep Is Nothing _
                             Select New With {Year, Month} Distinct

            For Each item In SalesTable
                Dim SelectedMonth As Integer = item.Month

                If Not SelectedYear = Nothing And Not SelectedMonth = Nothing Then
                    Dim MonthlySalesTable = From pc In db.PromoCodes Join o In db.Orders On o.PromoCode.ToLower Equals pc.Code.ToLower _
                                      Where o.ApprovalState = 1 And o.OrderDate.Year = SelectedYear And o.OrderDate.Month = SelectedMonth _
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

                    For Each SalesItem In MonthlySalesTable
                        Dim CSVRow As DataRow = CSVReport.CSVData.NewRow

                        CSVRow.Item(0) = SalesItem.SalesRep
                        CSVRow.Item(1) = SelectedYear
                        CSVRow.Item(2) = MonthName(SelectedMonth)
                        CSVRow.Item(3) = SalesItem.Cost
                        CSVRow.Item(4) = SalesItem.Price
                        CSVRow.Item(5) = SalesItem.Discount
                        CSVRow.Item(6) = SalesItem.Profit

                        CSVReport.AddRow(CSVRow)
                    Next

                    Dim MonthlyCost As Decimal = MonthlySalesTable.Sum(Function(s) s.Cost).Value
                    Dim MonthlySales As Decimal = MonthlySalesTable.Sum(Function(s) s.Price).Value
                    Dim MonthlyDiscounts As Decimal = MonthlySalesTable.Sum(Function(s) CType(s.Discount, Nullable(Of Decimal))).Value
                    Dim MonthlyProfit As Decimal = MonthlySalesTable.Sum(Function(s) CType(s.Profit, Nullable(Of Decimal))).Value

                    Dim CSVMonthlyTotalRow As DataRow = CSVReport.CSVData.NewRow

                    CSVMonthlyTotalRow.Item(0) = MonthName(SelectedMonth) & " Totals"
                    CSVMonthlyTotalRow.Item(1) = SelectedYear
                    CSVMonthlyTotalRow.Item(2) = MonthName(SelectedMonth)
                    CSVMonthlyTotalRow.Item(3) = MonthlyCost
                    CSVMonthlyTotalRow.Item(4) = MonthlySales
                    CSVMonthlyTotalRow.Item(5) = MonthlyDiscounts
                    CSVMonthlyTotalRow.Item(6) = MonthlyProfit

                    CSVReport.AddRow(CSVMonthlyTotalRow)
                End If
            Next

            Dim TotalsTable = From pc In db.PromoCodes Join o In db.Orders On o.PromoCode.ToLower Equals pc.Code.ToLower _
                              Where o.ApprovalState = 1 And o.OrderDate.Year = SelectedYear _
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

            CSVYearlyTotalRow.Item(0) = SelectedYear & " Totals"
            CSVYearlyTotalRow.Item(1) = SelectedYear
            CSVYearlyTotalRow.Item(2) = "All"
            CSVYearlyTotalRow.Item(3) = YearlyCost
            CSVYearlyTotalRow.Item(4) = YearlySales
            CSVYearlyTotalRow.Item(5) = YearlyDiscounts
            CSVYearlyTotalRow.Item(6) = YearlyProfit

            CSVReport.AddRow(CSVYearlyTotalRow)

            Dim CsvContent As String = CSVReport.GenerateCSVString

            'Request.Form.Set("d", CsvContent)
            'hfReportData.Value = CsvContent
            Session.Add("d", CsvContent)
            Response.Redirect("/Report/SalesReport.csv")
            'Page.Server.Transfer("/Download/Report/SalesReport.csv", True)

            Return CsvContent
        End Function

        Private Function GetBranchCsvFile() As String
            Dim db As New CommerceDataContext

            Dim CSVReport As New CCFramework.Core.CSVGenerator()

            CSVReport.AddColumn("Branch")
            CSVReport.AddColumn("Year")
            CSVReport.AddColumn("Month")
            CSVReport.AddColumn("Cost")
            CSVReport.AddColumn("Sales")
            CSVReport.AddColumn("Discounts")
            CSVReport.AddColumn("Profit")

            Dim SelectedYear As Integer = Integer.Parse(ddlYearSelection.SelectedValue)

            Dim SalesTable = From o In db.Orders _
                             Group By Year = o.OrderDate.Year, Month = o.OrderDate.Month Into Group _
                             Order By Year, Month Ascending _
                             Select New With {Year, Month} Distinct

            For Each item In SalesTable
                Dim SelectedMonth As Integer = item.Month

                If Not SelectedYear = Nothing And Not SelectedMonth = Nothing Then
                    Dim MonthlySalesTable = From o In db.Orders Join a In db.Addresses On o.SAddressID Equals a.ID _
                                      Where o.ApprovalState = 1 And o.OrderDate.Year = SelectedYear And o.OrderDate.Month = SelectedMonth _
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

                    Dim NewMonthlySalesTable = From st In MonthlySalesTable.AsEnumerable _
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

                    For Each SalesItem In NewMonthlySalesTable
                        Dim CSVRow As DataRow = CSVReport.CSVData.NewRow

                        CSVRow.Item(0) = SalesItem.Branch
                        CSVRow.Item(1) = SelectedYear
                        CSVRow.Item(2) = MonthName(SelectedMonth)
                        CSVRow.Item(3) = SalesItem.Cost
                        CSVRow.Item(4) = SalesItem.Price
                        CSVRow.Item(5) = SalesItem.Discount
                        CSVRow.Item(6) = SalesItem.Profit

                        CSVReport.AddRow(CSVRow)
                    Next

                    Dim MonthlyCost As Decimal = NewMonthlySalesTable.Sum(Function(s) s.Cost).Value
                    Dim MonthlySales As Decimal = NewMonthlySalesTable.Sum(Function(s) s.Price).Value
                    Dim MonthlyDiscounts As Decimal = NewMonthlySalesTable.Sum(Function(s) CType(s.Discount, Nullable(Of Decimal))).Value
                    Dim MonthlyProfit As Decimal = NewMonthlySalesTable.Sum(Function(s) CType(s.Profit, Nullable(Of Decimal))).Value

                    Dim CSVMonthlyTotalRow As DataRow = CSVReport.CSVData.NewRow

                    CSVMonthlyTotalRow.Item(0) = MonthName(SelectedMonth) & " Totals"
                    CSVMonthlyTotalRow.Item(1) = SelectedYear
                    CSVMonthlyTotalRow.Item(2) = MonthName(SelectedMonth)
                    CSVMonthlyTotalRow.Item(3) = MonthlyCost
                    CSVMonthlyTotalRow.Item(4) = MonthlySales
                    CSVMonthlyTotalRow.Item(5) = MonthlyDiscounts
                    CSVMonthlyTotalRow.Item(6) = MonthlyProfit

                    CSVReport.AddRow(CSVMonthlyTotalRow)
                End If
            Next

            Dim TotalsTable = From o In db.Orders Join a In db.Addresses On o.SAddressID Equals a.ID _
                              Where o.ApprovalState = 1 And o.OrderDate.Year = SelectedYear _
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

            CSVYearlyTotalRow.Item(0) = SelectedYear & " Totals"
            CSVYearlyTotalRow.Item(1) = SelectedYear
            CSVYearlyTotalRow.Item(2) = "All"
            CSVYearlyTotalRow.Item(3) = YearlyCost
            CSVYearlyTotalRow.Item(4) = YearlySales
            CSVYearlyTotalRow.Item(5) = YearlyDiscounts
            CSVYearlyTotalRow.Item(6) = YearlyProfit

            CSVReport.AddRow(CSVYearlyTotalRow)

            Dim CsvContent As String = CSVReport.GenerateCSVString

            'Request.Form.Set("d", CsvContent)
            'hfReportData.Value = CsvContent
            Session.Add("d", CsvContent)
            Response.Redirect("/Report/SalesReport.csv")
            'Page.Server.Transfer("/Download/Report/SalesReport.csv", True)

            Return CsvContent
        End Function

    End Class

End Namespace


'TODO implement AMJ Sales Report Query
'from o in [Orders] join a in Addresses on a.id equals o.SAddressId _
'join oi in orderitems on oi.OrderID equals o.id _
'Let Price = oi.Price * oi.Quantity _
'Let Cost = (From p in products where p.id = oi.ProductID select p.cost).First * oi.Quantity _
'Let SalesRep = (From pc in PromoCodes where String.Equals(pc.Code, o.PromoCode) select pc.SalesRep).First _
'Let Savings = o.Discount _
'Let PCZIPCode = a.PCZIP _
'Group by Year = o.OrderDate.Year, Month = o.OrderDate.Month, PCZIPCode, SalesRep into group _ 
'Order by Year, Month, SalesRep Ascending _
'select new with {Year, Month, .Code = PCZIPCode, .Rep = SalesRep, .Cost = Group.Sum(function(p) p.Cost), .Sales = Group.Sum(function(p) p.Price), _
'.Discounts = Group.Sum(function(p) p.Savings), .Profit = Group.Sum(function(p) p.Price) - Group.Sum(function(p) p.Cost) - Group.Sum(function(p) p.Savings)}