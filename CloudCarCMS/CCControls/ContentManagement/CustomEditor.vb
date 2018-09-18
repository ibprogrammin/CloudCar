Imports System
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.ComponentModel.Design.Serialization
Imports System.Reflection
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Drawing.Design
Imports System.Security.Permissions
Imports System.Collections
Imports System.Collections.ObjectModel
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Xml.Schema
Imports System.Globalization
Imports System.CodeDom
Imports System.Drawing
Imports System.IO
Imports AjaxControlToolkit
Imports AjaxControlToolkit.HTMLEditor
Imports AjaxControlToolkit.HTMLEditor.ToolbarButton

Namespace ContentControls

    Public Class CustomEditor
        Inherits Editor

        Dim imgButton As ToolbarButton.MethodButton = New ToolbarButton.MethodButton()

        Protected Overrides Sub FillTopToolbar()
            MyBase.FillTopToolbar()

            'TopToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.CustomToolbarButton.InsertEmoticon())
            'TopToolbar.Buttons.Add(New AjaxControlToolkit.HTMLEditor.CustomToolbarButton.InsertDatabaseImage())

            Dim btn As ToolbarButton.MethodButton = New ToolbarButton.MethodButton()

            btn.NormalSrc = "/images/buttons/ed_image_n.gif"
            btn.HoverSrc = "/images/buttons/ed_image_a.gif"
            btn.DownSrc = "/images/buttons/ed_image_n.gif"
            btn.ID = "btnUplaodImg"
            btn.Attributes.Add("onclick", "show();")

            TopToolbar.Buttons.Add(btn)
        End Sub

        Protected Overrides Sub FillBottomToolbar()
            MyBase.FillBottomToolbar()
        End Sub

    End Class

End Namespace


Namespace AjaxControlToolkit.HTMLEditor.CustomToolbarButton

    <ParseChildren(True)> _
    <PersistChildren(False)> _
    <RequiredScript(GetType(OkCancelPopupButton))> _
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance")> _
    Public Class InsertDatabaseImage
        Inherits DesignModePopupImageButton

#Region "Properties"

        Public Property IconsInRow() As Integer
            Get
                If IsNothing(ViewState("IconsInRow")) Then
                    Return 4
                Else
                    Return CType(ViewState("IconsInRow"), Integer)
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("IconsInRow") = value
            End Set
        End Property

        Public Property IconsFolder() As String
            Get
                'If IsNothing(ViewState("IconsFolder")) Then
                Return "/images/icons/"
                'Else
                'Return CType(ViewState("IconsFolder"), String)
                'End If
            End Get
            Set(ByVal value As String)
                ViewState("IconsFolder") = value
            End Set
        End Property

        Public Overrides Property ScriptPath() As String
            Get
                Return "/scripts/InsertDatabaseImage.js"
            End Get
            Set(ByVal value As String)
                MyBase.ScriptPath = value
            End Set
        End Property

        Public Overrides Property ToolTip() As String
            Get
                Return "Insert database images"
            End Get
            Set(ByVal value As String)
                MyBase.ToolTip = value
            End Set
        End Property

#End Region

#Region "Methods"

        Protected Overrides Sub OnInit(ByVal e As EventArgs)
            Dim RelatedPopup As New CustomPopups.InsertDBImagePopup()

            RelatedPopup.IconsInRow = IconsInRow
            RelatedPopup.IconsFolder = IconsFolder

            MyBase.RelatedPopup = RelatedPopup

            MyBase.OnInit(e)
        End Sub

        Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
            MyBase.RegisterButtonImages("ed_insertIcon")

            MyBase.NormalSrc = "/images/buttons/ed_image_n.gif"
            MyBase.HoverSrc = "/images/buttons/ed_image_a.gif"
            MyBase.DownSrc = "/images/buttons/ed_image_n.gif"

            MyBase.OnPreRender(e)
        End Sub

        Protected Overrides ReadOnly Property ClientControlType() As String
            Get
                Return "AjaxControlToolkit.HTMLEditor.CustomToolbarButton.InsertDatabaseImage"
            End Get
        End Property

#End Region

    End Class

    <ParseChildren(True)> _
    <PersistChildren(False)> _
    <RequiredScript(GetType(OkCancelPopupButton))> _
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance")> _
    Public Class InsertEmoticon
        Inherits DesignModePopupImageButton

