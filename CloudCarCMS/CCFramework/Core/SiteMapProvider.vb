Imports System.Xml
Imports CloudCar.CCFramework.Core

Namespace CCFramework.Generic

    'TODO implement this to add and remove pages from the Blogs.aspx, ContentPageDetails.aspx, Categories.aspx, Brands.aspx, ProductDetails.aspx
    'TODO add the ability to nest pages in a parent child relationship.
    Public Class SiteMapProvider

        Protected Function LoadSitemap(ByVal Context As HttpContext) As XmlDocument

            Dim xDoc As New XmlDocument()

            xDoc.PreserveWhitespace = False
            xDoc.Load(Context.Request.MapPath(Context.Request.ApplicationPath & Settings.GoogleSitemapPath))

            Return xDoc

        End Function

        Public Sub AddNewPage(ByVal Url As String, ByVal ChangeFrequency As String, ByVal Priority As Single, ByVal Context As HttpContext)

            Dim doc As XmlDocument = LoadSitemap(Context)
            Dim xmlns As String = "http://www.google.com/schemas/sitemap/0.90"

            Dim locationNode As XmlNode = doc.CreateElement("loc", xmlns)
            locationNode.InnerText = GetHostAddress() & Url

            Dim lastmodNode As XmlNode = doc.CreateElement("lastmod", xmlns)
            lastmodNode.InnerText = Date.Today.ToString("yyyy-MM-dd")

            Dim changefreqNode As XmlNode = doc.CreateElement("changefreq", xmlns)
            changefreqNode.InnerText = ChangeFrequency

            Dim priorityNode As XmlNode = doc.CreateElement("priority", xmlns)
            priorityNode.InnerText = Priority.ToString()

            Dim general As XmlNode = doc.CreateElement("url", xmlns)

            general.AppendChild(locationNode)
            general.AppendChild(lastmodNode)
            general.AppendChild(changefreqNode)
            general.AppendChild(priorityNode)

            doc.FirstChild.NextSibling.AppendChild(general)

            doc.Save(Context.Request.MapPath(Context.Request.ApplicationPath & Core.Settings.GoogleSitemapPath))

        End Sub

        Public Sub DeletePage(ByVal Url As String, ByVal Context As HttpContext)

            Dim doc As XmlDocument = LoadSitemap(Context)
            Dim xmlns As String = "http://www.google.com/schemas/sitemap/0.90"

            Dim list As XmlNodeList = doc.FirstChild.NextSibling.ChildNodes

            For Each node As XmlNode In list

                If node.FirstChild.InnerText = GetHostAddress() & Url Then
                    node.ParentNode.RemoveChild(node)
                End If

            Next

            doc.Save(Context.Request.MapPath(Context.Request.ApplicationPath & Settings.GoogleSitemapPath))

        End Sub

        Public Sub UpdatePage(ByVal OldUrl As String, ByVal NewUrl As String, ByVal ChangeFrequency As String, ByVal Priority As Single, ByVal Context As HttpContext)
            
            DeletePage(GetHostAddress() & OldUrl, Context)
            AddNewPage(GetHostAddress() & NewUrl, ChangeFrequency, Priority, Context)

        End Sub

        Public Shared Function GetHostAddress() As String
            Dim CurrentHostAddress As String

            If Settings.FullSSL Then
                CurrentHostAddress = String.Format("https://{0}", Settings.HostName)
            Else
                CurrentHostAddress = String.Format("http://{0}", Settings.HostName)
            End If

            Return CurrentHostAddress
        End Function

    End Class

End Namespace