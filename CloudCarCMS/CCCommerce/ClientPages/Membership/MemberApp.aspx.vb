Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCCommerce.Membership

    Partial Public Class MemberApp
        Inherits Page

        'TODO: If user is already logged in then just have them verify their credentials. If they are not logged in also give them an option to log in.

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                If Not Request.QueryString("Product") Is Nothing Then
                    ProductID = Integer.Parse(Request.QueryString("Product"))
                    If UserIsRegistered(UserID) Then
                        Response.Redirect("~/CCCommerce/Membership/Contract.aspx?Product=" & ProductID & "&User=" & UserID)
                    Else
                        RefreshDataSources()
                    End If
                Else
                    Response.Redirect("~/CCCommerce/Categories.aspx")
                End If
            End If
        End Sub

        Public Property UserID() As Integer
            Get
                If Session("UserID") Is Nothing Then
                    Session.Add("UserID", Nothing)
                End If

                Return CInt(Session("UserID"))
            End Get
            Set(ByVal value As Integer)
                If Not Session("UserID") Is Nothing Then
                    Session("UserID") = value
                Else
                    Session.Add("UserID", value)
                End If
            End Set
        End Property

        Public Property ProductID() As Integer
            Get
                If Session("ProductID") Is Nothing Then
                    Session.Add("ProductID", Nothing)
                End If

                Return CInt(Session("ProductID"))
            End Get
            Set(ByVal value As Integer)
                If Not Session("ProductID") Is Nothing Then
                    Session("ProductID") = value
                Else
                    Session.Add("ProductID", value)
                End If
            End Set
        End Property

        Private Sub RefreshDataSources()
            ddlCountry.Items.Clear()
            ddlCountry.Items.Add(New ListItem("Country", ""))
            ddlCountry.AppendDataBoundItems = True
            ddlCountry.DataSource = New CountryController().GetElements
            ddlCountry.DataBind()

            ddlCountry.SelectedValue = CCFramework.Core.Settings.DefaultCountryID.ToString

            ddlProvince.DataSource = New ProvinceController().GetCountryProvince(CCFramework.Core.Settings.DefaultCountryID)
            ddlProvince.DataBind()
        End Sub

        Private Sub btnRegister_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnRegister.Command
            Dim username As String = txtUserName.Text
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
            Dim hp As String = txtHP.Text
            Dim wp As String = txtWP.Text
            Dim cp As String = txtCP.Text
            Dim cmethod As String = ddlContactMethod.SelectedValue
            Dim bd As DateTime = DateTime.Parse(txtBirthDate.Text)
            Dim height As Double
            If Not Double.TryParse(txtHeight.Text, height) Then
                height = 0
            End If
            Dim weight As Double
            If Not Double.TryParse(txtWeight.Text, weight) Then
                weight = 0
            End If
            Dim gender As String = rblGender.SelectedValue
            Dim ecname As String = txtECName.Text
            Dim ecnumber As String = txtECPN.Text
            Dim ecrelation As String = txtECRelation.Text
            Dim mcondition As String = txtMC.Text
            Dim ptraining As String = txtPT.Text
            Dim hdyfus As String = txtHDYFUs.Text
            Dim rfl As String = txtRFL.Text
            Dim wbjj As String = txtWhyBJJ.Text
            Dim wyr As String = txtWYRefered.Text

            Try
                Dim su As Integer = New CCFramework.Core.SimpleUserController().Create(fname, mname, lname, email, hp)

                Dim addressID As Integer = New AddressController().Create(address, city, pcode, province, country)

                Dim ru As Integer = New CCFramework.Core.RegisteredUserController().Create(su, addressID, bd, "", "", username, EPriceLevel.Regular)

                Dim status As New MembershipCreateStatus()

                System.Web.Security.Membership.CreateUser(username, password, email, pwquestion, pwanswer, True, status)

                FormsAuthentication.SetAuthCookie(username, False)

                UserID = su

                mpMessage.Message = "Your membership has been saved and you have been logged in."
                mpMessage.Show()
            Catch ex As Exception
                mpMessage.Message = "An error occurred creating the user: " & ex.Message
                mpMessage.Show()
            End Try
        End Sub

        Private Sub mpMessage_ClickedOK() Handles mpMessage.ClickedOK
            'Response.Redirect("/Default.aspx")
            Response.Redirect("~/CCCommerce/Membership/Contract.aspx?Product=" & ProductID & "&User=" & UserID)
        End Sub

        Private Sub ddlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCountry.SelectedIndexChanged
            If Not ddlCountry.SelectedValue = "" Then
                Dim countryID As Integer = Integer.Parse(ddlCountry.SelectedValue)

                ddlProvince.DataSource = New ProvinceController().GetCountryProvince(countryID)
                ddlProvince.DataBind()

                ddlProvince.Focus()
            End If
        End Sub

        Private Function UserIsRegistered(ByRef UserId As Integer) As Boolean
            Dim username As String = System.Web.Security.Membership.GetUser.UserName

            Dim registeredUser As RegisteredUser = New CCFramework.Core.RegisteredUserController().GetByUserName(username)

            If Not registeredUser Is Nothing Then
                UserId = registeredUser.UserID
                Return True
            Else
                Return False
            End If
        End Function

    End Class
End NameSpace