Imports CloudCar.CCFramework.Model
Imports System.Data.Linq

Namespace CCFramework.ContentManagement.CallToActionModule

    Public Class CallToActionController

        Public Shared GetCallToActionByIdFunc As Func(Of ContentDataContext, Integer, CallToAction) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext, Id As Integer) _
                                      (From CurrentCalltoAction In CurrentDataContext.CallToActions _
                                       Where CurrentCalltoAction.Id = Id _
                                       Select CurrentCalltoAction).FirstOrDefault)

        Public Shared GetAllCallsToActionFunc As Func(Of ContentDataContext, IQueryable(Of CallToAction)) = _
            CompiledQuery.Compile(Function(CurrentDataContext As ContentDataContext) _
                                      From CurrentCalltoAction In CurrentDataContext.CallToActions _
                                      Select CurrentCalltoAction)

        Public Shared Function Create(ByVal Heading As String, ByVal Details As String, ByVal ButtonText As String, ByVal ImageUrl As String, LinkUrl As String) As Integer
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentCallToAction As New CallToAction
            Dim NewCallToActionId As Integer

            CurrentCallToAction.Heading = Heading
            CurrentCallToAction.Details = Details
            CurrentCallToAction.ButtonText = ButtonText
            CurrentCallToAction.ImageUrl = ImageUrl
            CurrentCallToAction.LinkUrl = LinkUrl

            CurrentDataContext.CallToActions.InsertOnSubmit(CurrentCallToAction)
            CurrentDataContext.SubmitChanges()

            NewCallToActionId = CurrentCallToAction.Id

            CurrentDataContext.Dispose()

            Return NewCallToActionId
        End Function

        Public Shared Sub Update(ByVal Id As Integer, ByVal Heading As String, ByVal Details As String, ByVal ButtonText As String, ByVal ImageUrl As String, LinkUrl As String)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentCallToAction As CallToAction = GetCallToActionByIdFunc(CurrentDataContext, Id)

            If CurrentCallToAction Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("Call To Action does not exist")
            Else
                CurrentCallToAction.Heading = Heading
                CurrentCallToAction.Details = Details
                CurrentCallToAction.ButtonText = ButtonText
                CurrentCallToAction.ImageUrl = ImageUrl
                CurrentCallToAction.LinkUrl = LinkUrl

                CurrentDataContext.SubmitChanges()

                CurrentDataContext.Dispose()
            End If
        End Sub

        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Try
                Dim CurrentDataContext As New ContentDataContext

                Dim CurrentCallToAction As CallToAction = GetCallToActionByIdFunc(CurrentDataContext, Id)

                CurrentDataContext.CallToActions.DeleteOnSubmit(CurrentCallToAction)
                CurrentDataContext.SubmitChanges()

                Return True
            Catch Ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetItem(ByVal Id As Integer) As CallToAction
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentCallToAction As CallToAction = GetCallToActionByIdFunc(CurrentDataContext, Id)

            If CurrentCallToAction Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("Call To Action does not exist")
            Else
                GetItem = CurrentCallToAction

                CurrentDataContext.Dispose()
            End If
        End Function

        Public Shared Function GetItems() As List(Of CallToAction)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentCallsToAction As List(Of CallToAction) = GetAllCallsToActionFunc(CurrentDataContext).ToList

            If CurrentCallsToAction Is Nothing Then
                CurrentDataContext.Dispose()

                GetItems = Nothing
                'Throw New Exception("There are no Calls To Action stored in the data table.")
            Else
                GetItems = CurrentCallsToAction

                CurrentDataContext.Dispose()
            End If
        End Function

    End Class

End Namespace