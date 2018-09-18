Imports CloudCar.CCFramework.ContentManagement.FormModule
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core

Namespace CCAdmin.ContentManagement.FormModule
    Public Class CustomIntakeForms
        Inherits Page

        'Regex for validation
        '1. Numeric
        '2. Email
        '3. Postal Code
        '4. Zip Code
        '5. Phone Number
        '6. Address
        Private BuiltInRegexs As String() = {"/^\d+$/", _
                                             "\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", _
                                             "^[ABCEGHJKLMNPRSTVXY]{1}\d{1}[A-Z]{1} *\d{1}[A-Z]{1}\d{1}$", _
                                             "^\d{5}(?:[-\s]\d{4})?$", _
                                             "^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$", _
                                             ""}

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Session("FieldOptionControls") = Nothing
            End If
        End Sub

        Private Sub FormsListBoxLoad(Sender As Object, Args As EventArgs) Handles FormsListBox.Load
            If Not Page.IsPostBack Then
                LoadForms()
            End If
        End Sub

        Private Sub LoadForms()
            FormsListBox.DataSource = FormController.Items
            FormsListBox.DataBind()
        End Sub

        Public Sub FormsListBoxSelectedIndexChanged(ByVal Sender As Object, ByVal Args As EventArgs) Handles FormsListBox.SelectedIndexChanged
            Dim CurrentFormId As Integer = Integer.Parse(FormsListBox.SelectedValue)

            LoadForm(CurrentFormId)
        End Sub

        Private Sub LoadForm(ByVal FormId As Integer)
            Dim CurrentForm As Form = FormController.Item(FormId)

            CurrentFormLink.Visible = True
            CurrentFormLink.Text = String.Format("<label>Current Form</label><div class=""display-message"" style=""width: 667px;""><a href=""/Forms/{0}.html"" target=""_blank"" style=""background-color: #FFF8C2;"">/Forms/{0}.html</a></div><br class=""clear-both""/><br />", CurrentForm.Permalink)

            FormIdHiddenField.Value = CurrentForm.Id.ToString
            FormNameTextBox.Text = CurrentForm.Name
            DetailsTextBox.Text = CurrentForm.Details
            PermalinkTextBox.Text = CurrentForm.Permalink
            CompletionMessageTextBox.Text = CurrentForm.CompleteMessage
            CallToActionLabelTextBox.Text = CurrentForm.CallToActionText
            CcAdminCheckBox.Checked = CurrentForm.CopyToAdmin

            PictureImageIdHiddenField.Value = CurrentForm.ImageId.ToString

            If CurrentForm.ImageId.HasValue And Not CurrentForm.ImageId.Value = 0 Then
                Try
                    PictureLocationLabel.Text = String.Format("/images/db/{0}/full/{1}", CurrentForm.ImageId, PictureController.GetPicture(CurrentForm.ImageId.Value).PictureFileName)
                    PictureLocationLabel.Visible = True

                    PictureImage.Src = String.Format("/images/db/{0}/220/{1}", CurrentForm.ImageId, PictureController.GetPicture(CurrentForm.ImageId.Value).PictureFileName)
                    PictureImage.Visible = True
                Catch ex As Exception

                End Try
            End If

            FormFieldRepeater.DataSource = FormFieldController.Items(CurrentForm.Id)
            FormFieldRepeater.DataBind()

            Dim CurrentFormData As DataTable = FormDataController.GetFormsDataFormated(CurrentForm.Id)

            If CurrentFormData.Rows.Count > 0 Then
                FormDataDisplayDataGrid.DataSource = FormDataController.GetFormsDataFormated(CurrentForm.Id)
                FormDataDisplayDataGrid.DataBind()
            End If
        End Sub

        Private Sub SaveForm()
            Dim FormName As String = FormNameTextBox.Text
            Dim Details As String = DetailsTextBox.Text
            Dim Permalink As String = PermalinkTextBox.Text
            Dim CompletionMessage As String = CompletionMessageTextBox.Text
            Dim CallToActionLabel As String = CallToActionLabelTextBox.Text
            Dim CcAdmin As Boolean = CcAdminCheckBox.Checked

            Dim PictureImageId As Integer
            If PictureFileUpload.HasFile Then
                PictureImageId = ImageFunctions.UploadImage(PictureFileUpload)
            Else
                If PictureImageIdHiddenField.Value = String.Empty Then
                    PictureImageId = 0
                Else
                    PictureImageId = Integer.Parse(PictureImageIdHiddenField.Value)
                End If
            End If
            
            If Not FormIdHiddenField.Value Is Nothing And Not FormIdHiddenField.Value = String.Empty Then
                Dim FormId As Integer = Integer.Parse(FormIdHiddenField.Value)

                FormController.Update(FormId, FormName, Details, Permalink, CallToActionLabel, CcAdmin, PictureImageId, CompletionMessage)
            Else
                FormController.Create(FormName, Details, Permalink, CallToActionLabel, CcAdmin, PictureImageId, CompletionMessage)
            End If

            LoadForms()
        End Sub

        Private Sub SaveFormField()
            Dim FormId As Integer = Integer.Parse(FormIdHiddenField.Value)
            Dim NewFieldLabel As String = NewFieldLabelTextBox.Text
            Dim NewFieldDetails As String = NewFieldDetailsTextBox.Text
            Dim NewFieldDataType As FormFieldController.FormDataType = CType(Integer.Parse(NewFieldDataTypeDropDown.SelectedValue), FormFieldController.FormDataType)
            Dim NewFieldControlType As FormFieldController.FormControlType = CType(Integer.Parse(NewFieldControlTypeDropDown.SelectedValue), FormFieldController.FormControlType)
            Dim NewFieldDefaults As String = NewFieldDefaultsTextBox.Text
            Dim NewFieldValidation As String

            If NewFieldValidationDropDown.SelectedValue = "7" Then
                NewFieldValidation = NewFieldCustomValidationTextBox.Text
            Else
                NewFieldValidation = BuiltInRegexs(Integer.Parse(NewFieldValidationDropDown.SelectedValue))
            End If

            'TODO Make a key value control
            Dim NewFieldOptions As String = String.Empty

            Dim TempKey As String = String.Empty
            Dim TempValue As String = String.Empty
            For Each CurrentControl As Control In FieldOptionsPlaceHolder.Controls
                If TypeOf CurrentControl Is TextBox Then
                    If CurrentControl.ID.StartsWith("KeyTextBox-") Then
                        TempKey = CType(CurrentControl, TextBox).Text
                    End If
                    If CurrentControl.ID.StartsWith("ValueTextBox-") Then
                        TempValue = CType(CurrentControl, TextBox).Text

                        If Not TempKey = String.Empty And Not TempValue = String.Empty Then
                            NewFieldOptions &= String.Format("{{{0},{1}}},", TempKey, TempValue)
                        End If

                        TempKey = String.Empty
                        TempValue = String.Empty
                    End If
                End If
            Next

            Dim NewFieldWatermark As String = NewFieldWatermarkTextBox.Text
            Dim NewFieldIndex As Integer = Integer.Parse(NewFieldIndexTextBox.Text)

            If Not NewFieldIdHiddenField.Value Is Nothing And Not NewFieldIdHiddenField.Value = String.Empty Then
                Dim FormFieldId As Integer = Integer.Parse(NewFieldIdHiddenField.Value)

                FormFieldController.Update(FormFieldId, FormId, NewFieldLabel, NewFieldDetails, NewFieldDataType, NewFieldControlType, NewFieldDefaults, NewFieldValidation, NewFieldWatermark, NewFieldIndex, NewFieldOptions)
            Else
                FormFieldController.Create(FormId, NewFieldLabel, NewFieldDetails, NewFieldDataType, NewFieldControlType, NewFieldDefaults, NewFieldValidation, NewFieldWatermark, NewFieldIndex, NewFieldOptions)
            End If


        End Sub

        Private Sub ClearForm()
            FormIdHiddenField.Value = Nothing
            FormNameTextBox.Text = ""
            DetailsTextBox.Text = ""
            PictureFileUpload = Nothing
            CompletionMessageTextBox.Text = ""
            PermalinkTextBox.Text = ""
            'FormFieldDataList.DataSource = Nothing

            PictureImageIdHiddenField.Value = ""

            PictureLocationLabel.Text = ""
            PictureLocationLabel.Visible = False

            PictureImage.Visible = False

            CurrentFormLink.Visible = False
            CurrentFormLink.Text = ""

            ClearNewFieldForm()
        End Sub

        Private Sub ClearNewFieldForm()
            NewFieldIdHiddenField.Value = Nothing
            NewFieldLabelTextBox.Text = ""
            NewFieldDetailsTextBox.Text = ""
            NewFieldDataTypeDropDown.SelectedValue = Nothing
            NewFieldControlTypeDropDown.SelectedValue = Nothing
            NewFieldDefaultsTextBox.Text = ""
            NewFieldValidationDropDown.SelectedValue = Nothing
            NewFieldCustomValidationTextBox.Text = ""

            FieldOptionsPlaceHolder.Controls.Clear()
            FieldOptionsPlaceHolder.Visible = False
            AddFieldOptionButton.Visible = False
            RemoveFieldOptionButton.Visible = False
        End Sub

        Private Sub DeleteForm()
            Dim FormId As Integer = Integer.Parse(FormIdHiddenField.Value)

            'TODO add code to delete form
            FormController.Delete(FormId)
        End Sub

