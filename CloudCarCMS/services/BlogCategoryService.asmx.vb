Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports CloudCar.CCFramework.Model

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class BlogCategoryService
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    <Script.Services.ScriptMethod()> _
    Public Function GetCategories(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim db As New BlogDataContext

        Dim Categories As String() = CCFramework.Blogging.BlogController.GetBlogCategoriesFunc(db, prefixText, count).ToArray

        Return Categories
    End Function

End Class