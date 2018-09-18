Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement

Namespace CCAdmin.ContentManagement

    Partial Public Class Videos
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub LoadVideo(ByVal VideoId As Integer)
            Try
                Dim CurrentVideo As Video = VideoController.GetElement(VideoID)

                hfVideoID.Value = CurrentVideo.ID.ToString

                txtTitle.Text = CurrentVideo.Title
                DetailsTextArea.InnerText = CurrentVideo.Details
                txtDescription.Text = CurrentVideo.Description
                txtKeywords.Text = CurrentVideo.Keywords
                txtVideoID.Text = CurrentVideo.VideoID

                ddlPlayerType.SelectedValue = CurrentVideo.Player.ToString

                'litCurrentPage.Visible = True
                'litCurrentPage.Text = "<label>Current Page</label><div class=""display-message"" style=""width: 650px;""><a href=""/Home/" & Page.permalink & ".html"" target=""_blank"" style=""background-color: #FFF8C2;"">/Home/" & Page.permalink & ".html</a></div><br style=""clear: both;""/><br />"
            Catch ex As Exception
                lblStatus.Text = ex.ToString
                lblStatus.Visible = True
            End Try
        End Sub

        Private Sub btnSave_Click(ByVal Sender As Object, ByVal Args As EventArgs) Handles btnSave.Click
            Dim VideoId As Integer

            If Page.IsValid Then
                Dim PlayerType As SMVideoType = CType(Integer.Parse(ddlPlayerType.SelectedValue), SMVideoType)

                If Integer.TryParse(hfVideoID.Value, VideoId) Then
                    VideoController.Update(VideoId, txtTitle.Text, txtVideoID.Text, PlayerType, DetailsTextArea.InnerText, txtDescription.Text, txtKeywords.Text)

                    lblStatus.Text = "Your video has been saved!"
                    lblStatus.Visible = True
                Else
                    VideoController.Create(txtTitle.Text, txtVideoID.Text, PlayerType, DetailsTextArea.InnerText, txtDescription.Text, txtKeywords.Text)

                    lblStatus.Text = "Your video has been created!"
                    lblStatus.Visible = True
                End If

                RefreshDataSources()
            End If
        End Sub

        Private Sub btnClear_Click(ByVal Sender As Object, ByVal Args As EventArgs) Handles btnClear.Click
            ClearControls()
        End Sub

        Private Sub RefreshDataSources()
            lbVideos.Items.Clear()
            lbVideos.Items.Add(New ListItem("To edit select a video from this list", ""))
            lbVideos.AppendDataBoundItems = True
            lbVideos.DataSource = VideoController.GetElements()
            lbVideos.DataBind()
        End Sub

        Private Sub ClearControls()
            hfVideoID.Value = Nothing

            txtTitle.Text = ""
            txtVideoID.Text = ""
            DetailsTextArea.InnerText = ""
            txtKeywords.Text = ""
            txtDescription.Text = ""

            ddlPlayerType.SelectedValue = Nothing

            'litCurrentPage.Visible = False
            'litCurrentPage.Text = ""
        End Sub

        Private Sub lbVideos_SelectedIndexChanged(ByVal Sender As Object, ByVal Args As EventArgs) Handles lbVideos.SelectedIndexChanged
            If Not lbVideos.SelectedValue = Nothing Then
                Dim VideoId As Integer = Integer.Parse(lbVideos.SelectedValue)

                'RefreshDataSources()
                LoadVideo(VideoId)
            End If
        End Sub

        Private Sub btnDelete_Command(ByVal Sender As Object, ByVal Args As CommandEventArgs) Handles btnDelete.Command
            If Not hfVideoID.Value = Nothing Then
                Dim VideoId As Integer = Integer.Parse(hfVideoID.Value)

                If VideoController.Delete(VideoId) = True Then
                    lblStatus.Text = "The selected video has been deleted permanently!"
                    lblStatus.Visible = True

                    RefreshDataSources()

                    ClearControls()
                Else
                    lblStatus.Text = "An error occured while trying to delete the current video."
                    lblStatus.Visible = True
                End If
            Else
                lblStatus.Text = "Sorry but you do not have a video selected."
                lblStatus.Visible = True
            End If
        End Sub

    End Class

End Namespace