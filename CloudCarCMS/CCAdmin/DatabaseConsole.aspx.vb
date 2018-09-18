Imports System.Data.SqlClient
Imports CloudCar.CCFramework.Model

Namespace CCAdmin

    Partial Public Class DatabaseConsole
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load

        End Sub

        Private Sub SubmitQueryButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles SubmitQueryButton.Click
            Dim DatabaseQueryCommand As String = QueryTextBox.Text

            'Dim CurrentDataContext As New CommerceDataContext
            'Dim RowCount As Integer

            Try
                Dim DataAdapter As New SqlDataAdapter(DatabaseQueryCommand, New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString))
                Dim ResultingDataTable As New DataTable("Results")

                DataAdapter.Fill(ResultingDataTable)

                Dim ResultTableStringBuilder As New StringBuilder()

                ResultTableStringBuilder.Append("<table width=""880px""><thead><tr>")

                For Each ColumnItem As DataColumn In ResultingDataTable.Columns
                    ResultTableStringBuilder.Append(<td><b><%= ColumnItem.ColumnName %></b></td>.ToString)
                Next

                ResultTableStringBuilder.Append("</tr>")

                For Each Item As DataRow In ResultingDataTable.Rows
                    Dim Columns As Integer = ResultingDataTable.Columns.Count

                    ResultTableStringBuilder.Append("<tr>")

                    For Index As Integer = 0 To Columns - 1
                        ResultTableStringBuilder.Append(<td><%= Item(Index).ToString %></td>.ToString)
                    Next

                    ResultTableStringBuilder.Append("</tr>")
                Next

                ResultTableStringBuilder.Append("</table>")

                StatusLabel.Text = ResultTableStringBuilder.ToString
            Catch Ex As Exception
                StatusLabel.Text = Ex.Message
            End Try

            StatusLabel.Visible = True

        End Sub

        Private Sub SubmitCommandButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles SubmitCommandButton.Click
            Dim DatabaseQueryCommand As String = CommandTextBox.Text

            Dim CurrentDataContext As New CommerceDataContext
            Dim RowCount As Integer

            Try
                RowCount = CurrentDataContext.ExecuteCommand(DatabaseQueryCommand)

                StatusLabel.Text = "Affected rows: " & RowCount
            Catch Ex As Exception
                StatusLabel.Text = Ex.Message
            End Try

            StatusLabel.Visible = True
        End Sub

    End Class

End Namespace