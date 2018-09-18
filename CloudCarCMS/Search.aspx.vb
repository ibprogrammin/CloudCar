Imports CloudCar.CCContentManagement
Imports CloudCar.CCFramework.Core

Partial Public Class Search
    Inherits CloudCarContentPage

    Public Sub New()
        MyBase.New()
        Permalink = Settings.SearchPage
    End Sub

    Private _Term As String
    Public Property Term As String
        Get
            Return _Term
        End Get
        Set(Value As String)
            _Term = Value
        End Set
    End Property

    Protected Overrides Sub OnInit(ByVal Args As EventArgs)
        If Term Is Nothing Then
            Term = Request.QueryString("q") '(From CurrentValues In RequestContext.RouteData.Values Where CurrentValues.Key = "term" Select New With {.id = CurrentValues.Value}).FirstOrDefault.id.ToString
        End If

        MyBase.OnInit(Args)
    End Sub

    Dim Index As Integer = 0

    Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                If Not Session("SearchQuery") Is Nothing Then
                    SearchContent(Session("SearchQuery").ToString)
                ElseIf Not Term Is Nothing Then
                    SearchContent(Term)
                End If
            Catch Ex As NullReferenceException

            End Try
        End If
    End Sub
    
    Private Sub SearchContent(ByVal SearchQuery As String)
        Try
            If Not SearchQuery = String.Empty Then
                Dim SearchProductItems As List(Of CCFramework.Core.SearchItem) = CCFramework.Core.Search.SearchProducts(Server.HtmlDecode(SearchQuery), Index)

                rptProducts.DataSource = SearchProductItems
                rptProducts.DataBind()

                If SearchProductItems.Count > 0 Then
                    rptProducts.Visible = True
                End If

                Dim SearchMediaItems As List(Of CCFramework.Core.SearchItem) = CCFramework.Core.Search.SearchMultimedia(Server.HtmlDecode(SearchQuery), Index)

                rptMultimedia.DataSource = SearchMediaItems
                rptMultimedia.DataBind()

                If SearchMediaItems.Count > 0 Then
                    rptMultimedia.Visible = True
                End If

                Dim SearchContentItems As List(Of CCFramework.Core.SearchItem) = CCFramework.Core.Search.SearchContent(Server.HtmlDecode(SearchQuery), CCFramework.Core.Settings.Root, Index)

                rptContent.DataSource = SearchContentItems
                rptContent.DataBind()

                If SearchContentItems.Count > 0 Then
                    rptContent.Visible = True
                End If

                Dim SearchBlogItems As List(Of CCFramework.Core.SearchItem) = CCFramework.Core.Search.SearchBlogs(Server.HtmlDecode(SearchQuery), Index)

                rptBlogs.DataSource = SearchBlogItems
                rptBlogs.DataBind()

                If SearchBlogItems.Count > 0 Then
                    rptBlogs.Visible = True
                End If

                litResults.Text = "Your search returned " & Index & " Result(s)"
            End If
        Catch ex As Exception
            StatusMessageLiteral.Text = "Oops! We are sorry, but there was an error processing your search request."
        End Try
    End Sub

End Class