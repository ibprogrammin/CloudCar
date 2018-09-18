Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCControls.ContentManagement

Namespace CCContentManagement

    Partial Public Class Gallery
        Inherits CloudCarContentPage

        Public Sub New()
            MyBase.New()
            Permalink = Settings.GalleryPage
        End Sub

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadGalleries()
            End If
        End Sub

        Private Sub LoadGalleries()
            Dim LightBoxScript As New ScriptLocation(String.Format("/CCTemplates/{0}/Scripts/jQueryLightBox/jquery.lightbox-0.5.js", Settings.Theme), 7)
            PageScriptManagementControl.AddScriptLocation(LightBoxScript)

            Dim CurrentDataContext As New ContentDataContext

            'TODO Make a compiled query
            Dim CurrentGalleries As List(Of ImageGallery) = (From ig In CurrentDataContext.ImageGalleries Where (From igi In CurrentDataContext.ImageGalleryItems Where igi.GalleryID = ig.ID).Count > 0 Select ig).ToList

            GalleriesRepeater.DataSource = CurrentGalleries
            GalleriesRepeater.DataBind()

            Dim CurrentGalleryScript As New StringBuilder

            CurrentGalleryScript.Append("<script type=""text/javascript"">" & vbNewLine)
            'CurrentGalleryScript.Append("jQuery.noConflict();" & vbNewLine)
            CurrentGalleryScript.Append("$(function() {" & vbNewLine)

            For Each item In CurrentGalleries
                CurrentGalleryScript.Append("$('#Gallery" & item.Title.Replace(" ", "") & " a').lightBox();" & vbNewLine)
            Next

            CurrentGalleryScript.Append("});" & vbNewLine)
            CurrentGalleryScript.Append("</script>" & vbNewLine)

            GalleryScriptLiteral.Text = CurrentGalleryScript.ToString

        End Sub

    End Class

End NameSpace