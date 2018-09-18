Imports CloudCar.CCFramework.Core

Namespace CCMasterPages

    Partial Public Class Email
        Inherits MasterPage

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            'ssMain.Href = "http://" & SMCore.Settings.HostName & ssMain.Href
            Logo.HRef = "http://" & Settings.HostName & Logo.HRef
            imgLogo.Src = "http://" & Settings.HostName & Settings.LogoImageUrl
        End Sub

    End Class

End Namespace