#Region "Button Click Events"

        Private Sub SaveFormButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles SaveFormButton.Click
            SaveForm()
        End Sub

        Private Sub DeleteFormButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles DeleteFormButton.Click
            DeleteForm()
            ClearForm()
        End Sub

        Private Sub AddFieldButtonClick(ByVal Sender As Object, ByVal E As EventArgs) Handles AddFieldButton.Click
            SaveFormField()
        End Sub

        Public Sub AddFieldOptionButtonClick(Sender As Object, Args As EventArgs) Handles AddFieldOptionButton.Click
            Dim KeyTextBox As New TextBox
            Dim ValueTextBox As New TextBox

            KeyTextBox.ID = String.Format("KeyTextBox-{0}", Guid.NewGuid.ToString)
            ValueTextBox.ID = String.Format("ValueTextBox-{0}", Guid.NewGuid.ToString)

            If Session("FieldOptionControls") Is Nothing Then
                Session("FieldOptionControls") = New List(Of Control)
            End If

            Session("FieldOptionControls").Add(KeyTextBox)
            Session("FieldOptionControls").Add(ValueTextBox)
            Session("FieldOptionControls").Add(New LiteralControl("<br class=""clear-both"" />"))

            FieldOptionsPlaceHolder.Controls.Add(KeyTextBox)
            FieldOptionsPlaceHolder.Controls.Add(ValueTextBox)
            FieldOptionsPlaceHolder.Controls.Add(New LiteralControl("<br class=""clear-both"" />"))
        End Sub

        Public Sub RemoveFieldOptionButtonClick(Sender As Object, Args As EventArgs) Handles RemoveFieldOptionButton.Click
            Dim ControlList As List(Of Control)
            If Not Session("FieldOptionControls") Is Nothing Then
                ControlList = CType(Session("FieldOptionControls"), List(Of Control))

                ControlList.Remove(ControlList.Where(Function(cl) cl.ID.StartsWith("KeyTextBox-")).LastOrDefault)
                ControlList.Remove(ControlList.Where(Function(cl) cl.ID.StartsWith("ValueTextBox-")).LastOrDefault)
                ControlList.Remove(ControlList.Where(Function(cl) TypeOf cl Is Literal).LastOrDefault)

                CType(ControlList.FirstOrDefault, TextBox).Text = ControlList.Where(Function(cl) cl.ID.StartsWith("KeyTextBox-")).LastOrDefault.ID
                'Response.Write(ControlList.Where(Function(cl) cl.ID.Contains("KeyTextBox-")).Last.ID & "lalalal")

                FieldOptionsPlaceHolder.Controls.Remove(ControlList.Where(Function(cl) cl.ID.StartsWith("KeyTextBox-")).LastOrDefault)
                FieldOptionsPlaceHolder.Controls.Remove(ControlList.Where(Function(cl) cl.ID.StartsWith("ValueTextBox-")).LastOrDefault)
                FieldOptionsPlaceHolder.Controls.Remove(ControlList.Where(Function(cl) TypeOf cl Is Literal).LastOrDefault)

                Session("FieldOptionControls") = ControlList
            End If
        End Sub

        Private Sub ClearButtonClick(Sender As Object, Args As EventArgs) Handles ClearButton.Click
            ClearForm()
        End Sub

#End Region

        Protected Overrides Sub OnInit(Args As EventArgs)
            MyBase.OnInit(Args)

            If Not Session("FieldOptionControls") Is Nothing Then
                For Each CurrentControl As Control In CType(Session("FieldOptionControls"), List(Of Control))
                    FieldOptionsPlaceHolder.Controls.Add(CurrentControl)
                Next
            End If
        End Sub

        Private Sub NewFieldControlTypeDropDownSelectedIndexChanged(Sender As Object, Args As EventArgs) Handles NewFieldControlTypeDropDown.SelectedIndexChanged
            Dim CurrentSelectedControlType As EFormFieldControlType = CType(Integer.Parse(NewFieldControlTypeDropDown.SelectedValue), EFormFieldControlType)

            Select Case CurrentSelectedControlType
                Case EFormFieldControlType.DropDownList, EFormFieldControlType.RadioButtonList
                    FieldOptionsPlaceHolder.Visible = True
                    AddFieldOptionButton.Visible = True
                    RemoveFieldOptionButton.Visible = True
                Case Else
                    FieldOptionsPlaceHolder.Visible = False
                    AddFieldOptionButton.Visible = False
                    RemoveFieldOptionButton.Visible = False
            End Select
        End Sub

    End Class

End Namespace