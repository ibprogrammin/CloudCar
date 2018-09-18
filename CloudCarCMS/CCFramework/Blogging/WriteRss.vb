Imports Microsoft.VisualBasic
Imports System
Imports System.Web
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Linq
Imports CloudCar.CCFramework.Model

Namespace CCFramework.Blogging.RssTwoPointZero

    Public Class WriteRss
        Private pTitle As String
        Private pLink As String
        Private pDescription As String
        Private pTtl As String
        Private pName As String
        Private pUser As String
        Private pLanguage As String
        Private pCopyright As String
        Private pCategory As String

        Private pImage As RssImage

        Private pItems As RssItemCollection

#Region "Constructors"

        'Public Sub WriteRss()

        'End Sub

        Public Sub WriteRss(ByVal title As String, ByVal link As String)

        End Sub

        Public Sub WriteRss(ByVal title As String, ByVal link As String, ByVal description As String)

        End Sub

        Public Sub WriteRss(ByVal title As String, ByVal link As String, ByVal description As String, _
                    ByVal ttl As String)

        End Sub

        Public Sub WriteRss(ByVal title As String, ByVal link As String, ByVal description As String, _
                    ByVal ttl As String, ByVal name As String)

        End Sub

        Public Sub WriteRss(ByVal title As String, ByVal link As String, ByVal description As String, _
                    ByVal ttl As String, ByVal name As String, ByVal user As String)

        End Sub

        Public Sub WriteRss(ByVal title As String, ByVal link As String, ByVal description As String, _
                    ByVal ttl As String, ByVal name As String, ByVal user As String, ByVal language As String)

        End Sub

        Public Sub WriteRss(ByVal title As String, ByVal link As String, ByVal description As String, _
                    ByVal ttl As String, ByVal name As String, ByVal user As String, ByVal language As String, _
                        ByVal copyright As String)

        End Sub

        Public Sub WriteRss(ByVal title As String, ByVal link As String, ByVal description As String, _
                    ByVal ttl As String, ByVal name As String, ByVal user As String, ByVal language As String, _
                        ByVal copyright As String, ByVal category As String)

        End Sub

#End Region

#Region "Properties"

        Public Property Title() As String
            Get
                Return Me.pTitle
            End Get
            Set(ByVal value As String)
                Me.pTitle = value
            End Set
        End Property

        Public Property Link() As String
            Get
                Return Me.pLink
            End Get
            Set(ByVal value As String)
                Me.pLink = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return Me.pDescription
            End Get
            Set(ByVal value As String)
                Me.pDescription = value
            End Set
        End Property

        Public Property Ttl() As String
            Get
                Return Me.pTtl
            End Get
            Set(ByVal value As String)
                Me.pTtl = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return Me.pName
            End Get
            Set(ByVal value As String)
                Me.pName = value
            End Set
        End Property

        Public Property User() As String
            Get
                Return Me.pUser
            End Get
            Set(ByVal value As String)
                Me.pUser = value
            End Set
        End Property

        Public Property Language() As String
            Get
                Return Me.pLanguage
            End Get
            Set(ByVal value As String)
                Me.pLanguage = value
            End Set
        End Property

        Public Property Copyright() As String
            Get
                Return Me.pCopyright
            End Get
            Set(ByVal value As String)
                Me.pCopyright = value
            End Set
        End Property

        Public Property Category() As String
            Get
                Return Me.pCategory
            End Get
            Set(ByVal value As String)
                Me.pCategory = value
            End Set
        End Property

        Public Property Items(ByVal index As Integer) As RssItem
            Get
                Return pItems.Item(index)
            End Get
            Set(ByVal value As RssItem)
                pItems.Item(index) = value
            End Set
        End Property

        Public Property Image() As RssImage
            Get
                Return pImage
            End Get
            Set(ByVal value As RssImage)
                pImage = value
            End Set
        End Property

#End Region

