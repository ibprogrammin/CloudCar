Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement.FormModule

    Public Class FormFieldController

        ''' <summary>
        ''' Returns a field when passed an Id
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared GetFieldByIdFunc As Func(Of ContentDataContext, Integer, FormField) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) _
                                      (From f In CurrentDataContext.FormFields Where f.Id = Id Select f).FirstOrDefault)

        ''' <summary>
        ''' Returns all fields
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared GetAllFieldsFunc As Func(Of ContentDataContext, IQueryable(Of FormField)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      From f In CurrentDataContext.FormFields Select f)

        ''' <summary>
        ''' Returns all form fields
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared GetAllFormFieldsFunc As Func(Of ContentDataContext, Integer, IQueryable(Of FormField)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, FormId As Integer) _
                                      From f In CurrentDataContext.FormFields Where f.FormId = FormId Select f)

        Public Shared Function Create(ByVal FormId As Integer, ByVal Label As String, ByVal Details As String, ByVal DataType As FormDataType, ByVal ControlType As FormControlType, ByVal DefaultValues As String, ByVal ValidationExpression As String, ByVal Watermark As String, ByVal FieldIndex As Integer, OptionData As String) As Integer
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentField As New FormField

            CurrentField.FormId = FormId
            CurrentField.Label = Label
            CurrentField.Details = Details
            CurrentField.DataType = DataType
            CurrentField.ControlType = ControlType
            CurrentField.DefaultValues = DefaultValues
            CurrentField.ValidationExpression = ValidationExpression
            CurrentField.Watermark = Watermark
            CurrentField.FieldIndex = FieldIndex
            CurrentField.OptionData = OptionData

            CurrentDataContext.FormFields.InsertOnSubmit(CurrentField)
            CurrentDataContext.SubmitChanges()

            Create = CurrentField.Id

            CurrentDataContext.Dispose()
        End Function

        Public Shared Sub Update(ByVal Id As Integer, ByVal FormId As Integer, ByVal Label As String, ByVal Details As String, ByVal DataType As FormDataType, ByVal ControlType As FormControlType, ByVal DefaultValues As String, ByVal ValidationExpression As String, ByVal Watermark As String, ByVal FieldIndex As Integer, OptionData As String)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentField As FormField

            CurrentField = GetFieldByIdFunc(CurrentDataContext, Id)

            If CurrentField Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidFormException()
            Else
                CurrentField.FormId = FormId
                CurrentField.Label = Label
                CurrentField.Details = Details
                CurrentField.DataType = DataType
                CurrentField.ControlType = ControlType
                CurrentField.DefaultValues = DefaultValues
                CurrentField.ValidationExpression = ValidationExpression
                CurrentField.Watermark = Watermark
                CurrentField.FieldIndex = FieldIndex
                CurrentField.OptionData = OptionData

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If
        End Sub

        ''' <summary>
        ''' A function that will delete a form field from the form field table when passed the Id of that item
        ''' </summary>
        ''' <param name="Id">An integer value representing the Id of the form field to delete</param>
        ''' <returns>A Boolean value indicating if the delete operation was a success</returns>
        ''' <remarks></remarks>
        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Dim CurrentDataContext As New ContentDataContext

            Try
                Dim CurrentFormField As FormField = GetFieldByIdFunc(CurrentDataContext, Id)

                CurrentDataContext.FormFields.DeleteOnSubmit(CurrentFormField)
                CurrentDataContext.SubmitChanges()

                Delete = True
            Catch Ex As Exception
                Delete = False
            End Try

            CurrentDataContext.Dispose()
        End Function

        ''' <summary>
        ''' A function that return all the form field items stored in the database
        ''' </summary>
        ''' <returns>A List(Of FormField) with all form field items currently stored in the form field table</returns>
        ''' <remarks></remarks>
        Public Shared Function Items() As List(Of FormField)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentFormFields As List(Of FormField) = GetAllFieldsFunc(CurrentDataContext).ToList

            If CurrentFormFields Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidFormException()
            Else
                Items = CurrentFormFields

                CurrentDataContext.Dispose()
            End If
        End Function

        ''' <summary>
        ''' A function that return all the form items stored in the database
        ''' </summary>
        ''' <returns>A List(Of FormField) of form field items with the FormId value passed currently stored in the form fields table</returns>
        ''' <remarks></remarks>
        Public Shared Function Items(ByVal FormId As Integer) As List(Of FormField)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentFormFields As List(Of FormField) = GetAllFormFieldsFunc(CurrentDataContext, FormId).ToList

            If CurrentFormFields Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidFormException()
            Else
                Items = CurrentFormFields

                CurrentDataContext.Dispose()
            End If
        End Function

        ''' <summary>
        ''' A function that returns a form field item from the database based on the Id that is passed
        ''' </summary>
        ''' <param name="Id">The Id of the form field you wish the function to return</param>
        ''' <returns>Form</returns>
        ''' <remarks></remarks>
        Public Shared Function Item(ByVal Id As Integer) As FormField
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentFormField As FormField

            CurrentFormField = GetFieldByIdFunc(CurrentDataContext, Id)

            If CurrentFormField Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidFormException()
            Else
                Item = CurrentFormField

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Enum FormDataType
            BooleanType = 1
            IntegerType = 2
            StringType = 3
            DecimalType = 4
            DateType = 5
            'ArrayType = 6
        End Enum

        Public Enum FormControlType
            TextBoxControl = 1
            DropDownControl = 2
            RadioButtonControl = 3
            CheckBoxControl = 4
            DateControl = 5
        End Enum

        Public Class InvalidFormException
            Inherits Exception

            Public Sub New()
                MyBase.New("The form you are looking for does not exist")
            End Sub

        End Class

    End Class

End Namespace