Imports CloudCar.CCFramework.Core

Namespace CCControls.Admin

    Public Class TipMessageControl
        Inherits UserControl

        Protected Sub PageLoad(ByVal Sender As Object, ByVal Args As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                GetMessages()
            End If
        End Sub

        Private Sub GetMessages()
            If Not Membership.GetUser Is Nothing Then
                If Roles.IsUserInRole("Administrator") Then
                    Dim CurrentSystemMessageReporter As New TipMessageReporter

                    MessageRepeater.DataSource = CurrentSystemMessageReporter.GetMessages()
                    MessageRepeater.DataBind()
                End If
            End If
        End Sub

    End Class
End Namespace