#Region "Functions"

        Public Function WriteToXml(ByVal file As String) As Boolean
            Dim xwriter As XmlTextWriter

            Try
                xwriter = New XmlTextWriter(file, System.Text.Encoding.UTF8)

                xwriter.Formatting = Formatting.Indented

                xwriter.WriteStartDocument()
                xwriter.WriteStartElement("rss")
                xwriter.WriteAttributeString("version", "2.0")

                xwriter.WriteStartElement("Channel")

                xwriter.WriteElementString("title", Me.Title)
                xwriter.WriteElementString("link", Me.Link)
                xwriter.WriteElementString("description", Me.Description)
                xwriter.WriteElementString("ttl", Me.Ttl)
                xwriter.WriteElementString("name", Me.Name)
                xwriter.WriteElementString("user", Me.User)
                xwriter.WriteElementString("language", Me.Language)
                xwriter.WriteElementString("copyright", Me.Copyright)

                xwriter.WriteStartElement("image")
                xwriter.WriteElementString("title", Me.Image.Title)
                xwriter.WriteElementString("link", Me.Image.Link)
                xwriter.WriteElementString("url", Me.Image.Url)
                xwriter.WriteEndElement()


                For i As Integer = 0 To pItems.Count - 1
                    xwriter.WriteStartElement("item")
                    xwriter.WriteElementString("title", Me.Items(i).Title)
                    xwriter.WriteStartElement("description")
                    xwriter.WriteCData(Me.Items(i).Description.Replace("<", "&lt;").Replace(">", "&gt;"))
                    xwriter.WriteEndElement()
                    xwriter.WriteElementString("link", Me.Items(i).Link)
                    xwriter.WriteElementString("pubDate", Me.Items(i).PubDate.ToString)
                    xwriter.WriteElementString("guid", Me.Items(i).Guid)
                    xwriter.WriteElementString("comments", Me.Items(i).Link + "#comments")
                    xwriter.WriteEndElement()
                Next

                xwriter.WriteEndElement()
                xwriter.WriteEndElement()
                xwriter.WriteEndDocument()

                xwriter.Close()

                Return True
            Catch ex As Exception
                Throw New Exception
                Return False
            End Try

        End Function

        Public Sub AddItem(ByVal item As RssItem)
            pItems.Add(item)
        End Sub

        Public Sub AddItem(ByVal title As String, ByVal link As String, ByVal description As String)

            Dim item As RssItem = New RssTwoPointZero.RssItem(title, link, description)

            pItems.Add(item)
        End Sub

        Public Sub AddItem(ByVal title As String, ByVal link As String, ByVal description As String, _
                        ByVal pubdate As Date)

            Dim item As RssItem = New RssTwoPointZero.RssItem(title, link, description, pubdate)

            pItems.Add(item)
        End Sub

        Public Sub AddItem(ByVal title As String, ByVal link As String, ByVal description As String, _
                        ByVal pubdate As Date, ByVal guid As String)

            Dim item As RssItem = New RssTwoPointZero.RssItem(title, link, description, pubdate, guid)

            pItems.Add(item)
        End Sub

        Public Sub SetDefaults()
            Me.Title = "Xml RSS file Writer Class"
            Me.Link = "http://"
            Me.Description = "Xml RSS file Writer Class"
            Me.Ttl = "10"
            Me.Name = "Default"
            Me.Copyright = "Copyright " & Date.Now()
            Me.Language = "en-us"
            Me.User = "Default"

            'Me.Image.SetDefaults()
        End Sub

#End Region

    End Class

    Public Class RssChannel
        Private pTitle As String
        Private pLink As String
        Private pDescription As String
        Private pWebMaster As String
        Private pManagingEditor As String
        Private pTtl As String
        Private pLanguage As String
        Private pCopyright As String
        Private pCategory As String

        Private pImage As RssImage

        'Private pItems2 As RssItem()
        Private pItems As RssItemCollection

