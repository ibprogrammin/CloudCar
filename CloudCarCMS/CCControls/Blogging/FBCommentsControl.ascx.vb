Namespace CCControls.Blogging

    Partial Public Class FBCommentsControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack And Enabled Then
                'LoadFBCommentsControl()
            End If
        End Sub

        Private Sub LoadFBCommentsControl()

            Dim control As String = "<div id=""fb-root""><fb:comments xid=" & Href & " href=" & Href & " width=" & Width.ToString & "></fb:comments></div>"

            Dim script As XElement = <script>
                                  window.fbAsyncInit = function() {
                                    FB.init({appId: '<%= CCFramework.Core.Settings.FacebookAppID %>', status: true, cookie: true, xfbml: true});
                                  };
                                  (function() {
                                    var e = document.createElement('script'); e.async = true;
                                    e.src = document.location.protocol + '//connect.facebook.net/en_US/all.js';
                                    document.getElementById('fb-root').appendChild(e);
                                  }());
                                </script>

            litFBCommentControl.Text = control
            litFBScript.Text = script.ToString
        End Sub

        Public Property Href() As String
            Get
                If Not ViewState("Href") Is Nothing Then
                    Return CType(ViewState("Href"), String)
                Else
                    Return ""
                End If
            End Get
            Set(ByVal value As String)
                If Not ViewState("Href") Is Nothing Then
                    ViewState("Href") = value
                Else
                    ViewState.Add("Href", value)
                End If

                LoadFBCommentsControl()
            End Set
        End Property

        <DefaultSettingValue("520")> _
        Public Property Width() As Integer
            Get
                If Not ViewState("Width") Is Nothing Then
                    Return CInt(ViewState("Width"))
                Else
                    Return 520
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

        <DefaultSettingValue("True")> _
        Public Property Enabled() As Boolean
            Get
                If Not ViewState("Enabled") Is Nothing Then
                    Return CBool(ViewState("Enabled"))
                Else
                    Return True
                End If
            End Get
            Set(ByVal value As Boolean)
                If Not ViewState("Enabled") Is Nothing Then
                    ViewState("Enabled") = value
                Else
                    ViewState.Add("Enabled", value)
                End If
            End Set
        End Property

    End Class

End Namespace