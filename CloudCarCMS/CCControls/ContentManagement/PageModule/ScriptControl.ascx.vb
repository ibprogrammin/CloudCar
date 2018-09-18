Imports CloudCar.CCFramework.ContentManagement

Namespace CCControls.ContentManagement.PageModule

    Public Class ScriptControl
        Inherits UserControl

        Public Property PageId() As Integer
            Get
                Return CInt(ViewState("PageId"))
            End Get
            Set(ByVal Value As Integer)
                ViewState("PageId") = Value

                LoadScripts()
            End Set
        End Property

        Private Sub LoadScripts()
            ScriptsLiteral.Text = ContentPageController.GetPageScript(PageId)
        End Sub

    End Class

End Namespace