Imports System.Web
Imports System.Web.Services

Public Class GetPDF
    Implements IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        'Dim arrContent As Byte()
        Try
            'Dim ENID As Integer
            'Dim db As New ContentDataContext
            'Dim en As EventAndNew
            'Dim mStream As System.IO.MemoryStream

            'ENID = context.Request.Item("ENID")
            'en = (From n In db.EventAndNews Where n.EventAndNewsID = ENID Select n).Single

            'arrContent = en.PDFData.ToArray
            'mStream = New System.IO.MemoryStream(arrContent, 0, en.PDFContentLength)

            'context.Response.Clear()
            'context.Response.ContentType = "application/pdf"
            'context.Response.Cache.SetCacheability(HttpCacheability.Public)
            'context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(15))
            'context.Response.Cache.SetMaxAge(New TimeSpan(0, 15, 0))
            'context.Response.BinaryWrite(arrContent.ToArray)

            'db = Nothing
            'en = Nothing
            'mStream = Nothing
        Catch ex As Exception
        End Try
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class