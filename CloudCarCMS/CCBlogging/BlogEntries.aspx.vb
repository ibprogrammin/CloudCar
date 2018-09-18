Imports CloudCar.CCFramework.Core

Namespace CCBlogging

    Partial Public Class BlogEntries
        Inherits RoutablePage

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                litBlogDescription.Text = Settings.BlogDescription

                If Settings.EnableSSL = True And Settings.FullSSL Then
                    PageCanonicalMeta.Attributes("href") = String.Format("https://{0}/Blog/Index.html", Settings.HostName)
                Else
                    PageCanonicalMeta.Attributes("href") = String.Format("http://{0}/Blog/Index.html", Settings.HostName)
                End If

                LoadBlogs()
            End If
        End Sub

        Private Sub LoadBlogs()
            'Dim BlogEntries As List(Of Blog) = SMBlogEngine.BlogController.GetLiveBlogs.ToList 'OrderByDescending(Function(t) t.DatePosted).ToList
            Dim BlogEntries = CCFramework.Blogging.BlogController.GetLiveBlogs.OrderByDescending(Function(b) b.DatePosted)

            'Dim dcs As New Serialization.DataContractSerializer(GetType(Blog))

            'Dim sb As StringBuilder = New StringBuilder()
            'Dim writer As System.Xml.XmlWriter = System.Xml.XmlWriter.Create(sb)

            'dcs.WriteObject(writer, BlogEntries)
            'writer.Close()

            'Dim xml As String = sb.ToString()

            Dim dt As New DataTable()

            dt.Columns.Add(New DataColumn("ID", GetType(Integer)))
            dt.Columns.Add(New DataColumn("Title", GetType(String)))
            dt.Columns.Add(New DataColumn("Permalink", GetType(String)))
            dt.Columns.Add(New DataColumn("ContentSummary", GetType(String)))
            dt.Columns.Add(New DataColumn("ImageLink", GetType(String)))
            dt.Columns.Add(New DataColumn("Author", GetType(String)))
            dt.Columns.Add(New DataColumn("DatePosted", GetType(String)))

            For Each item As CCFramework.Model.Blog In BlogEntries
                Dim row As DataRow = dt.NewRow

                row("ID") = item.id
                row("Title") = item.Title
                row("Permalink") = item.Permalink
                row("ContentSummary") = item.ContentSummary
                row("ImageLink") = item.ImageLink
                row("Author") = item.Author.Name
                row("DatePosted") = item.DatePosted

                dt.Rows.Add(row)
            Next

            rptBlogEntries.DataSource = dt
            rptBlogEntries.DataBind()
        End Sub

    End Class
End Namespace