Imports CloudCar.CCFramework.Model

Namespace CCAdmin.ContentManagement

    Partial Public Class SalesLeads
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadLeads()
            End If
        End Sub

        Private Sub LoadLeads()
            gvSalesLads.DataSource = CCFramework.ContentManagement.SalesInquiryController.GetElement.ToList
            gvSalesLads.DataBind()
        End Sub

        Private Sub gvSalesLads_PageIndexChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles gvSalesLads.PageIndexChanged
            gvSalesLads.CurrentPageIndex = e.NewPageIndex

            LoadLeads()
        End Sub

        Protected Sub btnSelect_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim salesLead As SalesInquiry = CCFramework.ContentManagement.SalesInquiryController.GetElement(id)

            litName.Text = salesLead.name
            litEmail.Text = salesLead.email
            litInquiry.Text = salesLead.inquiry
            litDate.Text = salesLead.datesent.ToString("dddd MMM d, yyyy")

            If Not salesLead.checked Then
                CCFramework.ContentManagement.SalesInquiryController.SetChecked(id)
            End If

            phDetails.Visible = True
        End Sub

        Protected Sub btnDelete_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            If CCFramework.ContentManagement.SalesInquiryController.Delete(id) Then
                LoadLeads()
            End If
        End Sub

    End Class
End Namespace