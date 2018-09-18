Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce
    Partial Public Class Customers
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadCustomers()
            End If
        End Sub

        Private Sub LoadCustomers()
            Dim CurrentSimpleUserController As New CCFramework.Core.SimpleUserController

            gvCustomers.DataSource = CurrentSimpleUserController.GetDistinctCustomers()
            gvCustomers.DataBind()
        End Sub

        Private Sub CustomersGridViewPageIndexChanged(ByVal Sender As Object, ByVal E As DataGridPageChangedEventArgs) Handles gvCustomers.PageIndexChanged
            gvCustomers.CurrentPageIndex = E.NewPageIndex

            LoadCustomers()
        End Sub

        Private Sub DownloadCsvButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles DownloadCsvButton.Click
            Dim CustomerCsvFile As New CCFramework.Core.CSVGenerator()

            CustomerCsvFile.AddColumn("First Name")
            CustomerCsvFile.AddColumn("Middle Name")
            CustomerCsvFile.AddColumn("Last Name")
            CustomerCsvFile.AddColumn("Email")
            CustomerCsvFile.AddColumn("Phone Number")

            Dim CurrentSimpleUserController As New CCFramework.Core.SimpleUserController

            For Each CustomerItem As SimpleUser In CurrentSimpleUserController.GetDistinctCustomers()
                Dim CsvRow As DataRow = CustomerCsvFile.CSVData.NewRow

                CsvRow.Item(0) = CustomerItem.FirstName
                CsvRow.Item(1) = CustomerItem.MiddleName
                CsvRow.Item(2) = CustomerItem.LastName
                CsvRow.Item(3) = CustomerItem.Email
                CsvRow.Item(4) = CustomerItem.PhoneNumber

                CustomerCsvFile.AddRow(CsvRow)
            Next

            Dim CsvContent As String = CustomerCsvFile.GenerateCSVString

            Session.Add("d", CsvContent)

            Response.Redirect("/Report/SalesReport.csv")

        End Sub

    End Class

End Namespace