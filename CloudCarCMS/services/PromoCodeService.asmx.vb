Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://localhost:56361/Services/", Name:="PromoCodeService")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class PromoCodeService
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function PromoCodeDiscount(ByVal Code As String, ByVal Subtotal As Decimal) As Decimal
        Dim db As New CommerceDataContext

        Dim pc As PromoCode = PromoCodeController.GetPromoCodeByCodeFunc(db, Code)

        Dim discount As Decimal = 0

        If Not pc Is Nothing Then
            If pc.FixedAmount Then
                discount = pc.Discount
            Else
                discount = CDec(Math.Round((Subtotal * (pc.Discount * 0.01)), 2))
            End If
        End If

        Return discount
    End Function

    '<WebMethod()> _
    'Public Function PromoCodeExists() As Boolean
    '    Return True
    'End Function

End Class