Imports System
Imports System.Web
Imports AjaxControlToolkit

Namespace CCFramework.Generic

    Public Class CombineScriptHandler
        Implements IHttpHandler

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            'For Each item As String In context.Request.QueryString
            'item = item.Replace("amp;", "")
            'context.Response.Write(item)
            'Next

            If Not ToolkitScriptManager.OutputCombinedScriptFile(context) Then
                Throw New InvalidOperationException("Combined script file output failed unexpectedly.")
            End If
        End Sub

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property

    End Class

End Namespace