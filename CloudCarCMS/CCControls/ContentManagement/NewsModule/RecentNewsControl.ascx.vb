Imports CloudCar.CCFramework.ContentManagement.NewsModule

Namespace CCControls.ContentManagement.NewsModule

    Partial Public Class RecentNewsControl
        Inherits UserControl

        Protected Overrides Sub OnLoad(ByVal E As EventArgs)
            If Not Page.IsPostBack Then
                LoadNews()
            End If
        End Sub

        Public Sub LoadNews()
            Try
                NewsRepeater.DataSource = NewsController.GetApprovedNews().OrderByDescending(Function(t) t.PublishDate).Take(Count)
                NewsRepeater.DataBind()
            Catch Ex As Exception

            End Try
        End Sub

        Public Property Count() As Integer
            Get
                If Not ViewState("Count") Is Nothing Then
                    Return CType(ViewState("Count"), Integer)
                Else
                    Return 2
                End If
            End Get
            Set(ByVal Value As Integer)
                If Not ViewState("Count") Is Nothing Then
                    ViewState("Count") = Value
                Else
                    ViewState.Add("Count", Value)
                End If
            End Set
        End Property

    End Class

End Namespace