#Region "Constructors"

        Public Sub New(ByVal title As String, ByVal link As String, _
                    Optional ByVal description As String = "Xml RSS Channel Class", _
                    Optional ByVal webmaster As String = "Default", _
                    Optional ByVal managingeditor As String = "Default", _
                    Optional ByVal ttl As String = "60", _
                    Optional ByVal language As String = "en-us", _
                    Optional ByVal copyright As String = "", _
                    Optional ByVal category As String = "")

            Me.Title = title
            Me.Link = link
            Me.Description = description
            Me.WebMaster = webmaster
            Me.ManagingEditor = managingeditor
            Me.Ttl = ttl
            Me.Language = language
            Me.Copyright = copyright
            Me.Category = category

            pItems = New RssItemCollection()
        End Sub

#End Region

#Region "Properties"

        Public Property Title() As String
            Get
                Return Me.pTitle
            End Get
            Set(ByVal value As String)
                Me.pTitle = value
            End Set
        End Property

        Public Property Link() As String
            Get
                Return Me.pLink
            End Get
            Set(ByVal value As String)
                Me.pLink = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return Me.pDescription
            End Get
            Set(ByVal value As String)
                Me.pDescription = value
            End Set
        End Property

        Public Property Ttl() As String
            Get
                Return Me.pTtl
            End Get
            Set(ByVal value As String)
                Me.pTtl = value
            End Set
        End Property

        Public Property WebMaster() As String
            Get
                Return Me.pWebMaster
            End Get
            Set(ByVal value As String)
                Me.pWebMaster = value
            End Set
        End Property

        Public Property ManagingEditor() As String
            Get
                Return Me.pManagingEditor
            End Get
            Set(ByVal value As String)
                Me.pManagingEditor = value
            End Set
        End Property

        Public Property Language() As String
            Get
                Return Me.pLanguage
            End Get
            Set(ByVal value As String)
                Me.pLanguage = value
            End Set
        End Property

        Public Property Copyright() As String
            Get
                Return Me.pCopyright
            End Get
            Set(ByVal value As String)
                Me.pCopyright = value
            End Set
        End Property

        Public Property Category() As String
            Get
                Return Me.pCategory
            End Get
            Set(ByVal value As String)
                Me.pCategory = value
            End Set
        End Property

        Public Property Items(ByVal index As Integer) As RssItem
            Get
                Return pItems(index)
            End Get
            Set(ByVal value As RssItem)
                pItems(index) = value
            End Set
        End Property

        Public Property Image() As RssImage
            Get
                Return pImage
            End Get
            Set(ByVal value As RssImage)
                pImage = value
            End Set
        End Property

#End Region

