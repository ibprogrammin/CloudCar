Imports CloudCar.CCFramework.Model

Namespace CCAdmin.ContentManagement

    Partial Public Class Testimonials
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Private Sub LoadTestimonial(ByVal testimonialID As Integer)
            Try
                Dim testimonial As Testimonial = New CCFramework.ContentManagement.TestimonialController().GetElement(testimonialID)

                hfTestimonialID.Value = testimonial.ID.ToString
                txtAuthor.Text = testimonial.Author
                QuoteTextArea.InnerText = testimonial.Quote

                hfImageID.Value = testimonial.ImageID.ToString
                If Not testimonial.ImageID = 0 Then
                    imgTestimonialImage.Src = "/images/db/" & testimonial.ImageID & "/120/" & testimonial.Picture.PictureFileName
                    imgTestimonialImage.Alt = testimonial.Author & "'s portrait"
                    imgTestimonialImage.Visible = True

                    lblFileName.Text = "/images/db/" & testimonial.ImageID & "/full/" & testimonial.Picture.PictureFileName
                    lblFileName.Visible = True
                End If

            Catch ex As Exception
                lblStatus.Text = ex.ToString
                lblStatus.Visible = True
            End Try
        End Sub

        Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Dim TestimonialID As Integer

            If Page.IsValid Then
                If Integer.TryParse(hfTestimonialID.Value, TestimonialID) Then
                    Dim ImageID As Integer

                    If fuImage.HasFile Then
                        ImageID = CCFramework.Core.ImageFunctions.UploadImage(fuImage)
                    Else
                        ImageID = Integer.Parse(hfImageID.Value)
                    End If

                    Dim CurrentTestimonial As New CCFramework.ContentManagement.TestimonialController()
                    CurrentTestimonial.Update(TestimonialID, QuoteTextArea.InnerText, txtAuthor.Text, True, "Administrator", ImageID)

                    lblStatus.Text = "Your testimonial has been saved!"
                    lblStatus.Visible = True
                Else
                    If fuImage.HasFile Then
                        Dim ImageId As Integer

                        ImageId = CCFramework.Core.ImageFunctions.UploadImage(fuImage)

                        Dim CurrentTestimonial As New CCFramework.ContentManagement.TestimonialController()
                        CurrentTestimonial.Create(QuoteTextArea.InnerText, txtAuthor.Text, True, "Administrator", ImageId)

                        lblStatus.Text = "Your testimonial has been created!"
                        lblStatus.Visible = True

                        RefreshDataSources()
                    Else
                        lblStatus.Text = "You have not added an image to this testimonial"
                        lblStatus.Visible = False
                    End If
                End If
            End If
        End Sub

        Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
            ClearControls()
        End Sub

        Private Sub RefreshDataSources()
            lbTestimonials.Items.Clear()
            lbTestimonials.Items.Add(New ListItem("Select a Testimonial", ""))
            lbTestimonials.AppendDataBoundItems = True
            lbTestimonials.DataSource = New CCFramework.ContentManagement.TestimonialController().GetElements
            lbTestimonials.DataBind()
        End Sub

        Private Sub ClearControls()
            hfTestimonialID.Value = Nothing
            hfImageID.Value = Nothing

            txtAuthor.Text = ""
            QuoteTextArea.InnerText = ""

            imgTestimonialImage.Src = ""
            imgTestimonialImage.Alt = ""
            imgTestimonialImage.Visible = False

            lblFileName.Text = ""
            lblFileName.Visible = False
        End Sub

        Private Sub lbTestimonials_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTestimonials.SelectedIndexChanged
            If Not lbTestimonials.SelectedValue = Nothing Then
                Dim TestimonialID As Integer = Integer.Parse(lbTestimonials.SelectedValue)

                LoadTestimonial(TestimonialID)
            End If
        End Sub

        Private Sub btnDelete_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnDelete.Command
            If Not hfTestimonialID.Value = Nothing Then
                Dim TestimonialID As Integer = Integer.Parse(hfTestimonialID.Value)

                Dim testimonial As New CCFramework.ContentManagement.TestimonialController()
                If testimonial.Delete(TestimonialID) = True Then
                    lblStatus.Text = "The selected testimonial has been deleted permanently!"
                    lblStatus.Visible = True

                    RefreshDataSources()

                    ClearControls()
                Else
                    lblStatus.Text = "An error occured while trying to delete the current testimonial."
                    lblStatus.Visible = True
                End If
            Else
                lblStatus.Text = "Sorry but you do not have a testimonial selected."
                lblStatus.Visible = True
            End If
        End Sub

    End Class
End Namespace