Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.ContentManagement

Namespace CCControls.ContentManagement

    Partial Public Class MenuDisplayControl
        Inherits UserControl

        Protected Overrides Sub OnLoad(ByVal E As EventArgs)
            If Not Page.IsPostBack Then
                LoadMenu()
            End If
        End Sub

        Private Sub LoadMenu()
            Dim MenuItems As List(Of MenuItem) = MenuItemController.GetOrderedElements()

            Dim CurrentMenu As String = Me.Attributes("Menu")

            If Not CurrentMenu Is Nothing AndAlso Not CurrentMenu = String.Empty Then
                MenuItems = MenuItems.Where(Function(f) f.Menu = CurrentMenu).ToList
            End If
            
            Select Case Mode
                Case MenuMode.Header
                    HeaderMenuRepeater.DataSource = MenuItems
                    HeaderMenuRepeater.DataBind()
                    HeaderMenuRepeater.Visible = True
                Case MenuMode.Footer
                    rptFooterMenu.DataSource = MenuItems
                    rptFooterMenu.DataBind()
                    rptFooterMenu.Visible = True
                Case Else
                    HeaderMenuRepeater.DataSource = MenuItems
                    HeaderMenuRepeater.DataBind()
                    HeaderMenuRepeater.Visible = True
            End Select
        End Sub

        Public Property Mode() As MenuMode
            Get
                If Not ViewState("MenuMode") Is Nothing Then
                    Return CType(ViewState("MenuMode"), MenuMode)
                Else
                    Return MenuMode.Standard
                End If
            End Get
            Set(ByVal value As MenuMode)
                If Not ViewState("MenuMode") Is Nothing Then
                    ViewState("MenuMode") = value
                Else
                    ViewState.Add("MenuMode", value)
                End If
            End Set
        End Property

        Protected Sub HeaderMenuRepeaterItemDataBound(ByVal Sender As Object, ByVal E As RepeaterItemEventArgs) Handles HeaderMenuRepeater.ItemDataBound
            Dim MenuItemIdHiddenField As HiddenField = CType(E.Item.FindControl("MenuItemIdHiddenField"), HiddenField)

            If Not MenuItemIdHiddenField Is Nothing Then
                Dim MenuItemId As Integer = Integer.Parse(MenuItemIdHiddenField.Value)

                If MenuItemController.DoesMenuItemHaveChildren(MenuItemId) Then
                    Dim SubMenuRepeater As Repeater = CType(E.Item.FindControl("SubMenuRepeater"), Repeater)

                    SubMenuRepeater.DataSource = MenuItemController.GetChildMenuItems(MenuItemId)
                    SubMenuRepeater.DataBind()
                End If
            End If
        End Sub

    End Class

    Public Enum MenuMode
        Standard = 0
        Header = 1
        Footer = 2
    End Enum

End Namespace