#Region "Functions"

        Public Sub AddItem(ByVal item As RssItem)
            pItems.Add(item)
        End Sub

        Public Sub AddItem(ByVal title As String, ByVal link As String, ByVal description As String)

            Dim item As RssItem = New RssTwoPointZero.RssItem(title, link, description)

            pItems.Add(item)
        End Sub

        Public Sub AddItem(ByVal title As String, ByVal link As String, ByVal description As String, _
                        ByVal pubdate As Date)

            Dim item As RssItem = New RssTwoPointZero.RssItem(title, link, description, pubdate)

            pItems.Add(item)
        End Sub

        Public Sub AddItem(ByVal title As String, ByVal link As String, ByVal description As String, _
                        ByVal pubdate As Date, ByVal guid As String)

            Dim item As RssItem = New RssTwoPointZero.RssItem(title, link, description, pubdate, guid)

            pItems.Add(item)
        End Sub

        ''' <summary>
        ''' Reads from an Sql database and loads the data into the RssItemCollection
        ''' </summary>
        ''' <param name="table">An Sql table containing the Rss data</param>
        ''' <param name="connectionString">A Sql connection string to connect to the data server</param>
        ''' <returns>Boolean value indicating the success of the function</returns>
        ''' <remarks></remarks>
        Public Function ReadSqlItems(ByVal table As String, ByVal connectionString As String, Optional ByVal link As String = "") As Integer
            Dim query As String = "SELECT TOP 15 a.ID, a.Title, a.Contents, a.Entered, a.description, b.UserName FROM " & table & " as a, aspnet_Users as b WHERE a.Author_ID = b.UserId ORDER BY Entered DESC"
            Dim conn As SqlConnection = New SqlConnection(connectionString)
            Dim command As SqlCommand = New SqlCommand(query, conn)
            Dim reader As SqlDataReader = Nothing

            Try
                conn.Open()

                reader = command.ExecuteReader()
                Dim i As Integer = 0
                While reader.Read

                    Dim tempitem As RssItem

                    tempitem = New RssItem()
                    tempitem.Title = reader.GetString(1)
                    If reader.GetString(2).Length > 300 Then
                        tempitem.Description = reader.GetString(4).Substring(0, 300)
                    Else
                        tempitem.Description = reader.GetString(4)
                    End If
                    tempitem.PubDate = reader.GetDateTime(3)
                    tempitem.Link = link & reader.GetValue(0).ToString()
                    tempitem.Guid = reader.GetValue(0).ToString
                    tempitem.Author = reader.GetString(5)

                    Me.AddItem(tempitem)
                    i += 1
                End While
                Return i
            Catch e As SqlException
                'Response.Write(e.Message)
                Return -1
            Finally
                If Not reader Is Nothing Then reader.Close()

                conn.Close()
            End Try
        End Function

        ''' <summary>
        ''' Reads from an Linq data source and loads the data into the RssItemCollection
        ''' </summary>
        ''' <param name="items">An enumerable list of Blog Entries</param>
        ''' <param name="link">The begining part of the url where the blog entry can be located</param>
        ''' <returns>Integer value indicating the number of rows added</returns>
        ''' <remarks></remarks>
        Public Function ReadLinqItems(ByVal items As IEnumerable(Of Blog), Optional ByVal link As String = "") As Integer
            Try
                Dim i As Integer = 0

                For Each item In items.OrderByDescending(Function(b) b.DatePosted)
                    Dim tempitem As New RssItem()

                    'Dim regex As New Regex("</?(.*)>", )
                    Dim Description = Regex.Replace(item.ContentSummary, "<[^>]*>", String.Empty, RegexOptions.IgnoreCase Or RegexOptions.Multiline)

                    tempitem.Title = item.Title
                    tempitem.Description = Description
                    tempitem.PubDate = item.DatePosted
                    tempitem.Link = link & item.Permalink & ".html"
                    tempitem.Guid = item.Guid.ToString
                    tempitem.Author = item.Author.Name

                    Me.AddItem(tempitem)

                    i += 1
                Next

                Return i
            Catch e As SqlException
                Return -1
            End Try
        End Function

        ''' <summary>
        ''' Writes a properly parsed xml+rss file to the given filename
        ''' </summary>
        ''' <param name="file"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function WriteToXml(ByVal file As String) As Boolean
            Dim xwriter As XmlTextWriter

            'Try
            xwriter = New XmlTextWriter(file, System.Text.Encoding.UTF8)

            xwriter.Formatting = Formatting.Indented

            xwriter.WriteStartDocument()
            xwriter.WriteStartElement("rss")
            xwriter.WriteAttributeString("version", "2.0")
            'xmlns:      atom = "http://www.w3.org/2005/Atom"
            xwriter.WriteAttributeString("atom", "xmlns", "http://www.w3.org/2005/Atom")

            xwriter.WriteStartElement("channel")

            xwriter.WriteElementString("title", Me.Title)
            xwriter.WriteElementString("link", Me.Link)

            '<atom:link href="http://dallas.example.com/rss.xml" rel="self" type="application/rss+xml" />
            xwriter.WriteElementString("description", Me.Description)
            xwriter.WriteElementString("lastBuildDate", Date.Now().ToString)

            If Not Me.ManagingEditor Is Nothing Then
                xwriter.WriteElementString("managingEditor", Me.ManagingEditor)
            End If
            If Not Me.WebMaster Is Nothing Then
                xwriter.WriteElementString("webMaster", Me.WebMaster)
            End If
            If Not Me.Ttl Is Nothing Then
                xwriter.WriteElementString("ttl", Me.Ttl)
            End If
            If Not Me.Language Is Nothing Then
                xwriter.WriteElementString("language", Me.Language)
            End If
            If Not Me.Copyright Is Nothing Then
                xwriter.WriteElementString("copyright", Me.Copyright)
            End If
            If Not Me.Category Is Nothing Then
                xwriter.WriteElementString("category", Me.Category)
            End If

            If Not Me.Image Is Nothing Then
                xwriter.WriteStartElement("image")
                xwriter.WriteElementString("title", Me.Image.Title)
                xwriter.WriteElementString("link", Me.Image.Link)
                xwriter.WriteElementString("url", Me.Image.Url)
                xwriter.WriteEndElement()
            End If

            For i As Integer = 0 To pItems.Count - 1
                xwriter.WriteStartElement("item")
                xwriter.WriteElementString("title", Me.Items(i).Title)
                xwriter.WriteStartElement("description")
                xwriter.WriteCData(Me.Items(i).Description.Replace("<", "&lt;").Replace(">", "&gt;"))
                xwriter.WriteEndElement()
                xwriter.WriteElementString("link", Me.Items(i).Link)
                xwriter.WriteElementString("pubDate", Me.Items(i).PubDate.ToString)
                xwriter.WriteElementString("guid", Me.Items(i).Guid)
                xwriter.WriteElementString("comments", Me.Items(i).Link & "#comments")
                xwriter.WriteElementString("author", Me.Items(i).Author)
                xwriter.WriteEndElement()
            Next


            xwriter.WriteEndElement()
            xwriter.WriteEndElement()
            xwriter.WriteEndDocument()

            xwriter.Close()

            Return True
            'Catch ex As Exception
            'Throw New Exception
            'Return False
            'End Try

        End Function

        ''' <summary>
        ''' Sets the default values of the RssChannel class
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetDefaults()
            Me.Title = "Xml RSS Channel Class"
            Me.Link = "http://"
            Me.Description = "Xml RSS Channel Class"
            Me.Ttl = "10"
            Me.WebMaster = "default@default.com"
            Me.Copyright = "Copyright " & Date.Now()
            Me.Language = "en-us"
            Me.ManagingEditor = "default@default.com"

            'Me.Image.SetDefaults()
        End Sub

