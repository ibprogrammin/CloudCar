Imports CloudCar.CCFramework.Blogging
Imports CloudCar.CCFramework.Model

Namespace CCAdmin.Blogging

    Partial Public Class Blogs
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                PostsListPlaceHolder.Visible = True
                RefreshDataSources()
            End If
        End Sub

        Private Sub LoadBlog(ByVal BlogId As Integer)
            Try
                Dim CurrentBlog As Blog = BlogController.GetBlog(BlogId)

                PostsListPlaceHolder.Visible = False
                PostEditPlaceHolder.Visible = True

                BackButton.Enabled = True
                SaveButton.Enabled = True
                DeleteButton.Enabled = True

                hfBlogID.Value = CurrentBlog.id.ToString
                pvPermalink.ItemID = CurrentBlog.id

                BlogTitleTextBox.Text = CurrentBlog.Title
                BlogContentTextArea.InnerText = CurrentBlog.BlogContent
                BlogSummaryTextBox.Text = CurrentBlog.ContentSummary
                BlogCategoryTextBox.Text = CurrentBlog.Category
                BlogSubHeadingTextBox.Text = CurrentBlog.SubHeading
                BlogHeadingImageTextBox.Text = CurrentBlog.ImageLink
                BlogThumbnailImageTextBox.Text = CurrentBlog.ThumnailImageLink

                BlogKeywordsTextBox.Text = CurrentBlog.Keywords
                BlogDescriptionTextBox.Text = CurrentBlog.Description
                BlogPermalinkTextBox.Text = CurrentBlog.Permalink

                BlogAuthorDropDown.SelectedValue = CurrentBlog.AuthorID.ToString

                litCurrentPage.Visible = True
                litCurrentPage.Text = String.Format("<label>Current Post</label><div class=""display-message"" style=""width: 667px;""><a href=""/Blog/{0}.html"" target=""_blank"" style=""background-color: #FFF8C2;"">/Blog/{0}.html</a></div><br class=""clear-both"" /><br />", CurrentBlog.Permalink)

                BlogSetLiveCheckBox.Checked = CurrentBlog.Live
            Catch Ex As BlogController.InvalidBlogException
                lblStatus.Text = "Uh-oh! An invalid blog entry has been selected. Please try again."
                lblStatus.Visible = True
            Catch Ex As Exception
                lblStatus.Text = Ex.ToString
                lblStatus.Visible = True
            End Try
        End Sub

        Private Sub SaveButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles SaveButton.Click
            Dim BlogId As Integer

            If Page.IsValid Then
                Dim AuthorId As Integer = Integer.Parse(BlogAuthorDropDown.SelectedValue)
                Dim BlogTitle As String = BlogTitleTextBox.Text.Trim
                Dim SubHeading As String = BlogSubHeadingTextBox.Text.Trim
                Dim Content As String = BlogContentTextArea.InnerText
                Dim Permalink As String = BlogPermalinkTextBox.Text.Trim
                Dim Keywords As String = BlogKeywordsTextBox.Text
                Dim Description As String = BlogDescriptionTextBox.Text
                Dim ContentSummary As String = BlogSummaryTextBox.Text.Trim
                Dim Category As String = BlogCategoryTextBox.Text.Trim
                Dim HeadingImage As String = BlogHeadingImageTextBox.Text
                Dim ThumbnailImage As String = BlogThumbnailImageTextBox.Text
                Dim MakeLive As Boolean = BlogSetLiveCheckBox.Checked

                If Integer.TryParse(hfBlogID.Value, BlogId) Then
                    BlogController.UpdateBlog(BlogId, AuthorId, BlogTitle, SubHeading, Content, Permalink, Keywords, Description, ContentSummary, Category, HeadingImage, ThumbnailImage, MakeLive)

                    lblStatus.Text = "Your blog entry has been saved!"
                    lblStatus.Visible = True
                Else
                    hfBlogID.Value = BlogController.CreateBlog(AuthorId, BlogTitle, SubHeading, Content, Permalink, Keywords, Description, ContentSummary, Category, HeadingImage, ThumbnailImage, MakeLive).ToString

                    UpdateRssFeed()

                    lblStatus.Text = "Your blog entry has been created!"
                    lblStatus.Visible = True

                    RefreshDataSources()
                End If
            End If
        End Sub

        Private Sub NewButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles NewButton.Click
            PostsListPlaceHolder.Visible = False
            PostEditPlaceHolder.Visible = True

            BackButton.Enabled = True
            SaveButton.Enabled = True
            DeleteButton.Enabled = True

            ClearControls()
        End Sub

        Private Sub RefreshDataSources()
            PostsList.DataSource = BlogController.GetBlog
            PostsList.DataBind()

            BlogAuthorDropDown.Items.Clear()
            BlogAuthorDropDown.Items.Add(New ListItem("Select", ""))
            BlogAuthorDropDown.AppendDataBoundItems = True
            BlogAuthorDropDown.DataSource = AuthorController.GetElements
            BlogAuthorDropDown.DataBind()
        End Sub

        Private Sub ClearControls()
            hfBlogID.Value = Nothing
            pvPermalink.ItemID = Nothing
            BlogTitleTextBox.Text = ""
            BlogSubHeadingTextBox.Text = ""
            BlogSummaryTextBox.Text = ""
            BlogContentTextArea.InnerText = ""
            BlogKeywordsTextBox.Text = ""
            BlogDescriptionTextBox.Text = ""
            BlogPermalinkTextBox.Text = ""
            BlogCategoryTextBox.Text = ""
            BlogHeadingImageTextBox.Text = ""
            BlogThumbnailImageTextBox.Text = ""
            BlogSetLiveCheckBox.Checked = False

            BlogAuthorDropDown.SelectedValue = Nothing

            litCurrentPage.Visible = False
            litCurrentPage.Text = ""
        End Sub

        Private Function UpdateRssFeed() As Boolean
            Try
                Dim rss As New CCFramework.Blogging.RssTwoPointZero.RssChannel( _
                    CType(CCFramework.Core.Settings.RssChannelTitle, String), _
                    CType(CCFramework.Core.Settings.RssChannelLink, String), _
                    CType(CCFramework.Core.Settings.RssChannelDescription, String), _
                    CType(CCFramework.Core.Settings.RssChannelWebMaster, String), _
                    CType(CCFramework.Core.Settings.RssChannelManagingEditor, String), _
                    CType(CCFramework.Core.Settings.RssChannelTtl, String), _
                    CType(CCFramework.Core.Settings.RssChannelLanguage, String), _
                    CType(CCFramework.Core.Settings.RssChannelCopyright, String), _
                    CType(CCFramework.Core.Settings.RssChannelCategory, String))

                Dim rssImage As New CCFramework.Blogging.RssTwoPointZero.RssImage( _
                    CType(CCFramework.Core.Settings.RssItemTitle, String), _
                    CType(CCFramework.Core.Settings.RssItemLink, String), _
                    CType(CCFramework.Core.Settings.RssItemLogoUrl, String))

                rss.Image = rssImage

                'Dim db As New BlogDataContext

                rss.ReadLinqItems(CCFramework.Blogging.BlogController.GetLiveBlogs, CType(CCFramework.Core.Settings.RssLink, String))

                Dim filename As String = Server.MapPath(CType(CCFramework.Core.Settings.RssXmlFile, String))
                'Dim file As System.IO.FileInfo = New System.IO.FileInfo(filename)
                'If file.Exists Then
                'file.Delete()
                'End If

                rss.WriteToXml(filename)

                Return True
            Catch ex As Exception
                'Response.Write(ex.Message)
                'Response.Write(ex.ToString)
                lblStatus.Text = ex.Message.ToString
                lblStatus.Visible = True

                Return False
            End Try
        End Function

        Private Sub DeleteButtonCommand(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles DeleteButton.Command
            If Not hfBlogID.Value = Nothing Then
                Dim BlogID As Integer = Integer.Parse(hfBlogID.Value)

                CommentController.DeleteBlogComments(BlogID)

                If BlogController.DeleteBlog(BlogID) = True Then
                    UpdateRssFeed()

                    lblStatus.Text = "The selected entry has been deleted permanently!"
                    lblStatus.Visible = True

                    RefreshDataSources()

                    ClearControls()
                Else
                    lblStatus.Text = "An error occured while trying to delete the current blog entry."
                    lblStatus.Visible = True
                End If
            Else
                lblStatus.Text = "Sorry but you do not have a blog selected."
                lblStatus.Visible = True
            End If
        End Sub

        Private Sub BindBlogCommentsGridView(ByVal blogId As Integer)

            Dim db As New BlogDataContext

            Dim data = From comments In db.Comments _
                    Where comments.BlogId = blogId _
                    Select Name = comments.Name, Email = comments.Email, Comment = comments.Comment, Approved = comments.Approved, CommentId = comments.id

            gvwCommentsDetails.DataSource = data
            gvwCommentsDetails.DataBind()

        End Sub

        Protected Sub ApprovedCheckBoxCheckChanged(ByVal Sender1 As Object, ByVal e As EventArgs)

            Dim ckb As CheckBox = CType(Sender1, CheckBox)

            Dim commentID As Integer = Integer.Parse(ckb.Attributes("CommentID"))

            Dim db As New BlogDataContext

            Dim comment = db.Comments.First(Function(c) c.id = commentID)
            comment.Approved = ckb.Checked

            db.SubmitChanges()

        End Sub

        Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs)

        End Sub

        Private Sub PagesListPageIndexChanging(ByVal Sender As Object, ByVal E As GridViewPageEventArgs) Handles PostsList.PageIndexChanging
            PostsList.PageIndex = E.NewPageIndex

            RefreshDataSources()
        End Sub

        Protected Sub PostsListRowCommand(ByVal Sender As Object, ByVal E As GridViewCommandEventArgs) Handles PostsList.RowCommand
            Dim PostId As Integer

            PostId = Integer.Parse(E.CommandArgument.ToString)

            If E.CommandName = "DeletePost" Then

                If BlogController.DeleteBlog(PostId) = True Then
                    lblStatus.Text = "The selected blog post has been deleted permanently!"
                    lblStatus.Visible = True

                    RefreshDataSources()

                    ClearControls()
                Else
                    lblStatus.Text = "An error occured while trying to delete the current blog post."
                    lblStatus.Visible = True
                End If

            ElseIf E.CommandName = "SelectPost" Then
                PostId = Integer.Parse(E.CommandArgument.ToString)

                LoadBlog(PostId)
                BindBlogCommentsGridView(PostId)
            End If
        End Sub

        Private Sub BackButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles BackButton.Click
            PostsListPlaceHolder.Visible = True
            PostEditPlaceHolder.Visible = False

            ClearControls()
            RefreshDataSources()

            BackButton.Enabled = False
            SaveButton.Enabled = False
            DeleteButton.Enabled = False
        End Sub

    End Class

End Namespace