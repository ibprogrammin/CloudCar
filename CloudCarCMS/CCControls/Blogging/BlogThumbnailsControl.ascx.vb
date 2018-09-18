Namespace CCControls.Blogging

    Partial Public Class BlogThumbnailsControl
        Inherits UserControl

        'TODO add a setting property to set the number of thumbnails to post

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                rptEntries.DataSource = CCFramework.Blogging.BlogController.GetLiveBlogs.OrderByDescending(Function(t) t.DatePosted).Take(ThumbnailCount)

                rptEntries.DataBind()
            End If
        End Sub

        Public Property ThumbnailCount() As Integer
            Get
                If Not ViewState("ThumbnailCount") Is Nothing Then
                    Return CInt(ViewState("ThumbnailCount"))
                Else
                    Return 0
                End If
            End Get
            Set(ByVal value As Integer)
                If Not ViewState("ThumbnailCount") Is Nothing Then
                    ViewState("ThumbnailCount") = value
                Else
                    ViewState.Add("ThumbnailCount", value)
                End If
            End Set
        End Property

    End Class

End Namespace