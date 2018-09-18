Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement.CareerModule

Namespace CCAdmin.ContentManagement.CareerModule

    Public Class Careers
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadCareers()
            End If
        End Sub

        Private Sub LoadCareer(ByVal CareerId As Integer)
            Try
                Dim SelectedCareer As Career = CareerController.GetElement(CareerId)

                CareerIdHiddenField.Value = SelectedCareer.id.ToString

                TitleTextBox.Text = SelectedCareer.title
                DepartmentDropDown.SelectedValue = SelectedCareer.department.ToString
                ReportToTextBox.Text = SelectedCareer.pointofcontact
                ExperienceTextBox.Text = SelectedCareer.experience
                LevelTextBox.Text = SelectedCareer.level
                ReferenceCodeTextBox.Text = SelectedCareer.referencecode
                DescriptionTextArea.InnerText = SelectedCareer.description

            Catch Ex As Exception
                StatusLabel.Text = Ex.ToString
                StatusLabel.Visible = True
            End Try
        End Sub

        Private Sub SaveButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles SaveButton.Click
            Dim CareerId As Integer

            If Page.IsValid Then
                Dim CareerTitle As String = TitleTextBox.Text
                Dim Department As Integer = Integer.Parse(DepartmentDropDown.SelectedValue)
                Dim PointOfContact As String = ReportToTextBox.Text
                Dim Experience As String = ExperienceTextBox.Text
                Dim Level As String = LevelTextBox.Text
                Dim ReferenceCode As String = ReferenceCodeTextBox.Text
                Dim Description As String = DescriptionTextArea.InnerText

                If Integer.TryParse(CareerIdHiddenField.Value, CareerId) Then
                    CareerController.Update(CareerId, CareerTitle, Department, PointOfContact, Experience, Level, ReferenceCode, Description)

                    StatusLabel.Text = "The career has been saved!"
                    StatusLabel.Visible = True

                    LoadCareer(CareerId)
                Else
                    CareerId = CareerController.Create(CareerTitle, Department, PointOfContact, Experience, Level, ReferenceCode, Description)

                    StatusLabel.Text = "The career has been created!"
                    StatusLabel.Visible = True

                    LoadCareer(CareerId)

                    LoadCareers()
                End If
            End If
        End Sub

        Private Sub NewButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles NewButton.Click
            ClearControls()
        End Sub

        Private Sub LoadCareers()
            CareersListBox.Items.Clear()
            CareersListBox.Items.Add(New ListItem("Select a career from this list", ""))
            CareersListBox.AppendDataBoundItems = True
            CareersListBox.DataSource = CareerController.GetElements
            CareersListBox.DataBind()
        End Sub

        Private Sub ClearControls()
            CareersListBox.SelectedValue = Nothing

            CareerIdHiddenField.Value = Nothing

            TitleTextBox.Text = ""
            DepartmentDropDown.SelectedValue = Nothing
            ReportToTextBox.Text = ""
            ExperienceTextBox.Text = ""
            LevelTextBox.Text = ""
            ReferenceCodeTextBox.Text = ""
            DescriptionTextArea.InnerText = ""
        End Sub

        Private Sub CareersListBoxSelectedIndexChanged(ByVal Sender As Object, ByVal E As EventArgs) Handles CareersListBox.SelectedIndexChanged
            If Not CareersListBox.SelectedValue = Nothing Then
                Dim CareerId As Integer = Integer.Parse(CareersListBox.SelectedValue)

                LoadCareer(CareerId)
            End If
        End Sub

        Private Sub DeleteButtonCommand(ByVal Sender As Object, ByVal E As CommandEventArgs) Handles DeleteButton.Command
            If Not CareerIdHiddenField.Value = Nothing Then
                Dim CareerId As Integer = Integer.Parse(CareerIdHiddenField.Value)

                If CareerController.Delete(CareerId) = True Then
                    StatusLabel.Text = "The selected career has been deleted permanently!"
                    StatusLabel.Visible = True

                    LoadCareers()

                    ClearControls()
                Else
                    StatusLabel.Text = "An error occured while trying to delete the current career."
                    StatusLabel.Visible = True
                End If
            Else
                StatusLabel.Text = "Sorry but you do not have a career selected."
                StatusLabel.Visible = True
            End If
        End Sub

    End Class

End Namespace