Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Web.Security
Imports CloudCar.CCFramework.Commerce

Partial Public Class TransferUsers
    Inherits Page

    Private XLFileName As String = "Users.xlsx"

    Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            lblFilePath.Text = "Loading from file: " & Server.MapPath(XLFileName)
        End If
    End Sub

    Private Sub btnReadXL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReadXL.Click
        lblData.Text = ""

        'DeleteAllRetailers()

        LoadRetailerTable("Users")
    End Sub

    Public Sub LoadRetailerTable(ByVal TableName As String)
        Dim connstring As String
        Dim conXlsx As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Server.MapPath(XLFileName) & ";Extended Properties=Excel 12.0;"
        Dim conXls As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Server.MapPath(XLFileName) & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"";"

        If XLFileName.EndsWith(".xlsx") Then
            connstring = conXlsx
        Else
            connstring = conXls
        End If

        Dim dr As DataRow
        Dim olecon As OleDbConnection
        Dim olecomm As OleDbCommand
        Dim oleadpt As OleDbDataAdapter
        Dim ds As DataSet
        Dim body As String = vbNullString
        Dim userCount As Integer = 0

        Try
            olecon = New OleDbConnection(connstring)
            olecomm = New OleDbCommand("Select Username, [First Name], [Middle Name], [Last Name], Address, City, [Country ID], [Province ID], [Postal Code], Email, Password, [Phone Number], [Password Question], [Password Answer] from [" & TableName & "$]", olecon)
            oleadpt = New OleDbDataAdapter(olecomm)

            ds = New DataSet()

            olecon.Open()

            oleadpt.Fill(ds, "Master")

            dgExcelData.DataSource = ds.Tables("Master")
            dgExcelData.DataBind()

            For Each item As DataRow In ds.Tables("Master").Rows
                If Not IsNothing(item("City")) And Not item("City").ToString = "" Then
                    If ckbIssueMS.Checked Then
                        Dim Username As String = ""
                        Dim FirstName As String = ""
                        Dim MiddleName As String = ""
                        Dim LastName As String = ""
                        Dim Email As String = ""
                        Dim Address As String = ""
                        Dim City As String = ""
                        Dim Country As Integer = 0
                        Dim Province As Integer = 0
                        Dim PostalCode As String = ""
                        Dim PhoneNumber As String = ""
                        Dim Password As String = ""
                        Dim PasswordQuestion As String = ""
                        Dim PasswordAnswer As String = ""

                        If Not IsNothing(item("Username")) Then
                            Username = item("Username").ToString
                        End If

                        If Not IsNothing(item("First Name")) Then
                            FirstName = item("First Name").ToString
                        End If

                        If Not IsNothing(item("Last Name")) Then
                            LastName = item("Last Name").ToString
                        End If

                        If Not IsNothing(item("Middle Name")) Then
                            MiddleName = item("Middle Name").ToString
                        End If

                        If Not IsNothing(item("Email")) Then
                            Email = item("Email").ToString
                        End If

                        If Not IsNothing(item("Address")) Then
                            Address = item("Address").ToString & " "
                        End If

                        If Not IsNothing(item("City")) Then
                            City = item("City").ToString
                        End If

                        If Not IsNothing(item("Country ID")) Then
                            Country = Integer.Parse(item("Country ID").ToString)
                        End If

                        If Not IsNothing(item("Province ID")) Then
                            Province = Integer.Parse(item("Province ID").ToString)
                        End If

                        If Not IsNothing(item("Postal Code")) Then
                            PostalCode = item("Postal Code").ToString
                        End If

                        If Not IsNothing(item("Phone Number")) Then
                            PhoneNumber = item("Phone Number").ToString
                        End If

                        If Not IsNothing(item("Password")) Then
                            Password = item("Password").ToString
                        End If

                        If Not IsNothing(item("Password Question")) Then
                            PasswordQuestion = item("Password Question").ToString
                        End If

                        If Not IsNothing(item("Password Answer")) Then
                            PasswordAnswer = item("Password Answer").ToString
                        End If


                        Dim su As Integer = New CCFramework.Core.SimpleUserController().Create(FirstName, MiddleName, LastName, Email, PhoneNumber)
                        Dim addressID As Integer = New AddressController().Create(Address, City, PostalCode, Province, Country)
                        Dim ru As Integer = New CCFramework.Core.RegisteredUserController().Create(su, addressID, Date.Today, "", "", Username, EPriceLevel.Regular)

                        Dim status As New MembershipCreateStatus()
                        Membership.CreateUser(Username, Password, Email, PasswordQuestion, PasswordAnswer, True, status)
                        Roles.AddUserToRole(Username, "Regular")

                        userCount = userCount + 1
                    End If

                    body &= "<b>User:</b> " & item("Username").ToString & " <b>City:</b> " & item("City").ToString & " <b>Email:</b> " & item("Email").ToString & "<br>"
                Else
                    body &= "<span style='color: red'><b>User: " & item("Username").ToString & " - There was Insufficient data to create this user. </b></span><br>"
                End If
            Next

            olecon.Close()

            olecon.Dispose()
            olecomm.Dispose()
            oleadpt.Dispose()

            ds.Dispose()

            lblData.Text = body
        Catch ex As Exception
            lblData.Text &= ex.Message & " " & ex.StackTrace
        Finally
            olecon = Nothing
            olecomm = Nothing
            oleadpt = Nothing
            ds = Nothing
            dr = Nothing
        End Try

        If ckbIssueMS.Checked Then
            lblData.Text &= "<br /><br />" & userCount.ToString & " Users Created in Application <br /><br />"
            'lblData.Text &= body
            'Send(New Net.Mail.MailAddress("dan30odd08@hotmail.com"), "Distributor account list for Semex", body)
        End If

        lblData.Visible = True
    End Sub

    Public Shared Sub Send(ByVal ToAddress As System.Net.Mail.MailAddress, ByVal Subject As String, ByVal Body As String)
        'Dim oMail As New System.Net.Mail.MailMessage
        'oMail.IsBodyHtml = True
        'oMail.Body = Body
        'oMail.From = New System.Net.Mail.MailAddress("no-reply@yourdomain.com", GlobalVariables.WebsiteName)
        'oMail.Subject = Subject
        'oMail.To.Add(ToAddress)
        'Dim smtp As New System.Net.Mail.SmtpClient
        'smtp.Host = GlobalVariables.GetSMTPHost
        'smtp.Port = GlobalVariables.GetSMTPPort
        'smtp.Credentials = New System.Net.NetworkCredential(GlobalVariables.GetSMTPUser, GlobalVariables.GetSMTPPass)
        'smtp.Send(oMail)
    End Sub

    'Private Function DeleteRetailer(ByVal companyName As String) As Boolean
    '    Dim db As New DataLayer.LinqLayer.LinqRetailersDataContext

    '    Dim retailer As DataLayer.LinqLayer.Retailer = (From r In db.Retailers Where r.name = companyName Select r).Single

    '    db.Retailers.DeleteOnSubmit(retailer)
    '    db.SubmitChanges()
    'End Function

    'Private Function DeleteAllRetailers() As Boolean
    '    Dim db As New DataLayer.LinqLayer.LinqRetailersDataContext

    '    Dim retailers = From r In db.Retailers Select r

    '    db.Retailers.DeleteAllOnSubmit(retailers)
    '    db.SubmitChanges()
    'End Function

End Class