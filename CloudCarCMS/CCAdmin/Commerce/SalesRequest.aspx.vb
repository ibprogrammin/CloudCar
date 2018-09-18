Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce

    Partial Public Class SalesRequest
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadSalesRequests()
            End If
        End Sub

        Private Sub LoadSalesRequests()
            Dim CurrentSalesRequestController As New SalesRequestController()
            Dim SalesRequests As List(Of CCFramework.Model.SalesRequest) = CurrentSalesRequestController.GetElements()

            dgSalesRequests.DataSource = SalesRequests
            dgSalesRequests.DataBind()
        End Sub

        Public Sub btnSendNotice_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim SalesRequestId As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim CurrentSalesRequestController As New SalesRequestController()
            Dim CurrentSalesRequest As CCFramework.Model.SalesRequest = CurrentSalesRequestController.GetElement(SalesRequestId)

            SalesRequestController.SendCustomerRequestEmail(CurrentSalesRequest.CustomerEmail, CurrentSalesRequest.CustomerName, CurrentSalesRequest.RequestKey)
        End Sub

        Private Sub SalesRequestsDataGridPageIndexChanged(ByVal Source As Object, ByVal E As DataGridPageChangedEventArgs) Handles dgSalesRequests.PageIndexChanged
            dgSalesRequests.CurrentPageIndex = e.NewPageIndex
            dgSalesRequests.DataBind()
        End Sub

    End Class
End Namespace