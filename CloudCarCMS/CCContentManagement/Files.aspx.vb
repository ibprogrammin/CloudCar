Namespace CCContentManagement
    Public Partial Class Files
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                rptFiles.DataSource = CCFramework.ContentManagement.FileUploadController.GetEnabledFileUploads()
                rptFiles.DataBind()
            End If
        End Sub

    End Class
End NameSpace