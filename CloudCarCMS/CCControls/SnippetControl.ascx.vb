Namespace CCControls

    Partial Public Class SnippetControl
        Inherits UserControl

        Private _itemID As Integer

        Public Property Title() As String
            Get
                Return lblTitle.Text
            End Get
            Set(ByVal value As String)
                lblTitle.Text = value
            End Set
        End Property

        Public Property Content() As String
            Get
                Return lblContent.Text
            End Get
            Set(ByVal value As String)
                Dim newContent As String = RemoveHTML(value)

                If newContent.Length > 320 Then
                    lblContent.Text = newContent.Substring(0, 320) & "..."
                Else
                    lblContent.Text = newContent
                End If
            End Set
        End Property

        Public Property ItemID() As Integer
            Get
                Return _itemID
            End Get
            Set(ByVal value As Integer)
                _itemID = value
                hlReadMore.NavigateUrl = "~/EventsAndNews.aspx#EventAndNewsID" & _itemID.ToString
            End Set
        End Property

        Public Function RemoveHTML(ByVal html As String) As String
            Dim parsedString As String = html

            parsedString = Regex.Replace(html, "<(.|\n)*?>", String.Empty)

            Return parsedString
        End Function

    End Class

End Namespace