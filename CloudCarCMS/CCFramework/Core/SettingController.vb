Imports System.Data.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Core

    Public Class SettingController

        Public Shared GetSettingsFunc As Func(Of ContentDataContext, IQueryable(Of Setting)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) From s In db.Settings Order By s.Category, s.Key Select s)

        Public Shared GetSettingByIDFunc As Func(Of ContentDataContext, Integer, Setting) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, SettingID As Integer) (From s In db.Settings Where s.ID = SettingID Select s).FirstOrDefault)

        Public Shared GetSettingByKeyFunc As Func(Of ContentDataContext, String, Setting) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, Key As String) (From s In db.Settings Where s.Key.ToLower = Key.ToLower Select s).FirstOrDefault)

        Public Shared GetSettingValueByKeyFunc As Func(Of ContentDataContext, String, String) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, Key As String) (From s In db.Settings Where s.Key.ToLower = Key.ToLower Select s).FirstOrDefault.Value)

        Public Shared GetSettingsByCategoryFunc As Func(Of ContentDataContext, String, IQueryable(Of Setting)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, Category As String) From s In db.Settings Where s.Category = Category Order By s.Category, s.Key Select s)

        Public Shared GetSettingsCategoriesFunc As Func(Of ContentDataContext, IQueryable(Of String)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) From s In db.Settings Order By s.Category Select s.Category Distinct)

        Public Shared GetReadableSettingsByCategoryFunc As Func(Of ContentDataContext, String, IQueryable(Of Setting)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext, Category As String) From s In db.Settings Where s.Category = Category And s.ReadOnly = False Order By s.Category, s.Key Select s)

        Public Shared GetReadableSettingsCategoriesFunc As Func(Of ContentDataContext, IQueryable(Of String)) = _
            CompiledQuery.Compile(Function(db As ContentDataContext) From s In db.Settings Where s.ReadOnly = False Order By s.Category Select s.Category Distinct)

        Public Shared Function Create(ByVal Key As String, ByVal Value As String, ByVal Category As String) As Integer
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentSettingId As Integer
            Dim CurrentSetting As New Setting

            CurrentSetting.Key = Key
            CurrentSetting.Value = Value
            CurrentSetting.Category = Category

            CurrentDataContext.Settings.InsertOnSubmit(CurrentSetting)
            CurrentDataContext.SubmitChanges()

            CurrentSettingId = CurrentSetting.ID

            CurrentDataContext.Dispose()

            Return CurrentSettingId
        End Function

        Public Shared Sub Update(ByVal SettingId As Integer, ByVal Key As String, ByVal Value As String, ByVal Category As String)
            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentSetting As Setting = GetSettingByIDFunc(CurrentDataContext, SettingId)

            If CurrentSetting Is Nothing Then
                Throw New InvalidSettingException
            Else
                CurrentSetting.Key = Key
                CurrentSetting.Value = Value
                CurrentSetting.Category = Category

                CurrentDataContext.SubmitChanges()
            End If

            CurrentDataContext.Dispose()
        End Sub

        Public Shared Function Delete(ByVal SettingId As Integer) As Boolean
            Try
                Dim CurrentDataContext As New ContentDataContext

                Dim CurrentSetting As Setting = GetSettingByIDFunc(CurrentDataContext, SettingId)

                CurrentDataContext.Settings.DeleteOnSubmit(CurrentSetting)
                CurrentDataContext.SubmitChanges()

                CurrentDataContext.Dispose()

                Return True
            Catch CurrentException As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetElement(ByVal SettingId As Integer) As Setting
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentSetting As Setting = GetSettingByIDFunc(CurrentDataContext, SettingId)

            If CurrentSetting Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidSettingException
            Else
                CurrentDataContext.Dispose()

                Return CurrentSetting
            End If
        End Function

        Public Shared Function GetElement(ByVal Key As String) As Setting
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentSetting As Setting = GetSettingByKeyFunc(CurrentDataContext, Key)

            If CurrentSetting Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidSettingException
            Else
                CurrentDataContext.Dispose()

                Return CurrentSetting
            End If
        End Function

        Public Shared Function GetSettingValue(ByVal Key As String) As String
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentSetting As String = GetSettingValueByKeyFunc(CurrentDataContext, Key)

            If CurrentSetting Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidSettingException
            Else
                CurrentDataContext.Dispose()

                Return CurrentSetting
            End If
        End Function

        Public Shared Function GetElements() As List(Of Setting)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentSettings As List(Of Setting) = GetSettingsFunc(CurrentDataContext).ToList

            If CurrentSettings Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidSettingException
            Else
                CurrentDataContext.Dispose()

                Return CurrentSettings
            End If
        End Function

        Public Shared Function GetSettingByCategory(ByVal Category As String) As List(Of Setting)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentSettings As List(Of Setting) = GetSettingsByCategoryFunc(CurrentDataContext, Category).ToList

            If CurrentSettings Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidSettingException
            Else
                CurrentDataContext.Dispose()

                Return CurrentSettings
            End If
        End Function

        Public Shared Function GetReadableSettingByCategory(ByVal Category As String) As List(Of Setting)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentSettings As List(Of Setting) = GetReadableSettingsByCategoryFunc(CurrentDataContext, Category).ToList

            If CurrentSettings Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New InvalidSettingException
            Else
                CurrentDataContext.Dispose()

                Return CurrentSettings
            End If
        End Function

        Public Shared Function GetSettingCategories() As List(Of String)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentCategories As List(Of String) = GetSettingsCategoriesFunc(CurrentDataContext).ToList

            If CurrentCategories Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("There are no categories to be returned.")
            Else
                CurrentDataContext.Dispose()

                Return CurrentCategories
            End If
        End Function

        Public Shared Function GetReadableSettingCategories() As List(Of String)
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentCategories As List(Of String) = GetReadableSettingsCategoriesFunc(CurrentDataContext).ToList

            If CurrentCategories Is Nothing Then
                CurrentDataContext.Dispose()

                Throw New Exception("There are no categories to be returned.")
            Else
                CurrentDataContext.Dispose()

                Return CurrentCategories
            End If
        End Function

        Public Class InvalidSettingException
            Inherits Exception

            Public Sub New()
                MyBase.New("The setting you are looking for does not exist")
            End Sub

        End Class

    End Class

End Namespace