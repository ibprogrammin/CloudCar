Imports CloudCar.CCFramework.Model
Imports System.Data.Linq

Namespace CCFramework.ContentManagement.FormModule

    Public Class FormDataController
        ''' <summary>
        ''' Returns a form data object when passed an Id
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared GetFormDataByIdFunc As Func(Of ContentDataContext, Integer, FormData) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) _
                                      (From f In CurrentDataContext.FormDatas Where f.Id = Id Select f).FirstOrDefault)

        ''' <summary>
        ''' Returns all Form Data
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared GetAllFormDataFunc As Func(Of ContentDataContext, IQueryable(Of FormData)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      From f In CurrentDataContext.FormDatas Select f)

        ''' <summary>
        ''' Returns all forms Form Data
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared GetAllFormsFormDataFunc As Func(Of ContentDataContext, Integer, IQueryable(Of FormData)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, FormId As Integer) _
                                      From f In CurrentDataContext.FormDatas Where f.FormId = FormId Select f)

        Public Shared Function Create(ByVal FormId As Integer, Data As String) As Integer
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentFormData As New FormData

            CurrentFormData.FormId = FormId
            CurrentFormData.Data = Data

            CurrentDataContext.FormDatas.InsertOnSubmit(CurrentFormData)
            CurrentDataContext.SubmitChanges()

            Create = CurrentFormData.Id

            CurrentDataContext.Dispose()
        End Function

        Public Shared Sub Update(ByVal Id As Integer, ByVal FormId As Integer, ByVal Data As String)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentFormData As FormData

            CurrentFormData = GetFormDataByIdFunc(CurrentDataContext, Id)

            If CurrentFormData Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New FormFieldController.InvalidFormException()
            Else
                CurrentFormData.FormId = FormId
                CurrentFormData.Data = Data

                CurrentDataContext.SubmitChanges()
                CurrentDataContext.Dispose()
            End If
        End Sub

        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Dim CurrentDataContext As New ContentDataContext

            Try
                Dim CurrentFormData As FormData = GetFormDataByIdFunc(CurrentDataContext, Id)

                CurrentDataContext.FormDatas.DeleteOnSubmit(CurrentFormData)
                CurrentDataContext.SubmitChanges()

                Delete = True
            Catch Ex As Exception
                Delete = False
            End Try

            CurrentDataContext.Dispose()
        End Function

        Public Shared Function Items() As List(Of FormData)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentFormData As List(Of FormData) = GetAllFormDataFunc(CurrentDataContext).ToList

            If CurrentFormData Is Nothing Then
                CurrentDataContext.Dispose()

                Items = Nothing
            Else
                Items = CurrentFormData

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function Items(ByVal FormId As Integer) As List(Of FormData)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentFormsFormData As List(Of FormData) = GetAllFormsFormDataFunc(CurrentDataContext, FormId).ToList

            If CurrentFormsFormData Is Nothing Then
                CurrentDataContext.Dispose()

                Items = Nothing
            Else
                Items = CurrentFormsFormData

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function Item(ByVal Id As Integer) As FormData
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentFormData As FormData

            CurrentFormData = GetFormDataByIdFunc(CurrentDataContext, Id)

            If CurrentFormData Is Nothing Then
                CurrentDataContext.Dispose()

                Item = Nothing
            Else
                Item = CurrentFormData

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetFormsDataFormated(FormId As Integer) As DataTable
            Dim CurrentFormsData As List(Of FormData) = Items(FormId)
            Dim CurrentFormsFields As List(Of FormField) = FormFieldController.Items(FormId)

            Dim CurrentDataTable As New DataTable
            
            For Each Item As FormField In CurrentFormsFields
                CurrentDataTable.Columns.Add(Item.Label)
            Next

            For Each UnformatedData As FormData In CurrentFormsData
                Dim CurrentRow As DataRow = CurrentDataTable.NewRow

                For Each CurrentData As String In UnformatedData.Data.Split(New String() {"{"}, StringSplitOptions.RemoveEmptyEntries)
                    If Not CurrentData = String.Empty Then

                        Dim TempData As String = String.Empty

                        If CurrentData.EndsWith("},") Then
                            TempData = CurrentData.Substring(0, CurrentData.Length - 2)
                        ElseIf CurrentData.EndsWith("}") Then
                            TempData = CurrentData.Substring(0, CurrentData.Length - 1)
                        End If

                        Dim CurrentId As Integer = Integer.Parse(CurrentData.Split(":"c).FirstOrDefault.Replace(":", "").Trim)

                        TempData = TempData.Replace(String.Format("{0}:", CurrentId), "")

                        CurrentRow(FormFieldController.Item(CurrentId).Label) = TempData

                    End If
                Next

                CurrentDataTable.Rows.Add(CurrentRow)
            Next

            GetFormsDataFormated = CurrentDataTable
        End Function

    End Class

End Namespace