Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce

    Partial Public Class Provinces
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
            Dim Province As Province = New ProvinceController().GetElement(ID)

            hfID.Value = Province.ID.ToString
            txtName.Text = Province.Name
            txtCode.Text = Province.Code
            txtTax.Text = Province.Tax.ToString
            ddlCountry.SelectedValue = Province.CountryID.ToString
        End Sub

        Protected Sub btnDelete_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim ID As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim Province As New ProvinceController

            Try
                Province.Delete(ID)

                lblStatus.Text = "You have successfully deleted the province (ID: " & ID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to delete the province (ID: " & ID.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub

        Protected Sub btnAdd_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAdd.Command
            Dim ID As Integer
            Dim Province As New ProvinceController

            If Integer.TryParse(hfID.Value, ID) Then
                Province.Update(ID, txtName.Text, Decimal.Parse(txtTax.Text), Integer.Parse(ddlCountry.SelectedValue), txtCode.Text)

                lblStatus.Text = "You have successfully updated the province (ID: " & ID.ToString & ")"
            Else
                ID = Province.Create(txtName.Text, Decimal.Parse(txtTax.Text), Integer.Parse(ddlCountry.SelectedValue), txtCode.Text)

                lblStatus.Text = "You have successfully created the province (ID: " & ID.ToString & ")"
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
            ddlCountry.SelectedValue = Nothing
        End Sub

        Private Sub RefreshDataSources()
            'Dim ProvinceList As New List(Of Province)
            'Dim CountryList As New List(Of Country)

            'ProvinceList = New Commerce.cProvince().GetElements
            'CountryList = New Commerce.cCountry().GetElements

            'Dim joinedProvinceList = From p In ProvinceList Join c In CountryList On p.CountryID Equals c.ID _
            '                         Select New With {p.ID, p.Name, p.Code, p.Tax, .CountryName = c.Name}

            'For Each Province In ProvinceList
            '    Province.Country = New Commerce.cCountry().GetElement(Province.CountryID)
            'Next

            'dgProvinces.DataSource = ProvinceList
            'dgProvinces.DataBind()
            Dim db As New CommerceDataContext

            Dim joinedProvinceList = (From p In db.Provinces Join c In db.Countries On p.CountryID Equals c.ID _
                    Select New With {p.ID, p.Name, p.Code, p.Tax, p.CountryID, .CountryName = c.Name})

            RegionGridView.DataSource = joinedProvinceList
            RegionGridView.DataBind()

            ddlCountry.Items.Clear()
            ddlCountry.Items.Add(New ListItem("Country", ""))
            ddlCountry.AppendDataBoundItems = True
            ddlCountry.DataSource = New CountryController().GetElements
            ddlCountry.DataBind()
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

        Private Sub RegionGridViewSorting(ByVal Source As Object, ByVal Args As GridViewSortEventArgs) Handles RegionGridView.Sorting
            If Not Args.SortExpression = lastSort Then
                sortAscending = True
            End If

            Dim db As New CommerceDataContext

            Dim joinedProvinceList = (From p In db.Provinces Join c In db.Countries On p.CountryID Equals c.ID _
                    Select New With {p.ID, p.Name, p.Code, p.Tax, p.CountryID, .CountryName = c.Name})

            Select Case Args.SortExpression
                Case "Name"
                    If sortAscending Then
                        RegionGridView.DataSource = joinedProvinceList.OrderBy(Function(p) p.Name)
                        sortAscending = False
                    Else
                        RegionGridView.DataSource = joinedProvinceList.OrderByDescending(Function(p) p.Name)
                        sortAscending = True
                    End If
                Case "Code"
                    If sortAscending Then
                        RegionGridView.DataSource = joinedProvinceList.OrderBy(Function(p) p.Code)
                        sortAscending = False
                    Else
                        RegionGridView.DataSource = joinedProvinceList.OrderByDescending(Function(p) p.Code)
                        sortAscending = True
                    End If
                Case "Tax"
                    If sortAscending Then
                        RegionGridView.DataSource = joinedProvinceList.OrderBy(Function(p) p.Tax)
                        sortAscending = False
                    Else
                        RegionGridView.DataSource = joinedProvinceList.OrderByDescending(Function(p) p.Tax)
                        sortAscending = True
                    End If
                Case "CountryID"
                    If sortAscending Then
                        RegionGridView.DataSource = joinedProvinceList.OrderBy(Function(p) p.CountryID)
                        sortAscending = False
                    Else
                        RegionGridView.DataSource = joinedProvinceList.OrderByDescending(Function(p) p.CountryID)
                        sortAscending = True
                    End If
                Case "ID"
                    If sortAscending Then
                        RegionGridView.DataSource = joinedProvinceList.OrderBy(Function(p) p.ID)
                        sortAscending = False
                    Else
                        RegionGridView.DataSource = joinedProvinceList.OrderByDescending(Function(p) p.ID)
                        sortAscending = True
                    End If
            End Select

            lastSort = Args.SortExpression

            RegionGridView.DataBind()

            db.Dispose()
        End Sub

        Private Sub RegionGridViewPageIndexChanging(ByVal Sender As Object, ByVal Args As GridViewPageEventArgs) Handles RegionGridView.PageIndexChanging
            RegionGridView.PageIndex = Args.NewPageIndex
            RefreshDataSources()
        End Sub

    End Class

End Namespace