Imports System.Web
Imports System.Web.Services
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Generic

    Public Class FileDownloadHandler
        Implements IHttpHandler

        Private Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            If Not context.Request.QueryString("k") Is Nothing And Not context.Request.QueryString("k") = String.Empty Then
                Dim guid As Guid = New Guid(context.Request.QueryString("k"))
                Dim ProductDownload As ProductDownload = New Commerce.ProductDownloadController().GetElement(guid)

                If Not ProductDownload Is Nothing Then
                    If Core.Settings.AllowableFileDowloads = 0 Then
                        StartDownload(ProductDownload, context)
                    Else
                        If ProductDownload.Downloads > Core.Settings.AllowableFileDowloads Then
                            context.Response.Redirect(Core.Settings.AccessDeniedFile)
                        Else
                            StartDownload(ProductDownload, context)
                        End If
                    End If
                Else
                    context.Response.Redirect(Core.Settings.AccessDeniedFile)
                End If
            Else
                context.Response.Redirect(Core.Settings.AccessDeniedFile)
            End If
        End Sub

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

        Private Sub StartDownload(ByVal ProductDownload As ProductDownload, ByVal context As HttpContext)
            If Not Membership.GetUser Is Nothing Then
                If Not Roles.IsUserInRole(Membership.GetUser.UserName, "Administrator") Then
                    Dim updateDownloads As Boolean = New Commerce.ProductDownloadController().Update(ProductDownload.GUID, ProductDownload.OrderItemID, ProductDownload.Filename, ProductDownload.Downloads + 1)
                End If
            Else
                Dim updateDownloads As Boolean = New Commerce.ProductDownloadController().Update(ProductDownload.GUID, ProductDownload.OrderItemID, ProductDownload.Filename, ProductDownload.Downloads + 1)
            End If

            context.Response.Buffer = True
            context.Response.Clear()
            context.Response.AddHeader("content-disposition", "attachement; filename=" & ProductDownload.Filename)
            context.Response.ContentType = GetContentType(ProductDownload.Filename)
            context.Response.WriteFile(Core.Settings.ProductUploadPath & ProductDownload.Filename)
        End Sub

        Private Function GetContentType(ByVal Filename As String) As String
            Dim ContentType As String = "application/octetstream"
            Dim Extension As String = System.IO.Path.GetExtension(Filename).ToLower()

            Dim RegistryKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Extension)

            If Not RegistryKey Is Nothing And Not RegistryKey.GetValue("Content Type") Is Nothing Then
                ContentType = RegistryKey.GetValue("Content Type").ToString()
            End If

            Return ContentType
        End Function

    End Class

End Namespace