Imports CloudCar.CCFramework.Core

Namespace CCMasterPages

    Partial Public Class Admin
        Inherits MasterPage

        Private Const _LogoutRedirectMessage As String = "Your session has expired, you will now be redirected to the login page!"

        Protected Overrides Sub OnLoad(ByVal E As EventArgs)
            If Not Page.IsPostBack Then
                imgLogoImage.Src = Settings.LogoImageUrl
            End If
        End Sub

        Protected Sub StoreMenuLoad(ByVal Sender1 As Object, ByVal E As EventArgs)
            If Not Settings.StoreAdminEnabled Then
                Sender1.Visible = False
            End If
        End Sub

        Protected Sub DatabaseConsoleLinkLoad(ByVal Sender As Object, ByVal E As EventArgs)
            If Roles.IsUserInRole(Membership.GetUser.UserName, "Super User") Then
                Sender.Visible = True
            End If
        End Sub

        Protected Sub MasterLoginStatusLoggingOut(ByVal Sender As Object, ByVal Args As LoginCancelEventArgs)
            If Not Session("SessionId") Is Nothing Then
                Session("SessionId") = Guid.NewGuid
            End If
        End Sub

        Protected Overrides Sub OnPreRender(Args As EventArgs)
            If Not Membership.GetUser Is Nothing Then
                AutoRedirect()
            End If
        End Sub

        Public Sub AutoRedirect()
            Dim MilliSecondsTimeOut As Integer = ((Session.Timeout + 2) * 60000)

            Dim CurrentScriptString As String = "intervalset = window.setInterval('Redirect()'," _
                                                & MilliSecondsTimeOut.ToString() & ");" _
                                                & "function Redirect() { " _
                                                & "alert('" & _LogoutRedirectMessage & "\n\n');" _
                                                & "window.location.href='/login.html';}"

            ScriptManager.RegisterClientScriptBlock(MyBase.Page, Me.GetType(), "Redirect", CurrentScriptString, True)
        End Sub

    End Class

End Namespace