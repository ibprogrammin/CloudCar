Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement.CalendarModule

    Public Class InstructorController
        Inherits CCFramework.Core.DataControllerClass

        Public Shared GetInstructorsFunc As Func(Of CommerceDataContext, IQueryable(Of Instructor)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                             From i In CurrentDataContext.Instructors Select i)

        Public Shared GetInstructorByIdFunc As Func(Of CommerceDataContext, Integer, Instructor) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, InstructorId As Integer) _
                                              (From i In CurrentDataContext.Instructors Where i.Id = InstructorId Select i).FirstOrDefault)

        Public Shared GetInstructorByPermalinkFunc As Func(Of CommerceDataContext, String, Instructor) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, Permalink As String) _
                                      (From i In CurrentDataContext.Instructors Where i.Permalink Like Permalink Select i).FirstOrDefault)

        Public Overloads Function Create(ByVal Name As String, ByVal Specialty As String, ByVal Bio As String, ByVal ProfileImageId As Integer, ByVal Permalink As String, ByVal Keywords As String, ByVal Description As String, ByVal BrowserTitle As String) As Integer
            Dim Item As New Instructor
            Dim ItemId As Integer

            Item.Name = Name
            Item.Specialty = Specialty
            Item.Bio = Bio
            Item.ProfileImageId = ProfileImageId
            Item.Permalink = Permalink
            Item.Keywords = Keywords
            Item.Description = Description
            Item.BrowserTitle = BrowserTitle

            db.Instructors.InsertOnSubmit(Item)
            db.SubmitChanges()

            ItemId = Item.Id

            Item = Nothing

            Return ItemId
        End Function

        Public Overloads Function Delete(ByVal Id As Integer) As Boolean
            Try
                Dim Item As Instructor = GetInstructorByIdFunc(db, Id)

                RemoveInstructorFromPrograms(Id)

                db.Instructors.DeleteOnSubmit(Item)
                db.SubmitChanges()

                Return True
            Catch Ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal Id As Integer, ByVal Name As String, ByVal Specialty As String, ByVal Bio As String, ByVal ProfileImageId As Integer, ByVal Permalink As String, ByVal Keywords As String, ByVal Description As String, ByVal BrowserTitle As String) As Boolean
            Dim Item As Instructor = GetInstructorByIdFunc(db, Id)

            If Item Is Nothing Then
                Throw New Exception("Instructor " & Id.ToString & " does not exist.")
            Else
                Item.Name = Name
                Item.Specialty = Specialty
                Item.Bio = Bio
                Item.ProfileImageId = ProfileImageId
                Item.Permalink = Permalink
                Item.Keywords = Keywords
                Item.Description = Description
                Item.BrowserTitle = BrowserTitle

                db.SubmitChanges()
            End If

            Item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal Id As Integer) As Instructor
            Dim Item As Instructor = GetInstructorByIdFunc(db, Id)

            If Item Is Nothing Then
                Throw New Exception("Instructor with ID: " & Id.ToString & " does not exist.")
            Else
                Return Item
            End If

            Item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of Instructor)
            Dim CurrentInstructors As List(Of Instructor) = GetInstructorsFunc(db).ToList

            If CurrentInstructors Is Nothing Then
                Throw New Exception("There are no instructors")
            Else
                Return CurrentInstructors
            End If
        End Function

        Public Shared Function GetInstructorFromLink(ByVal Permalink As String) As Instructor
            Dim CurrentDataContext As New CommerceDataContext
            Dim CurrentInstructor As Instructor

            CurrentInstructor = GetInstructorByPermalinkFunc(CurrentDataContext, Permalink)

            If CurrentInstructor Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("There was no instructor found linked to this address.")
            Else
                GetInstructorFromLink = CurrentInstructor

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared GetProgramInstructorsByInstructorIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of ProgramInstructor)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, InstructorId As Integer) _
                                              From pi In CurrentDataContext.ProgramInstructors Where pi.InstructorId = InstructorId Select pi)

        Public Shared Sub RemoveInstructorFromPrograms(ByVal InstructorId As Integer)
            Dim CurrentDataContext As New CommerceDataContext
            Dim ProgramInstructors As IQueryable(Of ProgramInstructor) = GetProgramInstructorsByInstructorIdFunc(CurrentDataContext, InstructorId)

            CurrentDataContext.ProgramInstructors.DeleteAllOnSubmit(ProgramInstructors)
            CurrentDataContext.SubmitChanges()

            CurrentDataContext.Dispose()
        End Sub

        Public Shared GetInstructorProgramsByInstructorIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Program)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, InstructorId As Integer) _
                                              From p In CurrentDataContext.Programs _
                                              Join pi In CurrentDataContext.ProgramInstructors _
                                              On p.Id Equals pi.ProgramId _
                                              Where pi.InstructorId = InstructorId _
                                              Order By p.Name _
                                              Select p)

        Public Shared Function GetInstructorPrograms(ByVal InstructorId As Integer) As List(Of Program)
            Dim CurrentDataContext As New CommerceDataContext

            GetInstructorPrograms = GetInstructorProgramsByInstructorIdFunc(CurrentDataContext, InstructorId).ToList

            CurrentDataContext.Dispose()
        End Function

        Public Shared GetProgramInstructorsByProgramIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of Instructor)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProgramId As Integer) _
                                              From i In CurrentDataContext.Instructors _
                                              Join pi In CurrentDataContext.ProgramInstructors _
                                              On i.Id Equals pi.ProgramId _
                                              Where pi.ProgramId = ProgramId _
                                              Order By i.Name _
                                              Select i)

        Public Shared Function GetProgramInstructors(ByVal ProgramId As Integer) As List(Of Instructor)
            Dim CurrentDataContext As New CommerceDataContext

            GetProgramInstructors = GetProgramInstructorsByProgramIdFunc(CurrentDataContext, ProgramId).ToList

            CurrentDataContext.Dispose()
        End Function

    End Class

End Namespace