#End Region

    End Class

    Public Class RssImage
        Private pTitle As String
        Private pLink As String
        Private pUrl As String

#Region "Constructors"

        Public Sub New(ByVal title As String, ByVal link As String, ByVal url As String)
            Me.Title = title
            Me.Url = url
            Me.Link = link
        End Sub

#End Region

#Region "Properties"

        Public Property Title() As String
            Get
                Return Me.pTitle
            End Get
            Set(ByVal value As String)
                Me.pTitle = value
            End Set
        End Property

        Public Property Link() As String
            Get
                Return Me.pLink
            End Get
            Set(ByVal value As String)
                Me.pLink = value
            End Set
        End Property

        Public Property Url() As String
            Get
                Return Me.pUrl
            End Get
            Set(ByVal value As String)
                Me.pUrl = value
            End Set
        End Property

#End Region

    End Class

    Public Class RssItemCollection
        'Inherits Collections.CollectionBase
        Implements IList
        Private arrList As New ArrayList

#Region "ICollection"

        Public Sub _CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
            arrList.CopyTo(array, index)
        End Sub

        Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
            Get
                Return arrList.Count
            End Get
        End Property

        Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
            Get
                Return arrList.IsSynchronized
            End Get
        End Property

        Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
            Get
                Return Me
            End Get
        End Property

#End Region

#Region "IEnumerable"

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return arrList.GetEnumerator()
        End Function

#End Region

