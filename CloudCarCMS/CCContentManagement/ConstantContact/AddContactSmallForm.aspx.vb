Imports System.Collections.Generic
imports System.Data
imports System.Globalization
imports System.Text
imports System.Web.UI
imports System.Web.UI.WebControls
imports ConstantContactBO
imports ConstantContactUtility
'imports UploadContactForm.App_Code

Partial Public Class AddContactSmallForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            Return
        End If

        If Not Session("NewContactEmailAddress") Is Nothing Then
            txtEmail.Text = Session("NewContactEmailAddress").ToString
            Session.Remove("NewContactEmailAddress")
        End If

        List = SMConstantContact.ConstantContact.GetUserContactListCollection(ClientScript)

        chkListContactLists.DataSource = List
        chkListContactLists.DataBind()

        ProvinceDataTable = SMConstantContact.ConstantContact.GetStateCollection
        dropDownState.DataSource = ProvinceDataTable
        dropDownState.DataBind()

        dropDownCountry.DataSource = SMConstantContact.ConstantContact.GetCountryCollection
        dropDownCountry.DataBind()
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Not Page.IsValid Then
            Return
        End If

        Dim emailAddress As String() = New String() {txtEmail.Text.Trim}

        Try
            Dim nextChunkId As String = Nothing
            Dim list As IList(Of Contact) = Utility.SearchContactByEmail(AuthenticationData, emailAddress, nextChunkId)

            If list.Count = 0 Then
                Dim contact As Contact = GetContactInformation()
                Utility.CreateNewContact(AuthenticationData, contact)

                Dim ma As New System.Net.Mail.MailAddress(txtEmail.Text)
                SMContentManagement.cSubscription.Add(ma)

                Response.Redirect("/Home/mailinglist-thank-you.html")
            Else
                Throw New ConstantException(String.Format(CultureInfo.CurrentCulture, "Email address ""{0}"" is already a contact", txtEmail.Text.Trim()))
            End If
        Catch ce As ConstantException
            Dim stringBuilder As StringBuilder = New StringBuilder()
            stringBuilder.Append("<script language='javascript'>")
            stringBuilder.AppendFormat("alert('{0}')", ce.Message)
            stringBuilder.Append("</script>")

            ClientScript.RegisterStartupScript(GetType(Page), "AlertMessage", stringBuilder.ToString())
        End Try
    End Sub

    Private ReadOnly Property AuthenticationData() As AuthenticationData
        Get
            If Session("AuthenticationData") Is Nothing Then
                Session.Add("AuthenticationData", SMConstantContact.ConstantContact.AuthenticationData)
            End If
            Return CType(Session("AuthenticationData"), AuthenticationData)
        End Get
    End Property

    Private Property List() As List(Of ContactList)
        Get
            Return CType(ViewState("contactList"), List(Of ContactList))
        End Get
        Set(ByVal value As List(Of ContactList))
            ViewState("contactList") = value
        End Set
    End Property

    Private Property ProvinceDataTable() As DataTable
        Get
            Return CType(ViewState("ProvinceTable"), DataTable)
        End Get
        Set(ByVal value As DataTable)
            ViewState("ProvinceTable") = value
        End Set
    End Property

    Private Function GetContactInformation() As Contact

        Dim contact As Contact = New Contact()

        contact.EmailAddress = Server.HtmlEncode(txtEmail.Text.Trim())
        contact.FirstName = Server.HtmlEncode(txtFirst.Text.Trim())
        contact.MiddleName = Server.HtmlEncode(txtMiddle.Text.Trim())
        contact.LastName = Server.HtmlEncode(txtLast.Text.Trim())
        contact.HomePhone = Server.HtmlEncode(txtHome.Text.Trim())
        contact.AddressLine1 = Server.HtmlEncode(txtAddr1.Text.Trim())
        contact.AddressLine2 = Server.HtmlEncode("") 'txtAddr2.Text.Trim())
        contact.AddressLine3 = Server.HtmlEncode("") 'txtAddr3.Text.Trim())
        contact.City = Server.HtmlEncode(txtCity.Text.Trim())

        contact.StateCode = IIf(String.IsNullOrEmpty(dropDownState.SelectedValue), String.Empty, dropDownState.SelectedValue)
        contact.StateName = IIf(String.IsNullOrEmpty(dropDownState.SelectedValue), Server.HtmlEncode(txtOtherState.Text.Trim()), dropDownState.SelectedItem.Text)

        contact.PostalCode = Server.HtmlEncode(txtZip.Text.Trim())
        contact.SubPostalCode = Server.HtmlEncode("") 'txtSubZip.Text.Trim())
        contact.CountryName = dropDownCountry.SelectedItem.Text
        contact.CountryCode = dropDownCountry.SelectedValue
        contact.OptInSource = ContactOptSource.ActionByCustomer
        contact.EmailType = ContactEmailType.Html

        For Each item As ListItem In chkListContactLists.Items
            If Not item.Selected Then Continue For

            Dim contactOptInList As ContactOptInList = New ContactOptInList()
            Dim contactList As New ContactList(item.Value, True)

            contactOptInList.ContactList = New ContactList(item.Value)
            contact.ContactLists.Add(contactOptInList)
        Next

        Return contact
    End Function

    Protected Sub customValidator_ServerValidate(ByVal sender As Object, ByVal args As ServerValidateEventArgs)

        Dim errorMessage As String = String.Empty

        If String.IsNullOrEmpty(txtEmail.Text.Trim()) Then
            errorMessage = "Please enter the contact email address."
            args.IsValid = False
        End If

        If args.IsValid And Not Utility.IsEmail(txtEmail.Text.Trim()) Then
            errorMessage = "Please enter a valid contact email address."
            args.IsValid = False
        End If

        If args.IsValid And Not String.IsNullOrEmpty(txtOtherState.Text.Trim()) And Not String.IsNullOrEmpty(dropDownState.SelectedValue) Then
            errorMessage = "Only one State/Province value is allowed, not both."
            args.IsValid = False
        End If

        If args.IsValid And Not String.IsNullOrEmpty(dropDownState.SelectedValue) And Not String.IsNullOrEmpty(dropDownCountry.SelectedValue) _
            And Not ProvinceDataTable.Rows(dropDownState.SelectedIndex)("CountryCode").ToString = dropDownCountry.SelectedValue Then
            errorMessage = "Mismatched State/Province and Country"
            args.IsValid = False
        End If

        If args.IsValid And Not String.IsNullOrEmpty(dropDownCountry.SelectedValue) _
                And (String.Compare(dropDownCountry.SelectedValue, SMConstantContact.ConstantContact.UnitedStatesCountryCode, StringComparison.Ordinal) = 0 _
                Or String.Compare(dropDownCountry.SelectedValue, SMConstantContact.ConstantContact.CanadaCountryCode, StringComparison.Ordinal) = 0) _
                And Not String.IsNullOrEmpty(txtOtherState.Text.Trim()) Then
            errorMessage = "For US & Canada, select State/Province from the dropdown list box."
            args.IsValid = False
        End If

        If args.IsValid Then
            Dim selected As Boolean = False
            For Each item As ListItem In chkListContactLists.Items
                If item.Selected Then
                    selected = True
                    Exit For
                End If
            Next

            If Not selected Then
                errorMessage = "Please select the list to which your contact will be added."
            End If

            args.IsValid = selected
        End If

        If args.IsValid Then Return

        Dim stringBuilder As StringBuilder = New StringBuilder()
        stringBuilder.Append("<script language='javascript'>")
        stringBuilder.AppendFormat("alert('{0}')", errorMessage)
        stringBuilder.Append("</script>")
        ClientScript.RegisterStartupScript(GetType(Page), "AlertMessage", stringBuilder.ToString())
    End Sub

End Class