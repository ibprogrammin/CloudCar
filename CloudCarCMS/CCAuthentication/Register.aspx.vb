Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Core

Namespace CCAuthentication
    Partial Public Class Register
        Inherits RoutablePage

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                If Membership.GetUser() Is Nothing Then
                    RefreshDataSources()
                Else
                    Response.RedirectToRoute("RouteProfile")
                End If
            End If
        End Sub

        Private Sub RefreshDataSources()
            ddlCountry.Items.Clear()
            ddlCountry.Items.Add(New ListItem("Select", ""))
            ddlCountry.AppendDataBoundItems = True
            ddlCountry.DataSource = New CountryController().GetElements
            ddlCountry.DataBind()

            ddlCountry.SelectedValue = Settings.DefaultCountryID.ToString

            ddlProvince.Items.Clear()
            ddlProvince.Items.Add(New ListItem("Select", ""))
            ddlProvince.AppendDataBoundItems = True
            ddlProvince.DataSource = New ProvinceController().GetCountryProvince(Settings.DefaultCountryID)
            ddlProvince.DataBind()
        End Sub

        Private Sub RegisterButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnRegister.Click
            Dim username As String = UserNameTextBox.Text
            Dim email As String = txtEmail.Text
            Dim password As String = txtPassword.Text
            Dim pwquestion As String

            If PasswordQuestionDropDown.SelectedValue = "Other" Then
                pwquestion = txtPWQuestion.Text
            Else
                pwquestion = PasswordQuestionDropDown.SelectedValue
            End If

            Dim pwanswer As String = txtPWAnswer.Text
            
            Dim fname As String = txtFirstName.Text
            Dim mname As String = "" 'txtMiddleName.Text
            Dim lname As String = txtLastName.Text
            Dim address As String = txtAddress.Text
            Dim city As String = txtCity.Text
            Dim province As Integer
            Dim country As Integer
            Dim pcode As String = txtPC.Text
            Dim phone As String = txtPhone.Text
            Dim bd As DateTime

            If Not DateTime.TryParse(txtBirthDate.Text, bd) Then
                bd = DateTime.Today
            End If

            If ddlCountry.SelectedValue = Nothing Then
                country = 3
            Else
                country = Integer.Parse(ddlCountry.SelectedValue)
            End If

            If ddlProvince.SelectedValue = Nothing Then
                province = 14
            Else
                province = Integer.Parse(ddlProvince.SelectedValue)
            End If

            Try
                Dim CurrentSimpleUser As Integer = New SimpleUserController().Create(fname, mname, lname, email, phone)

                Dim AddressId As Integer = New AddressController().Create(address, city, pcode, province, country)

                Dim CurrentRegisteredUser As Integer = New RegisteredUserController().Create(CurrentSimpleUser, AddressId, bd, "", "", username, EPriceLevel.Regular)

                Dim CurrentStatus As New MembershipCreateStatus()

                Membership.CreateUser(username, password, email, pwquestion, pwanswer, True, CurrentStatus)
                Roles.AddUserToRole(username, "Regular")

                FormsAuthentication.SetAuthCookie(username, False)

                If Settings.SendNoticeEmails Then
                    SendEmails.SendNewRegistrationNotice(username, fname, lname)
                End If

                lblStatus.Visible = True
                lblStatus.Text = "Thank your for joining us! Your user account has been saved and your are now logged in. " _
                                 & "Click <a href=""/"">here</a> to return to the home page."

                lblStatus.ForeColor = Drawing.Color.DeepSkyBlue

                StartMessage.Visible = False
                FormPlaceHolder.Visible = False
            Catch Ex As Exception
                lblStatus.Visible = True
                lblStatus.Text = "An error occurred creating your user account!"
            End Try
        End Sub

        Private Sub MessagePopupClickedOk() Handles mpMessage.ClickedOK
            Response.RedirectToRoute("RouteHomePageB")
        End Sub

        Private Sub CountryDropDownSelectedIndexChanged(ByVal Sender As Object, ByVal Args As EventArgs) Handles ddlCountry.SelectedIndexChanged
            If Not ddlCountry.SelectedValue = "" Then
                Dim CountryId As Integer = Integer.Parse(ddlCountry.SelectedValue)

                ddlProvince.Items.Clear()
                ddlProvince.Items.Add(New ListItem("Province / State", ""))
                ddlProvince.Items.Add(New ListItem("----------", ""))
                ddlProvince.AppendDataBoundItems = True
                ddlProvince.DataSource = New ProvinceController().GetCountryProvince(CountryId)
                ddlProvince.DataBind()

                ddlProvince.Focus()
            End If
        End Sub

        Private Sub PasswordQuestionDropDownSelectedIndexChanged(Sender As Object, Args As EventArgs) Handles PasswordQuestionDropDown.SelectedIndexChanged
            If PasswordQuestionDropDown.SelectedValue = "Other" Then
                txtPWQuestion.Visible = True
                txtPWQuestion.Focus()
            Else
                txtPWQuestion.Visible = False
            End If
        End Sub

    End Class

End Namespace