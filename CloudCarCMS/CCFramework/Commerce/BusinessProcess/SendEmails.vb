Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Core

    Public Class SendEmails

        Public Shared Function GetEmail(ByVal Context As HttpContext, ByVal PageURL As String) As String
            Dim sw As New System.IO.StringWriter

            Context.Server.Execute(PageURL, sw)

            GetEmail = sw.ToString
        End Function

        Public Shared Sub Send(ByVal ToAddress As Net.Mail.MailAddress, ByVal Subject As String, ByVal Body As String, Optional ByVal Attachments As List(Of Net.Mail.Attachment) = Nothing)
            Dim CurrentMailMessage As New Net.Mail.MailMessage

            CurrentMailMessage.IsBodyHtml = True
            CurrentMailMessage.Body = Body
            CurrentMailMessage.From = New Net.Mail.MailAddress(Settings.MailFromAddress, Settings.CompanyName)
            CurrentMailMessage.Subject = Subject
            CurrentMailMessage.To.Add(ToAddress)

            If Not Attachments Is Nothing Then
                For Each item In Attachments
                    CurrentMailMessage.Attachments.Add(item)
                Next
            End If

            Dim Smtp As New Net.Mail.SmtpClient

            Smtp.Host = Settings.SMTPHost
            Smtp.Port = Settings.SMTPPort
            Smtp.EnableSsl = Settings.EnableEmailSSL
            Smtp.UseDefaultCredentials = False
            Smtp.Credentials = New Net.NetworkCredential(Settings.SMTPUser, Settings.SMTPPass)

            Smtp.Send(CurrentMailMessage)
        End Sub

        Public Shared Sub SendEmailInvoice(ByVal OrderId As Integer, ByVal ToEmailAddress As String, ByVal CCAdmin As Boolean, ByVal Context As HttpContext)
            If Settings.SendNoticeEmails Then
                Dim email As String = SendEmails.GetEmail(Context, "~/CCCommerce/Email/NewOrderInvoice.aspx?Order=" & OrderId.ToString & "&Email=" & ToEmailAddress)

                SendEmails.Send(New Net.Mail.MailAddress(ToEmailAddress), "Thank you for your purchase - " & Settings.CompanyName, email)

                If CCAdmin Then
                    SendEmails.Send(New Net.Mail.MailAddress(Settings.AdminEmail, Settings.AdminName), "New Order", email)
                End If

                If Settings.NotifyDistributors Then
                    Dim db As New CommerceDataContext
                    Dim Prefix As String = OrderController.GetOrderPrefixFunc(db, OrderID)
                    Dim distributorId As Integer = Commerce.FixedShippingZoneController.GetShippingZoneDistributor(Prefix)
                    Dim user As SimpleUser = SimpleUserController.GetSimpleUserByIdFunc(db, distributorId)

                    Dim distributorEmail As String = SendEmails.GetEmail(Context, "~/CCCommerce/Email/DistributorOrderInvoice.aspx?Order=" & OrderId.ToString & "&Distributor=" & distributorId)

                    Try
                        SendEmails.Send(New Net.Mail.MailAddress(user.Email, "Distributor"), "There is a new order awaiting shipment - " & Settings.CompanyName, distributorEmail)
                    Catch ex As Exception
                        SendEmails.Send(New Net.Mail.MailAddress(Settings.AdminEmail, Settings.AdminName), Settings.CompanyName & " - There was an error sending email to distributor.", distributorEmail)
                    End Try

                    If Settings.NotifyShippingCompany Then
                        SendEmails.Send(New Net.Mail.MailAddress(Settings.ShippingCompanyEmail), "There is a new order awaiting shipment - " & Settings.CompanyName, distributorEmail)
                    End If
                End If
            End If
        End Sub

        Public Shared Sub SendDistributorInvoice(ByVal OrderId As Integer, ByVal Context As HttpContext)
            Dim db As New CommerceDataContext
            Dim Prefix As String = OrderController.GetOrderPrefixFunc(db, OrderID)
            Dim distributorId As Integer = FixedShippingZoneController.GetShippingZoneDistributor(Prefix)
            Dim user As SimpleUser = SimpleUserController.GetSimpleUserByIdFunc(db, distributorId)

            Dim distributorEmail As String = SendEmails.GetEmail(Context, "~/CCCommerce/Email/DistributorOrderInvoice.aspx?Order=" & OrderId.ToString & "&Distributor=" & distributorId)

            Try
                SendEmails.Send(New Net.Mail.MailAddress(user.Email, "Distributor"), "There is a new order awaiting shipment - " & Settings.CompanyName, distributorEmail)
            Catch ex As Exception
                SendEmails.Send(New Net.Mail.MailAddress(Settings.AdminEmail, Settings.AdminName), Settings.CompanyName & " - There was an error sending email to distributor.", distributorEmail)
            End Try
        End Sub

        Public Shared Sub SendUserInvoice(ByVal OrderID As Integer, ByVal ToEmailAddress As String, ByVal Context As HttpContext)
            Dim email As String = SendEmails.GetEmail(Context, "~/CCCommerce/Email/NewOrderInvoice.aspx?Order=" & OrderID.ToString & "&Email=" & ToEmailAddress)

            SendEmails.Send(New Net.Mail.MailAddress(ToEmailAddress), "Thank you for your purchase - " & Settings.CompanyName, email)
        End Sub

        Public Shared Sub SendNewRegistrationNotice(ByVal Username As String, ByVal FirstName As String, ByVal LastName As String)
            Dim AdminEmailAddress As New Net.Mail.MailAddress(Settings.AdminEmail)
            Dim Subject As String = "A New User Has Registered On Your Web Site"
            Dim Body As New StringBuilder

            Body.Append("The following user has just registered an account on your web site: " & vbNewLine)
            Body.Append(String.Format("Username: {0}" & vbNewLine, Username))
            Body.Append(String.Format("First Name: {0}" & vbNewLine, FirstName))
            Body.Append(String.Format("Last Name: {0}" & vbNewLine, LastName))

            Send(AdminEmailAddress, Subject, Body.ToString)
        End Sub

        Public Shared Sub SendNoticeMessage(ByVal Address As String, ByVal Subject As String, ByVal Message As String)
            Dim AdminEmailAddress As New Net.Mail.MailAddress(Address)

            Send(AdminEmailAddress, Subject, Message)
        End Sub

    End Class

End Namespace
