Namespace CCContentManagement
    Public Partial Class Unsuscribe
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load

            If Not Page.IsPostBack Then

                If Not Request.QueryString("Email") Is Nothing Then
                    Dim email As New System.Net.Mail.MailAddress(Request.QueryString("Email"))
                    txbUnsuscribe.Text = email.Address.ToString()
                End If

            End If
        End Sub

        Protected Sub btnUnsuscribe_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUnsuscribe.Click

            Try

                If Not txbUnsuscribe.Text Is "" Then
                    Unsuscribe(txbUnsuscribe.Text)
                    lbmsg.Text = ""
                Else
                    lbmsg.Text = "incorrect email address"
                End If

            Catch ex As Exception
                lbmsg.Text = "incorrect email address"
            End Try

        End Sub

        Protected Sub Unsuscribe(ByVal emailAddress As String)

            CCFramework.ContentManagement.SubscriptionController.OptOut(New System.Net.Mail.MailAddress(emailAddress))

        End Sub
    End Class
End NameSpace