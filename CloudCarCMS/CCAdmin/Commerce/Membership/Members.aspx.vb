Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce.Membership

    Partial Public Class Members
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub gvUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvUsers.SelectedIndexChanged
            Dim username As String = gvUsers.SelectedDataKey("UserName").ToString

            LoadUser(username)
        End Sub

        Protected Sub gvUsers_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUsers.RowCommand
            If e.CommandName = "Suspend" Then
                Dim userID As Integer = Integer.Parse(e.CommandArgument)
                Dim user As RegisteredUser = New CCFramework.Core.RegisteredUserController().GetByUserID(userID)

                Try
                    Dim db As New CommerceDataContext

                    Dim membership = From m In db.CurrentMemberships Where m.UserID = userID Select m

                    lblStatus.Text = "User: " & user.UserName & "'s membership has been suspended. They will be notified, by email, of the suspension."
                    lblStatus.Visible = True

                    If CCFramework.Core.Settings.SendNoticeEmails Then
                        Dim resetMessage As String = "This is an automated message from " & CCFramework.Core.Settings.CompanyName & " to inform you that your current membership has been suspended. <br /><br />"
                        resetMessage &= "Please contact us to reinstate your membership. <br /><br />"
                        resetMessage &= "Thank you for visiting our site."

                        CCFramework.Core.SendEmails.Send(New Net.Mail.MailAddress(user.SimpleUser.Email), CCFramework.Core.Settings.CompanyName & " - Your current membership has been suspended.", resetMessage)
                    End If
                Catch ex As Exception
                    lblStatus.Text = "There was an error suspending the membership."
                    lblStatus.Visible = True
                End Try
            End If
        End Sub

        Protected Sub lbSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim username As String = CType(sender, LinkButton).Text 'gvUsers.SelectedDataKey("UserName").ToString

            LoadUser(username)
        End Sub

        Private Sub LoadUser(ByVal username As String)
            'Dim user As RegisteredUser = New Commerce.cRegisteredUser().GetByUserName(username) ' cUserProfile.GetUserProfile(username)
            'Dim simpleUser As SimpleUser = New Commerce.cSimpleUser().GetElement(user.UserID)
            'Dim address As Address = New Commerce.cAddress().GetElement(user.AddressID)
            'Dim province As Province = New Commerce.cProvince().GetElement(address.ProvStateID)
            'Dim country As Country = New Commerce.cCountry().GetElement(province.CountryID)

            'With user
            '    lblUserName.Text = .UserName
            '    lblEmail.Text = Membership.GetUser(.UserName).Email
            '    lblBirthDate.Text = .BirthDate.ToLongDateString
            '    lblCellPhone.Text = .CellPN
            '    lblWorkPhone.Text = .WorkPN
            '    lblContactMethod.Text = .ContactMethod
            '    lblECName.Text = .ECName
            '    lblECPhone.Text = .ECNumber
            '    lblECRelation.Text = .ECRelation
            '    lblMedicalConditions.Text = .MedicalConditions
            '    lblGender.Text = .Sex
            '    lblHeight.Text = .Height.ToString
            '    lblWeight.Text = .Weight.ToString

            '    lblPMAT.Text = .PreviousTraining
            '    lblHFOC.Text = .howdidyoufindus
            '    lblRFL.Text = .reasonforlearning
            '    lblWBJJ.Text = .reasonforjoining
            '    lblWYR.Text = .wereyourefered
            'End With

            'With simpleUser
            '    lblName.Text = .LastName & ", " & .FirstName & " " & .MiddleName
            '    lblHousePhone.Text = .PhoneNumber
            'End With

            'With address
            '    lblAddress.Text = .Address & " " & .City & " " & province.Name & ", " & country.Name & " " & .PCZIP
            'End With

            'gvUsers.Visible = False
            'phUserProfile.Visible = True
            'btnAddUser.Visible = False
        End Sub

        Private Sub RefreshDataSources()
            Dim db As New CommerceDataContext

            'Dim ordersWithMembers = From oi In db.OrderItems _
            '                        Join p In db.Products On p.ID Equals oi.ProductID _
            '                        Join o In db.Orders On oi.OrderID Equals o.ID _
            '                        Where p.Membership = True And o.ApprovalState = Commerce.eApprovalState.Approved _
            '                        Select New With {.OrderID = oi.OrderID, .ProductId = p.ID, .UserID = o.UserID, .Date = o.OrderDate} _
            '                        Distinct

            Dim db2 As New ContentDataContext

            Dim members = From cm In db.CurrentMemberships _
                    Join ru In db.RegisteredUsers On ru.UserID Equals cm.UserID _
                    Select New With {.UserId = ru.UserID, .JoinDate = cm.JoinDate, .UserName = ru.UserName, _
                    .BillFrequency = cm.BillFrequency, .LastBillDate = GetLastBillDate(cm.JoinDate, 3), .NextBillDate = GetNextBillDate(cm.JoinDate, 3)}

            gvUsers.DataSource = members
            gvUsers.DataBind()
        End Sub

        Private Function GetLastBillDate(ByVal JoinDate As Date, ByVal Frequency As Integer) As Date
            Dim LastDate As Date = GetNextBillDate(JoinDate, Frequency)

            Return LastDate.AddMonths(-3)
        End Function

        Private Function GetNextBillDate(ByVal JoinDate As Date, ByVal Frequency As Integer) As Date
            Dim currentDate As Date = DateTime.Now
            Dim cycleDate As Date = JoinDate

            While cycleDate < currentDate
                cycleDate.AddMonths(Frequency)
            End While

            Return cycleDate
        End Function

    End Class
End Namespace