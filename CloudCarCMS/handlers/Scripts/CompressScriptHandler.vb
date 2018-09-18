#Region "Imports"

Imports System
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO.Compression
Imports System.Web.Caching
Imports System.Net
Imports System.Collections.Generic
Imports System.Net.Sockets

#End Region

Namespace CCFramework.Generic

    Public Class CompressScriptHandler
        Implements IHttpHandler

        Private Const DAYS_IN_CACHE As Integer = 365

        ''' <summary>
        ''' Retrieves the specified remote script using a WebClient.
        ''' </summary>
        ''' <param name="file">The remote URL</param>
        Private Shared Function RetrieveScript(ByVal File As String) As String
            Dim CurrentScript As String = Nothing

            Try
                Dim CurrentScriptUrl As Uri = New Uri(File, UriKind.Absolute)

                Dim CurrentRequest As HttpWebRequest
                CurrentRequest = CType(WebRequest.Create(CurrentScriptUrl), HttpWebRequest)
                CurrentRequest.Method = "GET"
                CurrentRequest.AutomaticDecompression = DecompressionMethods.GZip

                Using CurrentResponse As HttpWebResponse = CType(CurrentRequest.GetResponse(), HttpWebResponse)
                    Using CurrentReader As New StreamReader(CurrentResponse.GetResponseStream())
                        CurrentScript = CurrentReader.ReadToEnd()
                    End Using
                End Using
            Catch CurrentException As SocketException
                ' The remote site is currently down. Try again next time.
            Catch CurrentException As UriFormatException
                ' Only valid absolute URLs are accepted
            End Try

            Return CurrentScript
        End Function

        ''' <summary>
        ''' Strips the whitespace from any .js file.
        ''' </summary>
        Private Shared Function StripWhitespace(ByVal ScriptBody As String) As String

            Dim ScriptLines As String() = ScriptBody.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
            Dim EmptyLines As StringBuilder = New StringBuilder()

            For Each CurrentLine As String In ScriptLines
                Dim TrimedLine As String = CurrentLine.Trim()
                If TrimedLine.Length > 0 And Not TrimedLine.StartsWith("//") Then
                    EmptyLines.AppendLine(TrimedLine.Trim())
                End If
            Next

            ScriptBody = EmptyLines.ToString()

            ' remove C styles comments
            ScriptBody = Regex.Replace(ScriptBody, "/\*.*?\*/", String.Empty, RegexOptions.Compiled Or RegexOptions.Singleline)
            ' trim left
            ScriptBody = Regex.Replace(ScriptBody, "^\s*", String.Empty, RegexOptions.Compiled Or RegexOptions.Multiline)
            ' trim right
            ScriptBody = Regex.Replace(ScriptBody, "\s*[\r\n]", vbCr & vbLf, RegexOptions.Compiled Or RegexOptions.ECMAScript)
            ' remove whitespace beside of left curly braced
            ScriptBody = Regex.Replace(ScriptBody, "\s*{\s*", "{", RegexOptions.Compiled Or RegexOptions.ECMAScript)
            ' remove whitespace beside of coma
            ScriptBody = Regex.Replace(ScriptBody, "\s*,\s*", ",", RegexOptions.Compiled Or RegexOptions.ECMAScript)
            ' remove whitespace beside of semicolon
            ScriptBody = Regex.Replace(ScriptBody, "\s*;\s*", ";", RegexOptions.Compiled Or RegexOptions.ECMAScript)
            ' remove newline after keywords
            ScriptBody = Regex.Replace(ScriptBody, "\r\n(?<=\\b(abstract|boolean|break|byte|case|catch|char|class|const|continue|default|delete|do|double|else|extends|false|final|finally|float|for|function|goto|if|implements|import|in|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|super|switch|synchronized|this|throw|throws|transient|true|try|typeof|var|void|while|with)\r\n)", " ", RegexOptions.Compiled Or RegexOptions.ECMAScript)

            Return ScriptBody
        End Function

        ''' <summary>
        ''' This will make the browser and server keep the output
        ''' in its cache and thereby improve performance.
        ''' </summary>
        Private Shared Sub SetHeaders(ByVal CurrentHash As Integer, ByVal CurrentContext As HttpContext)
            CurrentContext.Response.ContentType = "text/javascript"
            CurrentContext.Response.Cache.VaryByHeaders("Accept-Encoding") = True

            'context.Response.AppendHeader("Expires", DateTime.Now.ToUniversalTime().AddDays(DAYS_IN_CACHE).ToString)
            'context.Response.AppendHeader("Cache-Control", String.Format("max-age=31536000"))

            CurrentContext.Response.Cache.SetExpires(Date.Now.ToUniversalTime().AddDays(DAYS_IN_CACHE))
            CurrentContext.Response.Cache.SetCacheability(HttpCacheability.Public)
            CurrentContext.Response.Cache.SetMaxAge(New TimeSpan(DAYS_IN_CACHE, 0, 0, 0))
            CurrentContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches)
            CurrentContext.Response.Cache.SetETag("""" + CurrentHash.ToString() & """")
        End Sub

#Region "Compression"

        Private Const GZIP As String = "gzip"
        Private Const DEFLATE As String = "deflate"

        Private Shared Sub Compress(ByVal CurrentContext As HttpContext)
            If Not CurrentContext.Request.UserAgent Is Nothing And CurrentContext.Request.UserAgent.Contains("MSIE 6") Then
                Return
            End If

            If IsEncodingAccepted(GZIP) Then
                CurrentContext.Response.Filter = New GZipStream(CurrentContext.Response.Filter, CompressionMode.Compress)
                SetEncoding(GZIP)
            ElseIf IsEncodingAccepted(DEFLATE) Then
                CurrentContext.Response.Filter = New DeflateStream(CurrentContext.Response.Filter, CompressionMode.Compress)
                SetEncoding(DEFLATE)
            End If
        End Sub

        ''' <summary>
        ''' Checks the request headers to see if the specified
        ''' encoding is accepted by the client.
        ''' </summary>
        Private Shared Function IsEncodingAccepted(ByVal Encoding As String) As Boolean
            If Not HttpContext.Current.Request.Headers("Accept-Encoding") Is Nothing Then
                If HttpContext.Current.Request.Headers("Accept-Encoding").Contains(Encoding) Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        '''<summary>
        ''' Adds the specified encoding to the response headers.
        ''' </summary>
        ''' <param name="encoding"></param>
        Private Shared Sub SetEncoding(ByVal encoding As String)
            HttpContext.Current.Response.AppendHeader("Content-Encoding", encoding)
        End Sub

#End Region

        '''<summary>
        '''Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
        '''</summary>
        '''<value></value>
        '''<returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
        Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

        ''' <summary>
        ''' Enables processing of HTTP Web requests by a custom 
        ''' HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        ''' </summary>
        ''' <param name="CurrentContext">An <see cref="T:System.Web.HttpContext"></see> object that provides 
        ''' references to the intrinsic server objects 
        ''' (for example, Request, Response, Session, and Server) used to service HTTP requests.
        ''' </param>
        Public Sub ProcessRequest(ByVal CurrentContext As HttpContext) Implements IHttpHandler.ProcessRequest
            Dim CurrentUrlRoot As String = CurrentContext.Request.Url.GetLeftPart(UriPartial.Authority)
            Dim CurrentPath As String = CurrentContext.Request.QueryString("path")
            Dim CurrentContent As String = String.Empty

            If Not String.IsNullOrEmpty(CurrentPath) Then
                If CurrentContext.Cache(CurrentPath) Is Nothing Then
                    Dim CurrentScripts As String() = CurrentPath.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)

                    For Each ScriptItem As String In CurrentScripts
                        'We only want to serve resource files for security reasons.
                        If ScriptItem.ToUpperInvariant().Contains("RESOURCE.AXD") OrElse ScriptItem.ToUpperInvariant().Contains("_TSM_") Then
                            CurrentContent &= RetrieveScript(CurrentUrlRoot & ScriptItem) & Environment.NewLine
                        End If
                    Next

                    CurrentContent = StripWhitespace(CurrentContent)
                    CurrentContext.Cache.Insert(CurrentPath, CurrentContent, Nothing, Cache.NoAbsoluteExpiration, New TimeSpan(DAYS_IN_CACHE, 0, 0, 0))
                End If
            End If

            CurrentContent = CType(CurrentContext.Cache(CurrentPath), String)

            If Not String.IsNullOrEmpty(CurrentContent) Then
                CurrentContext.Response.Write(CurrentContent)
                SetHeaders(CurrentContent.GetHashCode(), CurrentContext)
                Compress(CurrentContext)
            End If
        End Sub

    End Class


    ''' <summary>
    ''' Find scripts and change the src to the ScriptCompressorHandler.
    ''' </summary>
    Public Class CompressScriptModule
        Implements IHttpModule

#Region "IHttpModule Members"

        Public Sub Dispose() Implements IHttpModule.Dispose
            'Nothing to dispose
        End Sub

        Public Sub Init(ByVal context As HttpApplication) Implements IHttpModule.Init
            AddHandler context.PostRequestHandlerExecute, AddressOf context_BeginRequest
            'context.PostRequestHandlerExecute += New EventHandler(context_BeginRequest)
        End Sub

#End Region

        Public Sub context_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
            Dim app As HttpApplication = TryCast(sender, HttpApplication)

            If TypeOf app.Context.CurrentHandler Is Page And Not app.Request.RawUrl.Contains("serviceframe") Then
                If Not app.Context.Request.Url.Scheme.Contains("https") Then
                    app.Response.Filter = New WebResourceFilter(app.Response.Filter)
                End If
            End If
        End Sub

#Region "Stream filter"

        Private Class WebResourceFilter
            Inherits Stream

            Private _sink As Stream

            Public Sub New(ByVal sink As Stream)
                _sink = sink
            End Sub

            Public Overrides ReadOnly Property CanRead() As Boolean
                Get
                    Return True
                End Get
            End Property

            Public Overrides ReadOnly Property CanSeek() As Boolean
                Get
                    Return True
                End Get
            End Property

            Public Overrides ReadOnly Property CanWrite() As Boolean
                Get
                    Return True
                End Get
            End Property

            Public Overrides Sub Flush()
                _sink.Flush()
            End Sub

            Public Overrides ReadOnly Property Length() As Long
                Get
                    Return 0
                End Get
            End Property

            Private _position As Long
            Public Overrides Property Position() As Long
                Get
                    Return _position
                End Get
                Set(ByVal value As Long)
                    _position = value
                End Set
            End Property

            Public Overrides Function Read(ByVal buffer() As Byte, ByVal offset As Integer, ByVal count As Integer) As Integer
                _sink.Read(buffer, offset, count)

                Return offset + count
            End Function

            Public Overrides Function Seek(ByVal offset As Long, ByVal origin As System.IO.SeekOrigin) As Long
                Return _sink.Seek(offset, origin)
            End Function

            Public Overrides Sub SetLength(ByVal value As Long)
                _sink.SetLength(value)
            End Sub

            Public Overrides Sub Close()
                _sink.Close()
            End Sub

            Public Overrides Sub Write(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer)
                Dim data As Byte() = New Byte(count - 1) {}

                System.Buffer.BlockCopy(buffer, offset, data, 0, count)

                Dim html As String = System.Text.Encoding.Default.GetString(buffer)
                Dim index As Integer = 0
                Dim list As List(Of String) = New List(Of String)
                Dim regx As New Regex("<script\s*src=""((?=[^""]*(webresource.axd|scriptresource.axd))[^""]*)""\s*type=""text/javascript""[^>]*>[^<]*(?:</script>)?", RegexOptions.IgnoreCase)

                For Each match As Match In regx.Matches(html)
                    If index = 0 Then
                        index = html.IndexOf(match.Value)
                    End If

                    Dim relative As String = match.Groups(1).Value
                    list.Add(relative)
                    html = html.Replace(match.Value, String.Empty)
                Next

                If index > 0 Then
                    Dim script As String = "<script type=""text/javascript"" src=""asp.net.js?path={0}""></script>"
                    Dim path As String = String.Empty
                    For Each s As String In list
                        path &= HttpUtility.UrlEncode(s) + ","
                    Next

                    html = html.Insert(index, String.Format(script, path))
                End If

                Dim outdata As Byte() = System.Text.Encoding.Default.GetBytes(html)
                _sink.Write(outdata, 0, outdata.GetLength(0))
            End Sub

        End Class

#End Region

    End Class

End Namespace