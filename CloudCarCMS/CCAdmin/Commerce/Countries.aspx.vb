Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce

    Partial Public Class Countries
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()

                ViewState.Add("sortAscending", True)
                ViewState.Add("lastSort", "")
            End If

            lblStatus.Visible = False
        End Sub

        Protected Sub btnEdit_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim ID As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim Country As Country = New CountryController().GetElement(ID)

            hfID.Value = Country.ID.ToString
            txtName.Text = Country.Name
            txtCode.Text = Country.Code
            txtTax.Text = Country.Tax.ToString
        End Sub

        Protected Sub btnDelete_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim ID As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim Country As New CountryController()

            Try
                Country.Delete(ID)

                lblStatus.Text = "You have successfully deleted the country (ID: " & ID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to delete the country (ID: " & ID.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub

        Protected Sub btnAdd_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAdd.Command
            Dim ID As Integer
            Dim Country As New CountryController

            If Integer.TryParse(hfID.Value, ID) Then
                Country.Update(ID, txtName.Text, Decimal.Parse(txtTax.Text), txtCode.Text)

                lblStatus.Text = "You have successfully updated the country (ID: " & ID.ToString & ")"
            Else
                ID = Country.Create(txtName.Text, Decimal.Parse(txtTax.Text), txtCode.Text)

                lblStatus.Text = "You have successfully created the country (ID: " & ID.ToString & ")"
            End If

            lblStatus.Visible = True

            ClearControls()
            RefreshDataSources()
        End Sub

        Private Sub btnClear_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnClear.Command
            ClearControls()
        End Sub

        Private Sub ClearControls()
            hfID.Value = Nothing
            txtName.Text = ""
            txtCode.Text = ""
            txtTax.Text = ""
        End Sub

        Private Sub RefreshDataSources()
            dgCountries.DataSource = New CountryController().GetElements
            dgCountries.DataBind()
        End Sub


        Private Property sortAscending() As Boolean
            Get
                Return CBool(ViewState("sortAscending"))
            End Get
            Set(ByVal value As Boolean)
                ViewState("sortAscending") = value
            End Set
        End Property

        Private Property lastSort() As String
            Get
                Return ViewState("lastSort").ToString
            End Get
            Set(ByVal value As String)
                If Not String.IsNullOrEmpty(value) Then
                    ViewState("lastSort") = value
                End If
            End Set
        End Property

        Private Sub dgCountries_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgCountries.SortCommand
            If Not e.SortExpression = lastSort Then
                sortAscending = True
            End If

            Select Case e.SortExpression
                Case "Name"
                    If sortAscending Then
                        dgCountries.DataSource = New CountryController().GetElements.OrderBy(Function(p) p.Name)
                        sortAscending = False
                    Else
                        dgCountries.DataSource = New CountryController().GetElements.OrderByDescending(Function(p) p.Name)
                        sortAscending = True
                    End If
                Case "Code"
                    If sortAscending Then
                        dgCountries.DataSource = New CountryController().GetElements.OrderBy(Function(p) p.Code)
                        sortAscending = False
                    Else
                        dgCountries.DataSource = New CountryController().GetElements.OrderByDescending(Function(p) p.Code)
                        sortAscending = True
                    End If
                Case "Tax"
                    If sortAscending Then
                        dgCountries.DataSource = New CountryController().GetElements.OrderBy(Function(p) p.Tax)
                        sortAscending = False
                    Else
                        dgCountries.DataSource = New CountryController().GetElements.OrderByDescending(Function(p) p.Tax)
                        sortAscending = True
                    End If
                Case "ID"
                    If sortAscending Then
                        dgCountries.DataSource = New CountryController().GetElements.OrderBy(Function(p) p.ID)
                        sortAscending = False
                    Else
                        dgCountries.DataSource = New CountryController().GetElements.OrderByDescending(Function(p) p.ID)
                        sortAscending = True
                    End If
            End Select

            lastSort = e.SortExpression

            dgCountries.DataBind()
        End Sub

    End Class
End Namespace