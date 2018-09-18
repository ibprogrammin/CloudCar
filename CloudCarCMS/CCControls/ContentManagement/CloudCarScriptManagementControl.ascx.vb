Imports CloudCar.CCFramework.Core

Namespace CCControls.ContentManagement

    Public Class CloudCarScriptManagementControl
        Inherits UserControl

        Private ReadOnly _JQueryScriptLocation As String
        Private ReadOnly _MainScriptLocation As String
        Private ReadOnly _AdaptScriptLocation As String

        Private _ScriptLocations As List(Of ScriptLocation)

        Public Sub New()
            _ScriptLocations = New List(Of ScriptLocation)

            'Load default scripts
            _JQueryScriptLocation = "http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"
            _MainScriptLocation = String.Format("/CCTemplates/{0}/Scripts/main.js", Settings.Theme)
            _AdaptScriptLocation = String.Format("/CCTemplates/{0}/Scripts/adapt.js", Settings.Theme)

            AddScriptLocation(New ScriptLocation(_JQueryScriptLocation, 1))
            AddScriptLocation(New ScriptLocation(_MainScriptLocation, 5))
            'AddScriptLocation(New ScriptLocation(_AdaptScriptLocation, 6))
        End Sub

        Public Sub AddScriptLocation(ByVal NewScriptLocation As ScriptLocation)
            If Not CheckScriptAlreadyAdded(NewScriptLocation.Location) Then
                _ScriptLocations.Add(NewScriptLocation)
            Else
                UpdateScriptLocation(NewScriptLocation)
            End If
        End Sub

        Public Function CheckScriptAlreadyAdded(ByVal Location As String) As Boolean
            Return (From CurrentScriptLocation In _ScriptLocations Where CurrentScriptLocation.Location.ToLower Like Location.ToLower Select CurrentScriptLocation).Count > 0
        End Function

        Public Sub UpdateScriptLocation(ByVal Location As ScriptLocation)
            Dim CurrentLocation As ScriptLocation = (From CurrentScriptLocation In _ScriptLocations _
                                                     Where CurrentScriptLocation.Location.ToLower Like Location.Location.ToLower _
                                                     Select CurrentScriptLocation).FirstOrDefault

            CurrentLocation.SortOrder = Location.SortOrder
        End Sub

        Protected Overrides Sub Render(ByVal CurrentWriter As HtmlTextWriter)
            For Each LocationItem As ScriptLocation In _ScriptLocations.OrderBy(Function(f) f.SortOrder)
                CurrentWriter.WriteLine(LocationItem.GetScriptReferenceMarkup())
            Next

            MyBase.Render(CurrentWriter)
        End Sub

    End Class

    Public Class ScriptLocation
        Private _Location As String
        Private _SortOrder As Integer

        Public Sub New(ByVal Location As String, ByVal SortOrder As Integer)
            _Location = Location
            _SortOrder = SortOrder
        End Sub

        Public Property Location As String
            Get
                Return _Location
            End Get
            Set(ByVal Value As String)
                _Location = Value
            End Set
        End Property

        Public Property SortOrder As Integer
            Get
                Return _SortOrder
            End Get
            Set(ByVal Value As Integer)
                _SortOrder = Value
            End Set
        End Property

        Public Function GetScriptReferenceMarkup() As String
            Return <script src=<%= _Location %> type="text/javascript"></script>.ToString
        End Function

    End Class

End Namespace