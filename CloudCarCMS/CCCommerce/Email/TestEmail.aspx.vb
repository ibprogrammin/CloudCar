Namespace CCCommerce.Email
    Public Partial Class TestEmail
        Inherits Page

        Private Sub btnTest_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnTest.Command
            Dim email As String = CCFramework.Core.SendEmails.GetEmail(MyBase.Context, "~/CCCommerce/Email/NewOrderInvoice.aspx?Order=3")

            'TODO add the email address from the web config
            CCFramework.Core.SendEmails.Send(New System.Net.Mail.MailAddress("info@seriousmonkey.ca"), "a Test invoice", email)
        End Sub
    End Class
End NameSpace