#Region "IList"

        Public Function Add(ByVal value As RssItem) As Integer
            If value Is Nothing Then
                Throw New ArgumentNullException("value", "Collection does not accept null members")
            End If

            Return arrList.Add(value)

        End Function

        Public Function _Add(ByVal value As Object) As Integer Implements System.Collections.IList.Add
            If value Is Nothing Then
                Throw New ArgumentNullException("value", "Collection does not accept null members")
            End If

            If Not TypeOf (value) Is RssItem Then
                Throw New ArgumentException("Collection member must be of type RssItem", "value")
            End If

            Return Add(CType(value, RssItem))
        End Function

        Public Sub Clear()
            arrList.Clear()
        End Sub

        Public Sub _Clear() Implements System.Collections.IList.Clear
            Clear()
        End Sub

        Public Function Contains(ByVal value As RssItem) As Boolean
            If value Is Nothing Then
                Return False
            End If
            Return arrList.Contains(value)
        End Function

        Public Function _Contains(ByVal value As Object) As Boolean Implements System.Collections.IList.Contains
            If value Is Nothing Then
                Return False
            End If

            If Not TypeOf (value) Is RssItem Then
                Return False
            End If

            Return Contains(CType(value, RssItem))
        End Function

        Public Function IndexOf(ByVal value As RssItem) As Integer
            If value Is Nothing Then
                Return -1
            End If

            Return arrList.IndexOf(value)
        End Function

        Public Function _IndexOf(ByVal value As Object) As Integer Implements System.Collections.IList.IndexOf

            If value Is Nothing Then
                Return -1
            End If

            If Not TypeOf (value) Is RssItem Then
                Return -1
            End If

            Return IndexOf(CType(value, RssItem))

        End Function

        Public Sub Insert(ByVal index As Integer, ByVal value As RssItem)
            If value Is Nothing Then
                Throw New ArgumentNullException("value", "Collection does not accept null members")
            End If

            arrList.Insert(index, value)
        End Sub

        Public Sub _Insert(ByVal index As Integer, ByVal value As Object) Implements System.Collections.IList.Insert
            If value Is Nothing Then
                Throw New ArgumentNullException("value", "Collection does not accept null members")
            End If

            If Not TypeOf (value) Is RssItem Then
                Throw New ArgumentException("Collection member must be of type RssItem", "value")
            End If

            Insert(index, CType(value, RssItem))
        End Sub

        Public ReadOnly Property IsFixedSize() As Boolean
            Get
                Return arrList.IsFixedSize
            End Get
        End Property

        Public ReadOnly Property _IsFixedSize() As Boolean Implements System.Collections.IList.IsFixedSize
            Get
                Return IsFixedSize
            End Get
        End Property

        Public ReadOnly Property IsReadOnly() As Boolean
            Get
                Return arrList.IsReadOnly
            End Get
        End Property

        Public ReadOnly Property _IsReadOnly() As Boolean Implements System.Collections.IList.IsReadOnly
            Get
                Return IsReadOnly
            End Get
        End Property

        Public Property _Item(ByVal index As Integer) As Object Implements System.Collections.IList.Item
            Get
                Return CType(arrList(index), RssItem)
            End Get
            Set(ByVal Value As Object)
                If Not Value Is Nothing Then

                    If Not TypeOf (Value) Is RssItem Then
                        Throw New ArgumentException("Collection member must be of type Person", "value")
                    Else
                        arrList(index) = CType(Value, RssItem)
                    End If
                Else
                    Throw New ArgumentNullException("value", "Collection does not accept null members")
                End If
            End Set
        End Property

        Default Public Property Item(ByVal index As Integer) As RssItem
            Get
                Return CType(arrList(index), RssItem)
            End Get
            Set(ByVal Value As RssItem)
                If Not Value Is Nothing Then
                    arrList(index) = Value
                Else
                    Throw New ArgumentNullException("value", "Collection does not accept null members")
                End If
            End Set
        End Property

        Public Sub Remove(ByVal value As RssItem)
            If value Is Nothing Then
                Throw New ArgumentNullException("value", "You cannot remove collection member using null reference")
            End If

            arrList.Remove(value)
        End Sub

        Public Sub _Remove(ByVal value As Object) Implements System.Collections.IList.Remove
            If value Is Nothing Then
                Throw New ArgumentNullException("value", "You cannot remove collection member using null reference")
            End If

            If Not TypeOf (value) Is RssItem Then
                Throw New ArgumentException("You can remove only an object of type RssItem", "value")
            End If

            Remove(CType(value, RssItem))

        End Sub

        Public Sub RemoveAt(ByVal index As Integer)
            arrList.RemoveAt(index)
        End Sub

        Public Sub _RemoveAt(ByVal index As Integer) Implements System.Collections.IList.RemoveAt
            RemoveAt(index)
        End Sub

