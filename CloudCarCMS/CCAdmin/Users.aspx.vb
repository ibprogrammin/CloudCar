Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCAdmin

    Partial Public Class Users
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                If Not Request.QueryString("User") Is Nothing Then
                    Dim userID As Integer = CInt(Request.QueryString("User"))
                    Dim user As RegisteredUser = New CCFramework.Core.RegisteredUserController().GetByUserID(userID)

                    LoadUser(user.UserName)
                Else
                    phUserList.Visible = True
                End If
            End If
        End Sub

        Private Sub RefreshDataSources()
            ddlCountry.Items.Clear()
            ddlCountry.Items.Add(New ListItem("Country", ""))
            ddlCountry.AppendDataBoundItems = True
            ddlCountry.DataSource = New CountryController().GetElements
            ddlCountry.DataBind()

            ddlCountry.SelectedValue = CCFramework.Core.Settings.DefaultCountryID.ToString

            ddlProvince.Items.Clear()
            ddlProvince.Items.Add(New ListItem("Province", ""))
            ddlProvince.AppendDataBoundItems = True
            ddlProvince.DataSource = New ProvinceController().GetCountryProvince(CCFramework.Core.Settings.DefaultCountryID)
            ddlProvince.DataBind()
        End Sub

        Protected Sub AdminCheckBoxCheckChanged(ByVal Sender1 As Object, ByVal Args As EventArgs)
            If Not Roles.RoleExists("Administrator") Then
                Roles.CreateRole("Administrator")
            End If

            Dim CurrentCheckBox As CheckBox = CType(Sender1, CheckBox)
            Dim CurrentUserName As String = CurrentCheckBox.Attributes("UserName")

            If CurrentCheckBox.Checked Then
                Roles.AddUserToRole(CurrentUserName, "Administrator")
            Else
                If Not Roles.IsUserInRole(CurrentUserName, "Super User") Then
                    Roles.RemoveUserFromRole(CurrentUserName, "Administrator")
                Else
                    lblStatus.Text = String.Format("Ooops! You don't have permission to do that. Sorry!")
                    lblStatus.Visible = True
                End If
            End If

            gvUsers.DataBind()
        End Sub

        Protected Sub AdminCheckBoxDataBinding(ByVal Sender1 As Object, ByVal Args As EventArgs)
            Dim CurrentCheckBox As CheckBox = CType(Sender1, CheckBox)
            Try
                CurrentCheckBox.Checked = Roles.IsUserInRole(CurrentCheckBox.Attributes("UserName"), "Administrator")
            Catch CurrentException As NullReferenceException
                CurrentCheckBox.Checked = False
            End Try
        End Sub

        Protected Sub LockedOutCheckBoxCheckChanged(ByVal Sender1 As Object, ByVal Args As EventArgs)
            Dim CurrentCheckBox As CheckBox = CType(Sender1, CheckBox)
            Dim CurrentUserName As String = CurrentCheckBox.Attributes("UserName")

            If Not CurrentCheckBox.Checked Then
                Membership.GetUser(CurrentUserName).UnlockUser()
            End If

            gvUsers.DataBind()
        End Sub

        Protected Sub LockedOutCheckBoxDataBinding(ByVal Sender1 As Object, ByVal Args As EventArgs)
            Dim CurrentCheckBox As CheckBox = CType(Sender1, CheckBox)
            Dim CurrentUserName As String = CurrentCheckBox.Attributes("UserName")
            Dim IsLockedOut As Boolean

            Try
                IsLockedOut = Membership.GetUser(CurrentUserName).IsLockedOut
            Catch CurrentException As Exception
                IsLockedOut = True
            End Try

            If IsLockedOut Then
                Sender1.Checked = True
            Else
                Sender1.Checked = False
                Sender1.Enabled = False
            End If
        End Sub

        Protected Sub EnableUserCheckBoxDataBinding(ByVal Sender1 As Object, ByVal Args As EventArgs)
            Try
                Sender1.Checked = Membership.GetUser(Sender1.Attributes("UserName").ToString).IsApproved
            Catch CurrentException As NullReferenceException
                Sender1.Checked = False
            End Try
        End Sub

        Protected Sub EnableUserCheckBoxChecked(ByVal Sender1 As Object, ByVal Args As EventArgs)
            Dim CurrentCheckBox As CheckBox = CType(Sender1, CheckBox)
            Dim CurrentUserName As String = CurrentCheckBox.Attributes("UserName")

            If Not Roles.IsUserInRole(CurrentUserName, "Super User") Then
                Membership.GetUser(CurrentUserName).IsApproved = CurrentCheckBox.Checked
            Else
                If CurrentCheckBox.Checked Then
                    Membership.GetUser(CurrentUserName).IsApproved = CurrentCheckBox.Checked
                Else
                    lblStatus.Text = String.Format("Ooops! You don't have permission to do that. Sorry!")
                    lblStatus.Visible = True
                End If
            End If

            gvUsers.DataBind()
        End Sub

        Protected Sub PriceLevelDropDownIndexChanged(ByVal Sender1 As Object, ByVal e As EventArgs)
            Dim CurrentDropDown As DropDownList = CType(Sender1, DropDownList)
            Dim CurrentPriceLevel As EPriceLevel = CType(Integer.Parse(CurrentDropDown.SelectedValue), EPriceLevel)

            RegisteredUserController.SetUserPriceLevel(CurrentDropDown.Attributes("UserName"), CurrentPriceLevel)

            gvUsers.DataBind()
        End Sub

        Protected Sub PriceLevelDropDownDataBinding(ByVal Sender1 As Object, ByVal e As EventArgs)
            Try
                Sender1.SelectedValue = RegisteredUserController.GetUserPriceLevel(Sender1.Attributes("UserName"))
            Catch CurrentException As InvalidUserException
                Sender1.SelectedValue = "0"
            End Try
        End Sub

        Private Sub btnAddUser_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddUser.Command
            RefreshDataSources()

            phAddUser.Visible = True
            phUserList.Visible = False
            btnAddUser.Visible = False
        End Sub

        Private Sub btnCancel_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnCancel.Command
            phAddUser.Visible = False
            phUserList.Visible = True
            btnAddUser.Visible = True
        End Sub

        Private Sub btnRegister_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnRegister.Command
            Dim username As String = txtUsername.Text
            Dim password As String = txtPassword.Text
            Dim pwquestion As String = txtPWQuestion.Text
            Dim pwanswer As String = txtPWAnswer.Text
            Dim email As String = txtEmail.Text
            Dim fname As String = txtFirstName.Text
            Dim mname As String = txtMiddleName.Text
            Dim lname As String = txtLastName.Text
            Dim address As String = txtAddress.Text
            Dim city As String = txtCity.Text
            Dim province As Integer = Integer.Parse(ddlProvince.SelectedValue)
            Dim country As Integer = Integer.Parse(ddlCountry.SelectedValue)
            Dim pcode As String = txtPC.Text
            Dim phone As String = txtPhone.Text
            Dim bd As DateTime

            If Not DateTime.TryParse(txtBirthDate.Text, bd) Then
                bd = DateTime.Today
            End If

            Try
                Dim su As Integer = New CCFramework.Core.SimpleUserController().Create(fname, mname, lname, email, phone)

                Dim addressID As Integer = New AddressController().Create(address, city, pcode, province, country)

                Dim ru As Integer = New CCFramework.Core.RegisteredUserController().Create(su, addressID, bd, "", "", username, EPriceLevel.Regular)

                Dim status As New MembershipCreateStatus()

                Membership.CreateUser(username, password, email, pwquestion, pwanswer, True, status)
                Roles.AddUserToRole(username, "Regular")

                'FormsAuthentication.SetAuthCookie(username, False)

                StatusLabel.Text = "The new user profile has been created!"
                StatusLabel.Visible = True
            Catch ex As Exception
                StatusLabel.Text = "An error occurred creating the user: " & ex.Message
                StatusLabel.Visible = True
            End Try

            gvUsers.DataBind()
            phAddUser.Visible = False
            phUserList.Visible = True
            btnAddUser.Visible = True
        End Sub

        Private Sub btnBack_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnBack.Command
            phUserList.Visible = True
            phUserProfile.Visible = False
            btnAddUser.Visible = True
        End Sub

        'Private Sub gvUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvUsers.SelectedIndexChanged
        '    Dim username As String = gvUsers.SelectedItem.FindControl()("UserName").ToString

        '    LoadUser(username)
        'End Sub

        Protected Sub btnDeleteUser_Command(ByVal Sender As Object, ByVal Args As CommandEventArgs)
            Dim username As String = Args.CommandArgument.ToString

            If Not Roles.IsUserInRole(username, "Super User") Then
                Dim message As String = ""

                If CCFramework.Core.RegisteredUserController.DeleteUserProfile(username, message) Then
                    lblStatus.Text = "User: " & username & " has been successfully deleted."
                    lblStatus.Visible = True

                    gvUsers.DataBind()
                Else
                    lblStatus.Text = message ' "There was an error deleting the user."
                    lblStatus.Visible = True
                End If
            Else
                lblStatus.Text = "Uh Oh! You don't have permission to delete this user."
                lblStatus.Visible = True
            End If
        End Sub

        Protected Sub btnResetPassword_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim username As String = e.CommandArgument.ToString
            Dim password As String = ""
            Dim answer As String = CType(CType(sender, LinkButton).Parent.FindControl("txtPwdAnswer"), TextBox).Text

            If CCFramework.Core.RegisteredUserController.ResetUserPassword(username, password, answer) Then
                lblStatus.Text = "User: " & username & "'s password has been successfully reset to ( <b>" & password & "</b> ). They will be notified, by email, of their new password."
                lblStatus.Visible = True

                If CCFramework.Core.Settings.SendNoticeEmails Then
                    'TODO add a link to change your password form.
                    Dim resetMessage As String = "This is an automated message from " & CCFramework.Core.Settings.CompanyName & " to inform you that your password has been reset. <br /><br />"
                    resetMessage &= "You new password is <b>" & password & "</b><br /><br />"
                    resetMessage &= "Thank you for visiting our site."

                    CCFramework.Core.SendEmails.Send(New Net.Mail.MailAddress(Membership.GetUser(username).Email), CCFramework.Core.Settings.CompanyName & " - Your password has been reset.", resetMessage)
                End If
            Else
                lblStatus.Text = "There was an error reseting the users password. " & password
                lblStatus.Visible = True
            End If
        End Sub

        Protected Sub lbSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim username As String = CType(sender, LinkButton).Text 'gvUsers.SelectedDataKey("UserName").ToString

            LoadUser(username)
        End Sub

        Private Sub LoadUser(ByVal username As String)
            Dim user As RegisteredUser = New CCFramework.Core.RegisteredUserController().GetByUserName(username) ' cUserProfile.GetUserProfile(username)
            Dim simpleUser As SimpleUser = New CCFramework.Core.SimpleUserController().GetElement(user.UserID)
            Dim address As Address = New AddressController().GetElement(user.AddressID)
            Dim province As Province = New ProvinceController().GetElement(address.ProvStateID)
            Dim country As Country = New CountryController().GetElement(province.CountryID)

            With user
                lblUserName.Text = .UserName
                lblEmail.Text = Membership.GetUser(.UserName).Email
                lblBirthDate.Text = CType(.BirthDate, Date).ToLongDateString
            End With

            With simpleUser
                lblName.Text = .LastName & ", " & .FirstName & " " & .MiddleName
                lblPhone.Text = .PhoneNumber
            End With

            With address
                lblAddress.Text = .Address & " " & .City & " " & province.Name & ", " & country.Name & " " & .PCZIP
            End With

            phUserList.Visible = False
            phUserProfile.Visible = True
            btnAddUser.Visible = False
        End Sub

        Private Sub ddlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCountry.SelectedIndexChanged
            If Not ddlCountry.SelectedValue = "" Then
                Dim countryID As Integer = Integer.Parse(ddlCountry.SelectedValue)

                ddlProvince.DataSource = New ProvinceController().GetCountryProvince(countryID)
                ddlProvince.DataBind()

                ddlProvince.Focus()
            End If
        End Sub

        Private Sub gvUsers_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles gvUsers.PageIndexChanged
            gvUsers.CurrentPageIndex = e.NewPageIndex
            gvUsers.DataBind()
        End Sub

    End Class
End Namespace