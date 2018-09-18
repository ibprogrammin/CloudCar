Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement.FormModule

    Public Class FormController

        ''' <summary>
        ''' Returns a form when passed an Id
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared GetFormByIdFunc As Func(Of ContentDataContext, Integer, Form) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) _
                                      (From f In CurrentDataContext.Forms Where f.Id = Id Select f).FirstOrDefault)

        ''' <summary>
        ''' Returns all forms
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared GetAllFormsFunc As Func(Of ContentDataContext, IQueryable(Of Form)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      From f In CurrentDataContext.Forms Select f)

        Public Shared GetFormIdByPermalinkFunc As Func(Of ContentDataContext, String, Integer) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Permalink As String) _
                                      (From f In CurrentDataContext.Forms Where f.Permalink.Contains(Permalink) Select f.Id).FirstOrDefault)

        ''' <summary>
        ''' Function to create a new form
        ''' </summary>
        ''' <param name="Name">A String containing the name of the new form</param>
        ''' <param name="Details">A String containing the details of the new form</param>
        ''' <param name="CallToActionText">A String containing the text to be placed on the forms call to action</param>
        ''' <param name="CopyToAdmin">A Boolean that indicates if the admin should be notified of a new form submission</param>
        ''' <param name="ImageId">An Integer containing the id of the Image to attach to this form</param>
        ''' <param name="CompleteMessage">A String containing a message that will be displayed to the user upon completion</param>
        ''' <returns>An Integer that contains the Id of the new form</returns>
        ''' <remarks></remarks>
        Public Shared Function Create(ByVal Name As String, ByVal Details As String, Permalink As String, ByVal CallToActionText As String, ByVal CopyToAdmin As Boolean, ByVal ImageId As Integer, ByVal CompleteMessage As String) As Integer
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentForm As New Form

            CurrentForm.Name = Name
            CurrentForm.Details = Details
            CurrentForm.Permalink = Permalink
            CurrentForm.CallToActionText = CallToActionText
            CurrentForm.CopyToAdmin = CopyToAdmin
            CurrentForm.Details = Details
            CurrentForm.CompleteMessage = CompleteMessage
            CurrentForm.ImageId = ImageId

            CurrentDataContext.Forms.InsertOnSubmit(CurrentForm)
            CurrentDataContext.SubmitChanges()

            Create = CurrentForm.Id

            CurrentDataContext.Dispose()
        End Function

        ''' <summary>
        ''' Updates a form item from the form table with the values passed
        ''' </summary>
        ''' <param name="Id">Integer representing the Id of the form to update</param>
        ''' <param name="Name">A String containing the name of the new form</param>
        ''' <param name="Details">A String containing the details of the new form</param>
        ''' <param name="CallToActionText">A String containing the text to be placed on the forms call to action</param>
        ''' <param name="CopyToAdmin">A Boolean that indicates if the admin should be notified when the form is submitted</param>
        ''' <param name="ImageId">An integer containing the Id of the image to attach to this form</param>
        ''' <param name="CompleteMessage">A String containing a message that will be displayed to the user upon completion</param>
        ''' <remarks></remarks>
        Public Shared Sub Update(ByVal Id As Integer, ByVal Name As String, ByVal Details As String, Permalink As String, ByVal CallToActionText As String, ByVal CopyToAdmin As Boolean, ByVal ImageId As Integer, ByVal CompleteMessage As String)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentForm As Form

            CurrentForm = GetFormByIdFunc(CurrentDataContext, Id)

            If CurrentForm Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidFormException()
            Else
                CurrentForm.Name = Name
                CurrentForm.Details = Details
                CurrentForm.Permalink = Permalink
                CurrentForm.CallToActionText = CallToActionText
                CurrentForm.CopyToAdmin = CopyToAdmin
                CurrentForm.Details = Details
                CurrentForm.CompleteMessage = CompleteMessage
                CurrentForm.ImageId = ImageId

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If
        End Sub

        ''' <summary>
        ''' A function that will delete a form item from the form table when passed the Id of that item
        ''' </summary>
        ''' <param name="Id">An integer value representing the Id of the form to delete</param>
        ''' <returns>A Boolean value indicating if the delete operation was a success</returns>
        ''' <remarks></remarks>
        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Dim CurrentDataContext As New ContentDataContext

            Try
                Dim CurrentForm As Form = GetFormByIdFunc(CurrentDataContext, Id)

                CurrentDataContext.Forms.DeleteOnSubmit(CurrentForm)
                CurrentDataContext.SubmitChanges()

                Delete = True
            Catch Ex As Exception
                Delete = False
            End Try

            CurrentDataContext.Dispose()
        End Function

        ''' <summary>
        ''' A function that return all the form items stored in the database
        ''' </summary>
        ''' <returns>A List(Of Form) with all form items currently stored in the form table</returns>
        ''' <remarks></remarks>
        Public Shared Function Items() As List(Of Form)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentForms As List(Of Form) = GetAllFormsFunc(CurrentDataContext).ToList

            If CurrentForms Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidFormException()
            Else
                Items = CurrentForms

                CurrentDataContext.Dispose()
            End If
        End Function

        ''' <summary>
        ''' A function that returns a form item from the database based on the Id that is passed
        ''' </summary>
        ''' <param name="Id">The Id of the form you wish the function to return</param>
        ''' <returns>Form</returns>
        ''' <remarks></remarks>
        Public Shared Function Item(ByVal Id As Integer) As Form
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentForm As Form

            CurrentForm = GetFormByIdFunc(CurrentDataContext, Id)

            If CurrentForm Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidFormException()
            Else
                Item = CurrentForm

                'CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetFormIdByPermalink(ByVal Permalink As String) As Integer
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentFormId As Integer

            CurrentFormId = GetFormIdByPermalinkFunc(CurrentDataContext, Permalink)

            If CurrentFormId = Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidFormException()
            Else
                GetFormIdByPermalink = CurrentFormId
            End If

            CurrentDataContext.Dispose()
        End Function
        
        Public Class InvalidFormException
            Inherits Exception

            Public Sub New()
                MyBase.New("The form you are looking for does not exist")
            End Sub

        End Class

    End Class

End Namespace