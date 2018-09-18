Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.Commerce.ShoppingCart

Namespace CCAuthentication
    Partial Public Class Login
        Inherits RoutablePage

        Private Sub LoginButtonClick(ByVal Sender As Object, ByVal E As CommandEventArgs) Handles btnLogin.Command
            Dim CurrentSessionId As String = Session("SessionId").ToString

            Dim AuthenticatedUser As MembershipUser = Membership.GetUser(Username.Text)

            If Membership.ValidateUser(Username.Text, Password.Text) Then
                Dim CurrentExpiration As DateTime = DateTime.Now.AddMinutes(30)
                If RememberMe.Checked Then
                    CurrentExpiration = DateTime.Now.AddMonths(1)
                End If

                Dim Ticket As New FormsAuthenticationTicket(1, Username.Text, DateTime.Now, CurrentExpiration, RememberMe.Checked, FormsAuthentication.FormsCookiePath)
                Dim CookieString As String = FormsAuthentication.Encrypt(Ticket)
                Dim Cookie As New HttpCookie(FormsAuthentication.FormsCookieName, CookieString)

                Cookie.Expires = Ticket.Expiration
                Cookie.Path = FormsAuthentication.FormsCookiePath

                Response.Cookies.Add(Cookie)

                FormsAuthentication.RedirectFromLoginPage(Username.Text, RememberMe.Checked)

                Dim CurrentRegisteredUserController As New RegisteredUserController()
                Dim CurrentUserId As Integer = CurrentRegisteredUserController.GetByUserName(Username.Text).UserID
                CurrentRegisteredUserController.Dispose()

                Dim CurrentShoppingCartController As New ShoppingCartController
                CurrentShoppingCartController.MoveShoppingCartItems(CurrentSessionId, CurrentUserId)
                CurrentShoppingCartController.Dispose()

                If Roles.IsUserInRole(AuthenticatedUser.UserName, "Administrator") Then
                    If Request.QueryString("ReturnUrl") Is Nothing Then
                        Response.Redirect("/CCAdmin")
                    Else
                        Response.Redirect(Request.QueryString("ReturnUrl"))
                    End If
                Else
                    If Request.QueryString("ReturnUrl") Is Nothing Then
                        Response.RedirectToRoute("RouteHomePageB")
                    Else
                        Response.Redirect(Request.QueryString("ReturnUrl"))
                    End If
                End If
            Else
                lblError.Text = String.Format("<b>Ooops!</b> There was an error trying to log you in! Please make sure your username and password are correct.")
                lblError.Visible = True
            End If
        End Sub

    End Class
End Namespace