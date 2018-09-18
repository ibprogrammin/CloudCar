Namespace CCFramework.Core

    Public Class TextFunctions

        Public Shared Function StripShortString(ByVal text As String, ByVal length As Integer) As String
            Dim newContent As String = RemoveHTML(HttpUtility.HtmlDecode(text))

            If newContent.Length > length Then
                newContent = newContent.Substring(0, length) & "..."
            Else
                newContent = newContent
            End If

            Return newContent
        End Function

        Public Shared Function RemoveHTML(ByVal html As String) As String
            Dim parsedString As String = Regex.Replace(html, "<(.|\n)*?>", String.Empty)

            Return parsedString
        End Function

        Public Shared Function StripTextForURL(ByVal text As String) As String
            Dim returnString As String

            returnString = text.Trim(" "c).Replace("-", "").Replace(" ", "-").Replace("&", "").Replace("+", "").Replace("/", "-And-").Replace("""", "-Inch-").Replace(".", "").Replace("'", "").Replace(",", "").Replace("?", "").Replace("(", "").Replace(")", "")

            If returnString.EndsWith("-Hot-Tubs") Then
                returnString = returnString.Replace("-Hot-Tubs", "")
            End If

            Return returnString
        End Function

        Public Shared Function SwitchURLToText(ByVal url As String) As String
            Dim returnString As String

            returnString = url.Trim(" "c).Replace("-And-", "/").Replace("-", " ").Replace("+", "&")

            If returnString.EndsWith("-Hot-Tubs") Then
                returnString = returnString.Replace("-Hot-Tubs", "")
            End If

            Return returnString
        End Function

    End Class

End Namespace