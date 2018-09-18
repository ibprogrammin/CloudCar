Imports System.ComponentModel

Namespace CCControls.ContentManagement.PageModule

    Public MustInherit Class RotatorControl
        Inherits UserControl

        Private _PageId As Integer
        Private _ScriptManagementControlId As String

        <Bindable(True), _
        Category("Behavior"), _
        DefaultValue(0), _
        Description("The Page Id of the page that is being loaded")> _
        Public Property PageId() As Integer
            Get
                Return _PageId
            End Get
            Set(ByVal Value As Integer)
                _PageId = Value

                CheckControlInitialized()
            End Set
        End Property

        <Bindable(True), _
        Category("Behavior"), _
        DefaultValue(""), _
        Description("The Id of the Script Management Control to load the control scripts into")> _
        Public Property ScriptManagementControlId() As String
            Get
                Return _ScriptManagementControlId
            End Get
            Set(ByVal Value As String)
                _ScriptManagementControlId = Value

                CheckControlInitialized()
            End Set
        End Property

        <Bindable(True), _
        Category("Behavior"), _
        DefaultValue(940), _
        Description("The maximum size the control can be")> _
        Public Property MaxSize() As Integer
            Get
                If ViewState("MaxSize") Is Nothing Then
                    Return 940
                Else
                    Return CInt(ViewState("MaxSize"))
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("MaxSize") = Value

                CheckControlInitialized()
            End Set
        End Property

        Private Sub CheckControlInitialized()
            If Not PageId = Nothing And Not ScriptManagementControlId Is Nothing And Not ScriptManagementControlId = String.Empty Then
                LoadRotator()
            End If
        End Sub

        Protected Overridable Sub LoadRotator()
        End Sub

        Protected Overrides Sub OnInit(ByVal Args As EventArgs)
            MyBase.OnInit(Args)
            Page.RegisterRequiresControlState(Me)
        End Sub

    End Class

End Namespace