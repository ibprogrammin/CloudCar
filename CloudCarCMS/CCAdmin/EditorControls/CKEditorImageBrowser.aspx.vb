Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports System.IO
Imports System.Drawing

Namespace CCAdmin.EditorControls

    Public Class CKEditorImageBrowser
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadDatabaseImages()
            End If
        End Sub

        Private Sub LoadDatabaseImages()
            Dim CurrentPictures As List(Of Picture) = PictureController.GetPictures()

            For Each CurrentPicture As Picture In CurrentPictures
                Dim FileName As String = CurrentPicture.PictureFileName

                Try

                    Dim ReturnFunctionNumber As String = Request.QueryString("CKEditorFuncNum")
                    Dim CKEditorObject As String = Request.QueryString("CKEditor")
                    Dim LanguageCode As String = Request.QueryString("langCode")
                    Dim ImageUrl As String = String.Format("/images/db/{0}/full/{1}", CurrentPicture.PictureID, CurrentPicture.PictureFileName)

                    Dim ImageScript As String = String.Format("<img src=""/images/db/{4}/100/{3}"" class=""browser-image"" onclick=""javascript:window.opener.CKEDITOR.tools.callFunction({0},'{1}','{2}');"" alt=""{3}"" />", ReturnFunctionNumber, ImageUrl, Nothing, FileName, CurrentPicture.PictureID)

                    DatabaseImageTreeView.Text &= ImageScript
                Catch Ex As Exception

                End Try
            Next
        End Sub

        Function ThumbnailCallback() As Boolean
            Return False
        End Function

    End Class
End Namespace