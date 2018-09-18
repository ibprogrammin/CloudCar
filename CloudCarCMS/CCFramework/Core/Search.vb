Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Core

    Public Class Search

        Private Const _DefaultImageSize As Integer = 80

        Public Shared Function GetFromTerm(ByVal term As String) As List(Of SearchItem)
            Dim index As Integer = 0

            Dim productItems As List(Of SearchItem) = SearchProducts(term, index)
            Dim contentItems As List(Of SearchItem) = SearchContent(term, "Home", index)
            Dim blogItems As List(Of SearchItem) = SearchBlogs(term, index)
            Dim mediaItems As List(Of SearchItem) = SearchMultimedia(term, index)

            Return productItems.Union(contentItems.Union(blogItems.Union(mediaItems))).ToList
        End Function

        Public Shared Function SearchMultimedia(ByVal term As String, ByRef index As Integer) As List(Of SearchItem)
            Dim db As New ContentDataContext

            Dim images = (From i In db.Pictures _
                          Where i.PictureFileName.Contains(term) _
                          Select New With {.id = i.PictureID, .name = i.PictureFileName}).Distinct

            Dim itemCollection As New List(Of SearchItem)

            For Each item In images
                itemCollection.Add(New SearchItem(index, item.name, "", "/images/db/" & item.id & "/full/" & item.name))
                index += 1
            Next

            'Dim dbB As New ContentDataContext

            'Dim videos = (From v In dbB.Videos _
            '            Where v.title.Contains(term) OrElse v.description.Contains(term) OrElse v.file_name.Contains(term) _
            '            Select New With {.id = v.id, .title = v.title, .name = v.file_name}).Distinct

            'For Each item In videos
            '    itemCollection.Add(New ApplicationLayer.SearchItem(Index, item.title, "", "/Video.aspx?id=" & item.id))
            '    Index += 1
            'Next

            Return itemCollection
        End Function

        Public Shared Function SearchContent(ByVal term As String, ByVal root As String, ByRef index As Integer) As List(Of SearchItem)
            Dim db As New ContentDataContext

            Dim pages = (From p In db.ContentPages _
                         Where p.pageTitle.Contains(term) OrElse p.pageContent.Contains(term) _
                         OrElse p.description.Contains(term) OrElse p.keywords.Contains(term) _
                         Select New With {.id = p.id, .title = p.pageTitle, .description = p.description, .permalink = p.permalink}).Distinct

            Dim itemCollection As New List(Of SearchItem)

            For Each item In pages
                itemCollection.Add(New SearchItem(index, item.title, item.description, "/" & root & "/" & item.permalink & ".html"))

                index += 1
            Next

            Return itemCollection
        End Function

        Public Shared Function SearchBlogs(ByVal term As String, ByRef index As Integer) As List(Of SearchItem)
            Dim db As New BlogDataContext

            Dim pages = (From b In db.Blogs _
                         Where b.Title.Contains(term) OrElse b.BlogContent.Contains(term) OrElse b.ContentSummary.Contains(term) _
                         OrElse b.Description.Contains(term) OrElse b.Keywords.Contains(term) _
                         Select New With {.id = b.id, .title = b.Title, .description = b.Description, .permalink = b.Permalink}).Distinct

            Dim itemCollection As New List(Of SearchItem)

            For Each item In pages
                itemCollection.Add(New SearchItem(index, item.title, item.description, "/Blog/" & item.permalink & ".html"))

                index += 1
            Next

            Return itemCollection
        End Function

        Public Shared Function SearchProducts(ByVal term As String, ByRef index As Integer) As List(Of SearchItem)
            Dim db As New CommerceDataContext

            Dim products = (From p In db.Products _
                            Join c In db.Categories On c.ID Equals p.CategoryID _
                            Where p.Name.Contains(term) OrElse p.Description.Contains(term) _
                            OrElse c.Name.Contains(term) OrElse c.Description.Contains(term) OrElse c.Keywords.Contains(term) _
                            Select New With {.id = p.ID, .title = p.Name, .description = p.Description, .category = c.Name, .categoryPermalink = c.Permalink, .permalink = p.Permalink, .imageId = p.DefaultImageID}).Distinct

            Dim itemCollection As New List(Of SearchItem)

            For Each item In products
                itemCollection.Add(New SearchItem(index, _
                                                  item.title, _
                                                  item.description, _
                                                  "/Shop/" & item.categoryPermalink & "/" & item.permalink & ".html",
                                                  String.Format("/images/db/{0}/{1}/{2}", _
                                                                item.imageId, _
                                                                _DefaultImageSize, _
                                                                PictureController.GetPictureFilename(item.imageId)) _
                                                            ))

                index += 1
            Next

            Return itemCollection
        End Function

    End Class


    Public Class SearchItem
        Private _id As Integer
        Private _title As String
        Private _description As String
        Private _url As String
        Private _imageUrl As String

        Public Sub New(ByVal id As Integer, ByVal title As String, ByVal description As String, ByVal url As String)
            _id = id
            _title = title
            _description = description
            _url = url
        End Sub

        Public Sub New(ByVal id As Integer, ByVal title As String, ByVal description As String, ByVal url As String, ImageUrl As String)
            _id = id
            _title = title
            _description = description
            _url = url
            _imageUrl = ImageUrl
        End Sub

        Public ReadOnly Property ID() As Integer
            Get
                Return _id
            End Get
        End Property

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Public ReadOnly Property ShortDescription() As String
            Get
                Return TextFunctions.StripShortString(_description, 180)
            End Get
        End Property

        Public Property URL() As String
            Get
                Return _url
            End Get
            Set(ByVal value As String)
                _url = value
            End Set
        End Property

        Public Property ImageUrl As String
            Get
                Return _imageUrl
            End Get
            Set(Value As String)
                _imageUrl = Value
            End Set
        End Property

    End Class

End Namespace