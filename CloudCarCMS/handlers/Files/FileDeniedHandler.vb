Namespace CCFramework.Generic

    Public Class FileDeniedHandler
        Implements System.Web.IHttpHandler

        Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property

        Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
            context.Response.Redirect(Core.Settings.AccessDeniedFile)
        End Sub

    End Class

End Namespace