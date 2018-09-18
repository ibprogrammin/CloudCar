Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement
Imports CloudCar.CCFramework.Core

Namespace CCAdmin.ContentManagement

    Partial Public Class Links
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            lblStatus.Text = ""
        End Sub

        Private Sub btnAddLink_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAddLink.Command
            phAddLink.Visible = True
            gvLinks.Visible = False
            btnAddLink.Visible = False
        End Sub

        Private Sub btnAdd_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnAdd.Command
            Dim imageId As Integer

            If Not fuPicture.PostedFile.FileName = "" Then
                Dim filename As String = fuPicture.FileName
                Dim imgType As String = fuPicture.PostedFile.ContentType
                Dim imgLength As Integer = CInt(fuPicture.PostedFile.InputStream.Length)
                Dim data(imgLength) As Byte
                fuPicture.PostedFile.InputStream.Read(data, 0, imgLength)

                imageId = PictureController.CreatePicture(filename, imgType, imgLength, data)
            End If

            If Not hfID.Value = "" Then
                Dim ID As Integer = Integer.Parse(hfID.Value)

                If Not imageId = 0 Then
                    LinksController.UpdateLink(ID, txtTitle.Text, txtURL.Text, DescriptionTextArea.InnerText, imageId)
                Else
                    LinksController.UpdateLink(ID, txtTitle.Text, txtURL.Text, DescriptionTextArea.InnerText)
                End If

                lblStatus.Text = "Link has been successfully updated."
                lblStatus.Visible = True
            Else
                If Not imageId = 0 Then
                    LinksController.CreateLink(txtTitle.Text, txtURL.Text, DescriptionTextArea.InnerText, imageId)
                Else
                    LinksController.CreateLink(txtTitle.Text, txtURL.Text, DescriptionTextArea.InnerText)
                End If

                lblStatus.Text = "Link has been successfully created."
                lblStatus.Visible = True
            End If

            gvLinks.DataBind()

            phAddLink.Visible = False
            gvLinks.Visible = True
            btnAddLink.Visible = True

            hfID.Value = ""
        End Sub

        Private Sub btnCancel_Command(ByVal sender As Object, ByVal e As CommandEventArgs) Handles btnCancel.Command
            phAddLink.Visible = False
            gvLinks.Visible = True
            btnAddLink.Visible = True

            hfID.Value = ""
            DescriptionTextArea.InnerText = ""
            txtTitle.Text = ""
            txtURL.Text = ""
        End Sub

        Protected Sub gvLinks_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvLinks.RowCommand
            If e.CommandName = "DeleteLink" Then
                Dim linkId As Integer = Integer.Parse(e.CommandArgument.ToString)

                If LinksController.DeleteLink(linkId) Then
                    lblStatus.Text = "Link has been successfully deleted."
                    lblStatus.Visible = True
                End If

                gvLinks.DataBind()
            ElseIf e.CommandName = "SelectLink" Then
                Dim ID As Integer = Integer.Parse(e.CommandArgument.ToString)

                Dim selectedLink As Link = LinksController.GetLink(ID)

                With selectedLink
                    hfID.Value = ID.ToString
                    txtTitle.Text = .LinksTitle
                    DescriptionTextArea.InnerText = .LinkNotes
                    txtURL.Text = .LinkURL

                    If Not .PictureID = 0 Then
                        lblFileName.Text = "/images/db/" & .PictureID & "/full/" & .Picture.PictureFileName
                        imgLinkImage.Src = "/images/db/" & .PictureID & "/900/" & .Picture.PictureFileName

                        lblFileName.Visible = True
                        imgLinkImage.Visible = True
                    End If
                End With

                gvLinks.Visible = False
                phAddLink.Visible = True
                btnAddLink.Visible = False
            End If
        End Sub

    End Class
End Namespace