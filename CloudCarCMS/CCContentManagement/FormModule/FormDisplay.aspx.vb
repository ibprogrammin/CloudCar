Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement.FormModule
Imports CloudCar.CCFramework.Core

Namespace CCContentManagement.FormModule

    Public Class FormDisplay
        Inherits RoutablePage

        Public ReadOnly Property Permalink() As String
            Get
                Return (From CurrentValues In RequestContext.RouteData.Values Where CurrentValues.Key = "permalink" Select New With {.id = CurrentValues.Value}).SingleOrDefault.id.ToString
            End Get
        End Property

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            If Not Request.Item("FormId") Is Nothing Then
                LoadForm(Integer.Parse(Request.Item("FormId")))
            End If
        End Sub

        Protected Overrides Sub OnInit(ByVal Args As EventArgs)
            If Not Permalink Is Nothing Then
                Try
                    Dim CurrentFormId As Integer = FormController.GetFormIdByPermalink(Permalink)

                    LoadForm(CurrentFormId)
                Catch Ex As Exception

                End Try
            End If

            MyBase.OnInit(Args)
        End Sub

        Private Sub SaveForm()
            If Page.IsValid Then
                Dim CurrentFormData As String = String.Empty

                For Each CurrentFormField As Control In FormFieldsPlaceHolder.Controls
                    If Not CurrentFormField.ID Is Nothing Then
                        If CurrentFormField.ID.StartsWith("FormFieldControl") Then
                            Dim FieldId As Integer = Integer.Parse(CurrentFormField.ID.Replace("FormFieldControl", ""))

                            If TypeOf CurrentFormField Is TextBox Then
                                CurrentFormData &= String.Format("{{{0}:{1}}},", FieldId, CType(CurrentFormField, TextBox).Text.Replace("{", "").Replace("}", "").Replace(",", ""))
                            ElseIf TypeOf CurrentFormField Is CheckBox Then
                                CurrentFormData &= String.Format("{{{0}:{1}}},", FieldId, CType(CurrentFormField, CheckBox).Checked.ToString)
                            ElseIf TypeOf CurrentFormField Is DropDownList Then
                                CurrentFormData &= String.Format("{{{0}:{1}}},", FieldId, CType(CurrentFormField, DropDownList).SelectedValue)
                            ElseIf TypeOf CurrentFormField Is RadioButtonList Then
                                CurrentFormData &= String.Format("{{{0}:{1}}},", FieldId, CType(CurrentFormField, RadioButtonList).SelectedValue)
                            Else
                            End If
                        End If
                    End If
                Next

                CurrentFormData = CurrentFormData.Substring(0, CurrentFormData.Length - 1)

                FormDataController.Create(Integer.Parse(FormIdHiddenField.Value), CurrentFormData)

                'TODO If cc admin is selected send an email to the administrator with the data
            End If
        End Sub

        Private Sub LoadForm(FormId As Integer)
            Dim CurrentForm As Form = FormController.Item(FormId)

            Page.Title = CurrentForm.Name & Settings.SiteTitle

            FormIdHiddenField.Value = CurrentForm.Id.ToString
            FormNameLiteral.Text = CurrentForm.Name
            FormDetailsLiteral.Text = CurrentForm.Details
            FormSubmitButton.Text = CurrentForm.CallToActionText

            Dim CurrentFormValidationSummary As New ValidationSummary
            CurrentFormValidationSummary.DisplayMode = ValidationSummaryDisplayMode.List
            CurrentFormValidationSummary.ValidationGroup = "CurrentForm"
            CurrentFormValidationSummary.ShowMessageBox = True

            For Each CurrentFormField As FormField In CurrentForm.FormFields.OrderBy(Function(FormField) FormField.FieldIndex)
                Dim CurrentControlType As EFormFieldControlType = DirectCast(CurrentFormField.ControlType, EFormFieldControlType)

                FormFieldsPlaceHolder.Controls.Add(New LiteralControl(String.Format("<label>{0}</label>", CurrentFormField.Label)))

                Select Case CurrentControlType
                    Case EFormFieldControlType.TextBox
                        Dim CurrentControl As New TextBox()
                        CurrentControl.ID = String.Format("FormFieldControl{0}", CurrentFormField.Id)
                        CurrentControl.Text = CurrentFormField.DefaultValues
                        CurrentControl.ClientIDMode = ClientIDMode.Static
                        CurrentControl.CssClass = "form-text-box"

                        FormFieldsPlaceHolder.Controls.Add(CurrentControl)
                    Case EFormFieldControlType.TextArea
                        Dim CurrentControl As New TextBox()
                        CurrentControl.ID = String.Format("FormFieldControl{0}", CurrentFormField.Id)
                        CurrentControl.TextMode = TextBoxMode.MultiLine
                        CurrentControl.Text = CurrentFormField.DefaultValues
                        CurrentControl.ClientIDMode = ClientIDMode.Static
                        CurrentControl.CssClass = "form-text-area"

                        FormFieldsPlaceHolder.Controls.Add(CurrentControl)
                    Case EFormFieldControlType.CheckBox
                        Dim CurrentControl As New CheckBox()
                        CurrentControl.ID = String.Format("FormFieldControl{0}", CurrentFormField.Id)
                        CurrentControl.Checked = CBool(CurrentFormField.DefaultValues)
                        CurrentControl.ClientIDMode = ClientIDMode.Static
                        CurrentControl.CssClass = "form-check-box"

                        FormFieldsPlaceHolder.Controls.Add(CurrentControl)
                    Case EFormFieldControlType.DropDownList
                        Dim CurrentControl As New DropDownList()
                        CurrentControl.ID = String.Format("FormFieldControl{0}", CurrentFormField.Id)
                        CurrentControl.Items.AddRange(GetListItems(CurrentFormField.OptionData).ToArray)
                        CurrentControl.SelectedValue = CurrentFormField.DefaultValues
                        CurrentControl.ClientIDMode = ClientIDMode.Static
                        CurrentControl.CssClass = "form-select-box"

                        FormFieldsPlaceHolder.Controls.Add(CurrentControl)
                    Case EFormFieldControlType.RadioButtonList
                        Dim CurrentControl As New RadioButtonList()
                        CurrentControl.ID = String.Format("FormFieldControl{0}", CurrentFormField.Id)
                        CurrentControl.Items.AddRange(GetListItems(CurrentFormField.OptionData).ToArray)
                        CurrentControl.SelectedValue = CurrentFormField.DefaultValues
                        CurrentControl.ClientIDMode = ClientIDMode.Static
                        CurrentControl.CssClass = "form-radio-button"
                        CurrentControl.RepeatLayout = RepeatLayout.Flow

                        FormFieldsPlaceHolder.Controls.Add(CurrentControl)
                    Case Else
                        Dim CurrentControl As New TextBox()
                        CurrentControl.ID = String.Format("FormFieldControl{0}", CurrentFormField.Id)
                        CurrentControl.Text = CurrentFormField.DefaultValues
                        CurrentControl.ClientIDMode = ClientIDMode.Static
                        CurrentControl.CssClass = "form-text-box"

                        FormFieldsPlaceHolder.Controls.Add(CurrentControl)
                End Select

                Try
                    If Not CurrentControlType = EFormFieldControlType.CheckBox AndAlso Not CurrentControlType = EFormFieldControlType.RadioButtonList AndAlso Not CurrentControlType = EFormFieldControlType.DropDownList Then

                        If Not CurrentFormField.ValidationExpression Is Nothing AndAlso Not CurrentFormField.ValidationExpression = String.Empty Then
                            Dim CurrentRegexValidator As New RegularExpressionValidator()

                            CurrentRegexValidator.ID = String.Format("FormFieldValidatorControl{0}", CurrentFormField.Id)
                            CurrentRegexValidator.ErrorMessage = "You did not submit a value in the correct format"
                            CurrentRegexValidator.ValidationGroup = "CurrentForm"
                            CurrentRegexValidator.Enabled = True
                            CurrentRegexValidator.ValidationExpression = CurrentFormField.ValidationExpression
                            CurrentRegexValidator.ControlToValidate = String.Format("FormFieldControl{0}", CurrentFormField.Id)
                            CurrentRegexValidator.Display = ValidatorDisplay.None

                            FormFieldsPlaceHolder.Controls.Add(CurrentRegexValidator)
                        End If
                    End If
                Catch Ex As Exception

                End Try


                FormFieldsPlaceHolder.Controls.Add(New LiteralControl("<br class=""clear-both"" />"))
            Next
        End Sub

        Public Function GetListItems(OptionData As String) As List(Of ListItem)
            Dim CurrentListItems As New List(Of ListItem)

            Dim CurrentRow As Integer = 1
            Dim CurrentColumn As Integer = 1

            For Each CurrentRowData In OptionData.Replace("},", "").Split(New String() {"{"}, StringSplitOptions.RemoveEmptyEntries)
                Dim CurrentKey As String = String.Empty
                Dim CurrentValue As String = String.Empty

                For Each CurrentColumnData In CurrentRowData.Split(","c)
                    Select Case CurrentColumn
                        Case 1
                            CurrentKey = CurrentColumnData
                        Case 2
                            CurrentValue = CurrentColumnData
                    End Select

                    CurrentColumn += 1
                Next

                CurrentListItems.Add(New ListItem(CurrentKey, CurrentValue))

                CurrentColumn = 1
                CurrentRow += 1
            Next

            Return CurrentListItems
        End Function

        Private Sub FormSubmitButtonClick(Sender As Object, Args As EventArgs) Handles FormSubmitButton.Click
            SaveForm()
        End Sub

    End Class

    Public Class Person

    End Class


End Namespace