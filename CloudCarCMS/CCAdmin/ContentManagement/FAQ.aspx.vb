Namespace CCAdmin.ContentManagement
    Partial Public Class FAQ
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            lblStatus.Text = ""
        End Sub

        Private Sub btnAddFaq_Command(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles btnAddFaq.Command
            phAddFaq.Visible = True
            gvFAQ.Visible = False
            btnAddFaq.Visible = False
        End Sub

        Private Sub btnAdd_Command(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles btnAdd.Command
            CCFramework.ContentManagement.FAQController.Create(txtQuestion.Text, txtAnswer.Text, Integer.Parse(txtOrder.Text))

            gvFAQ.DataBind()

            phAddFaq.Visible = False
            gvFAQ.Visible = True
            btnAddFaq.Visible = True
        End Sub

        Private Sub btnCancel_Command(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles btnCancel.Command
            phAddFaq.Visible = False
            gvFAQ.Visible = True
            btnAddFaq.Visible = True
        End Sub

        Protected Sub gvFAQ_RowCommand(ByVal Sender As Object, ByVal Args As GridViewCommandEventArgs) Handles gvFAQ.RowCommand
            If Args.CommandName = "DeleteFAQ" Then
                Dim faqId As Integer = Integer.Parse(Args.CommandArgument.ToString)

                If CCFramework.ContentManagement.FAQController.Delete(faqId) Then
                    lblStatus.Text = "FAQ has been successfully deleted."
                    lblStatus.Visible = True
                End If

                gvFAQ.DataBind()
            End If
        End Sub

    End Class

End Namespace