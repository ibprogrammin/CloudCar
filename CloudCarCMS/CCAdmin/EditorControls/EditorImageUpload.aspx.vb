Public Partial Class EditorImageUpload
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim uploadingFolder As String = "~/Editor/images/users_images/"

        Page.Response.Write("<html>")
        Page.Response.Write("<head>")
        Page.Response.Write("<scr" & "ipt>")
        Page.Response.Write("var imageSaved = """ & Page.Request("notsaved") & """;")
        Page.Response.Write("var imageFileName = null;")
        Page.Response.Write("var imageFileTitle = null;")

        If Page.Request.Files.Count > 0 Then
            Dim PFile As System.Web.HttpPostedFile = Page.Request.Files(0)
            Dim ext As String = Path.GetExtension(PFile.FileName).ToLower()

            If ext = ".gif" Or ext = ".jpg" Or ext = ".jpeg" Or ext = ".png" Then
                If PFile.FileName.Trim().Length > 0 And PFile.ContentLength > 0 Then
                    Dim fName As String = Page.MapPath(ResolveUrl(uploadingFolder + Path.GetFileName(PFile.FileName)))

                    Try
                        If File.Exists(fName) Then File.Delete(fName)
                        PFile.SaveAs(fName)

                        Dim title As String = Page.Request("title")

                        If title.Length > 0 Then
                            fName = fName & ".description"
                            If File.Exists(fName) Then File.Delete(fName)
                            Dim sw As New System.IO.StreamWriter(fName)
                            sw.Write(title)
                            sw.Close()
                        End If
                        Page.Response.Write("imageSaved = """ & Page.Request("saved") & """;")
                        Page.Response.Write("imageFileName = """ & ResolveUrl(uploadingFolder) & Path.GetFileName(PFile.FileName) & """;")
                        Page.Response.Write("imageFileTitle = """ & title & """;")
                    Catch ev As Exception
                        Page.Response.Write("imageSaved = """ & ev.Message.Replace(Chr(10), " ").Replace(Chr(13), " ").Replace("\", "\\").Replace("""", "\""") + "\n\nTurn to your System Administrator."";")
                    End Try
                End If
            End If
        End If

        Page.Response.Write("</scr" & "ipt>")
        Page.Response.Write("</head>")
        Page.Response.Write("<body>")
        Page.Response.Write("</body>")
        Page.Response.Write("</html>")
        Page.Response.End()
    End Sub

End Class