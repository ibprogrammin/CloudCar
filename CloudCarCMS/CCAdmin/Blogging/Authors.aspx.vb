Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Blogging

    Partial Public Class Authors
        Inherits Page


        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub LoadAuthor(ByVal AuthorID As Integer)
            Try
                Dim author As Author = CCFramework.Blogging.AuthorController.GetElement(AuthorID)

                hfAuthorID.Value = author.ID.ToString
                pvPermalink.ItemID = author.ID

                txtName.Text = author.Name
                txtBiography.InnerText = author.Biography
                txtAvatarURL.Text = author.AvatarURL

                txtBrowserTitle.Text = author.Title
                txtKeywords.Text = author.Keywords
                txtDescription.Text = author.Description
                txtPermalink.Text = author.Permalink

            Catch ex As CCFramework.Blogging.BlogController.InvalidBlogException
                lblStatus.Text = "Uh-oh! An invalid author has been selected. Please try again."
                lblStatus.Visible = True
            Catch ex As Exception
                lblStatus.Text = ex.ToString
                lblStatus.Visible = True
            End Try
        End Sub

        Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Dim AuthorID As Integer

            If Page.IsValid Then
                Dim Name As String = txtName.Text.Trim
                Dim Biography As String = txtBiography.InnerText.Trim
                Dim AvatarURL As String = txtAvatarURL.Text.Trim

                Dim BrowserTitle As String = txtBrowserTitle.Text.Trim
                Dim Permalink As String = txtPermalink.Text.Trim
                Dim Keywords As String = txtKeywords.Text
                Dim Description As String = txtDescription.Text

                If Integer.TryParse(hfAuthorID.Value, AuthorID) Then
                    CCFramework.Blogging.AuthorController.Update(AuthorID, Name, Biography, AvatarURL, BrowserTitle, Keywords, Description, Permalink)

                    lblStatus.Text = "The author has been saved successfully!"
                    lblStatus.Visible = True
                Else
                    hfAuthorID.Value = CCFramework.Blogging.AuthorController.Create(Name, Biography, AvatarURL, BrowserTitle, Keywords, Description, Permalink).ToString

                    lblStatus.Text = "The author has been created successfully!"
                    lblStatus.Visible = True

                    RefreshDataSources()
                End If
            Else
                lblStatus.Text = "The form is not filled out properly!"
                lblStatus.Visible = True
            End If
        End Sub

        Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
            ClearControls()
        End Sub

        Private Sub RefreshDataSources()
            lbAuthors.Items.Clear()
            lbAuthors.Items.Add(New ListItem("Current list of saved authors", ""))
            lbAuthors.AppendDataBoundItems = True
            lbAuthors.DataSource = CCFramework.Blogging.AuthorController.GetElements
            lbAuthors.DataBind()

            'ddlUsers.Items.Clear()
            'ddlUsers.Items.Add(New ListItem("None", ""))
            'ddlUsers.AppendDataBoundItems = True
            'ddlUsers.DataSource = New SMCommerce.RegisteredUserController().GetElements
            'ddlUsers.DataBind()
        End Sub

        Private Sub ClearControls()
            lbAuthors.SelectedValue = Nothing

            hfAuthorID.Value = Nothing

            txtName.Text = ""
            txtBiography.InnerText = ""
            txtAvatarURL.Text = ""

            txtBrowserTitle.Text = ""
            pvPermalink.ItemID = Nothing
            txtKeywords.Text = ""
            txtDescription.Text = ""
            txtPermalink.Text = ""
        End Sub

        Private Sub lbAuthors_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAuthors.SelectedIndexChanged
            If Not lbAuthors.SelectedValue = Nothing Then
                Dim AuthorID As Integer = Integer.Parse(lbAuthors.SelectedValue)

                LoadAuthor(AuthorID)
            End If
        End Sub

        Private Sub btnDelete_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnDelete.Command
            If Not hfAuthorID.Value = Nothing Then
                Dim AuthorID As Integer = Integer.Parse(hfAuthorID.Value)

                If CCFramework.Blogging.AuthorController.Delete(AuthorID) = True Then
                    lblStatus.Text = "The selected author has been deleted permanently!"
                    lblStatus.Visible = True

                    RefreshDataSources()

                    ClearControls()
                Else
                    lblStatus.Text = "An error occured while trying to delete the current author."
                    lblStatus.Visible = True
                End If
            Else
                lblStatus.Text = "Sorry but you do not have an author selected."
                lblStatus.Visible = True
            End If
        End Sub

    End Class
End Namespace