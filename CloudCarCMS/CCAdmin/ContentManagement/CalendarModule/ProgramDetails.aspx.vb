Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCAdmin.ContentManagement.CalendarModule

    Partial Public Class ProgramDetails
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                hlNewInstructor.NavigateUrl = hlNewInstructor.NavigateUrl & "?Program=NEW"
                hlAddSchedule.NavigateUrl = hlAddSchedule.NavigateUrl & "?Program=NEW"

                Dim ProgramId As Integer

                If Integer.TryParse(Request.QueryString("Program"), ProgramId) Then
                    LoadProgram(ProgramId)
                End If
            End If
        End Sub

        Private Sub LoadProgram(ByVal ProgramId As Integer)
            Dim CurrentProgramController As New CCFramework.ContentManagement.CalendarModule.ProgramController

            Dim CurrentProgram As Program = CurrentProgramController.GetElement(ProgramId)

            litProgramId.Text = CurrentProgram.Id.ToString
            hfProgramId.Value = CurrentProgram.Id.ToString

            litProgramLink.Visible = True
            litProgramLink.Text = "<label>Program Link</label><a href=""/Calendar/Program/" & CurrentProgram.Permalink & ".html"" target=""_blank"" style=""background-color: #FFF8C2; width: 650px; float: left; position: relative; top: 10px;"">/Calendar/Program/" & CurrentProgram.Permalink & ".html</a><br style=""clear: both;"" /><br /><br />"

            hfIconImage.Value = CurrentProgram.IconImage

            If Not CurrentProgram.IconImage = "0" Then
                Try
                    lblIconImageLocation.Text = PictureController.GetPictureLink(Integer.Parse(CurrentProgram.IconImage))
                    lblIconImageLocation.Visible = True

                    imgIconImage.Src = PictureController.GetPictureLink(Integer.Parse(CurrentProgram.IconImage), 220) '"/images/db/" & CurrentProgram.IconImage & "/220/" & SMCore.PictureController.GetPicture(CurrentProgram.IconImage).PictureFileName
                    imgIconImage.Visible = True
                Catch Ex As Exception

                End Try
            End If

            hlNewInstructor.NavigateUrl = hlNewInstructor.NavigateUrl & "?Program=" & CurrentProgram.Id
            hlAddSchedule.NavigateUrl = hlAddSchedule.NavigateUrl & "?Program=" & CurrentProgram.Id

            txtName.Text = CurrentProgram.Name
            ContentTextArea.InnerText = CurrentProgram.Content

            txtDescription.Text = CurrentProgram.Description
            txtKeywords.Text = CurrentProgram.Keywords
            txtPermalink.Text = CurrentProgram.Permalink
            txtBrowserTitle.Text = CurrentProgram.PageTitle

            phProperties.Visible = True

            LoadInstructors(ProgramId)
            LoadSchedules(ProgramId)
        End Sub

        Private Sub SaveProgram()
            Dim Name As String = txtName.Text
            Dim [Content] As String = ContentTextArea.InnerText

            Dim PageTitle As String = txtBrowserTitle.Text
            Dim Keywords As String = txtKeywords.Text
            Dim Description As String = txtDescription.Text
            Dim Permalink As String = txtPermalink.Text

            Dim IconImage As Integer
            If fuIconImageImage.HasFile Then
                IconImage = CCFramework.Core.ImageFunctions.UploadImage(fuIconImageImage)
            Else
                If Not Integer.TryParse(hfIconImage.Value, IconImage) Then
                    IconImage = 0
                End If
            End If

            Dim CurrentProgramController As New CCFramework.ContentManagement.CalendarModule.ProgramController

            Dim ProgramId As Integer

            If Not hfProgramId.Value Is Nothing And Integer.TryParse(hfProgramId.Value, ProgramId) Then
                CurrentProgramController.Update(ProgramId, Name, Content, IconImage.ToString, Permalink, PageTitle, Keywords, Description)
            Else
                ProgramId = CurrentProgramController.Create(Name, Content, IconImage.ToString, Permalink, PageTitle, Keywords, Description)
            End If

            lblMessage.Text = "Program saved successfully! Program - " & Name & " - was commited"
            lblMessage.Visible = True

            LoadProgram(ProgramId)
        End Sub

        Private Sub DeleteProgram(ByVal ProgramId As Integer)
            Dim CurrentProgramController As New CCFramework.ContentManagement.CalendarModule.ProgramController

            CCFramework.ContentManagement.CalendarModule.ProgramController.RemoveInstructors(ProgramId)
            CCFramework.ContentManagement.CalendarModule.ProgramController.RemoveSchedules(ProgramId)

            If CurrentProgramController.Delete(ProgramId) Then
                ClearForm()

                lblMessage.Text = "The selected program has been successfully deleted."
                lblMessage.Visible = True
            End If
        End Sub

        Private Sub LoadInstructors(ByVal ProgramId As Integer)
            Dim CurrentInstructorController As New CCFramework.ContentManagement.CalendarModule.InstructorController

            Dim AvailableInstructors As List(Of Instructor) = CurrentInstructorController.GetElements()
            Dim ProgramInstructors As List(Of Instructor) = CCFramework.ContentManagement.CalendarModule.InstructorController.GetProgramInstructors(ProgramId)

            For Each Item In ProgramInstructors
                Dim CurrentInstructorId As Integer = Item.Id

                AvailableInstructors.RemoveAll(Function(i) i.Id = CurrentInstructorId)
            Next

            ddlAddInstructor.Items.Clear()
            ddlAddInstructor.Items.Add(New ListItem("Please Select...", ""))
            ddlAddInstructor.DataBind()
            ddlAddInstructor.DataSource = AvailableInstructors
            ddlAddInstructor.DataBind()

            dtgInstructors.DataSource = ProgramInstructors
            dtgInstructors.DataBind()
        End Sub

        Private Sub LoadSchedules(ByVal ProgramId As Integer)
            Dim ProgramSchedules As List(Of Schedule) = CCFramework.ContentManagement.CalendarModule.ScheduleController.GetFutureProgramSchedules(ProgramId).Take(10).ToList()

            dtgLatestSchedule.DataSource = ProgramSchedules
            dtgLatestSchedule.DataBind()
        End Sub

        Private Sub SaveButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnSave.Click
            SaveProgram()
        End Sub

        Private Sub DeleteButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnDelete.Click
            Dim ProgramId As Integer = Integer.Parse(hfProgramId.Value)

            DeleteProgram(ProgramId)
        End Sub

        Private Sub AddInstructorButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles btnAddInstructor.Click
            If Not ddlAddInstructor.SelectedValue Is Nothing And Not ddlAddInstructor.SelectedValue = "" Then
                Dim ProgramId As Integer = Integer.Parse(hfProgramId.Value)
                Dim InstructorId As Integer = Integer.Parse(ddlAddInstructor.SelectedValue)

                CCFramework.ContentManagement.CalendarModule.ProgramController.AddInstructor(ProgramId, InstructorId)

                lblInstructorMessage.Text = "The selected instructor has been successfully added to the program"
                lblInstructorMessage.Visible = True

                LoadInstructors(ProgramId)
            End If
        End Sub

        Private Sub ClearForm()
            hlNewInstructor.NavigateUrl = hlNewInstructor.NavigateUrl & "?Program=NEW"
            hlAddSchedule.NavigateUrl = hlAddSchedule.NavigateUrl & "?Program=NEW"

            litProgramId.Text = "New"
            hfProgramId.Value = String.Empty
            hfIconImage.Value = String.Empty

            litProgramLink.Text = ""
            litProgramLink.Visible = False

            txtName.Text = ""
            ContentTextArea.InnerText = ""
            txtKeywords.Text = ""
            txtDescription.Text = ""
            txtPermalink.Text = ""
            txtBrowserTitle.Text = ""

            lblIconImageLocation.Text = ""
            lblIconImageLocation.Visible = False

            ddlAddInstructor.SelectedIndex = 0
            ddlAddInstructor.DataSource = Nothing

            dtgInstructors.DataSource = Nothing
            dtgLatestSchedule.DataSource = Nothing
        End Sub

        Protected Sub InstructorsDataGridDeleteCommand(ByVal Sender As Object, ByVal E As CommandEventArgs) Handles dtgInstructors.DeleteCommand
            Dim ProgramId As Integer = Integer.Parse(hfProgramId.Value)
            Dim InstructorId As Integer = Integer.Parse(E.CommandArgument.ToString)

            CCFramework.ContentManagement.CalendarModule.ProgramController.RemoveInstructor(ProgramId, InstructorId)

            LoadInstructors(ProgramId)
        End Sub

    End Class
End Namespace