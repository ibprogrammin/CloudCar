Imports System.Web.Routing
Imports System.Security
Imports System.Web.Compilation

Public Class WebFormRouteHandler
    Implements IRouteHandler

    Public VirtualPath As String
    Public CheckPhysicalUrlAccess As Boolean

    ' Methods
    Public Sub New(ByVal VirtualPath As String)
        Me.New(VirtualPath, True)
    End Sub

    Public Sub New(ByVal VirtualPath As String, ByVal CheckPhysicalUrlAccess As Boolean)
        Me.VirtualPath = VirtualPath
        Me.CheckPhysicalUrlAccess = CheckPhysicalUrlAccess
    End Sub

    Public Function GetHttpHandler(ByVal RequestContext As RequestContext) As IHttpHandler Implements IRouteHandler.GetHttpHandler

        If (Me.CheckPhysicalUrlAccess AndAlso Not UrlAuthorizationModule.CheckUrlAccessForPrincipal(Me.VirtualPath, requestContext.HttpContext.User, requestContext.HttpContext.Request.HttpMethod)) Then
            Throw New SecurityException
        End If

        Dim CurrentPage As IHttpHandler = TryCast(BuildManager.CreateInstanceFromVirtualPath(Me.VirtualPath, GetType(Page)), IHttpHandler)

        If Not CurrentPage Is Nothing Then
            Dim RoutablePage As IRoutablePage = TryCast(CurrentPage, IRoutablePage)
            If Not RoutablePage Is Nothing Then
                RoutablePage.RequestContext = RequestContext
            End If
        End If

        Return CurrentPage

    End Function

End Class
