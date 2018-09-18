Public Class ClientHeaders
    Inherits Page

    Protected Overrides Sub OnLoad(ByVal E As EventArgs)
        For Each item In HttpContext.Current.Request.ServerVariables
            HeadersLiteral.Text &= item.ToString & " - " & HttpContext.Current.Request.ServerVariables(item).ToString & "<br />"
        Next
        'HeadersLiteral.Text &= HttpContext.Current.Request.ClientCertificate. & "<br />"
        HeadersLiteral.Text &= HttpContext.Current.Request.UserHostAddress & " - " & HttpContext.Current.Request.ServerVariables("LOCAL_ADDR")
    End Sub

End Class