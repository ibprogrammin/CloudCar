Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.ContentManagement

Namespace CCContentManagement.VideosModule

    Partial Public Class Videos
        Inherits CloudCarContentPage

        Public Sub New()
            MyBase.New()
            Permalink = Settings.VideosPage
        End Sub

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadVideos()
            End If
        End Sub

        Private Sub LoadVideos()
            Dim VideoList As List(Of Video) = VideoController.GetElements.ToList

            Dim Feature As Video = VideoList.FirstOrDefault

            If Not Feature Is Nothing Then
                litFeaturedVideo.Text = VideoController.GetPlayerHTML(CType(Feature.Player, SMVideoType), Feature.VideoID, 380, 305)
                litFeaturedTitle.Text = Feature.Title
                litFeaturedDetails.Text = Feature.Details
            End If

            rptVideos.DataSource = VideoList.Skip(1)
            rptVideos.DataBind()
        End Sub

    End Class

End Namespace