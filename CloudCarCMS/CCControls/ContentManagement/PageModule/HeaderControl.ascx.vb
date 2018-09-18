Imports CloudCar.CCFramework.ContentManagement

Namespace CCControls.ContentManagement.PageModule

    Public Class HeaderControl
        Inherits UserControl

        Public Property PageId() As Integer
            Get
                Return CInt(ViewState("PageId"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("PageId") = Value

                LoadHeaderContent()
            End Set
        End Property

        Private Sub LoadHeaderContent()
            If ContentPageController.GetPageShowHeading(PageId) Then
                ContentTitleLiteral.Text = ContentPageController.GetPageContentTitle(PageId)
            Else
                Visible = False
            End If
        End Sub

    End Class

End Namespace