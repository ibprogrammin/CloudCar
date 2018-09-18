Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Namespace CCCommerce.Forms

    Partial Public Class RequestForm
        Inherits Page

        'TODO make a setting for this property
        Private ReadOnly PackageProduct As String() = {"30", "30", "30"}

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadPackages()
            End If
        End Sub

        Private Sub AddButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnAdd.Click
            SaveSalesRequest()
        End Sub

        Private Sub ClearButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnClear.Click
            ClearForm()
        End Sub

        Private Sub LoadPackages()
            Dim PackageList As New List(Of Product)

            For Each Item As String In PackageProduct
                Dim CurrentProductId As Integer

                If Integer.TryParse(Item, CurrentProductId) Then
                    Dim CurrentProduct As Product = New ProductController().GetElement(CurrentProductId)

                    PackageList.Add(CurrentProduct)
                End If
            Next

            ddlPackage.DataSource = PackageList
            ddlPackage.DataBind()
        End Sub

        Private Sub ClearForm()
            txtAccessKey.Text = ""

            txtCustomerEmail.Text = ""
            txtCustomerName.Text = ""

            txtSalesRepName.Text = ""
            txtSalesRepEmail.Text = ""

            ddlPackage.SelectedValue = Nothing
        End Sub

        Private ReadOnly VerifiedSalesRepEmails As String() = {"daniel@seriousmonkey.ca", "melissa@seriousmonkey.ca"}
        Private ReadOnly ApprovedAccessKeys As String() = {"ABCD1234", "DCBA4321"}

        Private Sub SaveSalesRequest()
            Dim AccessKey As String = txtAccessKey.Text

            Dim SalesRepName As String = txtSalesRepName.Text
            Dim SalesRepEmail As String = txtSalesRepEmail.Text.ToLower

            Dim CustomerName As String = txtCustomerName.Text
            Dim CustomerEmail As String = txtCustomerEmail.Text.ToLower

            Dim ProductId As Integer = Integer.Parse(ddlPackage.SelectedValue)

            If ApprovedAccessKeys.Contains(AccessKey) And VerifiedSalesRepEmails.Contains(SalesRepEmail) Then
                Dim CurrentSalesRequestController As New SalesRequestController()
                Dim CurrentRequestId As Integer

                CurrentRequestId = CurrentSalesRequestController.Create(SalesRepName, SalesRepEmail, CustomerName, CustomerEmail, ProductId, 1)

                Dim CurrentRequestKey As Guid = CurrentSalesRequestController.GetElement(CurrentRequestId).RequestKey

                ClearForm()

                SalesRequestController.SendCustomerRequestEmail(CustomerEmail, CustomerName, CurrentRequestKey)
                SalesRequestController.SendSalesRepThankYouEmail(SalesRepEmail, SalesRepName)

                lblStatus.Text = "The package request has been sent and the customer will recieve an email shortly. Thank you!"
                lblStatus.Visible = True
            Else
                lblStatus.Text = "There was an error validating your request. Either your access key or your email address is incorrect. Please try again."
                lblStatus.Visible = True
            End If
        End Sub

    End Class
End NameSpace