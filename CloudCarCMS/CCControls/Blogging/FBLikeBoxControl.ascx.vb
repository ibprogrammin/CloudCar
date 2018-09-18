Namespace CCControls.Blogging

    Partial Public Class FBLikeBoxControl
        Inherits System.Web.UI.UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadFBLikeControl()
            End If
        End Sub

        Private Sub LoadFBLikeControl()
            Dim frame As XElement = <iframe src=<%= "http://www.facebook.com/plugins/likebox.php?id=" & CCFramework.Core.Settings.FacebookLikePageID & "&width=" & Width.ToString & "&connections=" & Connections.ToString & "&stream=" & ShowStream.ToString & "&header=" & ShowHeader.ToString & "&height=" & Height.ToString %> scrolling="no" frameborder="0" style=<%= "border: none; overflow: hidden; width: " & Width.ToString & "px; height: " & Height.ToString & "px;" %> allowTransparency="true"></iframe>

            litFBControl.Text = frame.ToString
        End Sub

        Public Property Width() As Integer
            Get
                If Not ViewState("Width") Is Nothing Then
                    Return CInt(ViewState("Width"))
                Else
                    Return 292
                End If
            End Get
            Set(ByVal value As Integer)
                If Not ViewState("Width") Is Nothing Then
                    ViewState("Width") = value
                Else
                    ViewState.Add("Width", value)
                End If
            End Set
        End Property

        Public Property Height() As Integer
            Get
                If Not ViewState("Height") Is Nothing Then
                    Return CInt(ViewState("Height"))
                Else
                    Return 565
                End If
            End Get
            Set(ByVal value As Integer)
                If Not ViewState("Height") Is Nothing Then
                    ViewState("Height") = value
                Else
                    ViewState.Add("Height", value)
                End If
            End Set
        End Property

        Public Property Connections() As Integer
            Get
                If Not ViewState("Connections") Is Nothing Then
                    Return CInt(ViewState("Connections"))
                Else
                    Return 10
                End If
            End Get
            Set(ByVal value As Integer)
                If Not ViewState("Connections") Is Nothing Then
                    ViewState("Connections") = value
                Else
                    ViewState.Add("Connections", value)
                End If
            End Set
        End Property

        Public Property ShowHeader() As Boolean
            Get
                If Not ViewState("ShowHeader") Is Nothing Then
                    Return CBool(ViewState("ShowHeader"))
                Else
                    Return True
                End If
            End Get
            Set(ByVal value As Boolean)
                If Not ViewState("ShowHeader") Is Nothing Then
                    ViewState("ShowHeader") = value
                Else
                    ViewState.Add("ShowHeader", value)
                End If
            End Set
        End Property

        Public Property ShowStream() As Boolean
            Get
                If Not ViewState("ShowStream") Is Nothing Then
                    Return CBool(ViewState("ShowStream"))
                Else
                    Return True
                End If
            End Get
            Set(ByVal value As Boolean)
                If Not ViewState("ShowStream") Is Nothing Then
                    ViewState("ShowStream") = value
                Else
                    ViewState.Add("ShowStream", value)
                End If
            End Set
        End Property

    End Class

End Namespace