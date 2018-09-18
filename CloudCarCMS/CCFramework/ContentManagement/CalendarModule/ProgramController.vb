Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement.CalendarModule

    Public Class ProgramController
        Inherits CCFramework.Core.DataControllerClass

        Public Shared GetProgramsFunc As Func(Of CommerceDataContext, IQueryable(Of Program)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext) _
                                             From p In CurrentDataContext.Programs Select p)

        Public Shared GetProgramByIdFunc As Func(Of CommerceDataContext, Integer, Program) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProgramId As Integer) _
                                              (From p In CurrentDataContext.Programs Where p.Id = ProgramId Select p).FirstOrDefault)

        Public Shared GetProgramByPermalinkFunc As Func(Of CommerceDataContext, String, Program) = _
            CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, Permalink As String) _
                                      (From p In CurrentDataContext.Programs Where p.Permalink Like Permalink Select p).FirstOrDefault)

        Public Overloads Function Create(ByVal Name As String, ByVal Content As String, ByVal IconImage As String, ByVal Permalink As String, ByVal PageTitle As String, ByVal Keywords As String, ByVal Description As String) As Integer
            Dim Item As New Program
            Dim ItemId As Integer

            Item.Name = Name
            Item.Content = Content
            Item.IconImage = IconImage
            Item.Permalink = Permalink
            Item.PageTitle = PageTitle
            Item.Keywords = Keywords
            Item.Description = Description

            db.Programs.InsertOnSubmit(Item)
            db.SubmitChanges()

            ItemId = Item.Id

            Item = Nothing

            Return ItemId
        End Function

        Public Overloads Function Delete(ByVal ID As Integer) As Boolean
            Try
                Dim Item As Program = GetProgramByIdFunc(db, ID)

                db.Programs.DeleteOnSubmit(Item)
                db.SubmitChanges()

                Return True
            Catch Ex As Exception
                Return False
            End Try
        End Function

        Public Overloads Function Update(ByVal Id As Integer, ByVal Name As String, ByVal Content As String, ByVal IconImage As String, ByVal Permalink As String, ByVal PageTitle As String, ByVal Keywords As String, ByVal Description As String) As Boolean
            Dim Item As Program = GetProgramByIdFunc(db, Id)

            If Item Is Nothing Then
                Throw New Exception("Program " & Id.ToString & " does not exist.")
            Else
                Item.Name = Name
                Item.Content = Content
                Item.IconImage = IconImage
                Item.Permalink = Permalink
                Item.PageTitle = PageTitle
                Item.Keywords = Keywords
                Item.Description = Description

                db.SubmitChanges()
            End If

            Item = Nothing

            Return True
        End Function

        Public Overloads Function GetElement(ByVal ID As Integer) As Program
            Dim Item As Program = GetProgramByIdFunc(db, ID)

            If Item Is Nothing Then
                Throw New Exception("Program with ID: " & ID.ToString & " does not exist.")
            Else
                Return Item
            End If

            Item = Nothing
        End Function

        Public Overloads Function GetElements() As List(Of Program)
            Dim CurrentPrograms As List(Of Program) = GetProgramsFunc(db).ToList

            If CurrentPrograms Is Nothing Then
                Throw New Exception("There are no Programs")
            Else
                Return CurrentPrograms
            End If
        End Function

        Public Shared Function GetProgramFromLink(ByVal Permalink As String) As Program
            Dim CurrentDataContext As New CommerceDataContext
            Dim CurrentProgram As Program

            CurrentProgram = GetProgramByPermalinkFunc(CurrentDataContext, Permalink)

            If CurrentProgram Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("There was no Program found linked to this address.")
            Else
                GetProgramFromLink = CurrentProgram

                CurrentDataContext.Dispose()
            End If
        End Function


        Public Shared GetProgramInstructorByIdFunc As Func(Of CommerceDataContext, Integer, Integer, ProgramInstructor) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProgramId As Integer, InstructorId As Integer) _
                                             (From pi In CurrentDataContext.ProgramInstructors Where pi.ProgramId = ProgramId And pi.InstructorId = InstructorId Select pi).FirstOrDefault())

        Public Shared GetProgramInstructorsByProgramIdFunc As Func(Of CommerceDataContext, Integer, IQueryable(Of ProgramInstructor)) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProgramId As Integer) _
                                             From pi In CurrentDataContext.ProgramInstructors Where pi.ProgramId = ProgramId Select pi)

        Public Shared ProgramHasInstructorFunc As Func(Of CommerceDataContext, Integer, Integer, Boolean) = _
                    CompiledQuery.Compile(Function(CurrentDataContext As CommerceDataContext, ProgramId As Integer, InstructorId As Integer) _
                                              (From pi In CurrentDataContext.ProgramInstructors Where pi.ProgramId = ProgramId And pi.InstructorId = InstructorId Select pi).Count > 0)

        Public Shared Function ProgramHasInstructor(ByVal ProgramID As Integer, ByVal InstructorID As Integer) As Boolean
            Dim CurrentDataContext As New CommerceDataContext

            ProgramHasInstructor = ProgramHasInstructorFunc(CurrentDataContext, ProgramID, InstructorID)

            CurrentDataContext.Dispose()
        End Function

        Public Shared Sub RemoveInstructor(ByVal ProgramId As Integer, ByVal InstructorId As Integer)
            If ProgramHasInstructor(ProgramId, InstructorId) Then
                Dim CurrentDataContext As New CommerceDataContext

                Dim CurrentProgramInstructor As ProgramInstructor = GetProgramInstructorByIdFunc(CurrentDataContext, ProgramId, InstructorId)

                CurrentDataContext.ProgramInstructors.DeleteOnSubmit(CurrentProgramInstructor)
                CurrentDataContext.SubmitChanges()

                CurrentDataContext.Dispose()
            End If
        End Sub

        Public Shared Sub RemoveInstructors(ByVal ProgramId As Integer)
            Dim CurrentDataContext As New CommerceDataContext

            Dim ProgramInstructors As System.Linq.IQueryable(Of ProgramInstructor)

            ProgramInstructors = GetProgramInstructorsByProgramIdFunc(CurrentDataContext, ProgramId)
            CurrentDataContext.ProgramInstructors.DeleteAllOnSubmit(ProgramInstructors)
            CurrentDataContext.SubmitChanges()

            CurrentDataContext.Dispose()
        End Sub

        Public Shared Sub AddInstructor(ByVal ProgramId As Integer, ByVal InstructorId As Integer)
            If Not ProgramHasInstructor(ProgramId, InstructorId) Then
                Dim CurrentDataContext As New CommerceDataContext

                Dim ProgramInstructor As New ProgramInstructor

                ProgramInstructor.ProgramId = ProgramId
                ProgramInstructor.InstructorId = InstructorId

                CurrentDataContext.ProgramInstructors.InsertOnSubmit(ProgramInstructor)
                CurrentDataContext.SubmitChanges()

                CurrentDataContext.Dispose()
            End If
        End Sub


        Public Shared Sub RemoveSchedules(ByVal ProgramId As Integer)
            Dim CurrentDataContext As New CommerceDataContext

            Dim CurrentProgramSchedules As IEnumerable(Of Schedule) = ScheduleController.GetProgramSchedules(ProgramId).AsEnumerable()

            CurrentDataContext.Schedules.DeleteAllOnSubmit(CurrentProgramSchedules)
            CurrentDataContext.SubmitChanges()

            CurrentDataContext.Dispose()
        End Sub

        Public Shared Function GetSchedule(ByVal ProgramID As Integer, ByVal Day As DayOfWeek) As String
            Dim db As New CommerceDataContext

            GetSchedule = (From s In db.Schedules Where s.ProgramId = ProgramID And s.BookingDate.Day = Day Select s.BookingDate.Hour).FirstOrDefault.ToString

            db = Nothing
        End Function

        Public Shared Function GetScheduleID(ByVal ProgramID As Integer, ByVal Day As DayOfWeek) As Integer
            Dim db As New CommerceDataContext

            GetScheduleID = (From s In db.Schedules Where s.ProgramId = ProgramID And s.BookingDate.Day = Day Select s.Id).FirstOrDefault

            db = Nothing
        End Function

    End Class

End Namespace