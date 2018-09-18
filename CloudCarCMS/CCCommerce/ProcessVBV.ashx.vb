Imports System.Web
Imports CloudCar.CCFramework.Core.BeanStreamBilling
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.PaymentProcessing
Imports CloudCar.CCFramework.Core
Imports System.Net
Imports System.IO

Namespace CCCommerce

    Public Class ProcessVBV
        Implements IHttpHandler

        Sub ProcessRequest(ByVal Context As HttpContext) Implements IHttpHandler.ProcessRequest
            context.Response.ContentType = "text/html"

            'TODO Should this be the way it is intended
            'If Not Context.Request.Item("PaRes") Is Nothing AndAlso Context.Request.Item("MD") Is Nothing Then
            If Not Context.Request.Item("PaRes") Is Nothing Or Context.Request.Item("MD") Is Nothing Then
                Dim PaRes As String = Context.Request.Item("PaRes")
                Dim CurrentAuthorizationCode As String = String.Empty
                Dim md As String = Context.Request.Item("MD")

                Dim ReturnMessage As String() = PostToBeanStream(PaRes, md).Split("&"c)

                Dim CurrentOrderId As Integer = 0
                Dim CurrentTransactionId As Integer = 0

                For Each CurrentReturnMessage In ReturnMessage
                    If CurrentReturnMessage.Contains("trnOrderNumber=") Then
                        CurrentOrderId = Integer.Parse(CurrentReturnMessage.Replace("trnOrderNumber=", ""))
                    ElseIf CurrentReturnMessage.Contains("trnId=") Then
                        CurrentTransactionId = Integer.Parse(CurrentReturnMessage.Replace("trnId=", ""))
                    ElseIf CurrentReturnMessage.Contains("authCode=") Then
                        CurrentAuthorizationCode = CurrentReturnMessage.Replace("authCode=", String.Empty)
                    End If
                Next

                If Not CurrentOrderId = 0 Then
                    Dim CurrentOrderAuthorized As Boolean
                    CurrentOrderAuthorized = PaymentController.AuthorizeVBV(CurrentAuthorizationCode, CurrentOrderId, CurrentTransactionId, PaymentStatus.Approved)

                    If CurrentOrderAuthorized Then
                        Dim CurrentOrderController As New OrderController
                        Dim CurrentSimpleUserController As New SimpleUserController

                        Dim CurrentOrder As Order = CurrentOrderController.GetElement(CurrentOrderId)
                        Dim CurrentSimpleUser As SimpleUser = CurrentSimpleUserController.GetElement(CurrentOrder.UserID)

                        'If message.IsMembership Then
                        '   Send an Email with the safety waiver
                        'Else
                        SendEmails.SendEmailInvoice(CurrentOrderId, CurrentSimpleUser.Email, True, Context)
                        'End If

                        'Context.Response.RedirectToRoute("RouteThankYouA")
                        Context.Response.Redirect("~/CCCommerce/ThankYou.aspx")
                    Else
                        'context.Response.Write("Line 45<br>")
                    End If
                Else
                    Context.Response.Write("Order ID not returned<br>")
                End If
            Else
                Context.Response.Write("We're sorry, the transaction was not completed, an error occured in the response from the credit card gateway.")
                'context.Response.Write("Line 50<br>")
                For Each key In Context.Request.Form.Keys
                    Context.Response.Write(key.ToString & ":" & Context.Request.Item(key.ToString) & "<br>")
                Next
            End If

            'context.Response.Write("We're sorry, the transaction was not completed, an error occured in the response from the credit card gateway<br>")
            'context.Response.Write("Line 56<br>")

            For Each key In Context.Request.Form.Keys
                Context.Response.Write(key.ToString & ":" & Context.Request.Item(key.ToString) & "<br>")
            Next

        End Sub

        Public Function PostToBeanStream(ByVal AuthorizationCode As String, ByVal OrderId As String) As String
            Dim CurrentRequestUrl As String = String.Format("https://{0}/scripts/process_transaction_auth.asp", Settings.BSHostAddress) 'TERM_URL
            Dim CurrentRequest As HttpWebRequest = CType(WebRequest.Create(CurrentRequestUrl), HttpWebRequest)

            Dim CurrentBuffer As Byte() = Encoding.UTF8.GetBytes("PaRes=" & AuthorizationCode & "&MD=" & OrderId)
            Dim CurrentProxy As String = Nothing

            CurrentRequest.Method = "POST"
            CurrentRequest.ContentType = "application/x-www-form-urlencoded"
            CurrentRequest.ContentLength = CurrentBuffer.Length
            CurrentRequest.Proxy = New WebProxy(CurrentProxy, True)

            Dim CurrentRequestStream As Stream = CurrentRequest.GetRequestStream
            CurrentRequestStream.Write(CurrentBuffer, 0, CurrentBuffer.Length)
            CurrentRequestStream.Flush()
            CurrentRequestStream.Close()

            Dim CurrentResponse As HttpWebResponse = CType(CurrentRequest.GetResponse(), HttpWebResponse)

            Dim CurrentResponseStream As Stream = CurrentResponse.GetResponseStream()
            Dim CurrentStreamReader As StreamReader = New StreamReader(CurrentResponseStream)

            Return CurrentStreamReader.ReadToEnd()
        End Function

        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
End NameSpace