Imports CloudCar.CCFramework.ContentManagement.CallToActionModule
Imports CloudCar.CCFramework.ContentManagement
Imports CloudCar.CCFramework.Model

Namespace CCControls.ContentManagement.PageModule

    Public Class CallToActionControl
        Inherits UserControl

        Public Property PageId() As Integer
            Get
                Return CInt(ViewState("PageId"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("PageId") = Value

                LoadCallToAction()
            End Set
        End Property

        Private Sub LoadCallToAction()
            Dim CurrentCallToActionId As Integer? = ContentPageController.GetPageCallToActioinId(PageId)

            If CurrentCallToActionId.HasValue AndAlso CurrentCallToActionId > 0 Then
                Dim CurrentCallToAction As CallToAction = CallToActionController.GetItem(CurrentCallToActionId.Value)

                HeadingLiteral.Text = CurrentCallToAction.Heading
                DetailsLiteral.Text = CurrentCallToAction.Details
                ButtonTextLiteral.Text = CurrentCallToAction.ButtonText

                LinkUrlAnchor.HRef = CurrentCallToAction.LinkUrl

                Me.Visible = True
            Else
                Me.Visible = False
            End If
        End Sub

    End Class
End Namespace