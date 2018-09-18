Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce

    Partial Public Class Categories
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()

                ViewState.Add("sortAscending", True)
                ViewState.Add("lastSort", "")

                BackCount = 0
            End If

            BackCount += 1

            Dim productID As Integer

            If Not Request.QueryString("Product") Is Nothing Then
                If Request.QueryString("Product") = "NEW" Then
                    hlBackToProduct.Attributes("onClick") = "javascript:history.go(" & (-BackCount).ToString & ")"
                    hlBackToProduct.Style.Add("cursor", "pointer")
                    hlBackToProduct.Visible = True
                ElseIf Integer.TryParse(Request.QueryString("Product"), productID) Then
                    hlBackToProduct.NavigateUrl = "~/CCAdmin/Commerce/ProductDetails.aspx?Product=" & productID
                    hlBackToProduct.Visible = True
                End If
            End If

            lblStatus.Visible = False
        End Sub

        Protected Sub btnEdit_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim CategoryID As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim Category As Category = New CategoryController().GetElement(CategoryID)

            hfCategoryID.Value = Category.ID.ToString
            txtName.Text = Category.Name
            txtDescription.Text = Category.Description
            txtKeywords.Text = Category.Keywords
            txtPermalink.Text = Server.UrlDecode(Category.Permalink)
            txtBrowserTitle.Text = Category.BrowserTitle
            DetailsTextArea.InnerText = Category.Details
        End Sub

        Protected Sub btnDelete_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim CategoryID As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim Category As New CategoryController

            Try
                Category.Delete(CategoryID)

                lblStatus.Text = "You have successfully deleted the category (ID: " & CategoryID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to delete the category (ID: " & CategoryID.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub

        Protected Sub btnAdd_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAdd.Command
            Dim CategoryID As Integer
            Dim Category As New CategoryController

            If Integer.TryParse(hfCategoryID.Value, CategoryID) Then
                Category.Update(CategoryID, txtName.Text, DetailsTextArea.InnerText, txtBrowserTitle.Text, txtDescription.Text, txtKeywords.Text, Server.UrlEncode(txtPermalink.Text))

                lblStatus.Text = "You have successfully updated the category (ID: " & CategoryID.ToString & ")"
            Else
                CategoryID = Category.Create(txtName.Text, DetailsTextArea.InnerText, txtBrowserTitle.Text, txtDescription.Text, txtKeywords.Text, Server.UrlEncode(txtPermalink.Text))

                lblStatus.Text = "You have successfully created the category (ID: " & CategoryID.ToString & ")"
            End If

            lblStatus.Visible = True

            ClearControls()
            RefreshDataSources()
        End Sub

        Private Sub btnClear_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnClear.Command
            ClearControls()
        End Sub

        Private Sub ClearControls()
            hfCategoryID.Value = Nothing
            txtName.Text = ""
            txtPermalink.Text = ""
            txtDescription.Text = ""
            txtKeywords.Text = ""
            txtBrowserTitle.Text = ""
            DetailsTextArea.InnerText = ""
        End Sub

        Private Sub RefreshDataSources()
            dgCategories.DataSource = New CategoryController().GetElements
            dgCategories.DataBind()
        End Sub

        Private Property BackCount() As Integer
            Get
                Return CInt(ViewState("BackCount"))
            End Get
            Set(ByVal value As Integer)
                ViewState("BackCount") = value
            End Set
        End Property


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

        Private Sub dgCategories_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgCategories.SortCommand
            If Not e.SortExpression = lastSort Then
                sortAscending = True
            End If

            Select Case e.SortExpression
                Case "Name"
                    If sortAscending Then
                        dgCategories.DataSource = New CategoryController().GetElements.OrderBy(Function(p) p.Name)
                        sortAscending = False
                    Else
                        dgCategories.DataSource = New CategoryController().GetElements.OrderByDescending(Function(p) p.Name)
                        sortAscending = True
                    End If
                Case "Permalink"
                    If sortAscending Then
                        dgCategories.DataSource = New CategoryController().GetElements.OrderBy(Function(p) p.Permalink)
                        sortAscending = False
                    Else
                        dgCategories.DataSource = New CategoryController().GetElements.OrderByDescending(Function(p) p.Permalink)
                        sortAscending = True
                    End If
                Case "Description"
                    If sortAscending Then
                        dgCategories.DataSource = New CategoryController().GetElements.OrderBy(Function(p) p.Description)
                        sortAscending = False
                    Else
                        dgCategories.DataSource = New CategoryController().GetElements.OrderByDescending(Function(p) p.Description)
                        sortAscending = True
                    End If
                Case "ID"
                    If sortAscending Then
                        dgCategories.DataSource = New CategoryController().GetElements.OrderBy(Function(p) p.ID)
                        sortAscending = False
                    Else
                        dgCategories.DataSource = New CategoryController().GetElements.OrderByDescending(Function(p) p.ID)
                        sortAscending = True
                    End If
            End Select

            lastSort = e.SortExpression

            dgCategories.DataBind()
        End Sub

    End Class
End Namespace