#Region "Properties"

        Public Property IconsInRow() As Integer
            Get
                If IsNothing(ViewState("IconsInRow")) Then
                    Return 10
                Else
                    Return CType(ViewState("IconsInRow"), Integer)
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("IconsInRow") = value
            End Set
        End Property

        Public Property IconsFolder() As String
            Get
                'If IsNothing(ViewState("IconsFolder")) Then
                Return "/images/Icons/"
                'Else
                'Return CType(ViewState("IconsFolder"), String)
                'End If
            End Get
            Set(ByVal value As String)
                ViewState("IconsFolder") = value
            End Set
        End Property

        Public Overrides Property ScriptPath() As String
            Get
                Return "../Scripts/InsertDatabaseImage.js"
            End Get
            Set(ByVal value As String)
                MyBase.ScriptPath = value
            End Set
        End Property

        Public Overrides Property ToolTip() As String
            Get
                Return "Insert emoticon"
            End Get
            Set(ByVal value As String)
                MyBase.ToolTip = value
            End Set
        End Property

#End Region

#Region "Methods"

        Protected Overrides Sub OnInit(ByVal e As EventArgs)
            Dim RelatedPopup As New CustomPopups.InsertEmoticonPopup()

            RelatedPopup.IconsInRow = IconsInRow
            RelatedPopup.IconsFolder = IconsFolder

            MyBase.RelatedPopup = RelatedPopup

            MyBase.OnInit(e)
        End Sub

        Protected Overrides Sub OnPreRender(ByVal e As EventArgs)
            MyBase.RegisterButtonImages("ed_insertIcon")

            MyBase.NormalSrc = "../Images/Buttons/ed_insertIcon_n.gif"
            MyBase.HoverSrc = "../Images/Buttons/ed_insertIcon_a.gif"
            MyBase.DownSrc = "../Images/Buttons/ed_insertIcon_n.gif"

            MyBase.OnPreRender(e)
        End Sub

        Protected Overrides ReadOnly Property ClientControlType() As String
            Get
                Return "AjaxControlToolkit.HTMLEditor.CustomToolbarButton.InsertDatabaseImage"
            End Get
        End Property

#End Region

    End Class

End Namespace

Namespace AjaxControlToolkit.HTMLEditor.CustomPopups

    <ParseChildren(True)> _
    <RequiredScript(GetType(Popups.AttachedTemplatePopup))> _
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance")> _
    Friend Class InsertDBImagePopup
        Inherits Popups.AttachedPopup

#Region "Properties"

        Public Property IconsInRow() As Integer
            Get
                If IsNothing(ViewState("IconsInRow")) Then
                    Return 4
                Else
                    Return CType(ViewState("IconsInRow"), Integer)
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("IconsInRow") = value
            End Set
        End Property

        Public Property IconsFolder() As String
            Get
                'If IsNothing(ViewState("IconsFolder")) Then
                Return "/Handlers/GetPicture.ashx?PictureID="
                'Else
                'Return CType(ViewState("IconsFolder"), String)
                'End If
            End Get
            Set(ByVal value As String)
                ViewState("IconsFolder") = value
            End Set
        End Property

#End Region

#Region "Methods"

        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")> _
        Protected Function LocalResolveUrl(ByVal path As String) As String
            Dim temp As String = MyBase.ResolveUrl(path)
            Dim _Regex As Regex = New Regex("(\(S\([A-Za-z0-9_]+\)\)/)", RegexOptions.Compiled)
            temp = _Regex.Replace(temp, "")
            Return temp
        End Function

        Protected Overrides Sub CreateChildControls()
            Dim table As Table = New Table()
            Dim row As TableRow = Nothing
            Dim cell As TableCell

            Dim j As Integer = 0

            Dim images = CCFramework.Core.PictureController.GetPictureIDs()

            For Each img As Integer In images
                If j = 0 Then
                    row = New TableRow()
                    table.Rows.Add(row)
                End If

                cell = New TableCell()
                cell.VerticalAlign = VerticalAlign.Middle
                cell.HorizontalAlign = HorizontalAlign.Center

                Dim image As System.Web.UI.WebControls.Image = New System.Web.UI.WebControls.Image()

                image.ImageUrl = IconsFolder & img.ToString & "&size=60"
                image.Attributes.Add("onmousedown", "insertImage(""" & IconsFolder & img.ToString & "' class='DBImage"")")
                image.Style(HtmlTextWriterStyle.Cursor) = "pointer"
                image.Style("Border") = "2px solid #DDDDDD"
                image.Style("Padding") = "2px"
                image.Style("background-color") = "White"

                cell.Controls.Add(image)

                row.Cells.Add(cell)

                j += 1

                If j = IconsInRow Then j = 0
            Next


            Dim table2 As New Table()
            row = New TableRow()
            table2.Rows.Add(row)

            Dim addImageLink As New HyperLink()

            addImageLink.NavigateUrl = "~/CCAdmin/ContentManagement/Images.aspx"
            addImageLink.Text = "Add an image"
            addImageLink.CssClass = "BottomLinks"
            addImageLink.Target = "_parent"

            cell = New TableCell()
            cell.ColumnSpan = IconsInRow
            cell.HorizontalAlign = HorizontalAlign.Right
            cell.Controls.Add(addImageLink)

            row.Cells.Add(cell)

            table.Attributes.Add("border", "0")
            table.Attributes.Add("cellspacing", "4")
            table.Attributes.Add("cellpadding", "0")
            table.Style("background-color") = "#EEEEEE"

            table2.Width = Unit.Percentage(99)

            Dim label As New Label()

            label.Style("height") = "260px"
            label.Style("overflow") = "auto"
            label.Style("display") = "block"

            label.Controls.Add(table)

            MyBase.Controls.Add(label)
            MyBase.Controls.Add(table2)
            MyBase.CssPath = "~/styles/main.styles.css"
            MyBase.CreateChildControls()
        End Sub

        Protected Overrides ReadOnly Property ClientControlType() As String
            Get
                Return "AjaxControlToolkit.HTMLEditor.CustomPopups.InsertDBImagePopup"
            End Get
        End Property

        Public Overrides Property ScriptPath() As String
            Get
                Return "/scripts/InsertDBImagePopup.js"
            End Get
            Set(ByVal value As String)
                MyBase.ScriptPath = value
            End Set
        End Property

