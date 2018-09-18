Imports CloudCar.CCFramework.Model

Namespace CCAdmin.ContentManagement

    Partial Public Class UploadFiles
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadFiles()
            End If
        End Sub

        Private Sub btnAdd_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAdd.Command
            If Page.IsValid Then
                Dim fileID As Integer
                Dim title As String = txtTitle.Text
                Dim description As String = txtDescription.Text
                Dim enabled As Boolean = ckbEnabled.Checked
                Dim filename As String

                If Integer.TryParse(hfFileID.Value, fileID) Then
                    If Not fuFile.PostedFile.FileName = "" Then
                        filename = fuFile.FileName

                        fuFile.PostedFile.SaveAs(Server.MapPath(CCFramework.Core.Settings.FileUploadPath & filename))

                        CCFramework.ContentManagement.FileUploadController.Update(fileID, filename, CCFramework.Core.Settings.FileUploadPath, title, description, enabled)

                        lblStatus.Text = "The file has been successfully uploaded!"

                        Clear()
                    Else
                        filename = hfFile.Value
                        Dim path As String = hfPath.Value

                        CCFramework.ContentManagement.FileUploadController.Update(fileID, filename, path, title, description, enabled)

                        lblStatus.Text = "The file has been successfully uploaded!"

                        Clear()
                    End If
                Else
                    If Not fuFile.PostedFile.FileName = "" Then
                        filename = fuFile.FileName

                        fuFile.PostedFile.SaveAs(Server.MapPath(CCFramework.Core.Settings.FileUploadPath & filename))

                        CCFramework.ContentManagement.FileUploadController.Create(filename, CCFramework.Core.Settings.FileUploadPath, title, description, enabled)

                        lblStatus.Text = "The file has been successfully uploaded!"

                        Clear()
                    Else
                        lblStatus.Text = "There was an error uploading the file!"
                    End If
                End If

                lblStatus.Visible = True
                LoadFiles()
            End If
        End Sub

        Private Sub btnCancel_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnCancel.Command
            Clear()
        End Sub

        Protected Sub btnSelect_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            Dim file As FileUpload = CCFramework.ContentManagement.FileUploadController.GetElement(id)

            hfFileID.Value = file.ID.ToString
            hfFile.Value = file.Filename
            hfPath.Value = file.Path

            txtTitle.Text = file.Title
            txtDescription.Text = file.Description

            lblFilename.Text = file.Path & file.Filename
            lblFilename.Visible = True

            ckbEnabled.checked = file.Enabled
        End Sub

        Protected Sub btnDelete_Command(ByVal sender As Object, ByVal e As CommandEventArgs)
            Dim id As Integer = Integer.Parse(e.CommandArgument.ToString)

            If CCFramework.ContentManagement.FileUploadController.Delete(id) Then
                lblStatus.Text = "The file was successfully deleted!"
                lblStatus.Visible = True

                LoadFiles()
            Else
                lblStatus.Text = "There was an error deleting the file!"
                lblStatus.Visible = True
            End If
        End Sub

        Private Sub dgUploadedFiles_PageIndexChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgUploadedFiles.PageIndexChanged
            dgUploadedFiles.CurrentPageIndex = e.NewPageIndex

            LoadFiles()
        End Sub

        Private Sub LoadFiles()
            dgUploadedFiles.DataSource = CCFramework.ContentManagement.FileUploadController.GetElement.ToList
            dgUploadedFiles.DataBind()
        End Sub

        Private Sub Clear()
            hfFileID.Value = Nothing
            hfFile.Value = Nothing
            hfPath.Value = Nothing

            txtTitle.Text = ""
            txtDescription.Text = ""

            lblFilename.Text = ""
            lblFilename.Visible = False

            ckbEnabled.Checked = False
        End Sub

    End Class
End Namespace