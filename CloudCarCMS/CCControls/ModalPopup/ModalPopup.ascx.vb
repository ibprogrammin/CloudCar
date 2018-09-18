Namespace CCControls.ModalPopup

    Partial Public Class ModalPopup
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load

        End Sub

        Public Sub Show()
            mpeMessage.Show()
        End Sub

        Public Sub Hide()
            mpeMessage.Hide()
        End Sub

        Public Event ClickedOK()

        Public Property Message() As String
            Get
                Return lblMessage.Text
            End Get
            Set(ByVal value As String)
                lblMessage.Text = value
            End Set
        End Property

        Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
            RaiseEvent ClickedOK()
        End Sub
    End Class

End Namespace