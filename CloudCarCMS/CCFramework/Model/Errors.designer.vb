﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.18052
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Data.Linq
Imports System.Data.Linq.Mapping
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Reflection

Namespace CCFramework.Model
	
    <Global.System.Data.Linq.Mapping.DatabaseAttribute(Name:="DataSource")> _
    Partial Public Class ErrorDataContext
        Inherits System.Data.Linq.DataContext

        Private Shared mappingSource As System.Data.Linq.Mapping.MappingSource = New AttributeMappingSource()

#Region "Extensibility Method Definitions"
        Partial Private Sub OnCreated()
        End Sub
        Partial Private Sub InsertSiteError(instance As SiteError)
        End Sub
        Partial Private Sub UpdateSiteError(instance As SiteError)
        End Sub
        Partial Private Sub DeleteSiteError(instance As SiteError)
        End Sub
#End Region

        Public Sub New()
            MyBase.New(Global.System.Configuration.ConfigurationManager.ConnectionStrings("ErrorDBConnectionString").ConnectionString, mappingSource)
            OnCreated()
        End Sub

        Public Sub New(ByVal connection As String)
            MyBase.New(connection, mappingSource)
            OnCreated()
        End Sub

        Public Sub New(ByVal connection As System.Data.IDbConnection)
            MyBase.New(connection, mappingSource)
            OnCreated()
        End Sub

        Public Sub New(ByVal connection As String, ByVal mappingSource As System.Data.Linq.Mapping.MappingSource)
            MyBase.New(connection, mappingSource)
            OnCreated()
        End Sub

        Public Sub New(ByVal connection As System.Data.IDbConnection, ByVal mappingSource As System.Data.Linq.Mapping.MappingSource)
            MyBase.New(connection, mappingSource)
            OnCreated()
        End Sub

        Public ReadOnly Property SiteErrors() As System.Data.Linq.Table(Of SiteError)
            Get
                Return Me.GetTable(Of SiteError)()
            End Get
        End Property
    End Class
	
	<Global.System.Data.Linq.Mapping.TableAttribute(Name:="dbo.SiteErrors")>  _
	Partial Public Class SiteError
		Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
		
		Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)
		
		Private _id As System.Guid
		
		Private _action As String
		
		Private _controller As String
		
		Private _timestamp As System.Nullable(Of Date)
		
		Private _ipaddress As String
		
		Private _helplink As String
		
		Private _message As String
		
		Private _source As String
		
		Private _stacktrace As String
		
		Private _targetsite As String
		
		Private _Application As String
		
    #Region "Extensibility Method Definitions"
    Partial Private Sub OnLoaded()
    End Sub
    Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
    End Sub
    Partial Private Sub OnCreated()
    End Sub
    Partial Private Sub OnidChanging(value As System.Guid)
    End Sub
    Partial Private Sub OnidChanged()
    End Sub
    Partial Private Sub OnactionChanging(value As String)
    End Sub
    Partial Private Sub OnactionChanged()
    End Sub
    Partial Private Sub OncontrollerChanging(value As String)
    End Sub
    Partial Private Sub OncontrollerChanged()
    End Sub
    Partial Private Sub OntimestampChanging(value As System.Nullable(Of Date))
    End Sub
    Partial Private Sub OntimestampChanged()
    End Sub
    Partial Private Sub OnipaddressChanging(value As String)
    End Sub
    Partial Private Sub OnipaddressChanged()
    End Sub
    Partial Private Sub OnhelplinkChanging(value As String)
    End Sub
    Partial Private Sub OnhelplinkChanged()
    End Sub
    Partial Private Sub OnmessageChanging(value As String)
    End Sub
    Partial Private Sub OnmessageChanged()
    End Sub
    Partial Private Sub OnsourceChanging(value As String)
    End Sub
    Partial Private Sub OnsourceChanged()
    End Sub
    Partial Private Sub OnstacktraceChanging(value As String)
    End Sub
    Partial Private Sub OnstacktraceChanged()
    End Sub
    Partial Private Sub OntargetsiteChanging(value As String)
    End Sub
    Partial Private Sub OntargetsiteChanged()
    End Sub
    Partial Private Sub OnApplicationChanging(value As String)
    End Sub
    Partial Private Sub OnApplicationChanged()
    End Sub
    #End Region
		
		Public Sub New()
			MyBase.New
			OnCreated
		End Sub
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_id", DbType:="UniqueIdentifier NOT NULL", IsPrimaryKey:=true)>  _
		Public Property id() As System.Guid
			Get
				Return Me._id
			End Get
			Set
				If ((Me._id = value)  _
							= false) Then
					Me.OnidChanging(value)
					Me.SendPropertyChanging
					Me._id = value
					Me.SendPropertyChanged("id")
					Me.OnidChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_action", DbType:="NVarChar(255)")>  _
		Public Property action() As String
			Get
				Return Me._action
			End Get
			Set
				If (String.Equals(Me._action, value) = false) Then
					Me.OnactionChanging(value)
					Me.SendPropertyChanging
					Me._action = value
					Me.SendPropertyChanged("action")
					Me.OnactionChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_controller", DbType:="NVarChar(255)")>  _
		Public Property controller() As String
			Get
				Return Me._controller
			End Get
			Set
				If (String.Equals(Me._controller, value) = false) Then
					Me.OncontrollerChanging(value)
					Me.SendPropertyChanging
					Me._controller = value
					Me.SendPropertyChanged("controller")
					Me.OncontrollerChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_timestamp", DbType:="DateTime")>  _
		Public Property timestamp() As System.Nullable(Of Date)
			Get
				Return Me._timestamp
			End Get
			Set
				If (Me._timestamp.Equals(value) = false) Then
					Me.OntimestampChanging(value)
					Me.SendPropertyChanging
					Me._timestamp = value
					Me.SendPropertyChanged("timestamp")
					Me.OntimestampChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_ipaddress", DbType:="NVarChar(50)")>  _
		Public Property ipaddress() As String
			Get
				Return Me._ipaddress
			End Get
			Set
				If (String.Equals(Me._ipaddress, value) = false) Then
					Me.OnipaddressChanging(value)
					Me.SendPropertyChanging
					Me._ipaddress = value
					Me.SendPropertyChanged("ipaddress")
					Me.OnipaddressChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_helplink", DbType:="NVarChar(500)")>  _
		Public Property helplink() As String
			Get
				Return Me._helplink
			End Get
			Set
				If (String.Equals(Me._helplink, value) = false) Then
					Me.OnhelplinkChanging(value)
					Me.SendPropertyChanging
					Me._helplink = value
					Me.SendPropertyChanged("helplink")
					Me.OnhelplinkChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_message", DbType:="NText", UpdateCheck:=UpdateCheck.Never)>  _
		Public Property message() As String
			Get
				Return Me._message
			End Get
			Set
				If (String.Equals(Me._message, value) = false) Then
					Me.OnmessageChanging(value)
					Me.SendPropertyChanging
					Me._message = value
					Me.SendPropertyChanged("message")
					Me.OnmessageChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_source", DbType:="NVarChar(500)")>  _
		Public Property source() As String
			Get
				Return Me._source
			End Get
			Set
				If (String.Equals(Me._source, value) = false) Then
					Me.OnsourceChanging(value)
					Me.SendPropertyChanging
					Me._source = value
					Me.SendPropertyChanged("source")
					Me.OnsourceChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_stacktrace", DbType:="NText", UpdateCheck:=UpdateCheck.Never)>  _
		Public Property stacktrace() As String
			Get
				Return Me._stacktrace
			End Get
			Set
				If (String.Equals(Me._stacktrace, value) = false) Then
					Me.OnstacktraceChanging(value)
					Me.SendPropertyChanging
					Me._stacktrace = value
					Me.SendPropertyChanged("stacktrace")
					Me.OnstacktraceChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_targetsite", DbType:="NVarChar(500)")>  _
		Public Property targetsite() As String
			Get
				Return Me._targetsite
			End Get
			Set
				If (String.Equals(Me._targetsite, value) = false) Then
					Me.OntargetsiteChanging(value)
					Me.SendPropertyChanging
					Me._targetsite = value
					Me.SendPropertyChanged("targetsite")
					Me.OntargetsiteChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Application", DbType:="NVarChar(255)")>  _
		Public Property Application() As String
			Get
				Return Me._Application
			End Get
			Set
				If (String.Equals(Me._Application, value) = false) Then
					Me.OnApplicationChanging(value)
					Me.SendPropertyChanging
					Me._Application = value
					Me.SendPropertyChanged("Application")
					Me.OnApplicationChanged
				End If
			End Set
		End Property
		
		Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging
		
		Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
		
		Protected Overridable Sub SendPropertyChanging()
			If ((Me.PropertyChangingEvent Is Nothing)  _
						= false) Then
				RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
			End If
		End Sub
		
		Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
			If ((Me.PropertyChangedEvent Is Nothing)  _
						= false) Then
				RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
			End If
		End Sub
	End Class
End Namespace