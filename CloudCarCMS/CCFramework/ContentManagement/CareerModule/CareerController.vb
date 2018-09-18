Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.ContentManagement.CareerModule

    Public Class CareerController

        Public Shared GetCareerByIdFunc As Func(Of ContentDataContext, Integer, Career) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) _
                                      (From CurrentCareer In CurrentDataContext.Careers _
                                       Where CurrentCareer.id = Id Select CurrentCareer).FirstOrDefault)

        Public Shared GetAllCareersFunc As Func(Of ContentDataContext, IQueryable(Of Career)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      From CurrentCareer In CurrentDataContext.Careers Select CurrentCareer)

        Public Shared Function Create(ByVal Title As String, ByVal Department As Integer, ByVal PointOfContact As String, ByVal Experience As String, ByVal Level As String, ByVal ReferenceCode As String, ByVal Description As String) As Integer
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentCareer As New Career
            Dim NewCareerId As Integer

            CurrentCareer.title = Title
            CurrentCareer.department = Department
            CurrentCareer.pointofcontact = PointOfContact
            CurrentCareer.experience = Experience
            CurrentCareer.level = Level
            CurrentCareer.referencecode = ReferenceCode
            CurrentCareer.description = Description

            CurrentDataContext.Careers.InsertOnSubmit(CurrentCareer)
            CurrentDataContext.SubmitChanges()

            NewCareerId = CurrentCareer.id

            CurrentDataContext.Dispose()

            Return NewCareerId
        End Function

        Public Shared Sub Update(ByVal Id As Integer, ByVal Title As String, ByVal Department As Integer, ByVal PointOfContact As String, ByVal Experience As String, ByVal Level As String, ByVal ReferenceCode As String, ByVal Description As String)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentCareer As Career = GetCareerByIdFunc(CurrentDataContext, Id)

            If CurrentCareer Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidCareerException("Career does not exist")
            Else
                CurrentCareer.title = Title
                CurrentCareer.department = Department
                CurrentCareer.pointofcontact = PointOfContact
                CurrentCareer.experience = Experience
                CurrentCareer.level = Level
                CurrentCareer.referencecode = ReferenceCode
                CurrentCareer.description = Description

                CurrentDataContext.SubmitChanges()

                CurrentDataContext.Dispose()
            End If
        End Sub

        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Try
                Dim CurrentDataContext As New ContentDataContext

                Dim CurrentCareer As Career = GetCareerByIdFunc(CurrentDataContext, Id)

                CurrentDataContext.Careers.DeleteOnSubmit(CurrentCareer)
                CurrentDataContext.SubmitChanges()

                Return True
            Catch Ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetElement(ByVal Id As Integer) As Career
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentCareer As Career = GetCareerByIdFunc(CurrentDataContext, Id)

            If CurrentCareer Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidCareerException("Career does not exist")
            Else
                GetElement = CurrentCareer

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetElements() As List(Of Career)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentCareers As List(Of Career) = GetAllCareersFunc(CurrentDataContext).ToList

            If CurrentCareers Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidCareerException("There are no careers stored in the data table.")
            Else
                GetElements = CurrentCareers

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared ReadOnly DepartmentList As String() = {"IT", "Accounting", "Sales"}

        Public Shared Function GetDepartmentLabel(ByVal DepartmentId As Integer) As String
            Return DepartmentList(DepartmentId)
        End Function

    End Class

    Public Class InvalidCareerException
        Inherits Exception

        Public Sub New(ByVal Message As String)
            MyBase.New(Message)
        End Sub

    End Class

End Namespace