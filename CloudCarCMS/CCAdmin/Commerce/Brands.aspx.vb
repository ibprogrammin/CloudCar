Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Commerce

    Partial Public Class Brands
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
            Dim BrandID As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim Brand As Brand = New BrandController().GetElement(BrandID)

            hfBrandID.Value = Brand.ID.ToString
            txtName.Text = Brand.Name
            txtDescription.Text = Brand.Description
            txtKeywords.Text = Brand.Keywords
            txtLink.Text = Brand.URL
            txtPermalink.Text = Server.UrlDecode(Brand.Permalink)

            If Brand.LogoImageID.HasValue AndAlso Not Brand.LogoImageID.Value = 0 Then
                hfLogoImageID.Value = Brand.LogoImageID.ToString

                lblLogoFileLocation.Text = "/images/db/" & Brand.LogoImageID & "/full/" & CCFramework.Core.PictureController.GetPicture(Brand.LogoImageID.Value).PictureFileName
                lblLogoFileLocation.Visible = True

                imgLogo.ImageUrl = "/images/db/" & Brand.LogoImageID & "/220/" & Brand.Permalink & ".jpg"
                imgLogo.Visible = True
            End If
        End Sub

        Protected Sub btnDelete_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim BrandID As Integer = Integer.Parse(e.CommandArgument.ToString)
            Dim Brand As New BrandController

            Try
                Brand.Delete(BrandID)

                lblStatus.Text = "You have successfully deleted the brand (ID: " & BrandID.ToString & ")"
            Catch ex As Exception
                lblStatus.Text = "There was an error trying to delete the brand (ID: " & BrandID.ToString & ")"
            End Try

            lblStatus.Visible = True

            RefreshDataSources()
        End Sub

        Protected Sub btnAdd_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAdd.Command
            Dim BrandID As Integer
            Dim Brand As New BrandController

            Dim LogoImageID As Integer

            If fuLogoImage.HasFile Then
                LogoImageID = CCFramework.Core.ImageFunctions.UploadImage(fuLogoImage)
            Else
                If Not Integer.TryParse(hfLogoImageID.Value, LogoImageID) Then
                End If
            End If

            If Integer.TryParse(hfBrandID.Value, BrandID) Then
                Brand.Update(BrandID, txtName.Text, txtDescription.Text, txtKeywords.Text, txtLink.Text, LogoImageID, Server.UrlEncode(txtPermalink.Text))

                lblStatus.Text = "You have successfully updated the brand (ID: " & BrandID.ToString & ")"
            Else
                BrandID = Brand.Create(txtName.Text, txtDescription.Text, txtKeywords.Text, txtLink.Text, LogoImageID, Server.UrlEncode(txtPermalink.Text))

                lblStatus.Text = "You have successfully created the brand (ID: " & BrandID.ToString & ")"
            End If

            lblStatus.Visible = True

            ClearControls()
            RefreshDataSources()
        End Sub

        Private Sub btnClear_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnClear.Command
            ClearControls()
        End Sub

        Private Sub ClearControls()
            hfBrandID.Value = Nothing
            hfLogoImageID.Value = Nothing
            txtName.Text = ""
            txtPermalink.Text = ""
            txtDescription.Text = ""
            txtKeywords.Text = ""
            txtLink.Text = ""
            imgLogo.Visible = False
        End Sub

        Private Sub RefreshDataSources()
            dgBrands.DataSource = New BrandController().GetElements
            dgBrands.DataBind()
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

        Private Sub dgBrands_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgBrands.SortCommand
            If Not e.SortExpression = lastSort Then
                sortAscending = True
            End If

            Select Case e.SortExpression
                Case "Name"
                    If sortAscending Then
                        dgBrands.DataSource = New BrandController().GetElements.OrderBy(Function(p) p.Name)
                        sortAscending = False
                    Else
                        dgBrands.DataSource = New BrandController().GetElements.OrderByDescending(Function(p) p.Name)
                        sortAscending = True
                    End If
                Case "Permalink"
                    If sortAscending Then
                        dgBrands.DataSource = New BrandController().GetElements.OrderBy(Function(p) p.Permalink)
                        sortAscending = False
                    Else
                        dgBrands.DataSource = New BrandController().GetElements.OrderByDescending(Function(p) p.Permalink)
                        sortAscending = True
                    End If
                Case "Description"
                    If sortAscending Then
                        dgBrands.DataSource = New BrandController().GetElements.OrderBy(Function(p) p.Description)
                        sortAscending = False
                    Else
                        dgBrands.DataSource = New BrandController().GetElements.OrderByDescending(Function(p) p.Description)
                        sortAscending = True
                    End If
                Case "ID"
                    If sortAscending Then
                        dgBrands.DataSource = New BrandController().GetElements.OrderBy(Function(p) p.ID)
                        sortAscending = False
                    Else
                        dgBrands.DataSource = New BrandController().GetElements.OrderByDescending(Function(p) p.ID)
                        sortAscending = True
                    End If
            End Select

            lastSort = e.SortExpression

            dgBrands.DataBind()
        End Sub

    End Class
End Namespace