Imports System
Imports System.Data.Linq
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model

Partial Public Class DistributorOrders
    Inherits Page

    Protected Sub PageLoad(ByVal Sender As Object, ByVal E As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            RefreshDataSources()

            sortAsc = True
            lastSort = ""
            'ViewState.Add("sortAscending", True)
            'ViewState.Add("lastSort", "")
        End If

        lblStatus.Visible = False
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim db As New CommerceDataContext

        Dim OrderID As Integer
        Dim ApprovalState As Integer
        Dim ShippedState As Integer

        Dim UserSearch As String = txtSUserName.Text

        Dim SearchOrderID As Boolean = Integer.TryParse(txtSOrderNumber.Text, OrderID)
        Dim SearchApproval As Boolean = False 'Integer.TryParse(ddlApproved.SelectedValue, ApprovalState)
        Dim SearchShipped As Boolean = Integer.TryParse(ddlShipped.SelectedValue, ShippedState)

        Dim distributorID As Integer = GetDistributorID()

        Dim oList As List(Of EStore_SimpleOrderView) = OrderController.DistributorsOrders(distributorID).ToList

        If SearchOrderID And SearchApproval And SearchShipped Then
            oList = oList.Where(Function(p) p.ID.Value = OrderID AndAlso p.User.ToLower.Contains(UserSearch.ToLower) AndAlso p.ApprovalState.Value = ApprovalState AndAlso p.Shipped = ShippedState).ToList
        ElseIf SearchOrderID And SearchApproval And Not SearchShipped Then
            oList = oList.Where(Function(p) p.ID.Value = OrderID And p.User.ToLower.Contains(UserSearch.ToLower) And p.ApprovalState.Value = ApprovalState).ToList
        ElseIf SearchOrderID And SearchShipped And Not SearchApproval Then
            oList = oList.Where(Function(p) p.ID.Value = OrderID And p.User.ToLower.Contains(UserSearch.ToLower) And p.Shipped = ShippedState).ToList
        ElseIf SearchApproval And SearchShipped And Not SearchOrderID Then
            oList = oList.Where(Function(p) p.User.ToLower.Contains(UserSearch.ToLower) And p.ApprovalState.Value = ApprovalState And p.Shipped = ShippedState).ToList
        ElseIf SearchApproval And Not SearchOrderID And Not SearchShipped Then
            oList = oList.Where(Function(p) p.User.ToLower.Contains(UserSearch.ToLower) And p.ApprovalState.Value = ApprovalState).ToList
        ElseIf SearchOrderID And Not SearchApproval And Not SearchShipped Then
            oList = oList.Where(Function(p) p.ID.Value = OrderID And p.User.Contains(UserSearch.ToLower)).ToList
        ElseIf SearchShipped And Not SearchOrderID And Not SearchApproval Then
            oList = oList.Where(Function(p) p.User.ToLower.Contains(UserSearch.ToLower) And p.Shipped = ShippedState).ToList
        Else
            oList = oList.Where(Function(p) p.User.ToLower.Contains(UserSearch.ToLower)).ToList
        End If

        'If Not String.IsNullOrEmpty(txtSOrderNumber.Text) Then oList = oList.Where(Function(p) p.ID Like Integer.Parse(txtSOrderNumber.Text))
        'If Not String.IsNullOrEmpty(txtSUserName.Text) Then oList = oList.Where(Function(p) p.User Like txtSUserName.Text)
        'If Not ddlApproved.SelectedValue Is Nothing Then oList = oList.Where(Function(p) p.ApprovalState = Integer.Parse(ddlApproved.SelectedValue))
        'If Not ddlShipped.SelectedValue Is Nothing Then oList = oList.Where(Function(p) p.Shipped = Integer.Parse(ddlShipped.SelectedValue))

        gvOrders.AllowPaging = False

        gvOrders.DataSource = oList
        gvOrders.DataBind()

        If gvOrders.Items.Count < 1 Then
            lblStatus.Text = "No orders were found matching your search criteria."
            lblStatus.Visible = True
        End If

        ClearSearch()
    End Sub

    Private Sub ClearSearch()
        txtSOrderNumber.Text = ""
        txtSUserName.Text = ""
        'ddlApproved.SelectedIndex = Nothing
        ddlShipped.SelectedIndex = Nothing
    End Sub

    Private Sub RefreshDataSources(Optional ByRef Sort As String = "ID", Optional ByVal SortDirection As WebControls.SortDirection = WebControls.SortDirection.Ascending)
        Try
            Dim db As New CommerceDataContext

            Dim sortAscending As Boolean

            If SortDirection = WebControls.SortDirection.Ascending Then
                sortAscending = True
            Else
                sortAscending = False
            End If

            Dim distributorID As Integer = GetDistributorID()

            Dim oList As List(Of EStore_SimpleOrderView) = OrderController.DistributorsOrders(distributorID).ToList

            Select Case Sort
                Case "User"
                    If sortAscending Then
                        sortAscending = True
                        oList = oList.OrderBy(Function(p) p.User).ToList
                    Else
                        oList = oList.OrderByDescending(Function(p) p.User).ToList
                    End If
                Case "OrderDate"
                    If sortAscending Then
                        oList = oList.OrderBy(Function(p) p.OrderDate).ToList
                    Else
                        oList = oList.OrderByDescending(Function(p) p.OrderDate).ToList
                    End If
                Case "ApprovalState"
                    If sortAscending Then
                        oList = oList.OrderBy(Function(p) p.ApprovalState).ToList
                    Else
                        oList = oList.OrderByDescending(Function(p) p.ApprovalState).ToList
                    End If
                Case "Shipped"
                    If sortAscending Then
                        oList = oList.OrderBy(Function(p) p.Shipped).ToList
                    Else
                        oList = oList.OrderByDescending(Function(p) p.Shipped).ToList
                    End If
                Case "Items"
                    If sortAscending Then
                        oList = oList.OrderBy(Function(p) p.Items).ToList
                    Else
                        oList = oList.OrderByDescending(Function(p) p.Items).ToList
                    End If
                Case "Total"
                    If sortAscending Then
                        oList = oList.OrderBy(Function(p) p.Total).ToList
                    Else
                        oList = oList.OrderByDescending(Function(p) p.Total).ToList
                    End If
                Case "ID"
                    If sortAscending Then
                        oList = oList.OrderBy(Function(p) p.ID).ToList
                    Else
                        oList = oList.OrderByDescending(Function(p) p.ID).ToList
                    End If
                Case Else
                    'oList = OrderController.GetSimpleOrderViewsFunc(db)
            End Select

            gvOrders.DataSource = oList
            gvOrders.DataBind()

            'ogOrders.DataSource = GetSimpleOrderViewsFunc(db) ' oList
            'ogOrders.DataBind()
        Catch ex As Exception
            lblStatus.Text = ex.Message
            lblStatus.Visible = True
        End Try
    End Sub

    Protected Function getApprovalState(ByVal state As EApprovalState) As String
        Select Case state
            Case EApprovalState.Pending
                Return "Pending"
            Case EApprovalState.Approved
                Return "Approved"
            Case EApprovalState.Declined
                Return "Declined"
            Case Else
                Return "Unknown"
        End Select
    End Function

    Protected Function getShippedState(ByVal state As Integer) As String
        Select Case state
            Case 0
                Return "None"
            Case 1
                Return "Partial"
            Case 2
                Return "Complete"
            Case Else
                Return "Error"
        End Select
    End Function

    Private Property sortAsc() As Boolean
        Get
            If ViewState("sortAsc") Is Nothing Then
                ViewState.Add("sortAsc", True)
            End If

            Return CBool(ViewState("sortAsc"))
        End Get
        Set(ByVal value As Boolean)
            If Not ViewState("sortAsc") Is Nothing Then
                ViewState("sortAsc") = value
            Else
                ViewState.Add("sortAsc", value)
            End If
        End Set
    End Property

    Private Property lastSort() As String
        Get
            If Not ViewState("lastSort") Is Nothing Then
                Return ViewState("sortAsc").ToString
            Else
                ViewState.Add("lastSort", True)
                Return ViewState("sortAsc").ToString
            End If
        End Get
        Set(ByVal value As String)
            If Not ViewState("lastSort") Is Nothing Then
                ViewState("lastSort") = value
            Else
                ViewState.Add("lastSort", value)
            End If
        End Set
    End Property

    Private Sub gvOrders_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles gvOrders.PageIndexChanged
        gvOrders.CurrentPageIndex = e.NewPageIndex

        RefreshDataSources()
    End Sub

    Private Sub gvOrders_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles gvOrders.SortCommand
        If Not e.SortExpression = lastSort Then
            sortAsc = True
        Else
            sortAsc = Not sortAsc
        End If

        If sortAsc Then
            RefreshDataSources(e.SortExpression, SortDirection.Ascending)
        Else
            RefreshDataSources(e.SortExpression, SortDirection.Descending)
        End If

        lastSort = e.SortExpression
    End Sub

    Private Function GetDistributorID() As Integer
        Dim username As String = Membership.GetUser().UserName

        Dim user As RegisteredUser = New CCFramework.Core.RegisteredUserController().GetByUserName(username)

        Return user.UserID
    End Function

End Class