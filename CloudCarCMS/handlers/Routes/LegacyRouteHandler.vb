Imports System.Security
Imports System.Web.Compilation
Imports System.Web.Routing

Namespace handlers.Routes

    Public Class LegacyRouteHandler
        Implements IRouteHandler

        Private ReadOnly _NewUrl As String
        Private ReadOnly _CheckPhysicalUrlAccess As Boolean

        Public Sub New(ByVal NewUrl As String)
            Me.New(NewUrl, True)
        End Sub

        Public Sub New(ByVal NewUrl As String, ByVal CheckPhysicalUrlAccess As Boolean)
            _NewUrl = NewUrl
            _CheckPhysicalUrlAccess = CheckPhysicalUrlAccess
        End Sub

        Public Function GetHttpHandler(ByVal RequestContext As RequestContext) As IHttpHandler Implements IRouteHandler.GetHttpHandler

            If (_CheckPhysicalUrlAccess AndAlso Not UrlAuthorizationModule.CheckUrlAccessForPrincipal(_NewUrl, RequestContext.HttpContext.User, RequestContext.HttpContext.Request.HttpMethod)) Then
                Throw New SecurityException
            End If

            'Dim page As IHttpHandler = CType(BuildManager.CreateInstanceFromVirtualPath(Me.VirtualPath, GetType(Page)), IHttpHandler)

            'If (Not page Is Nothing) Then
            '    Dim routablePage As IRoutablePage = TryCast(page, IRoutablePage)
            '    If (Not routablePage Is Nothing) Then
            '        routablePage.RequestContext.HttpContext.Response.Status = "301 Moved Permanently"
            '        routablePage.RequestContext.HttpContext.Response.AppendHeader("Location", VirtualPath)
            '        routablePage.RequestContext = requestContext
            '    End If
            'End If
            '
            'Return page

            Return New RedirectHandler(_NewUrl)

        End Function

    End Class

    Public Class RedirectHandler
        Implements IHttpHandler

        Private ReadOnly _NewUrl As String

        Public Sub New(NewUrl As String)
            _NewUrl = NewUrl
        End Sub

        Public ReadOnly Property IsReusable As Boolean Implements IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property

        Public Sub ProcessRequest(Context As HttpContext) Implements IHttpHandler.ProcessRequest
            Context.Response.Status = "301 Moved Permanently"
            Context.Response.StatusCode = 301
            Context.Response.AppendHeader("Location", _NewUrl)
            Return
        End Sub

    End Class


End Namespace