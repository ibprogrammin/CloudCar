Imports CloudCar.CCFramework.Model

Namespace CCControls.ContentManagement.GalleryControls

    Partial Public Class ImageGalleryControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                LoadGallery()
            End If
        End Sub

        Private Sub LoadGallery()
            Dim CurrentDataContext As New ContentDataContext

            Dim CurrentImageGalleryItems = _
                         From igi In CurrentDataContext.ImageGalleryItems _
                         Where igi.GalleryID = GalleryId _
                         Select igi Order By igi.Order Ascending

            rptGalleryItems.DataSource = CurrentImageGalleryItems
            rptGalleryItems.DataBind()
        End Sub

        Public Property GalleryId() As Integer
            Get
                If Not ViewState("GalleryID") Is Nothing Then
                    Return CInt(ViewState("GalleryID"))
                Else
                    Return -1
                End If
            End Get
            Set(ByVal value As Integer)
                If ViewState("GalleryID") Is Nothing Then
                    ViewState.Add("GalleryID", value)
                Else
                    ViewState("GalleryID") = value
                End If

                LoadGallery()
            End Set
        End Property

        Public Property Category() As String
            Get
                If Not ViewState("Category") Is Nothing Then
                    Return CStr(ViewState("Category"))
                Else
                    Return ""
                End If
            End Get
            Set(ByVal value As String)
                If ViewState("GallCategoryeryID") Is Nothing Then
                    ViewState.Add("Category", value)
                Else
                    ViewState("Category") = value
                End If
            End Set
        End Property

    End Class

End Namespace