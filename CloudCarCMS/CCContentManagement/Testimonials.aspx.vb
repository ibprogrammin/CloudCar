Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Core
Imports CloudCar.CCFramework.ContentManagement

Namespace CCContentManagement

    Partial Public Class Testimonials
        Inherits CloudCarContentPage

        Public Sub New()
            MyBase.New()
            Permalink = Settings.TestimonialPage
        End Sub

        Protected Overrides Sub Onload(ByVal Args As EventArgs)
            If Not Page.IsPostBack Then
                LoadVideos()
            End If
        End Sub

        Private Sub LoadVideos()
            Dim VideoList As List(Of Video) = VideoController.GetElements.ToList

            If Not VideoList Is Nothing AndAlso VideoList.Count > 0 Then
                rptVideos.DataSource = VideoList
                rptVideos.DataBind()
            End If
        End Sub

    End Class
End NameSpace