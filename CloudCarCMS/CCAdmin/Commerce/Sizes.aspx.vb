Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce

    Partial Public Class Sizes
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()

                ViewState.Add("sortAscending", True)
                ViewState.Add("lastSort", "")

                BackCount = 0
            End If

            BackCount += 1

            Dim ProductId As Integer

            If Not Request.QueryString("Product") Is Nothing Then
                If Request.QueryString("Product") = "NEW" Then
                    hlBackToProduct.Attributes("onClick") = "javascript:history.go(" & (-BackCount).ToString & ")"
                    hlBackToProduct.Style.Add("cursor", "pointer")
                    hlBackToProduct.Visible = True
                ElseIf Integer.TryParse(Request.QueryString("Product"), ProductId) Then
                    hlBackToProduct.NavigateUrl = "~/CCAdmin/Commerce/ProductDetails.aspx?Product=" & ProductId
                    hlBackToProduct.Visible = True
                End If
            End If

            lblStatus.Visible = False
        End Sub

        Protected Sub EditButtonClick(ByVal Sender As Object, ByVal E As CommandEventArgs)
            Dim SizeId As Integer = Integer.Parse(E.CommandArgument.ToString)
            Dim Size As Size = New SizeController().GetElement(SizeId)

            hfSizeID.Value = Size.ID.ToString
            txtName.Text = Size.Name
            txtAbreviation.Text = Size.Abreviation
        End Sub

        Protected Sub DeleteButtonClick(ByVal Sender As Object, ByVal E As CommandEventArgs)
            Dim SizeId As Integer = Integer.Parse(E.CommandArgument.ToString)
            Dim CurrentSizeController As New ProductSizeController

            Try
                CurrentSizeController.Delete(SizeId)

                lblStatus.Text = "You have successfully deleted the size (ID: " & SizeId.ToString & ")"
            Catch Ex As Exception
                lblStatus.Text = "There was an error trying to delete the size (ID: " & SizeId.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub

        Protected Sub AddButtonCommand(ByVal Sender As Object, ByVal E As CommandEventArgs) Handles btnAdd.Command
            Dim SizeId As Integer
            Dim CurrentSizeController As New SizeController

            If Integer.TryParse(hfSizeID.Value, SizeId) Then
                CurrentSizeController.Update(SizeId, txtName.Text, txtAbreviation.Text)

                lblStatus.Text = "You have successfully updated the size (ID: " & SizeId.ToString & ")"
            Else
                SizeId = CurrentSizeController.Create(txtName.Text, txtAbreviation.Text)

                lblStatus.Text = "You have successfully created the size (ID: " & SizeId.ToString & ")"
            End If

            lblStatus.Visible = True

            ClearControls()
            RefreshDataSources()
        End Sub

        Private Sub ClearButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnClear.Click
            ClearControls()
        End Sub

        Private Sub ClearControls()
            hfSizeID.Value = Nothing
            txtName.Text = ""
            txtAbreviation.Text = ""
        End Sub

        Private Sub RefreshDataSources()
            dgSizes.DataSource = New SizeController().GetElements
            dgSizes.DataBind()
        End Sub

        Private Property BackCount() As Integer
            Get
                Return CInt(ViewState("BackCount"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("BackCount") = Value
            End Set
        End Property

        Private Property SortAscending() As Boolean
            Get
                Return CBool(ViewState("sortAscending"))
            End Get
            Set(ByVal Value As Boolean)
                ViewState("sortAscending") = Value
            End Set
        End Property

        Private Property LastSort() As String
            Get
                Return ViewState("lastSort").ToString
            End Get
            Set(ByVal Value As String)
                If Not String.IsNullOrEmpty(Value) Then
                    ViewState("lastSort") = Value
                End If
            End Set
        End Property

        Private Sub dgSizes_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgSizes.SortCommand
            If Not e.SortExpression = lastSort Then
                sortAscending = True
            End If

            Select Case e.SortExpression
                Case "Name"
                    If sortAscending Then
                        dgSizes.DataSource = New ProductSizeController().GetElements.OrderBy(Function(p) p.Name)
                        sortAscending = False
                    Else
                        dgSizes.DataSource = New ProductSizeController().GetElements.OrderByDescending(Function(p) p.Name)
                        sortAscending = True
                    End If
                Case "ID"
                    If sortAscending Then
                        dgSizes.DataSource = New ProductSizeController().GetElements.OrderBy(Function(p) p.ID)
                        sortAscending = False
                    Else
                        dgSizes.DataSource = New ProductSizeController().GetElements.OrderByDescending(Function(p) p.ID)
                        sortAscending = True
                    End If
            End Select

            lastSort = e.SortExpression

            dgSizes.DataBind()
        End Sub

    End Class
End Namespace