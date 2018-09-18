Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement

Namespace CCAdmin.Archive

    Partial Public Class ContentPageDetails
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Dim PageName As String

                RefreshDataSources()

                If Not Request("ContentPageName") Is Nothing Then
                    PageName = Request("ContentPageName")
                    LoadPage(PageName)
                End If
            End If
        End Sub

        Private Sub LoadPage(ByVal PageId As Integer)
            Try
                Dim CurrentPage As ContentPage = ContentPageController.GetElement(PageId)

                'TODO add a validator to restrict the appropriate field lengths.
                hfPageID.Value = CurrentPage.id

                txtPageTitle.Text = CurrentPage.pageTitle
                txtContentTitle.Text = CurrentPage.contentTitle
                txtPageTitle.Text = CurrentPage.pageTitle
                txtPageContent.Content = CurrentPage.pageContent
                pvPermalink.ItemID = CurrentPage.id

                txtKeywords.Text = CurrentPage.keywords
                txtDescription.Text = CurrentPage.description
                hfPermalink.Value = CurrentPage.permalink
                txtPermalink.Text = CurrentPage.permalink
                txtScript.Text = CurrentPage.script

                ckbSubMenu.Checked = CurrentPage.displaysubmenu

                ddlMenu.SelectedValue = CurrentPage.menuID
                txtMenuOrder.Text = CurrentPage.menuOrder
                txtBreadcrumbTitle.Text = CurrentPage.breadcrumbTitle

                litCurrentPage.Visible = True
                litCurrentPage.Text = "<p><label>Current Page</label>&nbsp;&nbsp;&nbsp;<a href=""/Home/" & CurrentPage.permalink & ".html"" target=""_blank"" style=""background-color: #FFF8C2;"">/Home/" & CurrentPage.permalink & ".html</a></p>"

                RefreshParentPages()

                Try
                    RemoveLinkedPages(CurrentPage.breadcrumbTitle, CurrentPage.id, ContentPageController.GetElement())
                Catch ex As StackOverflowException
                    lblStatus.Text = "There was an error building the linked pages list."
                    lblStatus.Visible = True
                End Try

                If CurrentPage.parentPageID.HasValue AndAlso CurrentPage.parentPageID.HasValue AndAlso CurrentPage.parentPageID > 0 Then
                    ddlParentPage.SelectedValue = CurrentPage.parentPageID
                Else
                    ddlParentPage.SelectedIndex = 0
                End If
            Catch ex As Exception
                lblStatus.Text = ex.ToString
                lblStatus.Visible = True
            End Try
        End Sub

        Private Sub RemoveLinkedPages(ByVal PageTitle As String, ByVal pageID As Integer, ByVal Pages As List(Of ContentPage))
            ddlParentPage.Items.Remove(New ListItem(PageTitle, pageID.ToString))
            For Each item In Pages
                If item.parentPageID = pageID Then
                    RemoveLinkedPages(item.breadcrumbTitle, item.id, Pages)
                End If
            Next
        End Sub

        Private Sub btnSave_Click(ByVal Sender As Object, ByVal Args As EventArgs) Handles btnSave.Click
            Dim PageID As Integer
            Dim parentPageid As Integer
            If Not ddlParentPage.SelectedValue = Nothing Then
                parentPageid = Integer.Parse(ddlParentPage.SelectedValue)
            Else
                parentPageid = 0
            End If

            If Page.IsValid Then
                Dim sitemap As New CCFramework.Generic.SiteMapProvider

                If Integer.TryParse(hfPageID.Value, PageID) Then
                    ContentPageController.Update(PageID, txtContentTitle.Text, txtPageTitle.Text, txtBreadcrumbTitle.Text.Trim, txtPermalink.Text.Trim, txtKeywords.Text, txtDescription.Text, txtPageContent.Content, "", Integer.Parse(ddlMenu.SelectedValue), Integer.Parse(txtMenuOrder.Text), parentPageid, txtScript.Text, ckbSubMenu.Checked, True, -1)

                    sitemap.UpdatePage("/Home/" & hfPermalink.Value & ".html", "/Home/" & txtPermalink.Text & ".html", "", 0.7, MyBase.Context)

                    lblStatus.Text = "Your page has been saved!"
                    lblStatus.Visible = True
                Else
                    ContentPageController.Create(txtContentTitle.Text, txtPageTitle.Text, txtBreadcrumbTitle.Text, txtPermalink.Text.Trim, txtKeywords.Text, txtDescription.Text, txtPageContent.Content, "", Integer.Parse(ddlMenu.SelectedValue), Integer.Parse(txtMenuOrder.Text), parentPageid, txtScript.Text, ckbSubMenu.Checked, True, -1)

                    sitemap.AddNewPage("/Home/" & txtPermalink.Text & ".html", "", 0.7, MyBase.Context)

                    lblStatus.Text = "Your page has been created!"
                    lblStatus.Visible = True

                    RefreshDataSources()
                End If
            End If
        End Sub

        Private Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
            ClearControls()
        End Sub

        Private Sub RefreshDataSources()
            ddlMenu.DataSource = MenuController.GetMenu()
            ddlMenu.DataBind()

            RefreshParentPages()

            lbContentPages.Items.Clear()
            lbContentPages.Items.Add(New ListItem("Select a Content Page", ""))
            lbContentPages.AppendDataBoundItems = True
            lbContentPages.DataSource = ContentPageController.GetElement()
            lbContentPages.DataBind()
        End Sub

        Private Sub RefreshParentPages()
            ddlParentPage.Items.Clear()
            ddlParentPage.Items.Add(New ListItem("Select a Parent Page", ""))
            ddlParentPage.AppendDataBoundItems = True
            ddlParentPage.DataSource = ContentPageController.GetElement()
            ddlParentPage.DataBind()
        End Sub

        Private Sub ClearControls()
            RefreshParentPages()

            hfPageID.Value = Nothing
            pvPermalink.ItemID = Nothing

            txtContentTitle.Text = ""
            txtPageTitle.Text = ""
            txtPageContent.Content = ""
            txtKeywords.Text = ""
            txtDescription.Text = ""
            txtPermalink.Text = ""
            ddlMenu.SelectedIndex = 0
            ddlParentPage.SelectedIndex = 0
            txtMenuOrder.Text = ""
            txtBreadcrumbTitle.Text = ""
            txtScript.Text = ""

            litCurrentPage.Visible = False
            litCurrentPage.Text = ""
        End Sub

        Private Sub lbContentPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lbContentPages.SelectedIndexChanged
            If Not lbContentPages.SelectedValue = Nothing Then
                Dim PageID As Integer = Integer.Parse(lbContentPages.SelectedValue)

                'RefreshDataSources()
                LoadPage(PageID)
            End If
        End Sub

        Private Sub btnDelete_Command(ByVal sender As Object, ByVal e As CommandEventArgs) Handles btnDelete.Command
            If Not hfPageID.Value = Nothing Then
                Dim PageID As Integer = Integer.Parse(hfPageID.Value)

                If ContentPageController.Delete(PageID) = True Then
                    lblStatus.Text = "The selected page has been deleted permanently!"
                    lblStatus.Visible = True

                    RefreshDataSources()

                    ClearControls()
                Else
                    lblStatus.Text = "An error occured while trying to delete the current page."
                    lblStatus.Visible = True
                End If
            Else
                lblStatus.Text = "Sorry but you do not have a page selected."
                lblStatus.Visible = True
            End If
        End Sub

    End Class
End Namespace