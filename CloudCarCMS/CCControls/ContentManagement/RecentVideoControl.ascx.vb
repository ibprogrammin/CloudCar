Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement

Namespace CCControls.ContentManagement

    Partial Public Class RecentVideoControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadVideo()
            End If
        End Sub

        Private Sub LoadVideo()
            Dim VideoList As List(Of Video) = VideoController.GetElements.ToList

            If VideoList.Count > 0 Then
                Dim Feature As Video = VideoList.FirstOrDefault

                FeaturedVideoLiteral.Text = VideoController.GetPlayerHTML(CType(Feature.Player, SMVideoType), Feature.VideoID.Trim, 230, 156)
                VideoTitleLiteral.Text = Feature.Title
                VideoDescriptionLiteral.Text = Feature.Details

                VideoPlaceHolder.Visible = True
            End If
        End Sub

    End Class

End Namespace