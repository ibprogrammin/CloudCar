Namespace CCAdmin
    Partial Public Class Settings
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                RefreshDataSources()
            End If
        End Sub

        Public Sub RefreshDataSources()
            If Roles.IsUserInRole(Membership.GetUser.UserName, "Super User") Then
                RepeaterTabs.DataSource = CCFramework.Core.SettingController.GetSettingCategories()
                RepeaterTabs.DataBind()

                RepeaterTabContent.DataSource = CCFramework.Core.SettingController.GetSettingCategories()
                RepeaterTabContent.DataBind()

                'rptSettings.DataSource = CCFramework.Core.SettingController.GetSettingCategories()
                'rptSettings.DataBind()
            Else
                RepeaterTabs.DataSource = CCFramework.Core.SettingController.GetReadableSettingCategories()
                RepeaterTabs.DataBind()

                RepeaterTabContent.DataSource = CCFramework.Core.SettingController.GetReadableSettingCategories()
                RepeaterTabContent.DataBind()

                'rptSettings.DataSource = CCFramework.Core.SettingController.GetReadableSettingCategories()
                'rptSettings.DataBind()
            End If
        End Sub

        Private Sub SaveButtonClick(ByVal Sender As Object, ByVal Args As EventArgs) Handles SaveButton.Click
            For Each item As RepeaterItem In RepeaterTabContent.Items
                Dim hfCategory As HiddenField = CType(item.FindControl("hfCategory"), HiddenField)

                Dim category As String = hfCategory.Value

                For Each c As Control In item.Controls
                    If c.GetType Is GetType(Repeater) Then
                        Dim SettingList As Repeater = CType(c, Repeater)

                        For Each ri As RepeaterItem In SettingList.Items
                            Dim hfID As HiddenField = CType(ri.FindControl("hfID"), HiddenField)
                            Dim hfKey As HiddenField = CType(ri.FindControl("hfKey"), HiddenField)
                            Dim hfOldValue As HiddenField = CType(ri.FindControl("hfOldValue"), HiddenField)
                            Dim txtValue As TextBox = CType(ri.FindControl("txtValue"), TextBox)

                            Dim id As Integer = Integer.Parse(hfID.Value)
                            Dim key As String = hfKey.Value
                            Dim value As String = txtValue.Text
                            Dim oldValue As String = hfOldValue.Value

                            If Not value = oldValue Then
                                CCFramework.Core.SettingController.Update(id, key, value, category)
                            End If

                            'Response.Write(id & "/" & key & "/" & value & "/" & category & "<br />")
                        Next

                    End If
                Next
            Next

            CCFramework.Core.Settings.LoadSettings()

            RefreshDataSources()
        End Sub

    End Class
End Namespace