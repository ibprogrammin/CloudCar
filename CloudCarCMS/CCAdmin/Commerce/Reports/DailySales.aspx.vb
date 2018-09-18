Imports System.Globalization
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCAdmin.Commerce.Reports

    Partial Public Class DailySales
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                FillDefaultDates()
            End If
        End Sub

        Protected Sub lvwDates_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles lvwDates.ItemDataBound
            Dim db As New CommerceDataContext

            If e.Item.ItemType = ListViewItemType.DataItem Then
                Dim CurrentDate As Date = Date.Parse(CType(e.Item.FindControl("hDate"), HiddenField).Value)
                Dim LitDailyTotal As Literal = CType(e.Item.FindControl("litDailyTotal"), Literal)
                LitDailyTotal.Text = OrderController.GetDailySalesTotalFunc(db, CurrentDate).ToString("C")

                If Not CurrentDate = Nothing Then
                    Dim lvOrders As ListView = CType(e.Item.FindControl("lvwOrders"), ListView)

                    If Not lvOrders Is Nothing Then

                        Dim dataset = From o In db.Orders _
                                Where o.OrderDate.Day = CurrentDate.Day _
                                And o.OrderDate.Month = CurrentDate.Month _
                                And o.OrderDate.Year = CurrentDate.Year _
                                And o.ApprovalState = EApprovalState.Approved _
                                And o.OrderItems.Count > 0 _
                                Select New With {.OrderId = o.ID, .OrderDate = o.OrderDate}

                        lvOrders.DataSource = dataset
                        lvOrders.DataBind()
                    End If
                End If

            End If
        End Sub

        Protected Sub lvwOrders_ItemDataBound(ByVal Sender As Object, ByVal Args As ListViewItemEventArgs)
            If Args.Item.ItemType = ListViewItemType.DataItem Then
                Dim OrderID As Integer = Integer.Parse(CType(Args.Item.FindControl("litOrderId"), Literal).Text)
                If Not OrderID = Nothing Then
                    Dim lvProducts As ListView = CType(Args.Item.FindControl("lvwProducts"), ListView)

                    If Not lvProducts Is Nothing Then
                        Dim CurrentDataContext As New CommerceDataContext

                        Dim dataset = (From o In CurrentDataContext.Orders _
                                       Join ot In CurrentDataContext.OrderItems On o.ID Equals ot.OrderID _
                                       Join p In CurrentDataContext.Products On ot.ProductID Equals p.ID _
                                       Where o.ID = OrderID _
                                       Select New With { _
                                           .Name = p.Name, _
                                           .Quantity = ot.Quantity, _
                                           .Price = ot.Price, _
                                           .Total = ot.Price * ot.Quantity, _
                                           .Cost = p.Cost, _
                                           .Profit = (ot.Price * ot.Quantity) - (p.Cost * ot.Quantity) _
                                       }).ToList

                        lvProducts.DataSource = dataset
                        lvProducts.DataBind()

                        Dim CurrentTotal As Decimal = dataset.Sum(Function(CurrentItem) CurrentItem.Total)

                        Dim litTotal As Literal = CType(lvProducts.FindControl("litOrderTotal"), Literal)
                        If Not litTotal Is Nothing Then
                            litTotal.Text = CurrentTotal.ToString("C")
                        End If

                        CurrentDataContext.Dispose()
                    End If
                End If

            End If
        End Sub

        Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGo.Click
            Dim db As New CommerceDataContext

            Dim culture = New CultureInfo("en-US", True)

            Dim initialDate As String = DateTime.Parse(txbInitial.Text, culture, DateTimeStyles.NoCurrentDateDefault).ToShortDateString()
            Dim finalDate As String = DateTime.Parse(txbFinal.Text, culture, DateTimeStyles.NoCurrentDateDefault).ToShortDateString()

            lvwDates.DataSource = From o In db.Orders _
                                  Where o.ApprovalState = EApprovalState.Approved _
                                  Group o By Key = o.OrderDate Into Group _
                                  Where Key.Date >= initialDate And Key.Date <= finalDate _
                                  Select New With {.OrderDate = Key.Date} Distinct
            lvwDates.DataBind()

            db.Dispose()
        End Sub

        Protected Sub FillDefaultDates()

            Dim initialDate As Date = DateTime.Now.AddDays(-10)
            Dim finalDate As Date = DateTime.Now

            txbInitial.Text = initialDate.ToString("M/d/yyyy")
            txbFinal.Text = finalDate.ToString("M/d/yyyy")

        End Sub

        Private Sub DownloadCsvReport()
            Dim CurrentStoreDataContext As New CommerceDataContext

            Dim CSVReport As New CSVGenerator()

            CSVReport.AddColumn("Product")
            CSVReport.AddColumn("Quantity")
            CSVReport.AddColumn("Cost")
            CSVReport.AddColumn("Price")
            CSVReport.AddColumn("Profit")
            CSVReport.AddColumn("Total")

            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentCulture = New CultureInfo("en-US", True)

            Dim CurrentStartDate As String = DateTime.Parse(txbInitial.Text, CurrentCulture, DateTimeStyles.NoCurrentDateDefault).ToShortDateString()
            Dim CurrentEndDate As String = DateTime.Parse(txbFinal.Text, CurrentCulture, DateTimeStyles.NoCurrentDateDefault).ToShortDateString()

            Dim OrderDates = From o In CurrentDataContext.Orders _
                                Where o.ApprovalState = EApprovalState.Approved _
                                Group o By Key = o.OrderDate Into Group _
                                Where Key.Date >= CurrentStartDate And Key.Date <= CurrentEndDate _
                                Select New With {.OrderDate = Key.Date} Distinct

            For Each CurrentDate In OrderDates
                Dim CSVDayRow As DataRow = CSVReport.CSVData.NewRow

                CSVDayRow.Item(0) = CurrentDate.OrderDate.ToString("D").Replace(",", "")
                CSVDayRow.Item(1) = ""
                CSVDayRow.Item(2) = ""
                CSVDayRow.Item(3) = ""
                CSVDayRow.Item(4) = ""
                CSVDayRow.Item(5) = OrderController.GetDailySalesTotalFunc(CurrentStoreDataContext, CurrentDate.OrderDate)

                CSVReport.AddRow(CSVDayRow)

                Dim OrderDate As Date = CurrentDate.OrderDate

                Dim OrderDataSet = From o In CurrentStoreDataContext.Orders _
                        Where o.OrderDate.Day = OrderDate.Day _
                        And o.OrderDate.Month = OrderDate.Month _
                        And o.OrderDate.Year = OrderDate.Year _
                        And o.ApprovalState = EApprovalState.Approved _
                        And o.OrderItems.Count > 0 _
                        Select New With {.OrderId = o.ID, .OrderDate = o.OrderDate}

                For Each CurrentOrder In OrderDataSet
                    Dim CSVOrderRow As DataRow = CSVReport.CSVData.NewRow

                    CSVOrderRow.Item(0) = String.Format("Order ({0})", CurrentOrder.OrderId)
                    CSVOrderRow.Item(1) = ""
                    CSVOrderRow.Item(2) = ""
                    CSVOrderRow.Item(3) = ""
                    CSVOrderRow.Item(4) = ""
                    CSVOrderRow.Item(5) = ""

                    CSVReport.AddRow(CSVOrderRow)

                    Dim CurrentOrderId As Integer = CurrentOrder.OrderId

                    Dim OrderItemDataSet = (From o In CurrentDataContext.Orders _
                                           Join ot In CurrentDataContext.OrderItems On o.ID Equals ot.OrderID _
                                           Join p In CurrentDataContext.Products On ot.ProductID Equals p.ID _
                                           Where o.ID = CurrentOrderId _
                                           Select New With { _
                                               .Name = p.Name, _
                                               .Quantity = ot.Quantity, _
                                               .Price = ot.Price, _
                                               .Total = ot.Price * ot.Quantity, _
                                               .Cost = p.Cost, _
                                               .Profit = (ot.Price * ot.Quantity) - (p.Cost * ot.Quantity) _
                                           }).ToList

                    For Each CurrentOrderItem In OrderItemDataSet
                        Dim CSVOrderItemRow As DataRow = CSVReport.CSVData.NewRow

                        CSVOrderItemRow.Item(0) = CurrentOrderItem.Name
                        CSVOrderItemRow.Item(1) = CurrentOrderItem.Quantity
                        CSVOrderItemRow.Item(2) = CurrentOrderItem.Cost
                        CSVOrderItemRow.Item(3) = CurrentOrderItem.Price
                        CSVOrderItemRow.Item(4) = CurrentOrderItem.Profit
                        CSVOrderItemRow.Item(5) = CurrentOrderItem.Total

                        CSVReport.AddRow(CSVOrderItemRow)
                    Next

                    Dim CurrentOrderTotal As Decimal = OrderItemDataSet.Sum(Function(CurrentItem) CurrentItem.Total)
                    Dim CSVOrderTotalRow As DataRow = CSVReport.CSVData.NewRow

                    CSVOrderTotalRow.Item(0) = ""
                    CSVOrderTotalRow.Item(1) = ""
                    CSVOrderTotalRow.Item(2) = ""
                    CSVOrderTotalRow.Item(3) = ""
                    CSVOrderTotalRow.Item(4) = ""
                    CSVOrderTotalRow.Item(5) = CurrentOrderTotal

                    CSVReport.AddRow(CSVOrderTotalRow)
                Next

            Next

            Dim CsvContent As String = CSVReport.GenerateCSVString

            Session.Add("d", CsvContent)
            Response.Redirect("/Report/SalesReport.csv")
        End Sub

        Private Sub DownloadCsvButtonClick(Sender As Object, Args As EventArgs) Handles DownloadCsvButton.Click
            DownloadCsvReport()
        End Sub

    End Class

End Namespace