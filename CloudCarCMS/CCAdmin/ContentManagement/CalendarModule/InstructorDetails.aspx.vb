Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCAdmin.ContentManagement.CalendarModule

    Partial Public Class InstructorDetails
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                hlNewProgram.NavigateUrl = hlNewProgram.NavigateUrl & "?Program=NEW"

                Dim InstructorId As Integer

                If Integer.TryParse(Request.QueryString("Instructor"), InstructorId) Then
                    LoadInstructor(InstructorId)
                End If
            End If
        End Sub

        Private Sub LoadInstructor(ByVal InstructorId As Integer)
            Dim CurrentInstructorController As New CCFramework.ContentManagement.CalendarModule.InstructorController

            Dim CurrentInstructor As Instructor = CurrentInstructorController.GetElement(InstructorId)

            litInstructorId.Text = CurrentInstructor.Id.ToString
            hfInstructorID.Value = CurrentInstructor.Id.ToString

            hfProfileImage.Value = CurrentInstructor.ProfileImageId.Value.ToString

            If CurrentInstructor.ProfileImageId.HasValue AndAlso Not CurrentInstructor.ProfileImageId.Value = 0 Then
                Try
                    lblProfileImageLocation.Text = "/images/db/" & CurrentInstructor.ProfileImageId & "/full/" & PictureController.GetPicture(CurrentInstructor.ProfileImageId.Value).PictureFileName
                    lblProfileImageLocation.Visible = True

                    imgProfileImage.Src = "/images/db/" & CurrentInstructor.ProfileImageId & "/220/" & CCFramework.Core.PictureController.GetPicture(CurrentInstructor.ProfileImageId.Value).PictureFileName
                    imgProfileImage.Visible = True
                Catch Ex As Exception

                End Try
            End If

            hlNewProgram.NavigateUrl = hlNewProgram.NavigateUrl & "?Instructor=" & CurrentInstructor.Id

            txtName.Text = CurrentInstructor.Name
            BiographyTextArea.InnerText = CurrentInstructor.Bio
            txtSpecialty.Text = CurrentInstructor.Specialty

            txtPermalink.Text = CurrentInstructor.Permalink
            txtKeywords.Text = CurrentInstructor.Keywords
            txtDescription.Text = CurrentInstructor.Description
            txtBrowserTitle.Text = CurrentInstructor.BrowserTitle

            LoadPrograms(InstructorId)

            phProperties.Visible = True
        End Sub

        Private Sub SaveInstructor()
            Dim Name As String = txtName.Text
            Dim Bio As String = BiographyTextArea.InnerText
            Dim Specialty As String = txtSpecialty.Text
            Dim Permalink As String = txtPermalink.Text
            Dim Keywords As String = txtKeywords.Text
            Dim Description As String = txtDescription.Text
            Dim BrowserTitle As String = txtBrowserTitle.Text

            Dim ProfileImageId As Integer
            If fuProfileImage.HasFile Then
                ProfileImageId = CCFramework.Core.ImageFunctions.UploadImage(fuProfileImage)
            Else
                ProfileImageId = 0
            End If

            Dim CurrentInstructorController As New CCFramework.ContentManagement.CalendarModule.InstructorController

            Dim InstructorId As Integer

            If Not hfInstructorID.Value Is Nothing And Integer.TryParse(hfInstructorID.Value, InstructorId) Then
                CurrentInstructorController.Update(InstructorId, Name, Specialty, Bio, ProfileImageId, Permalink, Keywords, Description, BrowserTitle)
            Else
                InstructorId = CurrentInstructorController.Create(Name, Specialty, Bio, ProfileImageId, Permalink, Keywords, Description, BrowserTitle)
            End If

            lblMessage.Text = String.Format("Instructor saved successfully! Instructor - {0} - was commited", Name)
            lblMessage.Visible = True

            LoadInstructor(InstructorId)
        End Sub

        'TODO Remove Instructor from InstructorPrograms table
        Private Sub DeleteInstructor(ByVal InstructorId As Integer)
            Dim CurrentInstructorController As New CCFramework.ContentManagement.CalendarModule.InstructorController

            'CalendarModule.ProgramController.RemoveInstructors(ProgramId)
            'CalendarModule.ProgramController.RemoveSchedules(ProgramId)

            If CurrentInstructorController.Delete(InstructorId) Then
                ClearForm()

                lblMessage.Text = "The selected instructor has been successfully deleted."
                lblMessage.Visible = True
            End If
        End Sub

        Private Sub LoadPrograms(ByVal InstructorId As Integer)
            Dim CurrentProgramController As New CCFramework.ContentManagement.CalendarModule.ProgramController

            Dim AvailablePrograms As List(Of Program) = CurrentProgramController.GetElements()
            Dim InstructorPrograms As List(Of Program) = CCFramework.ContentManagement.CalendarModule.InstructorController.GetInstructorPrograms(InstructorId)

            For Each Item In InstructorPrograms
                Dim CurrentProgramId As Integer = Item.Id

                AvailablePrograms.RemoveAll(Function(i) i.Id = CurrentProgramId)
            Next

            ddlAddProgram.Items.Clear()
            ddlAddProgram.Items.Add(New ListItem("Please Select...", ""))
            ddlAddProgram.DataBind()
            ddlAddProgram.DataSource = AvailablePrograms
            ddlAddProgram.DataBind()

            dtgInstructorPrograms.DataSource = InstructorPrograms
            dtgInstructorPrograms.DataBind()
        End Sub

        Private Sub SaveBottomButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnSaveBottom.Click
            SaveInstructor()
        End Sub

        Private Sub SaveTopButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnSaveTop.Click
            SaveInstructor()
        End Sub

        Private Sub DeleteButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnDelete.Click
            Dim InstructorId As Integer = Integer.Parse(hfInstructorID.Value)

            DeleteInstructor(InstructorId)
        End Sub

        Private Sub AddProgramButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnAddProgram.Click
            Dim InstructorId As Integer = Integer.Parse(hfInstructorID.Value)
            Dim ProgramId As Integer = Integer.Parse(ddlAddProgram.SelectedValue)

            CCFramework.ContentManagement.CalendarModule.ProgramController.AddInstructor(ProgramId, InstructorId)

            lblMessage.Text = "The selected program has been successfully associated with the instructor"
            lblMessage.Visible = True

            LoadPrograms(InstructorId)
        End Sub

        Private Sub ClearForm()
            hlNewProgram.NavigateUrl = hlNewProgram.NavigateUrl & "?Instructor=NEW"

            litInstructorId.Text = "New"
            hfInstructorID.Value = String.Empty
            hfProfileImage.Value = String.Empty

            txtName.Text = ""
            BiographyTextArea.InnerText = ""
            txtSpecialty.Text = ""
            txtPermalink.Text = ""
            txtKeywords.Text = ""
            txtDescription.Text = ""
            txtBrowserTitle.Text = ""

            lblProfileImageLocation.Text = ""
            lblProfileImageLocation.Visible = False

            ddlAddProgram.SelectedIndex = 0
            ddlAddProgram.DataSource = Nothing

            dtgInstructorPrograms.DataSource = Nothing

            phProperties.Visible = False
        End Sub

        Protected Sub InstructorsDataGridDeleteCommand(ByVal Sender As Object, ByVal E As CommandEventArgs) Handles dtgInstructorPrograms.DeleteCommand
            Dim InstructorId As Integer = Integer.Parse(hfInstructorID.Value)
            Dim ProgramId As Integer = Integer.Parse(E.CommandArgument.ToString)

            CCFramework.ContentManagement.CalendarModule.ProgramController.RemoveInstructor(ProgramId, InstructorId)

            LoadPrograms(InstructorId)
        End Sub

    End Class
End Namespace