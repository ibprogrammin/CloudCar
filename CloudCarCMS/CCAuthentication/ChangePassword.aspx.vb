Namespace CCAuthentication

    Partial Public Class ChangePassword
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                If Not Membership.GetUser Is Nothing Then
                    UserNameTextBox.Value = Membership.GetUser.UserName
                End If
            End If
        End Sub

    End Class

End Namespace