Imports CloudCar.CCFramework.ContentManagement.CallToActionModule
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement

Namespace CCAdmin.ContentManagement

    Partial Public Class Pages
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                PagesListPlaceHolder.Visible = True
                RefreshDataSources()
            End If
        End Sub

        Private Sub LoadPage(ByVal PageId As Integer)
            Try
                Dim CurrentPage As ContentPage = ContentPageController.GetElement(PageId)

                PagesListPlaceHolder.Visible = False
                PageEditPlaceHolder.Visible = True

                BackButton.Enabled = True
                btnSave.Enabled = True
                btnDelete.Enabled = True

                'TODO add a validator to restrict the appropriate field lengths.
                hfPageId.Value = CurrentPage.id.ToString

                PageContentTextArea.InnerText = CurrentPage.pageContent
                SecondaryPageContentTextArea.InnerText = CurrentPage.secondaryContent

                ShowHeadingCheckBox.Checked = CurrentPage.ShowHeading
                txtPageTitle.Text = CurrentPage.pageTitle
                txtContentTitle.Text = CurrentPage.contentTitle
                pvPermalink.ItemID = CurrentPage.id

                If CurrentPage.CallToActionId.HasValue Then
                    CallToActionDropDown.SelectedValue = CurrentPage.CallToActionId.Value.ToString
                End If

                txtKeywords.Text = CurrentPage.keywords
                txtDescription.Text = CurrentPage.description
                hfPermalink.Value = CurrentPage.permalink
                txtPermalink.Text = CurrentPage.permalink
                txtScript.Text = CurrentPage.script

                ckbSubMenu.Checked = CurrentPage.displaysubmenu

                ddlMenu.SelectedValue = CurrentPage.menuID.ToString
                txtMenuOrder.Text = CurrentPage.menuOrder.ToString
                txtBreadcrumbTitle.Text = CurrentPage.breadcrumbTitle

                litCurrentPage.Visible = True
                'TODO Make a css style for this
                litCurrentPage.Text = String.Format("<label>Current Page</label><div class=""display-message""><a href=""/Home/{0}.html"" target=""_blank"" style=""background-color: #FFF8C2;"">/Home/{0}.html</a></div><br class=""clear-both""/><br />", CurrentPage.permalink)

                RefreshParentPages()

                Try
                    RemoveLinkedPages(CurrentPage.breadcrumbTitle, CurrentPage.id, ContentPageController.GetElement())
                Catch Ex As StackOverflowException
                    lblStatus.Text = "There was an error building the linked pages list."
                    lblStatus.Visible = True
                End Try

                If CurrentPage.parentPageID.HasValue AndAlso CurrentPage.parentPageID.HasValue AndAlso CurrentPage.parentPageID > 0 Then
                    ddlParentPage.SelectedValue = CurrentPage.parentPageID.ToString
                Else
                    ddlParentPage.SelectedIndex = 0
                End If
            Catch Ex As Exception
                lblStatus.Text = Ex.ToString
                lblStatus.Visible = True
            End Try
        End Sub

        Private Sub RemoveLinkedPages(ByVal PageTitle As String, ByVal PageId As Integer, ByVal Pages As List(Of ContentPage))
            ddlParentPage.Items.Remove(New ListItem(PageTitle, PageId.ToString))
            For Each item In Pages
                If item.parentPageID = PageId Then
                    RemoveLinkedPages(item.breadcrumbTitle, item.id, Pages)
                End If
            Next
        End Sub

        Private Sub SaveButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnSave.Click
            Dim PageId As Integer
            Dim ParentPageid As Integer

            If Not ddlParentPage.SelectedValue = Nothing Then
                ParentPageid = Integer.Parse(ddlParentPage.SelectedValue)
            Else
                ParentPageid = 0
            End If

            If Page.IsValid Then
                Dim CurrentSitemap As New CCFramework.Generic.SiteMapProvider

                If Integer.TryParse(hfPageId.Value, PageId) Then
                    ContentPageController.Update(PageId, txtContentTitle.Text, txtPageTitle.Text, txtBreadcrumbTitle.Text.Trim, txtPermalink.Text.Trim, txtKeywords.Text, txtDescription.Text, PageContentTextArea.InnerText, SecondaryPageContentTextArea.InnerText, Integer.Parse(ddlMenu.SelectedValue), Integer.Parse(txtMenuOrder.Text), ParentPageid, txtScript.Text, ckbSubMenu.Checked, ShowHeadingCheckBox.Checked, Integer.Parse(CallToActionDropDown.SelectedValue))

                    CurrentSitemap.UpdatePage(String.Format("/Home/{0}.html", hfPermalink.Value), String.Format("/Home/{0}.html", txtPermalink.Text), "monthly", 0.7, MyBase.Context)

                    lblStatus.Text = "Your page has been saved!"
                    lblStatus.Visible = True
                Else
                    PageId = ContentPageController.Create(txtContentTitle.Text, txtPageTitle.Text, txtBreadcrumbTitle.Text, txtPermalink.Text.Trim, txtKeywords.Text, txtDescription.Text, PageContentTextArea.InnerText, SecondaryPageContentTextArea.InnerText, Integer.Parse(ddlMenu.SelectedValue), Integer.Parse(txtMenuOrder.Text), ParentPageid, txtScript.Text, ckbSubMenu.Checked, ShowHeadingCheckBox.Checked, Integer.Parse(CallToActionDropDown.SelectedValue))

                    CurrentSitemap.AddNewPage(String.Format("/Home/{0}.html", txtPermalink.Text), "monthly", 0.7, MyBase.Context)

                    hfPageId.Value = PageId.ToString
                    pvPermalink.ItemID = PageId

                    lblStatus.Text = "Your page has been created!"
                    lblStatus.Visible = True
                End If

                RefreshDataSources()
                LoadPage(PageId)
            End If
        End Sub

        Private Sub NewButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles NewButton.Click
            PagesListPlaceHolder.Visible = False
            PageEditPlaceHolder.Visible = True

            BackButton.Enabled = True
            btnSave.Enabled = True
            btnDelete.Enabled = True

            ClearControls()
        End Sub

        Private Sub RefreshDataSources()
            ddlMenu.DataSource = MenuController.GetMenu()
            ddlMenu.DataBind()

            RefreshParentPages()

            PagesList.DataSource = ContentPageController.GetElement()
            PagesList.DataBind()

            CallToActionDropDown.Items.Clear()
            CallToActionDropDown.Items.Add(New ListItem("None", "-1"))
            CallToActionDropDown.AppendDataBoundItems = True
            CallToActionDropDown.DataSource = CallToActionController.GetItems
            CallToActionDropDown.DataBind()
        End Sub

        Private Sub RefreshParentPages()
            ddlParentPage.Items.Clear()
            ddlParentPage.Items.Add(New ListItem("Select", ""))
            ddlParentPage.AppendDataBoundItems = True
            ddlParentPage.DataSource = ContentPageController.GetElement()
            ddlParentPage.DataBind()
        End Sub

        Private Sub ClearControls()
            RefreshParentPages()

            hfPageId.Value = Nothing
            hfPermalink.Value = Nothing
            pvPermalink.ItemID = Nothing

            ShowHeadingCheckBox.Checked = True

            CallToActionDropDown.SelectedIndex = Nothing

            PageContentTextArea.InnerText = ""
            SecondaryPageContentTextArea.InnerText = ""

            txtContentTitle.Text = ""
            txtPageTitle.Text = ""
            txtKeywords.Text = ""
            txtDescription.Text = ""
            txtPermalink.Text = ""
            ddlMenu.SelectedIndex = 0
            ddlParentPage.SelectedIndex = 0
            txtMenuOrder.Text = "1"
            txtBreadcrumbTitle.Text = ""
            txtScript.Text = ""

            litCurrentPage.Visible = False
            litCurrentPage.Text = ""
        End Sub

        Private Sub DeleteButtonCommand(ByVal Sender As Object, ByVal E As CommandEventArgs) Handles btnDelete.Command
            If Not hfPageId.Value = Nothing Then
                Dim PageId As Integer = Integer.Parse(hfPageId.Value)

                If ContentPageController.Delete(PageId) = True Then
                    lblStatus.Text = "The selected page has been deleted permanently!"
                    lblStatus.Visible = True

                    RefreshDataSources()

                    ClearControls()

                    PagesListPlaceHolder.Visible = True
                    PageEditPlaceHolder.Visible = False
                    BackButton.Visible = False
                Else
                    lblStatus.Text = "An error occured while trying to delete the current page."
                    lblStatus.Visible = True
                End If
            Else
                lblStatus.Text = "Sorry but you do not have a page selected."
                lblStatus.Visible = True
            End If
        End Sub

        Private Sub PagesListPageIndexChanging(ByVal Sender As Object, ByVal E As GridViewPageEventArgs) Handles PagesList.PageIndexChanging
            PagesList.PageIndex = E.NewPageIndex

            RefreshDataSources()
        End Sub

        Protected Sub PagesListRowCommand(ByVal Sender As Object, ByVal E As GridViewCommandEventArgs) Handles PagesList.RowCommand
            Dim PageId As Integer

            PageId = Integer.Parse(E.CommandArgument.ToString)

            If E.CommandName = "DeletePage" Then

                If ContentPageController.Delete(PageId) = True Then
                    lblStatus.Text = "The selected page has been deleted permanently!"
                    lblStatus.Visible = True

                    RefreshDataSources()

                    ClearControls()
                Else
                    lblStatus.Text = "An error occured while trying to delete the current page."
                    lblStatus.Visible = True
                End If

            ElseIf E.CommandName = "SelectPage" Then
                PageId = Integer.Parse(E.CommandArgument.ToString)

                LoadPage(PageId)
            End If
        End Sub

        Private Sub BackButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles BackButton.Click
            PagesListPlaceHolder.Visible = True
            PageEditPlaceHolder.Visible = False

            ClearControls()
            RefreshDataSources()

            BackButton.Enabled = False
            btnSave.Enabled = False
            btnDelete.Enabled = False
        End Sub

    End Class

End Namespace