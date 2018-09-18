Namespace CCControls.Blogging

    Partial Public Class RecentBlogsControl
        Inherits UserControl

        'TODO add a property to set the number of blog posts to list

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                rptEntries.DataSource = CCFramework.Blogging.BlogController.GetLiveBlogs.OrderByDescending(Function(t) t.DatePosted).Take(3)
                rptEntries.DataBind()
            End If
        End Sub

    End Class

End Namespace