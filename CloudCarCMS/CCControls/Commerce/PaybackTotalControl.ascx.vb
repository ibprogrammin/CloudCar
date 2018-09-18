Namespace CCControls.Commerce

    Partial Public Class PaybackTotalControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                GetTotals()
            End If
        End Sub

        Private Sub GetTotals()
            Dim total As New CCFramework.Core.PaybackTotal

            litCostOfWebsite.Text = CCFramework.Core.PaybackTotal.WebsiteCost.ToString("C")
            litCostOfGoodsSold.Text = total.getCost.ToString("C")
            litTotalSales.Text = total.getRetail.ToString("C")
            litTotalProfitFromSales.Text = total.getProfit.ToString("C")
            litAmountRemaining.Text = (CCFramework.Core.PaybackTotal.WebsiteCost - total.getProfit).ToString("C")
        End Sub

    End Class

End Namespace