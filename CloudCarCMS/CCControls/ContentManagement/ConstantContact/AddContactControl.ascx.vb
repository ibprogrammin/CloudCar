Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Web.UI
Imports System.Globalization
Imports ConstantContactBO
Imports ConstantContactUtility
'Imports SMConstantContact

Partial Public Class AddContactControl
    Inherits System.Web.UI.UserControl

#Region "Evant Handler"

    ''' <summary>
    ''' Page Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    ''' <summary>
    ''' Search button click event handler.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Not Page.IsValid Then
            Return
        End If

        'Dim emailAddress As String() = New String() {txtNewEmail.Text.Trim()}

        Try
            Response.Redirect("~/cms/ConstantContact/AddContactSmallForm.aspx")

            'Dim nextChunkId As String = Nothing
            '' search Contact by email
            'Dim list As IList(Of Contact) = Utility.SearchContactByEmail(AuthenticationData, emailAddress, nextChunkId)
            'If Not list.Count = 0 Then
            '    ' save Contact Id to be updated into Session
            '    Session.Add("ContactIdToUpdate", list(0).Id)
            '    Session.Add("ContactEditLink", list(0).Link)
            '    ' redirect to update Contact
            '    Response.Redirect("~/UpdateContactSmallForm.aspx")
            'Else
            '    ' save the Contact E-mail Address
            '    'Session.Add("NewContactEmailAddress", txtEmail.Text.Trim())
            '    ' redirect to add Contact
            '    Response.Redirect("~/cms/ConstantContact/AddContactSmallForm.aspx")
            'End If
        Catch ce As Exception
            Dim stringBuilder As StringBuilder = New StringBuilder()
            stringBuilder.Append("<script language='javascript'>")
            stringBuilder.AppendFormat("alert('{0}')", ce.StackTrace)
            stringBuilder.Append("</script>")

            Page.ClientScript.RegisterStartupScript(GetType(Page), "AlertMessage", stringBuilder.ToString())
        End Try
    End Sub

    Protected Sub btnSignUp_Click(ByVal sender As Object, ByVal e As EventArgs)
        If Not Page.IsValid Then
            Return
        End If

        Dim emailAddress As String() = New String() {txtNewEmail.Text.Trim()}

        Try
            Dim nextChunkId As String = Nothing
            Dim list As IList(Of Contact) = Utility.SearchContactByEmail(AuthenticationData, emailAddress, nextChunkId)

            If list.Count = 0 Then
                ' create new Contact
                Dim contact As Contact = GetContactInformation()

                Utility.CreateNewContact(AuthenticationData, contact)
                'Response.Redirect("~/AddContactConfirmation.aspx")
                Response.Redirect("/Home/thank-you.html")
            Else
                Throw New ConstantException(String.Format(CultureInfo.CurrentCulture, "Email address ""{0}"" is already a contact", txtNewEmail.Text.Trim()))
            End If

        Catch ce As ConstantException
            Dim stringBuilder As StringBuilder = New StringBuilder()

            stringBuilder.Append("<script language='javascript'>")
            stringBuilder.AppendFormat("alert('{0}')", ce.Message)
            stringBuilder.Append("</script>")

            Page.ClientScript.RegisterStartupScript(GetType(Page), "AlertMessage", stringBuilder.ToString())
        End Try
    End Sub

    ''' <summary>
    ''' Validators provided information
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Protected Sub customValidator_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        'Dim errorMessage As String = String.Empty

        'If String.IsNullOrEmpty(txtNewEmail.Text.Trim()) Then
        '    errorMessage = "Please enter the contact email address."
        '    args.IsValid = False
        'End If

        'If args.IsValid And Not Utility.IsEmail(txtNewEmail.Text.Trim()) Then
        '    errorMessage = "Please enter a valid contact email address."
        '    args.IsValid = False
        'End If

        'If args.IsValid Then Return

        'Dim StringBuilder As StringBuilder = New StringBuilder()

        'StringBuilder.Append("<script language='javascript'>")
        'StringBuilder.AppendFormat("alert('{0}')", errorMessage)
        'StringBuilder.Append("</script>")

        'Page.ClientScript.RegisterStartupScript(GetType(Page), "AlertMessage", StringBuilder.ToString())
    End Sub

#End Region

#Region "Properties"

    ''' <summary>
    ''' Authentication Data
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property AuthenticationData() As AuthenticationData
        Get
            If Session("AuthenticationData") Is Nothing Then
                Session.Add("AuthenticationData", SMConstantContact.ConstantContact.AuthenticationData)
            End If

            Return CType(Session("AuthenticationData"), AuthenticationData)
        End Get
    End Property

#End Region

    Private Function GetContactInformation() As Contact

        Dim contact As Contact = New Contact()

        contact.EmailAddress = Server.HtmlEncode(txtNewEmail.Text.Trim())
        'contact.FirstName = "Pending" 'Server.HtmlEncode(txtFirst.Text.Trim())
        'contact.MiddleName = "" 'Server.HtmlEncode(txtMiddle.Text.Trim())
        'contact.LastName = "Pending" 'Server.HtmlEncode(txtLast.Text.Trim())
        'contact.HomePhone = Server.HtmlEncode(txtHome.Text.Trim())
        'contact.AddressLine1 = Server.HtmlEncode(txtAddr1.Text.Trim())
        'contact.AddressLine2 = Server.HtmlEncode(txtAddr2.Text.Trim())
        'contact.AddressLine3 = Server.HtmlEncode(txtAddr3.Text.Trim())
        'contact.City = Server.HtmlEncode(txtCity.Text.Trim())
        'contact.StateCode = String.IsNullOrEmpty(dropDownState.SelectedValue) ? string.Empty : dropDownState.SelectedValue
        'contact.StateName = String.IsNullOrEmpty(dropDownState.SelectedValue) ? Server.HtmlEncode(txtOtherState.Text.Trim()) : dropDownState.SelectedItem.Text
        'contact.PostalCode = Server.HtmlEncode(txtZip.Text.Trim())
        'contact.SubPostalCode = Server.HtmlEncode(txtSubZip.Text.Trim())
        'contact.CountryName = dropDownCountry.SelectedItem.Text
        'contact.CountryCode = dropDownCountry.SelectedValue
        contact.OptInSource = ContactOptSource.ActionByContact
        ' loop through all the checkbox controls
        'For Each item As ListItem In chkListContactLists.Items
        '    If Not item.Selected Then Continue For

        Dim ContactOptInList As ContactOptInList = New ContactOptInList()
        ContactOptInList.ContactList = New ContactList("General Interest")
        contact.ContactLists.Add(ContactOptInList)
        'Next

        Return contact
    End Function

End Class