Imports System.Web
Imports System.IO
Imports Microsoft.Win32

Namespace CCFramework.Generic

    Public Class CSVReportHandler
        Inherits Page
        Implements IHttpHandler
        Implements IRequiresSessionState

        Private Const _CurrentFileName As String = "SalesReport.csv"

        Private Shared Sub SetHeaders(ByVal CurrentContext As HttpContext)
            CurrentContext.Response.ContentType = GetContentType(_CurrentFileName)
            CurrentContext.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", _CurrentFileName))
        End Sub

        Private Shared Function GetContentType(ByVal Filename As String) As String
            Dim CurrentContentType As String = "text/csv"
            Dim Extension As String = Path.GetExtension(Filename).ToLower()

            Dim RegistryKey As RegistryKey = Registry.ClassesRoot.OpenSubKey(Extension)

            If Not RegistryKey Is Nothing And Not RegistryKey.GetValue("Content Type") Is Nothing Then
                CurrentContentType = RegistryKey.GetValue("Content Type").ToString()
            End If

            Return CurrentContentType
        End Function

        Public Overloads ReadOnly Property IsReusable() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides Sub ProcessRequest(ByVal CurrentContext As HttpContext)
            If Not CurrentContext.Session("d") Is Nothing Then
                Dim CurrentData As String = CurrentContext.Session("d").ToString

                SetHeaders(CurrentContext)
                CurrentContext.Response.Write(CurrentData)
            Else
                SetHeaders(CurrentContext)
                CurrentContext.Response.Write("This file contains no data.")
            End If
        End Sub

    End Class

End Namespace