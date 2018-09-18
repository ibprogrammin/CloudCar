Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce

    Partial Public Class Addresses
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()

                ViewState.Add("sortAscending", True)
                ViewState.Add("lastSort", "")

                If Not Request.QueryString("Address") Is Nothing Then
                    Dim AddressId As Integer = Integer.Parse(Request.QueryString("Address"))

                    LoadAddress(AddressId)
                End If

                If Not Request.QueryString("Order") Is Nothing Then
                    Dim OrderID As Integer = Integer.Parse(Request.QueryString("Order"))

                    hlBackToOrder.NavigateUrl = "~/CCAdmin/Commerce/OrderDetails.aspx?Order=" & OrderID
                    hlBackToOrder.Visible = True
                End If
            End If

            lblStatus.Visible = False
        End Sub

        Private Sub LoadAddress(ByVal AddressID As Integer)
            Dim Address As Address = New AddressController().GetElement(AddressID)
            Dim Province As Province = New ProvinceController().GetElement(Address.ProvStateID)

            hfID.Value = AddressID.ToString

            txtAddress.Text = Address.Address
            txtCity.Text = Address.City
            txtPostalCode.Text = Address.PCZIP

            ddlCountry.SelectedValue = Province.CountryID.ToString

            ddlProvince.DataSource = New ProvinceController().GetCountryProvince(Province.CountryID)
            ddlProvince.DataBind()

            ddlProvince.SelectedValue = Address.ProvStateID.ToString
        End Sub

        Protected Sub btnEdit_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim AddressID As Integer = Integer.Parse(e.CommandArgument.ToString)

            LoadAddress(AddressID)
        End Sub

        Protected Sub btnDelete_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim AddressID As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim Address As New AddressController

            Try
                Address.Delete(AddressID)

                lblStatus.Text = "You have successfully deleted the Address (ID: " & AddressID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to delete the Address (ID: " & AddressID.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub

        Protected Sub btnAdd_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAdd.Command
            Dim AddressID As Integer
            Dim Address As New AddressController

            If Integer.TryParse(hfID.Value, AddressID) Then
                Address.Update(AddressID, txtAddress.Text, txtCity.Text, txtPostalCode.Text, Integer.Parse(ddlProvince.SelectedValue), Integer.Parse(ddlCountry.SelectedValue))

                lblStatus.Text = "You have successfully updated the address (ID: " & AddressID.ToString & ")"
            Else
                AddressID = Address.Create(txtAddress.Text, txtCity.Text, txtPostalCode.Text, Integer.Parse(ddlProvince.SelectedValue), Integer.Parse(ddlCountry.SelectedValue))

                lblStatus.Text = "You have successfully created the address (ID: " & AddressID.ToString & ")"
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

            txtAddress.Text = ""
            txtCity.Text = ""
            txtPostalCode.Text = ""

            ddlCountry.SelectedValue = ""
            ddlProvince.Items.Clear()
        End Sub

        Private Sub RefreshDataSources()
            ddlCountry.Items.Clear()
            ddlCountry.Items.Add(New ListItem("Country", ""))
            ddlCountry.AppendDataBoundItems = True
            ddlCountry.DataSource = New CountryController().GetElements
            ddlCountry.DataBind()

            dgAddress.DataSource = New AddressController().GetElements
            dgAddress.DataBind()
        End Sub

        Private Sub dgAddress_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgAddress.PageIndexChanged
            dgAddress.CurrentPageIndex = e.NewPageIndex

            SortAddressGrid(lastSort, sortAscending)
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

        Private Sub dgAddress_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgAddress.SortCommand
            If e.SortExpression = lastSort Then
                sortAscending = Not sortAscending
            End If

            SortAddressGrid(e.SortExpression, sortAscending)
        End Sub

        Public Sub SortAddressGrid(ByVal SortExpression As String, ByVal sortAscending As Boolean)
            Select Case SortExpression
                Case "Address"
                    If sortAscending Then
                        dgAddress.DataSource = New AddressController().GetElements.OrderBy(Function(p) p.Address).ToList
                        sortAscending = False
                    Else
                        dgAddress.DataSource = New AddressController().GetElements.OrderByDescending(Function(p) p.Address).ToList
                        sortAscending = True
                    End If
                Case "City"
                    If sortAscending Then
                        dgAddress.DataSource = New AddressController().GetElements.OrderBy(Function(p) p.City).ToList
                        sortAscending = False
                    Else
                        dgAddress.DataSource = New AddressController().GetElements.OrderByDescending(Function(p) p.City).ToList
                        sortAscending = True
                    End If
                Case "Province"
                    If sortAscending Then
                        dgAddress.DataSource = New AddressController().GetElements.OrderBy(Function(p) p.Province.Name).ToList
                        sortAscending = False
                    Else
                        dgAddress.DataSource = New AddressController().GetElements.OrderByDescending(Function(p) p.Province.Name).ToList
                        sortAscending = True
                    End If
                Case "PCZIP"
                    If sortAscending Then
                        dgAddress.DataSource = New AddressController().GetElements.OrderBy(Function(p) p.PCZIP).ToList
                        sortAscending = False
                    Else
                        dgAddress.DataSource = New AddressController().GetElements.OrderByDescending(Function(p) p.PCZIP).ToList
                        sortAscending = True
                    End If
                Case "ID"
                    If sortAscending Then
                        dgAddress.DataSource = New AddressController().GetElements.OrderBy(Function(p) p.ID).ToList
                        sortAscending = False
                    Else
                        dgAddress.DataSource = New AddressController().GetElements.OrderByDescending(Function(p) p.ID).ToList
                        sortAscending = True
                    End If
            End Select

            lastSort = SortExpression

            dgAddress.DataBind()
        End Sub

        Private Sub ddlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCountry.SelectedIndexChanged
            If Not ddlCountry.SelectedValue = "" Then
                Dim countryID As Integer = Integer.Parse(ddlCountry.SelectedValue)

                ddlProvince.DataSource = New ProvinceController().GetCountryProvince(countryID)
                ddlProvince.DataBind()

                ddlProvince.Focus()
            End If
        End Sub

        Protected Function GetProvinceName(ByVal ProvinceID As Integer) As String
            Dim province As Province = New ProvinceController().GetElement(ProvinceID)

            Return province.Name
        End Function

        Protected Function GetCountryName(ByVal ProvinceID As Integer) As String
            Return New CountryController().GetCountryByProvince(ProvinceID).Name
        End Function

    End Class
End Namespace