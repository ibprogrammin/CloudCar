Imports CloudCar.CCFramework.Model

Namespace CCFramework.Core

    Public Class ErrorController

        Public Shared Sub LogError(ByVal E As Exception, ByVal Context As HttpContext, ByVal Project As String)
            'Dim lastError As Exception = filterContext.Exception
            'Dim filterContext As ActionExecutedContext = CType(stateInfo, ActionExecutedContext)

            Dim CurrentDataContext As New ErrorDataContext

            Dim newError As New SiteError()

            newError.id = Guid.NewGuid
            newError.action = "" 'filterContext.ActionDescriptor.ActionName
            newError.controller = "" 'filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
            newError.timestamp = Context.Timestamp 'filterContext.HttpContext.Timestamp
            newError.ipaddress = Context.Request.UserHostAddress 'filterContext.HttpContext.Request.UserHostAddress
            newError.helplink = e.HelpLink
            newError.message = e.Message & "*****************" & e.InnerException.Message
            newError.source = e.Source
            newError.stacktrace = e.StackTrace & "******" & e.InnerException.StackTrace
            newError.targetsite = e.TargetSite.Name
            newError.Application = Reflection.Assembly.GetExecutingAssembly.GetName.Name & "(" & Project & ")"

            CurrentDataContext.SiteErrors.InsertOnSubmit(newError)
            CurrentDataContext.SubmitChanges()

            CurrentDataContext.Dispose()
        End Sub

    End Class

End Namespace