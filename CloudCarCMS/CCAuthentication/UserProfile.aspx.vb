Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Core

Namespace CCAuthentication

    Partial Public Class UserProfile
        Inherits RoutablePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack And HttpContext.Current.User.Identity.IsAuthenticated Then
                phUserProfile.Visible = True
                SelectProfile()
            Else
                phMessage.Visible = True
            End If
        End Sub

        Private Sub RefreshDataSources()
            ddlCountry.Items.Clear()
            ddlCountry.Items.Add(New ListItem("Country", ""))
            ddlCountry.Items.Add(New ListItem("----------", ""))
            ddlCountry.AppendDataBoundItems = True
            ddlCountry.DataSource = New CountryController().GetElements
            ddlCountry.DataBind()

            'ddlProvince.DataSource = New Commerce.cProvince().GetCountryProvince(DefaultCountryID)
            'ddlProvince.DataBind()
        End Sub

        Protected Sub SelectProfile()
            Dim username As String = Membership.GetUser.UserName

            Dim registeredUser As RegisteredUser = New RegisteredUserController().GetByUserName(username)
            Dim simpleUser As SimpleUser = New SimpleUserController().GetElement(registeredUser.UserID)
            Dim address As Address = New AddressController().GetElement(registeredUser.AddressID)
            Dim province As Province = New ProvinceController().GetElement(address.ProvStateID)

            With simpleUser
                txtFirstName.Text = .FirstName
                txtMiddleName.Text = .MiddleName
                txtLastName.Text = .LastName
                txtPhone.Text = .PhoneNumber
            End With

            With address
                txtAddress.Text = .Address
                txtCity.Text = .City

                RefreshDataSources()
                LoadProvinces(.Province.CountryID)

                ddlCountry.SelectedValue = .Province.CountryID.ToString
                ddlProvince.SelectedValue = .ProvStateID.ToString
                txtPC.Text = .PCZIP
            End With

            With registeredUser
                txtUsername.Text = .UserName
                txtEmail.Text = Membership.GetUser(.UserName).Email
                txtBirthDate.Text = .BirthDate.Value.ToString
                'lblBirthDate.Text = .BirthDate.ToLongDateString
            End With

            If OrderController.DistributorHasOrder(registeredUser.UserID) Then
                pnlDistributor.Visible = True
            End If

            LoadDownloadableProducts(simpleUser.ID)

            registeredUser = Nothing
            simpleUser = Nothing
            address = Nothing
            province = Nothing
        End Sub

        Private Sub UpdateButtonCommand(ByVal Sender As Object, ByVal E As CommandEventArgs) Handles btnUpdate.Command
            Dim UserName As String = Membership.GetUser.UserName

            Dim CurrentRegisteredUser As RegisteredUser = New RegisteredUserController().GetByUserName(UserName)
            Dim CurrentSimpleUser As SimpleUser = New SimpleUserController().GetElement(CurrentRegisteredUser.UserID)
            Dim CurrentAddress As Address = New AddressController().GetElement(CurrentRegisteredUser.AddressID)
            Dim CurrentProvince As Province = New ProvinceController().GetElement(CurrentAddress.ProvStateID)

            'Dim password As String = txtPassword.Text
            'Dim pwquestion As String = txtPWQuestion.Text
            'Dim pwanswer As String = txtPWAnswer.Text
            Dim NewEmail As String = txtEmail.Text
            Dim NewFirstName As String = txtFirstName.Text
            Dim NewMiddleName As String = txtMiddleName.Text
            Dim NewLastName As String = txtLastName.Text
            Dim NewStreet As String = txtAddress.Text
            Dim NewCity As String = txtCity.Text
            Dim NewProvinceId As Integer = Integer.Parse(ddlProvince.SelectedValue)
            Dim NewCountryId As Integer = Integer.Parse(ddlCountry.SelectedValue)
            Dim NewPostalCode As String = txtPC.Text
            Dim NewPhone As String = txtPhone.Text
            Dim NewBirthDate As DateTime = DateTime.Parse(txtBirthDate.Text)

            Try
                Dim SimpleUserUpdated As Boolean = New SimpleUserController().Update(CurrentRegisteredUser.UserID, NewFirstName, NewMiddleName, NewLastName, NewEmail, NewPhone)

                Dim CurrentAddressId As Integer = CurrentAddress.ID

                If Not CurrentAddress.Address = NewStreet Or Not CurrentAddress.City = NewCity Or Not CurrentAddress.PCZIP = NewPostalCode Or Not CurrentAddress.ProvStateID = NewProvinceId Or Not CurrentProvince.CountryID = NewCountryId Then
                    Dim CurrentAddressController As New AddressController()

                    If AddressController.IsAddressSafeToUpdate(CurrentAddress.ID) Then
                        CurrentAddressController.Update(CurrentAddress.ID, NewStreet, NewCity, NewPostalCode, NewProvinceId, NewCountryId)
                    Else
                        CurrentAddressId = CurrentAddressController.Create(NewStreet, NewCity, NewPostalCode, NewProvinceId, NewCountryId)
                    End If
                End If

                Dim RegisteredUserUpdated As Boolean = New RegisteredUserController().Update(CurrentRegisteredUser.ID, CurrentRegisteredUser.UserID, CurrentAddressId, NewBirthDate, "", "", UserName, EPriceLevel.Regular)

                'Dim status As New MembershipCreateStatus()

                Dim CurrentMembershipUser As MembershipUser = Membership.GetUser()
                CurrentMembershipUser.Email = NewEmail
                Membership.UpdateUser(CurrentMembershipUser)

                'Membership.UpdateUser(username, password, email, pwquestion, pwanswer, True, status)
                'Roles.AddUserToRole(username, "Regular")

                FormsAuthentication.SetAuthCookie(UserName, False)

                lblStatus.Visible = True
                lblStatus.Text = "User Saved and Logged In"
            Catch Ex As Exception
                lblStatus.Visible = True
                lblStatus.Text = "An error occurred modfiying your user profile: " & Ex.Message
            End Try
        End Sub

        Private Sub ddlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCountry.SelectedIndexChanged
            If Not ddlCountry.SelectedValue = "" Then
                Dim countryID As Integer = Integer.Parse(ddlCountry.SelectedValue)

                LoadProvinces(countryID)

                ddlProvince.Focus()
            End If
        End Sub

        Private Sub LoadProvinces(ByVal CountryID As Integer)
            ddlProvince.Items.Clear()
            ddlProvince.Items.Add(New ListItem("Province", ""))
            ddlProvince.Items.Add(New ListItem("----------", ""))
            ddlProvince.AppendDataBoundItems = True
            ddlProvince.DataSource = New ProvinceController().GetCountryProvince(CountryID)
            ddlProvince.DataBind()
        End Sub

        Private Sub LoadDownloadableProducts(ByVal userID As Integer)
            Dim db As New CommerceDataContext

            Dim downloadableProducts As List(Of ProductDownloadDetail) = ProductDownloadController.GetProductDownloadDetailsByUserID(db, userID).ToList
            If downloadableProducts.Count > 0 Then
                phAvailableDownloads.Visible = True
                gvDownloadProducts.DataSource = downloadableProducts
                gvDownloadProducts.DataBind()
            End If
        End Sub

    End Class
End Namespace