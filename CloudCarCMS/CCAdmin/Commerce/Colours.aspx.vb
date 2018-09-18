Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce

    Partial Public Class Colours
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
                ElseIf Integer.TryParse(Request.QueryString("Product"), productID) Then
                    hlBackToProduct.NavigateUrl = "~/CCAdmin/Commerce/ProductDetails.aspx?Product=" & ProductId
                    hlBackToProduct.Visible = True
                End If
            End If

            lblStatus.Visible = False
        End Sub

        Protected Sub EditButtonClick(ByVal Sender As Object, ByVal E As CommandEventArgs)
            Dim ColorId As Integer = Integer.Parse(E.CommandArgument.ToString)

            Dim CurrentColourController As New ColourController

            Dim Color As Color = CurrentColourController.GetElement(ColorId)

            hfColorID.Value = Color.ID.ToString
            txtColor.Text = Color.Name
            txtColourCode.Text = Color.RGBColourCode
        End Sub

        Protected Sub DeleteButtonClick(ByVal Sender As Object, ByVal E As CommandEventArgs)
            Dim ColorId As Integer = Integer.Parse(E.CommandArgument.ToString)

            Dim CurrentColourController As New ColourController

            Try
                CurrentColourController.Delete(ColorId)

                lblStatus.Text = "You have successfully deleted color ID: " & ColorId.ToString
            Catch Ex As Exception
                lblStatus.Text = "There was an error trying to delete the colour with ID: " & ColorId.ToString
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub

        Protected Sub AddButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnAdd.Click
            Dim ColorId As Integer
            Dim CurrentColourController As New ColourController

            If Integer.TryParse(hfColorID.Value, ColorId) Then
                CurrentColourController.Update(ColorId, txtColor.Text, txtColourCode.Text)

                lblStatus.Text = "You have successfully updated the colour " & txtColor.Text & ""
            Else
                ColorId = CurrentColourController.Create(txtColor.Text, txtColourCode.Text)

                lblStatus.Text = "You have successfully created the colour " & txtColor.Text & ""
            End If

            lblStatus.Visible = True

            ClearControls()
            RefreshDataSources()
        End Sub

        Private Sub ClearButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnClear.Click
            ClearControls()
        End Sub

        Private Sub ClearControls()
            hfColorID.Value = Nothing
            txtColor.Text = ""
            txtColourCode.Text = ""
        End Sub

        Private Sub RefreshDataSources()
            dgColors.DataSource = New ColourController().GetElements
            dgColors.DataBind()
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

        Private Sub ColorsDataGridSortCommand(ByVal Sender As Object, ByVal E As DataGridSortCommandEventArgs) Handles dgColors.SortCommand
            If Not e.SortExpression = lastSort Then
                sortAscending = True
            End If

            Dim CurrentColourController As New ColourController

            Select Case e.SortExpression
                Case "Name"
                    If sortAscending Then
                        dgColors.DataSource = CurrentColourController.GetElements.OrderBy(Function(c) c.Name)
                        sortAscending = False
                    Else
                        dgColors.DataSource = CurrentColourController.GetElements.OrderByDescending(Function(c) c.Name)
                        sortAscending = True
                    End If
                Case "ID"
                    If sortAscending Then
                        dgColors.DataSource = CurrentColourController.GetElements.OrderBy(Function(c) c.ID)
                        sortAscending = False
                    Else
                        dgColors.DataSource = CurrentColourController.GetElements.OrderByDescending(Function(c) c.ID)
                        sortAscending = True
                    End If
            End Select

            lastSort = e.SortExpression

            dgColors.DataBind()
        End Sub

    End Class
End Namespace