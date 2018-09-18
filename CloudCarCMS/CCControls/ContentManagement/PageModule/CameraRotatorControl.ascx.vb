Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement

Namespace CCControls.ContentManagement.PageModule

    Partial Public Class CameraRotatorControl
        Inherits RotatorControl

        Private Function FindControlRecursive(ByRef RootControl As Control, ByVal ControlId As String) As Control
            If RootControl.ID Like ControlId Then
                Return RootControl
            End If

            For Each CurrentControl As Control In RootControl.Controls
                Dim FoundControl As Control = FindControlRecursive(CurrentControl, ControlId)

                If Not FoundControl Is Nothing Then
                    Return FoundControl
                End If
            Next

            Return Nothing
        End Function

        Private Sub LoadRequiredScripts()
            'Dim PageScriptManagementControl As CloudCarScriptManagementControl
            'PageScriptManagementControl = CType(FindControlRecursive(CType(Page, Control), ScriptManagementControlId), CloudCarScriptManagementControl)
            'PageScriptManagementControl.AddScriptLocation(New ScriptLocation(String.Format("/CCTemplates/{0}/Scripts/NivoSlider/jquery.nivo.slider.pack.js", Settings.Theme), 3))
        End Sub

        Protected Overrides Sub LoadRotator()
            LoadRequiredScripts()

            Dim CurrentDataContext As New ContentDataContext
            Dim CurrentRotatorItems As List(Of ImageRotator) = ImageRotatorController.GetElementByPage(CurrentDataContext, PageId).OrderBy(Function(i) i.order).ToList

            RotatorRepeater.DataSource = CurrentRotatorItems
            RotatorRepeater.DataBind()

            If RotatorRepeater.Items.Count > 0 Then
                RotatorRepeater.Visible = True
            End If
        End Sub

    End Class

End Namespace