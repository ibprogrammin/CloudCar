Imports System.Drawing.Imaging
Imports System.IO
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports obout_ASPTreeView_2_NET
Imports System.Drawing

Partial Public Class EditorImageBrowse
    Inherits Page

    Public emptySrc As String = ""

    Public Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim imageFolder As String = "~/CCTemplate/Default/Images/"

        LoadDatabaseImages()

        Dim maxWidth As Integer = 100
        Dim maxHeight As Integer = 100
        Dim separator As Char() = New [Char]() {"/"c}

        Dim protocol As String = Page.Request.ServerVariables("SERVER_PROTOCOL").Split(separator)(0).ToLower()
        Dim port As String = ":" & Page.Request.ServerVariables("SERVER_PORT")


        If Page.Request.ServerVariables("HTTPS") = "on" Then
            protocol = protocol & "s"
        End If

        If port = ":80" Or port = ":443" Then
            port = ""
        End If

        Dim curPath As String = protocol + "://" + Page.Request.ServerVariables("SERVER_NAME") + port
        Dim folder As String = HttpContext.Current.Server.MapPath(ResolveUrl(imageFolder))

        If Not Page.Request("imgtitle") Is Nothing Then
            If Page.Request("imgtitle").Length > 0 Then
                Dim imageFile As String = HttpContext.Current.Server.MapPath(Page.Request("imgtitle").Replace("%20", " "))
                Dim title As String = ""

                If File.Exists(imageFile + ".description") Then
                    Dim sr As New StreamReader(imageFile + ".description")
                    title = sr.ReadLine()
                    sr.Close()
                End If

                Page.Response.Write("<html><body style='font-size:10px;margin:0px;padding:0px;padding-left:4px;'>")
                Page.Response.Write(title)
                Page.Response.Write("</body></html>")
                Page.Response.End()

                Exit Sub
            End If
        End If

        If Not Page.Request("imgprop") Is Nothing Then
            If Page.Request("imgprop").Length > 0 Then
                Dim imageFile As String = HttpContext.Current.Server.MapPath(Page.Request("imgprop").Replace("%20", " "))

                If File.Exists(imageFile) Then
                    Dim binStream As FileStream = File.OpenRead(imageFile)
                    Dim objImage As System.Drawing.Image = System.Drawing.Image.FromStream(binStream)
                    Page.Response.Write("<html><body style='font-size:10px;margin:0px;padding:0px;padding-left:4px;'>")
                    Page.Response.Write("<center><span style='color:blue;'>" + Path.GetExtension(imageFile).ToLower().Replace(".", "") + "</span>")
                    Page.Response.Write(" w: <b>" + objImage.Width.ToString() + "</b> h: <b>" + objImage.Height.ToString() + "</b>")
                    Page.Response.Write("</center></body></html>")
                    objImage.Dispose()
                    binStream.Close()
                    Page.Response.End()
                End If

                Exit Sub
            End If
        End If

        If Not Page.Request("imgsrc") Is Nothing Then
            If Page.Request("imgsrc").Length > 0 Then
                If Boolean.Parse(Page.Request("db")) Then
                    Dim ImageID As Integer = Integer.Parse(Page.Request("imgsrc").Split("/")(3))
                    Dim image As Picture = CCFramework.Core.PictureController.GetPicture(ImageID)

                    Dim View As System.Drawing.Image
                    Dim objImage As System.Drawing.Image
                    Dim objStream As MemoryStream
                    Dim objStreamN As MemoryStream

                    Dim myCallback As New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)

                    objStream = New MemoryStream(image.PictureData.ToArray, 0, image.PictureContentLength)
                    objStream.Position = 0

                    Try
                        objImage = System.Drawing.Image.FromStream(objStream)

                        Dim mWidth As Integer = objImage.Width
                        Dim mHeight As Integer = objImage.Height

                        If mWidth > maxWidth Then
                            mHeight = CInt(CDbl(mHeight) * (CDbl(maxWidth) / CDbl(mWidth)))
                            mWidth = maxWidth
                        End If
                        If mHeight > maxHeight Then
                            mWidth = CInt(CDbl(mWidth) * (CDbl(maxHeight) / CDbl(mHeight)))
                            mHeight = maxHeight
                        End If

                        If mHeight = 0 Then mHeight = 5
                        If mWidth = 0 Then mWidth = 5

                        View = objImage.GetThumbnailImage(mWidth, mHeight, myCallback, IntPtr.Zero)
                        objStream.Close()
                        objStreamN = New MemoryStream()
                        objStreamN.Position = 0

                        View.Save(objStreamN, objImage.RawFormat)
                        View.Dispose()

                        objImage.Dispose()
                        objStreamN.Position = 0

                        Dim buf As Byte()
                        ReDim buf(CInt(objStreamN.Length))

                        objStreamN.Read(buf, 0, CInt(objStreamN.Length))
                        objStreamN.Close()

                        Page.Response.ContentType = "image/gif"
                        Page.Response.BinaryWrite(buf)
                        Page.Response.End()
                    Catch ex As Exception
                    End Try
                Else
                    Dim imageFile As String = System.Web.HttpContext.Current.Server.MapPath(Page.Request("imgsrc").Replace("%20", " "))

                    If File.Exists(imageFile) Then
                        Dim objStream As MemoryStream
                        Dim objStreamN As MemoryStream
                        Dim objImage As System.Drawing.Image
                        Dim View As System.Drawing.Image

                        Dim binStream As FileStream = File.OpenRead(imageFile)
                        Dim buf As Byte()
                        ReDim buf(CInt(binStream.Length))

                        Dim myCallback As New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)

                        binStream.Read(buf, 0, CInt(binStream.Length))
                        binStream.Close()

                        objStream = New MemoryStream(buf)
                        objStream.Position = 0
                        objImage = System.Drawing.Image.FromStream(objStream)

                        Dim mWidth As Integer = objImage.Width
                        Dim mHeight As Integer = objImage.Height

                        If mWidth > maxWidth Then
                            mHeight = CInt(CDbl(mHeight) * (CDbl(maxWidth) / CDbl(mWidth)))
                            mWidth = maxWidth
                        End If
                        If mHeight > maxHeight Then
                            mWidth = CInt(CDbl(mWidth) * (CDbl(maxHeight) / CDbl(mHeight)))
                            mHeight = maxHeight
                        End If

                        If mHeight = 0 Then mHeight = 5
                        If mWidth = 0 Then mWidth = 5

                        View = objImage.GetThumbnailImage(mWidth, mHeight, myCallback, IntPtr.Zero)
                        objStream.Close()
                        objStreamN = New MemoryStream()
                        objStreamN.Position = 0

                        View.Save(objStreamN, objImage.RawFormat)
                        View.Dispose()

                        objImage.Dispose()
                        objStreamN.Position = 0

                        ReDim buf(CInt(objStreamN.Length))

                        objStreamN.Read(buf, 0, CInt(objStreamN.Length))
                        objStreamN.Close()

                        Page.Response.ContentType = "image/gif"
                        Page.Response.BinaryWrite(buf)
                        Page.Response.End()
                    End If

                    Exit Sub
                End If
            End If
        End If

        Dim oTree As New obout_ASPTreeView_2_NET.Tree()

        oTree.AddRootNode(Page.Request.ServerVariables("SERVER_NAME") & port, "ball_redS.gif")

        oTree.FolderIcons = "tree2/icons"
        oTree.FolderScript = "tree2/script"
        oTree.FolderStyle = "tree2/style/Classic"

        oTree.SelectedEnable = False

        If Directory.Exists(folder) Then
            oTree.Add("root", "myRoot", "Images Root", True, Nothing, Nothing)

            Dim currentPath As String = ResolveUrl(Path.GetDirectoryName(Page.Request.ServerVariables("SCRIPT_NAME"))).Replace("\\", "/") & "/"
            Dim imgPath As String = ResolveUrl(imageFolder)
            Dim differ As String = imgPath.Replace(currentPath, "")

            directoryDive(oTree, "myRoot", folder, curPath & ResolveUrl(imageFolder) & "/", differ & "/")

        End If

        Dim oTree2 As New Tree()

        oTree2.AddRootNode("Database", "ball_redS.gif")

        oTree2.FolderIcons = "tree2/icons"
        oTree2.FolderScript = "tree2/script"
        oTree2.FolderStyle = "tree2/style/Classic"

        oTree2.SelectedEnable = False

        databaseDive(oTree2, folder & "/db", curPath & "/images/db", "/images/db")

        TreeView.Text = oTree.HTML() & oTree2.HTML()
    End Sub

    Function ThumbnailCallback() As Boolean
        Return False
    End Function

    Public Sub directoryDive(ByVal oTree As Tree, ByVal parentID As String, ByVal folder As String, ByVal folderPath As String, ByVal relPath As String)
        Dim currentID As String
        If parentID = "myRoot" Then
            currentID = "a_"
        Else
            currentID = parentID & "_"
        End If

        Dim entires As String() = Directory.GetFileSystemEntries(folder)
        Dim i As Integer = 0

        For Each entire As String In entires
            Dim attr As FileAttributes = File.GetAttributes(entire)
            Dim name As String = Path.GetFileName(entire)
            Dim pName As String = Path.GetFileNameWithoutExtension(entire)
            Dim title As String = ""

            If (attr And FileAttributes.Directory) = FileAttributes.Directory Then
                oTree.Add(parentID, currentID & i.ToString(), pName, False, Nothing, Nothing)
                directoryDive(oTree, currentID & i.ToString(), folder & "/" & name, folderPath & name & "/", relPath & name & "/")
            Else
                Dim ext As String = Path.GetExtension(entire).ToLower()
                If ext = ".gif" Or ext = ".jpg" Or ext = ".jpeg" Or ext = ".png" Then
                    Dim objImage As System.Drawing.Image
                    Dim objStream As MemoryStream

                    Dim binStream As FileStream = File.OpenRead(entire)
                    Dim buf As Byte()
                    ReDim buf(binStream.Length)

                    Dim myCallback As New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)

                    binStream.Read(buf, 0, binStream.Length)
                    binStream.Close()

                    objStream = New MemoryStream(buf)
                    objStream.Position = 0
                    Try
                        objImage = System.Drawing.Image.FromStream(objStream)

                        Dim mWidth As Integer = objImage.Width
                        Dim mHeight As Integer = objImage.Height

                        objImage.Dispose()

                        If File.Exists(entire & ".description") Then
                            Dim sr As New StreamReader(entire & ".description")
                            title = sr.ReadLine()
                            sr.Close()
                        End If
                        Dim html As String = "<span myAttrSrc='" & folderPath & name & "' myAttrRel='" & (relPath & name).Replace(" ", "%20") & "' myAttrDb='false' onmouseover=""this.style.cursor='pointer'"" onclick=""myOnClick(event,'" & folderPath & name & "','" & (relPath & name).Replace(" ", "%20") & "'," & mWidth & "," & mHeight & ",'" & title & "',false)"" ondblclick=""myOnClick(event,'" & folderPath & name & "','" & (relPath & name).Replace(" ", "%20") & "'," & mWidth & "," & mHeight & ",'" & title & "',true)"">" & pName & "</span>"
                        oTree.Add(parentID, currentID + i.ToString(), html, False, "square_yellowS.gif", Nothing)
                    Catch ex As Exception
                    End Try
                    objStream.Close()
                End If
            End If
            i = i + 1
        Next entire
    End Sub

    Public Sub databaseDive(ByVal oTree As Tree, ByVal folder As String, ByVal folderPath As String, ByVal relPath As String)
        oTree.Add("root", "Images", "Images", False, Nothing, Nothing)
        'oTree.Add("myRoot_images_db", "myRoot_images_db_full", "full", False, Nothing, Nothing)

        Dim parentID As String = "Images"
        Dim currentID As String = "Images_"

        Dim images As List(Of Picture) = PictureController.GetPictures()

        Dim c As Integer = 0

        For Each item As Picture In images
            Dim name As String = item.PictureFileName
            Dim title As String = ""

            Dim objImage As System.Drawing.Image
            Dim objStream As MemoryStream

            Dim myCallback As New System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)

            objStream = New MemoryStream(item.PictureData.ToArray, 0, item.PictureContentLength)
            objStream.Position = 0

            Try
                objImage = System.Drawing.Image.FromStream(objStream)

                Dim mWidth As Integer = objImage.Width
                Dim mHeight As Integer = objImage.Height

                objImage.Dispose()

                Dim html As String = "<span myAttrSrc='" & folderPath & "/" & item.PictureID & "/full/" & name & "' myAttrRel='" & (relPath & "/" & item.PictureID & "/full/" & name).Replace(" ", "%20") & "' myAttrDb='true' onmouseover=""this.style.cursor='pointer'"" onclick=""myOnClick(event,'" & folderPath & "/" & item.PictureID & "/full/" & name & "','" & (relPath & "/" & item.PictureID & "/full/" & name).Replace(" ", "%20") & "'," & mWidth & "," & mHeight & ",'" & title & "',false)"" ondblclick=""myOnClick(event,'" & folderPath & "/" & item.PictureID & "/full/" & name & "','" & (relPath & "/" & item.PictureID & "/full/" & name).Replace(" ", "%20") & "'," & mWidth & "," & mHeight & ",'" & title & "',true)"">" & name & "</span>"

                oTree.Add(parentID, currentID + c.ToString(), html, False, "square_yellowS.gif", Nothing)

            Catch ex As Exception
            End Try

            objStream.Close()

            c += 1
        Next
    End Sub

    Private Sub LoadDatabaseImages()
        Dim images As List(Of Picture) = PictureController.GetPictures()

        Dim separator As Char() = New [Char]() {"/"c}
        Dim protocol As String = Page.Request.ServerVariables("SERVER_PROTOCOL").Split(separator)(0).ToLower()
        Dim port As String = ":" & Page.Request.ServerVariables("SERVER_PORT")
        
        If Page.Request.ServerVariables("HTTPS") = "on" Then
            protocol = protocol & "s"
        End If

        If port = ":80" Or port = ":443" Then
            port = ""
        End If

        'HttpContext.Current.Server.MapPath(ResolveUrl("~/CCTemplate/Default/Images/"))
        Dim folderPath As String = "/images/db"
        Dim relPath As String = protocol & "://" + Page.Request.ServerVariables("SERVER_NAME") & port & "/images/db"

        Dim c As Integer = 0

        For Each CurrentPicture As Picture In images
            Dim FileName As String = CurrentPicture.PictureFileName
            Dim PictureTitle As String = ""
            
            Dim objImage As Image
            Dim objStream As MemoryStream

            Dim myCallback As New Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)

            objStream = New MemoryStream(CurrentPicture.PictureData.ToArray, 0, CurrentPicture.PictureContentLength)
            objStream.Position = 0

            Try
                objImage = Image.FromStream(objStream)

                Dim mWidth As Integer = objImage.Width
                Dim mHeight As Integer = objImage.Height

                objImage.Dispose()

                Dim html As String = "<span myAttrSrc='" & folderPath & "/" & CurrentPicture.PictureID & "/full/" & FileName & "' myAttrRel='" & (relPath & "/" & CurrentPicture.PictureID & "/full/" & FileName).Replace(" ", "%20") & "' myAttrDb='true' onmouseover=""this.style.cursor='pointer'"" onclick=""myOnClick(event,'" & folderPath & "/" & CurrentPicture.PictureID & "/full/" & FileName & "','" & (relPath & "/" & CurrentPicture.PictureID & "/full/" & FileName).Replace(" ", "%20") & "'," & mWidth & "," & mHeight & ",'" & Title & "',false)"" ondblclick=""myOnClick(event,'" & folderPath & "/" & CurrentPicture.PictureID & "/full/" & FileName & "','" & (relPath & "/" & CurrentPicture.PictureID & "/full/" & FileName).Replace(" ", "%20") & "'," & mWidth & "," & mHeight & ",'" & Title & "',true)"">" & FileName & "</span>"

                Dim ReturnFunctionNumber As String = Request.QueryString("CKEditorFuncNum")
                Dim CKEditorObject As String = Request.QueryString("CKEditor")
                Dim LanguageCode As String = Request.QueryString("langCode")
                Dim ImageUrl As String = String.Format("/images/db/{0}/full/{1}", CurrentPicture.PictureID, CurrentPicture.PictureFileName)
                
                Dim ImageScript As String = String.Format("<input type=""button"" onclick=""javascript:window.opener.CKEDITOR.tools.callFunction({0},'{1}','{2}');"" value=""{3}"" /><br />", ReturnFunctionNumber, ImageUrl, Nothing, FileName, CKEditorObject)

                DatabaseImageTreeView.Text &= ImageScript
            Catch ex As Exception

            End Try

            objStream.Close()

            c += 1
        Next
    End Sub

End Class