Imports System.IO
Imports System.Net

Namespace CCFramework.Core

    Public Class FtpHelper

        Private Const _DefaultBufferSize As Integer = 2048

        Public Shared Function DownloadFile(DownloadFileUrl As String, LocalFileName As String, UserName As String, Password As String) As String
            Dim CurrentRequest As FtpWebRequest = CType(FtpWebRequest.Create(DownloadFileUrl), FtpWebRequest)

            CurrentRequest.Method = WebRequestMethods.Ftp.DownloadFile
            CurrentRequest.Credentials = New NetworkCredential(UserName, Password)
            CurrentRequest.UseBinary = True
            CurrentRequest.UsePassive = True
            CurrentRequest.KeepAlive = True

            Dim CurrentResponse As FtpWebResponse = CType(CurrentRequest.GetResponse(), FtpWebResponse)
            Dim CurrentResponseStream As Stream = CurrentResponse.GetResponseStream()
            Dim CurrentReader As StreamReader = New StreamReader(CurrentResponseStream)

            Dim CurrentLocalFileStream As FileStream = New FileStream(LocalFileName, FileMode.Create)
            Dim CurrentByteBuffer(_DefaultBufferSize) As Byte
            Dim CurrentBytesRead As Integer = CurrentResponseStream.Read(CurrentByteBuffer, 0, _DefaultBufferSize)

            Try
                While CurrentBytesRead > 0
                    CurrentLocalFileStream.Write(CurrentByteBuffer, 0, CurrentBytesRead)
                    CurrentBytesRead = CurrentResponseStream.Read(CurrentByteBuffer, 0, _DefaultBufferSize)
                End While
            Catch Ex As Exception
                Return Ex.ToString()
            End Try

            CurrentLocalFileStream.Close()
            CurrentReader.Close()
            CurrentResponse.Close()

            CurrentLocalFileStream.Dispose()
            CurrentReader.Dispose()

            Return "Success!"
        End Function

        Public Shared Function GetDirectoryListSimple(DirectoryUrl As String, UserName As String, Password As String) As String()
            Try
                Dim CurrentFtpRequest As FtpWebRequest = CType(FtpWebRequest.Create(DirectoryUrl), FtpWebRequest)
                CurrentFtpRequest.Credentials = New NetworkCredential(UserName, Password)
                CurrentFtpRequest.UseBinary = True
                CurrentFtpRequest.UsePassive = True
                CurrentFtpRequest.KeepAlive = True
                CurrentFtpRequest.Method = WebRequestMethods.Ftp.ListDirectory

                Dim CurrentFtpResponse As FtpWebResponse = CType(CurrentFtpRequest.GetResponse(), FtpWebResponse)
                Dim CurrentFtpResponseStream As Stream = CurrentFtpResponse.GetResponseStream()
                Dim CurrentFtpReader As StreamReader = New StreamReader(CurrentFtpResponseStream)

                Dim CurrentRawDirectory As String = Nothing

                Try
                    While Not CurrentFtpReader.Peek = -1
                        CurrentRawDirectory &= CurrentFtpReader.ReadLine & "|"
                    End While
                Catch Ex As Exception
                    Return New String() {""}
                End Try

                CurrentFtpReader.Close()
                CurrentFtpResponse.Close()

                CurrentFtpReader.Dispose()

                Return CurrentRawDirectory.Split("|".ToCharArray())
            Catch Ex As Exception
                Return New String() {""}
            End Try
        End Function

    End Class

End Namespace