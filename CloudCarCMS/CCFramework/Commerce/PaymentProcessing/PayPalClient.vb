Imports System.Text
Imports System.Web

'TODO Implement paypal processing into this solution
Imports CloudCar.CCFramework.Core

Namespace CCFramework.Commerce.PaymentProcessing

    Public Class PayPalClient
        Private Const _ReleaseUrl As String = "https://www.paypal.com/cgi-bin/webscr?"
        Private Const _TestUrl As String = "https://www.sandbox.paypal.com/us/cgi-bin/webscr?"

        Public LastResponse As String = ""

        Public BuyerEmail As String = ""        'The Buyers Email Address
        Public AccountEmail As String = ""      'The Email address attached to the PayPal account recieving money orders for the site
        Public Amount As Decimal = 0            'The Total amount to be charged to the buyer
        Public LogoUrl As String = ""           'The URL of the company logo for the current web store.
        Public ItemName As String = ""          '
        Public InvoiceNo As String = ""         'Must be unique for every transaction. No obligatory.
        Public SuccessUrl As String = ""        'The url the user will be redirected when the payment is a success.
        Public CancelUrl As String = ""         'The url the user will be redirected when the payment is cancelled.
        Public FirstName As String = ""         'Max 32 Chars
        Public LastName As String = ""          'Max 32 Chars
        Public PostalCode As String = ""        'Max 32 Chars
        Public City As String = ""              'Max 40 chars
        Public Country As String = ""           'Max 2 Digit Country Code
        Public State As String = ""             'Max 2 Digit State/Province Code
        Public Address1 As String = ""          'Max 100 Digit Address
        Public Address2 As String = ""          'Max 100 Digit Address

        'Remember in order to test IPN with the debugger you will need to test
        'the application on a publicly accessible IP address so PayPal can find
        'your machine.

        'You need to configure PayPal by:

        '* Login to PayPal
        '* Go to your Profile
        '* Instant Payment Notification
        '* Set the checkbox and set the URL to your local server/ip for this page
        '* When you go live remember to change this URL to your live site
        '* Only one IPN address per PayPal account is allowed.

        Public Function ProcessRequest() As Boolean
            Dim CurrentRequest As HttpRequest = HttpContext.Current.Request

            LastResponse = ""

            '// *** Make sure our payment goes back to our own account
            Dim CurrentReceiverEmail As String = CurrentRequest.Form("receiver_email")
            If CurrentReceiverEmail = Nothing OrElse Not CurrentReceiverEmail.Trim().ToLower() = AccountEmail.ToLower() Then
                LastResponse = "Invalid receiver email"

                Return False
            End If

            Dim CurrentHttpWebClient As Net.WebClient = New Net.WebClient()
            CurrentHttpWebClient.Headers.Add("cmd, _notify-validate")

            For Each CurrentPostKey As String In CurrentRequest.Form
                CurrentHttpWebClient.Headers.Add(CurrentPostKey & ", " & CurrentRequest.Form(CurrentPostKey))
            Next

            '// *** Retrieve the HTTP result to a string
            Dim CurrentRequestedHtml() As Byte

            If Settings.TestMode Then
                CurrentRequestedHtml = CurrentHttpWebClient.DownloadData(_TestUrl)
            Else
                CurrentRequestedHtml = CurrentHttpWebClient.DownloadData(_ReleaseUrl)
            End If


            Dim CurrentUtfEncoding As New UTF8Encoding()
            LastResponse = CurrentUtfEncoding.GetString(CurrentRequestedHtml)

            If LastResponse = "VERIFIED" Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function GetSubmitUrl() As String
            Dim CurrentUrl As StringBuilder = New StringBuilder()

            If Settings.TestMode Then
                CurrentUrl.AppendFormat("{0}cmd=_xclick&business={1}", _TestUrl, HttpUtility.UrlEncode(AccountEmail))
            Else
                CurrentUrl.AppendFormat("{0}cmd=_xclick&business={1}", _ReleaseUrl, HttpUtility.UrlEncode(AccountEmail))
            End If

            If Not BuyerEmail = Nothing And Not BuyerEmail = "" Then
                CurrentUrl.AppendFormat("&email={0}", HttpUtility.UrlEncode(BuyerEmail))
            End If

            If Not Amount = 0 Then
                CurrentUrl.AppendFormat("&amount={0:f2}", Amount)
            End If

            If Not LogoUrl = Nothing And Not LogoUrl = "" Then
                CurrentUrl.AppendFormat("&image_url={0}", HttpUtility.UrlEncode(LogoUrl))
            End If

            If Not ItemName = Nothing And Not ItemName = "" Then
                CurrentUrl.AppendFormat("&item_name={0}", HttpUtility.UrlEncode(ItemName))
            End If

            If Not FirstName = Nothing And Not FirstName = "" Then
                CurrentUrl.AppendFormat("&first_name={0}", HttpUtility.UrlEncode(FirstName))
            End If

            If Not LastName = Nothing And Not LastName = "" Then
                CurrentUrl.AppendFormat("&last_name={0}", HttpUtility.UrlEncode(LastName))
            End If

            If Not Address1 = Nothing And Not Address1 = "" Then
                CurrentUrl.AppendFormat("&address1={0}", HttpUtility.UrlEncode(Address1))
            End If

            If Not Address2 = Nothing And Not Address2 = "" Then
                CurrentUrl.AppendFormat("&address2={0}", HttpUtility.UrlEncode(Address2))
            End If

            If Not Country = Nothing And Not Country = "" Then
                CurrentUrl.AppendFormat("&country={0}", HttpUtility.UrlEncode(Country))
            End If

            If Not City = Nothing And Not City = "" Then
                CurrentUrl.AppendFormat("&city={0}", HttpUtility.UrlEncode(City))
            End If

            If Not State = Nothing And Not State = "" Then
                CurrentUrl.AppendFormat("&state={0}", HttpUtility.UrlEncode(State))
            End If

            If Not PostalCode = Nothing And Not PostalCode = "" Then
                CurrentUrl.AppendFormat("&zip={0}", HttpUtility.UrlEncode(PostalCode))
            End If

            If Not InvoiceNo = Nothing And Not InvoiceNo = "" Then
                CurrentUrl.AppendFormat("&invoice={0}", HttpUtility.UrlEncode(InvoiceNo))
            End If

            If Not SuccessUrl = Nothing And Not SuccessUrl = "" Then
                CurrentUrl.AppendFormat("&return={0}", HttpUtility.UrlEncode(SuccessUrl))
            End If

            If Not CancelUrl = Nothing And Not CancelUrl = "" Then
                CurrentUrl.AppendFormat("&cancel_return={0}", HttpUtility.UrlEncode(CancelUrl))
            End If

            Return CurrentUrl.ToString()

        End Function

    End Class

End Namespace