Imports CloudCar.CCFramework.ContentManagement

Namespace CCControls.ContentManagement.PageModule

    Public Class ChildPageMenuControl
        Inherits UserControl

        Public Property PageId() As Integer
            Get
                Return CInt(ViewState("PageId"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("PageId") = Value

                LoadChildMenu()
            End Set
        End Property

        Private Sub LoadChildMenu()
            Dim CurrentNestedPages = ContentPageController.GetNestedPages(PageId)

            If CurrentNestedPages.Count > 0 Then
                MenuPanel.Visible = True

                LinksRepeater.DataSource = CurrentNestedPages
                LinksRepeater.DataBind()
            End If
        End Sub

    End Class

End Namespace