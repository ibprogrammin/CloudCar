Imports CloudCar.CCFramework.ContentManagement.CalendarModule
Imports CloudCar.CCFramework.Core

Namespace CCAdmin.ContentManagement.CalendarModule
    Partial Public Class MembershipStatus
        Inherits Page

        Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
            End If
        End Sub

        Protected Sub MembershipEnabledCheckBoxDataBinding(ByVal Sender1 As Object, ByVal E As EventArgs)
            Dim CurrentCheckBox As CheckBox = CType(Sender1, CheckBox)
            Try
                Dim UserName As String = CurrentCheckBox.Attributes("UserName").ToString

                CurrentCheckBox.Checked = RegisteredUserController.GetUserMembershipStatus(UserName)
            Catch Ex As NullReferenceException
                CurrentCheckBox.Checked = False
            End Try
        End Sub

        Protected Sub MembershipEnabledCheckBoxCheckChanged(ByVal Sender1 As Object, ByVal E As EventArgs)
            Dim CurrentCheckBox As CheckBox = CType(Sender1, CheckBox)

            Dim UserName As String = CurrentCheckBox.Attributes("UserName")

            RegisteredUserController.SetUserMembershipStatus(UserName, CurrentCheckBox.Checked)

            If CurrentCheckBox.Checked Then
                Dim CurrentUserEmail As String = Membership.GetUser(UserName).Email
                Dim CurrentSubjust As String = String.Format(CalendarResources.MembershipEnabledSubject)
                Dim CurrentMessage As String = String.Format(CalendarResources.MembershipEnabledMessage)

                SendEmails.SendNoticeMessage(CurrentUserEmail, CurrentSubjust, CurrentMessage)
            End If

            lblStatus.Text = String.Format(CalendarResources.MembershipStatusChangedMessage, UserName)
            lblStatus.Visible = True

            UsersDataGrid.DataBind()
        End Sub

        Private Sub UsersDataGridPageIndexChanged(ByVal Source As Object, ByVal E As DataGridPageChangedEventArgs) Handles UsersDataGrid.PageIndexChanged
            UsersDataGrid.CurrentPageIndex = E.NewPageIndex
            UsersDataGrid.DataBind()
        End Sub

    End Class
End Namespace