Imports System.Text
Imports System.IO
Imports System.Net

Namespace CCFramework.GAConnect

    Public Class GAHttpRequests

        Public Shared Function HttpPostRequest(Url As String, Post As String) As String
            Dim CurrentEncoding As New ASCIIEncoding
            Dim CurrentData As Byte() = CurrentEncoding.GetBytes(Post)

            Dim CurrentRequest As WebRequest = WebRequest.Create(Url)

            CurrentRequest.Method = "POST"
            CurrentRequest.ContentType = "application/x-www-form-urlencoded"
            CurrentRequest.ContentLength = CurrentData.Length

            Dim CurrentStream As Stream = CurrentRequest.GetRequestStream()
            CurrentStream.Write(CurrentData, 0, CurrentData.Length)
            CurrentStream.Close()

            Dim CurrentResponse As WebResponse = CurrentRequest.GetResponse()
            Dim CurrentResult As String

            Using CurrentReader As New StreamReader(CurrentResponse.GetResponseStream())
                CurrentResult = CurrentReader.ReadToEnd
                CurrentReader.Close()
            End Using

            CurrentResponse.Close()

            Return CurrentResult
        End Function
 
        Public Shared Function HttpGetRequest(Url As String, Headers As String()) As String
            Dim CurrentResult As String

            Dim CurrentRequest As WebRequest = WebRequest.Create(Url)
            If Headers.Length > 0 Then
                For Each CurrentHeader As String In Headers
                    CurrentRequest.Headers.Add(CurrentHeader)
                Next
            End If

            Dim CurrentResponse As WebResponse = CurrentRequest.GetResponse()
            Using CurrentReader As New StreamReader(CurrentResponse.GetResponseStream)
                CurrentResult = CurrentReader.ReadToEnd
                CurrentReader.Close()
            End Using

            Return CurrentResult
        End Function

    End Class

End Namespace