#End Region

    End Class

    <ParseChildren(True)> _
    <RequiredScript(GetType(Popups.AttachedTemplatePopup))> _
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1501:AvoidExcessiveInheritance")> _
    Friend Class InsertEmoticonPopup
        Inherits Popups.AttachedTemplatePopup

#Region "Properties"

        Public Property IconsInRow() As Integer
            Get
                If IsNothing(ViewState("IconsInRow")) Then
                    Return 10
                Else
                    Return CType(ViewState("IconsInRow"), Integer)
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("IconsInRow") = value
            End Set
        End Property

        Public Property IconsFolder() As String
            Get
                'If IsNothing(ViewState("IconsFolder")) Then
                Return "/images/icons/"
                'Else
                'Return CType(ViewState("IconsFolder"), String)
                'End If
            End Get
            Set(ByVal value As String)
                ViewState("IconsFolder") = value
            End Set
        End Property

#End Region

#Region "Methods"

        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")> _
        Protected Function LocalResolveUrl(ByVal path As String) As String
            Dim temp As String = MyBase.ResolveUrl(path)
            Dim _Regex As Regex = New Regex("(\(S\([A-Za-z0-9_]+\)\)/)", RegexOptions.Compiled)
            temp = _Regex.Replace(temp, "")
            Return temp
        End Function

        Protected Overrides Sub CreateChildControls()
            MyBase.CssClass = "PopOutControl"

            Dim table As Table = New Table()
            Dim row As TableRow = Nothing
            Dim cell As TableCell

            Dim iconsFolder As String = LocalResolveUrl(Me.IconsFolder)
            If iconsFolder.Length > 0 Then
                Dim lastCh As String = iconsFolder.Substring(iconsFolder.Length - 1, 1)
                If Not lastCh = "\\" And Not lastCh = "/" Then iconsFolder &= "/"
            End If

            If Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(iconsFolder)) Then

                Dim files As String() = Directory.GetFiles(System.Web.HttpContext.Current.Server.MapPath(iconsFolder))
                Dim j As Integer = 0

                For Each file As String In files

                    Dim ext As String = Path.GetExtension(file).ToLower()
                    If ext = ".gif" Or ext = ".jpg" Or ext = ".jpeg" Or ext = ".png" Then
                        If j = 0 Then
                            row = New TableRow()
                            table.Rows.Add(row)
                        End If
                        cell = New TableCell()

                        Dim image As System.Web.UI.WebControls.Image = New System.Web.UI.WebControls.Image()
                        image.ImageUrl = iconsFolder & Path.GetFileName(file)
                        image.Attributes.Add("onmousedown", "insertImage('" & iconsFolder & Path.GetFileName(file) & "')")
                        image.Style(HtmlTextWriterStyle.Cursor) = "pointer"

                        cell.Controls.Add(image)

                        row.Cells.Add(cell)

                        j += 1
                        If j = IconsInRow Then j = 0
                    End If
                Next
            End If

            table.Attributes.Add("border", "0")
            table.Attributes.Add("cellspacing", "4")
            table.Attributes.Add("cellpadding", "0")
            table.Style("background-color") = "#EEEEEE"

            MyBase.Content.Add(table)

            MyBase.CreateChildControls()
        End Sub

        Protected Overrides ReadOnly Property ClientControlType() As String
            Get
                Return "AjaxControlToolkit.HTMLEditor.CustomPopups.InsertDBImagePopup"
            End Get
        End Property

        Public Overrides Property ScriptPath() As String
            Get
                Return "/scripts/InsertDBImagePopup.js"
            End Get
            Set(ByVal value As String)
                MyBase.ScriptPath = value
            End Set
        End Property

#End Region

    End Class

End Namespace



