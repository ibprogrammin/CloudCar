Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://seriousmonkey.ca/commerce")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class WebServices
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function UserNameExists(ByVal UserName As String) As Boolean
        If Membership.GetUser(UserName) Is Nothing Then
            Return False
        Else
            Return True
        End If
    End Function

End Class