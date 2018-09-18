Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce

    Partial Public Class PromoCodes
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub RefreshDataSources()
            dgPromoCodes.DataSource = New PromoCodeController().GetElements.OrderBy(Function(f) f.SalesRep).ToList
            dgPromoCodes.DataBind()
        End Sub

        Private Sub dgPromoCodes_PageIndexChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgPromoCodes.PageIndexChanged
            dgPromoCodes.CurrentPageIndex = e.NewPageIndex

            RefreshDataSources()
        End Sub

        Protected Sub btnSelect_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim pc As PromoCode = New PromoCodeController().GetElement(id)

            hfPromoCodeID.Value = pc.ID.ToString
            txtCode.Text = pc.Code
            txtDiscount.Text = pc.Discount.ToString
            txtSalesRep.Text = pc.SalesRep
            ckbFixed.Checked = pc.FixedAmount

            phDetails.Visible = True
        End Sub

        Protected Sub btnDelete_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim ID As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim PromoCode As New PromoCodeController()

            Try
                PromoCode.Delete(ID)

                lblStatus.Text = "You have successfully deleted the Promo Code (ID: " & ID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to delete the Promo Code (ID: " & ID.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub

        Protected Sub btnAdd_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAdd.Command
            If Page.IsValid Then
                Dim ID As Integer
                Dim PromoCode As New PromoCodeController

                If Integer.TryParse(hfPromoCodeID.Value, ID) Then
                    PromoCode.Update(ID, txtCode.Text, Decimal.Parse(txtDiscount.Text), ckbFixed.Checked, txtSalesRep.Text)

                    lblStatus.Text = "You have successfully updated the Promo Code (ID: " & ID.ToString & ")"
                Else
                    ID = PromoCode.Create(txtCode.Text, Decimal.Parse(txtDiscount.Text), ckbFixed.Checked, txtSalesRep.Text)

                    lblStatus.Text = "You have successfully created the Promo Code (ID: " & ID.ToString & ")"
                End If

                lblStatus.Visible = True

                ClearControls()
                RefreshDataSources()
            Else
                lblStatus.Text = "The form is not filled out properly!"
                lblStatus.Visible = True
            End If
        End Sub

        Private Sub btnClear_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnClear.Command
            ClearControls()
        End Sub

        Private Sub ClearControls()
            hfPromoCodeID.Value = Nothing

            txtCode.Text = ""
            txtDiscount.Text = ""
            txtSalesRep.Text = ""

            ckbFixed.Checked = False
        End Sub

    End Class
End Namespace