#End Region

    End Class

    Public Class RssItemEnum
        Implements IEnumerator

        Public _rssitem() As RssItem

        Dim position As Integer = -1

        Public Sub New(ByVal list() As RssItem)
            _rssitem = list
        End Sub

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            position = position + 1
            Return (position < _rssitem.Length)
        End Function

        Public Sub Reset() Implements IEnumerator.Reset
            position = -1
        End Sub

        Public ReadOnly Property Current() As Object Implements IEnumerator.Current
            Get
                Try
                    Return _rssitem(position)
                Catch ex As IndexOutOfRangeException
                    Throw New InvalidOperationException()
                End Try
            End Get
        End Property

    End Class

    Public Class RssItem

        Private pTitle As String
        Private pLink As String
        Private pDescription As String
        Private pAuthor As String
        Private pPubDate As Date
        Private pGuid As String

        'Private pItems As RssItem()

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal title As String, ByVal link As String, ByVal description As String)
            Me.Title = title
            Me.Link = link
            Me.Description = description
            Me.PubDate = Date.Now
            Me.Guid = link
        End Sub

        Public Sub New(ByVal title As String, ByVal link As String, ByVal description As String, _
                    ByVal pubdate As Date)
            Me.Title = title
            Me.Link = link
            Me.Description = description
            Me.PubDate = pubdate
            Me.Guid = link
        End Sub

        Public Sub New(ByVal title As String, ByVal link As String, ByVal description As String, _
                    ByVal pubdate As Date, ByVal guid As String)
            Me.Title = title
            Me.Link = link
            Me.Description = description
            Me.PubDate = pubdate
            Me.Guid = guid
        End Sub

#End Region

#Region "Properties"

        Public Property Title() As String
            Get
                Return Me.pTitle
            End Get
            Set(ByVal value As String)
                Me.pTitle = value
            End Set
        End Property

        Public Property Link() As String
            Get
                Return Me.pLink
            End Get
            Set(ByVal value As String)
                Me.pLink = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return Me.pDescription
            End Get
            Set(ByVal value As String)
                Me.pDescription = value
            End Set
        End Property

        Public Property Author() As String
            Get
                Return Me.pAuthor
            End Get
            Set(ByVal value As String)
                Me.pAuthor = value
            End Set
        End Property

        Public Property PubDate() As Date
            Get
                Return Me.pPubDate
            End Get
            Set(ByVal value As Date)
                Me.pPubDate = value
            End Set
        End Property

        Public Property Guid() As String
            Get
                Return Me.pGuid
            End Get
            Set(ByVal value As String)
                Me.pGuid = value
            End Set
        End Property

#End Region

#Region "Operators"

        Public Shared Operator =(ByVal value1 As RssItem, ByVal value2 As RssItem) As Boolean
            If value1.Title = value2.Title And value1.Description = value2.Description And value1.Link = value2.Link And value1.PubDate = value2.PubDate And value1.Guid = value2.Guid Then
                Return True
            Else
                Return False
            End If
        End Operator

        Public Shared Operator <>(ByVal value1 As RssItem, ByVal value2 As RssItem) As Boolean
            If value1.Title = value2.Title And value1.Description = value2.Description And value1.Link = value2.Link And value1.PubDate = value2.PubDate And value1.Guid = value2.Guid Then
                Return False
            Else
                Return True
            End If
        End Operator

#End Region

    End Class

End Namespace