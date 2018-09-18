Imports CloudCar.CCContentManagement
Imports CloudCar.CCFramework.Core


Partial Public Class CloudCarDefault
    Inherits CloudCarContentPage

    Public Sub New()
        MyBase.New()
        Permalink = Settings.HomePage
    End Sub

    Private Sub LoadPage()

        If Settings.EnableSSL = True And Settings.FullSSL Then
            PageCanonicalMeta.Attributes("href") = String.Format("https://{0}", Settings.HostName)
        Else
            PageCanonicalMeta.Attributes("href") = String.Format("http://{0}", Settings.HostName)
        End If

    End Sub

    Protected Sub PageBoxCalculatorCartItemAdded(ByVal Sender As Object, ByVal Args As EventArgs)
        Response.RedirectToRoute("RouteShoppingCartA")
    End Sub

End Class