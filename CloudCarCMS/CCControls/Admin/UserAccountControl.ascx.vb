Namespace CCControls.Admin

    Public Class UserAccountControl
        Inherits UserControl

        Protected Sub LoginStatusLoggingOut(ByVal Sender As Object, ByVal Args As LoginCancelEventArgs)
            If Not Session("SessionId") Is Nothing Then
                Session.Abandon()
                'Session.RemoveAll()
                'Session("SessionId") = Guid.NewGuid
            End If
        End Sub

    End Class

End Namespace