Imports System.Web.UI
Imports System.Web.UI.Adapters
Imports System.Data.SqlClient
Imports System
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports Microsoft.Practices.EnterpriseLibrary.Caching

Namespace handlers.Adapters

    Public Class SessionPageStateAdapter
        Inherits PageAdapter

        Public Overrides Function GetStatePersister() As PageStatePersister

            Return New SessionPageStatePersister(Page)
            'Return MyBase.GetStatePersister()

        End Function

    End Class

    Public Class CustomPageStateAdapter
        Inherits PageAdapter

        Public Overrides Function GetStatePersister() As PageStatePersister

            Return New CustomPageStatePersister(Page)
            'Return MyBase.GetStatePersister()

        End Function

    End Class

    Public Class CustomPageStatePersisterConfiguration
        Private Const CustomPageStatePersister As String = "cache sql" ' custom off

        Private Const CONFIGURATION_KEY As String = "CustomPageStatePersister"
        Private Const CONFIGURATION_OFF As String = "off"
        Private Const CONFIGURATION_SQL As String = "sql"
        Private Const CONFIGURATION_CACHE As String = "cache"
        Private Const CONFIGURATION_CUSTOM As String = "custom"

        Public IsSwitchOff As Boolean
        Public IsCompressed As Boolean
        Public IsSqlPersisted As Boolean
        Public IsCached As Boolean
        Public IsOnCustomPageOnly As Boolean

        Public Sub New()
            IsSwitchOff = True
            IsCompressed = False
            IsSqlPersisted = False
            IsCached = False
            IsOnCustomPageOnly = False

            Dim ConfigString As String = CustomPageStatePersister 'ConfigurationManager.AppSettings(CONFIGURATION_KEY)

            If Not String.IsNullOrEmpty(ConfigString) Then
                ConfigString = ConfigString.ToLower()

                If Not ConfigString.Contains(CONFIGURATION_OFF) Then
                    IsSwitchOff = False
                End If

                If ConfigString.Contains(CONFIGURATION_SQL) Then
                    IsSqlPersisted = True
                End If

                If ConfigString.Contains(CONFIGURATION_CACHE) Then
                    IsCached = True
                End If

                If ConfigString.Contains(CONFIGURATION_CUSTOM) Then
                    IsOnCustomPageOnly = True
                End If

            End If
        End Sub

    End Class

    Public Class CustomPageStatePersister
        Inherits HiddenFieldPageStatePersister

        Private configuration As New CustomPageStatePersisterConfiguration
        Private Const VIEWSTATE_CACHEKEY_PREFIX As String = "__VIEWSTATE-"
        Private Const VIEWSTATE_SQLCONNECTION_NAME As String = "MainConnectionString"
        
        Public Sub New(ByVal Page As Page)
            MyBase.New(Page)
        End Sub

        Public Overrides Sub Load()
            MyBase.Load()

            If configuration.IsSwitchOff OrElse (Not configuration.IsSqlPersisted AndAlso Not configuration.IsCached AndAlso Not configuration.IsCompressed) Then
                Return
            End If

            If (configuration.IsOnCustomPageOnly _
                    AndAlso Not Page.GetType Is GetType(ICustomStatePersistedPage)) _
                    OrElse Page.GetType Is GetType(IDefaultStatePersistedPage) Then
                Return
            End If

            Dim CurrentViewState As String = MyBase.ViewState.ToString

            If CurrentViewState.Length = 36 Then
                Dim CurrentCustomViewState As CustomViewState = Nothing

                If configuration.IsCached Then
                    Dim CurrentCacheKey As String = CStr(VIEWSTATE_CACHEKEY_PREFIX & MyBase.ViewState)

                    Try
                        CurrentCustomViewState = GetFromCache(CurrentCacheKey)
                    Catch ex As Exception

                    End Try
                End If

                If CurrentCustomViewState Is Nothing AndAlso configuration.IsSqlPersisted Then
                    CurrentCustomViewState = GetFromSql(CurrentViewState)
                End If

                Deserialize(CurrentCustomViewState.Data)
            End If
        End Sub

        Public Overrides Sub Save()
            If configuration.IsSwitchOff OrElse (Not configuration.IsSqlPersisted AndAlso Not configuration.IsCached AndAlso Not configuration.IsCompressed) Then
                MyBase.Save()
                Return
            End If

            If (configuration.IsOnCustomPageOnly _
                    AndAlso Not Page.GetType Is GetType(ICustomStatePersistedPage)) _
                    OrElse Page.GetType Is GetType(IDefaultStatePersistedPage) Then
                MyBase.Save()
                Return
            End If


            Dim CurrentViewState As New CustomViewState

            CurrentViewState.Id = Guid.NewGuid().ToString()
            CurrentViewState.Data = Serialize()
            CurrentViewState.TimeStamp = DateTime.Now

            If configuration.IsCached Then
                Dim SavedToCache As Boolean = False

                Try
                    SaveToCache(CurrentViewState, VIEWSTATE_CACHEKEY_PREFIX & CurrentViewState.Id)
                    SavedToCache = True
                Catch CurrentException As Exception

                End Try

                If Not SavedToCache AndAlso configuration.IsSqlPersisted Then
                    SaveToSql(CurrentViewState)
                End If
            End If

            If configuration.IsSqlPersisted AndAlso Not configuration.IsCached Then
                SaveToSql(CurrentViewState)
            End If

            MyBase.ViewState = CurrentViewState.Id
            MyBase.ControlState = ""
            
            MyBase.Save()
        End Sub

        Public Function Serialize() As String
            Dim CurrentViewStatePair As Pair = New Pair(ViewState, ControlState)

            Return StateFormatter.Serialize(CurrentViewStatePair)
        End Function

        Public Sub Deserialize(ByVal SerializedViewState As String)
            If Not SerializedViewState Is Nothing Then
                Dim CurrentViewStatePair As Object = StateFormatter.Deserialize(SerializedViewState)

                If CurrentViewStatePair.GetType Is GetType(Pair) Then
                    Dim myPair As Pair = CType(CurrentViewStatePair, Pair)
                    ViewState = myPair.First
                    ControlState = myPair.Second
                Else
                    ViewState = CurrentViewStatePair
                End If
            End If

        End Sub

        Public Sub SaveToSql(ByVal CurrentViewState As CustomViewState)
            If Not CurrentViewState Is Nothing AndAlso Not String.IsNullOrEmpty(CurrentViewState.Id) AndAlso Not String.IsNullOrEmpty(CurrentViewState.Data) Then
                Dim connectionString As String = ConfigurationManager.ConnectionStrings(VIEWSTATE_SQLCONNECTION_NAME).ConnectionString

                Using connection As New SqlConnection(connectionString)

                    Using command As SqlCommand = connection.CreateCommand()

                        command.Connection = connection
                        command.CommandType = CommandType.StoredProcedure
                        command.CommandText = "usp_InsertViewState"

                        command.Parameters.AddWithValue("@VsId", CurrentViewState.Id)
                        command.Parameters.AddWithValue("@VsData", CurrentViewState.Data)
                        command.Parameters.AddWithValue("@VsTimeStamp", CurrentViewState.TimeStamp)

                        command.Connection.Open()
                        command.ExecuteNonQuery()
                    End Using
                End Using
            End If
        End Sub

        Public Function GetFromSql(ByVal ViewStateId As String) As CustomViewState
            If String.IsNullOrEmpty(ViewStateId) Then
                Return Nothing
            End If

            Dim CurrentViewState As CustomViewState
            Dim CurrentConnectionString As String = ConfigurationManager.ConnectionStrings(VIEWSTATE_SQLCONNECTION_NAME).ConnectionString

            Using CurrentConnection As New SqlConnection(CurrentConnectionString)

                If CurrentConnection.State = ConnectionState.Closed Then
                    CurrentConnection.Open()
                End If

                Using CurrentCommand As SqlCommand = New SqlCommand("", CurrentConnection)
                    CurrentCommand.CommandType = CommandType.StoredProcedure
                    CurrentCommand.CommandText = "usp_GetViewState"
                    CurrentCommand.Parameters.AddWithValue("@VsId", ViewStateId)

                    Using CurrentDataReader As SqlDataReader = CurrentCommand.ExecuteReader
                        CurrentViewState = New CustomViewState()

                        While CurrentDataReader.Read
                            CurrentViewState.Id = CurrentDataReader("VsId").ToString
                            CurrentViewState.Data = CurrentDataReader("VsData").ToString
                            CurrentViewState.TimeStamp = Date.Parse(CurrentDataReader("VsTimeStamp").ToString)
                        End While
                    End Using
                End Using
            End Using

            Return CurrentViewState
        End Function

        Public Sub SaveToCache(ByVal CurrentViewState As CustomViewState, ByVal CacheKey As String)
            If Not CurrentViewState Is Nothing AndAlso Not String.IsNullOrEmpty(CurrentViewState.Id) AndAlso Not String.IsNullOrEmpty(CurrentViewState.Data) Then
                Dim CurrentCacheManager As ICacheManager = CacheFactory.GetCacheManager()
                CurrentCacheManager.Add(CacheKey, CurrentViewState)
            End If
        End Sub

        Public Function GetFromCache(ByVal CacheKey As String) As CustomViewState
            If String.IsNullOrEmpty(CacheKey) Then
                Return Nothing
            End If

            Dim CurrentCachingManager As ICacheManager = CacheFactory.GetCacheManager()
            Return CType(CurrentCachingManager.GetData(CacheKey), CustomViewState)
        End Function


        Public Sub SaveToSession(ByVal vs As CustomViewState, ByVal cacheKey As String)
            If Not vs Is Nothing AndAlso Not String.IsNullOrEmpty(vs.Id) AndAlso Not String.IsNullOrEmpty(vs.Data) Then
                HttpContext.Current.Session.Add(cacheKey, vs)
            End If
        End Sub

        Public Function GetFromSession(ByVal CacheKey As String) As CustomViewState
            If String.IsNullOrEmpty(CacheKey) Then
                Return Nothing
            End If

            Return CType(HttpContext.Current.Session(CacheKey), CustomViewState)
        End Function

    End Class

    Public Interface ICustomStatePersistedPage
    End Interface

    Public Interface IDefaultStatePersistedPage
    End Interface

    Public Class CustomViewState
        Public Id As String
        Public Data As String
        Public TimeStamp As DateTime
    End Class

End Namespace