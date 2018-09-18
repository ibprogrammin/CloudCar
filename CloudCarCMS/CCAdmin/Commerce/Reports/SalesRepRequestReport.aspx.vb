Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce.Reports

    Partial Public Class SalesRepRequestReport
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

        Protected Sub lvMonthData_ItemDataBound(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles lvMonthData.ItemDataBound
            If e.Item.ItemType = ListViewItemType.DataItem Then
                Dim SelectedYear As Integer = Integer.Parse(ddlYearSelection.SelectedValue)
                Dim SelectedMonth As Integer = Integer.Parse(CType(e.Item.FindControl("hfMonth"), HiddenField).Value)

                If Not SelectedYear = Nothing And Not SelectedMonth = Nothing Then
                    Dim SalesBreakdownDataGrid As DataGrid = CType(e.Item.FindControl("dgSalesBreakdown"), DataGrid)

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
            phDisplay.Visible = True

            Dim SalesTable = From o In CurrentStoreDataContext.Orders _
                    Where o.OrderDate.Year = SelectedYear _
                          And o.ApprovalState = 1 _
                          And (From oi In CurrentStoreDataContext.OrderItems _
                                  Where oi.OrderID = o.ID Select oi).Count > 0 _
                          And (From sr In CurrentStoreDataContext.SalesRequests _
                                  Where sr.OrderId = o.ID Select sr).Count > 0 _
                    Group By Year = o.OrderDate.Year, Month = o.OrderDate.Month Into Group _
                    Order By Year, Month Ascending _
                    Select New With {Year, Month} Distinct

            lvMonthData.DataSource = SalesTable
            lvMonthData.DataBind()

            CurrentStoreDataContext.Dispose()

            lvMonthData.Visible = True
        End Sub

        Private Sub DownloadCsvReport()
            Dim CurrentStoreDataContext As New CommerceDataContext

            Dim CSVReport As New CCFramework.Core.CSVGenerator()

            CSVReport.AddColumn("Branch")
            CSVReport.AddColumn("SalesRep")
            CSVReport.AddColumn("Year")
            CSVReport.AddColumn("Month")
            CSVReport.AddColumn("Product")
            CSVReport.AddColumn("Quantity")

            Dim SelectedYear As Integer = Integer.Parse(ddlYearSelection.SelectedValue)

            Dim SalesTable = From o In CurrentStoreDataContext.Orders _
                    Where o.OrderDate.Year = SelectedYear _
                          And o.ApprovalState = 1 _
                          And (From oi In CurrentStoreDataContext.OrderItems _
                                  Where oi.OrderID = o.ID Select oi).Count > 0 _
                          And (From sr In CurrentStoreDataContext.SalesRequests _
                                  Where sr.OrderId = o.ID Select sr).Count > 0 _
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
                        CSVRow.Item(2) = SalesItem.Year
                        CSVRow.Item(3) = MonthName(SalesItem.Month)
                        CSVRow.Item(4) = SalesItem.Product
                        CSVRow.Item(5) = SalesItem.ActualQuantity

                        CSVReport.AddRow(CSVRow)
                    Next

                    Dim CSVMonthlyTotalRow As DataRow = CSVReport.CSVData.NewRow

                    CSVReport.AddRow(CSVMonthlyTotalRow)
                End If
            Next

            Dim CsvContent As String = CSVReport.GenerateCSVString

            'Request.Form.Set("d", CsvContent)
            'hfReportData.Value = CsvContent
            Session.Add("d", CsvContent)
            Response.Redirect("/Report/SalesReport.csv")
            'Page.Server.Transfer("/Download/Report/SalesReport.csv", True)
        End Sub

        Private Function GetReportData() As IEnumerable(Of SalesRequestReportData)
            Dim CurrentStoreDataContext As New CommerceDataContext

            Dim CurrentReportData = From sr In CurrentStoreDataContext.SalesRequests _
                    Where sr.Redeemed = True And sr.OrderId.HasValue _
                    Join p In CurrentStoreDataContext.Products On sr.ProductId Equals p.ID _
                    Let Year = CType((From o In CurrentStoreDataContext.Orders _
                    Where o.ID = sr.OrderId Select o.OrderDate.Year).FirstOrDefault,  _
                                     Nullable(Of Integer)) _
                    Let Month = CType((From o In CurrentStoreDataContext.Orders _
                    Where o.ID = sr.OrderId Select o.OrderDate.Month).FirstOrDefault,  _
                                      Nullable(Of Integer)) _
                    Let Branch = (From o In CurrentStoreDataContext.Orders _
                    Where o.ID = sr.OrderId _
                    Join a In CurrentStoreDataContext.Addresses _
                    On o.SAddressID Equals a.id _
                    Let CurrentBranch = If(Not o.DistributorId.HasValue, CType(o.DistributorId, String), a.PCZIP) _
                    Select CurrentBranch).FirstOrDefault _
                    Let SalesRep = sr.SalesRepName _
                    Let ProductName = p.Name _
                    Let Quantity = sr.Quantity _
                    Let ActualQuantity = CType((From oi In CurrentStoreDataContext.OrderItems _
                    Where sr.ProductId = oi.ProductID _
                          And oi.OrderID = sr.OrderId _
                    Group By oi.ProductID Into Group _
                    Select New With {.ProductId = ProductID, _
                    .Quantity = Group.Sum(Function(f) f.Quantity)}).FirstOrDefault.Quantity,  _
                                               Nullable(Of Integer)) _
                    Group By Year, Month, Branch, SalesRep, ProductName Into Group _
                    Select New With {Year, Month, Branch, SalesRep, ProductName, .Quantity = Group.Sum(Function(f) f.Quantity), .ActualQuantity = Group.Sum(Function(f) f.ActualQuantity)}

            Dim AdjustedReportData = From rd In CurrentReportData.AsEnumerable _
                    Let Year = rd.Year _
                    Let Month = rd.Month _
                    Let Branch = CCFramework.Commerce.FixedShippingZoneController.GetBranchCity(rd.Branch) _
                    Let SalesRep = rd.SalesRep _
                    Let Product = rd.ProductName _
                    Let Quantity = rd.Quantity _
                    Let ActualQuantity = rd.ActualQuantity _
                    Group By Year, Month, Branch, SalesRep, Product Into Group _
                    Select New SalesRequestReportData With _
                    {.Year = Year.Value, .Month = Month.Value, .Branch = Branch, .SalesRep = SalesRep, .Product = Product, _
                    .Quantity = Group.Sum(Function(f) f.Quantity), .ActualQuantity = Group.Sum(Function(f) f.ActualQuantity).Value}

            Return AdjustedReportData.AsEnumerable

            'CurrentStoreDataContext.Dispose()
        End Function

        Protected Sub FillDefaultDates()
            Dim EarliestYear As Integer = OrderController.GetEarliestOrderYear
            Dim LatestYear As Integer = OrderController.GetLatestOrderYear

            ddlYearSelection.Items.Add(New ListItem("Year", ""))

            For i As Integer = EarliestYear To LatestYear
                ddlYearSelection.Items.Add(New ListItem(i.ToString, i.ToString))
            Next
        End Sub

        Public Class SalesRequestReportData
            Private _Year As Integer
            Private _Month As Integer
            Private _Branch As String
            Private _SalesRep As String
            Private _Product As String
            Private _Quantity As Integer
            Private _ActualQuantity As Integer

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

            Public Property ActualQuantity() As Integer
                Get
                    Return _ActualQuantity
                End Get
                Set(ByVal value As Integer)
                    _ActualQuantity = value
                End Set
            End Property

        End Class

    End Class
End Namespace